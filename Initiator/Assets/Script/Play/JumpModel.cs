using UnityEngine;
using System.Collections;

public class JumpModel : ModelBase {
	private bool _isMoving = false;
	private int _fingerId = -1;

	void Start() {
		player.Dir = new Vector3(1, 1, 0);
		player.BaseSpeed = Config.JUMP_PLAYER_SPEED;
		map.Dir = Vector3.down;
	}

	void FixedUpdate () {
		if (_isMoving) {
			map.transform.Translate(map.Dir*Config.JUMP_MAP_MOVE_SPEED*Time.deltaTime);
		}

		bool jump = false;
		if (!jump) {// mouse
			if (Input.GetMouseButtonDown (0)) {
				jump = true;
			}
		}
		if (!jump) {// touches
			for (var i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch(i).phase == TouchPhase.Began) {
					jump = true;
					break;
				}
			}
		}
		if (jump) {
			if (!_isMoving) {
				SetIsMoving (true);
			}
			player.GetComponent<Rigidbody2D> ().velocity = player.Speed;
		}
	}

	void SetIsMoving(bool value) {
		_isMoving = value;
		player.GetComponent<Rigidbody2D> ().gravityScale = _isMoving ? Config.JUMP_PLAYER_G_SCALE : 0;
	}

	void SetPlayer(Vector3 screenPos) {
		screenPos.x -= Screen.width / 2;
		player.transform.localPosition = new Vector3(GameManager.Curr.AdjustToDesign(screenPos).x, player.transform.localPosition.y, 0);
		//		Debug.Log ("screenPos = " + screenPos + ", player.transform.localPosition = " + player.transform.localPosition + ", player.transform.position = " + player.transform.position);
	}
}
