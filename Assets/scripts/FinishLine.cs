using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] float loadDelay = 2f;
    [SerializeField] ParticleSystem FinishEffect;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("you did it!");
            FinishEffect.Play();
            GetComponent<AudioSource>().Play();
            Invoke("reloadScene", loadDelay);
        }
    }
    void reloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
