using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	public GameObject projectilePrefab;
	public float projectileSpeed = 20.0f;
	public float attackPeriodSeconds = 0.5f;
	public KeyboardTargeter targeter;
	public KeyboardConfiguration keyboardConfig;
	public History history;
	
	void Reset() {
		targeter = this.GetComponent<KeyboardTargeter>();
		keyboardConfig = this.GetComponent<KeyboardConfiguration>();
		history = this.GetComponent<History>();
	}

	void Start () {
		StartCoroutine(Shoot());
	}

	IEnumerator Shoot() {
		while (true) {
			if (Input.GetKeyDown(keyboardConfig.shoot)) {
				Vector3 heading = targeter.newTargetOffset;
				Vector3 startPosition = this.transform.position + heading;
				GameObject o = (GameObject)Instantiate(projectilePrefab, startPosition, this.transform.rotation);
				o.rigidbody.AddForce(heading * projectileSpeed, ForceMode.Force);
				history.addChild(o);
				yield return new WaitForSeconds(attackPeriodSeconds);
			}
			yield return null;
		}
	}
}
