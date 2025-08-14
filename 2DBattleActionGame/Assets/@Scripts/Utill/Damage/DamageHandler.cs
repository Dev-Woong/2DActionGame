using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    // public Transform target;
    private BoxCollider2D _boxCollider2D;
    private float _minYPos;
    private float _maxXPos;
    private Vector3 _hitStartPos;
    private readonly HashSet<IDamageAble> _damagedTargets = new();
    private readonly WaitForSeconds _interval = new(0.04f);
    private Coroutine _coDamageProcess;
    public void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }
    public void CreateMeleeAttackBox(AttackData attackData)
    {
        if (attackData.AttackType != AttackType.Melee || attackData ==null)
            return;
        if (attackData.AttackSFX != null)
            SFXManager.s_Instance.PlaySFX(attackData.AttackSFX);

        _damagedTargets.Clear();
        _minYPos = _boxCollider2D.bounds.min.y;
        _maxXPos = _boxCollider2D.bounds.center.x;
        _hitStartPos = transform.right*transform.localScale.x + new Vector3(_maxXPos+(attackData.AttackRange.x / 2 * transform.localScale.x) + (attackData.StartAttackPoint.x * transform.localScale.x), _minYPos +(attackData.AttackRange.y/2) + attackData.StartAttackPoint.y, 0);
        
        Collider2D[] hits = Physics2D.OverlapBoxAll(_hitStartPos, attackData.AttackRange, 0, attackData.TargetLayer);
        Debug.DrawLine(_hitStartPos,attackData.AttackRange, Color.red, 2f);
        if (attackData.AttackHitType == AttackHitType.MultiTarget)
        {
            foreach (var hit in hits)
            {
                Debug.Log("MultiTargetProcess.1");
                if (hit.transform.position.z >= transform.position.z - attackData.RangeDownOffsetZ && hit.transform.position.z <= transform.position.z + attackData.RangeUpOffsetZ)
                {
                    Debug.Log("MultiTargetProcess.2");
                    if (hit.bounds.min.y >= (_hitStartPos.y - (attackData.AttackRange.y / 2)) && hit.bounds.min.y <= (_hitStartPos.y + (attackData.AttackRange.y / 2)))
                    {
                        Debug.Log("MultiTargetProcess.3");
                        IDamageAble dmg = hit.GetComponent<IDamageAble>();
                        if (dmg != null && !_damagedTargets.Contains(dmg))
                        {
                            _damagedTargets.Add(dmg);
                            if (hit.GetComponent<DamageAbleBase>().DamageAble == true)
                            {
                                Debug.Log("MultiTargetProcess.4");
                                _coDamageProcess = StartCoroutine(HitDamage(dmg, attackData, hit.gameObject));
                            }
                        }
                    }
                }
            }
        }
        if (attackData.AttackHitType == AttackHitType.SingleTarget)
        {
            float minDistance = float.MaxValue;
            float curDistance;
            int curHits = 0;
            if (hits == null)
            {
                return;
            }
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.position.z >= transform.position.z - attackData.RangeDownOffsetZ&& hits[i].transform.position.z <= transform.position.z + attackData.RangeUpOffsetZ)
                {
                    curDistance = transform.position.x - hits[i].transform.position.x;
                    if (curDistance < 0)
                    {
                        curDistance *= -1;
                    }
                    if (curDistance < minDistance)
                    {
                        minDistance = curDistance;
                        curHits = i;
                        IDamageAble dmg = hits[curHits].GetComponent<IDamageAble>();
                        _coDamageProcess = StartCoroutine(HitDamage(dmg, attackData, hits[curHits].gameObject));
                    }
                }
                else { continue; }
            }
            
        }
    }
    IEnumerator HitDamage(IDamageAble dmg, AttackData attackData, GameObject target)
    {
        if (attackData.HitCount >= 1)
        {
            int currentHits = 0;
            Vector3 effectPos = new Vector3(attackData.EffectPos.x, attackData.EffectPos.y, 0);

            if (attackData.AttackEffectPrefabName != "")
            {
                var attackEffect = ObjectPoolManager.instance.GetObject(attackData.AttackEffectPrefabName);
                attackEffect.transform.position = transform.position + attackData.EffectPos;
            }
            else yield return null;
            if (target.GetComponent<ObjectStatus>().OnKnockBack == true && target.GetComponent<ObjectStatus>().OnSuperArmor == false) // 공격 넉백 프로세스
            {
                float xKnockbackForce = attackData.KnockBackForce.x;
                target.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
                if (target.transform.position.x < transform.position.x)
                {
                    xKnockbackForce = -xKnockbackForce;
                }
                target.GetComponent<Rigidbody2D>().AddForce(new Vector3(xKnockbackForce, attackData.KnockBackForce.y, 0), ForceMode2D.Impulse);
            }
            while (currentHits < attackData.HitCount)
            {
                dmg.TakeDamage((attackData.Damage), attackData.WeaponType);
                if (attackData.HitEffectPrefabName != "")
                {
                    if (target.GetComponent<ObjectStatus>().IsDie == false)
                    {
                        var effect = ObjectPoolManager.instance.GetObject(attackData.HitEffectPrefabName);

                        effect.transform.position = target.transform.position + effectPos;
                        if (transform.localScale.x == -1)
                        {
                            effect.GetComponent<SpriteRenderer>().flipX = true;
                        }
                    }
                    else yield return null;
                }
                else yield return null;
                currentHits++;
                yield return _interval;
            }
            yield return new WaitForSeconds(0.4f);
        }
        else
        {
            Debug.LogWarning("공격 데이터의 HitCount를 설정해주세요 , , ,");
        }
    }
}