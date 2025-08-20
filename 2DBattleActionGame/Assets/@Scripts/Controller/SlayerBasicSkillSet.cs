using Mono.Cecil.Cil;
using TreeEditor;
using UnityEngine;

public class SlayerBasicSkillSet : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private ObjectStatus _objectStatus;
    [SerializeField] private bool InputRightArrow;
    [SerializeField] private bool InputLeftArrow;

    void Awake()
    {
        _animator = GetComponent<Animator>();   
        _objectStatus = GetComponent<ObjectStatus>();
        _rigidbody = GetComponent<Rigidbody2D>();   
    }
    private void InputBool()
    {
        InputRightArrow = Input.GetKey(KeyCode.RightArrow);
        InputLeftArrow = Input.GetKey(KeyCode.LeftArrow);   
    }
    public void SkillMove(AttackData attackData)
    {
        float forceX = attackData.MoveForceX;
        float forceY = attackData.MoveForceY;
        string effectPrefabName = attackData.MoveEffectPrefabName;
        var moveEffect = ObjectPoolManager.instance.GetObject(effectPrefabName);

        if (transform.localScale.x == 1)
        {
            if (InputLeftArrow == true)
            {
                forceX *= -1;
                moveEffect.transform.position = new Vector3(transform.position.x - attackData.MoveEffectPos.x, transform.position.y + attackData.MoveEffectPos.y, 0);
                moveEffect.transform.localScale *= -1;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            _rigidbody.linearVelocity = new Vector2(forceX,forceY);
            moveEffect.transform.position = new Vector3(transform.position.x + attackData.MoveEffectPos.x, transform.position.y + attackData.MoveEffectPos.y, 0);
        }
        else if (transform.localScale.x == -1)
        {
            
            if (InputRightArrow == true)
            {
                moveEffect.transform.position = new Vector3(transform.position.x + attackData.MoveEffectPos.x, transform.position.y + attackData.MoveEffectPos.y, 0);
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                forceX *= -1;
                moveEffect.transform.position = new Vector3(transform.position.x - attackData.MoveEffectPos.x, transform.position.y + attackData.MoveEffectPos.y, 0);
                moveEffect.transform.localScale *= -1;
            }
            _rigidbody.linearVelocity = new Vector2(forceX, forceY);
        }
        
       
    }
    #region UpperSlash
    [SerializeField] private AttackData _upperSlashAttackData;
    private float _upperSlashCoolTime;
    public void UpperSlash()
    {
        _upperSlashCoolTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Z)&&_upperSlashCoolTime<=0 && _objectStatus.IsAttacking == false)
        {
            _rigidbody.linearVelocity = Vector3.zero;   
            _objectStatus.IsAttacking = true;
            _objectStatus.CanMove = false;
            _animator.SetTrigger("UpperSlash");
            _upperSlashCoolTime = _upperSlashAttackData.CoolTime;
        }
    }

    #endregion
    #region DangongCham
    private int _dangongchamStack;
    private float _dangongchamCoolTime;
    public void DangongCham()
    {
        _dangongchamCoolTime -= Time.deltaTime; 
        if (Input.GetKeyDown(KeyCode.G) && _dangongchamCoolTime <= 0 &&_objectStatus.IsAttacking == false)
        {
            _animator.SetBool("IsMove", false);
            _objectStatus.IsAttacking = true;
            _objectStatus.CanMove = false;
            _animator.SetTrigger("Dangongcham");
            _animator.SetInteger("DangongchamStack",_dangongchamStack);
        }
    }
    public void DangongchamCoolTimeProcess(AttackData dangongcham) // Animation Event
    {
        _dangongchamCoolTime = dangongcham.CoolTime; ;
    }
    public void DangongchamStack(int checkFinish) // Animation event
    {
        if (checkFinish == 1)
        {
            _dangongchamStack = 0;
            _animator.SetInteger("DangongchamStack", _dangongchamStack);
        }
        else
        {
            _dangongchamStack++;
        }
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        InputBool();
        UpperSlash();
        DangongCham();
    }
}
