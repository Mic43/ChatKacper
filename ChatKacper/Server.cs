using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatKacper
{
    public delegate void ClientConnected(AddressFamily addressFamily);
   // public delegate void ClientConnected2(string arg);

    class Server
    {
        private object locker = new object();
        public Server(string host, int port)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentException($"'{nameof(host)}' cannot be null or empty", nameof(host));
            }
            Host = host;
            Port = port;
        }

        public event ClientConnected ClientConnected;

        public string Host { get; }
        public int Port { get; }
        public bool Stopped { get => stopped; private set => stopped = value; }
        public bool Stopping
        {
            get
            {
                lock (locker)
                {
                    return stopping;
                }
            }

            private set
            {
                lock (locker)
                {
                    stopping = value;
                }
            }
        }

        private bool stopping = false;
        private bool stopped = true;

        public void Start()
        {
            Stopped = false;
            TcpListener _clientsListener = new TcpListener(IPAddress.Parse(Host), Port);

            while (!Stopping)
            {
                if (_clientsListener.Pending())
                {
                    TcpClient tcpClient = _clientsListener.AcceptTcpClient();                    
                    ClientConnected.Invoke(tcpClient.Client.AddressFamily);
                }
                Thread.Sleep(1000);
            }
            Stopped = true;

        }
        public void Stop()
        {
            Stopping = true;
        }
    }
}
