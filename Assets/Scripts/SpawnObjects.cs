using UnityEngine;
using System.Collections;

public class SpawnObjects : MonoBehaviour
{
	public GameObject spawnObject;
	public int numberOfObjects = 5;
	public Vector3 spawnArea = new Vector3(1, 1, 1);

	void Start ()
	{
		float xMin = transform.position.x - (spawnArea.x / 2);
		float xMax = transform.position.x + (spawnArea.x / 2);
		float yMin = transform.position.y - (spawnArea.y / 2);
		float yMax = transform.position.y + (spawnArea.y / 2);
		float zMin = transform.position.z - (spawnArea.z / 2);
		float zMax = transform.position.z + (spawnArea.z / 2);

		for (int i = 0; i < numberOfObjects; i++) {
			Vector3 pos = new Vector3(
				Random.Range(xMin, xMax),
				Random.Range(yMin, yMax),
				Random.Range(zMin, zMax)
			);
			Instantiate(spawnObject, pos, Quaternion.identity);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, spawnArea);
	}
}
