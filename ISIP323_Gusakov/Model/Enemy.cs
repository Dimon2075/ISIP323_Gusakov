using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public abstract class Enemy //Враг
    {
        public string Name { get; set; } //Название
        public int Health { get; set; } //Здоровье
        public int Attack { get; set; } //Атака
        public int Zashita { get; set; } //Защита
        public bool Crit { get; set; } //Крит урон
        public Enemy(string name, int health, int attack, int zashita)
        {
            Name = name;
            Health = health;
            Attack = attack;
            Zashita = zashita;
            Crit = false;
        }
        public void TakeDamage(int uron)
        {
            Health -= uron;
            if (Health < 0) Health = 0;
            Console.WriteLine($"{Name} получил {uron} урона. Осталось {Health}");
        }
        public abstract void AttackPlayer(Player player); //Метод атаки
        //public abstract void SpecialSposob(Player player); //Метод спец способности
        public bool IsAlive() { return Health > 0; }
    }
}
