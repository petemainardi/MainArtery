using System.Collections.Generic;
using System.Linq;
#if !NET8_0_OR_GREATER
using System.Linq.Expressions;
#endif

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

#if NET8_0_OR_GREATER
        /*
         * Compiling expressions at runtime is slow at scale, so it is ideal to move away from that
         * as the means of providing debug metadata. The built-in DescriptionAttribute provides a
         * clear way to designate a user-defined string as the metadata description of a Condition,
         * however attributes cannot be applied to lambda functions before C#10. To maximize
         * compatibility, the version of the class making use of expressions has been retained for
         * versions of .NET before 8, and the version using attributes will be available for .NET8+.
         */
        /// =======================================================================================
        /// Fields & Properties
        /// =======================================================================================
        private readonly List<Condition> _conditions = [];

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members",
            Justification = "Used in debugging to inspect conditions")]
        private (bool Fulfilled, string Description)[] DebugInspect
            => _conditions.Select(pair => DebugFulfilled(pair)).ToArray();

        public bool Fulfilled => _conditions.All(c => c());

        /// =======================================================================================
        /// Constructor
        /// =======================================================================================
        public ConditionSet() { }
        public ConditionSet(params Condition[] conditions) : this()
        {
            Add(conditions);
        }

        /// =======================================================================================
        /// Methods
        /// =======================================================================================
        public ConditionSet Add(params Condition[] conditions)
        {
            _conditions.AddRange(conditions);
            return this;
        }

        public ConditionSet Add(ConditionSet other)
        {
            _conditions.AddRange(other._conditions);
            return this;
        }

        private static (bool Fulfilled, string Description) DebugFulfilled(Condition c)
        {
            var desc = c.Method.GetCustomAttributes(true).OfType<System.ComponentModel.DescriptionAttribute>().FirstOrDefault();
            return (c.Invoke(), desc?.Description ?? string.Empty);
        }
        /// =======================================================================================
#else
        /// =======================================================================================
        /// Fields & Properties
        /// =======================================================================================
        private readonly Dictionary<Condition, Expression<Condition>> _conditions =
            new Dictionary<Condition, Expression<Condition>>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members",
            Justification = "Used in debugging to inspect conditions")]
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
#endif
    }
    /// ===========================================================================================
    /// |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
    /// ===========================================================================================
}
