using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private Animator _animator;
    [SerializeField] ObjectStatus _objectStatus;
    public float Speed = 1.5f;
    public float RunningSpeed = 3;
    private const float _runningInputReady = 1f;
    public float _canRunningInput = 0;
    public bool _canRunning = false;
    private float _playerPositionZ= 0;
    private bool _isMoving = false;
    //private float _playerYPos = 0;
    private void Awake()
    {
        _playerPositionZ = transform.position.y;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _objectStatus = GetComponent<ObjectStatus>();
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
            if (_objectStatus.CanMove == true)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                Vector3 _moveDir = new Vector3(h, v, v).normalized;
                if (_canRunning == false) 
                { 
                    _animator.SetFloat("Speed", Speed); 
                    transform.Translate(Time.deltaTime * Speed * new Vector3( _moveDir.x,  _moveDir.y, _moveDir.z)); 
                }
                else 
                { 
                    _animator.SetFloat("Speed", RunningSpeed);
                    transform.Translate(Time.deltaTime * RunningSpeed * new Vector3( _moveDir.x, _moveDir.y,  _moveDir.z));
                }
                _animator.SetBool("IsMove", true);
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
            _animator.SetBool("IsMove", false);
            if (_canRunningInput >= 0)
            {
                _canRunning = true;
            }
            else { _canRunning = false; }
        }
    }
    public void SetRunningCondition()
    {
        _canRunningInput -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _canRunningInput = _runningInputReady;
        }
    }
    public void Update()
    {
        SetRunningCondition();
        Movement();
    }
}
