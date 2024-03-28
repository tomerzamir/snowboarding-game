using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrushDetect : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.9f;
    [SerializeField] ParticleSystem CrushEffect;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "snow" || other.tag == "rock")
        {
            Debug.Log("crushed it!");
            CrushEffect.Play();
            Invoke("reloadScene", loadDelay);
        }
    }
    void reloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
