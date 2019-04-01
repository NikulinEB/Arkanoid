using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallRelease : MonoBehaviour
{
    [SerializeField]
    private float _ballSpeed = 3;
    private Rigidbody2D _rigidbody;
    private Vector3 _startPosition;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
        Events.StartLevel += ResetPosition;
        Events.StartLevel += ReleaseBall;
    }

    private void OnDestroy()
    {
        Events.StartLevel -= ResetPosition;
        Events.StartLevel -= ReleaseBall;
    }

    private void ResetPosition()
    {
        transform.position = _startPosition;
    }

    private void ReleaseBall()
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(_ballSpeed * Vector2.up, ForceMode2D.Impulse);
    }
}
