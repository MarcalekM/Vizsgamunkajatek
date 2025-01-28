using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroToGame : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(LoadLevel("Swamp").GetEnumerator());

    }
    IEnumerable LoadLevel(string sceneName)
    {
        //transition.SetTrigger("Start");

        yield return new WaitForSeconds(60.2f);

        SceneManager.LoadScene(sceneName);
    }
}
