using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScoreTableManager : MonoBehaviour
{
    public GameObject scoreEntryPrefab; // Bir Text prefab� olacak
    public Transform contentParent;     // Scroll View i�indeki Content objesi
    public Text highestScoreText;       // En y�ksek skoru g�sterecek Text objesi

    private void Start()
    {
        LoadScores();
    }

    private void LoadScores()
    {
        string path = Application.persistentDataPath + "/scores.txt";

        if (!File.Exists(path))
        {
            Debug.LogWarning("Score file not found at path: " + path);
            return;
        }

        string[] scoreLines = File.ReadAllLines(path);
        string highestScorer = "";
        int highestScore = 0;

        foreach (string line in scoreLines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                // Her bir skor sat�r�n� ekrana bas
                GameObject scoreEntry = Instantiate(scoreEntryPrefab, contentParent);
                scoreEntry.GetComponent<Text>().text = line;

                // Skor sat�r�n� analiz et
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string playerName = parts[0].Trim();
                    int playerScore;
                    if (int.TryParse(parts[1].Trim(), out playerScore))
                    {
                        if (playerScore > highestScore)
                        {
                            highestScore = playerScore;
                            highestScorer = playerName;
                        }
                    }
                }
            }
        }

        // En y�ksek skoru g�ster
        highestScoreText.text = "Highest Score: " + highestScorer + " - " + highestScore;

        // ��erik yerle�imini g�ncelle
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent.GetComponent<RectTransform>());
    }
}
