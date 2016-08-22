using UnityEngine;
using System.Collections;

public class JumpModel : ModelBase {
	private bool _isMoving = false;

	void Start() {
		player.Dir = new Vector3(1, 1, 0);
		player.BaseSpeed = Config.JUMP_PLAYER_SPEED;
		map.Dir = Vector3.down;
		map.BaseSpeed = Config.MAP_MOVE_SPEED_H;
	}

	void Update () {
		if (_isMoving) {
			map.transform.Translate (map.Speed * Time.deltaTime);

			bool changeDir = false;
			if (player.transform.localPosition.x < 0) {
				changeDir = true;
				player.Dir = new Vector3(1, 1, 0);
				player.transform.localPosition = new Vector3 (-player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
			} else if (player.transform.localPosition.x > Config.DESIGN_WIDTH) {
				changeDir = true;
				player.Dir = new Vector3(-1, 1, 0);
				player.transform.localPosition = new Vector3 (Config.DESIGN_WIDTH*2-player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
			}

			if (changeDir) {
				Rigidbody2D rb = player.GetComponent<Rigidbody2D> ();
				rb.velocity = new Vector2 (-rb.velocity.x, rb.velocity.y);
			}
			CheckWin ();
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
