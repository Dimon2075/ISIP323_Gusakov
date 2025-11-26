using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public abstract class Item //Предмет
    {
        public string Name { get; set; } //Название

        public abstract void Use(Player player); //Использование
        public virtual string GetInfo()
        {
            return Name;
        }
    }
}
