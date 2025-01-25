using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Script : MonoBehaviour
{
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        switch(other.tag)
        {
            case "EnemyBullet":
                Destroy(other.gameObject);
                GetDamage(50);
                break;
        }
    }

    public void GetDamage(float damage){
        if(damage < player.ShieldHP) player.ShieldHP -= damage; 
        else player.ShieldHP = 0;
        AvailableShield();
    }

    private void AvailableShield(){
        if (player.ShieldHP == 0) player.ShieldAlive = false;
    }

    
}
