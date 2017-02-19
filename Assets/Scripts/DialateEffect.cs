using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DialateEffect : MonoBehaviour
{

    public int size;
    private Material material;
    RenderTexture posMap;

    void Start()
    {
    }

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("PointCloud/PointDialate"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (size == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetFloat("_samples", size);
        material.SetFloat("_sampleSizeX", 1f / Screen.currentResolution.width);
        material.SetFloat("_sampleSizeY", 1f / Screen.currentResolution.height);
        Graphics.Blit(source, destination, material);
    }
}
