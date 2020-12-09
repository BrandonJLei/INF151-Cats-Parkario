using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    [SerializeField] private string newLevel;
    [SerializeField] private GameMaster gm;     

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gm.isLevelComplete = true;
            Debug.Log(gm.isLevelComplete);
            SceneManager.LoadScene(newLevel);
        }
    }
}
