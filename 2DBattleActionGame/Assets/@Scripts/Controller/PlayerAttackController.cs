using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAttackController : MonoBehaviour
{
    #region Component
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] PlayerMovement _playerMovement;
    public int NormalAtkCount=0;
    public float MoveForceValue = 5;
    public bool IsAttacking = false;
    #endregion
    #region NormalAttack
    public void NormalAttack()
    {
        // 일반공격 트리거명: NormalAtk
        if (Input.GetKeyDown(KeyCode.X))
        {
            IsAttacking = true;
            _rigidbody.linearVelocity = Vector3.zero;
            if (Input.GetButton("Horizontal"))
            {
                float h = Input.GetAxisRaw("Horizontal");
                Vector3 forceDir = new Vector3 (h, 0, 0);
                if (h !=0) 
                {
                    _rigidbody.AddForce(Time.fixedDeltaTime*MoveForceValue*forceDir, ForceMode2D.Impulse);
                }
            }
            _animator.SetBool("IsMove", false);
            _animator.SetTrigger("NormalAtk");
        }
    }
    public void NormalAttackFinish() // Animation Event
    {
        IsAttacking = false;
    }
    #endregion
    #region LifeCycle
    void Awake()
    {       
        _rigidbody = GetComponent<Rigidbody2D>();   
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        NormalAttack();
    }
    #endregion
}
