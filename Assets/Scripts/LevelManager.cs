using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        FindObjectOfType<ColorSwap>().lineX = Input.mousePosition.x / Screen.width;
    }
}
