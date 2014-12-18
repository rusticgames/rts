using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LegsIntent { NONE, STAND, WALK, CROUCH, RUN, JUMP }
public delegate IEnumerator IntentionSetter();
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
	public LegsIntent currentLegsIntent;
	public LegsFunction currentLegs;
	public Vector2 lastGravity = Vector2.zero;
	public Vector2 currentGravity;

	private IntentionSetter legIntentSetter;
	private Dictionary<LegsIntent, LegsFunction> legIntentToFunction;


	void Start () {
		legIntentToFunction = new Dictionary<LegsIntent, LegsFunction>();
		legIntentToFunction.Add(LegsIntent.NONE, MovementUtility.relax);
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
		
		legIntentSetter = manualLegs;
		if(!fullManual) {
			legIntentSetter = autoIntent;
			if(tryInputs) {
				legIntentSetter = getIntentFromInputs;
			}
			StartCoroutine(doLegs());
		}
		StartCoroutine(legIntentSetter());
	}
	public IEnumerator autoIntent() {
		yield return null;
		while(true) {
			if(left) {
				legOne.footAdvanceFactor = -1f;
				legTwo.footAdvanceFactor = -1f;
			}
			currentLegsIntent = LegsIntent.STAND;
			if(walking) { currentLegsIntent = LegsIntent.WALK; }
			if(crouching) { currentLegsIntent = LegsIntent.CROUCH; }
			if(jumping) { currentLegsIntent = LegsIntent.JUMP; }
			yield return new WaitForEndOfFrame();
		}
	}
	public IEnumerator getIntentFromInputs ()
	{
		yield return null;
		while(true) {
			float xAxis = Input.GetAxis("Horizontal");
			legOne.footAdvanceFactor = xAxis;
			legTwo.footAdvanceFactor = xAxis;
			if(Input.GetKeyDown(KeyCode.G)) 
			{
				currentGravity = lastGravity;
				lastGravity = Physics2D.gravity;
				Physics2D.gravity = currentGravity;
			}
			if(Input.GetKeyDown(KeyCode.D)) { 
				delayJump = !delayJump; 
				if(delayJump) {
					legIntentToFunction[LegsIntent.JUMP] = MovementUtility.delayJump;
				} else {
					legIntentToFunction[LegsIntent.JUMP] = MovementUtility.jump;
				}
			}

			currentLegsIntent = LegsIntent.STAND;
			if(Input.GetKey(KeyCode.LeftControl)) { currentLegsIntent = LegsIntent.CROUCH; } else
			if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick2Button0)) { currentLegsIntent = LegsIntent.JUMP; } else
			if(!(Mathf.Approximately(xAxis, 0f))) { currentLegsIntent = LegsIntent.WALK; }
			yield return null;
		}
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
			currentLegs = legIntentToFunction[currentLegsIntent];
			yield return StartCoroutine(currentLegs(legOne, legTwo));
			currentLegs = legIntentToFunction[currentLegsIntent];
			yield return StartCoroutine(currentLegs(legTwo, legOne));
		}
	}

	//TODO: This should probably account for inclines at some point, but right now assuming no rotation
	public float getSpeedRatio(float desiredSpeed) {
		float currentSpeed = this.rigidbody2D.velocity.x;
		float difference = desiredSpeed - currentSpeed;
		return difference / desiredSpeed;
	}

}