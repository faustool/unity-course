using UnityEngine;
using System.Collections;

public class NumberWizard : MonoBehaviour {
	private int max = 1000;
	private int min = 1;
	private int currentGuess = 500;

	// Use this for initialization
	void Start () {
		max = 1000;
		min = 1;
		currentGuess = 500;
		print("=================================================");
		print("Welcome to Number Wizard");
		print("Pick a number in your head, but don't tell me");

		print("The highest number you can pick is " + (max - 1));
		print("The lowest number you can pick is " + min);

		checkAnswer();
		max += 1;
	}

	void checkAnswer() {
		currentGuess = (max + min) / 2;
		print("Did you pick " + currentGuess + "?");
		print("Press RETURN to confirm or the Up arrow for higher, Down for lower");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			min = currentGuess;
			checkAnswer();
		} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			max = currentGuess;
			checkAnswer();
		} else if (Input.GetKeyDown(KeyCode.Return)) {
			print("I won!");
			Start();
		}
	}
}
