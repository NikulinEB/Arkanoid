using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events
{
    public static event Action<MenuType> ShowMenu;
    public static void ShowMenu_Call(MenuType menuType) { ShowMenu?.Invoke(menuType); }

    public static event Action<float> Swipe;
    public static void Swipe_Call(float delta) { Swipe?.Invoke(delta); }

    public static event Action SwipeEnd;
    public static void SwipeEnd_Call() { SwipeEnd?.Invoke(); }

    public static event Action<Block> BlockDestroyed;
    public static void BlockDestroyed_Call(Block blockType) { BlockDestroyed?.Invoke(blockType); }

    public static event Action StartLevel;
    public static void StartLevel_Call() { StartLevel?.Invoke(); }

    public static event Action GameOver;
    public static void GameOver_Call() { GameOver?.Invoke(); }

    public static event Action<int> TimerTic;
    public static void TimerTic_Call(int seconds) { TimerTic?.Invoke(seconds); }

    public static event Action<int> ScoreChanged;
    public static void ScoreChanged_Call(int score) { ScoreChanged?.Invoke(score); }
}
