using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Affiliation : MonoBehaviour {
	public Affiliation teamLeader;
	public float drawRadius = 1f;
	public Vector3 drawOffset = new Vector3(0f, 2f, 0f);
	public bool useMyColor = false;
	public Color color;

 
	// Implement this OnDrawGizmos if you want
	// to draw gizmos that are also pickable
	// and always drawn.
	void OnDrawGizmos () {
		Color drawColor = color; 
		
		if (teamLeader != null) {
			drawColor = teamLeader.color;
		}

			Gizmos.color = drawColor;
			Gizmos.DrawWireSphere(this.transform.position + drawOffset, drawRadius);
	}
}
