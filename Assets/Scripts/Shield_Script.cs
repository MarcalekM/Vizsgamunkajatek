using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Script : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "EnemyBullet"){
            Destroy(other.gameObject);
            GetDamage(50);
        }
        switch(other.tag)
        {
            case "EnemyBullet":
                Destroy(other.gameObject);
                GetDamage(50);
                break;
        }
    }

    public void GetDamage(float damage){
        if(damage < gameObject.GetComponentInParent<PlayerController>().ShieldHP) gameObject.GetComponentInParent<PlayerController>().ShieldHP -= damage; 
        else gameObject.GetComponentInParent<PlayerController>().ShieldHP = 0;
        AvaibleShield();
    }

    void AvaibleShield(){
        if (gameObject.GetComponentInParent<PlayerController>().ShieldHP == 0) gameObject.GetComponentInParent<PlayerController>().ShieldAlive = false;
    }

    
}
