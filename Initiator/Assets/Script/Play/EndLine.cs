using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndLine : MonoBehaviour {
	public RectTransform line;

	public bool CheckWin(Player player) {
		if (player.transform.position.y >= line.position.y) {
			return true;
		}
		return false;
	}
}
