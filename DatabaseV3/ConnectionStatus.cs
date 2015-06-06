namespace DatabaseV3
{
    /// <summary>
    /// Represents the status of a connection.
    /// </summary>
    public enum ConnectionStatus
    {
        /// <summary>
        /// The connection is being created and confirmed.
        /// </summary>
        Creating,

        /// <summary>
        /// The connection has been established.
        /// </summary>
        Connected,

        /// <summary>
        /// The connection is no longer alive.
        /// </summary>
        Disconnected
    }
}