using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using MTUnity.UI;

public class MainView : MonoBehaviour {
	public ScrollRectHelper scorllRectHelper;
	public MainItemView itemPerfab;


	public void CreateNew() {
		GameManager gm = GameManager.Curr;
		string key = gm.LevelManager.CreateNew ();
		gm.EditeLevel (key);
	}

	void OnEnable() {
		UpdateList ();
	}

	void UpdateList() {
		List<RectTransform> list = new List<RectTransform> ();
		LevelManager lv = GameManager.Curr.LevelManager;
		for(int i = 0; i < lv.keys.Count; i++) {
			MainItemView item = Instantiate (itemPerfab) as MainItemView;
			item.SetLevelInfo (lv.GetLevelInfo(lv.keys[i]));
			list.Add (item.transform as RectTransform);
		}

		scorllRectHelper.SetContent (list);
	}
}
