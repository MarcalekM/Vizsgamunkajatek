using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManager;

public class ToEnding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        sceneManager.LoadScene("End");
    }
}
