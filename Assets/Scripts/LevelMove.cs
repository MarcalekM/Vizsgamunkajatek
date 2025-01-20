using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMove : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private LevelTransition transitionElement;

    public void TriggerEvent()
    {
        StartCoroutine(NextSceneTransform());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            StartCoroutine(NextSceneTransform());
    }
    private IEnumerator NextSceneTransform()
    {
        StartCoroutine(transitionElement.LoadLevelWithTrasition(sceneName));

        yield return new WaitForSeconds(1);
    }
}
