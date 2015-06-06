using System;
using System.Net.Sockets;

namespace DatabaseV3
{
    /// <summary>
    /// Represents a network connection.
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="node">The node the connection is to.</param>
        /// <param name="client">The client that is the connection.</param>
        /// <param name="connectionTime">The time the connection was created.</param>
        public Connection(NetworkNode node, TcpClient client, DateTime connectionTime)
        {
            Node = node;
            Client = client;
            ConnectionTime = connectionTime;
            Status = ConnectionStatus.Creating;
        }

        /// <summary>
        /// Gets the client that is the connection.
        /// </summary>
        public TcpClient Client
        {
            get; private set;
        }

        /// <summary>
        /// Gets the time the connection was created.
        /// </summary>
        public DateTime ConnectionTime
        {
            get; private set;
        }

        /// <summary>
        /// Gets the node the connection is to.
        /// </summary>
        public NetworkNode Node
        {
            get; private set;
        }

        /// <summary>
        /// Gets the status of the connection.
        /// </summary>
        public ConnectionStatus Status
        {
            get; private set;
        }

        /// <summary>
        /// Mark the connection as connected.
        /// </summary>
        internal void Connected()
        {
            Status = ConnectionStatus.Connected;
        }

        /// <summary>
        /// Mark the connection as disconnected.
        /// </summary>
        internal void Disconnected()
        {
            Status = ConnectionStatus.Disconnected;
        }
    }
}