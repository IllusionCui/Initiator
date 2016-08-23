using UnityEngine;
using System.Collections;

public class JumpModel : ModelBase {
    public override void Init() {
        base.Init();

        player.Dir = new Vector3(1, 1, 0);
        player.BaseSpeed = Config.JUMP_PLAYER_SPEED;
        map.Dir = Vector3.down;
        map.BaseSpeed = Config.MAP_MOVE_SPEED_H;

        _status = ModelStatus.Start;
    }

    void Update () {
		if (IsStart) {
            if (HasTouchBegin()) {
                Jump();
                player.Rigidbody2D.gravityScale = Config.JUMP_PLAYER_G_SCALE;
                _status = ModelStatus.Play;
            }
		} else if (IsPlay) {
            map.transform.Translate(map.Speed*Time.deltaTime);
            if (map.EndLine.CheckWin(player)) {
                End(true);
            } else {
                if (player.transform.localPosition.x < 0) {
                    player.Dir = new Vector3(1, 1, 0);
                    player.Rigidbody2D.velocity = new Vector2 (-player.Rigidbody2D.velocity.x, player.Rigidbody2D.velocity.y);
                    player.transform.localPosition = new Vector3 (-player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
                } else if (player.transform.localPosition.x > Config.DESIGN_WIDTH) {
                    player.Dir = new Vector3(-1, 1, 0);
                    player.Rigidbody2D.velocity = new Vector2 (-player.Rigidbody2D.velocity.x, player.Rigidbody2D.velocity.y);
                    player.transform.localPosition = new Vector3 (Config.DESIGN_WIDTH*2-player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
                }
                if (HasTouchBegin()) {
                    Jump();
                }
            }
		} else if (IsWin) {
            if (player.Rigidbody2D.velocity.y <= 0) {
                player.Rigidbody2D.velocity = player.Speed;
                if (player.transform.localPosition.y > Config.DESIGN_HEIGHT) {
                    FinsihedEndEffect();
                }
            }
        }
    }

    public override void StartEndEffect() {
        if (IsWin) {
            player.Dir = Vector3.up;
            player.Rigidbody2D.velocity = player.Speed;
        }
    }

    bool HasTouchBegin() {
        // mouse
        if (Input.GetMouseButtonDown (0)) {
            return true;
        }
        // touches
        for (var i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                return true;
            }
        }
        return false;
    }

    void Jump() {
        player.Rigidbody2D.velocity = player.Speed;
	}
}
