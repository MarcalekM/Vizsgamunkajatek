using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    public override void GetDamage(float damageTaken, float magicDamageTaken)
    {
        HP -= magicDamageTaken;
        if (HP <= 0) MakeDead();
    }
}
