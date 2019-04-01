using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

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
    private int _columnsCount = 8;
    [SerializeField]
    private float _secondsBeforeLowering;
    public Dictionary<int, Block> BlockTypes { get; private set; } = new Dictionary<int, Block>();
    private Coroutine _loweringTimer;
    [SerializeField]
    private float _baseBlockMin = 1.06f;
    [SerializeField]
    private float _baseBlockMax = 1.2f;
    [SerializeField]
    private int _levelNumber = 1;

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
        RemoveBlocks();
        CreateLevel(_levelNumber);
        _loweringTimer = StartCoroutine(LoweringTimer(_secondsBeforeLowering));
    }

    private void FillBlocksDict(Block[] blocks)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            try
            {
                BlockTypes.Add(blocks[i].HitCount, blocks[i]);
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

    private void RemoveBlocks()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void CreateLevel(int levelNumber)
    {
        var rows = Mathf.Clamp(levelNumber + 1, 1, 10);
        var minBlock = Mathf.Clamp(Mathf.RoundToInt(Mathf.Pow(_baseBlockMin, levelNumber)), BlockTypes.Keys.Min(), BlockTypes.Keys.Max());
        var maxBlock = Mathf.Clamp(Mathf.RoundToInt(Mathf.Pow(_baseBlockMax, levelNumber)), BlockTypes.Keys.Min(), BlockTypes.Keys.Max());
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < _columnsCount; column++)
            {
                CreateBlock(
                    BlockTypes[UnityEngine.Random.Range(minBlock, maxBlock)],
                    new Vector3(
                        _startPosition.x + _blockWidth * (1 + _blocksInterval) * column,
                       _startPosition.y - _blockHeight * (1 + _blocksInterval) * row,
                        0));
            }
        }
    }
    
    private BlockObject CreateBlock(Block block, Vector3 position)
    {
        var blockObject = Instantiate(_blockPrefab, position, Quaternion.identity, transform).GetComponent<BlockObject>();
        blockObject.SetController(this);
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
        _levelNumber = 1;
        Score = 0;
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