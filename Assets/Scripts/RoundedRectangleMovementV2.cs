using UnityEngine;

public class RoundedRectangleMovementV2 : MonoBehaviour
{
    public float width = 5f;
    public float height = 3f;
    public float radius = 1f;
    public float speed = 2f;

    private float progress = 0f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Draw top edge
        Gizmos.DrawLine(new Vector3(-width / 2f + radius, height / 2f, 0f), 
                        new Vector3(width / 2f - radius, height / 2f, 0f));

        // Draw right edge
        Gizmos.DrawLine(new Vector3(width / 2f - radius, height / 2f, 0f), 
                        new Vector3(width / 2f, height / 2f - radius, 0f));

        // Draw top-right corner (quarter-circle)
        DrawQuarterCircle(new Vector3(width / 2f - radius, height / 2f - radius, 0f), radius, 0f, -Mathf.PI / 2f);

        // Draw bottom edge
        Gizmos.DrawLine(new Vector3(width / 2f - radius, -height / 2f, 0f), 
                        new Vector3(-width / 2f + radius, -height / 2f, 0f));

        // Draw bottom-right corner (quarter-circle)
        DrawQuarterCircle(new Vector3(width / 2f - radius, -height / 2f + radius, 0f), radius, -Mathf.PI / 2f, 0f);

        // Draw left edge
        Gizmos.DrawLine(new Vector3(-width / 2f + radius, -height / 2f, 0f), 
                        new Vector3(-width / 2f, -height / 2f + radius, 0f));

        // Draw bottom-left corner (quarter-circle)
        DrawQuarterCircle(new Vector3(-width / 2f + radius, -height / 2f + radius, 0f), radius, Mathf.PI, Mathf.PI / 2f);

        // Draw top-left corner (quarter-circle)
        DrawQuarterCircle(new Vector3(-width / 2f + radius, height / 2f - radius, 0f), radius, Mathf.PI / 2f, Mathf.PI);
    }

    private void DrawQuarterCircle(Vector3 center, float radius, float startAngle, float endAngle)
    {
        int segments = 32;
        float angleStep = (endAngle - startAngle) / segments;

        Vector3 lastPoint = Vector3.zero; 

        for (int i = 0; i <= segments; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector3 point = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);

            if (i > 0) 
            {
                Gizmos.DrawLine(lastPoint, point); 
            }

            lastPoint = point;
        }
    }

    Vector3 lastPoint;
    
    void Update()
    {
        progress += Time.deltaTime * speed;

        // Calculate position based on progress (0 to 1)
        float x = Mathf.Lerp(-width / 2f, width / 2f, progress);
        float y; // Declare y variable here

        // Handle corner cases for smoother transitions
        if (progress < 0.25f) 
        {
            // Top-Left corner
            float t = progress * 4f; // Scale progress for this quadrant
            x = -width / 2f + radius * Mathf.Cos(Mathf.PI * 0.5f * t); 
            y = height / 2f - radius * Mathf.Sin(Mathf.PI * 0.5f * t);
        }
        else if (progress < 0.5f) 
        {
            // Top edge
            x = Mathf.Lerp(-width / 2f + radius, width / 2f - radius, (progress - 0.25f) * 4f);
            y = height / 2f;
        }
        else if (progress < 0.75f) 
        {
            // Top-Right corner
            float t = (progress - 0.5f) * 4f; 
            x = width / 2f - radius * Mathf.Cos(Mathf.PI * 0.5f * t); 
            y = height / 2f - radius * Mathf.Sin(Mathf.PI * 0.5f * t);
        }
        else if (progress < 1f) 
        {
            // Right edge
            x = Mathf.Lerp(width / 2f - radius, width / 2f, (progress - 0.75f) * 4f);
            y = height / 2f;
        }
        else 
        {
            // Reset progress for looping
            progress -= 1f; 
            x = -width / 2f; // Start from the left again
            y = height / 2f; 
        }

        // Apply position to the object
        transform.position = new Vector3(x, y, 0f); 
    }
    
}
