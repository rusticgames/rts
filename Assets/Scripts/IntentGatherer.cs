// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//These are various types of controls/commands that the player might want
//might want to break these up into nouns and verbs (so player/unit/box are nouns, select/issue-order/move are verbs, with preposition-likes or something to allow chaining)

public interface IntentGatherer {
	List<ControllerIntent> getCurrentIntents();
}
