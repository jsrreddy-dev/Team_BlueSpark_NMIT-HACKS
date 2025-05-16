using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTManager : MonoBehaviour
{
    public string apiKey = "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";
    public string endpoint = "https://api.openai.com/v1/chat/completions";

    public void RequestDialogue(string area, System.Action<string> onResponse)
    {
        string prompt = $"You are a helpful villager who needs water for your area. Be indirect and friendly. Say how much you need using a clue, not a number. The area is a {area}. What's your request today?";
        StartCoroutine(SendChatGPTRequest(prompt, onResponse));
    }

    private IEnumerator SendChatGPTRequest(string prompt, System.Action<string> onResponse)
    {
        var json = "{\"model\":\"gpt-3.5-turbo\",\"messages\":[{\"role\":\"user\",\"content\":\"" + prompt + "\"}]}";
        var request = new UnityWebRequest(endpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse response (simplified)
            string responseText = request.downloadHandler.text;
            onResponse?.Invoke(responseText);
        }
        else
        {
            onResponse?.Invoke("Error contacting ChatGPT.");
        }
    }
}
