using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Block") {
			GameManager.Curr.PlayFailed ();
		} else if (other.gameObject.tag == "EndLine") {
			GameManager.Curr.PlayWin ();
		}
	}
}
