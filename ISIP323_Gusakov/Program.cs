public class Player //Игрок
{
    public int MaxHealth { get; set; } //Максимальное здоровье
    public int Health { get; set; } //Здоровье на данный момент
    public Weapon EcipWeapon { get; set; } //Надетое оружие
    public Armor EcipArmor { get; set; } //Надетая броня
    public bool Zamorozka { get; set; } //Заморозка (пропуск хода)
}
public class Weapon //Оружие
{
    public string Name { get; set; } //Название
    public int Uron { get; set; } //Урон
    public bool Crit { get; set; } //Крит возможность урона
}
public class Armor //Броня
{
    public string Name { get; set; } //Название
    public int Zashita { get; set; } //Защита
}
public abstract class Enemy //Враг
{
    public string Name { get; set; } //Название
    public int Health { get; set; } //Здоровье
    public int Attack { get; set; } //Атака
    public int Zashita {  get; set; } //Защита
    public bool Crit { get; set; } //Крит урон
    public abstract void AttackPlayer(Player player); //Метод атаки
    public abstract void SpecialSposob(Player player); //Метод спец способности
}
public class Goblin : Enemy //Гоблин
{
    public bool VozCrit { get; set; } //Вероятность крита
    public override void AttackPlayer(Player player) { } //Метод атаки
}
public class Skeleton : Enemy //Скелет
{
    public bool IgnZashitu { get; set; } //Игор защиты
    public override void AttackPlayer(Player player) { } //Метод атаки
}
public class Mag : Enemy //Маг
{
    public bool CanFreeze { get; set; } //Заморозка
    public override void AttackPlayer(Player player) { } //Метод атаки
}
public class Boss : Enemy //Босс
{

}
public class Chest //Сундук
{
    public Item SundItem { get; set; } //Предмет из сундука
    public void Open(Player player) { } //Открыть сундук
}
public abstract class Item //Предмет
{
    public string Name { get; set; } //Название
    public abstract void Use(Player player); //Использование
}
public class Potion : Item //Зелье
{
    public override Use(Player player) { } //Использование
}
public class WeaponItem : Item //Оружие
{
    public int Uron {  get; set; } //Урон
    public override Use(Player player) { } //Использование
}
public class ArmorItem : Item //Доспехи
{
    public int Zashita { get; set; } //Защита
    public override Use(Player player) { } //Использование
}
public class Game
{
    private Player player;
    private int turnCount;
    private Random rng;
    private List<Enemy> enemies;
    public void Start() { }
    private void NextTurn() { }
    private void GenerateEvent() { }
    private void FightEnemy(Enemy enemy) { }
    private void GenerateBoss() { }
}



