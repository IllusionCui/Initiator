﻿using UnityEngine;
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

	public float BaseSpeed {
		get;
		set;
	}

	public Vector3 Speed {
		get { 
			return  Dir * BaseSpeed;
		}
	}

	public StartLine StartLine {
		get;
		private set;
	}

	public EndLine EndLine {
		get;
		private set;
	}

	public void Init(LevelData ld, GameStatus status) {
		_items.Clear ();
		TransformUtil.RemoveAllChildren (transform);

		for(int i = 0; i < ld.items.Count; i++) {
			AddItem (ld.items [i]);
		}

		if (status == GameStatus.Play) {
			// startline
			RectTransform startLine = ResouceManager.Curr.CreateGameObject ("StartLine").transform as RectTransform;
			startLine.SetParent (this.transform);
			startLine.localScale = Vector3.one;
			startLine.localPosition = Vector3.zero;
			StartLine = startLine.GetComponent<StartLine> ();

			// endline
			RectTransform endLine = ResouceManager.Curr.CreateGameObject ("EndLine").transform as RectTransform;
			endLine.SetParent (this.transform);
			endLine.localScale = Vector3.one;
			endLine.localPosition = new Vector3 (0, 4800, 0);
			EndLine = endLine.GetComponent<EndLine> ();
		} else {
			StartLine = null;
			EndLine = null;
		}
	}

	public bool AddItem(Vector3 pos) {
		for(int i = 0; i < _items.Count; i++) {
			Rect rect = new Rect (new Vector2(_items [i].localPosition.x + _items[i].rect.x, _items [i].localPosition.y + _items[i].rect.y), new Vector2(_items[i].rect.width, _items[i].rect.height));
			if (rect.Contains(pos)) {
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
