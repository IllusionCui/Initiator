using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private Vector3 _dir = Vector3.zero;
	public Vector3 Dir {
		get { return _dir; }
		set {
			_dir = value;
		}
	}

	public Vector2 BaseSpeed {
		get;
		set;
	}

	public Vector2 Speed {
		get { 
			Vector2 v = BaseSpeed;
			v.x *= Dir.x;
			v.y *= Dir.y;
			return v;
		}
	}

    private int _hp = 1;

    private int _fadeCount = 0;
    public void BeHit() {
        if (_fadeCount > 0) {
            _fadeCount = 3;
            return;
        }
        _hp--;
        if (_hp == 0) {
            ModelBase model = GameManager.Curr.playView.Model;
            if (model != null && model.IsPlay) {
                model.End(false);
            }
        }
        gameObject.GetComponent<ImageFade>().FadeOutInAnim(3, delegate() {
            ModelBase model = GameManager.Curr.playView.Model;
            if (model != null && model.IsFailed) {
                model.FinsihedEndEffect();
            }
        });
    }
}
