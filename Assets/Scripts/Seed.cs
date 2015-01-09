using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SproutAction : UnityEngine.Events.UnityEvent<SproutActionData> {}

public class SproutActionData {
	
	public GameObject seed;

	public SproutActionData (GameObject seed) {
		this.seed = seed;
	}
	
}

public class Seed : MonoBehaviour {

	public SproutAction sproutAction = new SproutAction();
	public float sproutInterval = 2.0f;

	void Start () {
		StartCoroutine(Sprout());
	}

	IEnumerator Sprout() {
		yield return new WaitForSeconds(sproutInterval);
		sproutAction.Invoke(new SproutActionData(this.gameObject));
	}
}
