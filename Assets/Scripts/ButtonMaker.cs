using UnityEngine;
using System.Collections;

public class ButtonMaker : MonoBehaviour {

	public GameObject panel;
	public GameObject buttonPrefab;
	public PhysicalPropertiesManager ppm;
	public PhysicalPropertyTemplate propertiesTemplate;
	public PhysicalProperties objectToModify;
	
	void Start () {
		ppm.templates.ForEach(t => {
			GameObject btn = (GameObject)GameObject.Instantiate(buttonPrefab);
			btn.transform.SetParent(panel.transform, false);
			UnityEngine.UI.Text btnText = btn.GetComponentInChildren<UnityEngine.UI.Text>();
			btnText.text = t.name;
			btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
				PhysicalPropertiesManager.ApplyPhysicalPropertiesTemplate(objectToModify, t);
				GameObject.Destroy(this.gameObject);
			});
		});
	}
}
