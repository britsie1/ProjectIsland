using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameServer
{
    public enum ActionStatus
    {
        Invalid,
        Valid
    }

    public class Action
    {
        public Action(string command, string location, long time)
        {
            Command = command;
            Location = location;
            Time = time;
        }

        public string Command { get; set; }
        public string Location { get; set; }
        public long Time { get; set; }
    }

    public class Player
    {
        public Player(string nickname)
        {
            Nickname = Regex.Replace(nickname, "<.*?>", String.Empty).Substring(0, (nickname.Length > 10 ? 10 : nickname.Length));
            ID = Guid.NewGuid();
            Health = 10;
            Stamina = 10;
            Damage = 1;
            Vision = 10;
            Inventory = new int[5];
            Location = new Point(0,0);
            LastActionStatus = ActionStatus.Valid;
            //TODO:: populate visible map and items after map has been generated
        }
        public Guid ID { get; set; }
        public int Health { get; set; }
        public int Vision { get; set; }
        public int Stamina { get; set; }
        public int[] Inventory { get; set; }
        public Point Location { get; set; }
        public string Nickname { get; set; }
        public int[,] VisibleMap { get; set; }
        public int[,] VisibleItems { get; set; }
        public ActionStatus LastActionStatus { get; set; }
        public long LastActionTime { get; set; }
        public int Damage { get; set; }
        public Point GlobalLocation { get; set; }
        public bool Alive { get; set; }
    }
}
