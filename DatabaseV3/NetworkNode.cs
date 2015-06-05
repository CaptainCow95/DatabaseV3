using System.Net;

namespace DatabaseV3
{
    /// <summary>
    /// Represents a network node.
    /// </summary>
    public class NetworkNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkNode"/> class.
        /// </summary>
        /// <param name="hostname">The hostname of the node.</param>
        /// <param name="port">The port of the node.</param>
        public NetworkNode(string hostname, int port)
        {
            Hostname = hostname;
            Port = port;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkNode"/> class.
        /// </summary>
        /// <param name="port">The port of the node.</param>
        public NetworkNode(int port)
        {
            Hostname = Dns.GetHostName();
            Port = port;
        }

        /// <summary>
        /// Gets the hostname of the node.
        /// </summary>
        public string Hostname
        {
            get; private set;
        }

        /// <summary>
        /// Gets the port of the node.
        /// </summary>
        public int Port
        {
            get; private set;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            NetworkNode node = obj as NetworkNode;
            return node != null && node.Hostname == Hostname && node.Port == Port;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Hostname + ':' + Port;
        }
    }
}