using UnityEngine;
using System.Collections;

public class Producer : MonoBehaviour {

	public Environment environment;
	public Vector3 initialSize = new Vector3(1f, 0.1f, 1f);
	public Vector3 currentSize;
	public float horizontalGrowthRate = 1.01f;
	public float verticalGrowthRate = 1.001f;
	public float growthInterval = 0.2f;
	public float energy = 100f;
	public float desiredEnergyAmount = 1f;
	public Color finalColor;
	public float upkeepFactor = 1f;
	public float growthFactor = 0.5f;
	
	void Start () {
		StartCoroutine(Live());
	}

	void Reset () {
		this.transform.localScale = initialSize;
		this.currentSize = initialSize;
	}
	
	float GetGrowthCost (Vector3 oldSize, Vector3 newSize) {
		return (newSize - oldSize).magnitude * growthFactor;
	}
	
	float GetUpkeepCost (Vector3 size) {
		return size.magnitude * upkeepFactor;
	}
	
	Vector3 GetDesiredSize () {
		Vector3 desiredSize = currentSize;
		desiredSize.x *= horizontalGrowthRate;
		desiredSize.y *= verticalGrowthRate;
		desiredSize.z *= horizontalGrowthRate;
		return desiredSize;
	}

	IEnumerator Live () {
		while (energy > 0) {
			energy += environment.ReleaseEnergy(desiredEnergyAmount);
			currentSize = GetDesiredSize();
			energy = energy - (GetUpkeepCost(currentSize) + GetGrowthCost(this.transform.localScale, currentSize));
			this.transform.localScale = currentSize;
			yield return new WaitForSeconds(growthInterval);
		}
		this.gameObject.renderer.material.color = finalColor;
	}


}
