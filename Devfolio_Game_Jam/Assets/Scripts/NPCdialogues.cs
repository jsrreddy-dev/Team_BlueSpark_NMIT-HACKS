using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Text;

public class NPCdialogues : MonoBehaviour
{
    [Header("References")]
    public AreaManagement connectedArea;
    public GameObject dialogueUI;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public GameObject nextButtonUI;
    public Button startLevelButton;

    [Header("NPC Info")]
    public string areaInchargeName = "Area Incharge";
    public string areaType = "area";

    private List<string> dialogueLines = new List<string>();
    private int currentLine = 0;
    private bool dialogueActive = false;

    // Gemini API constants
    private const string GEMINI_API_KEY = "AIzaSyARnUJHfqHdPXeM0Q4n1kDY7TUzZnLz6iA";
    private const string GEMINI_API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key=" + GEMINI_API_KEY;

    private void Awake()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
            nextButtonUI.SetActive(false);
        }

        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(NextDialogue);
        }
        if (startLevelButton != null)
            startLevelButton.gameObject.SetActive(false);
    }

    public async void StartDialogue()
    {
        if (dialogueUI == null || speakerNameText == null || dialogueText == null || nextButton == null)
        {
            Debug.LogError($"[{gameObject.name}] NPCdialogues: UI references not set!");
            return;
        }

        int population = connectedArea != null ? connectedArea.areaPopulation : 0;
        dialogueLines = new List<string>();
        currentLine = 0;
        dialogueActive = true;

        dialogueUI.SetActive(true);
        nextButtonUI.SetActive(true);

        nextButton.interactable = false;
        if (startLevelButton != null)
            startLevelButton.gameObject.SetActive(false);

        speakerNameText.text = areaInchargeName;
        dialogueText.text = "...";

        // Fetch dialogue from Gemini API
        var lines = await RequestGeminiDialogueAsync(population, areaType, areaInchargeName);
        if (lines != null && lines.Count > 0)
        {
            dialogueLines = lines;
        }
        else
        {
            Debug.LogWarning("Gemini API returned no lines, using fallback.");
            dialogueLines = GetFallbackDialogues(population, areaType, areaInchargeName);
        }

        currentLine = 0;
        nextButton.interactable = true;
        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (!dialogueActive || dialogueLines == null || dialogueLines.Count == 0)
        {
            dialogueUI?.SetActive(false);
            nextButtonUI?.SetActive(false);
            return;
        }

        currentLine = Mathf.Clamp(currentLine, 0, dialogueLines.Count - 1);

        if (speakerNameText != null)
            speakerNameText.text = (currentLine % 2 == 0) ? "You" : areaInchargeName;

        if (dialogueText != null)
            dialogueText.text = dialogueLines[currentLine];
    }

    public void NextDialogue()
    {
        if (!dialogueActive || dialogueLines == null || dialogueLines.Count == 0)
            return;

        if (currentLine < dialogueLines.Count - 1)
        {
            currentLine++;
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        if (dialogueUI != null)
            dialogueUI.SetActive(false);
        if (nextButtonUI != null)
            nextButtonUI.SetActive(false);

        if (nextButton != null)
            nextButton.interactable = false;

        if (startLevelButton != null)
            startLevelButton.gameObject.SetActive(true);
    }

    // --- Gemini API Integration ---

    private async Task<List<string>> RequestGeminiDialogueAsync(int population, string areaType, string incharge)
    {
        string prompt = $"Generate a 4-line conversation between a player and the {incharge} of a {areaType} with a population of {population}. Alternate lines between the player and the incharge. Make it about water supply issues and possible solutions. Return only the dialogue lines, separated by newline.";

        string jsonBody = FixJsonForGemini(prompt);

        using (UnityWebRequest request = new UnityWebRequest(GEMINI_API_URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var asyncOp = request.SendWebRequest();
            while (!asyncOp.isDone)
                await Task.Yield();

#if UNITY_2020_1_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                Debug.LogWarning($"Gemini API error: {request.error}");
                return null;
            }

            string responseText = request.downloadHandler.text;
            Debug.Log($"Gemini API raw response: {responseText}");

            string content = ExtractGeminiContentFromJson(responseText);

            // Split lines and trim
            var lines = new List<string>();
            foreach (var line in content.Split('\n'))
            {
                var trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    lines.Add(trimmed);
            }
            return lines;
        }
    }

    // --- Helpers and Fallbacks ---

    private List<string> GetFallbackDialogues(int population, string areaType, string incharge)
    {
        string popType = population < 30 ? "small" : (population < 100 ? "medium-sized" : "large");
        return new List<string>
        {
            $"Hello! I was wondering about the water situation in this {areaType}.",
            $"We've been managing, but our {popType} community needs more consistent water supply.",
            $"What can I do to help improve the water distribution?",
            $"Ensure our pipes are maintained and prioritize our area during drought. Every drop counts for us!"
        };
    }

    private string FixJsonForGemini(string prompt)
    {
        return $@"{{
            ""contents"": [
                {{
                    ""parts"": [
                        {{
                            ""text"": ""{EscapeJsonString(prompt)}""
                        }}
                    ]
                }}
            ],
            ""generationConfig"": {{
                ""temperature"": 0.7,
                ""maxOutputTokens"": 256,
                ""topP"": 0.95
            }}
        }}";
    }

    private string EscapeJsonString(string text)
    {
        return text.Replace("\"", "\\\"").Replace("\n", "\\n");
    }

    // --- Robust Gemini JSON Extraction ---

    [System.Serializable]
    private class GeminiResponseWrapper
    {
        public Candidate[] candidates;
    }

    [System.Serializable]
    private class Candidate
    {
        public Content content;
    }

    [System.Serializable]
    private class Content
    {
        public Part[] parts;
    }

    [System.Serializable]
    private class Part
    {
        public string text;
    }

    private string ExtractGeminiContentFromJson(string json)
    {
        try
        {
            var wrapper = JsonUtility.FromJson<GeminiResponseWrapper>(json);
            if (wrapper != null && wrapper.candidates != null && wrapper.candidates.Length > 0)
            {
                var parts = wrapper.candidates[0].content.parts;
                if (parts != null && parts.Length > 0)
                    return parts[0].text;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parsing Gemini JSON: " + e.Message);
        }
        return "I'm here to help with the water needs of our area.";
    }
}
