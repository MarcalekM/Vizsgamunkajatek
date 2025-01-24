using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControllerScript : MonoBehaviour
{
    [SerializeField] AudioSource Background;
    [SerializeField] AudioSource Battle;
    [SerializeField] PlayerController Player;

    private bool spotted;
    // Start is called before the first frame update
    void Start()
    {
        spotted = Player.PlayerSpotted;
    }

    // Update is called once per frame
    void Update()
    {
        if (spotted != Player.PlayerSpotted)
        {
            spotted = Player.PlayerSpotted;
            SwapMusic();
        }
    }

    public void SwapMusic()
    {
        StopAllCoroutines();

        StartCoroutine(FadeMusic());
    }

    private IEnumerator FadeMusic()
    {
        float timeToFade = 2.5f;
        float timeElapsed = 0;
        if (Player.PlayerSpotted)
        {
            Battle.Play();
            while (timeElapsed < timeToFade)
            {
                Background.volume = Mathf.Lerp(0.75f, 0, timeElapsed / timeToFade);
                Battle.volume = Mathf.Lerp(0, 0.75f, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            Background.Stop();
        }
        else
        {
            Background.Play();
            while (timeElapsed < timeToFade)
            {
                Background.volume = Mathf.Lerp(0, 0.75f, timeElapsed / timeToFade);
                Battle.volume = Mathf.Lerp(0.75f, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            Battle.Stop();
        }
    }
}
