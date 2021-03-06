﻿using UnityEngine;
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

public enum ModelStatus {
    Init,
    Start,
    Play,
    Win,
    Failed
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

	public static float MAP_MOVE_SPEED_H = 32;

	public static Vector2 JUMP_PLAYER_SPEED = new Vector2(55, 119);
	public const float JUMP_PLAYER_G_SCALE = 34.5f;
}
