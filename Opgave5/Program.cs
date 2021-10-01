using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using ObligatoriskStudieAktivitetsOpgave;

namespace Opgave5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is the server");
            TcpListener listener = new TcpListener(IPAddress.Loopback, 2121);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Server ready");
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("Incoming client");
                DoClient(socket);
            }
        }

        //private static void DoClient(TcpClient socket)
        //{
        //    NetworkStream ns = socket.GetStream();
        //    StreamReader reader = new StreamReader(ns);
        //    StreamWriter writer = new StreamWriter(ns);
        //    string message = reader.ReadLine();
        //    Console.WriteLine("Server received: " + message);
        //    writer.Write(message);
        //    writer.Flush();
        //    socket.Close();
        //}

        public static List<FootballPlayer> footballplayers = new List<FootballPlayer>()
        {

            new FootballPlayer(1, "Emil", 100, 10),
            new FootballPlayer(2, "Kasper", 100, 11)
        };

        private static void DoClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            while(true)
            {
                string message1 = reader.ReadLine();
                string message2 = reader.ReadLine();
                Console.WriteLine("Server received: " + message1);

                switch (message1)
                {
                    case "HentAlle":

                        foreach (FootballPlayer footBallplayer in footballplayers)
                        {
                            Console.WriteLine($"Current player: ID: {footBallplayer.Id} | Name: {footBallplayer.Name} | Price: {footBallplayer.Price} | Shirtnumber: {footBallplayer.ShirtNumber}");
                            writer.WriteLine($"Current player: ID: {footBallplayer.Id} | Name: {footBallplayer.Name} | Price: {footBallplayer.Price} | Shirtnumber: {footBallplayer.ShirtNumber}");
                        }
                        writer.WriteLine("HentAlle");
                        writer.WriteLine(message1);
                        writer.WriteLine(message2);
                        writer.Flush();

                        break;


                    case "Hent":
                        FootballPlayer footballplayer = footballplayers.FirstOrDefault(f => f.Id.ToString() == message2);
                        Console.WriteLine($"Player searched for: ID: {footballplayer.Id} | Name: {footballplayer.Name} | Price: {footballplayer.Price} | Shirtnumber: {footballplayer.ShirtNumber}");
                        writer.WriteLine($"Player searched for: ID: {footballplayer.Id} | Name: {footballplayer.Name} | Price: {footballplayer.Price} | Shirtnumber: {footballplayer.ShirtNumber}");
                        writer.WriteLine("Hent");
                        writer.WriteLine(message1);
                        writer.WriteLine(message2);
                        writer.Flush();
                        break;

                    case "Gem":
                        FootballPlayer Jsonplayer = JsonSerializer.Deserialize<FootballPlayer>(message2);
                        footballplayers.Add(Jsonplayer);
                        //Console.WriteLine($"New Footballplayer added: ID: {Jsonplayer.Id++} | Name: {Jsonplayer.Name} | Price: {Jsonplayer.Price} | Shirtnumber: {Jsonplayer.ShirtNumber}");

                        writer.WriteLine("Gem");
                        writer.WriteLine(message1);
                        writer.WriteLine(message2);
                        writer.Flush();
                        break;


                    case "Shutdown":

                        writer.WriteLine("Closing server for now");
                        writer.Flush();
                        socket.Close();
                        break;
                }
            }
        }

    }
}
