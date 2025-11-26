using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class PestovC : Skeleton
    {
        public double FreezeChance { get; private set; }
        public PestovC() : base()
        {
            Name = "Пестов С--";
            Health = 65;
            Attack = 18;
            Zashita = 6;
            FreezeChance = 0.3;
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
