using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MTUnity.UI;
using System;

public class EditeView : MonoBehaviour , MenuControllerListener {
	public MenuController menuController;
	public MenuController modelController;
	public MenuController operationController;

	private Dictionary<GameType, MenuItemView> modelViews = new Dictionary<GameType, MenuItemView>();
	private Dictionary<EditeOperationType, MenuItemView> operationViews = new Dictionary<EditeOperationType, MenuItemView>();

	private LevelInfo _levelInfo;
	private LevelData _levelData;

	private EditeOperationType _type;

	void Awake () {
		InitMenuController ();
		InitModelController ();
		InitOperationController ();
	}

	void InitMenuController() {
		var names = new String[]{"save", "test", "cancel"};
		menuController.SetMenuItemViews (menuController.CreateCommonMenuItemViews(MenuItem.CreateMenuItems(names)), MenuControllerSelectType.OnlyOne);
		menuController.AddMenuControllerListener (this);
	}

	void InitModelController() {
		var names = Enum.GetNames (typeof(GameType));
		List<MenuItemView> menuItemViews = new List<MenuItemView> ();
		for(int i = 0; i < names.Length; i++) {
			MenuItemView item = Instantiate(modelController.commonMenuItemViewPerfab, Vector3.zero, Quaternion.identity) as MenuItemView;
			item.SetMenuItem (new MenuItem(names[i]));
			item.DefaultInitTextHelper ();
			menuItemViews.Add (item);
			modelViews.Add ((GameType)Enum.Parse (typeof(GameType), names[i]), item);
		}
		modelController.SetMenuItemViews (menuItemViews, MenuControllerSelectType.MustOne);
		modelController.AddMenuControllerListener (this);
	}

	void InitOperationController() {
		var names = Enum.GetNames (typeof(EditeOperationType));
		List<MenuItemView> menuItemViews = new List<MenuItemView> ();
		for(int i = 0; i < names.Length; i++) {
			EditeOperationType type = (EditeOperationType)Enum.Parse (typeof(EditeOperationType), names [i]);
			MenuItem item = new MenuItem (names [i]);
			item.Selected = type == Config.DEFAULT_EDITE_OPERATION;
			MenuItemView itemView = Instantiate(operationController.commonMenuItemViewPerfab, Vector3.zero, Quaternion.identity) as MenuItemView;
			itemView.SetMenuItem (item);
			itemView.GetComponent<MenuItemViewImageHelper> ().normalSprite = ResouceManager.Curr.GetSprite (names [i]);
			itemView.DefaultInitTextHelper ();
			menuItemViews.Add (itemView);
			operationViews.Add (type, itemView);
		}
		operationController.SetMenuItemViews (menuItemViews, MenuControllerSelectType.MustOne);
		operationController.AddMenuControllerListener (this);
	}

	public void EditeLevel(string key) {
		GameManager gm = GameManager.Curr;
		_levelInfo = gm.LevelManager.GetLevelInfo (key);
		_levelData = gm.LevelManager.GetLevelData (key);

		modelViews[_levelInfo.type].menuItem.buttonClickedEvent.Invoke ();
		operationViews[Config.DEFAULT_EDITE_OPERATION].menuItem.buttonClickedEvent.Invoke ();
	}

	public void OnMenuItemViewClick(MenuController mc, MenuItemView menuItemView) {
//		Debug.Log ("[EditeView OnMenuItemViewClick] menuController = " + mc.name + ", menuItemView = " + menuItemView.name);
		if (mc == menuController) {
			if (menuItemView.name == "save") {
				GameManager gm = GameManager.Curr;
				gm.LevelManager.SaveLevel (_levelInfo, _levelData);
				gm.GotoMain ();
			} else if (menuItemView.name == "test") {
				GameManager.Curr.PlayLevel (_levelInfo.type, _levelData);
			} else if (menuItemView.name == "cancel") {
				GameManager.Curr.GotoMain ();
			}
		} else if (mc == modelController) {
			_levelInfo.type = (GameType)Enum.Parse (typeof(GameType), menuItemView.name);
		} else if (mc == operationController) {
			_type = (EditeOperationType)Enum.Parse (typeof(EditeOperationType), menuItemView.name);
		}
	}
}
