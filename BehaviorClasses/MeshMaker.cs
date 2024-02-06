using System;
using UnityEngine;

public class MeshMaker : MonoBehaviour
{
    public bool autoUpdate;
    public bool makeCylinder;
    public bool addWormhole;

    [Range(3, 40)]
    public int numSides;

    [Min(0.01f)]
    public float length;

    [Min(0.01f)]
    public float polygonSideLength;

    [Range(0, 4)]
    public int numSubdivisions;


    public void MakeMesh()
    {
        if (makeCylinder)
        {
            MakeCylinderMesh();
        }
        else
        {
            MakeIcosahedronMesh();
        }
        // MakeIcosahedronMesh();
    }

    public void MakeCylinderMesh()
    {
        Debug.Log("GenerateMesh invoked...");

        Debug.Log("Getting texture...");
        var texture = GetTexture();

        var meshData = new MeshData();

        Debug.Log("Getting mesh...");
        var pc = new PolygonCylinder(numSides, length, polygonSideLength);
        pc.AddPolygonCylinderToMesh(meshData);

        if (addWormhole)
        {
            Debug.Log("Getting SPLAY data...");
            var splayData = new SplayData(50, 50);
            Debug.Log("Getting SPLAY mesh...");
            var pcs = new PolygonalCylinderSplay(pc.polygon, pc.polygonSideLength, splayData);
            pcs.AddPolygonalCylinderSplayToMesh(meshData);
        }

        Debug.Log("Drawing mesh...");
        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        drawer.DrawMesh(meshData, texture);

        Debug.Log("Mesh generation complete");
    }

    public void MakeIcosahedronMesh()
    {
        Debug.Log("GenerateMesh invoked...");

        Debug.Log("Getting texture...");
        var texture = GetTexture();

        Debug.Log("Getting mesh...");
        var icosahedron = new IcosahedronGenerator();
        icosahedron.Initialize();
        icosahedron.Subdivide(numSubdivisions);

        var meshData = icosahedron.GetMeshData();

        Debug.Log("Drawing mesh...");
        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        drawer.DrawMesh(meshData, texture);

        Debug.Log("Mesh generation complete");
    }

    private static Texture2D GetTexture()
    {
        Texture2D texture = new Texture2D(1, 1);
        return texture;
    }
}