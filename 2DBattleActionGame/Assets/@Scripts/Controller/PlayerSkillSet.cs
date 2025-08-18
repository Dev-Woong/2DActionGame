using UnityEngine;

public class PlayerSkillSet : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private AttackData _upperSlashAttackData;
    [SerializeField] private ObjectStatus _objectStatus;
    private float _upperSlashCoolTime;
    void Awake()
    {
        _animator = GetComponent<Animator>();   
        _objectStatus = GetComponent<ObjectStatus>();
        _rigidbody = GetComponent<Rigidbody2D>();   
    }
    public void UpperSlash()
    {
        _upperSlashCoolTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Z)&&_upperSlashCoolTime<=0)
        {
            _animator.SetTrigger("UpperSlash");
            _rigidbody.linearVelocity = Vector3.zero;   
            _objectStatus.IsAttacking = true;
            _upperSlashCoolTime = _upperSlashAttackData.CoolTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpperSlash();   
    }
}
