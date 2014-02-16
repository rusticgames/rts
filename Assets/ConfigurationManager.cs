using UnityEngine;
using System.Collections;

public class ConfigurationManager : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
				Application.runInBackground = true;
				Debug.Log (Application.loadedLevelName);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
