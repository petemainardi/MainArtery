using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace MainArtery.Utilities
{
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    /// <summary>
    /// Wrap a set of functions that must all evaluate to True in order for the whole set to be
    /// considered fulfilled.
    /// <br/>
    /// Includes privately visible metadata for debugging to view whether each individual condition
    /// is currently fulfilled.
    /// <br/><br/>
    /// NOTE:<br/>
    /// Adding non-pure functions as conditions can cause unexpected behavior, as side effects will
    /// be triggered just by checking the condition.
    /// </summary>
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
    public class ConditionSet
    {
        public delegate bool Condition();

        /// =======================================================================================
        /// Fields & Properties
        /// =======================================================================================
        private readonly Dictionary<Condition, Expression<Condition>> _conditions =
            new Dictionary<Condition, Expression<Condition>>();

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used in debugging to inspect conditions")]
        private (bool Fulfilled, Expression<Condition> Expression)[] DebugInspect
            => _conditions.Select(pair => (pair.Key.Invoke(), pair.Value)).ToArray();

        public bool Fulfilled => _conditions.Keys.All(c => c());

        /// =======================================================================================
        /// Constructor
        /// =======================================================================================
        public ConditionSet() { }
        public ConditionSet(params Expression<Condition>[] conditions) : this()
        {
            Add(conditions);
        }

        /// =======================================================================================
        /// Methods
        /// =======================================================================================
        public ConditionSet Add(params Expression<Condition>[] conditions)
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                Condition c = conditions[i].Compile();
                _conditions.Add(c, conditions[i]);
            }
            return this;
        }

        public ConditionSet Add(ConditionSet other)
        {
            Add(other._conditions.Values.ToArray());
            return this;
        }
        /// =======================================================================================
    }
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
}
