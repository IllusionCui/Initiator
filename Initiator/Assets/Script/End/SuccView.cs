using UnityEngine;
using System.Collections;

public class SuccView : MonoBehaviour {
	public void OnOK() {
		GameManager.Curr.EndPlay ();
	}
}
