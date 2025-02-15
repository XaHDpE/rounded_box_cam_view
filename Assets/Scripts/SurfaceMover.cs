using System;
using UnityEngine;



public class SurfaceMover : MonoBehaviour
{
    
    // Объект, по поверхности которого будет двигаться mover
    public Transform baseObject; 

    // Скорость движения объекта
    public float moveSpeed = 25f; 

    // Плавность поворота объекта (чем выше значение, тем плавнее поворот)
    public float rotationSmoothness = 125f; 

    // Текущая позиция объекта (публичная переменная для отладки)
    public Vector3 currentPosition;

    // Меш (сетка) базового объекта
    private Mesh _baseMesh;

    // Вершины меша базового объекта
    private Vector3[] _vertices;

    // Нормали меша базового объекта
    private Vector3[] _normals;

    private void Start()
    {
        // Проверка, что базовый объект назначен
        if (baseObject == null)
        {
            Debug.LogError("Base object is not assigned!");
            return;
        }

        // Получаем компонент MeshFilter базового объекта
        var meshFilter = baseObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("Base object does not have a MeshFilter!");
            return;
        }

        // Получаем меш и данные о вершинах и нормалях
        _baseMesh = meshFilter.mesh;
        _vertices = _baseMesh.vertices;
        _normals = _baseMesh.normals;
        
    }

    private void Update()
    {
        // Проверка, что меш и данные о вершинах и нормалях загружены
        if (_baseMesh == null || _vertices == null || _normals == null)
            return;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            MoveOnSurface(-transform.right);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            MoveOnSurface(transform.right);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            MoveOnSurface(transform.forward);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            MoveOnSurface(-transform.forward);
        currentPosition = transform.position;
    }
    
    private void MoveOnSurface(Vector3 direction)
    {
        var closestVertexIndex = MeshHelper.FindClosestVertex(transform.position, baseObject);
        var surfaceNormal = baseObject.TransformDirection(_normals[closestVertexIndex]);
        var newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
        newPosition = ProjectOnSurface(newPosition);
        if (IsPositionValid(newPosition))
            transform.position = newPosition;
        
        var targetByUpRotation = Quaternion.FromToRotation(transform.up, surfaceNormal);
        var targetByRightRotation = Vector3.Cross(surfaceNormal, Vector3.up);

        var rightOffsetRotationAfterTargetByUpRotation = Quaternion.FromToRotation(
            targetByUpRotation * transform.right, targetByRightRotation);

        var targetRotation = rightOffsetRotationAfterTargetByUpRotation * targetByUpRotation * transform.rotation;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
        
        Debug.DrawLine(transform.position, transform.position + surfaceNormal, Color.green, 10f);
        Debug.DrawLine(transform.position, transform.position + targetByRightRotation, Color.red, 10f);
        
        // Debug.Log($"Position: {transform.position}, Normal: {surfaceNormal}, Closest Vertex: {_vertices[closestVertexIndex]}");
    }


    private Vector3 ProjectOnSurface(Vector3 position)
    {
        // Находим ближайшую вершину меша к заданной позиции
        var closestVertexIndex = MeshHelper.FindClosestVertex(position, baseObject);

        // Получаем нормаль поверхности в ближайшей вершине
        var surfaceNormal = baseObject.TransformDirection(_normals[closestVertexIndex]);

        // Получаем позицию ближайшей вершины в мировых координатах
        var surfacePoint = baseObject.TransformPoint(_vertices[closestVertexIndex]);

        // Проецируем позицию на поверхность, используя нормаль и точку поверхности
        var projectedPosition = position - surfaceNormal * Vector3.Dot(position - surfacePoint, surfaceNormal);

        // Возвращаем спроецированную позицию
        return projectedPosition;
    }

    private bool IsPositionValid(Vector3 position)
    {
        // Проверяем, находится ли позиция в пределах меша
        Bounds meshBounds = _baseMesh.bounds;
        Vector3 localPosition = baseObject.InverseTransformPoint(position);

        return meshBounds.Contains(localPosition);
    }
}