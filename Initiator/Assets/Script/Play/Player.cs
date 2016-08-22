using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private Vector3 _dir = Vector3.zero;
	public Vector3 Dir {
		get { return _dir; }
		set {
			Rigidbody2D rb = GetComponent<Rigidbody2D> ();
			Vector2 v = rb.velocity;
			if (_dir.x != 0) {
				v.x *= value.x / _dir.x;
			}
			if (_dir.y != 0) {
				v.y *= value.y / _dir.y;
			}
			rb.velocity = v;
			_dir = value;
		}
	}
}
