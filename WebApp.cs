using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace DotnetSWF
{
    public class WebApp
    {
        private IWebAppInitializer _initializer;
        private Socket _server;
        private int _port = 5000;
        private string _iP = "127.0.0.1";
        private int _backLog = 1;
        private const int bufferSize = 512;
        private bool _detailedLog = true;
        private IRouter _router = new DefaultRouter();

        public IRouter Router
        {
            get => _router;
            set => _router = value;
        }

        public WebApp(IWebAppInitializer initializer)
        {
            _initializer = initializer;
            _initializer.Initialize(this);
        }

        public void Run()
        {
            StartServer();
            try
            {
                while (true)
                {
                    Socket client = _server.Accept();
                    Console.WriteLine("Client was connected");
                    byte[] buffer = new byte[bufferSize];
                    int bytes = 0;
                    do
                    {
                        bytes = client.Receive(buffer);
                    }
                    while (client.Available > 0);

                    string stringData = Encoding.UTF8.GetString(buffer);
                    if (_detailedLog)
                    {
                        Console.WriteLine("This data was sent by client:");
                        Console.WriteLine(stringData);
                    }

                    HttpRequest request = null;
                    HttpResponse response = null;
                    if (HttpRequest.TryParse(stringData, ref request))
                    {
                        response = _router.GetHttpResponseByRoute(request).GetHttpResponse();
                        if (_detailedLog)
                        {
                            System.Console.WriteLine("Response was sent: ");
                           Console.WriteLine(response.ToString());
                        }
                        client.Send(response.GetBytes(Encoding.UTF8));
                    }
                    else
                    {
                        client.Send(HttpResponse.NotFound.GetBytes(Encoding.UTF8));
                    }
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
            }
            catch (SocketException socketEx)
            {
                Console.WriteLine("Socket Exception!");
                Console.WriteLine(socketEx.Message);
                Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error was occured!!!");
                Console.WriteLine("Server will stop working!");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _server.Close();
            }
        }

        private void StartServer()
        {
            try
            {
                EndPoint endPoint = new IPEndPoint(IPAddress.Parse(_iP), _port);
                _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _server.Bind(endPoint);
                _server.Listen(_backLog);
                Console.WriteLine($"Server started at http://{_iP}:{_port}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while server socket was creating\n");
                Console.WriteLine("Server will stop working");
                Console.WriteLine(ex.Message);
            }
        }
    }
}