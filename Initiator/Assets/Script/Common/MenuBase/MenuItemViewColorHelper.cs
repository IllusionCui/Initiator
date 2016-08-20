using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MTUnity.UI {
	[RequireComponent (typeof (MenuItemView))]
	public class MenuItemViewColorHelper : MonoBehaviour {
		public Color unselectedColor = Color.white;
		public Color selectedColor = Color.black;
		public Image image;

		void Start () {
			Debug.Assert (image != null, "MenuItemViewColorHelper [Start]  image = null, name = " + gameObject.name);

			MenuItemView menuItemView = GetComponent<MenuItemView> ();
			menuItemView.meunItemViewChangedDelegate += OnMeunItemViewChanged;
			UpdateView (menuItemView);
		}

		void OnMeunItemViewChanged (MenuItemView menuItemView) {
			UpdateView (menuItemView);
		}

		void UpdateView(MenuItemView menuItemView) {
			if (menuItemView == null) {
				Debug.LogWarning ("MenuItemViewColorHelper [UpdateView]  menuItemView = null, name = " + gameObject.name);
				return;
			}

			image.color = menuItemView.menuItem.Selected ? selectedColor : unselectedColor;
		}
	}
}
