using UnityEngine;


public class LookAtController : MonoBehaviour
{
    public Transform target;
    public Transform rb;

    private Mesh _baseMesh;
    private Vector3 _currentLookAt;

    private Quaternion _prevRotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _baseMesh = rb.GetComponent<MeshFilter>().mesh;
    }
    
    private void Update()
    {
        // Получаем точку, на которую нужно смотреть
        _currentLookAt = MeshHelper.GetClosestPointOnCollider(target, transform.position);

        // Вычисляем целевое вращение
        var targetRotation = Quaternion.LookRotation(_currentLookAt - transform.position);

        if (targetRotation != _prevRotation)
            Debug.Log($"changed, prev: {_prevRotation}, current: {targetRotation}");
        
        transform.LookAt(_currentLookAt);
        _prevRotation = targetRotation;
    }
    
}
