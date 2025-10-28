using Rogalic;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
namespace Rogalic
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
            EcipWeapon = new Weapon("Кулаки", 7,false);
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
    public class Skeleton : Enemy //Скелет
    {
        public bool IgnZashitu { get; set; } //Игор защиты
        public Skeleton() : base("Скелет", 50, 7, 3)
        {
            IgnZashitu = false;
        }
        public override void AttackPlayer(Player player) //Метод атаки
        {
            Console.WriteLine($"{Name} атакует игрока.");
            int uron = Attack;
            if (IgnZashitu)
            {

            }
            else
            {
                uron -= player.EcipArmor.Zashita;
                if (uron < 0) uron = 0;
            }
            player.Health -= uron;
            if (player.Health < 0) player.Health = 0;
            Console.WriteLine($"Игрок получил {uron} урона. Осталось {player.Health} HP.");
        }
    }
    public class Mag : Enemy //Маг
    {
        public bool CanFreeze { get; set; } //Заморозка
        public Mag() : base ("Маг", 40, 10, 5)
        {
            CanFreeze = false;
        }

        public override void AttackPlayer(Player player) 
        {
            Console.WriteLine($"{Name} атакует игрока.");
            int uron = Attack;
            if (new Random().NextDouble() < 0.3)
            {
                uron *= 2;
                Console.WriteLine("Маг нанес крит урон!");
            }
            uron -= player.EcipArmor.Zashita;
            if (uron < 0) uron = 0;
            player.Health -= uron;
            if (player.Health < 0) player.Health = 0;
            Console.WriteLine($"Игрок получил {uron} урона. Осталось {player.Health} HP.");
        } //Метод атаки
    }
    
    public class VVG : Goblin // конкретные боссы
    {
        public VVG() : base()
        {
            Name = "ВВГ";
            Health = 100;
            Attack = 15;
            Zashita = 12;
        }
        public override void AttackPlayer(Player player)
        {
            Console.WriteLine($"{Name} атакует игрока.");
            int uron = Attack;

            player.Health -= uron;
            if (player.Health < 0) player.Health = 0;
            Console.WriteLine($"Игрок получил {uron} урона. Осталось {player.Health} HP.");
        }
    }

    public class Kovalsky : Skeleton
    {
        public Kovalsky() : base()
        {
            Name = "Ковальский";
            Health = 125;
            Attack = 13;
            Zashita = 14;
        }
        public override void AttackPlayer(Player player)
        {
            Console.WriteLine($"{Name} атакует игрока.");
            int uron = Attack;

            player.Health -= uron;
            if (player.Health < 0) player.Health = 0;
            Console.WriteLine($"Игрок получил {uron} урона. Осталось {player.Health} HP.");
        }
    }

    public class ArchmageCPP : Mag
    {
        public double FreezeChance { get; private set; }
        public ArchmageCPP() : base()
        {
            Name = "Архимаг C++";
            Health = 90;
            Attack = 16;
            Zashita = 11;
            FreezeChance = 0.25;
        }
        public override void AttackPlayer(Player player)
        {
            Console.WriteLine($"{Name} атакует игрока.");
            int uron = Attack;

            player.Health -= uron;
            if (player.Health < 0) player.Health = 0;
            Console.WriteLine($"Игрок получил {uron} урона. Осталось {player.Health} HP.");
        }
    }

    public class PestovC : Skeleton
    {
        public double FreezeChance { get; private set; }
        public PestovC() : base()
        {
            Name = "Пестов С--";
            Health = 65;
            Attack = 18;
            Zashita = 6;
            FreezeChance = 0.3;
        }
        public override void AttackPlayer(Player player)
        {
            Console.WriteLine($"{Name} атакует игрока.");
            int uron = Attack;

            player.Health -= uron;
            if (player.Health < 0) player.Health = 0;
            Console.WriteLine($"Игрок получил {uron} урона. Осталось {player.Health} HP.");
        }
    }


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
    public abstract class Item //Предмет
    {
        public string Name { get; set; } //Название
        
        public abstract void Use(Player player); //Использование
        public virtual string GetInfo()
        {
            return Name;
        }
    }
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
            player.EcipWeapon = new Weapon(Name, Uron,Crit);
            Console.WriteLine($"Игрок экипировал оружие \"{Name}\" с уроном {Uron}.");
        }
    }
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
    public class Game
    {
        private Player player;
        private int turnCount;
        private Random rng;
        private List<Enemy> enemies;
        public Game()
        {
            rng = new Random();
            turnCount = 0;
            player = new Player(100);
            enemies = new List<Enemy>();
        }
        public void Start()
        {
            Console.WriteLine("Да нанутся голодные игры");
            while (player.Health > 0)
            {
                turnCount++;
                Console.WriteLine($"\n--- Ход {turnCount} ---");
                player.ShowStatus();
                GenerateEvent();
                Console.WriteLine("Продолжай");
                Console.ReadKey();
            }
            Console.WriteLine("Игра окончена. Героев не забываем");
        }
        private void NextTurn()
        {
           
        }
        private void GenerateEvent() 
        {
            int eventType = rng.Next(0, 4);
            if (eventType == 0)
            {
                Enemy enemy;
                enemy = new Skeleton();
                enemies.Add(enemy);
                Console.WriteLine($"Пришел враг: {enemy.Name}");
                FightEnemy(enemy);
            }
            else if (eventType == 1)
            {
                Enemy enemy;
                enemy = new Mag();
                enemies.Add(enemy);
                Console.WriteLine($"Пришел враг: {enemy.Name}");
                FightEnemy(enemy);
            }
            else if (eventType == 2)
            {
                Enemy enemy;
                enemy = new Goblin();
                enemies.Add(enemy);
                Console.WriteLine($"Пришел враг: {enemy.Name}");
                FightEnemy(enemy);
            }
            else if (turnCount % 10 == 3)
            {
                Enemy boss = GenerateBoss();
                enemies.Add(boss);
                Console.WriteLine($"Появился босс: {boss.Name}");
                FightEnemy(boss);
                //Item potion = new Potion("Зелье восстановления", 20);
                //Chest chest = new Chest(potion);
                //chest.Open(player);
            }
            else
            {
                OpenChest();
            }
        }
        private void FightEnemy(Enemy enemy)
        {
            bool bt = true;
            while (player.IsAlive() && enemy.IsAlive())
            {
                
                // Первый ход: игрок атакует
                PlayerTurn(enemy);
                if (!enemy.IsAlive()) break;
                enemy.AttackPlayer(player);

                if (!enemy.IsAlive())
                {
                    Console.WriteLine($"\n*** {enemy.Name} побежден! ***");
                }

                Console.WriteLine("Продолжить атаку");

                
            }
            
        }
        private void PlayerTurn(Enemy enemy)
        {
            Console.WriteLine("\n--- Ваш ход ---");
            Console.WriteLine("1. Атаковать");
            Console.WriteLine("2. Защищаться");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    player.Attack(enemy); break;
                case "2":
                    player.Defend(); break;
                default:
                    Console.WriteLine("Неверный выбор! Вы пропускаете ход.");
                    break;
            }
        }
        private Enemy GenerateBoss() 
        {
            int bossType = rng.Next(4);
            return bossType switch
            {
                0 => new VVG(),
                1 => new Kovalsky(),
                2 => new ArchmageCPP(),
                3 => new PestovC(),
                _ => new VVG()
            };
        }
        public void OpenChest()
        {
            Console.WriteLine("\n*** Вы нашли сундук! ***");
            Chest chest = new Chest();
            Item item = chest.Open();
            Console.WriteLine($"В сундуке: {item.GetInfo()}");
            if (item is Potion potion)
            {
                Console.WriteLine("Использовать зелье? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    potion.Use(player);
                }
            }
            else if (item is WeaponItem weapon)
            {

                Console.WriteLine($"Текущее оружие: {player.EcipWeapon.GetInfo()}");
                Console.WriteLine("Заменить оружие? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    weapon.Use(player);
                }
            }
            else if (item is ArmorItem armor)
            {
                Console.WriteLine($"Текущие доспехи: {player.EcipArmor}");
                Console.WriteLine("Заменить доспехи? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    armor.Use(player);
                }
            }
        }
    }
    class Pr6
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}



