using UnityEngine;

public class RoundedRectangleMovement : MonoBehaviour
{
    // Parameters of the described rounded box
    public float a = 4f; // Width of the box
    public float b = 3f; // Height of the box
    public float c = 2f; // Length of the box
    public float r = 0.5f; // Radius of the rounded corners

    // Speed of movement (units per second)
    public float speed = 1f;

    // Parameter t (time) to trace the boundary
    private float t = 0f;

    // Precomputed values
    private float straightLength, cornerLength, totalLength;

    // Segment boundary variables
    private float rightEdgeEnd;
    private float topRightCornerEnd;
    private float topEdgeEnd;
    private float topLeftCornerEnd;
    private float leftEdgeEnd;
    private float bottomLeftCornerEnd;
    private float bottomEdgeEnd;

    // Structure to store segment boundaries and logic
    private struct Segment
    {
        public float Start;
        public float End;
        public System.Func<float, Vector3> ComputePosition;

        public Segment(float start, float end, System.Func<float, Vector3> computePosition)
        {
            Start = start;
            End = end;
            ComputePosition = computePosition;
        }
    }

    private Segment[] segments;

    private void Start()
    {
        PrecomputeValues();
        InitializeSegments();
    }

    private void Update()
    {
        // Increment t based on time and speed
        t += Time.deltaTime * speed;

        // Ensure t loops within the range [0, 1]
        if (t > 1f)
        {
            t -= 1f;
        }

        // Compute the position using parametric equations
        Vector3 newPosition = ComputePosition(t);

        // Update the object's position
        transform.position = newPosition;
    }

    private void PrecomputeValues()
    {
        // Precompute values
        straightLength = 2 * (a + b) - 8 * r; // Total length of straight edges
        cornerLength = 2 * Mathf.PI * r; // Total length of rounded corners (4 quarter-circles)
        totalLength = straightLength + cornerLength;

        // Precompute segment boundaries
        rightEdgeEnd = b - 2 * r;
        topRightCornerEnd = rightEdgeEnd + Mathf.PI * r / 2;
        topEdgeEnd = topRightCornerEnd + (a - 2 * r); // Corrected length for the top edge
        topLeftCornerEnd = topEdgeEnd + Mathf.PI * r / 2;
        leftEdgeEnd = topLeftCornerEnd + b - 2 * r;
        bottomLeftCornerEnd = leftEdgeEnd + Mathf.PI * r / 2;
        bottomEdgeEnd = bottomLeftCornerEnd + (a - 2 * r);
    }

    private void InitializeSegments()
    {
        // Define segment boundaries and corresponding logic
        segments = new[]
        {
            new Segment(0, rightEdgeEnd, normalizedT => 
            {
                // Right edge (bottom to top)
                return new Vector3(a / 2, -b / 2 + r + normalizedT, 0);
            }),
            new Segment(rightEdgeEnd, topRightCornerEnd, normalizedT => 
            {
                // Top-right corner (quarter-circle segment)
                float angle = (normalizedT - rightEdgeEnd) / r;
                return ComputeCornerPosition(a / 2 - r, b / 2 - r, angle);
            }),
            new Segment(topRightCornerEnd, topEdgeEnd, normalizedT => 
            {
                // Top edge (right to left)
                return new Vector3(a / 2 - r - (normalizedT - topRightCornerEnd), b / 2, 0);
            }),
            new Segment(topEdgeEnd, topLeftCornerEnd, normalizedT => 
            {
                // Top-left corner (quarter-circle segment)
                float angle = Mathf.PI / 2 + (normalizedT - topEdgeEnd) / r;
                return ComputeCornerPosition(-a / 2 + r, b / 2 - r, angle);
            }),
            new Segment(topLeftCornerEnd, leftEdgeEnd, normalizedT => 
            {
                // Left edge (top to bottom)
                return new Vector3(-a / 2, b / 2 - r - (normalizedT - topLeftCornerEnd), 0);
            }),
            new Segment(leftEdgeEnd, bottomLeftCornerEnd, normalizedT => 
            {
                // Bottom-left corner (quarter-circle segment)
                float angle = Mathf.PI + (normalizedT - leftEdgeEnd) / r;
                return ComputeCornerPosition(-a / 2 + r, -b / 2 + r, angle);
            }),
            new Segment(bottomLeftCornerEnd, bottomEdgeEnd, normalizedT => 
            {
                // Bottom edge (left to right)
                return new Vector3(-a / 2 + r + (normalizedT - bottomLeftCornerEnd), -b / 2, 0);
            }),
            new Segment(bottomEdgeEnd, totalLength, normalizedT => 
            {
                // Bottom-right corner (quarter-circle segment)
                float angle = 3 * Mathf.PI / 2 + (normalizedT - bottomEdgeEnd) / r;
                return ComputeCornerPosition(a / 2 - r, -b / 2 + r, angle);
            })
        };
    }

    private Vector3 ComputeCornerPosition(float centerX, float centerY, float angle)
    {
        // Helper method to compute positions for rounded corners
        float x = centerX + r * Mathf.Cos(angle);
        float y = centerY + r * Mathf.Sin(angle);
        return new Vector3(x, y, 0);
    }

    private Vector3 ComputePosition(float t)
    {
        // Normalize t to the total length
        float normalizedT = t * totalLength;

        // Find the segment that contains normalizedT
        int segmentIndex = 0;
        while (segmentIndex < segments.Length && normalizedT >= segments[segmentIndex].End)
        {
            segmentIndex++;
        }

        // Handle cases at segment boundaries
        if (segmentIndex == segments.Length)
        {
            // Loop back to the first segment if t exceeds totalLength
            return segments[0].ComputePosition(0);
        }
        else if (Mathf.Approximately(normalizedT, segments[segmentIndex].End))
        {
            // Use the end position of the previous segment for exact match
            return segments[segmentIndex - 1].ComputePosition(segments[segmentIndex - 1].End);
        }
        else
        {
            // Use the segment's logic to compute position within the segment
            return segments[segmentIndex].ComputePosition(normalizedT);
        }
    }

    private void OnDrawGizmos()
    {
        // Ensure values are precomputed even in the editor
        PrecomputeValues();
        InitializeSegments();

        // Draw the original box
        DrawOriginalBox();

        // Draw the rounded rectangle trajectory on the main face (Z = 0)
        DrawRoundedRectangleTrajectory(0f);

        // Draw the rounded rectangle trajectory on the front face (Z = c / 2)
        DrawRoundedRectangleTrajectory(c / 2);

        // Draw the rounded rectangle trajectory on the back face (Z = -c / 2)
        DrawRoundedRectangleTrajectory(-c / 2);

        // Draw debug rays at the start and end of each segment
        DrawSegmentMarkers();
    }

    private void DrawRoundedRectangleTrajectory(float zOffset)
    {
        int segmentsCount = 100;
        for (int i = 0; i < segmentsCount; i++)
        {
            float t1 = i / (float)segmentsCount;
            float t2 = (i + 1) / (float)segmentsCount;
            Vector3 pos1 = ComputePosition(t1);
            Vector3 pos2 = ComputePosition(t2);

            // Apply Z offset for front and back faces
            pos1.z = zOffset;
            pos2.z = zOffset;

            // Apply additional offset for front and back faces
            if (Mathf.Abs(zOffset) > 0) // Only for front and back faces
            {
                // Right edge (red segment)
                if (Mathf.Approximately(pos1.x, a / 2) && Mathf.Approximately(pos2.x, a / 2))
                {
                    pos1.x -= r;
                    pos2.x -= r;
                }
                // Left edge (magenta segment)
                else if (Mathf.Approximately(pos1.x, -a / 2) && Mathf.Approximately(pos2.x, -a / 2))
                {
                    pos1.x += r;
                    pos2.x += r;
                }
                // Top edge (green segment)
                else if (Mathf.Approximately(pos1.y, b / 2) && Mathf.Approximately(pos2.y, b / 2))
                {
                    pos1.y -= r;
                    pos2.y -= r;
                }
                // Bottom edge (blue segment)
                else if (Mathf.Approximately(pos1.y, -b / 2) && Mathf.Approximately(pos2.y, -b / 2))
                {
                    pos1.y += r;
                    pos2.y += r;
                }
            }

            Gizmos.color = GetSegmentColor(t1);
            Gizmos.DrawLine(pos1, pos2);
        }
    }

    private void DrawOriginalBox()
    {
        // Define the 8 corners of the box
        Vector3[] corners = new Vector3[8]
        {
            new(-a / 2, -b / 2, -c / 2), // Bottom-back-left
            new(a / 2, -b / 2, -c / 2),  // Bottom-back-right
            new(a / 2, b / 2, -c / 2),   // Top-back-right
            new(-a / 2, b / 2, -c / 2),  // Top-back-left
            new(-a / 2, -b / 2, c / 2),  // Bottom-front-left
            new(a / 2, -b / 2, c / 2),   // Bottom-front-right
            new(a / 2, b / 2, c / 2),    // Top-front-right
            new(-a / 2, b / 2, c / 2)    // Top-front-left
        };

        // Set the color for the original box
        Gizmos.color = Color.gray;

        // Draw the 12 edges of the box
        DrawBoxEdge(corners[0], corners[1]); // Bottom-back edge
        DrawBoxEdge(corners[1], corners[2]); // Right-back edge
        DrawBoxEdge(corners[2], corners[3]); // Top-back edge
        DrawBoxEdge(corners[3], corners[0]); // Left-back edge

        DrawBoxEdge(corners[4], corners[5]); // Bottom-front edge
        DrawBoxEdge(corners[5], corners[6]); // Right-front edge
        DrawBoxEdge(corners[6], corners[7]); // Top-front edge
        DrawBoxEdge(corners[7], corners[4]); // Left-front edge

        DrawBoxEdge(corners[0], corners[4]); // Bottom-left edge
        DrawBoxEdge(corners[1], corners[5]); // Bottom-right edge
        DrawBoxEdge(corners[2], corners[6]); // Top-right edge
        DrawBoxEdge(corners[3], corners[7]); // Top-left edge
    }

    private void DrawBoxEdge(Vector3 start, Vector3 end)
    {
        // Helper method to draw a single edge of the box
        Gizmos.DrawLine(start, end);
    }

    private void DrawSegmentMarkers()
    {
        // Draw markers for the start and end of each segment
        foreach (var segment in segments)
        {
            var startPos = ComputePosition(segment.Start / totalLength);
            var endPos = ComputePosition(segment.End / totalLength);

            // Draw a ray at the start of the segment (along +Z axis)
            Gizmos.color = Color.white;
            Gizmos.DrawRay(startPos, Vector3.forward * 0.5f);

            // Draw a ray at the end of the segment (along -Z axis)
            Gizmos.color = Color.black;
            Gizmos.DrawRay(endPos, Vector3.back * 0.5f);
        }
    }

    private Color GetSegmentColor(float t)
    {
        float normalizedT = t * totalLength;

        if (normalizedT < rightEdgeEnd) return Color.red; // Right edge
        if (normalizedT < topRightCornerEnd) return Color.yellow; // Top-right corner
        if (normalizedT < topEdgeEnd) return Color.green; // Top edge
        if (normalizedT < topLeftCornerEnd) return Color.cyan; // Top-left corner
        if (normalizedT < leftEdgeEnd) return Color.magenta; // Left edge
        if (normalizedT < bottomLeftCornerEnd) return Color.white; // Bottom-left corner
        if (normalizedT < bottomEdgeEnd) return Color.blue; // Bottom edge
        return Color.black; // Bottom-right corner
    }
}