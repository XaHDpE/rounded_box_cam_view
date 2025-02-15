using UnityEngine;

class MeshHelper
{
    
    public static int FindClosestVertex(Vector3 position, Transform bo)
    {
        // Получаем меш из объекта
        var meshFilter = bo.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter не найден на объекте " + bo.name);
            return -1;
        }

        // Получаем вершины меша
        Vector3[] verticesArr = meshFilter.mesh.vertices;

        // Находим ближайшую вершину меша к заданной позиции
        int closestIndex = 0;
        float closestDistance = Vector3.Distance(position, bo.TransformPoint(verticesArr[0]));

        // Проходим по всем вершинам меша
        for (int i = 1; i < verticesArr.Length; i++)
        {
            // Преобразуем вершину в мировые координаты
            Vector3 worldVertex = bo.TransformPoint(verticesArr[i]);

            // Вычисляем расстояние до вершины
            float distance = Vector3.Distance(position, worldVertex);

            // Если текущая вершина ближе, обновляем индекс ближайшей вершины
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        // Возвращаем индекс ближайшей вершины
        return closestIndex;
    }
    
    public static Vector3 GetClosestPointOnCollider(Transform targetObject, Vector3 testPoint)
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target object is null.");
            return Vector3.zero;
        }

        // Get the collider of the target object
        var collider = targetObject.GetComponent<Collider>();

        if (collider == null)
        {
            Debug.LogWarning("Target object does not have a collider.");
            return Vector3.zero;
        }

        // Find and return the closest point on the surface of the collider
        return collider.ClosestPoint(testPoint);
    }
    
}