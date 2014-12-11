using UnityEngine;
using System.Collections;

public class KeyboardConfiguration : MonoBehaviour {

	public KeyCode up;
	public KeyCode down;
	public KeyCode left;
	public KeyCode right;
	public KeyCode shoot;

	[ContextMenu("Set WASD")]
	void SetWASD() {
		up    = KeyCode.W;
		down  = KeyCode.S;
		left  = KeyCode.A;
		right = KeyCode.D;
		shoot = KeyCode.Space;
	}

	[ContextMenu("Set Arrows")]
	void SetArrows() {
		up    = KeyCode.UpArrow;
		down  = KeyCode.DownArrow;
		left  = KeyCode.LeftArrow;
		right = KeyCode.RightArrow;
		shoot = KeyCode.RightControl;
	}
}
