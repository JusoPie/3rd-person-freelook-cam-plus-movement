using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 5;

    private PlayerActions _playerActions;
    private Rigidbody _rb;
    private Vector3 _moveInput;

    public Transform camTransform;

    private void Awake()
    {
        _playerActions = new PlayerActions();
        _rb = GetComponent<Rigidbody>();
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

        //camForward.Normalize();
        //camRight.Normalize(); Not necessary

        Vector3 moveDir = camForward * _moveInput.z + camRight * _moveInput.x;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            transform.forward = moveDir;
        }

        _rb.linearVelocity = moveDir * _speed;
    }
}