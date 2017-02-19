using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloudDisplay : MonoBehaviour {

    public RenderTexture texture;
    MeshFilter filter;
    public float pointSize = 0.1f;

	// Use this for initialization
	void Start () {
        filter = GetComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> texcoords = new List<Vector2>();
        int[] indices = new int[texture.width * texture.height * 4 + 2];
        int i = 0;
        float offset = (1.0f / texture.width) / 2f;
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                vertices.Add(new Vector3( pointSize,  pointSize, 0));
                vertices.Add(new Vector3( pointSize, -pointSize, 0));
                vertices.Add(new Vector3(-pointSize, -pointSize, 0));
                vertices.Add(new Vector3(-pointSize,  pointSize, 0));
                texcoords.Add(new Vector2(x / (float)texture.width + offset, y / (float)texture.height + offset));
                texcoords.Add(new Vector2(x / (float)texture.width + offset, y / (float)texture.height + offset));
                texcoords.Add(new Vector2(x / (float)texture.width + offset, y / (float)texture.height + offset));
                texcoords.Add(new Vector2(x / (float)texture.width + offset, y / (float)texture.height + offset));
                indices[i] = i;
                indices[i + 1] = i + 1;
                indices[i + 2] = i + 2;
                indices[i + 3] = i + 3;
                i += 4;
            }
        }

        //vertices.Add(new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
        //vertices.Add(new Vector3(-float.MaxValue, -float.MaxValue, -float.MaxValue));
        //texcoords.Add(new Vector2(0,0));
        //texcoords.Add(new Vector2(0,0));
        //indices[i] = i;
        //indices[i + 1] = i + 1;

        mesh.SetVertices(vertices);
        mesh.SetUVs(0, texcoords);
        mesh.SetIndices(indices, MeshTopology.Quads, 0);
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 99999);
	}
	
	// Update is called once per frame
	void Update () {

    }
}
