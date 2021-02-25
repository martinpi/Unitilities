using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unitilities.FSM;

/*
A simple text class for the state machine implementation
*/

public class SimpleStateMachine : MonoBehaviour {

	public enum States {
		Init,
		Normal,
		None
	}

	class InternalStateMachine : EnumStateMachine<States> {

		SimpleStateMachine _context;

		public InternalStateMachine(SimpleStateMachine context) {
			_context = context;
			InitialState = States.Init;
		}
		public void SetState(States state) {
			CurrentState = state;
		}

		public void Init_Enter() {
			Debug.Log("Enter Init");
		}
		public void Init_Exit() {
			Debug.Log("Exit Init");
		}
		public void Normal_Enter() {
			Debug.Log("Enter Normal");
		}
		public void Normal_Exit() {
			Debug.Log("Exit Normal");
		}

	}

	private InternalStateMachine _machine;

	private void Awake() {
		_machine = new InternalStateMachine(this);
		_machine.SetState(States.Normal);
		_machine.SetState(States.Init);
	}

}
