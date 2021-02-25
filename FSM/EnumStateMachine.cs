using System.Collections;
using System.Collections.Generic;
using System;

namespace Unitilities.FSM {

	public abstract class EnumStateMachine<TEnum>  
		where TEnum : struct, IComparable, IFormattable {
		class StateMethodCache {
			public Action EnterState;
			public Action ExitState;
		}

		protected float elapsedTimeInState = 0f;
		protected TEnum previousState;
		Dictionary<TEnum, StateMethodCache> _stateCache;
		StateMethodCache _stateMethods;

		TEnum _currentState;

		protected TEnum CurrentState {
			get => _currentState;
			set {
				// dont change to the current state
				if (_stateCache.Comparer.Equals(_currentState, value))
					return;

				// swap previous/current
				previousState = _currentState;
				_currentState = value;

				// exit the state, fetch the next cached state methods then enter that state
				if (_stateMethods.ExitState != null)
					_stateMethods.ExitState();

				elapsedTimeInState = 0f;
				_stateMethods = _stateCache[_currentState];

				if (_stateMethods.EnterState != null)
					_stateMethods.EnterState();
			}
		}

		protected TEnum InitialState {
			set {
				_currentState = value;
				_stateMethods = _stateCache[_currentState];

				if (_stateMethods.EnterState != null)
					_stateMethods.EnterState();
			}
		}

		public EnumStateMachine(IEqualityComparer<TEnum> customComparer = null) {
			_stateCache = new Dictionary<TEnum, StateMethodCache>(customComparer);

			// cache all of our state methods
			var enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));
			foreach (var e in enumValues)
				ConfigureAndCacheState(e);
		}

		void ConfigureAndCacheState(TEnum stateEnum) {
			var stateName = stateEnum.ToString();

			var state = new StateMethodCache();
			state.EnterState = GetDelegateForMethod(stateName + "_Enter");
			state.ExitState = GetDelegateForMethod(stateName + "_Exit");

			_stateCache[stateEnum] = state;
		}

		Action GetDelegateForMethod(string methodName) {
			var methodInfo = ReflectionUtils.GetMethodInfo(this, methodName);
			if (methodInfo != null)
				return ReflectionUtils.CreateDelegate<Action>(this, methodInfo);

			return null;
		}
	}
}