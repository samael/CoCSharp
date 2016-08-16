﻿using CoCSharp.Network;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans command sent in CommandMessage.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Maximum amount of embedded command allowed to be read.
        /// This field is constant.
        /// </summary>
        public const int MaxEmbeddedDepth = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        public Command()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the ID of the <see cref="Command"/>.
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// Reads the <see cref="Command"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="Command"/>.
        /// </param>
        public abstract void ReadCommand(MessageReader reader);

        /// <summary>
        /// Writes the <see cref="Command"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="Command"/>.
        /// </param>
        public abstract void WriteCommand(MessageWriter writer);

        /// <summary>Throws ArgumentNullException if reader is null.</summary>
        protected void ThrowIfReaderNull(MessageReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
        }

        /// <summary>Throws ArgumentNullException if writer is null.</summary>
        protected void ThrowIfWriterNull(MessageWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
        }

        // Depth of this command. This is mostly for checking embedded command recursion.
        internal int Depth { get; set; }
    }
}