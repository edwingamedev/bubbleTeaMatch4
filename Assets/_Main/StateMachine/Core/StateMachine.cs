using System;
using System.Collections.Generic;

namespace EdwinGameDev.BubbleTeaMatch4
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> currentTransitions = new List<Transition>();
        private List<Transition> anyTransitions = new List<Transition>();

        private readonly List<Transition> emptyTransitions = new List<Transition>(0);

        public void Execute()
        {
            var transition = GetTransition();

            if (transition != null)
                SetState(transition.To);

            CurrentState?.Tick();
        }

        public void SetState(IState state)
        {
            if (state == CurrentState)
                return;

            CurrentState?.OnExit();
            CurrentState = state;

            transitions.TryGetValue(CurrentState.GetType(), out currentTransitions);

            if (currentTransitions == null)
                currentTransitions = emptyTransitions;

            //UnityEngine.Debug.Log(CurrentState.GetType());
            CurrentState.OnEnter();
        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (this.transitions.TryGetValue(from.GetType(), out var _transitions) == false)
            {
                _transitions = new List<Transition>();
                this.transitions[from.GetType()] = _transitions;
            }

            _transitions.Add(new Transition(to, condition));
        }

        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            anyTransitions.Add(new Transition(to, condition));
        }

        private Transition GetTransition()
        {
            foreach (var transition in anyTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            foreach (var transition in currentTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            return null;
        }
    }
}