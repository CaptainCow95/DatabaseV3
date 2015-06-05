using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace DatabaseV3
{
    /// <summary>
    /// Represents a network.
    /// </summary>
    public class Network : IDisposable
    {
        /// <summary>
        /// A list of the current open connections.
        /// </summary>
        private List<Connection> _connections = new List<Connection>();

        /// <summary>
        /// The lock for the connections list.
        /// </summary>
        private ReaderWriterLockSlim _connectionsLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The nodes to stay connected to.
        /// </summary>
        private List<NetworkNode> _nodesToConnectTo = new List<NetworkNode>();

        /// <summary>
        /// A value indicating whether the network is running.
        /// </summary>
        private bool _running = false;

        /// <summary>
        /// The wait handle to trigger when the network is no longer running.
        /// </summary>
        private ManualResetEvent _runningWaitHandle = new ManualResetEvent(false);

        /// <summary>
        /// A value indicating whether the object has been disposed.
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        public Network()
        {
            Thread reconnectionThread = new Thread(RunReconnection);
            reconnectionThread.Start();
        }

        /// <summary>
        /// Connects to a node.
        /// </summary>
        /// <param name="node">The node to connect to.</param>
        public void Connect(NetworkNode node)
        {
            _connectionsLock.EnterWriteLock();
            _nodesToConnectTo.Add(node);
            ConnectToNode(node);
            _connectionsLock.ExitWriteLock();
        }

        /// <summary>
        /// Disconnects from a node.
        /// </summary>
        /// <param name="node">The node to disconnect from.</param>
        public void Disconnect(NetworkNode node)
        {
            _connectionsLock.EnterWriteLock();
            _nodesToConnectTo.Add(node);
            _connectionsLock.ExitWriteLock();
        }

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Shutdown the network.
        /// </summary>
        public void Shutdown()
        {
            _running = false;
            _runningWaitHandle.Set();
        }

        /// <summary>
        /// Disposes of the the object.
        /// </summary>
        /// <param name="disposing">A value indicating whether we are disposing of managed objects.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _connectionsLock.Dispose();
                    _runningWaitHandle.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Attempts to connect to a node.
        /// </summary>
        /// <param name="node">The node to connect to.</param>
        private void ConnectToNode(NetworkNode node)
        {
            _connections.Add(new Connection(node, new TcpClient(node.Hostname, node.Port)));
        }

        /// <summary>
        /// Runs the reconnection thread.
        /// </summary>
        private void RunReconnection()
        {
            _runningWaitHandle.WaitOne(5000);
            while (_running)
            {
                _connectionsLock.EnterWriteLock();

                foreach (var n in _connections.Select(e => e.Node).Except(_nodesToConnectTo))
                {
                    ConnectToNode(n);
                }

                _connectionsLock.ExitWriteLock();
            }
        }
    }
}