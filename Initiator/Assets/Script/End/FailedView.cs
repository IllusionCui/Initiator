using UnityEngine;
using System.Collections;

public class FailedView : MonoBehaviour {
	public void OnOK() {
		GameManager.Curr.EndPlay ();
	}

	public void OnReplay() {
		GameManager.Curr.Replay ();
	}
}
