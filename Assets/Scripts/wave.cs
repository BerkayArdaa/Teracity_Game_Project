using UnityEngine;
using TMPro; // Make sure to include this to use TextMesh Pro

public class EnemyManager : MonoBehaviour
{
    public GameObject[] bossPrefabs;
    public GameObject[] enemyPrefabs; // List of enemy prefabs
    public GameObject bossPrefab; // Boss prefab
    public Transform playerTransform;
    public float spawnRadius = 8f;
    public TextMeshProUGUI waveText; // Reference to the TextMeshProUGUI component for displaying the wave number
    private int waveNumber = 1; // Start at wave 1
    private int totalEnemiesKilled = 0; // Tracks the total number of enemies killed
    private int nextSpawnCount = 2; // Initially, 2 enemies will spawn
    private int activeEnemies = 0; // Tracks the number of active enemies
    private int nextBossSpawnKillCount = 20; // Tracks the next kill count at which the boss will spawn

    void Start()
    {
        UpdateWaveText(); // Update the wave text at the start
        SpawnEnemies(nextSpawnCount); // Spawn enemies for the first wave
    }

    void Update()
    {
        // If all active enemies are killed, proceed to the next wave
        if (activeEnemies == 0)
        {
            waveNumber++; // Increment wave number
            UpdateWaveText(); // Update the wave text

            // Spawn regular enemies
            SpawnEnemies(nextSpawnCount);

            nextSpawnCount *= 2; // Double the number of enemies for the next wave
        }
    }

    void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPosition = Random.insideUnitCircle * spawnRadius + (Vector2)playerTransform.position;
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; // Randomly select an enemy prefab
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            activeEnemies++; // Increment active enemies count for each enemy spawned
        }
    }

    void SpawnBoss()
    {
        Vector2 spawnPosition = Random.insideUnitCircle * spawnRadius + (Vector2)playerTransform.position;
        int bossIndex = (totalEnemiesKilled / 10) % bossPrefabs.Length; // 10 öldürmede bir farklý boss seçmek için
        Instantiate(bossPrefabs[bossIndex], spawnPosition, Quaternion.identity);
        activeEnemies++; // Boss spawnlandýðýnda aktif düþman sayýsýný artýr
    }

    public void EnemyKilled()
    {
        totalEnemiesKilled++;
        activeEnemies--; // Aktif düþman sayýsýný azalt

        // Her 10 öldürmede bir boss spawnla
        if (totalEnemiesKilled % 10 == 0)
        {
            SpawnBoss();
        }
    }

    // Updates the wave number text on the UI
    void UpdateWaveText()
    {
        waveText.text = "Wave: " + waveNumber;
    }
}
