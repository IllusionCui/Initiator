using UnityEngine;
using System.Collections.Generic;

public class ResouceManager : MonoBehaviour {
	public Texture2D[] textures;
	public GameObject[] perfabs;

	private Dictionary<string, Sprite> _sprites;
	private Dictionary<string, GameObject> _perfabs;

	private static ResouceManager _curr = null;
	public static ResouceManager Curr {
		get { return _curr; }
	}

	void Awake () {
		if (_curr != null && _curr != this) {
			Destroy (this.gameObject);
			return;
		}
		_curr = this;

		DontDestroyOnLoad (gameObject);

		_sprites = new Dictionary<string, Sprite> ();
		for (int i = 0, n = textures.Length; i < n; i++) {
			Texture2D tex = textures [i];
			if (tex != null) {
				CreateSprite (_sprites, tex);
			}
		}

		_perfabs = new Dictionary<string, GameObject> ();
		for (int i = 0, n = perfabs.Length; i < n; i++) {
			_perfabs.Add (perfabs[i].name, perfabs[i]);
		}
	}

	public Sprite GetSprite (string name, bool clone = false) {
		return GetSprite (_sprites, name, clone);
	}

	public GameObject CreateGameObject (string name) {
		GameObject res = null;
		if (_perfabs.ContainsKey(name)) {
			res = Instantiate (_perfabs[name]);
		}
		return res;
	}

	void CreateSprite (Dictionary<string, Sprite> sprites, Texture2D tex) {
		Rect rect = new Rect (0, 0, tex.width, tex.height);
		Vector2 pivot = new Vector2 (0.5f, 0.5f);
		Sprite spr = Sprite.Create (tex, rect, pivot);
		spr.name = tex.name;

		sprites.Add (spr.name, spr);
	}

	Sprite GetSprite (Dictionary<string, Sprite> sprites, string name, bool clone) {
		Sprite spr = null;
		if (sprites.TryGetValue (name, out spr)) {
			if (clone && spr != null) {
				spr = Sprite.Create (spr.texture, spr.textureRect, spr.pivot);
			}
		}
		return spr;
	}
}
