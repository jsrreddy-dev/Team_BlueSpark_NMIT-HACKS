using UnityEngine;
using UnityEngine.UI;

public class PipeManagement : MonoBehaviour
{
    public bool pipeState = false;

    public Texture2D onPipe;
    public Texture2D offPipe;

    public ParticleSystem[] particleSystems;

    private void Update()
    {
        if(pipeState)
        {
            GetComponent<RawImage>().texture = onPipe;
            foreach (ParticleSystem ps in particleSystems)
            {
                if (!ps.isPlaying)
                {
                    ps.Play();
                }
            }
        }
        else
        {
            GetComponent<RawImage>().texture = offPipe;

            foreach (ParticleSystem ps in particleSystems)
            {
                if (ps.isPlaying)
                {
                    ps.Stop();
                }
            }
        }
    }

    public void TogglePipeState()
    {
        if (Time.timeScale == 0f)
            return; // Prevent toggling while paused

        pipeState = !pipeState;

        if (pipeState)
        {
            GetComponent<RawImage>().texture = onPipe;
        }
        else
        {
            GetComponent<RawImage>().texture = offPipe;
        }

    }
}
