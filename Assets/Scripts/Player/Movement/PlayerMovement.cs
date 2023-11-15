using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    public event Action<float> OnGetGrounded;
    public event Action OnGetOfTheGround;
    public event Action OnMove;
    public event Action OnStop;

    [SerializeField] private float _gravityScale = 1f;
    [SerializeField, Range(0, 1)] private float _minSpeed = 0.01f;
    [SerializeField, Range(0, 2)] private float _grounedTimer = 0.2f;

    private CharacterController _controller;
    private Vector3 _moveVector;
    private float _groundedTimerCounter = 0f;
    private float _verticalVelocity = 0f;

    private const float NON_GROUNDED_MULTIPLIER = 0.95f;
    private const float GROUNDED_MULTIPLIER = 0.05f;

    private void Awake()
    {
		_controller = GetComponent<CharacterController>();
	}

	private void LateUpdate()
	{
		_verticalVelocity -= Physics.gravity.magnitude * _gravityScale * Time.deltaTime;
        Vector3 moveVector = (_moveVector + _verticalVelocity * Vector3.up) * Time.deltaTime;

        Vector3 originalMoveVector = _moveVector;
        _moveVector *= IsGrounded
            ? GROUNDED_MULTIPLIER
            : NON_GROUNDED_MULTIPLIER
            * (1 - Time.deltaTime); 
        if (moveVector.magnitude < _minSpeed && originalMoveVector != Vector3.zero)
        {
            OnStop?.Invoke();
            _moveVector = Vector3.zero;
        }
        else
			OnMove?.Invoke();

        var collisionFlags = _controller.Move(moveVector);
        if (collisionFlags == CollisionFlags.Below)
        {
            if (!IsGrounded)
                OnGetGrounded?.Invoke(_verticalVelocity);
			IsGrounded = true;
			_groundedTimerCounter = _grounedTimer;
            _verticalVelocity = 0f;
		}
        else if (collisionFlags == CollisionFlags.Above)
        {
			_verticalVelocity = 0f;
		}
		else if (IsGrounded)
        {
            if (_groundedTimerCounter > 0)
                _groundedTimerCounter -= Time.deltaTime;
            else
            {
                IsGrounded = false;
                OnGetOfTheGround?.Invoke();
            }
		}
	}

    public void Move(Vector3 moveVector)
    {
		_moveVector = moveVector;
        _moveVector.y = 0f;
	}

    public void Jump(float jumpForce)
    {
        if (jumpForce <= 0)
            return;

        IsGrounded = false;
		_verticalVelocity = jumpForce;
	}
}
