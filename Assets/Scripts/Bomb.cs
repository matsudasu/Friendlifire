using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int life = 16;

    public Transform attackRangeLine;
    float attackRangeRatio = 1f;

    public GameObject firePrefab;
    public GameObject hitPrefab;

    Collider col;

    float timer;

    void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!col.enabled && timer > 1f)
        {
            col.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            attackRangeRatio *= 1.3f;
            attackRangeLine.localScale = Vector3.one * attackRangeRatio;
            life--;

            Instantiate(hitPrefab, transform.position, hitPrefab.transform.rotation);
        }

        if (other.CompareTag("Fire"))
        {
            attackRangeRatio = other.transform.localScale.x;
            life = 0;
        }

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            life = 0;
        }

        if (life <= 0)
        {
            var f = Instantiate<GameObject>(firePrefab, transform.position, transform.rotation);
            f.transform.localScale = new Vector3(attackRangeRatio, 0.5f, attackRangeRatio);
            Destroy(f.gameObject, 1f);

            Destroy(gameObject);
        }
    }
}
