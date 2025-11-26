using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class WeaponItem : Item //Оружие
    {
        public int Uron { get; set; } //Урон
        public bool Crit { get; set; } //Урон
        public WeaponItem(string name, int uron, bool crit)
        {
            Name = name;
            Uron = uron;
            Crit = crit;
        }
        public override void Use(Player player) //Использование
        {
            player.EcipWeapon = new Weapon(Name, Uron, Crit);
            Console.WriteLine($"Игрок экипировал оружие \"{Name}\" с уроном {Uron}.");
        }
    }
}
