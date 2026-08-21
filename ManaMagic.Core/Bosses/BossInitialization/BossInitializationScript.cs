using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable enable

namespace ManaMagic.Core.Bosses.BossInitialization
{
    [DebuggerDisplay("Count: {Commands.Count}")]
    public sealed class BossInitializationScript : IEnumerable<BossInitializationCommand>
    {
        private readonly List<BossInitializationCommand> commands;

        /// <summary>
        /// Gets a read-only list of <see cref="BossInitializationCommand"/>s in this initialization script.
        /// </summary>
        public IReadOnlyList<BossInitializationCommand> Commands { get { return this.commands; } }

        /// <summary>
        /// Gets the <see cref="BossInitializationCommand"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="BossInitializationCommand"/>.</param>
        /// <returns></returns>
        public BossInitializationCommand this[int index] { get { return this.commands[index]; } }

        /// <summary>
        /// Create a new instance of the <see cref="BossInitializationScript"/> class.
        /// </summary>
        public BossInitializationScript()
        {
            this.commands = new List<BossInitializationCommand>();
        }

        /// <summary>
        /// Create a new instance of the <see cref="BossInitializationScript"/> class with the specified command list.
        /// </summary>
        /// <param name="commands">A list of <see cref="BossInitializationCommand"/>s that represent the initialization script.</param>
        public BossInitializationScript(List<BossInitializationCommand> commands)
        {
            this.commands = commands;
        }

        /// <inheritdoc/>
        public IEnumerator<BossInitializationCommand> GetEnumerator()
        {
            return this.commands.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}