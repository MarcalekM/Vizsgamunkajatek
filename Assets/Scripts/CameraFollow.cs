using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followDelay = 1f;
    [SerializeField] private float viewDistance = 5f;

    private Vector3 offset;
    private float lowY;
    private bool facingRight;


    private void Start()
    {
        facingRight = true;
        offset = transform.position - target.position;
        offset.x += viewDistance / 2;
        lowY = 0f;
    }



    private void FixedUpdate()
    {
        if (facingRight != target.localScale.x > 0)
        {
            facingRight = target.localScale.x > 0;
            offset.x += facingRight ? viewDistance : -viewDistance;
        }

        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            followDelay * Time.deltaTime);

        if (transform.position.y < lowY)
        {
            transform.position = new Vector3(
                transform.position.x, lowY, transform.position.z);
        }
    }

}
