using UnityEngine;

public class SoundScript : MonoBehaviour
{
    void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
        if (hasFocus)
        {
            Time.timeScale = 1.0f;
        }
        if (!hasFocus)
        {
            Time.timeScale = 0f;
        }
    }
    void OnApplicationPause(bool isPaused)
    {
        Silence(isPaused);
    }
    private void Silence(bool silence)
    {
        AudioListener.pause = silence;
    }
}
