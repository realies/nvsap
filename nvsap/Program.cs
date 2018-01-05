using AutoIt;
using Nancy;
using Nancy.Hosting.Self;
using System;

namespace nvsap
{
    public class SimpleModule : NancyModule
    {
        public SimpleModule()
        {
            Get["/auth/{code}"] = parameters =>
            {
                Program.Log($"Auth request received from {this.Request.UserHostAddress}");
                return AuthenticateShield(parameters.code) ? "true" : "false";
            };
        }

        private bool AuthenticateShield(string code)
        {
            if (Program.Busy)
                return false;
            try
            {
                Program.Busy = true;
                string targetWindowClass = "[CLASS:#32770]";
                string targetWindowText = "SHIELD is requesting to connect";
                if (AutoItX.WinWait(targetWindowClass, targetWindowText, 1) == 0)
                {
                    Program.Log("Warning: Auth window does not exist");
                    return false;
                }
                AutoItX.ControlSetText(targetWindowClass, "", "[CLASS:Edit; INSTANCE:1]", code);
                AutoItX.ControlClick(targetWindowClass, "", "[CLASS:Button; INSTANCE:1]");
                Program.Log("Auth code sent");
                return true;
            }
            catch (Exception ex)
            {
                Program.Log($"Authentication error: {ex.Message}");
                return false;
            }
            finally
            {
                Program.Busy = false;
            }
        }
    }

    class Program
    {
        public static bool Busy = false;
        private string url = "http://localhost";
        private int port = 47980;
        private NancyHost nancy;

        public Program()
        {
            var configuration = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true },
                RewriteLocalhost = true
            };
            var uri = new Uri($"{url}:{port}/");
            nancy = new NancyHost(configuration, uri);
        }

        private void Start()
        {
            nancy.Start();
            Log($"Listening on port {port}");
            Console.ReadKey();
            nancy.Stop();
        }

        static void Main(string[] args)
        {
            var p = new Program();
            p.Start();
        }

        public static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] {message}");
        }
    }
}
