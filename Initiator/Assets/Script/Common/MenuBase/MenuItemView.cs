using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MTUnity.UI {
	[System.Serializable]
	public class MenuItem {
		public string name;
		public Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();

		public delegate void MenuItemChanged(MenuItem menuItem);
		public MenuItemChanged meunItemChangedDelegate;

		private bool _selected = false;
		public bool Selected {
			set { 
				_selected = value;
				InvokeMeunItemChangedDelegate ();
			}
			get { return _selected; }
		}

		private void InvokeMeunItemChangedDelegate () {
			if (meunItemChangedDelegate != null) {
				meunItemChangedDelegate.Invoke (this);
			}
		}

		public MenuItem(string name_) {
			name = name_;
		}

		public static List<MenuItem> CreateMenuItems(List<string> names) {
			List<MenuItem> res = new List<MenuItem> ();
			for(int i = 0; i < names.Count; i++) {
				res.Add (new MenuItem (names [i]));
			}
			return res;
		}

		public static List<MenuItem> CreateMenuItems(string[] names) {
			List<MenuItem> res = new List<MenuItem> ();
			for(int i = 0; i < names.Length; i++) {
				res.Add (new MenuItem (names [i]));
			}
			return res;
		}
	}

	public class MenuItemView : MonoBehaviour {
		[Header("View")]
		[Tooltip("menuItemView button, defalut the Button component of the child named 'Button'")]
		public Button button;
		[Header("Data")]
		public MenuItem menuItem;
		[Header("menuController")]
		public MenuController menuController;

		public delegate void MenuItemViewChanged(MenuItemView menuItemView);
		public MenuItemViewChanged meunItemViewChangedDelegate;

		void Awake() {
			if (button == null) {
				for(int i = 0; i < transform.childCount; i++) {
					Transform child = transform.GetChild (i);
					if (child.name == "Button") {
						button = child.gameObject.GetComponent<Button>();
						break;
					}
				}
			}

			Debug.Assert (button != null, "MenuItem button == null: gameObjectName = " + gameObject.name);

			if (menuItem != null) {
				SetMenuItem (menuItem);
			}
		}

		private void InvokeMeunItemViewChangedDelegate () {
			if (meunItemViewChangedDelegate != null) {
				meunItemViewChangedDelegate.Invoke (this);
			}
		}

		public void DefaultInitTextHelper() {
			MenuItemViewTextHelper menuItemViewTextHelper = gameObject.GetComponent<MenuItemViewTextHelper> ();
			if (menuItemViewTextHelper != null && menuItem != null) {
				menuItemViewTextHelper.normalText = menuItem.name;
			}
		}

		public void SetMenuItem(MenuItem item) {
			Debug.Assert (item != null, "MenuItem item == null: gameObjectName = " + gameObject.name);

			if (menuItem != null) {
				menuItem.meunItemChangedDelegate -= OnMeunItemChanged;
			}
			menuItem = item;
			menuItem.meunItemChangedDelegate = OnMeunItemChanged;

			gameObject.name = menuItem.name;
			button.onClick = menuItem.buttonClickedEvent;

			InvokeMeunItemViewChangedDelegate ();
		}

		void OnMeunItemChanged(MenuItem item) {
			if (item == null || menuItem != item) {
				return;
			}

			InvokeMeunItemViewChangedDelegate ();
		}
	}
}
