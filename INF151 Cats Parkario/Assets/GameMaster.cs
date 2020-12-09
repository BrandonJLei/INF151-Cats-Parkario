using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    public Vector2 lastCheckPointPos;
    public bool isLevelComplete;
    
    void Start()
    {
        isLevelComplete = false;
    }
    void Awake()
    {
        if(isLevelComplete == false)
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Update()
    {
        if(isLevelComplete == true)
        {
            Destroy(gameObject);
        }
    }
}
