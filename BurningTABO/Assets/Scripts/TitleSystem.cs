﻿using UnityEngine;
using System.Collections;

public class TitleSystem : MonoBehaviour {

    // Use this for initialization
    void Start () {

        
        
    }
    
    // Update is called once per frame
    void Update () {

        if(Input.touchCount > 0 || Input.GetMouseButton(0))
        {
                GoToMainScene();
        }
    }

    public void GoToMainScene()
    {
        Application.LoadLevel("main");
    }
}
