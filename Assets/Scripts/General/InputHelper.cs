using UnityEngine;

namespace General {
	public class InputHelper {
		public const string VERTICAL = "Vertical";
		public const string HORIZONTAL = "Horizontal";
		public const string JUMP = "Jump";
		public const string MOUSE_X = "Mouse X";
		public const string MOUSE_Y = "Mouse Y";

		public static Vector2 GetInputDirection() {
			return new Vector2(Input.GetAxis(HORIZONTAL), Input.GetAxis(VERTICAL));
		}
		
		public static Vector2 GetInputMouseDirection() {
			return new Vector2(Input.GetAxis(MOUSE_X), Input.GetAxis(MOUSE_Y));
		}
	}
}