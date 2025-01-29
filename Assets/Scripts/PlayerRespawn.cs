using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] float x;
    [SerializeField] float y;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = new Vector3(x, y, 4);
        }
    }
}
