using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 15f;
    [SerializeField] private bool _loadingJump = false;
    [SerializeField] private bool _isAirborne = false;

    private Rigidbody _rb;

    public Transform camTransform; // Free form camera

    //Just get specific actions for now
    InputAction moveAction; 
    InputAction jumpAction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.Enable();
        jumpAction = InputSystem.actions.FindAction("Jump");
        jumpAction.Enable();
    }

    private void Start()
    {
        jumpAction.started += JumpAction_started;
        jumpAction.performed += JumpAction_performed;
        jumpAction.canceled += JumpAction_canceled;
        Debug.Log("Input enabled");

    }



    private void JumpAction_started(InputAction.CallbackContext context)
    {
        if (_isAirborne)
        {
            return;
        }

        else
        {
            _loadingJump = true;
            moveAction.Disable();
        }
    }

    private void JumpAction_performed(InputAction.CallbackContext context)
    {
        _loadingJump = false;
        moveAction.Enable();
        var forceEffect = context.duration * 2;
        _rb.AddForce(Vector3.up * (_jumpForce * (float)forceEffect), ForceMode.Impulse);    
    }

    private void JumpAction_canceled(InputAction.CallbackContext context)
    {
        if (_loadingJump == true) 
        {
            _loadingJump = false;
            moveAction.Enable();
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);  
        }
        
    }

    private void FixedUpdate()
    {
        

        Vector3 moveValue = moveAction.ReadValue<Vector3>(); //Reads value from Player_Map.Move

        //Set Camera rotation to own variables
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;

        //reset cam y transforms
        camForward.y = 0;
        camRight.y = 0;

        //Player direction depending on the camera angle in 2D plane
        Vector3 moveDir = camForward * moveValue.z + camRight * moveValue.x;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            transform.forward = moveDir;
        }

        //Move player
        _rb.linearVelocity = moveDir * _speed + Vector3.up * _rb.linearVelocity.y;

    }

}