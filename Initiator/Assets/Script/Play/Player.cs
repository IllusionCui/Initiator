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
}
