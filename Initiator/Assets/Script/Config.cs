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

public enum EditeOperationType {
	Rect,
	Clear
}

public class Config {
	public const float DESIGN_WIDTH = 1080;
	public const float DESIGN_HEIGHT = 1920;

	public const GameType DEFAULT_GAME_TYPE = GameType.Jump;
	public const EditeOperationType DEFAULT_EDITE_OPERATION = EditeOperationType.Rect;

	public const float SLIDE_MAP_MOVE_SPEED = 20;

	public const float JUMP_MAP_MOVE_SPEED = 12;
	public static Vector2 JUMP_PLAYER_SPEED = new Vector2(100, 200);
	public const float JUMP_PLAYER_G_SCALE = 80f;
}
