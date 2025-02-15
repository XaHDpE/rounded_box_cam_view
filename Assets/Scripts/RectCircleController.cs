using UnityEngine;
using UnityEditor;

public class RectCircleController : MonoBehaviour
{
    public float a = 4f; // Width of the box
    public float b = 3f; // Height of the box
    public float c = 2f; // Length of the box
    public float bevelSize = 0.5f; // Length of the bevel (фаска)

    public Vector3[] corners; // Вершины оригинальной коробки
    public Vector3[] bevelCorners; // Вершины фасок

    private void OnDrawGizmos()
    {
        // Draw the beveled edges and labels
        DrawBeveledBox();
    }

    private void DrawBoxEdge(Vector3 start, Vector3 end)
    {
        // Helper method to draw a single edge of the box
        Gizmos.DrawLine(start, end);
    }

    private void DrawBeveledBox()
    {
        // Define the 8 corners of the original box
        corners = new Vector3[8]
        {
            new(-a / 2, -b / 2, -c / 2), // Bottom-back-left (A)
            new(a / 2, -b / 2, -c / 2),  // Bottom-back-right (B)
            new(a / 2, b / 2, -c / 2),   // Top-back-right (C)
            new(-a / 2, b / 2, -c / 2),  // Top-back-left (D)
            new(-a / 2, -b / 2, c / 2),  // Bottom-front-left (E)
            new(a / 2, -b / 2, c / 2),   // Bottom-front-right (F)
            new(a / 2, b / 2, c / 2),    // Top-front-right (G)
            new(-a / 2, b / 2, c / 2)    // Top-front-left (H)
        };

        // Define the 24 corners of the beveled faces
        bevelCorners = new Vector3[24]
        {
            // Top face bevel
            new(-a / 2 + bevelSize, b / 2 + bevelSize, -c / 2 + bevelSize), // Top-back-left bevel (I)
            new(a / 2 - bevelSize, b / 2 + bevelSize, -c / 2 + bevelSize),  // Top-back-right bevel (J)
            new(a / 2 - bevelSize, b / 2 + bevelSize, c / 2 - bevelSize),   // Top-front-right bevel (K)
            new(-a / 2 + bevelSize, b / 2 + bevelSize, c / 2 - bevelSize),  // Top-front-left bevel (L)

            // Bottom face bevel
            new(-a / 2 + bevelSize, -b / 2 - bevelSize, -c / 2 + bevelSize), // Bottom-back-left bevel (M)
            new(a / 2 - bevelSize, -b / 2 - bevelSize, -c / 2 + bevelSize),  // Bottom-back-right bevel (N)
            new(a / 2 - bevelSize, -b / 2 - bevelSize, c / 2 - bevelSize),   // Bottom-front-right bevel (O)
            new(-a / 2 + bevelSize, -b / 2 - bevelSize, c / 2 - bevelSize),  // Bottom-front-left bevel (P)

            // Left face bevel
            new(-a / 2 - bevelSize, -b / 2 + bevelSize, -c / 2 + bevelSize), // Left-bottom-back bevel (Q)
            new(-a / 2 - bevelSize, b / 2 - bevelSize, -c / 2 + bevelSize),  // Left-top-back bevel (R)
            new(-a / 2 - bevelSize, b / 2 - bevelSize, c / 2 - bevelSize),   // Left-top-front bevel (S)
            new(-a / 2 - bevelSize, -b / 2 + bevelSize, c / 2 - bevelSize),  // Left-bottom-front bevel (T)

            // Right face bevel
            new(a / 2 + bevelSize, -b / 2 + bevelSize, -c / 2 + bevelSize), // Right-bottom-back bevel (U)
            new(a / 2 + bevelSize, b / 2 - bevelSize, -c / 2 + bevelSize),  // Right-top-back bevel (V)
            new(a / 2 + bevelSize, b / 2 - bevelSize, c / 2 - bevelSize),   // Right-top-front bevel (W)
            new(a / 2 + bevelSize, -b / 2 + bevelSize, c / 2 - bevelSize),  // Right-bottom-front bevel (X)

            // Front face bevel
            new(-a / 2 + bevelSize, -b / 2 + bevelSize, c / 2 + bevelSize), // Front-bottom-left bevel (Y)
            new(a / 2 - bevelSize, -b / 2 + bevelSize, c / 2 + bevelSize),  // Front-bottom-right bevel (Z)
            new(a / 2 - bevelSize, b / 2 - bevelSize, c / 2 + bevelSize),   // Front-top-right bevel (AA)
            new(-a / 2 + bevelSize, b / 2 - bevelSize, c / 2 + bevelSize),  // Front-top-left bevel (AB)

            // Back face bevel
            new(-a / 2 + bevelSize, -b / 2 + bevelSize, -c / 2 - bevelSize), // Back-bottom-left bevel (AC)
            new(a / 2 - bevelSize, -b / 2 + bevelSize, -c / 2 - bevelSize),  // Back-bottom-right bevel (AD)
            new(a / 2 - bevelSize, b / 2 - bevelSize, -c / 2 - bevelSize),   // Back-top-right bevel (AE)
            new(-a / 2 + bevelSize, b / 2 - bevelSize, -c / 2 - bevelSize)   // Back-top-left bevel (AF)
        };

        // Set the color for the beveled edges
        Gizmos.color = Color.red;

        // Draw the top face bevel
        DrawBoxEdge(bevelCorners[0], bevelCorners[1]);
        DrawBoxEdge(bevelCorners[1], bevelCorners[2]);
        DrawBoxEdge(bevelCorners[2], bevelCorners[3]);
        DrawBoxEdge(bevelCorners[3], bevelCorners[0]);

        // Draw the bottom face bevel
        DrawBoxEdge(bevelCorners[4], bevelCorners[5]);
        DrawBoxEdge(bevelCorners[5], bevelCorners[6]);
        DrawBoxEdge(bevelCorners[6], bevelCorners[7]);
        DrawBoxEdge(bevelCorners[7], bevelCorners[4]);

        // Draw the left face bevel
        DrawBoxEdge(bevelCorners[8], bevelCorners[9]);
        DrawBoxEdge(bevelCorners[9], bevelCorners[10]);
        DrawBoxEdge(bevelCorners[10], bevelCorners[11]);
        DrawBoxEdge(bevelCorners[11], bevelCorners[8]);

        // Draw the right face bevel
        DrawBoxEdge(bevelCorners[12], bevelCorners[13]);
        DrawBoxEdge(bevelCorners[13], bevelCorners[14]);
        DrawBoxEdge(bevelCorners[14], bevelCorners[15]);
        DrawBoxEdge(bevelCorners[15], bevelCorners[12]);

        // Draw the front face bevel
        DrawBoxEdge(bevelCorners[16], bevelCorners[17]);
        DrawBoxEdge(bevelCorners[17], bevelCorners[18]);
        DrawBoxEdge(bevelCorners[18], bevelCorners[19]);
        DrawBoxEdge(bevelCorners[19], bevelCorners[16]);

        // Draw the back face bevel
        DrawBoxEdge(bevelCorners[20], bevelCorners[21]);
        DrawBoxEdge(bevelCorners[21], bevelCorners[22]);
        DrawBoxEdge(bevelCorners[22], bevelCorners[23]);
        DrawBoxEdge(bevelCorners[23], bevelCorners[20]);

        // Draw the connecting edges between the original faces and the beveled faces
        // Top face connections
        DrawBoxEdge(corners[2], bevelCorners[1]);
        DrawBoxEdge(corners[3], bevelCorners[0]);
        DrawBoxEdge(corners[6], bevelCorners[2]);
        DrawBoxEdge(corners[7], bevelCorners[3]);

        // Bottom face connections
        DrawBoxEdge(corners[0], bevelCorners[4]);
        DrawBoxEdge(corners[1], bevelCorners[5]);
        DrawBoxEdge(corners[4], bevelCorners[7]);
        DrawBoxEdge(corners[5], bevelCorners[6]);

        // Left face connections
        DrawBoxEdge(corners[0], bevelCorners[8]);
        DrawBoxEdge(corners[3], bevelCorners[9]);
        DrawBoxEdge(corners[7], bevelCorners[10]);
        DrawBoxEdge(corners[4], bevelCorners[11]);

        // Right face connections
        DrawBoxEdge(corners[1], bevelCorners[12]);
        DrawBoxEdge(corners[2], bevelCorners[13]);
        DrawBoxEdge(corners[6], bevelCorners[14]);
        DrawBoxEdge(corners[5], bevelCorners[15]);

        // Front face connections
        DrawBoxEdge(corners[4], bevelCorners[16]);
        DrawBoxEdge(corners[5], bevelCorners[17]);
        DrawBoxEdge(corners[6], bevelCorners[18]);
        DrawBoxEdge(corners[7], bevelCorners[19]);

        // Back face connections
        DrawBoxEdge(corners[0], bevelCorners[20]);
        DrawBoxEdge(corners[1], bevelCorners[21]);
        DrawBoxEdge(corners[2], bevelCorners[22]);
        DrawBoxEdge(corners[3], bevelCorners[23]);

        // Draw labels for the original corners
        DrawLabel(corners[0], "A"); // Bottom-back-left
        DrawLabel(corners[1], "B"); // Bottom-back-right
        DrawLabel(corners[2], "C"); // Top-back-right
        DrawLabel(corners[3], "D"); // Top-back-left
        DrawLabel(corners[4], "E"); // Bottom-front-left
        DrawLabel(corners[5], "F"); // Bottom-front-right
        DrawLabel(corners[6], "G"); // Top-front-right
        DrawLabel(corners[7], "H"); // Top-front-left

        // Draw labels for the beveled corners
        DrawLabel(bevelCorners[0], "I"); // Top-back-left bevel
        DrawLabel(bevelCorners[1], "J"); // Top-back-right bevel
        DrawLabel(bevelCorners[2], "K"); // Top-front-right bevel
        DrawLabel(bevelCorners[3], "L"); // Top-front-left bevel
        DrawLabel(bevelCorners[4], "M"); // Bottom-back-left bevel
        DrawLabel(bevelCorners[5], "N"); // Bottom-back-right bevel
        DrawLabel(bevelCorners[6], "O"); // Bottom-front-right bevel
        DrawLabel(bevelCorners[7], "P"); // Bottom-front-left bevel
        DrawLabel(bevelCorners[8], "Q"); // Left-bottom-back bevel
        DrawLabel(bevelCorners[9], "R"); // Left-top-back bevel
        DrawLabel(bevelCorners[10], "S"); // Left-top-front bevel
        DrawLabel(bevelCorners[11], "T"); // Left-bottom-front bevel
        DrawLabel(bevelCorners[12], "U"); // Right-bottom-back bevel
        DrawLabel(bevelCorners[13], "V"); // Right-top-back bevel
        DrawLabel(bevelCorners[14], "W"); // Right-top-front bevel
        DrawLabel(bevelCorners[15], "X"); // Right-bottom-front bevel
        DrawLabel(bevelCorners[16], "Y"); // Front-bottom-left bevel
        DrawLabel(bevelCorners[17], "Z"); // Front-bottom-right bevel
        DrawLabel(bevelCorners[18], "AA"); // Front-top-right bevel
        DrawLabel(bevelCorners[19], "AB"); // Front-top-left bevel
        DrawLabel(bevelCorners[20], "AC"); // Back-bottom-left bevel
        DrawLabel(bevelCorners[21], "AD"); // Back-bottom-right bevel
        DrawLabel(bevelCorners[22], "AE"); // Back-top-right bevel
        DrawLabel(bevelCorners[23], "AF"); // Back-top-left bevel
    }

    private void DrawLabel(Vector3 position, string label)
    {
        // Draw a label at the specified position
        Handles.Label(position, label);
    }
}