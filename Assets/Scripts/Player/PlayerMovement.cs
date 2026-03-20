using UnityEngine;
using static PlayerActions;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 15f;
    private bool _jumped = false;

    private PlayerActions _playerActions;
    private Rigidbody _rb;
    private Vector3 _moveInput;

    public Transform camTransform;

    private void Awake()
    {
        _playerActions = new PlayerActions();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //OnEnable();
        _playerActions.Player_Map.Jump.performed += Jump_performed;
        _playerActions.Player_Map.Jump.canceled += Jump_canceled;
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _jumped = true;
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        Debug.Log("Jumped");
    }

    private void Jump_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_jumped == false)
        {
            var forceEffect = obj.duration;
            _rb.AddForce(Vector3.up * (_jumpForce * (float)forceEffect), ForceMode.Impulse);
        }
    }

    private void OnEnable()
    {
        _playerActions.Player_Map.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Player_Map.Disable();
    }


    private void FixedUpdate()
    {
        _moveInput = _playerActions.Player_Map.Move.ReadValue<Vector3>();

        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        Vector3 moveDir = camForward * _moveInput.z + camRight * _moveInput.x;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            transform.forward = moveDir;
        }

        if (!_jumped) 
        {
            _rb.linearVelocity = moveDir * _speed + Vector3.up * _rb.linearVelocity.y;
        }

    }
}