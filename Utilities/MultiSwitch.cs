﻿using System;

namespace MainArtery.Utilities
{
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    /// <summary>
    /// Track current state from among many possible states, where only one state may be occupied
    /// at a time.
    /// </summary>
    /// <typeparam name="TState">The state representation</typeparam>
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    public class MultiSwitch<TState>
    {
        private TState _state;

        public event Action<TState> StateChanged;


        public MultiSwitch(TState initialState)
        {
            _state = initialState;
        }
        public MultiSwitch() : this(default) { }


        public ConditionSet StateConditions { get; } = new ConditionSet();

        public TState State
        {
            get => _state;
            set
            {
                if (!Equals(_state, value) && StateConditions.Fulfilled)
                {
                    _state = value;
                    StateChanged?.Invoke(_state);
                }
            }
        }
    }
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    /// <summary>
    /// Track current state between two possible states.
    /// </summary>
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    public class ToggleSwitch : MultiSwitch<bool>
    {
        public ToggleSwitch(bool initialState = false) : base(initialState) { }

        public void Toggle() => State = !State;
    }
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
}
