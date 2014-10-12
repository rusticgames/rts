// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementUtility {
	public static SimpleLeg spawnLeg(GameObject body, Object legPrefab, Object footPrefab, LegData legData, float footXOffset, string nameDecorator) {
		Vector3 spawnPoint = body.transform.position;
		Quaternion rotation = body.transform.rotation;
		SimpleLeg newLeg;

		spawnPoint.y = spawnPoint.y - 0.5f;
		newLeg = ((GameObject)Object.Instantiate(legPrefab, spawnPoint, rotation)).GetComponent<SimpleLeg>();
		newLeg.name = legPrefab.name + " " + nameDecorator + " -- (" + body.name + ")";
		newLeg.legData = legData;
		newLeg.thigh = newLeg.GetComponent<SliderJoint2D>();
		newLeg.thigh.connectedBody = body.rigidbody2D;
		spawnPoint.x = spawnPoint.x + footXOffset;
		spawnPoint.y = spawnPoint.y - .25f;
		newLeg.foot = ((GameObject)Object.Instantiate(footPrefab, spawnPoint, rotation)).GetComponent<SliderJoint2D>();
		newLeg.foot.connectedBody = newLeg.thigh.rigidbody2D;
		newLeg.foot.name = footPrefab.name + " " + nameDecorator  + " -- (" + body.name + ")";
		return newLeg;
	}
	public static IEnumerator stand (SimpleLeg legOne, SimpleLeg legTwo)
	{
		legOne.footAdvanceFactor = 1f;
		legTwo.footAdvanceFactor = 1f;
		legOne.lower();
		legTwo.lower();
		if(SimpleLeg.getXAxisDiff(legOne, legTwo) < 0f) {
			legOne.advance();
			legTwo.advanceOpposed();
		}else{
			legOne.advanceOpposed();
			legTwo.advance();
		}
		yield return new WaitForFixedUpdate();
	}
	public static IEnumerator jump (SimpleLeg legOne, SimpleLeg legTwo)
	{
		legOne.jump();
		legTwo.jump();
		while(!(legOne.isFullyLowered() || legTwo.isFullyLowered())) {
			yield return new WaitForFixedUpdate();
		}
		
		yield return new WaitForFixedUpdate();
	}
	public static IEnumerator delayJump (SimpleLeg legOne, SimpleLeg legTwo)
	{
		legOne.lift();
		legTwo.lift();
		legOne.advance();
		legTwo.advance();
		yield return new WaitForSeconds(0.2f);

		legOne.jump();
		legTwo.jump();
		while(!(legOne.isFullyLowered() || legTwo.isFullyLowered())) {
			yield return new WaitForFixedUpdate();
		}
		
		yield return new WaitForFixedUpdate();
	}
	public static IEnumerator crouch (SimpleLeg legOne, SimpleLeg legTwo)
	{
		legOne.lift();
		legTwo.lift();
		yield return new WaitForFixedUpdate();
	}
	
	public static void checkFeet(SimpleLeg legOne, SimpleLeg legTwo) {
		float heightDiff = SimpleLeg.getYAxisDiff(legOne, legTwo);
		SimpleLeg highLeg = legOne;
		SimpleLeg lowLeg = legTwo;
		
		if(heightDiff < 0f) {
			lowLeg = legOne;
			highLeg = legTwo;
		}
		
		highLeg.advanceOpposed();
		lowLeg.advance();
	}
	
	public static IEnumerator walkOneLeg (SimpleLeg leg1, SimpleLeg leg2) {
		MovementUtility.checkFeet(leg1, leg2);
		leg1.lower();
		leg2.lift();
		while(!leg1.isFullyLowered()) {
			leg1.lower();
			leg2.lift();
			MovementUtility.checkFeet(leg1, leg2);
			yield return new WaitForFixedUpdate();
		}
		yield return null;
	}
}
