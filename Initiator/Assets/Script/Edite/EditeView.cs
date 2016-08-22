using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MTUnity.UI;
using System;
using MTUnity.Utils;

public class EditeView : MonoBehaviour , MenuControllerListener {
	public MenuController menuController;
	public MenuController modelController;
	public MenuController operationController;
	public RectTransform viewPort;
	public Map map;

	private bool _init = false;
	private Dictionary<GameType, MenuItemView> modelViews = new Dictionary<GameType, MenuItemView>();
	private Dictionary<EditeOperationType, MenuItemView> operationViews = new Dictionary<EditeOperationType, MenuItemView>();

	private LevelInfo _levelInfo;
	private LevelData _levelData;

	private EditeOperationType _operationType;

	private int _fingerId = -1;


	void Init () {
		if (_init) {
			return;
		}
		_init = true;
		InitMenuController ();
		InitModelController ();
		InitOperationController ();
	}

	void InitMenuController() {
		var names = new String[]{"save", "test", "cancel"};
		menuController.SetMenuItemViews (menuController.CreateCommonMenuItemViews(MenuItem.CreateMenuItems(names)), MenuControllerSelectType.None);
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
		_operationType = Config.DEFAULT_EDITE_OPERATION;
		var names = Enum.GetNames (typeof(EditeOperationType));
		List<MenuItemView> menuItemViews = new List<MenuItemView> ();
		for(int i = 0; i < names.Length; i++) {
			EditeOperationType type = (EditeOperationType)Enum.Parse (typeof(EditeOperationType), names [i]);
			MenuItem item = new MenuItem (names [i]);
			item.Selected = type == _operationType;
			MenuItemView itemView = Instantiate(operationController.commonMenuItemViewPerfab, Vector3.zero, Quaternion.identity) as MenuItemView;
			itemView.SetMenuItem (item);
			itemView.GetComponent<MenuItemViewImageHelper> ().normalSprite = ResouceManager.Curr.GetSprite (names [i]);
			menuItemViews.Add (itemView);
			operationViews.Add (type, itemView);
		}
		operationController.SetMenuItemViews (menuItemViews, MenuControllerSelectType.MustOne);
		operationController.AddMenuControllerListener (this);
	}

	void Update() {
		// mouse
		{
			if (_fingerId == -1 && Input.GetMouseButtonDown (0)) {
				_fingerId = 0;
			}
			if (_fingerId == 0) {
				if (Input.GetMouseButtonUp (0)) {
					_fingerId = -1;
				} else {
					Operation (Input.mousePosition);
				}
			}
		}
		// touches
		{
			for (var i = 0; i < Input.touchCount; ++i) {
				Touch touch = Input.GetTouch(i);
				if (_fingerId == -1) {
					if (touch.phase == TouchPhase.Began) {
						_fingerId = 0;
					}
				}
				if (touch.fingerId == _fingerId) {
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
						_fingerId = -1;
					} else {
						Operation (touch.position);
					}
				}
			}
		}
	}

	void Operation(Vector3 screenPos) {
		Vector3 pos = GameManager.Curr.AdjustToDesign (screenPos);
//		Debug.Log ("screenPos = " + screenPos + ", pos = " + pos + ", viewPort.rect = " + viewPort.rect + ", viewPort.localPosition = " + viewPort.localPosition);
		if (!viewPort.rect.Contains(pos)) {
			return;
		}
		pos.y -= map.transform.localPosition.y;
		if (EditeOperationType.Rect == _operationType) {
			map.AddItem (pos);
		} else if (EditeOperationType.Clear == _operationType) {
			map.RemoveItem (pos);
		}
	}

	public void EditeLevel(string key) {
		Init ();
		_fingerId = -1;
		_operationType = Config.DEFAULT_EDITE_OPERATION;

		GameManager gm = GameManager.Curr;
		_levelInfo = gm.LevelManager.GetLevelInfo (key);
		_levelData = gm.LevelManager.GetLevelData (key);

		modelViews[_levelInfo.type].menuItem.buttonClickedEvent.Invoke ();
		operationViews[_operationType].menuItem.buttonClickedEvent.Invoke ();

		map.Init (_levelData, GameStatus.Edite);
	}

	public void OnMenuItemViewClick(MenuController mc, MenuItemView menuItemView) {
//		Debug.Log ("[EditeView OnMenuItemViewClick] menuController = " + mc.name + ", menuItemView = " + menuItemView.name);
		if (mc == menuController) {
			if (menuItemView.name == "save") {
				map.UpdateLevelData (_levelData);

				GameManager gm = GameManager.Curr;
				gm.LevelManager.SaveLevel (_levelInfo, _levelData);
				gm.GotoMain ();
			} else if (menuItemView.name == "test") {
				map.UpdateLevelData (_levelData);

				GameManager.Curr.PlayLevel (_levelInfo.type, _levelData);
			} else if (menuItemView.name == "cancel") {
				GameManager.Curr.GotoMain ();
			}
		} else if (mc == modelController) {
			_levelInfo.type = (GameType)Enum.Parse (typeof(GameType), menuItemView.name);
		} else if (mc == operationController) {
			_operationType = (EditeOperationType)Enum.Parse (typeof(EditeOperationType), menuItemView.name);
		}
	}
}
