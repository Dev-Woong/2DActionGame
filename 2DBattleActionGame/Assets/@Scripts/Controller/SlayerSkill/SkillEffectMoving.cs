using System.Collections;
using UnityEngine;

public class SkillEffectMoving : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    public float MoveForce;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnDisable()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        transform.localScale= Vector3.one;
    }
}
