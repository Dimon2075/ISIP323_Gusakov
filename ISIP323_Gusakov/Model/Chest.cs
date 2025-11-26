using Rogalic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class Chest //Сундук
    {
        private Random rng;
        private List<Item> possibleItems;
        public Chest()
        {
            rng = new Random();
            possibleItems = new List<Item>
            {
                new Potion("Лечебное зелье", 30),
                new Potion("Большое зелье", 50),
                new WeaponItem("Меч", 15,true),
                new WeaponItem("Топор", 18,true),
                new WeaponItem("Магический посох", 20,true),
                new ArmorItem("Кожаные доспехи", 5),
                new ArmorItem("Кольчуга", 8),
                new ArmorItem("Латные доспехи", 12)
            };
        }
        public Item Open()
        {
            int index = rng.Next(possibleItems.Count);
            return possibleItems[index];
        }
    }
}
