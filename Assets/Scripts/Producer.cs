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
	public float expectedEnergyIntake;
	public float reproductionPeriod;
	public float horizontalGrowthRate;
}

public class Producer : MonoBehaviour {
	public Environment environment;
	public float growthInterval = 0.2f;
	public Color deathColor;
	public float upkeepFactor = 2f;
	public float growthFactor = 10f;
	public GameObject seedTemplate;
	public float seedCreateCost = 100f;
	public ProducerData initialData;
	public ProducerData currentData;
	public float lastEnergy;
	public float reproduceThreshold = 1f;
	public AnimationCurve growthCurve;
	public float growthBase = 3f;


	void Start () {
		this.currentData = this.initialData;
		this.lastEnergy = this.currentData.energy;
		StartCoroutine(Live());
	}

	public void refresh() {
		this.transform.localScale = new Vector3(currentData.size.radius, currentData.size.height, currentData.size.radius);
		this.renderer.material.color = currentData.color;
		this.lastEnergy = this.currentData.energy;
	}
	
	float GetGrowthCost (ProducerSize oldSize, ProducerSize newSize) {
		return (newSize.getVolume() - oldSize.getVolume()) * growthFactor;
	}
	
	float GetUpkeepCost (ProducerSize size) {
		return size.getVolume() * upkeepFactor;
	}
	
	ProducerSize GetDesiredSize () {
		ProducerSize desiredSize = currentData.size;
		desiredSize.radius *= (1 + (currentData.horizontalGrowthRate / 100f));
		return desiredSize;
	}

	public void SeedAction (SproutActionData d) {
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
		if(this.currentData.horizontalGrowthRate > this.reproduceThreshold) {
			this.currentData.energy -= (GetReproduceCost () / this.currentData.reproductionPeriod);
			this.currentData.seedCount += 1f / this.currentData.reproductionPeriod;
		}
	}
	
	void Grow ()
	{
		ProducerSize oldSize = this.currentData.size;
		this.currentData.size = GetDesiredSize();
		this.currentData.energy -= GetGrowthCost (oldSize, this.currentData.size);
	}

	private float lastEnergyIntake, intakeRatio;
	void Think ()
	{
		lastEnergyIntake = this.currentData.energy - this.lastEnergy;
		intakeRatio = lastEnergyIntake / this.currentData.expectedEnergyIntake;
		this.currentData.horizontalGrowthRate = growthBase * growthCurve.Evaluate(intakeRatio);
	}

	IEnumerator Live () {
		while (this.currentData.energy > 0) {
			this.currentData.energy += GetIntakeAmount ();
			Grow ();

			this.currentData.energy -= GetUpkeepCost(this.currentData.size);
			Reproduce ();

			Think();
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
