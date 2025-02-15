using UnityEngine;

public class SurfaceMover1 : MonoBehaviour
{
    public RectCircleController shapeController; // Ссылка на контроллер фигуры
    public float moveSpeed = 2f; // Скорость перемещения
    public float pauseTime = 1f; // Время паузы между перемещениями

    private Vector3[] allCorners; // Все вершины фигуры
    private int[][] externalEdges; // Список внешних ребер (каждое ребро — это массив из двух индексов вершин)
    private int currentEdgeStartIndex = 0; // Индекс начальной вершины текущего ребра
    private int currentEdgeEndIndex = 1; // Индекс конечной вершины текущего ребра
    private float progressAlongEdge = 0f; // Прогресс перемещения вдоль ребра (0 до 1)
    private float pauseTimer = 0f; // Таймер для паузы

    private void Start()
    {
        // Инициализация массива всех вершин
        allCorners = new Vector3[shapeController.corners.Length + shapeController.bevelCorners.Length];
        shapeController.corners.CopyTo(allCorners, 0);
        shapeController.bevelCorners.CopyTo(allCorners, shapeController.corners.Length);

        // Инициализация списка внешних ребер
        externalEdges = new int[][]
        {
            new int[] { 0, 1 },  // Ребро между вершинами A и B
            new int[] { 1, 2 },  // Ребро между вершинами B и C
            new int[] { 2, 3 },  // Ребро между вершинами C и D
            new int[] { 3, 0 },  // Ребро между вершинами D и A
            new int[] { 4, 5 },  // Ребро между вершинами E и F
            new int[] { 5, 6 },  // Ребро между вершинами F и G
            new int[] { 6, 7 },  // Ребро между вершинами G и H
            new int[] { 7, 4 },  // Ребро между вершинами H и E
            new int[] { 0, 4 },  // Ребро между вершинами A и E
            new int[] { 1, 5 },  // Ребро между вершинами B и F
            new int[] { 2, 6 },  // Ребро между вершинами C и G
            new int[] { 3, 7 }   // Ребро между вершинами D и H
        };

        // Начальная позиция объекта
        if (allCorners.Length > 0)
        {
            transform.position = allCorners[0];
        }
    }

    private void Update()
    {
        if (allCorners.Length == 0) return;

        // Если пауза, ждем
        if (pauseTimer > 0)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        // Перемещение вдоль текущего ребра
        Vector3 startPosition = allCorners[currentEdgeStartIndex];
        Vector3 endPosition = allCorners[currentEdgeEndIndex];
        progressAlongEdge += moveSpeed * Time.deltaTime / Vector3.Distance(startPosition, endPosition);

        // Если достигли конца ребра, выбираем следующее ребро
        if (progressAlongEdge >= 1f)
        {
            progressAlongEdge = 0f;
            pauseTimer = pauseTime; // Начинаем паузу

            // Выбираем следующее ребро
            currentEdgeStartIndex = currentEdgeEndIndex;
            currentEdgeEndIndex = GetNextEdgeEndIndex(currentEdgeStartIndex);
        }

        // Позиция объекта вдоль ребра
        transform.position = Vector3.Lerp(startPosition, endPosition, progressAlongEdge);
    }

    private int GetNextEdgeEndIndex(int startIndex)
    {
        // Ищем следующее ребро, начинающееся с текущей вершины
        foreach (var edge in externalEdges)
        {
            if (edge[0] == startIndex)
            {
                return edge[1];
            }
        }
        return 0; // Если следующее ребро не найдено, возвращаем первую вершину
    }
}