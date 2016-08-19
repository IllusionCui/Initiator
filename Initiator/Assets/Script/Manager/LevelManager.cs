using System.Collections.Generic;

public class LevelInfo {
	public string key;
	public string name;
	public GameType type;

	public LevelInfo(string key_, string name_, GameType type_) {
		key = key_;
		name = name_;
		type = type_;
	}

	public LevelInfo(string key_) : this(key_, key_, Config.DEFAULT_GAME_TYPE) {
	}
}

public class LevelData {
	public string key;


}

public class LevelManager {
	public List<LevelInfo> levelInfoes = new List<LevelInfo>();

	public LevelManager() {
		LoadLevel ();
	}

	public string CreateNew() {
		string key = "LV "+ (levelInfoes.Count + 1);
		LevelInfo info = new LevelInfo (key);

		levelInfoes.Add (info);
		SaveLevel (key);

		return key;
	}
		
	public void SaveLevel(string key) {
		
	}

	void LoadLevel() {
		// test
		for(int i = 0; i < 10; i++) {
			CreateNew ();
		}
	}
}
