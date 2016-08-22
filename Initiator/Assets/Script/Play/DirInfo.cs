using UnityEngine;
using System.Collections;

public class DirInfo : MonoBehaviour {
	public Vector3 dir;

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("DirInfo");
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.GetComponent<Player> ().Dir = dir;
		}
	}
}
