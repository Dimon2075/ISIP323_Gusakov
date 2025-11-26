using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class ArmorItem : Item //Доспехи
    {
        public int Zashita { get; set; } //Защита
        public ArmorItem(string name, int zashita)
        {
            Name = name;
            Zashita = zashita;
        }
        public override void Use(Player player) //Использование
        {
            player.EcipArmor = new Armor(Name, Zashita);
            Console.WriteLine($"Игрок экипировал броньку \"{Name}\" с защитой {Zashita}.");
        }
    }
}
