using System.Collections;
using UnityEngine;

public class SkillEffectMoving : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] private float _moveForce;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        _rigidbody.AddForce(Vector3.right * _moveForce, ForceMode2D.Impulse);
    }
    private void OnDisable()
    {
        _rigidbody.linearVelocity = Vector3.zero;
    }
}
