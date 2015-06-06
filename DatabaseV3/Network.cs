using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        /// A value indicating whether the object has been disposed.
        /// </summary>
        private bool _disposedValue = false;

        /// <summary>
        /// The listener listening for connections.
        /// </summary>
        private TcpListener _listener = null;

        /// <summary>
        /// The message listener thread.
        /// </summary>
        private Thread _messageListenerThread;

        /// <summary>
        /// The nodes to stay connected to.
        /// </summary>
        private List<NetworkNode> _nodesToConnectTo = new List<NetworkNode>();

        /// <summary>
        /// The port to listen on.
        /// </summary>
        private int _port = -1;

        /// <summary>
        /// The reconnection thread.
        /// </summary>
        private Thread _reconnectionThread;

        /// <summary>
        /// A value indicating whether the network is running.
        /// </summary>
        private bool _running = false;

        /// <summary>
        /// The wait handle to trigger when the network is no longer running.
        /// </summary>
        private ManualResetEvent _runningWaitHandle = new ManualResetEvent(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        /// <param name="port">The port to listen to connection on.</param>
        public Network(int port) : this()
        {
            _port = port;
            Logger.Log("Listening on port " + _port + ".", LogLevel.Info);
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.BeginAcceptTcpClient(AcceptTcpClient, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        public Network()
        {
            _reconnectionThread = new Thread(RunReconnection);
            _reconnectionThread.Start();

            _messageListenerThread = new Thread(RunMessageListener);
            _messageListenerThread.Start();
        }

        /// <summary>
        /// Connects to a node.
        /// </summary>
        /// <param name="node">The node to connect to.</param>
        public void Connect(NetworkNode node)
        {
            Logger.Log("Connecting to " + node.ToString() + ".", LogLevel.Info);
            lock (_connections)
            {
                _nodesToConnectTo.Add(node);
                ConnectToNode(node);
            }
        }

        /// <summary>
        /// Disconnects from a node.
        /// </summary>
        /// <param name="node">The node to disconnect from.</param>
        public void Disconnect(NetworkNode node)
        {
            Logger.Log("Disconnecting from " + node.ToString() + ".", LogLevel.Info);
            lock (_connections)
            {
                _nodesToConnectTo.Remove(node);
            }
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
            Logger.Log("Shutting down network.", LogLevel.Info);
            _running = false;
            _runningWaitHandle.Set();

            _reconnectionThread.Join();
            _messageListenerThread.Join();
        }

        /// <summary>
        /// Disposes of the the object.
        /// </summary>
        /// <param name="disposing">A value indicating whether we are disposing of managed objects.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _runningWaitHandle.Dispose();

                    if (_listener != null)
                    {
                        ((IDisposable)_listener).Dispose();
                    }
                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Accepts a <see cref="TcpClient"/> from the <see cref="TcpListener"/>.
        /// </summary>
        /// <param name="result">The result of the async operation.</param>
        private void AcceptTcpClient(IAsyncResult result)
        {
            TcpClient client;
            try
            {
                client = _listener.EndAcceptTcpClient(result);
                _listener.BeginAcceptTcpClient(AcceptTcpClient, null);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            NetworkNode node;
            DateTime connectionTime;
            try
            {
                node = new NetworkNode(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), ((IPEndPoint)client.Client.RemoteEndPoint).Port);
                byte[] buffer = new byte[sizeof(long)];
                client.GetStream().Read(buffer, 0, sizeof(long));
                connectionTime = DateTime.FromBinary(BitConverter.ToInt64(buffer, 0));
                Logger.Log("New connection request from " + node + ".", LogLevel.Debug);
            }
            catch
            {
                Logger.Log("Error while processing connection request.", LogLevel.Debug);
                return;
            }

            lock (_connections)
            {
                try
                {
                    Connection existingConnection = _connections.FirstOrDefault(e => e.Node.Equals(node));
                    if (existingConnection != null)
                    {
                        if (connectionTime < existingConnection.ConnectionTime)
                        {
                            _connections.Remove(existingConnection);
                            Connection connection = new Connection(node, client, connectionTime);
                            client.GetStream().Write(BitConverter.GetBytes(true), 0, sizeof(bool));
                            connection.Connected();
                            _connections.Add(connection);
                            Logger.Log("Connection request overriding current connection.", LogLevel.Debug);
                        }
                        else
                        {
                            client.GetStream().Write(BitConverter.GetBytes(false), 0, sizeof(bool));
                            Logger.Log("Connection request failed, newer connection already established.", LogLevel.Debug);
                        }
                    }
                    else
                    {
                        Connection connection = new Connection(node, client, connectionTime);
                        client.GetStream().Write(BitConverter.GetBytes(true), 0, sizeof(bool));
                        connection.Connected();
                        _connections.Add(connection);
                        Logger.Log("Connection request successful.", LogLevel.Debug);
                    }
                }
                catch
                {
                    Logger.Log("Error while processing connection request.", LogLevel.Debug);
                }
            }
        }

        /// <summary>
        /// Attempts to connect to a node.
        /// </summary>
        /// <param name="node">The node to connect to.</param>
        private void ConnectToNode(NetworkNode node)
        {
            try
            {
                DateTime connectionTime = DateTime.UtcNow;
                TcpClient client = new TcpClient(node.Hostname, node.Port);
                byte[] connectionTimeBytes = BitConverter.GetBytes(connectionTime.Ticks);
                client.GetStream().Write(connectionTimeBytes, 0, connectionTimeBytes.Length);
                _connections.Add(new Connection(node, new TcpClient(node.Hostname, node.Port), connectionTime));
                Logger.Log("Connection request sent to " + node + ".", LogLevel.Debug);
            }
            catch
            {
                Logger.Log("Error connecting to " + node + ".", LogLevel.Debug);
            }
        }

        /// <summary>
        /// Runs the message listener thread.
        /// </summary>
        private void RunMessageListener()
        {
            while (_running)
            {
                lock (_connections)
                {
                    foreach (var c in _connections)
                    {
                        if (c.Client.Available > 0)
                        {
                            if (c.Status == ConnectionStatus.Creating)
                            {
                                byte[] keepAliveBuffer = new byte[sizeof(bool)];
                                c.Client.GetStream().Read(keepAliveBuffer, 0, sizeof(bool));
                                bool keepAlive = BitConverter.ToBoolean(keepAliveBuffer, 0);
                                if (!keepAlive)
                                {
                                    c.Client.Close();
                                    c.Disconnected();
                                    Logger.Log("Connection to " + c.Node + " failed.", LogLevel.Debug);
                                }
                                else
                                {
                                    c.Connected();
                                    Logger.Log("Connection established to " + c.Node + ".", LogLevel.Debug);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Runs the reconnection thread.
        /// </summary>
        private void RunReconnection()
        {
            _runningWaitHandle.WaitOne(5000);
            while (_running)
            {
                lock (_connections)
                {
                    foreach (var n in _connections.Select(e => e.Node).Except(_nodesToConnectTo))
                    {
                        Logger.Log("Attempting to reconnect to " + n + ".", LogLevel.Debug);
                        ConnectToNode(n);
                    }
                }

                _runningWaitHandle.WaitOne(5000);
            }
        }
    }
}