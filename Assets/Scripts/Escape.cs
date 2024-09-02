using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseObject; // ESC tu�una bas�ld���nda g�r�necek obje
    private bool isPaused = false;

    void Start()
    {
        // Oyunun ba��nda objeyi g�r�nmez yapar
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
        pauseObject.SetActive(true); // Objeyi g�r�n�r yapar
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Oyunu devam ettirir
        pauseObject.SetActive(false); // Objeyi gizler
        isPaused = false;
    }
}
