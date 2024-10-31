using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;

    private void Update()
    {
        CameraMove();
    }
        
    private void CameraMove()
    {
        transform.position = cameraPosition.position;
    }
}
