using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnabler : MonoBehaviour
{
    [SerializeField] private List<GameObject> Enemies = new();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(EnableEnemies());
    }

    private IEnumerator EnableEnemies()
    {
        yield return new WaitForSeconds(1);
        foreach (var enemy in Enemies)
            enemy.SetActive(true);
    }

}
