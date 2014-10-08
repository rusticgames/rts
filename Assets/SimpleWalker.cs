using UnityEngine;
using System.Collections;

public enum LEGS_INTENT { NONE, STAND, WALK, CROUCH, RUN, JUMP }
public class SimpleWalker : MonoBehaviour {
	public GameObject legPrefab;
	public GameObject footPrefab;
	public LegData legData;

	public SimpleLeg legOne;
	public SimpleLeg legTwo;
	public bool walking;
	public bool jumping;
	public bool delayJump;
	public bool crouching;
	public bool left;
	public bool tryInputs = false;
	public bool fullManual;
	public LEGS_INTENT currentLegsIntent = LEGS_INTENT.STAND;

	SimpleLeg spawnLeg(float footXOffset) {
		Vector3 spawnPoint = this.transform.position;
		Quaternion rotation = this.transform.rotation;
		SimpleLeg newLeg;
		spawnPoint.y = spawnPoint.y - 0.5f;
		newLeg = ((GameObject)GameObject.Instantiate(legPrefab, spawnPoint, rotation)).GetComponent<SimpleLeg>();
		newLeg.legData = legData;
		newLeg.thigh = newLeg.GetComponent<SliderJoint2D>();
		newLeg.thigh.connectedBody = this.rigidbody2D;
		spawnPoint.x = spawnPoint.x + footXOffset;
		spawnPoint.y = spawnPoint.y - .25f;
		newLeg.foot = ((GameObject)GameObject.Instantiate(footPrefab, spawnPoint, rotation)).GetComponent<SliderJoint2D>();
		newLeg.foot.connectedBody = newLeg.thigh.rigidbody2D;
		return newLeg;
	}

	void Start () {
		if(legOne == null) {
			legOne = spawnLeg(.3f);
		}
		if(legTwo == null) {
			legTwo = spawnLeg(-.3f);
		}
		if(fullManual) {
			StartCoroutine(manualLegs());
		} else {
			StartCoroutine(doLegs());
			if(tryInputs) {
				StartCoroutine(checkInputs());
			}
		}
	}
	
	public IEnumerator checkInputs ()
	{
		while(true) {
			walking = false;
			crouching = false;
			jumping = false;
			if(Input.GetKey(KeyCode.LeftArrow)) {
				left = true;
				walking = true;
			}
			if(Input.GetKey(KeyCode.RightArrow)) {
				left = false;
				walking = true;
			}
			if(Input.GetKey(KeyCode.LeftControl)) {
				crouching = true;
			}
			if(Input.GetKey(KeyCode.Space)) {
				jumping = true;
			}
			yield return new WaitForFixedUpdate();
		}
	}

	public delegate IEnumerator LegsFunction(SimpleLeg legOne, SimpleLeg legTwo);
	public LegsFunction currentLegs;
	public IEnumerator stand (SimpleLeg legOne, SimpleLeg legTwo)
	{
		legOne.lower();
		legTwo.lower();
		yield return new WaitForFixedUpdate();
	}
	public IEnumerator crouch (SimpleLeg legOne, SimpleLeg legTwo)
	{
		legOne.lift();
		legTwo.lift();
		yield return new WaitForFixedUpdate();
	}
	public IEnumerator jump (SimpleLeg legOne, SimpleLeg legTwo)
	{
		if(delayJump) {
			legOne.lift();
			legTwo.lift();
			legOne.advance();
			legTwo.advance();
			while(jumping) {
				yield return new WaitForFixedUpdate();
			}
		}

		legOne.jump();
		legTwo.jump();
		while(!(legOne.isFullyLowered() || legTwo.isFullyLowered())) {
			yield return new WaitForFixedUpdate();
		}

		//legOne.relaxThigh();
		//legTwo.relaxThigh();
		yield return new WaitForFixedUpdate();
	}
	public IEnumerator manualLegs () {
		while(true) {
			if(Input.GetKeyDown(KeyCode.Joystick1Button0)) {
				Debug.Log("Key-oop");
				yield return StartCoroutine(jump (legOne, legTwo));
			} else {
				legOne.footAdvanceFactor = Input.GetAxis("Horizontal");
				legTwo.footAdvanceFactor = Input.GetAxis("Right Stick X Axis");
				legOne.liftFactor = Input.GetAxis("Vertical");
				legTwo.liftFactor = Input.GetAxis("Right Stick Y Axis");
				legOne.lift();
				legTwo.lift();
				legOne.advanceOpposed();
				legTwo.advanceOpposed();
				//Debug.Log("1: " + foot1 + ", " + leg1 + "  |  2: " + foot2 + ", " + leg2);
				yield return null;
			}
		}
	}
	public IEnumerator doLegs ()
	{
		while(true) {
			currentLegs = stand;
			if(jumping) {
				currentLegs = jump;
			}else if(crouching) {
				currentLegs = crouch;
			}else if(walking) {
				currentLegs = walkOneLeg;
			}
			yield return StartCoroutine(currentLegs(legOne, legTwo));
			if(walking) {
				yield return StartCoroutine(currentLegs(legTwo, legOne));
			}
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
		float heightDiff = SimpleLeg.getHigher(legOne, legTwo);
		SimpleLeg highLeg = legOne;
		SimpleLeg lowLeg = legTwo;
		if(left) {
			heightDiff = -heightDiff;
		}

		if(heightDiff < 0f) {
			lowLeg = legOne;
			highLeg = legTwo;
		}

		highLeg.advanceOpposed();
		lowLeg.advance();
	}

	public IEnumerator walkOneLeg (SimpleLeg leg1, SimpleLeg leg2) {
		checkFeet();
		leg1.lower();
		leg2.lift();
		while(!leg1.isFullyLowered()) {
			leg1.lower();
			leg2.lift();
			checkFeet();
			yield return new WaitForFixedUpdate();
		}
		yield return null;
	}
}
