using System;
using UnityEngine;

public class MeshMaker : MonoBehaviour
{
    public bool debugModeEnabled;
    public bool autoUpdate;
    public bool makeCylinder;
    public bool addWormhole;

    [Range(3, 40)]
    public int numSides;

    [Min(0.01f)]
    public float length;

    [Min(0.01f)]
    public float polygonVertexRadius;

    [Range(0, 4)]
    public int numIcoSubdivisions;
    [Range(0, 20)]
    public int numSplaySubdivisions;
    public float totChangeInU;


    public void MakeMesh()
    {
        Config.debugModeEnabled = debugModeEnabled;
        if (makeCylinder)
        {
            MakeCylinderMesh();
        }
        else
        {
            MakeIcosahedronMesh();
        }
    }

    public void MakeCylinderMesh()
    {
        if (Config.debugModeEnabled) Debug.Log("GenerateMesh invoked...");


        if (Config.debugModeEnabled) Debug.Log("Getting texture...");
        var texture = GetTexture();

        var meshData = new MeshData();

        if (Config.debugModeEnabled) Debug.Log("Getting mesh...");
        var pc = new PolygonCylinder(numSides, length, polygonVertexRadius);
        pc.AddPolygonCylinderToMesh(meshData);

        if (addWormhole)
        {
            if (Config.debugModeEnabled) Debug.Log("Getting SPLAY data...");
            var splayData = new SplayData(numSplaySubdivisions, totChangeInU);
            if (Config.debugModeEnabled) splayData.SaveToCSV("./Assets/data/splayData.csv");
            if (Config.debugModeEnabled) Debug.Log("Getting SPLAY mesh...");
            var pcs = new PolygonalCylinderSplay(pc.polygon, pc.polygonVertexRadius, splayData);
            pcs.AddPolygonalCylinderSplayToMesh(meshData);
        }

        if (Config.debugModeEnabled) Debug.Log("Drawing mesh...");
        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        drawer.DrawMesh(meshData, texture);

        if (Config.debugModeEnabled) Debug.Log("Mesh generation complete");
    }

    public void MakeIcosahedronMesh()
    {
        if (Config.debugModeEnabled) Debug.Log("GenerateMesh invoked...");

        if (Config.debugModeEnabled) Debug.Log("Getting texture...");
        var texture = GetTexture();

        if (Config.debugModeEnabled) Debug.Log("Getting mesh...");
        var icosahedron = new IcosahedronGenerator();
        icosahedron.Initialize();
        icosahedron.Subdivide(numIcoSubdivisions);

        var meshData = icosahedron.GetMeshData();

        if (Config.debugModeEnabled) Debug.Log("Drawing mesh...");
        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        drawer.DrawMesh(meshData, texture);

        if (Config.debugModeEnabled) Debug.Log("Mesh generation complete");
    }

    private static Texture2D GetTexture()
    {
        Texture2D texture = new Texture2D(1, 1);
        return texture;
    }
}