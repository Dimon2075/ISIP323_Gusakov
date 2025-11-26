using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class ArchmageCPP : Mag
    {
        public double FreezeChance { get; private set; }
        public ArchmageCPP() : base()
        {
            Name = "Архимаг C++";
            Health = 90;
            Attack = 16;
            Zashita = 11;
            FreezeChance = 0.25;
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
