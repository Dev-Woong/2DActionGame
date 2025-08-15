using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private Animator _animator;
    private PlayerAttackController _playerAttackController;
    public float Speed = 2f;
    private const float _runningInputReady = 0.5f;
    private float _canRunningTime = 0;
    private bool _canRunning = false;
    private float _playerPositionZ= 0;
    private bool _isMoving = false;
    private float _playerYPos = 0;
    private float _moveInputTime = 0;
    private void Awake()
    {
        _playerPositionZ = transform.position.y;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _playerAttackController = GetComponent<PlayerAttackController>();
    }
    void Start()
    {   
        transform.position = new Vector3(transform.position.x, transform.position.y ,_playerPositionZ);
    }
    public void OffsetZPos()
    {
        if (_isMoving == true)
        {
            
        }
    }
    void Movement()
    {
        if (Input.GetButton("Horizontal")||Input.GetButton("Vertical"))
        {
            if (_playerAttackController.IsAttacking == false)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                Vector3 _moveDir = new Vector3(h, v, v / 100);
                _animator.SetBool("IsMove", true);
                _animator.SetFloat("Speed", Speed);
                _rigidbody.linearVelocity = Time.fixedDeltaTime * new Vector3(Speed * _moveDir.x, Speed * _moveDir.y);
                _transform.Translate(0, 0, _moveDir.z);
                if (h < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (h > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
        else
        {
            _isMoving = false;
            _rigidbody.linearVelocity = Vector3.zero;
            _animator.SetBool("IsMove", false);
        }
    }
    public void SetRunningCondition()
    {
        _canRunningTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _canRunningTime = _runningInputReady;
        }
    }
    public void Update()
    {
        
    }
    void FixedUpdate()
    {
        Movement();
    }
}
