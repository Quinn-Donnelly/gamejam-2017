using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DialateEffect : MonoBehaviour
{

    public float intensity;
    private Material material;

    void Start()
    {
    }

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/DamageShader"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        material.SetFloat("_intensity", intensity);
        Graphics.Blit(source, destination, material);
    }
}
