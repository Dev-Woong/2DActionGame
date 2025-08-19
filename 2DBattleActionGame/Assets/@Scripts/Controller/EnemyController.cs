using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : DamageAbleBase, IDamageAble
{
    public int CurHp = 100;
    private readonly WaitForSeconds _interval = new(0.04f);
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
            Debug.Log("Ÿ�� Ž��");
            if (other.GetComponent<ProjectileController>().TargetLayer == _layerMask)
            {
                Debug.Log("Ÿ�� �浹���� ��ġ");
                if (other.GetComponent<ProjectileController>().HitCount > 0)
                {
                    Debug.Log("�ڷ�ƾ ����");
                    _coProjectileDamagedProcess = StartCoroutine(DamagedProjectile(other.gameObject));
                }
                else
                {
                    Debug.LogWarning($"{other.gameObject.name}�� ��Ʈī��Ʈ ���� �缳�����ּ���");
                }
            }
        }
    }
    IEnumerator DamagedProjectile(GameObject other)
    {
        int hitCount = other.GetComponent<ProjectileController>().HitCount;
        int curHitCount = 0;
        yield return null;
        Vector2 otherPosition = other.transform.position;
        while (curHitCount<hitCount)
        {
            if (GetComponent<ObjectStatus>().OnKnockBack == true && GetComponent<ObjectStatus>().OnSuperArmor == false) // ���� �˹� ���μ���
            {
                float xKnockbackForce =other.GetComponent<ProjectileController>().KnockbackForce.x;
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
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
