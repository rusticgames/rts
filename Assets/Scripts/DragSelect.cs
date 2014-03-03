using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragSelect
{
	private Rect box;
	private Rect guiBox;
	private Color boxColor;
	private GUIStyle style;

	public DragSelect ()
	{
		ClearBox();
		style = new GUIStyle();
		BoxColor = new Color(0.0f, 1.0f, 0.0f, 0.2f);
	}
	
	public Rect Box
	{
		get { return box; }
	}

	public Rect GUIBox
	{
		get { return guiBox; }
	}

	public Color BoxColor
	{
		get { return boxColor; }

		set
		{
			boxColor = value;
			Texture2D texture = new Texture2D (1, 1);
			texture.SetPixel (0, 0, value);
			texture.Apply ();
			style.normal.background = texture;
		}
	}

	public GUIStyle Style
	{
		get { return style; }
	}

	public void UpdateBox (Vector2 pos, Vector2 size)
	{
		if (size.x < 0)
		{
			size.x *= -1;
			pos.x = pos.x - size.x;
		}
		
		if (size.y < 0)
		{
			size.y *= -1;
			pos.y = pos.y - size.y;
		}

		box = new Rect(pos.x, pos.y, size.x, size.y);
		guiBox = new Rect(box.x, Screen.height - (box.y + box.height), box.width, box.height);
	}

	public void ClearBox ()
	{
		UpdateBox(Vector2.zero, Vector2.zero);
	}
}