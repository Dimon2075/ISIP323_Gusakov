using Rogalic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
    public class Player //Игрок
    {
        public int MaxHealth { get; set; } //Максимальное здоровье
        public int Health { get; set; } //Здоровье на данный момент
        public Weapon EcipWeapon { get; set; } //Надетое оружие
        public Armor EcipArmor { get; set; } //Надетая броня
        public bool Zamorozka { get; set; } //Заморозка (пропуск хода)
        public bool IsDefending { get; set; }
        private Random rng;
        public Player(int maxHealth)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
            EcipWeapon = new Weapon("Кулаки", 7, false);
            EcipArmor = new Armor("Рубаха", 1);
            Zamorozka = false;
        }
        public void Attack(Enemy enemy)
        {
            Console.WriteLine($"Игрок атакует {enemy.Name}.");
            int uron = EcipWeapon.Uron;
            if (EcipWeapon.Crit && new Random().NextDouble() < 0.2)
            {
                uron *= 2;
                Console.WriteLine("Крит урон");
            }
            enemy.TakeDamage(uron);

        }
        public bool IsAlive() => Health > 0;
        public void ShowStatus()
        {
            Console.WriteLine($"=== Статус игрока ===");
            Console.WriteLine($"HP: {Health}/{MaxHealth}");
            Console.WriteLine($"Оружие: {EcipWeapon.GetInfo()}");
            Console.WriteLine($"Доспехи: {EcipArmor.GetInfo()}");
            Console.WriteLine($"=====================");
        }
        //public void Heal(Potion potion)
        //{
        //    potion.Use(this);
        //}
        public void Defend()
        {

            IsDefending = true;
            Console.WriteLine("Вы приготовились к защите!");
        }
        public void TakeDamage(int damage, bool ignoreArmor = false)
        {
            if (IsDefending)
            {
                // 40% шанс уклониться
                if (rng.NextDouble() < 0.4)
                {
                    Console.WriteLine("Вы увернулись от атаки!");
                    IsDefending = false;
                    return;
                }

                // Блокирование урона
                if (!ignoreArmor)
                {
                    double blockPercent = 0.7 + (rng.NextDouble() * 0.3); // 70-100%
                    int blockedDamage = (int)(EcipArmor.Zashita * blockPercent);
                    damage = Math.Max(0, damage - blockedDamage);
                    Console.WriteLine($"Вы заблокировали {blockedDamage} урона!");
                }

                IsDefending = false;
            }

            Health -= damage;
            Health = Math.Max(0, Health);
            Console.WriteLine($"Вы получили {damage} урона. Осталось HP: {Health}");
        }
    }

}
