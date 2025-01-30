using System.Collections;
using TMPro;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private PlayerController playerController;

    [SerializeField] private GameObject Goblin;
    [SerializeField] private GameObject Ghost;
    [SerializeField] private GameObject Golem;

    [SerializeField] private TMP_Text HighscoreText;
    [SerializeField] private TMP_Text CurrentScoreText;

    private void Start()
    {
        playerController = Player.GetComponent<PlayerController>();
        playerController.inArena = true;
        StartCoroutine(SpawnEnemies());
        if (PlayerPrefs.GetString("LoginToken") != string.Empty && Menu_UI_Manager.UserData != null)
        {
            HighscoreText.text = $"Legtöbb pont: {Menu_UI_Manager.UserData.high_score}";
        }
        else
        {
            int highscore = PlayerPrefs.GetInt("Highscore", 0);
            HighscoreText.text = $"Legtöbb pont: {highscore}";
        }
    }

    private void Update()
    {
        CurrentScoreText.text = $"{playerController.kills}";
    }

    private IEnumerator SpawnEnemies()
    {
        while (Player.activeSelf)
        {
            yield return new WaitForSeconds(4f);

            for (int i = 0; i < 2; i++)
            {
                GameObject enemyPrefab = null;
                int rnd = Random.Range(0, 9);

                if (rnd <= 5)
                    enemyPrefab = Goblin;
                else if (rnd > 5 && rnd <= 8)
                    enemyPrefab = Ghost;
                else
                    enemyPrefab = Golem;

                Vector2 playerPosition = Player.GetComponent<BoxCollider2D>().bounds.center;

                float spawnOffsetX = Random.Range(7f, 9f);
                bool spawnOnLeft = Random.value > 0.5f;

                if (spawnOnLeft)
                    spawnOffsetX *= -1;

                Vector2 spawnPosition = new Vector2(playerPosition.x + spawnOffsetX, playerPosition.y);

                GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                enemyInstance.transform.localScale = enemyPrefab.transform.localScale;

                if (spawnOnLeft)
                    enemyInstance.transform.localScale = new Vector3(-enemyInstance.transform.localScale.x,
                                                                     enemyInstance.transform.localScale.y,
                                                                     enemyInstance.transform.localScale.z);
            }
        }
    }
}
