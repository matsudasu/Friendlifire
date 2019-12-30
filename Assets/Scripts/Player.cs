using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 8f;

    public Bullet bulletPrefab;
    public int remainBullets;
    public int maxBullets = 16;
    public float shotInterval = 0.1f;
    float shotTimer;

    public Bomb bombPrefab;
    public GameObject firePrefab;
    public Collider field;

    public bool isDead;

    Collider col;
    Animator anim;

    float timer;
    bool useGamepad;

    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
    }

    public void Init(Vector3 pos)
    {
        transform.position = pos;
        remainBullets = maxBullets;
        col.enabled = false;
        anim.SetBool("IsInvincible", true);

        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        shotTimer += Time.deltaTime;

        if (timer < 1f || isDead)
        {
            return;
        }

        if (!col.enabled && timer > 2f)
        {
            col.enabled = true;
            anim.SetBool("IsInvincible", false);
        }

        Move();

        var fireDir = new Vector3(
            Input.GetAxis("Fire X"),
            0,
            Input.GetAxis("Fire Y")
        );

        if (fireDir.sqrMagnitude > float.Epsilon)
        {
            useGamepad = true;
        }
        if (Input.GetButton("Fire"))
        {
            useGamepad = false;
        }

        if (useGamepad)
        {
            transform.LookAt(transform.position + fireDir);
        }
        else
        {
            var mp = Input.mousePosition;
            mp.z = Camera.main.transform.position.y;
            transform.LookAt(Camera.main.ScreenToWorldPoint(mp));
        }

        if (Input.GetButton("Fire") || fireDir.sqrMagnitude > float.Epsilon)
        {
            if (remainBullets > 0 && shotTimer > shotInterval)
            {
                Instantiate(bulletPrefab, transform.position, transform.rotation);
                remainBullets--;
                shotTimer = 0;

                if (remainBullets == 0)
                {
                    Instantiate(bombPrefab, transform.position, transform.rotation);
                }
            }
        }
    }

    private void Move()
    {
        var dir = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        var pos = transform.position + (dir.normalized * moveSpeed * Time.deltaTime);

        transform.position = new Vector3(
            Mathf.Clamp(pos.x, field.bounds.min.x, field.bounds.max.x),
            0,
            Mathf.Clamp(pos.z, field.bounds.min.z, field.bounds.max.z)
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isDead = true;
            anim.SetBool("IsDead", true);

            var f = Instantiate<GameObject>(firePrefab, transform.position, transform.rotation);
            f.transform.localScale = new Vector3(8.15f, 0.5f, 8.15f);
            Destroy(f.gameObject, 1f);
        }
    }
}
