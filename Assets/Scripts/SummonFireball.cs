using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SummonFireball : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 15f;

    private Rigidbody2D rigidbody2d;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        rigidbody2d.AddForce(new Vector2(transform.rotation.z == 0 ? 1 : -1, 0) * projectileSpeed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.gameObject.SetActive(false);
    }

    public void Stop()
    {
        transform.localRotation = Quaternion.Euler(0, 0, -90);
        rigidbody2d.velocity = new(0, 0);
    }

    

}
