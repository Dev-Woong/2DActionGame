using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    #region Component
    [SerializeField] Animator _animator;
    public int NormalAtkCount=0;
    public bool IsAttacking = false;
    #endregion
    #region NormalAttack
    public void NormalAttack()
    {
        // 일반공격 트리거명: NormalAtk
        if (Input.GetKeyDown(KeyCode.X))
        {
            IsAttacking = true;
            _animator.SetTrigger("NormalAtk");
        }
    }
    public void NormalAttackFinish() // Animation Event
    {
        IsAttacking = false;
    }
    #endregion
    #region LifeCycle
    void Start()
    {       
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        NormalAttack();
    }
    #endregion
}
