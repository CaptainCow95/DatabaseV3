using System;
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
        /// <param name="connectionName">The connection in the format "hostname:port".</param>
        public NetworkNode(string connectionName)
        {
            string[] parts = connectionName.Split(':');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Parameter is not in the format \"hostname:port\".", nameof(connectionName));
            }

            int port;
            if (!int.TryParse(parts[1], out port))
            {
                throw new ArgumentException("Parameter is not in the format \"hostname:port\".", nameof(connectionName));
            }

            Hostname = parts[0];
            if (Hostname.Equals("localhost", StringComparison.InvariantCultureIgnoreCase))
            {
                Hostname = Dns.GetHostName();
            }

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