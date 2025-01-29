using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] float aliveTime = 3f;

    private void Awake() => Destroy(gameObject, aliveTime);
}
