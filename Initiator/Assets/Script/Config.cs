using UnityEngine;
using System.Collections;

public enum GameType {
	Jump,
	Slide
}

public class Config {
	public const GameType DEFAULT_GAME_TYPE = GameType.Jump;


	public static Color GetColorByType(GameType type) {
		if (type == GameType.Jump) {
			return Color.red;	
		} else {
			return Color.blue;
		}
	}
}
