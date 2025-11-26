using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class Skeleton : Enemy //Скелет
    {
        public bool IgnZashitu { get; set; } //Игор защиты
        public Skeleton() : base("Скелет", 50, 7, 3)
        {
            IgnZashitu = false;
        }
        public override void AttackPlayer(Player player) //Метод атаки
        {
            Console.WriteLine($"{Name} атакует игрока.");
            int uron = Attack;
            if (IgnZashitu)
            {

            }
            else
            {
                uron -= player.EcipArmor.Zashita;
                if (uron < 0) uron = 0;
            }
            player.Health -= uron;
            if (player.Health < 0) player.Health = 0;
            Console.WriteLine($"Игрок получил {uron} урона. Осталось {player.Health} HP.");
        }
    }
}
