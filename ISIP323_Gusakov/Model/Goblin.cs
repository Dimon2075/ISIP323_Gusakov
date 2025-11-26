using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class Goblin : Enemy //Гоблин
    {
        public bool VozCrit { get; set; } //Вероятность крита
        public Goblin() : base("Гоблин", 30, 5, 2)
        {
            VozCrit = true;
        }
        public override void AttackPlayer(Player player) //Метод атаки
        {


            Console.WriteLine($"{Name} атакует игрока.");
            int uron = Attack;
            if (VozCrit && new Random().NextDouble() < 0.3)
            {
                uron *= 2;
                Console.WriteLine("Гоблин нанес крит урон!");
            }
            uron -= player.EcipArmor.Zashita;
            if (uron < 0) uron = 0;
            player.Health -= uron;
            if (player.Health < 0) player.Health = 0;
            Console.WriteLine($"Игрок получил {uron} урона. Осталось {player.Health} HP.");


        }
    }
}
