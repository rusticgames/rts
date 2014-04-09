using UnityEngine;
using System.Collections;

[AddComponentMenu("Periodic/Rendering")]
public class Fader : MonoBehaviour {
	public float timeInterval = 0.1f;
	public bool keepOnKeepingOn = true;
	[Range (0,1)]
	public float fadeTarget = 1.0f;
	[Range (0.001f,1f)]
	public float fadeRate = 1.0f;
	private const string FADE_PROPERTY_NAME = "_Cutoff";

	void Start () {
		StartCoroutine("fade");
	}

	
	IEnumerator fade () {
		while(keepOnKeepingOn) {
			while(fadeTarget != this.renderer.material.GetFloat(FADE_PROPERTY_NAME)) {
				print(string.Format("Imbalance -- \r\n\tTarget: {0}\r\n\tCurrent: {1}", fadeTarget, this.renderer.material.GetFloat(FADE_PROPERTY_NAME)));
				while(fadeTarget - fadeRate > this.renderer.material.GetFloat(FADE_PROPERTY_NAME)) {
					this.renderer.material.SetFloat(FADE_PROPERTY_NAME, this.renderer.material.GetFloat(FADE_PROPERTY_NAME) + fadeRate);
					print(string.Format("Increase -- To: {0}", this.renderer.material.GetFloat(FADE_PROPERTY_NAME)));
					yield return new WaitForSeconds(timeInterval);
				}
				print(string.Format("After Increase -- \r\n\tTarget: {0}\r\n\tCurrent: {1}", fadeTarget, this.renderer.material.GetFloat(FADE_PROPERTY_NAME)));
				while(fadeTarget + fadeRate < this.renderer.material.GetFloat(FADE_PROPERTY_NAME)) {
					this.renderer.material.SetFloat(FADE_PROPERTY_NAME, this.renderer.material.GetFloat(FADE_PROPERTY_NAME) - fadeRate);
					print(string.Format("Decrease -- To: {0}", this.renderer.material.GetFloat(FADE_PROPERTY_NAME)));
					yield return new WaitForSeconds(timeInterval);
				}
				print(string.Format("After Decrease -- \r\n\tTarget: {0}\r\n\tCurrent: {1}", fadeTarget, this.renderer.material.GetFloat(FADE_PROPERTY_NAME)));
				this.renderer.material.SetFloat(FADE_PROPERTY_NAME, fadeTarget);
			}
			yield return new WaitForSeconds(timeInterval);
		}
	}
}
