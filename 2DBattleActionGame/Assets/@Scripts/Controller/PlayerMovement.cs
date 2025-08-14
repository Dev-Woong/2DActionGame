using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private PlayerAttackController _playerAttackController;
    public float Speed = 2f;
    private bool _isRunning = false;
    private const float _runInputOffset = 0.1f;
    private float _playerPositionZ= 0;
    private void Awake()
    {
        _playerPositionZ = transform.position.y;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerAttackController = GetComponent<PlayerAttackController>();
    }
    void Start()
    {   
        transform.position = new Vector3(transform.position.x, transform.position.y ,_playerPositionZ);
    }

    void Movement()
    {
        if (Input.GetButton("Horizontal")||Input.GetButton("Vertical"))
        {
            
            if (_playerAttackController.IsAttacking == false)
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                Vector3 _moveDir = new Vector3(h, v, v);
                transform.Translate(Time.deltaTime * Speed * _moveDir.normalized);
                _animator.SetBool("IsMove", true);
                _animator.SetFloat("Speed", Speed);
                if (h < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (h>0) 
                { 
                    transform.localScale = new Vector3(1, 1, 1); 
                }
            }
        }
        else
        {
            _animator.SetBool("IsMove", false);
        }
    }
    void Update()
    {
        Movement();
    }
}
