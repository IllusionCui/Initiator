using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainItemView : MonoBehaviour {
	public Text name;
	public Image type;

	private LevelInfo _levelInfo;

	public void SetLevelInfo(LevelInfo levelInfo) {
		_levelInfo = levelInfo;
		UpdateView ();
	}

	public void OnPlay() {
		GameManager.Curr.PlayLevel (_levelInfo.key);
	}

	public void OnEdite() {
		GameManager.Curr.EditeLevel (_levelInfo.key);
	}

	void UpdateView() {
		name.text = _levelInfo.name;
		type.color = Config.GetColorByType (_levelInfo.type);
	}
}
