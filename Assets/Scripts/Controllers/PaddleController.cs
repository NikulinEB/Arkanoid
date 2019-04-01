using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PaddleController : MonoBehaviour
{
    [SerializeField]
    private float _forceMultiplier;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Events.Swipe += Shift;
        Events.StartLevel += StopShifting;
        Events.StartLevel += ResetPosition;
    }

    private void OnDestroy()
    {
        Events.Swipe -= Shift;
        Events.StartLevel -= StopShifting;
        Events.StartLevel -= ResetPosition;
    }

    private void Shift(float force)
    {
        _rigidbody.AddForce(force * _forceMultiplier * new Vector2(1, 0), ForceMode2D.Impulse);
    }

    private void StopShifting()
    {
        _rigidbody.velocity = new Vector2(0, 0);
    }

    private void ResetPosition()
    {
        transform.position = Vector3.zero;
    }
}
