using System.Collections;
using System.Collections.Generic;

#nullable enable

namespace ZwellTech.SuperNintendo.Audio.Akao
{
    public sealed class AkaoSequence : IEnumerable<AkaoMessage>
    {
        public static AkaoSequence Empty { get; } = new AkaoSequence(new List<AkaoMessage>());

        public IReadOnlyList<AkaoMessage> Messages { get; }

        public int Count { get { return this.Messages.Count; } }

        public AkaoMessage this[int index] { get { return this.Messages[index]; } }

        public AkaoSequence(IReadOnlyList<AkaoMessage> messages)
        {
            this.Messages = messages;
        }

        /// <inheritdoc />
        public IEnumerator<AkaoMessage> GetEnumerator()
        {
            return this.Messages.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}