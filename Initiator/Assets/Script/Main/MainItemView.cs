using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainItemView : MonoBehaviour {
	public Text id;
	public Image type;

	private LevelInfo _levelInfo;

	public void SetLevelInfo(LevelInfo levelInfo) {
		_levelInfo = levelInfo;
		UpdateView ();
	}

	public void OnPlay() {
		GameManager gm = GameManager.Curr;
		gm.PlayLevel (_levelInfo.type, gm.LevelManager.GetLevelData(_levelInfo.key));
	}

	public void OnEdite() {
		GameManager.Curr.EditeLevel (_levelInfo.key);
	}

	void UpdateView() {
		id.text = _levelInfo.name;
		type.sprite = ResouceManager.Curr.GetSprite (_levelInfo.type.ToString());
	}
}
