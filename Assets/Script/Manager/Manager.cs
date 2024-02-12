using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.targetFrameRate = 60;
#else
        Application.targetFrameRate = 60;



#endif
        DontDestroyOnLoad(gameObject);

    }



}
