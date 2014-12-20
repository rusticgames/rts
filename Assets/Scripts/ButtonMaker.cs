using UnityEngine;
using System.Collections;

public class ButtonMaker : MonoBehaviour {

	public GameObject panel;
	public GameObject buttonPrefab;
	public RMaterialManager manager;
	public RMaterialTemplate template;
	public RMaterial objectToModify;
	
	void Start () {
		manager.templates.ForEach(t => {
			GameObject btn = (GameObject)GameObject.Instantiate(buttonPrefab);
			btn.transform.SetParent(panel.transform, false);
			UnityEngine.UI.Text btnText = btn.GetComponentInChildren<UnityEngine.UI.Text>();
			btnText.text = t.name;
			btn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
				RMaterialManager.ApplyTemplate(objectToModify, t);
				GameObject.Destroy(this.gameObject);
			});
		});
	}
}
