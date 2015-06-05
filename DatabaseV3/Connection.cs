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
        public Connection(NetworkNode node, TcpClient client)
        {
            Node = node;
            Client = client;
        }

        /// <summary>
        /// Gets the client that is the connection.
        /// </summary>
        public TcpClient Client
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
    }
}