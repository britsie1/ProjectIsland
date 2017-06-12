using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient
{
    public enum ActionStatus
    {
        Invalid,
        Valid
    }

    public class Player
    {
        public Player(string nickname)
        {
            Nickname = nickname;
            ID = Guid.NewGuid();
            Health = 10;
            Stamina = 10;
            Damage = 1;
            Vision = 10;
            Inventory = new int[5];
            Location = new Point(0, 0);
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
        public int Damage { get; set; }
    }
}
