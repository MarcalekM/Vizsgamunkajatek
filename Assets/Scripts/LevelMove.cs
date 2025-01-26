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
        if (collision.gameObject.CompareTag("Player"))
            StartCoroutine(NextSceneTransform());
    }
    public IEnumerator NextSceneTransform()
    {
        StartCoroutine(transitionElement.LoadLevelWithTrasition(sceneName));

        yield return new WaitForSeconds(1);
    }
}
