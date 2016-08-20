using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MTUnity.UI {
	[RequireComponent (typeof (MenuItemView))]
	public class MenuItemViewTextHelper : MonoBehaviour {
		public string normalText;
		public string selectText;
		public Text text;

		void Start () {
			Debug.Assert (text != null, "[MenuItemViewTextHelper Start]  text = null, name = " + gameObject.name);

			MenuItemView menuItemView = GetComponent<MenuItemView> ();
			menuItemView.meunItemViewChangedDelegate += OnMeunItemViewChanged;
			UpdateView (menuItemView);
		}

		void OnMeunItemViewChanged (MenuItemView menuItemView) {
			UpdateView (menuItemView);
		}

		void UpdateView(MenuItemView menuItemView) {
			if (menuItemView == null) {
				Debug.LogWarning ("[MenuItemViewTextHelper UpdateView]  menuItemView = null, name = " + gameObject.name);
				return;
			}

			if (menuItemView.menuItem.Selected && selectText.Length > 0) {
				text.text = selectText;
			} else {
				text.text = normalText;
			}
		}
	}
}
