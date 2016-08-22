using UnityEngine;
using System.Collections;

public class ModelBase : MonoBehaviour {
	public Player player;
	public Map map;

	public bool CheckWin() {
		if (map.EndLine.CheckWin(player)) {
			GameManager.Curr.PlayWin ();
			return true;
		}
		return true;
	}
}
