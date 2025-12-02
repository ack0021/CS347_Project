using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    private static MenuMusic instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);   
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Music continues through exposition scene
    }
}
