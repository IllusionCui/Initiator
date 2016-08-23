using UnityEngine;
using System.Collections;

public class ModelBase : MonoBehaviour {
	public Player player;
	public Map map;

    protected ModelStatus _status = ModelStatus.Init;

    public bool IsStart {
        get { return _status == ModelStatus.Start; }
    }

    public bool IsPlay {
        get { return _status == ModelStatus.Play; }
    }

    public bool IsWin {
        get { return _status == ModelStatus.Win; }
    }

    public bool IsFailed {
        get { return _status == ModelStatus.Failed; }
    }

    public bool IsEnd {
        get { return _status == ModelStatus.Win || _status == ModelStatus.Failed; }
    }

    public void End(bool win) {
        _status = win ? ModelStatus.Win : ModelStatus.Failed;
        StartEndEffect();
    }

    public virtual void Init() {
        _status = ModelStatus.Init;

        PlayView playView = GameManager.Curr.playView;
        map.Init (playView.Data, GameStatus.Play);
    }

    protected virtual void StartEndEffect() {
    
    }

    protected virtual void FinsihedEndEffect() {
        if (IsWin) {
            GameManager.Curr.PlayWin ();
        } else if (IsFailed) {
            GameManager.Curr.PlayFailed ();
        }
    }
}
