using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DamageHandler : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;
    private float _minYPos;
    private float _maxXPos;
    private Vector3 _meleeHitStartPos;
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
        if (attackData.VoiceSFX != null)
            VoiceManager.s_Instance.PlayVoiceSFX(attackData.VoiceSFX);
        if (attackData.AttackEffectPrefabName != "")
        {
            var attackEffect = ObjectPoolManager.instance.GetObject(attackData.AttackEffectPrefabName);
            float moveForce = attackEffect.GetComponent<SkillEffectMoving>().MoveForce;
            if (transform.localScale.x == -1)
            {
                attackEffect.transform.position = new Vector3(transform.position.x - attackData.EffectPos.x, transform.position.y + attackData.EffectPos.y, 0);
                attackEffect.transform.localScale *= -1;
                moveForce *= -1;
            }
            else
            {
                attackEffect.transform.position = new Vector3(transform.position.x + attackData.EffectPos.x, transform.position.y + attackData.EffectPos.y, 0);
            }
            attackEffect.GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveForce, ForceMode2D.Impulse);
        }

        _damagedTargets.Clear();
        _minYPos = _boxCollider2D.bounds.min.y;
        _maxXPos = _boxCollider2D.bounds.center.x;
        _meleeHitStartPos = transform.right*transform.localScale.x + new Vector3(_maxXPos+(attackData.AttackRange.x / 2 * transform.localScale.x) + (attackData.StartAttackPoint.x * transform.localScale.x), _minYPos +(attackData.AttackRange.y/2) + attackData.StartAttackPoint.y, 0);
        
        Collider2D[] hits = Physics2D.OverlapBoxAll(_meleeHitStartPos, attackData.AttackRange, 0, attackData.TargetLayer);
        Debug.DrawLine(_meleeHitStartPos,attackData.AttackRange, Color.red, 2f);
        if (attackData.AttackHitType == AttackHitType.MultiTarget)
        {
            foreach (var hit in hits)
            {
                if (hit.transform.position.z >= transform.position.z + attackData.RangeDownOffsetZ && hit.transform.position.z <= transform.position.z + attackData.RangeUpOffsetZ)
                {
                    if (hit.bounds.min.y >= (_meleeHitStartPos.y - (attackData.AttackRange.y / 2)) && hit.bounds.min.y <= (_meleeHitStartPos.y + (attackData.AttackRange.y / 2)))
                    {
                        IDamageAble dmg = hit.GetComponent<IDamageAble>();
                        if (dmg != null && !_damagedTargets.Contains(dmg))
                        {
                            _damagedTargets.Add(dmg);
                            if (hit.GetComponent<DamageAbleBase>().DamageAble == true)
                            {
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
                if (hits[i].transform.position.z >= transform.position.z + attackData.RangeDownOffsetZ&& hits[i].transform.position.z <= transform.position.z + attackData.RangeUpOffsetZ)
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
    public void CreateRangeAttack(AttackData attackData)
    {
        if (attackData.ProjectilePrefabName == "")
        {
            Debug.LogWarning("투사체 프리팹이 없습니다!");
            return;
        }
        if (attackData.AttackType != AttackType.Range || attackData == null)
            return;
        if (attackData.AttackSFX != null)
            SFXManager.s_Instance.PlaySFX(attackData.AttackSFX);
        if (attackData.VoiceSFX != null)
            VoiceManager.s_Instance.PlayVoiceSFX(attackData.VoiceSFX);
        var rangeAttackObject = ObjectPoolManager.instance.GetObject(attackData.ProjectilePrefabName);
        float moveForce = attackData.ProjectileForce;
        if (transform.localScale.x == -1)
        {
            rangeAttackObject.transform.position = new Vector3(transform.position.x - attackData.StartAttackPoint.x, transform.position.y + attackData.StartAttackPoint.y, transform.position.z);
            rangeAttackObject.transform.localScale *= -1;
            moveForce *= -1;
        }
        else
        {
            rangeAttackObject.transform.position = new Vector3(transform.position.x + attackData.StartAttackPoint.x, transform.position.y + attackData.StartAttackPoint.y, transform.position.z);
        }
        rangeAttackObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveForce, ForceMode2D.Impulse);
        rangeAttackObject.GetComponent<ProjectileController>().TimeCountEventStart=true;
    }
    
    IEnumerator HitDamage(IDamageAble dmg, AttackData attackData, GameObject target)
    {
        if (attackData.HitCount >= 1)
        {
            int currentHits = 0;
            Vector3 effectPos = new Vector3(attackData.EffectPos.x, attackData.EffectPos.y, 0);            
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