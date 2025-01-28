using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    private Enemy _enemy;
    private PlayerController _player;
    // Start is called before the first frame update
    void Start()
    {
        _enemy = gameObject.GetComponentInParent<Enemy>();
        _player = FindObjectOfType<PlayerController>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            _enemy.PlayerSpotted = true;
            _player.PlayerSpotted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            _enemy.PlayerSpotted = false;
            _player.PlayerSpotted = false;
        }
    }
}
