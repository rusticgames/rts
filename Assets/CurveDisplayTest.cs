using UnityEngine;
using System.Collections;

public class CurveDisplayTest : MonoBehaviour {
	public AnimationCurve distribution;
	public float maxDamage;
	public float minDamage;
	
	public float randomDamage {
		get {
			// Get a random number between 0 and 1
			float x = Random.value;
			
			// Find that value on your distribution curve
			float y = distribution.Evaluate(x);
			
			// Scale it to be between your max and min
			return minDamage + (y * (maxDamage - minDamage));
		}
	}
}
