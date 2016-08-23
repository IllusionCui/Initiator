using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ImageFade : MonoBehaviour {
	public Image image;
	public float fadeInTime = 0.3f;
	public float fadeOutTime = 0.1f;

	private float _alpValue;

	private bool _action = false;
	private float _targetAlpValue;
	private float _changeAplSpeed;
	private float _endTime;
	private Action _actionCallBack;

    private int _actionCount;

	public float AlpValue {
		set { 
			_alpValue = value;
		}
	}

    public void FadeOutInAnim(int count, Action actionCallBack) {
        if (_actionCount > 0) {
            return;
        }
        _actionCount = count * 2;
        FadeOutAnim(actionCallBack);
    }

	public void FadeInAnim(Action actionCallBack, bool reset = true) {
		if (image != null && (!_action || _changeAplSpeed < 0)) {
			if (reset) {
				Color color = image.color;
				color.a = 0;
				image.color = color;
			}
			ActionAnim (_alpValue, actionCallBack);
		}
	}

	public void FadeOutAnim(Action actionCallBack) {
		if (image != null && (!_action || _changeAplSpeed > 0)) {
			ActionAnim (0, actionCallBack);
		}
	}

	public void StopActionAnim() {
		_action = false;
        if (_actionCount > 0) {
            _actionCount--;
        }
        if (_actionCount == 0) {
            if (_actionCallBack != null) {
                _actionCallBack.Invoke();
            }
            _actionCallBack = null;
        } else {
            if (_actionCount % 2 == 1) {
                FadeInAnim(_actionCallBack);   
            }
        }
	}

	void ActionAnim(float targetAlpValue, Action actionCallBack) {
		StopActionAnim ();

		_actionCallBack = actionCallBack;

		float changeValue = targetAlpValue - image.color.a;
		if (changeValue == 0) {
			StopActionAnim ();
			return;
		}

		_action = true;
		_targetAlpValue = targetAlpValue;
		float changeTime = changeValue > 0 ? fadeInTime : fadeOutTime;
		_changeAplSpeed = changeValue / changeTime;
		_endTime = Time.time + changeTime;
	}

	void Start() {
		if (image == null) {
			image = GetComponent<Image> ();
		}
		if (image != null) {
			_alpValue = image.color.a;
		} else {
			Debug.LogWarning ("[ImageFade]  image = null");
		}
	}

	void Update() {
		if (_action) {
			Color color = image.color;
			if (Time.time < _endTime) {
				color.a += _changeAplSpeed * Time.deltaTime;
			} else {
				color.a = _targetAlpValue;
				StopActionAnim ();
			}
			image.color = color;
		}
	}
}
