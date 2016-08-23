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

    private Rigidbody2D _rigidbody2D;
    public Rigidbody2D Rigidbody2D {
        get { 
            if (_rigidbody2D == null) {
                _rigidbody2D = GetComponent<Rigidbody2D> ();
            }
            return _rigidbody2D;
        }
    }

    private int _hp = 1;

    public void BeHit() {
        // anim
        gameObject.GetComponent<ImageFade>().FadeOutInAnim(3, delegate() {
            ModelBase model = GameManager.Curr.playView.Model;
            if (model != null && model.IsFailed) {
                model.FinsihedEndEffect();
            }
        });

        if (_hp > 0) {
            _hp--;
            if (_hp == 0) {
                ModelBase model = GameManager.Curr.playView.Model;
                if (model != null && model.IsPlay) {
                    model.End(false);
                }
            }
        }
    }
}
