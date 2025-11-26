using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class Kovalsky : Skeleton
    {
        public Kovalsky() : base()
        {
            Name = "Ковальский";
            Health = 125;
            Attack = 13;
            Zashita = 14;
        }
        public override void AttackPlayer(Player player)
        {
            Console.WriteLine($"{Name} атакует игрока.");
            int uron = Attack;

            player.Health -= uron;
            if (player.Health < 0) player.Health = 0;
            Console.WriteLine($"Игрок получил {uron} урона. Осталось {player.Health} HP.");
        }
    }
}
