using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] float aliveTime = 3f;

    private void Awake() => Destroy(gameObject, aliveTime);
}
