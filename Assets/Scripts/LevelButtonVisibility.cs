using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonVisibility : MonoBehaviour
{
    [SerializeField] private Button button;
    private PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            button.gameObject.SetActive(true);
            player.canGoToNextLevel = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            try
            {
                button.gameObject.SetActive(false);
            }
            catch (UnityEngine.MissingReferenceException e)
            {
                Debug.Log(e);
            }
            player.canGoToNextLevel = false;
        }
    }
}
