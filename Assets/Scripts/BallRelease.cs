using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallRelease : MonoBehaviour
{
    [SerializeField]
    private float _ballSpeed = 3;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(_ballSpeed * Vector2.down, ForceMode2D.Force);
    }
}
