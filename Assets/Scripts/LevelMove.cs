using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMove : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private LevelTransition transitionElement;

    public void OnLoadStory()
    {
        if (Menu_UI_Manager.UserData is not null && Menu_UI_Manager.UserData.json_save != string.Empty)
        {
            JsonSaveData data = JsonUtility.FromJson<JsonSaveData>(Menu_UI_Manager.UserData.json_save);
            sceneName = data.Scene;
        }
        StartCoroutine(NextSceneTransform());
    }
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
