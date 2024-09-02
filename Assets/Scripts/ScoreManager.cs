using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText2;
    public Text scoreText;
    public Text filePathText;  // Dosya yolunu gösterecek Text objesi
    public GameObject gameOverPanel;
    public InputField nameInputField;
    public Button submitButton;
    public int score = 0;
    [SerializeField]
    GameObject Player;

    private void Awake()
    {
        // Singleton pattern to ensure there's only one instance of ScoreManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ScoreManager'ý sahneler arasýnda koru
        }
        else
        {
            Destroy(gameObject); // Ýkinci bir ScoreManager oluþmasýný engelle
        }
    }

    private void Start()
    {
        Debug.Log("Start called. Initial score: " + score);
        UpdateScoreText();
        gameOverPanel.SetActive(false);
        filePathText.gameObject.SetActive(false); // Baþlangýçta dosya yolu text'i gizli
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("AddScore called. Current score: " + score);
        UpdateScoreText();
    }

    public void GameOver()
    {
        Debug.Log("GameOver called. Final score: " + score);
        // Oyun sonu panelini göster
        gameOverPanel.SetActive(true);

        // Butonu tekrar aktif hale getir
        submitButton.gameObject.SetActive(true);
    }

    private void UpdateScoreText()
    {
        Debug.Log("UpdateScoreText called. Displaying score: " + score);
        scoreText.text = "Score: " + score.ToString();
        scoreText2.text=score.ToString();
    }
}
