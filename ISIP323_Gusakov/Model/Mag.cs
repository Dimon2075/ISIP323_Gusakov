using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class Mag : Enemy //Маг
    {
        public bool CanFreeze { get; set; } //Заморозка
        public Mag() : base("Маг", 40, 10, 5)
        {
            CanFreeze = false;
        }

        public override void AttackPlayer(Player player)
        {
            Console.WriteLine($"{Name} атакует игрока.");
            int uron = Attack;
            if (new Random().NextDouble() < 0.3)
            {
                uron *= 2;
                Console.WriteLine("Маг нанес крит урон!");
            }
            uron -= player.EcipArmor.Zashita;
            if (uron < 0) uron = 0;
            player.Health -= uron;
            if (player.Health < 0) player.Health = 0;
            Console.WriteLine($"Игрок получил {uron} урона. Осталось {player.Health} HP.");
        } //Метод атаки
    }
}
