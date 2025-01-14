using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    //public Animator transition;
    private void Awake()
    {
        //StartCoroutine(LoadLevel().GetEnumerator());

    }
    IEnumerable LoadLevel(string sceneName)
    {
        //transition.SetTrigger("Start");

        yield return new WaitForSeconds(0.0f);

        SceneManager.LoadScene(sceneName);
    }
}