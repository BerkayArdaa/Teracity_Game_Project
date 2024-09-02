using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseObject; // ESC tuþuna basýldýðýnda görünecek obje
    private bool isPaused = false;

    void Start()
    {
        // Oyunun baþýnda objeyi görünmez yapar
        pauseObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0f; // Oyunu durdurur
        pauseObject.SetActive(true); // Objeyi görünür yapar
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Oyunu devam ettirir
        pauseObject.SetActive(false); // Objeyi gizler
        isPaused = false;
    }
}
