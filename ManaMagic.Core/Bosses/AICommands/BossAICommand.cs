using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable enable

namespace ManaMagic.Core.Bosses.AICommands
{
    [DebuggerDisplay("Count: {Actions.Count}")]
    public sealed class BossAICommand : IEnumerable<BossAICommandAction>
    {
        private List<BossAICommandAction> actions;

        public IReadOnlyList<BossAICommandAction> Actions { get { return this.actions; } }

        public BossAICommandAction this[int index] { get { return this.actions[index]; } }

        public BossAICommand()
        {
            this.actions = new List<BossAICommandAction>();
        }

        public BossAICommand(List<BossAICommandAction> actions)
        {
            this.actions = actions;
        }

        public void Add(BossAICommandAction action)
        {
            this.actions.Add(action);
        }

        public void Insert(int index, BossAICommandAction action)
        {
            this.actions.Insert(index, action);
        }

        public void Replace(int index, BossAICommandAction action)
        {
            this.actions[index] = action;
        }

        public void Move(int oldIndex, int newIndex)
        {
            BossAICommandAction action = this.actions[oldIndex];
            this.actions.RemoveAt(oldIndex);
            this.actions.Insert(newIndex, action);
        }

        /// <inheritdoc/>
        public IEnumerator<BossAICommandAction> GetEnumerator()
        {
            return this.actions.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}