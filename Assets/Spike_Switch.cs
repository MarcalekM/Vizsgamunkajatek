using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Switch : MonoBehaviour
{
    public float timer = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= 1.0f;
        if (timer <= 0.0f)
        {
            Switch();
        }
    }

    void Switch()
    {
        GameObject.Find("Player").transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject.Destroy(this.gameObject);
    }
}
