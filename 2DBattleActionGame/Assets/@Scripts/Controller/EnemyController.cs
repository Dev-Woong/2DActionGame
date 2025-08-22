using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : DamageAbleBase, IDamageAble
{
    BoxCollider2D _boxCollider;
    private bool _projectileOnHit = false;
    public int CurHp = 100;
    private readonly WaitForSeconds _interval = new(0.2f);
    private Coroutine _coProjectileDamagedProcess;
    [SerializeField]private LayerMask _layerMask;
    public override void OnDamage(float damage, WeaponType wType)
    {
        if (DamageAble == true)
        {
            CurHp -= Mathf.RoundToInt(damage);
            Debug.Log($"GetDamage : {Mathf.RoundToInt(damage)} \nobjName : {this.name}\ncurHp : {CurHp}");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (other.GetComponent<ProjectileController>().TargetLayer == _layerMask)
            {
                if (other.GetComponent<ProjectileController>().HitCount > 0)
                {
                    _projectileOnHit = true;
                    _coProjectileDamagedProcess = StartCoroutine(DamagedProjectile(other.gameObject));
                }
                else
                {
                    Debug.LogWarning($"{other.gameObject.name}의 히트카운트 수를 재설정해주세요");
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            _projectileOnHit = false;
        }
    }
    IEnumerator DamagedProjectile(GameObject other)
    {
        int hitCount = other.GetComponent<ProjectileController>().HitCount;
        int curHitCount = 0;
        yield return null;
        Vector2 otherPosition = other.transform.position;
        while (curHitCount < hitCount && _projectileOnHit == true)
        {
            if (GetComponent<ObjectStatus>().OnKnockBack == true && GetComponent<ObjectStatus>().OnSuperArmor == false) // 공격 넉백 프로세스
            {
                float xKnockbackForce = other.GetComponent<ProjectileController>().KnockbackForce.x;
                GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;

                if (transform.position.x < otherPosition.x)
                {
                    xKnockbackForce = -xKnockbackForce;
                }
                GetComponent<Rigidbody2D>().AddForce(new Vector3(xKnockbackForce, other.GetComponent<ProjectileController>().KnockbackForce.y, 0), ForceMode2D.Impulse);
            }
            OnDamage(other.GetComponent<ProjectileController>().Damage, WeaponType.Projectile);
            yield return _interval;
            curHitCount++;
        }
        yield return null;  
    }
    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
