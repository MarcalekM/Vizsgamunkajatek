using UnityEngine;

public class Proba : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag.Equals("Magic2")) Debug.Log("Erzekel");
    }
}
