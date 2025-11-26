using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class Potion : Item //Зелье
    {
        public int HealAmount { get; set; }
        public Potion(string name, int healamount)
        {
            Name = name;
            HealAmount = healamount;
        }
        public override void Use(Player player) //Использование
        {
            player.Health += HealAmount;
            if (player.Health > player.MaxHealth)
                player.Health = player.MaxHealth;
            Console.WriteLine($"Игрок использовал {Name} и восстановил {HealAmount} HP. Текущие HP: {player.Health}");
        }
    }
}
