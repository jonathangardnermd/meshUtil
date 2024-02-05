using UnityEngine;
using System.Linq;

public class Polygon
{
    public int numSides;
    private Vector2[] vertices;
    public float[] angularUvs;

    private float angle;

    public float vertexRadius;

    public Polygon(int numSides)
    {
        this.numSides = numSides;
        SetUnitVertices();
    }

    public Vector2[] GetVertices(float sideLength)
    {
        return vertices.Select(v => v * sideLength).ToArray();
    }

    public float SideLengthToVertexRadius(float sideLength)
    {
        return sideLength * 0.5f / Mathf.Sin(0.5f * angle); // r = (sideLength/2) / sin(angle/2)
    }
    public float VertexRadiusToSideLength(float vertexRadius)
    {
        return 2 * vertexRadius * Mathf.Sin(0.5f * angle); // s =  2 * r * sin(angle/2)
    }

    void SetUnitVertices()
    {
        angle = 2 * Mathf.PI / numSides;

        vertexRadius = 0.5f / Mathf.Sin(0.5f * angle); // r = (sideLength/2) / sin(angle/2)

        vertices = new Vector2[numSides];
        angularUvs = new float[numSides];

        for (int i = 0; i < numSides; i++)
        {
            float x = Mathf.Cos(i * angle);
            float y = Mathf.Sin(i * angle);
            vertices[i] = new Vector2(x, y);
            angularUvs[i] = (float)i / numSides;
        }
    }
}