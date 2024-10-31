using UnityEngine;

public class PlayerRigidbodyMovement : MonoBehaviour
{
    #region Camera

    [SerializeField] private Transform _cameraTransform;

    private Vector2 _mouseInput;
    private float _xRotation;
    [Range(0.1f, 10.0f)] [SerializeField] private float _mouseSensitivity = 7.0f;
    [Range(-90.0f, .0f)] [SerializeField] private float _minVert = -60.0f;
    [Range(0.0f, 90.0f)] [SerializeField] private float _maxVert = 60.0f;
    private string _mouseX = "Mouse X";
    private string _mouseY = "Mouse Y";
    
    #endregion
    
    /// <summary>
    /// Ground check
    /// </summary>
    [SerializeField] private float _groundDrag = 5f;
    [SerializeField] private float _rayLength = 1.5f;
    [SerializeField] private LayerMask _whatIsGrounded;
    [SerializeField] private bool _grounded;
    private RaycastHit _hit;
    
    #region RigidbodyMove

    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private Transform orientation;
    private Vector3 _playerMoveInput;
    private Vector3 _moveVector;
    private float _speed = 10.0f;
    private float _jumpForce = 8.0f;
    private string _horizontal = "Horizontal";
    private string _vertical = "Vertical";

    #endregion
    
    /// <summary>
    /// Jump
    /// </summary>
    [SerializeField] private bool _jumpInput;
    [SerializeField] private bool _readyToJump;
    [SerializeField] private float _jumpCooldown = 0.25f;
    
    private void Start()
    {
        GetRigidbody();
        FreezeRigidbodyRotation();
        LockCursor();
    }

    private void Update()
    {
        GroundCheck();
        PlayerMoveInput();
        GetMouseInput();
        JumpInput();
        
        if (_grounded)
        {
            Debug.Log("Collide with" + _hit.collider.name);
            rigidbody.drag = _groundDrag;
            ResetJump();
        }
        else
        {
            Debug.Log("No collide");
            rigidbody.drag = 0f;
        }

        if (_jumpInput && _grounded && _readyToJump)
        {
            _readyToJump = false;
            Debug.Log("Jump");
            Jump(_jumpForce);
            
            Invoke(nameof(_jumpInput), _jumpCooldown);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        MoveCamera();
    }

    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void GetRigidbody()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    
    private void FreezeRigidbodyRotation()
    {
        rigidbody.freezeRotation = true;
    }

    private void GetMouseInput()
    {
        _mouseInput = new Vector2(Input.GetAxis(_mouseX), Input.GetAxis(_mouseY));
    }

    private void MoveCamera()
    {
        _xRotation -= _mouseInput.y * _mouseSensitivity;
        _xRotation = Mathf.Clamp(_xRotation, _minVert, _maxVert);
        transform.Rotate(0f, _mouseInput.x * _mouseSensitivity, 0f);
        _cameraTransform.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void PlayerMoveInput()
    {
        _playerMoveInput = new Vector3(Input.GetAxis(_horizontal), 0, Input.GetAxis(_vertical));
    }

    private void MovePlayer()
    {
        _moveVector = transform.TransformDirection(_playerMoveInput) * _speed;
        rigidbody.velocity = new Vector3(_moveVector.x, rigidbody.velocity.y, _moveVector.z);
        //rigidbody.AddForce(new Vector3(_moveVector.x, rigidbody.velocity.y, _moveVector.z), ForceMode.VelocityChange);
    }

    private void JumpInput()
    {
        _jumpInput = Input.GetKeyDown(KeyCode.Space);
    }

    private void Jump(float force)
    {
        _readyToJump = false;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, 
            rigidbody.velocity.z);
        rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
    }
    
    private void GroundCheck()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, out _hit, 
            _rayLength, _whatIsGrounded);
        Debug.DrawRay(orientation.position, Vector3.down, Color.red, _rayLength);
    }
    
    private void ResetJump()
    {
        _readyToJump = true;
    }
}
