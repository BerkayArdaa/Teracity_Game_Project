using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScoreTableManager : MonoBehaviour
{
    public GameObject scoreEntryPrefab; // Bir Text prefabý olacak
    public Transform contentParent;     // Scroll View içindeki Content objesi
    public Text highestScoreText;       // En yüksek skoru gösterecek Text objesi

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
                // Her bir skor satýrýný ekrana bas
                GameObject scoreEntry = Instantiate(scoreEntryPrefab, contentParent);
                scoreEntry.GetComponent<Text>().text = line;

                // Skor satýrýný analiz et
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

        // En yüksek skoru göster
        highestScoreText.text = "Highest Score: " + highestScorer + " - " + highestScore;

        // Ýçerik yerleþimini güncelle
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent.GetComponent<RectTransform>());
    }
}
