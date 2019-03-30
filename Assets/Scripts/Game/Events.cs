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
}
