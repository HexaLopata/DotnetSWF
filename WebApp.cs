using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using DotnetSWF.Routing;
using DotnetSWF.HTTPInteraction;
using DotnetSWF.FileManagement;

namespace DotnetSWF
{
    public class WebApp
    {
        private Socket _server;
        private int _port = 5000;
        private string _iP = "192.168.0.11";
        private int _backLog = 10;
        private const int bufferSize = 4096;
        private bool _detailedLog = true;
        private IRouter _router;

        public IRouter Router
        {
            get => _router;
            set => _router = value;
        }

        public WebApp() 
        {
            _router = new DefaultRouter(new StaticFileWorker());
        }

        public WebApp(IRouter router) 
        {
            _router = router;
        }

        public void Run()
        {
            StartServer();
            try
            {
                while (true)
                {
                    Socket client = _server.Accept();
                    byte[] buffer = new byte[bufferSize];
                    int bytes = 0;
                    do
                    {
                        bytes = client.Receive(buffer);
                    }
                    while (client.Available > 0);

                    string stringData = Encoding.UTF8.GetString(buffer).Trim('\0');
                    HttpRequest request = null;
                    HttpResponse response = null;
                    Console.WriteLine("Client connected");
                    if (HttpRequest.TryParse(stringData, ref request))
                    {
                        if (_detailedLog)
                        {
                            Console.WriteLine("\tMethod: " + request.Method);
                            Console.WriteLine("\tPath: " + request.Path);
                        }
                        response = _router.GetHttpResponseByRoute(request).GetHttpResponse();
                        client.Send(response.GetBytes(Encoding.UTF8));
                    }
                    else
                    {
                        Console.WriteLine("\tInvalid Request");
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