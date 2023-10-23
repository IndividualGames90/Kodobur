using UnityEngine;

namespace IndividualGames.Player
{
    /// <summary>
    /// FPS controls utilizing Unity charactercontroller and New input system.
    /// </summary>
    public class FPSController : MonoBehaviour
    {
        public bool CanMove { get; private set; } = true;

        [Header("Game Components")]
        [SerializeField] private CharacterController _controller;
        [SerializeField] private GameObject _barrel;
        [SerializeField] private GroundedChecker _groundedChecker;

        #region CharacterController Data
        [Header("PlayerData")]
        [SerializeField] private float _moveSpeed = 2.0f;
        [SerializeField] private float _sprintSpeed = 3.0f;

        private bool _grounded = true;
        private Vector3 _playerVelocity;
        private float _jumpHeight = 1.0f;
        private float _gravityValue = -9.81f;
        #endregion

        #region PlayerInput
        private PlayerInputs _input;
        private Vector2 Movement => _input.Player.Movement.ReadValue<Vector2>();
        private bool MouseChanged => _input.Player.Look.ReadValue<Vector2>().magnitude > .1f;
        private bool Sprinting => _input.Player.Sprint.ReadValue<float>() > .5f;
        private bool Jumped => _input.Player.Jump.ReadValue<float>() > .5f;
        #endregion

        private void Awake()
        {
            _input = new();
            _groundedChecker.Grounded.Connect(OnGrounded);
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void Update()
        {
            //_grounded = _controller.isGrounded;
            if (_grounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            //move
            Vector3 moveDirection = Vector3.zero;
            if (!Sprinting)
            {
                var barrelForward = _barrel.transform.forward;
                var barrelRight = _barrel.transform.right;
                Vector3 movementVector = (barrelForward * Movement.y + barrelRight * Movement.x) * _moveSpeed * Time.deltaTime;
                movementVector.y = 0f;
                moveDirection = movementVector * _moveSpeed;
            }
            else if (Sprinting && _grounded)
            {
                var barrelForward = _barrel.transform.forward;
                moveDirection = barrelForward * Time.deltaTime * _sprintSpeed;
            }

            if (moveDirection != Vector3.zero)
            {
                _controller.Move(moveDirection);
            }

            //gravity
            if (Jumped && _grounded)
            {
                _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3f * _gravityValue);
            }
            _playerVelocity.y += _gravityValue * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        private void OnGrounded(bool grounded)
        {
            _grounded = grounded;
        }
    }
}