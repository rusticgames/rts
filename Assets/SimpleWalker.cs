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
		StartCoroutine(doLegs());
		if(tryInputs) {
		StartCoroutine(checkInputs());
		}
	}
	
	public IEnumerator checkInputs ()
	{
		while(true) {
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

	delegate void WalkFunction();
	delegate bool CheckFunction();
	public delegate IEnumerator LegsFunction(SimpleLeg legOne, SimpleLeg legTwo);
	public LegsFunction currentLegs;
	public IEnumerator stand (SimpleLeg legOne, SimpleLeg legTwo)
	{
		legOne.lower();
		legTwo.lower();
		yield return new WaitForFixedUpdate();
	}
	public IEnumerator doLegs ()
	{
		while(true) {
			currentLegs = stand;
			if(walking) {
				currentLegs = walkOneLeg;
			}
			yield return StartCoroutine(currentLegs(legOne, legTwo));
			if(walking) {
				currentLegs = walkOneLeg;
			}
			yield return StartCoroutine(currentLegs(legTwo, legOne));
		}
	}

	//TODO: This should probably account for inclines at some point, but right now assuming no rotation
	public float getSpeedRatio(float desiredSpeed) {
		float currentSpeed = this.rigidbody2D.velocity.x;
		float difference = desiredSpeed - currentSpeed;
		return difference / desiredSpeed;
	}
	
		/* 
		 * if we are standing still
		 * * lift one leg (Lf)
		 * * apply forward motion to foot on Lf
		 * * lower other leg (Ls)
		 * * apply backwards motion to foot on Ls
		 * 
		 * 
		 * 
		 */

	public void checkFeet() {
		SimpleLeg highLeg = SimpleLeg.getHigher(legOne, legTwo);
		SimpleLeg lowLeg = legOne;

		if(highLeg == lowLeg) {
			lowLeg = legTwo;
		}
		if(left) {
			SimpleLeg temp = highLeg;
			highLeg = lowLeg;
			lowLeg = temp;
		}
		highLeg.advanceOpposed();
		lowLeg.advance();
	}

	public IEnumerator walkOneLeg (SimpleLeg leg1, SimpleLeg leg2) {
		checkFeet();
		leg1.lower();
		leg2.lift();
		while(!leg1.isFullyLowered()) {
			checkFeet();
			yield return new WaitForFixedUpdate();
		}
		yield return null;
	}
}
