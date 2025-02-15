using UnityEngine;

public class SceneComposer : MonoBehaviour
{
    [SerializeField] private RoundedBox roundedBox;
    [SerializeField] private Transform target;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform camPivot;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        CenterTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CenterTarget()
    {
        var bounds = roundedBox.GetComponent<Renderer>().bounds;
        var trajectoryCenter = bounds.center;
        target.position = trajectoryCenter;
        
    }
    
}
