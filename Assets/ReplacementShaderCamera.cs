using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementShaderCamera : MonoBehaviour {

    public Shader shader;
	// Use this for initialization
	void Start () {
        Camera cam = GetComponent<Camera>();
        cam.SetReplacementShader(shader, "");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
