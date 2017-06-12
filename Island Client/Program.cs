using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameClient
{
    class Program
    {
        public static IHubProxy _hub;
        public static Guid playerID;

        static void Main(string[] args)
        {
            Console.Write("Enter URL: ");
            string url = Console.ReadLine().Trim();
            var connection = new HubConnection(url);
            _hub = connection.CreateHubProxy("ServerHub");
            connection.Start().Wait();

            _hub.On("Response", x => UpdatePlayer(x)); //receives the player state
            _hub.On("Turn", x => DoAction(x)); //receives the new turn
            _hub.On("Poll", x => Alive()); //notifies server that you are still conected

            string line = null;
            while ((line = System.Console.ReadLine()) != null)
            {
                _hub.Invoke("Spawn", line).Wait();
            }
        }

        public static void Alive()
        {
            _hub.Invoke("Alive", playerID);
        }

        public static void UpdatePlayer(object data)
        {
            Player player = Newtonsoft.Json.JsonConvert.DeserializeObject<Player>(data.ToString());
            playerID = player.ID;
        }

        public static void DoAction(object turn)
        {
            if (playerID != Guid.Empty)
            {                
                _hub.Invoke("DoAction", "move", "N", playerID);
                Console.WriteLine("Turn: " + turn);
            }
        }
    }
}
