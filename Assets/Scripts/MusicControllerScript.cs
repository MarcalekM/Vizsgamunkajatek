using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControllerScript : MonoBehaviour
{
    [SerializeField] AudioSource Source;
    [SerializeField] AudioClip BackgroundClip;
    [SerializeField] AudioClip BattleClip;
    [SerializeField] PlayerController Player;
    // Start is called before the first frame update
    void Start()
    {
        Source.clip = BackgroundClip;
        Source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.PlayerSpotted && Source.clip != BattleClip) {
            Source.clip = BattleClip;
            Source.Play();
        }
        else if(!Player.PlayerSpotted && Source.clip != BackgroundClip) {
            Source.clip = BackgroundClip;
            Source.Play();
        }
    }
}
