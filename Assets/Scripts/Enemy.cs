using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1f;

    public Player player;
    public Main main;

    public GameObject firePrefab;

    void Start()
    {
        transform.localScale = new Vector3(1f, Random.Range(0.5f, 1.5f), 1);
        transform.LookAt(player.transform);
    }

    void Update()
    {
        if (player.isDead)
        {
            return;
        }

        var dir = (player.transform.position - transform.position).normalized;
        transform.LookAt(transform.position + Vector3.Slerp(transform.forward, dir, Time.deltaTime));

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        main.UpdateScore();
        Destroy(Instantiate(firePrefab, transform.position, transform.rotation), 1f);

        Destroy(gameObject);
    }
}
