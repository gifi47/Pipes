using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMesh : MonoBehaviour
{
    public int cells;
    public float cellSize;

    public float radius = 1;

    MeshFilter meshFilter;
    Mesh mesh;
    Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        vertices = new Vector3[(cells + 1) * (cells + 1)];
        int[] triangles = new int[cells * cells * 6];
        Vector2[] uv = new Vector2[vertices.Length];
        for (int cellX = 0; cellX < cells + 1; cellX++)
        {
            for (int cellY = 0; cellY < cells + 1; cellY++)
            {
                vertices[cellX * (cells + 1) + cellY] = new Vector3(cellX * cellSize, cellY * cellSize, 0);
                uv[cellX * (cells + 1) + cellY] = new Vector2(0, 0);
            }
        }
        int index = 0;
        for (int cellX = 0; cellX < cells; cellX++)
        {
            for (int cellY = 0; cellY < cells; cellY++)
            {
                triangles[index++] = cellX * (cells + 1) + cellY;
                triangles[index++] = cellX * (cells + 1) + cellY + 1;
                triangles[index++] = (cellX + 1) * (cells + 1) + cellY;

                triangles[index++] = (cellX + 1) * (cells + 1) + cellY;
                triangles[index++] = cellX * (cells + 1) + cellY + 1;
                triangles[index++] = (cellX + 1) * (cells + 1) + cellY + 1;


                //Debug.Log($"[{vertices[triangles[index - 6]].x};{vertices[triangles[index - 6]].y}], [{vertices[triangles[index - 5]].x};{vertices[triangles[index - 5]].y}], [{vertices[triangles[index - 4]].x};{vertices[triangles[index - 4]].y}]");
                //Debug.Log($"[{vertices[triangles[index - 3]].x};{vertices[triangles[index - 3]].y}], [{vertices[triangles[index - 2]].x};{vertices[triangles[index - 2]].y}], [{vertices[triangles[index - 1]].x};{vertices[triangles[index - 1]].y}]");
            }
        }
        Debug.Log(index);
        mesh.SetVertices(vertices);
        mesh.triangles = triangles;
        mesh.uv = uv; 
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        var meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;

    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                Debug.Log("cast");
                Vector3 point = transform.InverseTransformPoint(raycastHit.point);

                int x = (int)(point.x / cellSize);
                int y = (int)(point.y / cellSize);
                int cellXStart = Mathf.Max(x - 5, 0);
                int cellXEnd = Mathf.Min(x + 5, cells + 1);
                int cellYStart = Mathf.Max(y - 5, 0);
                int cellYEnd = Mathf.Min(y + 5, cells + 1);
                for (int cellX = cellXStart; cellX < cellXEnd; cellX++)
                {
                    for (int cellY = cellYStart; cellY < cellYEnd; cellY++)
                    {
                        int vertIndex = cellX * (cells + 1) + cellY;
                        if (Vector3.SqrMagnitude(mesh.vertices[vertIndex] - point) < radius)
                        {
                            vertices[vertIndex].z = 2;
                        }
                    }
                }
                /*for (int i = 0; i < mesh.vertexCount; i++)
                {
                    if (Vector3.SqrMagnitude(mesh.vertices[i] - point) < radius)
                    {
                        vertices[i].z = 2;
                        Debug.Log("Ye");
                    }
                }*/
                mesh.vertices = vertices;
            }
        }
    }
}
