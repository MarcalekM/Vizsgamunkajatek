using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    EnemyMovement Enemy;
    // Start is called before the first frame update
    void Start()
    {
        Enemy = gameObject.GetComponentInParent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Player")) Enemy.PlayerSpotted = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player")) Enemy.PlayerSpotted = false;
    }
}
