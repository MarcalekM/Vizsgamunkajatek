using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f; // Mozg�si sebess�g
    private Vector2 direction = Vector2.left; // Kezdeti ir�ny
    private float timer = 0f; // Id�z�t�
    private Rigidbody2D rb;
    private bool PlayerSpotted = false;

    private void Start()
    {
        // Megkeress�k a Rigidbody2D komponenst
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(PlayerSpotted) speed = 10f;
        else speed = 5f;
        // N�velj�k az id�z�t�t a deltaTime-mal (az el�z� frame �ta eltelt id�)
        timer += Time.deltaTime;

        // Ha eltelt 5 m�sodperc, megford�tjuk az ir�nyt �s lenull�zzuk az id�z�t�t
        if(!PlayerSpotted){
            if (timer >= 3f){
                direction = -direction;
                Flip();
                timer = 0f;
            }
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
    }
}
