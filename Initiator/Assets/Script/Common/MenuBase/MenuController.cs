using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MTUnity.UI {
	public interface MenuControllerListener {
		void OnMenuItemViewClick(MenuController menuController, MenuItemView menuItemView);
	}

	public enum MenuControllerSelectType {
		None,
		OnlyOne,
		Multiple,
		MustOne
	}

	public class MenuController : MonoBehaviour {
		[Header("View")]
		[Tooltip("ScrollRectHelper")]
		public ScrollRectHelper scrollRectHelper;
		[Header("MenuItemViews")]
		[Tooltip("Common MenuItemView Perfab")]
		public MenuItemView commonMenuItemViewPerfab;
		[Header("Init Info")]
		[Tooltip("Init Common MenuItemView Datas without other clickListener")]
		public List<string> initCommonMenuItemNames;
		[Tooltip("Init Common MenuItemView Datas with other clickListener")]
		public List<MenuItem> initCommonMenuItems;
		[Tooltip("Init Special MenuItemViews")]
		public List<MenuItemView> initSpecialMenuItemViews;
		[Tooltip("Init Can selected multiple menu items")]
		public MenuControllerSelectType initMenuControllerSelectType;

		// data
		protected MenuControllerSelectType _menuControllerSelectType;
		protected HashSet<MenuItemView> _currSelectItemViews = new HashSet<MenuItemView>();
		protected HashSet<MenuItemView> _menuItemViews = new HashSet<MenuItemView>();

		// listener
		protected HashSet<MenuControllerListener> _listeners = new HashSet<MenuControllerListener>();

		protected virtual void Awake () {
			List<MenuItem> menuItems = MenuItem.CreateMenuItems(initCommonMenuItemNames);
			menuItems.AddRange (initCommonMenuItems);
			List<MenuItemView> menuItemViews = CreateCommonMenuItemViews(menuItems);
			menuItemViews.AddRange (initSpecialMenuItemViews);
			SetMenuItemViews (menuItemViews, initMenuControllerSelectType);
		}

		public List<MenuItemView> CreateCommonMenuItemViews(List<MenuItem> menuItems) {
			List<MenuItemView> res = new List<MenuItemView> ();
			if (menuItems != null) {
				for(int i = 0; i < menuItems.Count; i++) {
					MenuItemView item = Instantiate(commonMenuItemViewPerfab, Vector3.zero, Quaternion.identity) as MenuItemView;
					item.SetMenuItem (menuItems[i]);
					res.Add (item);

					item.DefaultInitTextHelper ();
				}
			}
			return res;
		}

		// update menu items
		public void SetMenuItemViews(List<MenuItemView> menuItemViews, MenuControllerSelectType menuControllerSelectType, int pageItemNum = 1) {
			_menuControllerSelectType = menuControllerSelectType;
			_menuItemViews.Clear ();
			_currSelectItemViews.Clear ();

			List<RectTransform> scrollItems = new List<RectTransform> ();
			for(int i = 0; i < menuItemViews.Count; i++) {
				MenuItemView item = menuItemViews[i];
				if (item.menuItem.Selected) {
					if (_menuControllerSelectType == MenuControllerSelectType.Multiple) {
						_currSelectItemViews.Add (item);
					} else if (_menuControllerSelectType == MenuControllerSelectType.OnlyOne || _menuControllerSelectType == MenuControllerSelectType.MustOne) {
						if (_currSelectItemViews.Count == 0) {
							_currSelectItemViews.Add (item);
						} else {
							item.menuItem.Selected = false;
						}
					} else {
						item.menuItem.Selected = false;
					}
				}
				item.menuItem.buttonClickedEvent.AddListener (delegate { OnMenuItemViewClick (item); });
				_menuItemViews.Add (item);
				scrollItems.Add ((RectTransform)item.transform);
			}

			Debug.Assert (scrollRectHelper != null, "scorllRectHelper == null: name = " + gameObject.name);
			scrollRectHelper.SetContent (scrollItems, pageItemNum);

			if (_menuControllerSelectType == MenuControllerSelectType.MustOne && _currSelectItemViews.Count == 0 && menuItemViews.Count > 0) {
				menuItemViews [0].menuItem.buttonClickedEvent.Invoke ();
			}
		}

		public bool AddMenuControllerListener(MenuControllerListener listener) {
			return _listeners.Add (listener);
		}

		public bool OnMenuItemViewClick(MenuItemView menuItemView) {
			if (!_menuItemViews.Contains(menuItemView)) {
				return false;
			}

			if (_menuControllerSelectType == MenuControllerSelectType.MustOne && _currSelectItemViews.Contains(menuItemView)) {
				return false;
			}

			if (_menuControllerSelectType != MenuControllerSelectType.None) {
				menuItemView.menuItem.Selected = !menuItemView.menuItem.Selected;
				if (_menuControllerSelectType == MenuControllerSelectType.OnlyOne || _menuControllerSelectType == MenuControllerSelectType.MustOne) {
					var currSelectedItemViewEnum = _currSelectItemViews.GetEnumerator ();
					while(currSelectedItemViewEnum.MoveNext()) {
						currSelectedItemViewEnum.Current.menuItem.Selected = false;
					}
					_currSelectItemViews.Clear ();
				} else {
					_currSelectItemViews.Remove (menuItemView);
				}
				if (menuItemView.menuItem.Selected) {
					_currSelectItemViews.Add (menuItemView);
				}
			}

			var listenerEnum = _listeners.GetEnumerator ();
			while(listenerEnum.MoveNext()) {
				listenerEnum.Current.OnMenuItemViewClick (this, menuItemView);
			}

			return true;
		}
	}
}
