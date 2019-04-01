using UnityEngine;
using System.Collections;

public class NextLevelButton : ShowMenuButton
{
    public override void ShowMenu()
    {
        Events.NextLevel_Call();
        base.ShowMenu();
    }
}
