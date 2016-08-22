﻿using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Block");
		if (other.gameObject.tag == "Player") {
			GameManager.Curr.PlayFailed ();
		}
	}
}