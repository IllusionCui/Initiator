using UnityEngine;
using System.Collections;

public class SlideModel : ModelBase {
	private bool _isMoving = false;
	private int _fingerId = -1;

	void Start() {
		map.Dir = Vector3.down;
	}

	void FixedUpdate () {
		if (_isMoving) {
			map.transform.Translate(map.Dir*Config.SLIDE_MAP_MOVE_SPEED*Time.deltaTime);
		}
			
		// mouse
		{
			if (_fingerId == -1 && Input.GetMouseButtonDown (0)) {
				_fingerId = 0;
				_isMoving = true;
			}
			if (_fingerId == 0) {
				if (Input.GetMouseButtonUp (0)) {
					_fingerId = -1;
				} else {
					SetPlayer (Input.mousePosition);
				}
			}
		}
		// touches
		{
			for (var i = 0; i < Input.touchCount; ++i) {
				Touch touch = Input.GetTouch(i);
				if (_fingerId == -1) {
					if (touch.phase == TouchPhase.Began) {
						_fingerId = 0;
						_isMoving = true;
					}
				}
				if (touch.fingerId == _fingerId) {
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
						_fingerId = -1;
					} else {
						SetPlayer (touch.position);
					}
				}
			}
		}
	}

	void SetPlayer(Vector3 screenPos) {
		player.transform.localPosition = new Vector3(GameManager.Curr.AdjustToDesign(screenPos).x, player.transform.localPosition.y, 0);
//		Debug.Log ("screenPos = " + screenPos + ", player.transform.localPosition = " + player.transform.localPosition + ", player.transform.position = " + player.transform.position);
	}
}
