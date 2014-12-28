using UnityEngine;
using System.Collections;

public class HeightDestroyer : MonoBehaviour {
	
	public float minHeight = -1f;
	
	public void checkHeight(History historyObject, float timestamp) {
		if(historyObject.transform.position.y < minHeight) {
			Object.Destroy(this.gameObject);
		}
	}
}
