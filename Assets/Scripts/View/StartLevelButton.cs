using UnityEngine;
using System.Collections;

public class StartLevelButton : ShowMenuButton
{
    public override void ShowMenu()
    {
        Events.StartLevel_Call();
        base.ShowMenu();
    }
}
