using UnityEngine;

public class PipeManagement : MonoBehaviour
{
    public bool pipeState = false;

    public void TogglePipeState()
    {
        if (Time.timeScale == 0f)
            return; // Prevent toggling while paused

        pipeState = !pipeState;
        // Add visual feedback if needed
    }
}
