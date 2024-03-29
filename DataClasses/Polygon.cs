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

    public Vector2[] GetVertices(float vertexRadius)
    {
        return vertices.Select(v => v * vertexRadius).ToArray();
    }

    void SetUnitVertices()
    {
        angle = 2 * Mathf.PI / numSides;

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