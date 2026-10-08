using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EscPlayerController : MonoBehaviour
{
    #region 인스펙터
    [Header("이동")]
    [SerializeField] private float _moveSpeed = 6.0f;
    [SerializeField] private Transform _camTr;

    [Header("회전")]
    [SerializeField] private float _turnSharpness = 15.0f;

    [Header("점프 / 중력")]
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _gravity = -15.0f;
    [SerializeField] private float _groundedStick = -2.0f;
    [SerializeField] private float _jumpBufferTime = 0.15f;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    #endregion

    #region 내부 변수
    private CharacterController _cc;

    private Vector3 _moveDir = Vector3.zero;
    private float _verticalVelocity = 0.0f;
    private float _jumpPressedTime = -1.0f;
    #endregion

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();

        if (_cc == null)
        {
            Debug.LogWarning("캐릭터 컨트롤러 없음 (EscPlayerController)");
            enabled = false;

            return;
        }

        if (_camTr == null && Camera.main != null)
        {
            _camTr = Camera.main.transform;
        }

        if (_camTr == null)
        {
            Debug.LogWarning("카메라 없음 / 월드 축 기준으로 이동 (EscPlayerController)");
        }
    }

    private void Update()
    {
        ReadInput();
        TickGravityAndJump();
        ApplyMove();
        TickRotation();
    }

    private void ReadInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

        if (_camTr != null)
        {
            forward = Vector3.ProjectOnPlane(_camTr.forward, Vector3.up).normalized;
            right = Vector3.ProjectOnPlane(_camTr.right, Vector3.up).normalized;
        }

        Vector3 dir = forward * v + right * h;
        _moveDir = Vector3.ClampMagnitude(dir, 1.0f);

        if (Input.GetKeyDown(_jumpKey))
        {
            _jumpPressedTime = Time.time;
        }
    }

    private void TickGravityAndJump()
    {
        if (_cc.isGrounded && _verticalVelocity < 0.0f)
        {
            _verticalVelocity = _groundedStick;
        }

        if (Time.time - _jumpPressedTime >= _jumpBufferTime)
        {
            _jumpPressedTime = -1.0f;
        }

        if (_jumpPressedTime >= 0.0f && _cc.isGrounded)
        {
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
            _jumpPressedTime = -1.0f;
        }

        _verticalVelocity += _gravity * Time.deltaTime;
    }

    private void ApplyMove()
    {
        Vector3 velocity = _moveDir * _moveSpeed + Vector3.up * _verticalVelocity;
        _cc.Move(velocity * Time.deltaTime);
    }

    private void TickRotation()
    {
        if (_moveDir.sqrMagnitude < 0.0001f)
        {
            return;
        }

        float t = 1.0f - Mathf.Exp(-_turnSharpness * Time.deltaTime);

        Quaternion rot = Quaternion.LookRotation(_moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, t);
    }
}