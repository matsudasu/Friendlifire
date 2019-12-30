using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Player player;
    public Transform next;
    public Collider field;

    public Enemy enemyPrefab;

    AudioSource waveAudio;

    public int score;
    public int wave;
    float timer;

    public Text scoreText;
    public Text waveText;
    public Slider bulletSlider;
    public Text bulletText;
    public GameObject gameOver;

    void Start()
    {
        waveAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (player.isDead)
        {
            timer += Time.deltaTime;

            if (timer > 3f && Input.anyKeyDown)
            {
                SceneManager.LoadScene("Title");
            }
            gameOver.SetActive(true);

            return;
        }

        bulletSlider.value = player.remainBullets;
        bulletText.text = "Bullet " + player.remainBullets.ToString();

        if (player.remainBullets == 0)
        {
            UpdateWave();
        }
    }

    public void UpdateScore()
    {
        score += wave * 10;
        scoreText.text = score.ToString("0,0");        
    }

    public void UpdateWave()
    {
        wave++;
        waveText.text = "Wave " + wave.ToString();

        player.Init(next.position);
        next.position = GetRandomPosition(field);

        for (int i = 0; i < wave; i++)
        {
            var e = Instantiate<Enemy>(enemyPrefab, GetRandomPosition(field), Quaternion.identity);
            e.player = player;
            e.main = this;
        }

        waveAudio.Play();
    }

    Vector3 GetRandomPosition(Collider c)
    {
        var b = c.bounds;

        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            0,
            Random.Range(b.min.z, b.max.z)
        );
    }
}
