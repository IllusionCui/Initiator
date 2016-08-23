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

        // player
        RectTransform playerRTF = ResouceManager.Curr.CreateGameObject("Player").transform as RectTransform;
        playerRTF.SetParent (this.transform);
        playerRTF.localScale = Vector3.one;
        playerRTF.localPosition = new Vector3(Config.DESIGN_WIDTH/2, Config.DESIGN_HEIGHT/3, 0);
        player = playerRTF.GetComponent<Player>();

        PlayView playView = GameManager.Curr.playView;
        map.Init (playView.Data, GameStatus.Play);
    }

    public virtual void StartEndEffect() {
    
    }

    public virtual void FinsihedEndEffect() {
        if (IsWin) {
            GameManager.Curr.PlayWin ();
        } else if (IsFailed) {
            GameManager.Curr.PlayFailed ();
        }
    }
}
