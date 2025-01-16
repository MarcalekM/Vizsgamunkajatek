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
        Debug.Log("Next Level Triggered");
        StartCoroutine(NextSceneTransform());
    }
    private IEnumerator NextSceneTransform()
    {
        StartCoroutine(transitionElement.LoadLevelWithTrasition(sceneName));

        yield return new WaitForSeconds(1);
    }
}
