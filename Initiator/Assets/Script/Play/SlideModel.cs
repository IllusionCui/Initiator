using UnityEngine;
using System.Collections;

public class SlideModel : ModelBase {
	private int _fingerId = -1;

    public override void Init() {
        base.Init();

        map.Dir = Vector3.down;
        map.BaseSpeed = Config.MAP_MOVE_SPEED_H;

        _status = ModelStatus.Start;
    }

	void Update () {
        if (_status == ModelStatus.Start) {
            Vector3 fingerPos;
            if (GetFingerPos(out fingerPos)) {
                SetPlayer(fingerPos);
                _status = ModelStatus.Play;
            }
        } else if (_status == ModelStatus.Play) {
            map.transform.Translate(map.Speed*Time.deltaTime);
            if (map.EndLine.CheckWin(player)) {
                End(true);
            } else {
                Vector3 fingerPos;
                if (GetFingerPos(out fingerPos)) {
                    _status = ModelStatus.Play;
                    SetPlayer(fingerPos);
                }
            }
		}
	}

    bool GetFingerPos(out Vector3 fingerPos) {
        // mouse
        if (Input.GetMouseButtonDown (0)) {
            _fingerId = 0;
        } else if (Input.GetMouseButtonUp (0)) {
            _fingerId = -1;
        }
        if (_fingerId == 0) {
            fingerPos = Input.mousePosition;
            return true;
        }
        // touches
        for (var i = 0; i < Input.touchCount; ++i) {
            Touch touch = Input.GetTouch(i);
            if (_fingerId == -1) {
                if (touch.phase == TouchPhase.Began) {
                    _fingerId = 0;
                }
            }
            if (touch.fingerId == _fingerId) {
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                    _fingerId = -1;
                } else {
                    fingerPos = touch.position;
                    return true;
                }
            }
        }

        fingerPos = Vector3.zero;
        return false;
    }

	void SetPlayer(Vector3 screenPos) {
		player.transform.localPosition = new Vector3(GameManager.Curr.AdjustToDesign(screenPos).x, player.transform.localPosition.y, 0);
	}
}
