using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SubmitButtonHandler : MonoBehaviour
{
    public InputField nameInputField;
    public Text filePathText;
    public Button submitButton;
    public ScoreManager scoreManager;

    private void Start()
    {
        if (scoreManager == null)
        {
            scoreManager = ScoreManager.instance;
        }
    }

    public void SubmitScore()
    {
        string playerName = nameInputField.text;
        if (!string.IsNullOrEmpty(playerName) && scoreManager != null)
        {
            Debug.Log("Submitting score. Current score: " + scoreManager.score);

            SaveScore(playerName, scoreManager.score);
            ShowFilePath(); // Dosya yolunu hemen göster

            // Gerekirse sadece belirli UI elemanlarýný gizle
            nameInputField.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);

            // Skor tablosu sahnesine geçiþ yap
            SceneManager.LoadScene("Score Table");
        }
    }

    private void SaveScore(string playerName, int score)
    {
        Debug.Log("Saving score for player: " + playerName + " with score: " + score);
        string path = Application.persistentDataPath + "/scores.txt";
        string content = playerName + ": " + score.ToString() + "\n";
        File.AppendAllText(path, content);
    }

    private void ShowFilePath()
    {
        string path = Application.persistentDataPath + "/scores.txt";
        filePathText.text = "Score saved to: " + path;
        filePathText.gameObject.SetActive(true); // Dosya yolu text'ini görünür yap
    }
}
