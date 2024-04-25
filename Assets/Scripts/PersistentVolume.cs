using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentVolume : MonoBehaviour
{
    public static PersistentVolume instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Add any additional methods to help manage the audio settings
}