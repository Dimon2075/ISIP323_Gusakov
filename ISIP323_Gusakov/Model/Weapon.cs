using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class Weapon //Оружие
    {
        public string Name { get; set; } //Название
        public int Uron { get; set; } //Урон
        public bool Crit { get; set; } //Крит возможность урон
        public Weapon(string name, int uron, bool crit)
        {
            Name = name;
            Uron = uron;
            Crit = crit;
        }
        public virtual string GetInfo()
        {
            return Name;
        }
    }

}
