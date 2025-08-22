using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class WeaponMaster : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private ObjectStatus _objectStatus;
    [SerializeField] private PlayerAttackController _playerAttackController;
    [SerializeField] private Animator _animator;
    [SerializeField] private AttackData[] _weaponMasterAttackData;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _objectStatus = GetComponent<ObjectStatus>();
        _playerAttackController = GetComponent<PlayerAttackController>();
        _weaponMasterAttackData = Resources.LoadAll<AttackData>("AttackData/Player/WeaponMaster");
    }
    void Update()
    {
        IllusionSlash();
    }
    
    
    
    private AttackData FindAttackData(int id)
    {
        foreach (AttackData attackData in _weaponMasterAttackData)
        {
            if (attackData.SkillID == id)
            {
                return attackData;
            }
            
        }
        Debug.LogWarning("id�� ������ �߸��ϼ̰ų� �ش� attackData�� �߸��� ��ο� ���� �� �ֽ��ϴ�.");
        return null;
    }
    #region Illusion_Slash
    [Header("ȯ���˹�")]
    [SerializeField] private float _illusionSlashCurTime;
    public void IllusionSlash()
    {
        _illusionSlashCurTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.W)&&_illusionSlashCurTime<=0&&_playerAttackController.useSkill == false)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _objectStatus.CanMove = false;
            _playerAttackController.useSkill = true;
            _objectStatus.IsAttacking = true;
            _animator.SetTrigger("IllusionSlash");
            _illusionSlashCurTime =FindAttackData(1).CoolTime;   
        }
    }
    #endregion
}
