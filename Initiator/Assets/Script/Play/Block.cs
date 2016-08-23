using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Block");
		if (other.gameObject.tag == "Player") {
            var model = GameManager.Curr.playView.Model;
            if (model != null && model.IsPlay) {
                model.End(false);
            }
		}
	}
}
