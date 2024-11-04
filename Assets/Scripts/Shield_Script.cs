using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Script : MonoBehaviour
{
    [SerializeField] PlayerController Shield;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "EnemyAttack"){
            GetDamage(Shield.MagicDamage);
        }
    }

    void GetDamage(int damage){
        if(damage < Shield.ShieldHP) Shield.ShieldHP -= damage; 
        else Shield.ShieldHP = 0;
        AvaibleShield();
    }

    void AvaibleShield(){
        if(Shield.ShieldHP == 0) Shield.ShieldAlive = false;
    }
}
