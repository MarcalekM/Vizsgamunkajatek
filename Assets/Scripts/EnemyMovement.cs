using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed;
    private Vector2 direction = Vector2.left; 
    private float timer = 0f; 
    private Rigidbody2D rb;
    public bool PlayerSpotted = false;
    private GameObject player;
    [SerializeField] PlayerController p;
    public bool isFacingRight = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        p.PlayerSpotted = PlayerSpotted;
        if (PlayerSpotted)
        {
            if (gameObject.name.Contains("Ghost")) speed = 6f;
            else if (gameObject.name.Contains("Goblin")) speed = 10f;
            else speed = 2f;
        }
        else
        {
            if (gameObject.name.Contains("Ghost")) speed = 3f;
            else if (gameObject.name.Contains("Goblin")) speed = 5f;
            else speed = 1f;
        }
       
        timer += Time.deltaTime;

        
        if(!PlayerSpotted){
            if (timer >= 3f){
                direction = -direction;
                Flip();
                timer = 0f;
            }
        }
        else
        {
            Vector2 targetPosition = player.transform.position;
            Vector2 currentPosition = transform.position;
            direction = (targetPosition - currentPosition).normalized;
        }
        
    }

    private void FixedUpdate()
    {
        // Mozgatjuk az objektumot a Rigidbody2D-vel az aktu�lis ir�nyban
        rb.velocity = direction * speed;
    }

    private void Flip(){
        Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
        isFacingRight = !isFacingRight;
    }
}
