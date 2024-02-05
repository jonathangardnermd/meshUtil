using UnityEngine;

public class PolygonalCylinderSplay
{
    private Polygon polygon;
    private SplayData splayData;
    private float baseSideLength;

    public PolygonalCylinderSplay(Polygon polygon, float baseSideLength, SplayData splayData)
    {
        this.polygon = polygon;
        this.splayData = splayData;
        this.baseSideLength = baseSideLength;
    }

    public void AddPolygonalCylinderSplayToMesh(MeshData meshData)
    {
        var baseVertexRadius = this.polygon.SideLengthToVertexRadius(this.baseSideLength);
        var prevVertexRadius = baseVertexRadius;
        var prevZ = 0f;

        float sideLength2 = 0f;
        var totSplayLength = splayData.GetTotalChangeInY();
        for (int splayLevel = 1; splayLevel <= this.splayData.numDivisions; splayLevel++)
        {
            var sd = this.splayData.xyChanges[splayLevel];
            var nextVertexRadius = baseVertexRadius + sd.x;

            var sideLength1 = this.polygon.VertexRadiusToSideLength(prevVertexRadius);
            sideLength2 = this.polygon.VertexRadiusToSideLength(nextVertexRadius);

            var nextZ = -sd.y;
            PolygonCylinder.StackPolygons(meshData, polygon, totSplayLength, sideLength1, sideLength2, prevZ, nextZ);

            prevVertexRadius = nextVertexRadius;
            prevZ = nextZ;
        }
        PolygonCylinder.StackPolygons(meshData, polygon, totSplayLength, sideLength2, sideLength2 * 100, prevZ, prevZ);

        Debug.Log("SPLAY Triangles used:\n" + meshData.TrianglesToString());
        Debug.Log($"SPLAY NumVertices={meshData.vertices.Count}, NumTriangleIdxs={meshData.triangleIdxs.Count}, NumTriangles={meshData.Triangles.Length}");
    }
}


