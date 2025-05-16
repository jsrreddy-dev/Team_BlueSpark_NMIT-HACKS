using UnityEngine;

public class PipeManagement : MonoBehaviour
{
    public bool pipeState = false;

    public void TogglePipeState()
    {
        pipeState = !pipeState;
        // Add visual feedback if needed
    }
}
