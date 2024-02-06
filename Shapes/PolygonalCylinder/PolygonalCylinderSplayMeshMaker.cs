using UnityEngine;
using System.IO;


public class PolygonalCylinderSplay
{
    private Polygon polygon;
    private SplayData splayData;
    private float baseVertexRadius;

    public PolygonalCylinderSplay(Polygon polygon, float baseVertexRadius, SplayData splayData)
    {
        this.polygon = polygon;
        this.splayData = splayData;
        this.baseVertexRadius = baseVertexRadius;
    }

    public void AddPolygonalCylinderSplayToMesh(MeshData meshData)
    {
        var baseVertexRadius = this.baseVertexRadius;
        var prevVertexRadius = baseVertexRadius;
        var prevZ = 0f;

        float nextVertexRadius = 0f;
        var totSplayLength = splayData.GetTotalChangeInY();
        for (int splayLevel = 1; splayLevel <= this.splayData.numDivisions; splayLevel++)
        {
            var sd = this.splayData.xyChanges[splayLevel];
            nextVertexRadius = baseVertexRadius + sd.x;

            var nextZ = -sd.y;
            PolygonCylinder.StackPolygons(meshData, polygon, totSplayLength, prevVertexRadius, nextVertexRadius, prevZ, nextZ);


            if (Config.debugModeEnabled) Debug.Log($"prevZ={prevZ}, nextZ={nextZ}, prevVertexRadius={prevVertexRadius}, nextVertexRadius={nextVertexRadius}");
            prevVertexRadius = nextVertexRadius;
            prevZ = nextZ;
        }
        PolygonCylinder.StackPolygons(meshData, polygon, totSplayLength, nextVertexRadius, nextVertexRadius * 100, prevZ, prevZ);

        var triangleStr = meshData.TrianglesToString();
        if (Config.debugModeEnabled) Debug.Log("SPLAY Triangles used:\n" + triangleStr);
        if (Config.debugModeEnabled) SaveToCSV(triangleStr, "./Assets/data/triangles.txt");
        if (Config.debugModeEnabled) Debug.Log($"SPLAY NumVertices={meshData.vertices.Count}, NumTriangleIdxs={meshData.triangleIdxs.Count}, NumTriangles={meshData.Triangles.Length}");
    }

    public void SaveToCSV(string triangleStr, string filePath)
    {
        // Create a StringBuilder to build the CSV content
        System.Text.StringBuilder csvContent = new System.Text.StringBuilder();
        // var s = meshData.TrianglesToString();
        // Add header row
        csvContent.AppendLine(triangleStr);

        // Add data rows
        // for (int i = 0; i < uvChanges.Count; i++)
        // {
        //     var line = string.Format("{0},{1},{2},{3}",
        //         uvChanges[i].x,
        //         uvChanges[i].y,
        //         xyChanges[i].x,
        //         xyChanges[i].y);
        //     Debug.Log($"SPLAYDATA: {uvChanges[i]}");
        //     csvContent.AppendLine(line);
        // }

        // Write the CSV content to the file
        File.WriteAllText(filePath, csvContent.ToString());
    }
}


