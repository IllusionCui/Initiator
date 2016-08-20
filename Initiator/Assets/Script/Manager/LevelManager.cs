using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MTUnity.Utils;
using System;

public class LevelInfo {
	public string key;
	public string name;
	public GameType type;

	public LevelInfo(string key_) {
		key = key_;
		name = key_;
		type = Config.DEFAULT_GAME_TYPE;
	}

	public MTJSONObject Serialize() {
		MTJSONObject res = MTJSONObject.CreateDict ();
		res.Add ("key", key);
		res.Add ("name", name);
		res.Add ("type", type.ToString());
		return res;
	}

	public void Deserialize(MTJSONObject json) {
		key = json ["key"].s;
		name = json ["name"].s;
		type = (GameType)Enum.Parse (typeof(GameType), json ["type"].s);
	}
}

public class LevelData {
	public string key;

	public LevelData(string key_) {
		key = key_;
	}

	public MTJSONObject Serialize() {
		MTJSONObject res = MTJSONObject.CreateDict ();
		res.Add ("key", key);
		return res;
	}

	public void Deserialize(MTJSONObject json) {
		key = json ["key"].s;
	}
}

public class LevelManager {
	public List<string> keys = new List<string>();

	private string _basePath = "Levels";

	public LevelManager() {
		_basePath = Path.Combine (Application.persistentDataPath, _basePath);
		if (!Directory.Exists(_basePath)) {
			Directory.CreateDirectory (_basePath);
		}
		// keys
		var subDirs = Directory.GetDirectories (_basePath);
		for(int i = 0; i < subDirs.Length; i++) {
			keys.Add (new DirectoryInfo (subDirs [i]).Name);
		}
	}

	public string CreateNew() {
		string key = "LV "+ (keys.Count + 1);
		keys.Add (key);

		Directory.CreateDirectory (Path.Combine (_basePath, key));
		SaveLevel (new LevelInfo (key), new LevelData(key));

		return key;
	}

	public LevelInfo GetLevelInfo(string key) {
		LevelInfo res = null;

		if (res == null) {
			res = new LevelInfo (key);
			SaveLevel (res, null);
		}

		return res;
	}

	public LevelData GetLevelData(string key) {
		LevelData res = null;

		if (res == null) {
			res = new LevelData (key);
			SaveLevel (null, res);
		}

		return res;
	}
		
	public void SaveLevel(LevelInfo info, LevelData data) {
		if (info != null) {
			string path = Path.Combine (_basePath, info.key);
			path = Path.Combine (path, "info");
			File.WriteAllText (path, info.Serialize().ToString());
		}

		if (data != null) {
			string path = Path.Combine (_basePath, data.key);
			path = Path.Combine (path, "data");
			File.WriteAllText (path, data.Serialize().ToString());
		}
	}
}
