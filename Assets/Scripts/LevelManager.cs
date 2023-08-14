using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [HideInInspector]
    public float lineX;

    public Transform hitBar;

    private void Awake() 
    {
        lineX = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        lineX = Input.mousePosition.x / Screen.width;
    }
}
