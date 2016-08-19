using UnityEngine;
using System.Collections;

public enum GameStatus {
	Main,
	Edite,
	Play,
	Succ,
	Failed
}

public class GameManager : MonoBehaviour {
	// views
	public MainView mainView;
	public EditeView editeView;
	public PlayView playView;
	public SuccView succView;
	public FailedView failedView;

	// data
	private LevelManager _levelManager;
	public LevelManager LevelManager {
		get { return _levelManager; }
	}

	private static GameManager _curr = null;
	public static GameManager Curr {
		get { return _curr; }
	}

	private GameStatus _status = GameStatus.Main;
	public GameStatus Status {
		get{ return _status; }
		private set {
			_status = value;
			UpdateView ();
		}
	}

	void Awake() {
		if (_curr != null && _curr != this) {
			Destroy (this.gameObject);
			return;
		}
		_curr = this;

		Init ();
	} 

	void Init() {
		_levelManager = new LevelManager ();

		Status = GameStatus.Main;
	}

	void UpdateView() {
		mainView.gameObject.SetActive (Status == GameStatus.Main);
		editeView.gameObject.SetActive (Status == GameStatus.Edite);
		playView.gameObject.SetActive (Status == GameStatus.Play);
		succView.gameObject.SetActive (Status == GameStatus.Succ);
		failedView.gameObject.SetActive (Status == GameStatus.Failed);
	}

	public void EditeLevel(string key) {
		editeView.EditeLevel (key);
		Status = GameStatus.Edite;
	}

	public void PlayLevel(string key) {
		playView.PlayLevel (key);
		Status = GameStatus.Play;
	}
}
