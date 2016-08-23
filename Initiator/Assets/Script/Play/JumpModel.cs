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
        if (_status == ModelStatus.Start) {
            if (HasTouchBegin()) {
                Jump();
                player.GetComponent<Rigidbody2D> ().gravityScale = Config.JUMP_PLAYER_G_SCALE;
                _status = ModelStatus.Play;
            }
        } else if (_status == ModelStatus.Play) {
            map.transform.Translate(map.Speed*Time.deltaTime);


            if (map.EndLine.CheckWin(player)) {
                End(true);
            } else {
                if (player.transform.localPosition.x < 0) {
                    player.Dir = new Vector3(1, 1, 0);
                    Rigidbody2D rb = player.GetComponent<Rigidbody2D> ();
                    rb.velocity = new Vector2 (-rb.velocity.x, rb.velocity.y);
                    player.transform.localPosition = new Vector3 (-player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
                } else if (player.transform.localPosition.x > Config.DESIGN_WIDTH) {
                    player.Dir = new Vector3(-1, 1, 0);
                    Rigidbody2D rb = player.GetComponent<Rigidbody2D> ();
                    rb.velocity = new Vector2 (-rb.velocity.x, rb.velocity.y);
                    player.transform.localPosition = new Vector3 (Config.DESIGN_WIDTH*2-player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
                }
                if (HasTouchBegin()) {
                    Jump();
                }
            }
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
        player.GetComponent<Rigidbody2D> ().velocity = player.Speed;
	}
}
