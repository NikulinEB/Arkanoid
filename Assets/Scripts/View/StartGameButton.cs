using UnityEngine;
using System.Collections;

public class StartGameButton : ShowMenuButton
{
    public override void ShowMenu()
    {
        Events.StartGame_Call();
        base.ShowMenu();
    }
}
