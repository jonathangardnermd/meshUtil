using UnityEngine;

public class PolygonCylinder
{
    public int numSides;
    public float length;
    public float polygonSideLength;

    public Polygon polygon;

    public PolygonCylinder(int numSides, float length, float polygonSideLength)
    {
        this.numSides = numSides;
        this.length = length;
        this.polygonSideLength = polygonSideLength;
    }

    public void AddPolygonCylinderToMesh(MeshData meshData)
    {
        polygon = new Polygon(numSides);

        StackPolygons(meshData, polygon, length, polygonSideLength, polygonSideLength, 0, length);

        Debug.Log("Triangles used:\n" + meshData.TrianglesToString());
        Debug.Log($"NumVertices={meshData.vertices.Count}, NumTriangleIdxs={meshData.triangleIdxs.Count}, NumTriangles={meshData.Triangles.Length}");
    }

    public static void StackPolygons(MeshData meshData, Polygon polygon, float totLength, float sideLength1, float sideLength2, float z1, float z2)
    {
        Vector2[] poly1Vs = polygon.GetVertices(sideLength1);
        Vector2[] poly2Vs = polygon.GetVertices(sideLength2);

        float[] angularUvs = polygon.angularUvs;

        for (int i1 = 0; i1 < polygon.numSides; i1++)
        {
            int i2 = (i1 + 1) % polygon.numSides;

            float angularUv1 = angularUvs[i1];
            float angularUv2 = angularUvs[i2];

            float zUv1 = z1 / totLength;
            float zUv2 = z2 / totLength;

            int idx1 = meshData.AddVertex(new Vector3(poly1Vs[i1].x, poly1Vs[i1].y, z1), new Vector2(angularUv1, zUv1));
            int idx2 = meshData.AddVertex(new Vector3(poly1Vs[i2].x, poly1Vs[i2].y, z1), new Vector2(angularUv2, zUv1));
            int idx3 = meshData.AddVertex(new Vector3(poly2Vs[i1].x, poly2Vs[i1].y, z2), new Vector2(angularUv1, zUv2));
            meshData.AddTriangleIdxs(idx1, idx2, idx3);

            idx1 = meshData.AddVertex(new Vector3(poly1Vs[i2].x, poly1Vs[i2].y, z1), new Vector2(angularUv2, zUv1));
            idx2 = meshData.AddVertex(new Vector3(poly2Vs[i2].x, poly2Vs[i2].y, z2), new Vector2(angularUv2, zUv2));
            idx3 = meshData.AddVertex(new Vector3(poly2Vs[i1].x, poly2Vs[i1].y, z2), new Vector2(angularUv1, zUv2));
            meshData.AddTriangleIdxs(idx1, idx2, idx3);
        }
    }
}
