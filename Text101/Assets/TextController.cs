using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextController : MonoBehaviour {

	public Text text;

	private State currentState = new CellState();

	// Use this for initialization
	void Start () {
		printCurrentState();
	}

	void printCurrentState() {
		text.text = currentState.getDescription() + "\n" + currentState.getMovements();
		print("Started");
	}
	
	// Update is called once per frame
	void Update () {
		foreach(KeyValuePair<KeyCode, State> entry in currentState.NextStates)
		{
			if (Input.GetKeyDown(entry.Key)) {
				currentState = entry.Value;
				printCurrentState();
			}
		}
	}
}

public abstract class State {
	private readonly State source;
	public State(State source) {
		this.source = source;
	}
	public State() {
		this.source = null;
	}
    protected Dictionary<KeyCode, State> nextStates = new Dictionary<KeyCode, State>();
	public Dictionary<KeyCode, State> NextStates {
		get {
			return this.nextStates;
		}
	}
    public abstract string getDescription();
	public abstract string getMovements();
}

public class CellState : State
{
	public CellState() {
		this.nextStates[KeyCode.M] = new MirrorState(this);
		this.nextStates[KeyCode.L] = new LockedState(this);
		this.nextStates[KeyCode.S] = new SheetsState(this);
	}
    public override string getDescription()
    {
        return "You are in a prision cell, and you need to escape. There are some dirty sheets on the bed, a mirror on the wall, and the door is locked from the outside.";
    }

    public override string getMovements()
    {
        return "Press S to view Sheets, M to view Mirror and L to view Lock.";
    }
}

public class MirrorState : State
{
    public MirrorState(State source) : base(source){
		this.nextStates[KeyCode.R] = source;
	}
    public override string getDescription()
    {
        return "This is a very simple mirror.";
    }
    public override string getMovements()
    {
        return "Press T to take the mirror or R to return.";
    }
}

public class LockedState : State
{
    public LockedState(State source) : base(source){
		this.nextStates[KeyCode.R] = source;
	}
    public override string getDescription()
    {
        return "This door is still locked.";
    }
    public override string getMovements()
    {
        return "Press R to return.";
    }
}

public class SheetsState : State {
	public SheetsState(State source): base(source) {
		this.nextStates[KeyCode.R] = source;
	}
    public override string getDescription()
    {
        return "There's nothing in the sheets.";
    }
    public override string getMovements()
    {
        return "Press R to return.";
    }
}