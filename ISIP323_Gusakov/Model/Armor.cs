using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class Armor //Броня
    {
        public string Name { get; set; } //Название
        public int Zashita { get; set; } //Защита
        public Armor(string name, int zashita)
        {
            Name = name;
            Zashita = zashita;
        }
        public virtual string GetInfo()
        {
            return Name;
        }
    }
}
