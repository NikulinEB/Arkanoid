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
    private BlocksController _blocksController;

    private void OnEnable()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Ball"))
        {
            HitCount--;
            TryChangeColor();
        }
    }

    public void SetController(BlocksController blocksController)
    {
        _blocksController = blocksController;
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

    public void TryChangeColor()
    {
        if (_blocksController.BlockTypes.ContainsKey(HitCount))
        {
            SetBlock(_blocksController.BlockTypes[HitCount]);
        }
    }
}
