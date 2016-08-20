using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MTUnity.UI {
	[RequireComponent (typeof (MenuItemView))]
	public class MenuItemViewImageHelper : MonoBehaviour {
		public Sprite normalSprite;
		public Sprite selectSprite;
		public Image image;

		void Start () {
			Debug.Assert (image != null, "[MenuItemViewImageHelper Start]  image = null, name = " + gameObject.name);

			MenuItemView menuItemView = GetComponent<MenuItemView> ();
			menuItemView.meunItemViewChangedDelegate += OnMeunItemViewChanged;
			UpdateView (menuItemView);
		}

		void OnMeunItemViewChanged (MenuItemView menuItemView) {
			UpdateView (menuItemView);
		}

		void UpdateView(MenuItemView menuItemView) {
			if (menuItemView == null) {
				Debug.LogWarning ("[MenuItemViewImageHelper UpdateView]  menuItemView = null, name = " + gameObject.name);
				return;
			}

			if (menuItemView.menuItem.Selected && selectSprite != null) {
				image.sprite = selectSprite;
			} else {
				image.sprite = normalSprite;
			}
		}
	}
}
