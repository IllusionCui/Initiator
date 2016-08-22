using UnityEngine;
using System.Collections;

public class EndLine : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("EndLine");
		if (other.gameObject.tag == "Player") {
			GameManager.Curr.PlayWin ();
		}
	}
}
