using UnityEngine;
using System.Collections;
using MTUnity.Utils;

public class PlayView : MonoBehaviour {
	private GameType _type;
	public GameType Type {
		get { return _type; }
	}

	private LevelData _levelData;
	public LevelData Data {
		get { return _levelData; }
	}

    private ModelBase _model;
    public ModelBase Model {
        get { return _model; }
    }

	public void PlayLevel(GameType type, LevelData ld) {
		_type = type;
		_levelData = ld;

		TransformUtil.RemoveAllChildren (transform);
		Transform modelTF = ResouceManager.Curr.CreateGameObject (type.ToString () + "Model").transform;
		modelTF.SetParent (transform);
		modelTF.localPosition = Vector3.zero;
		modelTF.localScale = Vector3.one;

        _model = modelTF.GetComponent<ModelBase>();
        _model.Init();
	}

	public void Replay() {
		PlayLevel (_type, _levelData);
	}
}
