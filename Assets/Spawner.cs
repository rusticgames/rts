using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject spawnType;
	public void spawn ()
	{
		GameObject spawned = Instantiate(spawnType) as GameObject;
		Vector3 pos = this.transform.position;
		pos.y = pos.y + 1.0f;
		spawned.transform.position = pos;
	}
}
