using UnityEngine;
using UnityEngine.SceneManagement;

public class butonButonu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Bu metot di�er sahneye ge�i� yapar
    public void SahneDegistir()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Bu metot StartMenu sahnesine ge�i� yapar
    public void GoToStart()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void GoToTable()
    {
        SceneManager.LoadScene("Score Table");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GoToController()
    {
        SceneManager.LoadScene("Controller");
    }
}
