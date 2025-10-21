using System;
using System.Collections.Generic;
namespace Rogalic
{
    public abstract class Item
    {
        public string Name { get; set; }
    }
    public class Weapon : Item
    {
        public int AttackPower { get; set; }

    }
    public class Armor : Item
    {
        public int Defence { get; set; }
    }
    public class Potion : Item
    {
        public int HealAmount { get; set; } = 30;
    }

    public class Player
    {
        public int Health { get; set; } = 100;
        public Weapon EcipWeapon { get; private set; }
        public Armor EcipArmor { get; private set; }
        public bool Propusk { get; set; }
        public Player(int health)
        {
            Health = health;
            EcipWeapon = null;
            EcipArmor = null;
            Propusk = false;
        }
        public void Attack(Enemy enemy) 
        {
            int damage = EcipWeapon != null ? EcipWeapon.AttackPower : 5;
            enemy.TakeDamage(damage);
            Console.WriteLine($"Вы нанесли врагу {damage} урона.");
        }
        public void Defend() { }
        public void TakeDamage(int damage) 
        {
            Health -= damage;
            if(Health < 0) Health = 0;
            Console.WriteLine($"Здоровье: {Health}"
        }
        public void Heal() { }
        public void EcWeapon(Weapon weapon) { }
        public void EcArmor(Armor armor) { }
        public bool TryDodge() { return false; }
    }
    public abstract class Enemy
    {
        public int Health { get; set; }
        public int AttackPower { get; set; }
        public int Defence { get; set; }
        public Enemy(int health, int attackPower, int defence)
        {
            Health = health;
            AttackPower = attackPower;
            Defence = defence;
        }
        public abstract void Attack(Player player);
        public void TakeDamage(int damage) { }
        public virtual bool SpecialEffect(Player player) { return false; }
    }
    public class Goblin : Enemy
    {
        public double CritChance { get; set; }
        public Goblin(int health, int attackPower, int defence, double critchance) : base(health, attackPower, defence)
        {
            CritChance = critchance;
        }
        public override void Attack(Player player) { }
        public override bool SpecialEffect(Player player) { return false; }
    }
    public class Skeleton : Enemy
    {
        public Skeleton(int health, int attackPower, int defence) : base(health, attackPower, defence) { }
        public override void Attack(Player player) { }
        public override bool SpecialEffect(Player player) { return false; }
    }
    public class Mag : Enemy
    {
        public double FreezeChance { get; set; }
        public Mag(int health, int attackPower, int defense, double freezeChance) : base(health, attackPower, defense)
        {
            FreezeChance = freezeChance;
        }
        public override void Attack(Player player) { }
        public override bool SpecialEffect(Player player) { return false;}
    }
    public class VVG : Goblin
    {
        public VVG(int health, int attackPower, int defence, double critchance) : base(health, attackPower, defence, critchance) { }
    }
    public class Koval : Skeleton
    {
        public Koval(int health, int attackPower, int defence) : base(health, attackPower, defence) { }
    }
    public class ArhiMag_Cplusplus : Mag
    {
        public ArhiMag_Cplusplus(int health, int attackPower, int defence, double freezeChance) : base(health, attackPower, defence, freezeChance) { } 
    }
    public class Pestov : Skeleton
    {
        public double FreezeChance { get; set;}
        public Pestov(int health, int attackPower,int defence,double freezeChance) : base(health, attackPower,defence)
        {
            FreezeChance = freezeChance;
        }
        public override void Attack(Player player) { }
        public override bool SpecialEffect(Player player) { return false;}
    }
    
    public class Chest
    {
        public Item ContainedItem { get; set; }
        public Chest(Item item)
        {
            ContainedItem = item;
        }
        public Item Open() { return ContainedItem; }
    }
    public class Game
    {
        public Player Player { get; set; }
        public int TurnCount { get; set; }
        public Random RandomGen {  get; set; }
        public Game()
        {
            Player = new Player(health: 100);
            TurnCount = 0;
            RandomGen = new Random();
        }
        public void Start() { }
        public void PlayTurn() { }
        public void HandleChest() { }
        public void HandlEnemyEncounter() { }
        public Enemy GenerateRanEnemy() { return null; }
        public void GenerateBoss() { }
        public void Battle() { }

    }
}