using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class SkillEffectMoving : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    public float MoveForce;
    private Vector3 _nativeScale;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _nativeScale = transform.localScale;
    }
    private void OnDisable()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        transform.localScale= _nativeScale;
    }
}
