using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MTUnity.Utils;

public class Map : MonoBehaviour {
	private List<RectTransform> _items = new List<RectTransform> ();

	private Vector3 _dir = Vector3.zero;
	public Vector3 Dir {
		get { return _dir; }
		set { _dir = value; }
	}

	public void Init(LevelData ld, GameStatus status) {
		TransformUtil.RemoveAllChildren (transform);

		for(int i = 0; i < ld.items.Count; i++) {
			AddItem (ld.items [i]);
		}

		if (status == GameStatus.Play) {
			RectTransform endLine = ResouceManager.Curr.CreateGameObject("EndLine").transform as RectTransform;
			endLine.SetParent (this.transform);
			endLine.localScale = Vector3.one;
			endLine.localPosition = new Vector3(endLine.localPosition.x, 4700, 0);
		}
	}

	public bool AddItem(Vector3 pos) {
		for(int i = 0; i < _items.Count; i++) {
			Rect rect = new Rect (new Vector2(_items [i].localPosition.x + _items[i].rect.x, _items [i].localPosition.y + _items[i].rect.y), new Vector2(_items[i].rect.width, _items[i].rect.height));
			Rect rect2 = new Rect (new Vector2(pos.x + _items[i].rect.x, pos.y + _items[i].rect.y), new Vector2(_items[i].rect.width, _items[i].rect.height));
//			Debug.Log ("i = " + i + ", rect2 = " + rect2 + ", rect = " + rect);
			if (rect.Overlaps(rect2)) {
				return false;
			}
		}

		RectTransform item = ResouceManager.Curr.CreateGameObject(EditeOperationType.Rect.ToString()).transform as RectTransform;
		item.SetParent (this.transform);
		item.localScale = Vector3.one;
		item.localPosition = new Vector3(pos.x, pos.y, 0);
		_items.Add (item);
		return true;
	}

	public void RemoveItem(Vector3 pos) {
		for(int i = _items.Count - 1; i >= 0; i--) {
			Rect rect = new Rect (new Vector2(_items [i].localPosition.x + _items[i].rect.x, _items [i].localPosition.y + _items[i].rect.y), new Vector2(_items[i].rect.width, _items[i].rect.height));
			if (rect.Contains(pos)) {
				Destroy (_items[i].gameObject);
				_items.RemoveAt (i);
			}
		}
	}

	public void UpdateLevelData(LevelData levelData) {
		levelData.items.Clear ();
		for(int i = 0; i < _items.Count; i++) {
			levelData.items.Add (_items[i].localPosition);
		}
	}
}
