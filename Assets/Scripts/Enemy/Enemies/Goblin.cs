using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    Vector3 playerLastPosition;
    protected override void ApplyMovement()
    {
        if (IsGrounded(transform.position + new Vector3(direction.x, direction.y, 0) * hSpeed) || PlayerSpotted)
        {
            var force = direction * hSpeed;
            Debug.Log(rb.velocity.magnitude);
            if (PlayerSpotted && rb.velocity.magnitude < 0.001)
            {
                rb.velocity = new Vector2(rb.velocity.x, hSpeed);
            }
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, force.x, Time.deltaTime * 100), rb.velocity.y);
            
        }
    }
}
