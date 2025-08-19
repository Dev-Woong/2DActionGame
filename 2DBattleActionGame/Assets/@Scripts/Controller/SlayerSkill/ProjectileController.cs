using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] AttackData _attackData;
    public PoolAble PoolAble;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _curLifeTime;
    public bool TimeCountEventStart = false;
    public LayerMask TargetLayer;
    public float Damage;
    public int HitCount;
    public Vector2 KnockbackForce;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if (PoolAble == null)
            PoolAble = GetComponent<PoolAble>();
        if (_attackData != null)
        {
            TargetLayer = _attackData.TargetLayer;
            Damage = _attackData.Damage;
            HitCount = _attackData.HitCount;
            KnockbackForce = _attackData.KnockBackForce;
        }

    }
    public void StartLifeTime()
    {
        if (_lifeTime == 0)
            return;
        _curLifeTime -= Time.deltaTime;
        if (_curLifeTime < 0)
        {
            PoolAble.ReleaseObject();
        }
    }
   
    private void Update()
    {
        if (TimeCountEventStart == true)
            StartLifeTime();
    }
    private void OnDisable()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        transform.localScale = Vector3.one;
        TimeCountEventStart = false;
        _curLifeTime = _lifeTime;
    }
}
