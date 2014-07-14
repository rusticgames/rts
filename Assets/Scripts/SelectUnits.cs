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

public enum SelectionType 
{
	MOUSE_CLICK_OR_DRAG,
	MANUAL,
	TAG
}

public class SelectUnits : MonoBehaviour
{
	//it occurs to me, maybe for the first time, that we could limit selection 
	//to certain cameras. are there interesting possibilities with modal 
	//interactions (spoiler: yes vi-rts)
	public HUD cameraProvider;
	public static bool GAME_IS_RUNNING = true;
	public IntentPipe intentProvider;
	public List<GameObject> selectedUnits = new List<GameObject>();
	public MouseSelector selector;
	public string tagFilter = "Unit";
	public string followFilter = "Unit";
	public string attackFilter = "Unit";
	public bool DEBUG_MODE = false;
 public delegate IEnumerator InterpretationDelegate();
 public Dictionary<ControllerIntent, InterpretationDelegate> interpretationMap = new Dictionary<ControllerIntent, InterpretationDelegate>();

	void Start()
	{
    interpretationMap.Add(ControllerIntent.SPECIFY_POINT, CheckSelect);
    interpretationMap.Add(ControllerIntent.ORDER_MOVE, CheckMove);
    interpretationMap.Add(ControllerIntent.ORDER_ATTACK, CheckFight);
    interpretationMap.Add(ControllerIntent.ORDER_STOP, CheckStop);
    interpretationMap.Add(ControllerIntent.ORDER_JUMP, CheckJump);

    foreach (var intent in interpretationMap.Keys) {
      StartCoroutine(CheckInterpretation(intent));
    }
	}

	IEnumerator CheckInterpretation(ControllerIntent i)
	{
		while(GAME_IS_RUNNING){
			while(! intentProvider.intents.Contains(i) ) { yield return null;	}
			if(DEBUG_MODE) Debug.Log(i);
   yield return StartCoroutine(interpretationMap[i]());
		}
		yield return null;
	}

	IEnumerator CheckSelect(){
  Vector3 clickPoint = Input.mousePosition; //TODO: pull out position and button into new type of intent specifier
  List<GameObject> lo;

  yield return new WaitForSeconds (selector.selectBoxDelay);

		if(!intentProvider.intents.Contains(ControllerIntent.SPECIFY_POINT)) {
    lo = selector.selectPoint(clickPoint);
  }else{
			while(intentProvider.intents.Contains(ControllerIntent.SPECIFY_POINT)) {
      selector.UpdateSelectBox(clickPoint, Input.mousePosition);
      yield return null;
    }
    lo = selector.selectBox(clickPoint, Input.mousePosition);
  }
  if(tagFilter.Length > 0 ) { lo.RemoveAll(x => !x.CompareTag(tagFilter)); }

		if (intentProvider.intents.Contains (ControllerIntent.RETAIN_SELECTION)) {
    lo.AddRange(selectedUnits);
  }

  selectedUnits = lo;
  yield return null;
  }
	
	IEnumerator CheckFight()
	{
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
	
	IEnumerator CheckStop()
	{
    selectedUnits.ForEach(x => x.GetComponent<Mover>().stop());
    yield return null;
	}
	
	IEnumerator CheckJump()
	{
    selectedUnits.ForEach(x => x.GetComponent<Mover>().jump());
    yield return null;
	}

	IEnumerator CheckMove(){
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
	
	void OnDrawGizmos () {
		Gizmos.color = Color.green;
		foreach (var unit in selectedUnits) {
			Gizmos.DrawWireSphere(unit.transform.position, 2.0f);
		}
	}
}
