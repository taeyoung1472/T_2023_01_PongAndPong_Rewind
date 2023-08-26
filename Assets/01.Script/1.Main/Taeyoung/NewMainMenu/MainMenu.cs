using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    bool isTrigged;

    private void Update()
    {
        if(!isTrigged && Input.GetKeyDown(KeyCode.Space))
        {
            LoadingSceneManager.LoadScene(-1);
        }
    }
}
