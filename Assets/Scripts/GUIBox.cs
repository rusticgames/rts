using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIBox
{
	private Rect worldRect;
	private Rect screenRect;
	private Color boxColor;
	private GUIStyle style;

	public GUIBox ()
	{
		ClearBox();
		style = new GUIStyle();
		BoxColor = new Color(0.0f, 1.0f, 0.0f, 0.2f);
		worldRect = new Rect();
		screenRect = new Rect();
	}
	
	public Rect WorldRect
	{
		get { return worldRect; }
	}

	public Rect ScreenRect
	{
		get { return screenRect; }
	}

	public Color BoxColor
	{
		get { return boxColor; }

		set {
			boxColor = value;
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0, 0, value);
			texture.Apply();
			style.normal.background = texture;
		}
	}

	public GUIStyle Style
	{
		get { return style; }
	}

	public void UpdateBoxFromScreen(Vector2 screenOrigin, Vector2 screenSize)
	{
		if (screenSize.x < 0) {
			screenSize.x *= -1;
			screenOrigin.x = screenOrigin.x - screenSize.x;
		}
		
		if (screenSize.y < 0) {
			screenSize.y *= -1;
			screenOrigin.y = screenOrigin.y - screenSize.y;
		}

		worldRect = new Rect(screenOrigin.x, screenOrigin.y, screenSize.x, screenSize.y);
		screenRect = new Rect(worldRect.x, Screen.height - (worldRect.y + worldRect.height), worldRect.width, worldRect.height);
	}

	public void ClearBox()
	{
		UpdateBoxFromScreen(Vector2.zero, Vector2.zero);
	}
}