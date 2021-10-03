using System;
using System.Collections.Generic;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class StateMachine
    {
        private IState _currentState;

        private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> _currentTransitions = new List<Transition>();
        private List<Transition> _anyTransitions = new List<Transition>();

        private readonly List<Transition> emptyTransitions = new List<Transition>(0);

        public void Execute()
        {
            var transition = GetTransition();

            if (transition != null)
                SetState(transition.To);

            _currentState?.Tick();
        }

        public void SetState(IState state)
        {
            if (state == _currentState)
                return;

            _currentState?.OnExit();
            _currentState = state;

            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);

            if (_currentTransitions == null)
                _currentTransitions = emptyTransitions;

            UnityEngine.Debug.Log(_currentState.GetType());
            _currentState.OnEnter();
        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, condition));
        }

        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            _anyTransitions.Add(new Transition(to, condition));
        }

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            return null;
        }
    }
}