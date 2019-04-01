using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class BlockObject : MonoBehaviour
{
    private Block _blockType;
    private int _hitCount;
    public int HitCount {
        get
        {
            return _hitCount;
        }
        private set
        {
            if (value == 0)
            {
                Destroy(gameObject);
                Events.BlockDestroyed_Call(_blockType);
            }
            else
            {
                _hitCount = value;
            }
        }
    }
    private SpriteRenderer _renderer;

    private void OnEnable()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Ball"))
        {
            HitCount--;
        }
    }

    public void SetBlock(Block block)
    {
        _blockType = block;
        _renderer.sprite = block.Sprite;
        HitCount = block.HitCount;
    }

    public void Move(Vector3 delta)
    {
        transform.Translate(delta);
    }
}
