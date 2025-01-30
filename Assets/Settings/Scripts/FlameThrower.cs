using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    private PlayerController player;
    private float timer = 0f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void OnTriggerStay2D(Collider2D collision)
    {
        timer += Time.deltaTime;
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.GetDamage(0, player.MagicDamage * 0.4f * timer * Time.deltaTime);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        timer = 0f;
    }
}
