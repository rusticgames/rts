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
	RETAIN_SELECTION,
	ORDER_STOP
}

public class SelectUnits : MonoBehaviour
{
	//it occurs to me, maybe for the first time, that we could limit selection 
	//to certain cameras. are there interesting possibilities with modal 
	//interactions (spoiler: yes vi-rts)
	public HUD cameraProvider;
	public Dictionary<KeyCode, ControllerIntent> keyMapping = new Dictionary<KeyCode, ControllerIntent>();
	public const bool GAME_IS_RUNNING = true;
	public List<GameObject> selectedUnits = new List<GameObject>();
	public List<ControllerIntent> intents = new List<ControllerIntent>();
	public MouseSelector selector;
	public string tagFilter = "Unit";
	public string followFilter = "Unit";
	public string attackFilter = "Unit";
	public bool DEBUG_MODE = false;

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
		StartCoroutine(CheckStop());
		StartCoroutine(CheckJump());

		defaultKeymap.ForEach(mapping => keyMapping.Add(mapping.input,mapping.output));
	}

	IEnumerator UpdateIntentsFromInputs ()
	{
		while(GAME_IS_RUNNING) {
			yield return new WaitForFixedUpdate();
			List<ControllerIntent> newIntents = new List<ControllerIntent>();

			foreach (var keycode in keyMapping.Keys) {
				if(Input.GetKey(keycode) && GUIUtility.hotControl == 0) { //TODO: implement a system for differentiating controls based on down vs held vs up, and also for capturing optional relevant positional data
					if(DEBUG_MODE) {
						Debug.Log(keycode);
						Debug.Log(keyMapping[keycode]);
					}
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
			List<GameObject> lo;

			yield return new WaitForSeconds (selector.selectBoxDelay);

			if(!intents.Contains(ControllerIntent.SPECIFY_POINT)) {
				lo = selector.selectPoint(clickPoint);
			}else{
				while(intents.Contains(ControllerIntent.SPECIFY_POINT)) {
					selector.UpdateSelectBox(clickPoint, Input.mousePosition);
					yield return null;
				}
				lo = selector.selectBox(clickPoint, Input.mousePosition);
			}
			if(tagFilter.Length > 0 ) { lo.RemoveAll(x => !x.CompareTag(tagFilter)); }
			
			if (intents.Contains (ControllerIntent.RETAIN_SELECTION)) {
				lo.AddRange(selectedUnits);
			}

			selectedUnits = lo;
			yield return null;
		}
	}
	
	IEnumerator CheckFight()
	{
		while(GAME_IS_RUNNING){
			while(! intents.Contains(ControllerIntent.ORDER_ATTACK) ) { yield return null;	}
			
			HUD.ScreenPointToWorldInfo i = cameraProvider.getWorldInfoAtScreenPoint(Input.mousePosition);
			if(i.isValid)
			{
				if(attackFilter.Length == 0 || i.objectAtPoint.CompareTag(attackFilter))
				{
					selectedUnits.ForEach(x => x.GetComponent<Mover>().attack(i.objectAtPoint));
				}
			}
			yield return null;
		}
	}
	
	IEnumerator CheckStop()
	{
		while(GAME_IS_RUNNING){
			while(! intents.Contains(ControllerIntent.ORDER_STOP) ) { yield return null;	}
			selectedUnits.ForEach(x => x.GetComponent<Mover>().stop());
			yield return null;
		}
	}
	
	IEnumerator CheckJump()
	{
		while(GAME_IS_RUNNING){
			while(! intents.Contains(ControllerIntent.ORDER_JUMP) ) { yield return null;	}
			selectedUnits.ForEach(x => x.GetComponent<Mover>().jump());
			yield return null;
		}
	}

	IEnumerator CheckMove(){
		while(GAME_IS_RUNNING){
			while(! intents.Contains(ControllerIntent.ORDER_MOVE) ) { yield return null;	}

			HUD.ScreenPointToWorldInfo i = cameraProvider.getWorldInfoAtScreenPoint(Input.mousePosition);
			if(i.isValid)
			{
				if(followFilter.Length == 0 || i.objectAtPoint.CompareTag(followFilter))
				{
					selectedUnits.ForEach(x => x.GetComponent<Mover>().follow(i.objectAtPoint));
				} else {
					selectedUnits.ForEach(x => x.GetComponent<Mover>().moveTo(i.worldPoint));
				}
			}
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