using System.Runtime.InteropServices;
using UnityEngine;

public class WeaponMaster : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private ObjectStatus _objectStatus;
    [SerializeField] private Animator _animator;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _objectStatus = GetComponent<ObjectStatus>();
    }
    void Update()
    {
        IllusionSlash();
    }
    #region Illusion_Slash
    [Header("환영검무")]
    [SerializeField] private AttackData _illusionSlashData;
    [SerializeField] private float _illusionSlashCoolTime;
    public void IllusionSlash()
    {
        _illusionSlashCoolTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.W)&&_illusionSlashCoolTime<=0)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _objectStatus.CanMove = false;
            _objectStatus.IsAttacking = true;
            _animator.SetTrigger("IllusionSlash");
            _illusionSlashCoolTime = _illusionSlashData.CoolTime;   
        }
    }
    #endregion
}
