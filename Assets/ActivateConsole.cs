using UnityEngine;
using System.Collections;

public class ActivateConsole : MonoBehaviour
{

	private UIInput cli;
	private Camera consoleCamera;
	private MonoBehaviour motor;
	private GameObject player;
	private Vector3 lastPlayerPosition;

	void Start()
	{
		player = GameObject.Find("First Person Controller");
		cli = GameObject.Find("Console Input").GetComponent<UIInput>();
		cli.enabled = false;
		consoleCamera = Camera.allCameras[0];
		motor = GameObject.Find("First Person Controller").GetComponent<CharacterMotor>();
	}

	void OnTriggerStay(Collider other)
	{
		if (Input.GetKeyDown(KeyCode.E) && !cli.enabled) {
			consoleCamera.depth = 2;
			lastPlayerPosition = player.transform.position;
			player.transform.position = new Vector3(0, -0.6f, 5.2f);
			Toggle();
		} else if (Input.GetKeyDown(KeyCode.Escape) && cli.enabled) {
			ExitConsole();
		}
	}

	public void ExitConsole() {
		consoleCamera.depth = 0;
		player.transform.position = lastPlayerPosition;
		Toggle();
		cli.text = "% ";
	}

	void Toggle()
	{
		cli.enabled = !cli.enabled;
		cli.selected = !cli.selected;
		motor.enabled = !motor.enabled;
	}
}
