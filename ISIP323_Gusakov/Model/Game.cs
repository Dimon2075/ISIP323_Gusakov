using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP323_Gusakov.Model
{
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
}
