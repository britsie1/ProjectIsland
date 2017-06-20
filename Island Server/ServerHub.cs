using CefSharp;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    [HubName("ServerHub")]
    public class ServerHub : Hub
    {
        //Game game = Form1.game;
        public int TurnTimeout = 3; //time before player turns time out
        public int TurnTime = 0; //current time of turn
        public System.Timers.Timer tmrTurn;
        public DateTime startTurnTime;

        public ServerHub()
        {
            if (tmrTurn == null)
            {
                tmrTurn = new System.Timers.Timer();
                tmrTurn.Interval = 1000;
                tmrTurn.Elapsed += TmrTurn_Elapsed;
            }
        }

        private void TmrTurn_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Turn();
        }

        public void Alive(string playerID)
        {
            Game.Players.Where(x => x.ID == Guid.Parse(playerID)).First().Alive = true;
        }

        public object PlayerState(Player player)
        {
            //hides properties that the clients don't need to see
            return new
            {
                ID = player.ID,
                Health = player.Health,
                Vision = player.Vision,
                Stamina = player.Stamina,
                Inventory = player.Inventory,
                Location = player.Location,
                Nickname = player.Nickname,
                VisibleMap = player.VisibleMap,
                VisibleItems = player.VisibleItems,
                LastActionStatus = player.LastActionStatus,
                Damage = player.Damage
            };
        }

        public void Turn()
        {

            TurnTime++;

            ExecuteScript($"UpdateMap('{ Game.Turn } ({ TurnTime }s)',{ Newtonsoft.Json.JsonConvert.SerializeObject(Game.Players) });");

            if (TurnTime == TurnTimeout || (Game.ActionQueue.Count == Game.Players.Where(x => x.Alive).Count() && TurnTime == 1 && Game.ActionQueue.Count > 0))
            {
                startTurnTime = DateTime.Now;
                Clients.All.Turn(Game.Turn++);
                TurnTime = 0;
                Game.ActionQueue.Clear();
                Game.Players.ForEach(x => x.Alive = false);
            }
            Clients.All.Poll();
        }

        public void Spawn(string nickname)
        {
            if (!Game.Started)
            {
                Game.Started = true;
                tmrTurn.Start();
            }

            Player player = new Player(nickname);
            Game.Players.Add(player);
            player.GlobalLocation = Game.Spawn();

            Clients.Caller.Response(PlayerState(player));
            ExecuteScript("UpdateLog('<span style=\"color: #FFFF00;\">" + player.Nickname + "</span> washed up on the shore.');");
        }

        public void ExecuteScript(string script)
        {
            try
            {
                GameServerForm.ChromeBrowser.GetMainFrame().ExecuteJavaScriptAsync(script);
            }
            catch (Exception)
            { }
        }

        public void DoAction(string action, string location, string playerID)
        {
            Player player = Game.Players.Where(x => x.ID == Guid.Parse(playerID)).FirstOrDefault();
            TimeSpan ts = DateTime.Now - startTurnTime;
            int timeTook = ts.Milliseconds;
            player.LastActionTime = timeTook;
            player.Alive = true;

            bool ActionUsesTurn = false;
            /*
                move        - move player
                take        - pick up item
                use         - use item
                useInv      - use item in inventory
                inspect     - look at item properties
                inspectInv  - look at item properties in inventory
                drop        - drop item from Inventory
                attack      - attack target
            */
            switch (action)
            {
                case "move":
                    ActionUsesTurn = true;
                    break;
                case "use":
                    ActionUsesTurn = true;
                    break;
                case "useInv":
                    ActionUsesTurn = true;
                    break;
                case "inspect":
                    ActionUsesTurn = true;
                    break;
                case "inspectInv":
                    ActionUsesTurn = true;
                    break;
                case "take":
                    ActionUsesTurn = false;
                    break;
                case "drop":
                    ActionUsesTurn = false;
                    break;
                case "attack":
                    ActionUsesTurn = true;
                    break;
                default:
                    ActionUsesTurn = false;
                    break;
            }

            if (ActionUsesTurn)
            {
                if (Game.ActionQueue.Where(x => x.Key == player.ID).Count() == 0)
                {
                    Game.ActionQueue.Add(new KeyValuePair<Guid, Action>(player.ID, new Action(action, location, timeTook)));
                    player.LastActionStatus = ActionStatus.Valid;
                }
                else
                    player.LastActionStatus = ActionStatus.Invalid;
            }
            else
            {
                //TODO:: Validate actions that don't take turns
            }

            Clients.Caller.Response(PlayerState(player));
        }
    }
}
