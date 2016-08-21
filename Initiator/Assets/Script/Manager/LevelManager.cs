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

	public LevelInfo(MTJSONObject json) {
		Deserialize (json);
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
	public List<Vector3> items = new List<Vector3>();

	public LevelData(string key_) {
		key = key_;
	}

	public LevelData(MTJSONObject json) {
		Deserialize (json);
	}

	public MTJSONObject Serialize() {
		MTJSONObject res = MTJSONObject.CreateDict ();
		res.Add ("key", key);
		MTJSONObject itemsJO = MTJSONObject.CreateList ();
		for (int i = 0; i < items.Count; i++) {
			MTJSONObject itemJO = MTJSONObject.CreateDict ();
			itemJO.Add ("x", items[i].x);
			itemJO.Add ("y", items[i].y);
			itemsJO.Add (itemJO);
		}
		res.Add ("items", itemsJO);
		return res;
	}

	public void Deserialize(MTJSONObject json) {
		key = json ["key"].s;
		items.Clear ();
		for (int i = 0; i < json ["items"].list.Count; i++) {
			MTJSONObject itemJO = json ["items"].Get (i);
			Vector3 item = new Vector3 (itemJO["x"].f, itemJO["y"].f, 0);
			items.Add (item);
		}
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

		string path = GetLevelInfoPath (key);
		if (File.Exists(path)) {
			res = new LevelInfo (MTJSON.Deserialize(File.ReadAllText (path)));
		} else {
			res = new LevelInfo (key);
			SaveLevel (res, null);
		}

		return res;
	}

	public LevelData GetLevelData(string key) {
		LevelData res = null;

		string path = GetLevelDataPath (key);
		if (File.Exists(path)) {
			res = new LevelData (MTJSON.Deserialize(File.ReadAllText (path)));
		} else {
			res = new LevelData (key);
			SaveLevel (null, res);
		}

		return res;
	}
		
	public void SaveLevel(LevelInfo info, LevelData data) {
		if (info != null) {
			File.WriteAllText (GetLevelInfoPath(info.key), info.Serialize().ToString());
		}

		if (data != null) {
			File.WriteAllText (GetLevelDataPath(data.key), data.Serialize().ToString());
		}
	}

	string GetLevelInfoPath(string key) {
		string path = Path.Combine (_basePath, key);
		path = Path.Combine (path, "info");
		return path;
	}

	string GetLevelDataPath(string key) {
		string path = Path.Combine (_basePath, key);
		path = Path.Combine (path, "data");
		return path;
	}
}
