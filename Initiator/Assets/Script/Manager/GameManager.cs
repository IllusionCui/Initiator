using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Canvas canvas;

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

	// info
	public GameStatus StatusBeforePlay {
		private set;
		get;
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

	public Vector3 AdjustToDesign(Vector3 pos) {
		return new Vector3 (pos.x / canvas.scaleFactor, pos.y / canvas.scaleFactor,pos.z);
	}

	void Awake() {
		if (_curr != null && _curr != this) {
			Destroy (this.gameObject);
			return;
		}
		_curr = this;
		DontDestroyOnLoad (gameObject);

		Init ();
	} 

	void Init() {
		_levelManager = new LevelManager ();

	}

	void Start() {
		Status = GameStatus.Main;
	}

	void UpdateView() {
		mainView.gameObject.SetActive (Status == GameStatus.Main);
		editeView.gameObject.SetActive (Status == GameStatus.Edite);
		playView.gameObject.SetActive (Status == GameStatus.Play);
		succView.gameObject.SetActive (Status == GameStatus.Succ);
		failedView.gameObject.SetActive (Status == GameStatus.Failed);
	}

	public void GotoMain() {
		Status = GameStatus.Main;
	}

	public void EditeLevel(string key) {
		editeView.EditeLevel (key);
		Status = GameStatus.Edite;
	}

	public void PlayLevel(GameType type, LevelData ld) {
		StatusBeforePlay = Status;

		playView.PlayLevel (type, ld);
		Status = GameStatus.Play;
	}

	public void Replay() {
		playView.Replay ();
		Status = GameStatus.Play;
	}

	public void PlayWin() {
		Status = GameStatus.Succ;
	}

	public void PlayFailed() {
		Status = GameStatus.Failed;
	}

	public void EndPlay() {
		Status = StatusBeforePlay;
	}
}
