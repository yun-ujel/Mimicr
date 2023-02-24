using UnityEngine;

public class EndingSequence : MonoBehaviour
{
    [SerializeField] GameObject[] frame;
    float frameTimer;


    private void Start()
    {
        for (int i = frame.Length - 1; i > 0; i--)
        {
            frame[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (!frame[frame.Length - 1].activeInHierarchy)
        {
            if (frameTimer > 4f || Input.anyKeyDown)
            {
                NextFrame();
                frameTimer = 0f;
            }
            else
            {
                frameTimer += Time.deltaTime;
            }
        }
        else
        {
            if (frameTimer > 6f)
            {
                Application.Quit();
            }
            else
            {
                frameTimer += Time.deltaTime;
            }
        }
    }

    void NextFrame()
    {
        for (int i = 0; i < frame.Length; i++)
        {
            if (frame[i].activeInHierarchy)
            {
                continue;
            }
            frame[i].SetActive(true);
            break;
        }
    }
}
