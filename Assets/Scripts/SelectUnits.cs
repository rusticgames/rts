using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//These are various types of controls/commands that the player might want
//might want to break these up into nouns and verbs (so player/unit/box are nouns, select/issue-order/move are verbs, with preposition-likes or something to allow chaining)
public enum ControllerIntent
{
	SPECIFY_POINT,
	SPECIFY_BOX,
	SPECIFY_CIRCLE,
	SPECIFY_UNIT,
	ORDER_MOVE,
	ORDER_FOLLOW,
	ORDER_ATTACK,
	ORDER_JUMP,
	RETAIN_SELECTION
}

public class SelectUnits : MonoBehaviour
{
	//it occurs to me, maybe for the first time, that we could limit selection 
	//to certain cameras. are there interesting possibilities with modal 
	//interactions (spoiler: yes vi-rts)
	public HUD cameraProvider;
	private ScreenToWorldMapper mouseChecker = new ScreenToWorldMapper();
	public Dictionary<KeyCode, ControllerIntent> keyMapping = new Dictionary<KeyCode, ControllerIntent>();
	private const bool GAME_IS_RUNNING = true;
	public HashSet<GameObject> selectedUnits = new HashSet<GameObject>();
	public HashSet<ControllerIntent> intents = new HashSet<ControllerIntent>();
	public MouseSelector selector;

	public List<SwitchInputMapping> defaultKeymap;

	public void AddInputMapping() {
		defaultKeymap.Add(new SwitchInputMapping());
	}

	[System.Serializable]
	public class SwitchInputMapping {
		public KeyCode input;
		public ControllerIntent output;
	}

	void Start()
	{
		StartCoroutine(UpdateIntentsFromInputs());
		StartCoroutine(CheckSelect());
		StartCoroutine(CheckMove());
		StartCoroutine(CheckFight());

		foreach (var mapping in defaultKeymap) {
			keyMapping.Add(mapping.input,mapping.output);
		}
	}

	IEnumerator UpdateIntentsFromInputs ()
	{
		while(GAME_IS_RUNNING) {
			yield return new WaitForFixedUpdate();
			HashSet<ControllerIntent> newIntents = new HashSet<ControllerIntent>();
			foreach (var keycode in keyMapping.Keys) {
				if(Input.GetKey(keycode) && GUIUtility.hotControl == 0) { //TODO: implement a system for differentiating controls based on down vs held vs up, and also for capturing optional relevant positional data
					Debug.Log(keycode.ToString());
					newIntents.Add(keyMapping[keycode]);
				}
			}
			intents = newIntents;
			yield return null;
		}
	}

	IEnumerator CheckSelect(){
		while(GAME_IS_RUNNING){
			while(! intents.Contains(ControllerIntent.SPECIFY_POINT)) {
				yield return null;
			}
			
			Vector3 clickPoint = Input.mousePosition; //TODO: pull out position and button into new type of intent specifier
			SelectionType s = SelectionType.MOUSE_POINT;

			yield return new WaitForSeconds (selector.selectBoxDelay);

			if(intents.Contains(ControllerIntent.SPECIFY_POINT)) {
				s = SelectionType.MOUSE_BOX;
			}
			
			yield return StartCoroutine(selector.select(clickPoint, s));
			HashSet<GameObject> newSelection = new HashSet<GameObject>(selector.selection);
			
			if (intents.Contains (ControllerIntent.RETAIN_SELECTION)) {
				newSelection.UnionWith (selectedUnits);
			}

			selectedUnits = newSelection;
			yield return null;
		}
	}
	
	IEnumerator CheckFight(){	yield return null; }

	static void moveOrFollow (Mover m, GameObject lastSelection)
	{
		if (lastSelection.tag == "Unit") {
			Debug.Log("4");
			m.follow (lastSelection);
			return;
		}
		m.moveTo (lastSelection.transform.position);
		Debug.Log("5");
	}

	IEnumerator CheckMove(){
		while(GAME_IS_RUNNING){
			while(! intents.Contains(ControllerIntent.ORDER_MOVE) ) { yield return null;	}

			selector.select(Input.mousePosition, SelectionType.MOUSE_POINT);
			Debug.Log("1");
			if(selector.selection.Count == 1)
			{
				GameObject lastSelection = selector.selection[0];
				Debug.Log("2");
				foreach (GameObject unit in selectedUnits) { 
					moveOrFollow(unit.GetComponent<Mover>(), lastSelection);
				}			
			}
			Debug.Log("6");
			yield return null;
		}
	}
	
	void OnDrawGizmos () {
		Gizmos.color = Color.green;
		foreach (var unit in selectedUnits) {
			Gizmos.DrawWireSphere(unit.transform.position, 2.0f);
		}
	}
}