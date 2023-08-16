using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwap : MonoBehaviour
{
    const string SHADER_NAME = "Unlit/ColorSwap";

    private Material postProcessing;
    public Color colorA;
    public Color colorB;

    private void Start() 
    {
        postProcessing = new Material(Shader.Find(SHADER_NAME));
        postProcessing.SetVector("_Color_A", colorA);
        postProcessing.SetVector("_Color_B", colorB);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) 
    {
        postProcessing.SetFloat("_Swap_Line_X", FindObjectOfType<LevelManager>().LineX);
        Graphics.Blit(src, dest, postProcessing);
    }
}
