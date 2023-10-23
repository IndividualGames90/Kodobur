using UnityEngine;

namespace IndividualGames.Player
{
    /// <summary>
    /// Player FPS controls.
    /// </summary>
    public class FPSController : MonoBehaviour
    {
        public bool CanMove { get; private set; } = true;

        [Header("Game Components")]
        [SerializeField] private CharacterController _controller;

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
        private Vector2 PlayerMovement => _input.Player.Movement.ReadValue<Vector2>();
        private Vector2 MouseDelta => _input.Player.Look.ReadValue<Vector2>();
        private bool PlayerSprint => _input.Player.Sprint.ReadValue<float>() > .5f;
        private bool PlayerJumped => _input.Player.Jump.ReadValue<float>() > .5f;
        #endregion

        private void Awake()
        {
            _input = new();
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void Update()
        {
            _grounded = _controller.isGrounded;
            if (_grounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            //move
            if (!PlayerSprint)
            {
                Vector3 movementVector = new Vector3(PlayerMovement.x, 0f, PlayerMovement.y);
                _controller.Move(movementVector * Time.deltaTime * _moveSpeed);
            }

            Vector3 sprintVelocity;
            //sprint
            if (PlayerSprint && _grounded)
            {
                sprintVelocity = transform.forward * Time.deltaTime * _sprintSpeed;
                _controller.Move(sprintVelocity);
            }

            //gravity
            if (PlayerJumped && _grounded)
            {
                _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3f * _gravityValue);
            }
            _playerVelocity.y += _gravityValue * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }

    }
}