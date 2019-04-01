using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class BlocksController : MonoBehaviour
{
    private int _score;
    public int Score {
        get
        {
            return _score;
        }
        private set
        {
            _score = value;
            Events.ScoreChanged_Call(_score);
        }
    }
    public Dictionary<int, Block> BlockTypes { get; private set; } = new Dictionary<int, Block>();
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
    private Coroutine _loweringTimer;
    [SerializeField]
    private float _baseBlockMin = 1.06f;
    [SerializeField]
    private float _baseBlockMax = 1.2f;
    [SerializeField]
    private int _levelNumber = 1;
    private int _blockCounter;

    private void Start()
    {
        FillBlocksDict(_blocks);
        LoadPrefab();
        Events.BlockDestroyed += BlockDestroyed;
        Events.GameOver += GameOver;
        Events.StartGame += StartGame;
        Events.NextLevel += NextLevel;
    }

    private void OnDestroy()
    {
        Events.BlockDestroyed -= BlockDestroyed;
        Events.GameOver -= GameOver;
        Events.StartGame -= StartGame;
        Events.NextLevel -= NextLevel;
    }

    private void NextLevel()
    {
        StopTimer();
        CreateLevel(_levelNumber);
        _loweringTimer = StartCoroutine(LoweringTimer(_secondsBeforeLowering));
    }

    private void StartGame()
    {
        ClearGameState();
        NextLevel();
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
        _blockCounter = 0;
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
                _blockCounter++;
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
        _blockCounter--;
        Score += _pointsForHit * blockType.HitCount;
        if (IsGameWin())
        {
            Events.ShowMenu_Call(MenuType.Win);
            _levelNumber++;
        }
    }

    private bool IsGameWin()
    {
        return _blockCounter == 0;
    }

    private void GameOver()
    {
        _levelNumber = 1;
        Score = 0;
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
            BroadcastMessage("Move", new Vector3(0, -_blockHeight, 0), SendMessageOptions.DontRequireReceiver);
        }
    }

    private void ClearGameState()
    {
        _levelNumber = 1;
        Score = 0;
        RemoveBlocks();
    }

    private void StopTimer()
    {
        if (_loweringTimer != null)
        {
            StopCoroutine(_loweringTimer);
        }
    }
}