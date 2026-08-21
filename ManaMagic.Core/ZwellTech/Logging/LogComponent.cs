namespace ZwellTech.Logging
{
    /// <summary>
    /// Represents the component that generates a log message.
    /// </summary>
    public enum LogComponent
    {
        // General
        /// <summary>
        /// The General component.
        /// </summary>
        General = 0,
        /// <summary>
        /// The Initialization component.
        /// </summary>
        Initialization,

        // Debugging
        /// <summary>
        /// The Debug component.
        /// </summary>
        Debug,
        /// <summary>
        /// The Metrics component.
        /// </summary>
        Metrics,

        // Execution
        /// <summary>
        /// The Async component.
        /// </summary>
        Async,
        /// <summary>
        /// The Thread component.
        /// </summary>
        Thread,
        /// <summary>
        /// The Memory component.
        /// </summary>
        Memory,
        /// <summary>
        /// The Graphics component.
        /// </summary>
        Graphics,
        /// <summary>
        /// The File System component.
        /// </summary>
        FileSystem,

        // Formats
        /// <summary>
        /// The Html component.
        /// </summary>
        Html,
        /// <summary>
        /// The Xml component.
        /// </summary>
        Xml,
        /// <summary>
        /// The Json component.
        /// </summary>
        Json,

        // APIs
        /// <summary>
        /// The HtmlParser component.
        /// </summary>
        HtmlParser,
        /// <summary>
        /// The NeuralNetwork component.
        /// </summary>
        NeuralNetwork,
        /// <summary>
        /// The RomReader component.
        /// </summary>
        RomReader,
        /// <summary>
        /// The Compression component.
        /// </summary>
        Compression,

        // Networking
        /// <summary>
        /// The Internet component.
        /// </summary>
        Internet,
    }
}
