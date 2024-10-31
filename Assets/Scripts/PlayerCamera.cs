using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float sensitivity = 400f;
    
    [SerializeField] private Transform playerBody;
    //[SerializeField] private Transform _orientation;
    
    private float _xRotation;
    private float _yRotation;
    private float _zRotation = 0.00f;
    
    private float _mouseX;
    private float _mouseY;
    private float _minAngle = -80.00f;
    private float _maxAngle = 80.00f;

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        MousePlayerInput();
    }

    private void FixedUpdate()
    {
        MouseMove();
    }
    
    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void MousePlayerInput()
    {
        _mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        _mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;
    }
        
    private void MouseMove()
    {
        _yRotation += _mouseX;
        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, _minAngle, _maxAngle);

        transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, _zRotation);
        //_orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
        playerBody.Rotate(Vector3.up * _mouseX);
    }
}