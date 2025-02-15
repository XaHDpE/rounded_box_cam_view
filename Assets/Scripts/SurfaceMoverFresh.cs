using UnityEngine;

public class SurfaceMoverFresh : MonoBehaviour
{
    public Transform baseObject; // The object whose surface the mover will follow
    public float moveSpeed = 5f; // Constant speed of movement
    public float rotationSmoothness = 5f; // Smoothness factor for rotation (higher = smoother)

    private Mesh baseMesh;
    private Vector3[] vertices;
    private Vector3[] normals;

    private void Start()
    {
        if (baseObject == null)
        {
            Debug.LogError("Base object is not assigned!");
            return;
        }

        // Get the mesh data from the base object
        MeshFilter meshFilter = baseObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("Base object does not have a MeshFilter!");
            return;
        }

        baseMesh = meshFilter.mesh;
        vertices = baseMesh.vertices;
        normals = baseMesh.normals;
    }

    private void Update()
    {
        if (baseMesh == null || vertices == null || normals == null)
        {
            return;
        }

        // Move the object forward at a constant speed
        MoveOnSurface();
    }

    private void MoveOnSurface()
    {
        // Find the closest vertex on the base mesh to the mover's position
        int closestVertexIndex = FindClosestVertex(transform.position);

        // Get the surface normal at the closest vertex
        Vector3 surfaceNormal = baseObject.TransformDirection(normals[closestVertexIndex]);

        // Move the object forward in its local space
        Vector3 moveDirection = transform.forward;
        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

        // Project the new position onto the surface
        newPosition = ProjectOnSurface(newPosition);

        // Update the mover's position
        transform.position = newPosition;

        // Smoothly align the mover with the surface normal
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, surfaceNormal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
    }

    private int FindClosestVertex(Vector3 position)
    {
        int closestIndex = 0;
        float closestDistance = Vector3.Distance(position, baseObject.TransformPoint(vertices[0]));

        for (int i = 1; i < vertices.Length; i++)
        {
            Vector3 worldVertex = baseObject.TransformPoint(vertices[i]);
            float distance = Vector3.Distance(position, worldVertex);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private Vector3 ProjectOnSurface(Vector3 position)
    {
        // Find the closest vertex on the base mesh
        int closestVertexIndex = FindClosestVertex(position);
        Vector3 surfaceNormal = baseObject.TransformDirection(normals[closestVertexIndex]);
        Vector3 surfacePoint = baseObject.TransformPoint(vertices[closestVertexIndex]);

        // Project the position onto the surface
        Vector3 projectedPosition = position - surfaceNormal * Vector3.Dot(position - surfacePoint, surfaceNormal);

        return projectedPosition;
    }
}