using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LegsIntent { NONE, STAND, WALK, CROUCH, RUN, JUMP }
public delegate LegsIntent IntentionGetter();
public delegate IEnumerator LegsFunction(SimpleLeg legOne, SimpleLeg legTwo);
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
	public LegsFunction currentLegs;

	private IntentionGetter legIntentGetter;
	private Dictionary<LegsIntent, LegsFunction> legIntentToFunction;


	void Start () {
		legIntentToFunction = new Dictionary<LegsIntent, LegsFunction>();
		legIntentToFunction.Add(LegsIntent.STAND, MovementUtility.stand);
		legIntentToFunction.Add(LegsIntent.CROUCH, MovementUtility.crouch);
		if(delayJump) {
			legIntentToFunction.Add(LegsIntent.JUMP, MovementUtility.delayJump);
		} else {
			legIntentToFunction.Add(LegsIntent.JUMP, MovementUtility.jump);
		}
		legIntentToFunction.Add(LegsIntent.WALK, MovementUtility.walkOneLeg);

		if(legOne == null) {
			Vector3 position = this.transform.position;
			position.y = position.y + 1f;
			this.transform.position = position;
			legOne = MovementUtility.spawnLeg(this.gameObject, legPrefab, footPrefab, legData, .4f, "One");
		}
		if(legTwo == null) {
			legTwo = MovementUtility.spawnLeg(this.gameObject, legPrefab, footPrefab, legData, -.4f, "Two");
		}

		if(fullManual) {
			StartCoroutine(manualLegs());
		} else {
			legIntentGetter = autoIntent;
			if(tryInputs) {
				legIntentGetter = getIntentFromInputs;
			}
			StartCoroutine(doLegs());
		}
	}
	public LegsIntent autoIntent() {
		if(left) {
			legOne.footAdvanceFactor = -1f;
			legTwo.footAdvanceFactor = -1f;
		}
		if(walking) { return LegsIntent.WALK; }
		if(crouching) { return LegsIntent.CROUCH; }
		if(jumping) { return LegsIntent.JUMP; }
		return LegsIntent.STAND;
	}
	public LegsIntent getIntentFromInputs ()
	{
		float xAxis = Input.GetAxis("Horizontal");
		legOne.footAdvanceFactor = xAxis;
		legTwo.footAdvanceFactor = xAxis;
		if(Input.GetKey(KeyCode.LeftControl)) { return LegsIntent.CROUCH; }
		if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick2Button0)) { return LegsIntent.JUMP; }
		if(!(Mathf.Approximately(xAxis, 0f))) { return LegsIntent.WALK; }
		return LegsIntent.STAND;
	}

	public IEnumerator manualLegs () {
		while(true) {
			if(Input.GetKeyDown(KeyCode.Joystick1Button0)) {
				yield return StartCoroutine(MovementUtility.jump (legOne, legTwo));
			} else {
				legOne.footAdvanceFactor = Input.GetAxis("Horizontal");
				legTwo.footAdvanceFactor = Input.GetAxis("Right Stick X Axis");
				legOne.liftFactor = Input.GetAxis("Vertical");
				legTwo.liftFactor = Input.GetAxis("Right Stick Y Axis");
				legOne.lift();
				legTwo.lift();
				legOne.advanceOpposed();
				legTwo.advanceOpposed();
			}
			yield return null;
		}
	}
	public IEnumerator doLegs ()
	{
		while(true) {
			LegsIntent li = legIntentGetter();
			currentLegs = legIntentToFunction[li];
			yield return StartCoroutine(currentLegs(legOne, legTwo));
			if(li == LegsIntent.WALK) {
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

}