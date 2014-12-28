using UnityEngine;
using System.Collections;

public class ButtonMaker : MonoBehaviour {

	public GameObject panel;
	public GameObject buttonPrefab;
	public RMaterialManager manager;
	public RMaterial template;
	public RMaterial objectToModify;
	
	void makeButtonsFromMaterial (RMaterialManager.CanonicalRMaterial t)
	{
		GameObject btn = (GameObject)Object.Instantiate (buttonPrefab);
		btn.transform.SetParent (panel.transform, false);
		UnityEngine.UI.Text btnText = btn.GetComponentInChildren<UnityEngine.UI.Text> ();
		btnText.text = t.materialName;
		btn.GetComponent<UnityEngine.UI.Button> ().onClick.AddListener (() => {
			objectToModify.update(t.materialTemplate);
			Object.Destroy (this.gameObject);
		});
	}
	void Start () {
		manager.templates.ForEach (makeButtonsFromMaterial);
	}
}
