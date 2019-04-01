using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlocksController : MonoBehaviour
{
    public int Score { get; private set; }
    [SerializeField]
    private int _pointsForHit;
    [SerializeField]
    private Vector3 _startPosition;
    private GameObject _blockPrefab;
    [SerializeField]
    private float _blocksInterval;
    private float _blockWidth;
    private float _blockHeight;
    [SerializeField]
    private Block[] _blocks;
    [SerializeField]
    private float _secondsBeforeLowering;
    private Dictionary<int, Block> _blockTypes = new Dictionary<int, Block>();
    private int[,] _blocksTest = { { 1, -1, 1, 2, 1, 3, 4 }, { 1, 1, 1, 1, 1, 2, 2}, {1, 1, 1,1, 1, 3, 2 }, {1, 1, 1, 1, 1, 3, 4 }, { 1, 1, 1, 1, 1,1, 1 } };
    private Coroutine _loweringTimer;

    private void Start()
    {
        FillBlocksDict(_blocks);
        LoadPrefab();
        Events.BlockDestroyed += BlockDestroyed;
        Events.GameOver += GameOver;
        Events.StartLevel += StartLevel;
    }

    private void OnDestroy()
    {
        Events.BlockDestroyed -= BlockDestroyed;
        Events.GameOver -= GameOver;
        Events.StartLevel -= StartLevel;
    }

    private void StartLevel()
    {
        RemoveBlock();
        CreateLevel(_blocksTest);
        _loweringTimer = StartCoroutine(LoweringTimer(_secondsBeforeLowering));
    }

    private void FillBlocksDict(Block[] blocks)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            try
            {
                _blockTypes.Add(blocks[i].HitCount, blocks[i]);
            }
            catch (Exception ex)
            {
                Debug.Log("An error while filling blocks' dictionary: " + ex.Message);
            }
        }
    }

    private void LoadPrefab()
    {
        _blockPrefab = Resources.Load<GameObject>("Block");
        var renderer = _blockPrefab.GetComponent<SpriteRenderer>();
        _blockWidth = renderer.bounds.size.x;
        _blockHeight = renderer.bounds.size.y;
    }

    private void RemoveBlock()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void CreateLevel(int[,] blockTypes)
    {
        for (int row = 0; row < blockTypes.GetLength(0); row++)
        {
            for (int column = 0; column < blockTypes.GetLength(1); column++)
            {
                CreateBlock(_blockTypes[blockTypes[row,column]], new Vector3( _startPosition.x + _blockWidth * column, _startPosition.y + _blockHeight * row, 0));
            }
        }
    }
    
    private BlockObject CreateBlock(Block block, Vector3 position)
    {
        var blockObject = Instantiate(_blockPrefab, position, Quaternion.identity, transform).GetComponent<BlockObject>();
        blockObject.SetBlock(block);
        return blockObject;
    }

    private void BlockDestroyed(Block blockType)
    {
        Score += _pointsForHit * blockType.HitCount;
        Events.ScoreChanged_Call(Score);
        if (IsGameWin())
        {
            Events.ShowMenu_Call(MenuType.Win);
        }
    }

    private bool IsGameWin()
    {
        if (transform.childCount == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GameOver()
    {
        StopCoroutine(_loweringTimer);
        Events.ShowMenu_Call(MenuType.Defeat);
    }

    private IEnumerator LoweringTimer(float interval)
    {
        float timer;
        while (true)
        {
            timer = interval;
            while (timer > 0)
            {
                Events.TimerTic_Call(Mathf.RoundToInt(timer));
                yield return new WaitForSeconds(1);
                timer -= 1;
            }
            BroadcastMessage("Move", new Vector3(0, -_blockHeight, 0));
        }
    }
}