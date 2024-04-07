using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrushDetect : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.9f;
    [SerializeField] ParticleSystem CrushEffect;
    bool isCrushing = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "snow")
        {
            CrushEffect.Play();
            if (!isCrushing)
            {
                FindObjectOfType<PlayerController>().disableControls();
                Debug.Log("crushed it!");
                GetComponent<AudioSource>().Play();
                isCrushing = true;
                Invoke("reloadScene", loadDelay);
            }
        }
        if (other.tag == "Player")
        {
            CrushEffect.Play();
            if (!isCrushing)
            {
                isCrushing = true;
                FindObjectOfType<PlayerController>().disableControls();
                Debug.Log("crushed it!");
                GetComponent<AudioSource>().Play();
                Invoke("reloadScene", loadDelay);
            }
        }

    }
    void reloadScene()
    {
        FindObjectOfType<PlayerController>().PrintScore();
        SceneManager.LoadScene(0);
        isCrushing = false;
    }
}
