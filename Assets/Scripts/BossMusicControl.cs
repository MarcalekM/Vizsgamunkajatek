using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicControl : MonoBehaviour
{
    [SerializeField] public AudioSource bossMusic1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.tag == "Player")
        {
            bossMusic1.Play();
        }
        //this.gameObject.SetActive(false);
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }
}