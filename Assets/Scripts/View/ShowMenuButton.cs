using UnityEngine;
using System.Collections;

public class ShowMenuButton : MonoBehaviour
{
    [SerializeField]
    protected MenuType menuType;

    public virtual void ShowMenu() {
        Events.ShowMenu_Call(menuType);
    }

}
