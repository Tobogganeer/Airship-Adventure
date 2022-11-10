using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    static MeshGenerator instance;
    private void Awake()
    {
        instance = this;
        _mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = _mesh;
    }

    public AnimationCurve heightCurve;
    public float heightMult = 5;
    public int _detailMult = 1;

    Vector3[] _verts;
    Color[] _colours;
    int[] _tris;

    Mesh _mesh;
    static int detailMult => instance._detailMult;
    static Vector3[] verts => instance._verts;
    static Color[] colours => instance._colours;
    static int[] tris => instance._tris;
    static Mesh mesh => instance._mesh;

    public static void Create(int size, float[,] height, Color[,] colour)
    {
        instance._verts = new Vector3[size * size * detailMult * detailMult];
        instance._colours = new Color[size * size * detailMult * detailMult];

        int vert = 0;

        for (int i = 0; i < size * detailMult; i++)
        {
            for (int j = 0; j < size * detailMult; j++)
            {
                float y = instance.heightCurve.Evaluate(height[i / detailMult, j / detailMult]) * instance.heightMult;
                instance._verts[vert] = new Vector3(i / detailMult, y, j / detailMult);
                instance._colours[vert++] = colour[i / detailMult, j / detailMult];
            }
        }

        int size_1 = size * detailMult - 1;
        instance._tris = new int[size_1 * size_1 * 6];

        vert = 0;
        int tri = 0;

        for (int y = 0; y < size_1; y++)
        {
            for (int x = 0; x < size_1; x++)
            {
                tris[tri++] = vert;
                tris[tri++] = vert + 1;
                tris[tri++] = vert + size_1 + 1;
                tris[tri++] = vert + 1;
                tris[tri++] = vert + size_1 + 2;
                tris[tri++] = vert + size_1 + 1;

                vert++;
            }

            vert++;
        }

        UpdateMesh();

        #region +1
        /*
        instance._verts = new Vector3[(size + 1) * (size + 1)];

        int vert = 0;

        for (int i = 0; i <= size; i++)
        {
            for (int j = 0; j <= size; j++)
            {
                instance._verts[vert++] = new Vector3(i, 0, j);
            }
        }

        instance._tris = new int[size * size * 6];

        vert = 0;
        int tri = 0;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                tris[tri++] = vert;
                tris[tri++] = vert + size + 1;
                tris[tri++] = vert + 1;
                tris[tri++] = vert + 1;
                tris[tri++] = vert + size + 1;
                tris[tri++] = vert + size + 2;

                vert++;
            }

            vert++;
        }
        */
        #endregion
    }

    private static void UpdateMesh()
    {
        mesh.Clear();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = verts;
        mesh.colors = colours;
        mesh.triangles = tris;

        mesh.RecalculateNormals();
        //Vector3[] normals = mesh.normals;
        //for (int i = 0; i < normals.Length; i++)
        //    normals[i] = -normals[i];

        //mesh.normals = normals;
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        mesh.Optimize();
    }
}
