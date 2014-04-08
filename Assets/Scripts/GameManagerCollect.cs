using UnityEngine;
using System.Collections;

public class GameManagerCollect : MonoBehaviour
{
	private int collectablesRemaining = 1;
	private GameObject player1;
	private GameObject player2;
	private int player1Score;
	private int player2Score;
	private bool gameOver = false;
	private GUIStyle guiStyle;

	void Start()
	{
		player1 = GameObject.Find("Player 1");
		player2 = GameObject.Find("Player 2");
	}

	void Update()
	{
		player1Score = player1.GetComponent<Collector>().collection.ToArray().Length;
		player2Score = player2.GetComponent<Collector>().collection.ToArray().Length;

		if (!gameOver) {
			collectablesRemaining = GameObject.FindGameObjectsWithTag("Collectable").Length;
			if (collectablesRemaining == 0) {
				gameOver = true;
			}
		} else if (Input.GetKeyDown(KeyCode.Space)) {
			Application.LoadLevel("2P");
		}
	}

	void OnGUI()
	{
		guiStyle = GUI.skin.GetStyle("Label");
		guiStyle.alignment = TextAnchor.MiddleCenter;

		if (gameOver) {
			string winner = player1Score > player2Score ? "Player 1" : "Player 2";
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 - 40, 100, 20), "- GAME OVER -", guiStyle);
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 - 20, 100, 20), winner + " wins!", guiStyle);
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 + 20, 100, 20), "Press Spacebar", guiStyle);
		}
	}
}
