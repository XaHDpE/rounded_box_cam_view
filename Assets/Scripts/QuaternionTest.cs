using UnityEngine;

public class QuaternionTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject oppositeObj;
    [SerializeField] private Quaternion finalRot;
    [SerializeField] private Vector3 surfaceNormal;

    private void Start()
    {
        finalRot = Quaternion.Euler(0, 0, 45);
        surfaceNormal = oppositeObj.transform.up.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.right, surfaceNormal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}
