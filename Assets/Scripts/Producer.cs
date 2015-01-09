using UnityEngine;
using System.Collections;

[System.Serializable]
public struct ProducerSize {
	public float radius;
	public float height;
	
	public float getSurfaceArea ()
	{
		return 2 * (Mathf.PI * radius * height + Mathf.PI * radius * radius);
	}
	
	public float getVolume ()
	{
		return Mathf.PI * radius * radius * height;
	}
}

[System.Serializable]
public struct ProducerData {
	public float energy;
	public Color color;
	public float seedCount;
	public ProducerSize size;
	
}

public class Producer : MonoBehaviour {
	public Environment environment;
	public float horizontalGrowthRate = 1.01f;
	public float verticalGrowthRate = 1.001f;
	public float growthInterval = 0.2f;
	public Color deathColor;
	public float upkeepFactor = 1f;
	public float growthFactor = 0.5f;
	public GameObject seedTemplate;
	public float seedCreateCost = 1f;
	public ProducerData initialData;
	public ProducerData currentData;

	void Start () {
		this.currentData = this.initialData;
		StartCoroutine(Live());
	}

	public void refresh() {
		this.transform.localScale = new Vector3(currentData.size.radius, currentData.size.height, currentData.size.radius);
		this.renderer.material.color = currentData.color;
	}
	
	float GetGrowthCost (ProducerSize oldSize, ProducerSize newSize) {
		return (newSize.getVolume() - oldSize.getVolume()) * growthFactor;
	}
	
	float GetUpkeepCost (ProducerSize size) {
		return size.getVolume() * upkeepFactor;
	}
	
	ProducerSize GetDesiredSize () {
		ProducerSize desiredSize = currentData.size;
		desiredSize.radius *= horizontalGrowthRate;
		desiredSize.height *= verticalGrowthRate;
		return desiredSize;
	}

	public void SeedAction (SproutActionData d) {
		Debug.Log ("SeedAction");
		Instantiate(this.gameObject, d.seed.transform.position, d.seed.transform.rotation);
		Object.Destroy(d.seed);
	}

	float GetIntakeAmount ()
	{
		return environment.ReleaseEnergy (currentData.size.getSurfaceArea ());
	}

	float GetReproduceCost ()
	{
		return (this.initialData.energy + seedCreateCost);
	}

	static void drawMeasurement (Color color, float value, float baseline, Vector3 position, int number)
	{
		Gizmos.color = color;
		Vector3 drawSize = Vector3.one / 10f;
		drawSize.y = value / baseline;
		Vector3 drawPosition = position;
		drawPosition.x += (0.25f * number);
		drawPosition.y += 0.5f + (drawSize.y / 2);
		Gizmos.DrawCube (drawPosition, drawSize);
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.black;
		Vector3 drawSize = new Vector3(6 * .25f, 0.00001f, .1f);
		Vector3 drawPosition = this.transform.position;
		drawPosition.y += 0.5f;
		Gizmos.DrawCube (drawPosition, drawSize);
		Gizmos.color = Color.white;
		drawPosition.y += 1f;
		Gizmos.DrawCube (drawPosition, drawSize);
		int pos = -2;
		drawMeasurement (Color.green, this.currentData.energy, this.initialData.energy, this.transform.position, pos++);
		drawMeasurement (Color.blue, this.currentData.seedCount, 10f, this.transform.position, pos++);
		drawMeasurement (Color.red, GetIntakeAmount(), this.initialData.energy, this.transform.position, pos++);
		drawMeasurement (Color.yellow, GetUpkeepCost(this.currentData.size), this.initialData.energy, this.transform.position, pos++);
		drawMeasurement (Color.gray, GetIntakeAmount(), GetUpkeepCost(this.currentData.size), this.transform.position, pos++);
		drawMeasurement (Color.white, this.currentData.size.getSurfaceArea(), this.currentData.size.getVolume(), this.transform.position, pos++);
	}

	void Reproduce ()
	{
		this.currentData.energy -= GetReproduceCost ();
		this.currentData.seedCount++;
	}

	void Grow ()
	{
		ProducerSize oldSize = this.currentData.size;
		this.currentData.size = GetDesiredSize();
		this.currentData.energy -= GetGrowthCost (oldSize, this.currentData.size);
	}

	IEnumerator Live () {
		while (this.currentData.energy > 0) {
			Debug.Log ("starting energy this frame: " + this.currentData.energy);
			Debug.Log ("intake: " + GetIntakeAmount ().ToString());
			this.currentData.energy += GetIntakeAmount ();
			Grow ();
			this.currentData.energy -= GetUpkeepCost(this.currentData.size);
		 Reproduce ();

			Debug.Log ("upkeep: -" + GetUpkeepCost(this.currentData.size).ToString() + ", reproduce: -" + GetReproduceCost ().ToString());
			Debug.Log ("new energy: " + this.currentData.energy);
			refresh();
			yield return new WaitForSeconds(growthInterval);
		}

		for (int i = 0; i < currentData.seedCount; i++) {
			GameObject o = (GameObject)Instantiate(seedTemplate, transform.position, transform.rotation);
			o.gameObject.GetComponent<Seed>().sproutAction.AddListener(SeedAction);
		}

		this.currentData.color = deathColor;
		refresh();
	}
}
