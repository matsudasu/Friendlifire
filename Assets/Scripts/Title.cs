using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject quitText;

    float timer;

    void Start()
    {
        quitText.SetActive(Application.platform != RuntimePlatform.WebGLPlayer);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (timer > 1f && Input.anyKeyDown)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
