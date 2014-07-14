using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//These are various types of controls/commands that the player might want
//might want to break these up into nouns and verbs (so player/unit/box are nouns, select/issue-order/move are verbs, with preposition-likes or something to allow chaining)

public class InputInterpreter : MonoBehaviour 
{
	public Dictionary<KeyCode, ControllerIntent> keyMapping = new Dictionary<KeyCode, ControllerIntent>();
	public List<SwitchInputMapping> defaultKeymap;
	public bool DEBUG_MODE = false;
	public bool GAME_IS_RUNNING = true;
	public IntentPipe intentSink;

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
 	defaultKeymap.ForEach(mapping => keyMapping.Add(mapping.input,mapping.output));
  StartCoroutine(UpdateIntentsFromInputs());
	}

	IEnumerator UpdateIntentsFromInputs ()
	{
		while(GAME_IS_RUNNING) {
			yield return new WaitForFixedUpdate();
   List<ControllerIntent> newIntents = new List<ControllerIntent>();

   foreach (var keycode in keyMapping.Keys) {
    if(Input.GetKey(keycode) && GUIUtility.hotControl == 0) { //TODO: implement a system for differentiating controls based on down vs held vs up, and also for capturing optional relevant positional data
     if(DEBUG_MODE) { Debug.Log("Detected input: " + keycode + ", maps to: " + keyMapping[keycode]); }
     newIntents.Add(keyMapping[keycode]);
    }
   }
			intentSink.intents = newIntents;
		}
		yield return null;
	}
}
