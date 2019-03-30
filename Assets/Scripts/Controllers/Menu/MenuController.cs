using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    private Dictionary<MenuType, Menu> _menuDictionary = new Dictionary<MenuType, Menu>();
    private MenuType _currentMenuType;
    private MenuType _previousMenuType;

	// Use this for initialization
	void Start () {
        InitMenu();
        Events.ShowMenu += ShowMenu;
        Events.ShowMenu_Call(MenuType.Start);
    }

    private void OnDestroy()
    {
        Events.ShowMenu -= ShowMenu;
    }

    private void InitMenu() {
        Menu[] menuArray = GetComponentsInChildren<Menu>(false);
        for (int i = 0; i < menuArray.Length; i++) {
            _menuDictionary.Add(menuArray[i].menuType, menuArray[i]);
        }
    }

    private void ShowMenu(MenuType newMenu) {
        _previousMenuType = _currentMenuType;
        _menuDictionary[_currentMenuType].Hide(() => {
            _currentMenuType = newMenu;
            _menuDictionary[newMenu].Show();
        });
    }
}
