using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GUITexture))]

public class FadeInOut : MonoBehaviour {
	public float duration = 1.5f;
	public Color startColor = Color.black;
	public Color endColor = Color.clear;
	
	void Awake()
	{
		guiTexture.color = startColor;
		guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
	}

	void Start()
	{
		StartCoroutine(Fade());
	}

	public IEnumerator Fade() {
		float timeElapsed = 0;
		Color newColor = (guiTexture.color == startColor) ? endColor : startColor;
		Color finalColor = guiTexture.color;
	
		Debug.Log("Starting fade from " + finalColor + ", to " + newColor + ": " + Time.time);

		while(timeElapsed < duration) {
			guiTexture.color = Color.Lerp(finalColor, newColor, timeElapsed);
			yield return null;
			timeElapsed += Time.deltaTime;
		}

		guiTexture.color = newColor;
		Debug.Log("Finished fade to " + newColor + ": " + Time.time);
	}
}