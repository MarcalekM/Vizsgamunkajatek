using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroToGame : MonoBehaviour
{
    private void Start()
    {
        var player = FindObjectOfType<PlayerController>();
        player?.JsonSavePlayer("Swamp");

        SceneManager.LoadScene("Swamp");

    }
}
