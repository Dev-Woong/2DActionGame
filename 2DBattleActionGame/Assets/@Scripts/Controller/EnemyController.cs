using UnityEngine;

public class EnemyController : DamageAbleBase, IDamageAble
{
    public int CurHp = 100;
    public override void OnDamage(float damage, WeaponType wType)
    {
        if (DamageAble == true)
        {
            CurHp -= Mathf.RoundToInt(damage);
            Debug.Log($"GetDamage : {Mathf.RoundToInt(damage)} \nobjName : {this.name}\ncurHp : {CurHp}");
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
