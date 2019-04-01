using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class BottomCollisions : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Ball") || collision.transform.tag.Equals("Block"))
        {
            Events.GameOver_Call();
        }
    }
}
