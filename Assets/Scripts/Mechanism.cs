using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mechanism : MonoBehaviour
{
	public Mechanism context;
	public ForceMode forceMode;
	public ForceMode angularForceMode;
	
	public void applyAngularForce (Rigidbody toBody, Vector3 desiredTorque, bool relative)
	{
		if(relative) {
			toBody.AddRelativeTorque(desiredTorque, this.angularForceMode);
		}
		else {
			toBody.AddTorque(desiredTorque, this.angularForceMode);
		}
	}
	
	public void applyAngularForceViaQuaternion (Rigidbody toBody, Vector3 desiredTorque)
	{
		toBody.rotation.SetLookRotation(desiredTorque);
	}
	public void applyLinearForce (Rigidbody toBody, Vector3 desiredForce, bool relative)
	{
		if(relative) 
			toBody.AddRelativeForce (desiredForce, this.forceMode);
		else
			toBody.AddForce (desiredForce, this.forceMode);
	}
}

