using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mechanism : MonoBehaviour
{
	public Mechanism context;
	public ForceMode forceMode;
	public ForceMode angularForceMode;

	public void applyAngularForce (Rigidbody toBody, Vector3 desiredTorque)
	{
		toBody.AddTorque(desiredTorque, this.angularForceMode);
	}
	public void applyLinearForce (Rigidbody toBody, Vector3 desiredForce)
	{
		toBody.AddForce (desiredForce, this.forceMode);
	}
}

