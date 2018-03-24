using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextController : MonoBehaviour {

	public Text text;
	private StateMachine stateMachine;

	// Use this for initialization
	void Start () {
		stateMachine = new StateMachine(
			new State("You are in a prision cell, and you need to escape. There are some dirty sheets on the bed, a mirror on the wall, and the door is locked from the outside.",
				"Press S to view Sheets, M to view Mirror and L to view Lock.",
				new Action(KeyCode.M, new State("This is a very simple mirror.", "Press T to take the mirror or R to return.", 
					ReturnAction.Instance,
					new Action(KeyCode.T, new State("You found a key attached to the back of the mirror!", "Press S to look at the sheets again, or L to try the key in the locker",
						new Action(KeyCode.S, new State("There's nothing in the sheets.", "Press R to return.", ReturnAction.Instance)),
						new Action(KeyCode.L, new State("Congratulations, you unlock thee door!", "Press O to open it, or R to return to your cell.",
							ReturnAction.Instance, 
							new Action(KeyCode.O, new State("You're free of the cell!")))
						))
					))
				),
				new Action(KeyCode.S, new State("There's nothing in the sheets.", "Press R to return.", ReturnAction.Instance)),
				new Action(KeyCode.L, new State("This door is still locked.", "Press R to return.", ReturnAction.Instance)))
		);
		printCurrentState();
	}

	void printCurrentState() {
		text.text = stateMachine.Current.Description + "\n" + stateMachine.Current.Movements;
		print("Started");
	}
	
	// Update is called once per frame
	void Update () {
		foreach(KeyValuePair<KeyCode, State> entry in stateMachine.Current.NextStates)
		{
			if (Input.GetKeyDown(entry.Key)) {
				stateMachine.Move(entry.Key);
				printCurrentState();
				break;
			}
		}
	}
}

public class StateMachine {
	private State root;
	private State current;
	public StateMachine(State root) {
		this.root = root;
		this.current = root;
	}
	public void Move(KeyCode key) {
		State next = current.NextStates[key];
		if (next.GetType() == typeof(ReturnState)) {
			current = current.Source;
		} else if (next != null) {
			current = next;
		}
	}
	public State Current {
		get {
			return current;
		}
	}
}

public class Action {
	private KeyCode key;
	private State state;
	public KeyCode Key {
		get {
			return key;
		} 
	}
	public State State {
		get {
			return state;
		}
	}

	public Action(KeyCode key, State state) {
		this.key = key;
		this.state = state;
	}
}

public sealed class ReturnAction : Action
{
	public static readonly Action Instance = new ReturnAction();
    private ReturnAction() : base(KeyCode.R, ReturnState.Instance)
    {
    }
}

public class State {
    protected Dictionary<KeyCode, State> nextStates = new Dictionary<KeyCode, State>();
	private string description;
	private string movements;
	private State source;
	public State(string description, string movements, params Action[] stateKeys) {
		this.description = description;
		this.movements = movements;
		foreach(Action sk in stateKeys) {
			nextStates[sk.Key] = sk.State;
			sk.State.source = this;
		}
	}
	public State(string description) {
		this.description = description;
		this.movements = "";
	}
	public Dictionary<KeyCode, State> NextStates {
		get {
			return this.nextStates;
		}
	}
	public string Description {
		get {
			return description;
		}
	}
	public string Movements {
		get {
			return movements;
		}
	}
	public State Source {
		get {
			return source;
		}
	}
}

public sealed class ReturnState : State
{
	public static readonly ReturnState Instance = new ReturnState();
    private ReturnState() : base("")
    {
    }
}
