using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private Vector3 _dir = Vector3.zero;
	public Vector3 Dir {
		get { return _dir; }
		set { _dir = value; }
	}

	void OnTriggerEnter2D(Collider2D other) {
		DirInfo dirInfo = other.gameObject.GetComponent<DirInfo> ();
		if (dirInfo != null) {
			Dir = dirInfo.dir;
		}
		if (other.gameObject.tag == "Block") {
			GameManager.Curr.PlayFailed ();
		} else if (other.gameObject.tag == "EndLine") {
			GameManager.Curr.PlayWin ();
		}
	}
}
