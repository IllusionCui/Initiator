using UnityEngine;
using System.Collections;

public enum GameStatus {
	Main,
	Edite,
	Play,
	Succ,
	Failed
}

public enum GameType {
	Jump,
	Slide
}

public enum EditeOperationType {
	Rect,
	Clear
}

public class Config {
	public const float DESIGN_WIDTH = 1080;
	public const float DESIGN_HEIGHT = 1920;

	public const GameType DEFAULT_GAME_TYPE = GameType.Jump;
	public const EditeOperationType DEFAULT_EDITE_OPERATION = EditeOperationType.Rect;
	public const float SLIDE_MOVE_SPEED = 20;
}
