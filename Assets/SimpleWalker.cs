using UnityEngine;
using System.Collections;

public class SimpleWalker : MonoBehaviour {
	public SimpleLeg legOne;
	public SimpleLeg legTwo;
	public bool walking;
	public bool left;
	public bool tryInputs = false;
	// Use this for initialization
	void Start () {
		Debug.LogWarning("NEED TO DEFINE REST STATE AND HOW TO CHECK DEVIATION THEREFROM");
		StartCoroutine(walk());
		if(tryInputs) {
		StartCoroutine(checkInputs());
		}
	}
	
	public IEnumerator checkInputs ()
	{
		while(true) {
			left = false;
			walking = false;
			if(Input.GetKey(KeyCode.LeftArrow)) {
				left = true;
				walking = true;
			}
			if(Input.GetKey(KeyCode.RightArrow)) {
				left = false;
				walking = true;
			}
			yield return new WaitForEndOfFrame();
		}
	}
	
	delegate void walkFunction();
	delegate bool checkFunction();
	public IEnumerator walk ()
	{
		while(true) {
			if(walking) {
				yield return StartCoroutine(walkOneLeg(legOne, legTwo));
			}
			if(walking) {
				yield return StartCoroutine(walkOneLeg(legTwo, legOne));
			}
			yield return new WaitForFixedUpdate();
		}
	}

	
	public IEnumerator walkOneLeg (SimpleLeg leg1, SimpleLeg leg2) {
		Debug.LogWarning("Leg <" + leg1 +"> lowering, Leg <" + leg2 +"> rising");
		leg1.lower();
		leg2.lift();
		while(!leg1.isFullyLowered()) {
			yield return new WaitForEndOfFrame();
		}
		if(!walking) yield return null;
		
		walkFunction advance = leg1.advance;
		checkFunction check = leg1.isPastCenterOfMass;
		checkFunction checkTwo = leg1.isAdvanced;
		if(left) {
			advance = leg1.advanceOpposed;
			check = leg1.isPastCenterOfMassOpposed;
			checkTwo = leg1.isAdvancedOpposed;
		}
		
		Debug.LogWarning("Leg <" + leg1 +"> advancing, Leg <" + leg2 +"> relaxing");
		advance();
		leg2.relax();
		while(!check()) {
			yield return new WaitForEndOfFrame();
		}
		leg2.lift ();
		leg1.lower();
		Debug.LogWarning("Midpoint early: Leg <" + leg1 +"> advancing, Leg <" + leg2 +"> relaxing");
		while(!checkTwo()) {
			yield return new WaitForEndOfFrame();
		}
		Debug.LogWarning("walk cycle complete");
		yield return null;
	}
}
