using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAttackController : MonoBehaviour
{
    #region Component
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] ObjectStatus _objectStatus;

    public int NormalAtkCount=0;
    public bool InputRightArrow = false;
    public bool InputLeftArrow = false;
    #endregion
    #region NormalAttack
    public void NormalAttack()
    {
        // 일반공격 트리거명: NormalAtk
        if (Input.GetKeyDown(KeyCode.X) && _objectStatus.IsAttacking == false)
        {
            _objectStatus.IsAttacking = true;
            _objectStatus.CanMove = false;
            _animator.SetBool("IsMove", false);
            _animator.SetTrigger("NormalAtk");
        }
        InputRightArrow = Input.GetKey(KeyCode.RightArrow);
        InputLeftArrow = Input.GetKey(KeyCode.LeftArrow);
    }
    public void CanInputAttack() // Animation Event
    {
        _objectStatus.IsAttacking = false;
    }
    public void CanMove() // Animation Event
    {
        _objectStatus.CanMove=true;
    }
    public void VelocityZero()// Animation Event
    {
        _rigidbody.linearVelocity = Vector3.zero;
    }
    public void OnAttackMove(float xForce) // Animation Event
    {
        Debug.Log("어택무브호출 성공");
        if (transform.localScale.x == 1)
        {
            if (InputRightArrow == true)
            {
                _rigidbody.AddForce(Vector3.right * 2.5f , ForceMode2D.Impulse);
                
            }
            else if (InputLeftArrow == true)
            {
                _rigidbody.linearVelocity = Vector3.zero ;
            }
            else
            {
                _rigidbody.AddForce(Vector3.right, ForceMode2D.Impulse);
            }
        }
        if (transform.localScale.x == -1)
        {
            if (InputLeftArrow == true)
            {
                _rigidbody.AddForce(Vector3.left* 2.5f, ForceMode2D.Impulse);
            }
            else if (InputRightArrow == true)
            {
                _rigidbody.linearVelocity = Vector3.zero;
            }
            else
            {
                _rigidbody.AddForce(Vector3.left, ForceMode2D.Impulse);
            }
        }
    }
    #endregion
    #region LifeCycle
    void Awake()
    {       
        _rigidbody = GetComponent<Rigidbody2D>();   
        _animator = GetComponent<Animator>();
        _objectStatus = GetComponent<ObjectStatus>();
    }
    void Update()
    {
        NormalAttack();
    }
    #endregion
}
