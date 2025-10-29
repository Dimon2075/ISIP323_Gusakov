using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ISIP323_Gusakov_Framework1
{
    public static class GameAuto
    {
        private static Random rnd = new Random();
        public static void ObnPostavok()
        {
            
        }
        public static void ProcessClientTurn()
        {
            
        }
        public static void ShowShop()
        {
           
        }
        public static bool CheckGameOver()
        {
           
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в Ваш Автосервис!");  
                while (true)
                {
                    GameAuto.ObnPostavok();
                    GameAuto.ProcessClientTurn();
                    if (GameAuto.CheckGameOver())
                    {
                        break;
                    }
                    GameAuto.ShowShop();
                }
            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
        }
    }
}