using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIInput))]
public class ConsoleInput : MonoBehaviour
{
	public UITextList textList;

	private UIInput cli;

	void Start()
	{
		cli = GetComponent<UIInput>();
	}

	void Update()
	{
		cli.selected = true;
	}

	void OnSubmit()
	{
		textList.Add(cli.text);
		if (cli.text == "exit") {
			GameObject.Find("Trigger").GetComponent<ActivateConsole>().ExitConsole();
		}
		cli.text = "";
	}
}
