using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassEffectTheGhurstRebellion
{
    class Cursor
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Icon { get; set; }
        public Weapon SelectedWeapon { get; set; }
        public Item SelectedItem { get; set; }
        public int SelectedChoice { get; set; }
        public bool ChoiceMade { get; set; }

        public int choice = 0;

        public Cursor()
        {
            X = 3;
            Y = 6;
            Icon = ">";
            ChoiceMade = false;
        }

        public void Print()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Icon);
        }

        public void FirstMenuMoveAndSelection(int min, int max, Player player)
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                while (Console.KeyAvailable) { Console.ReadKey(true); }

                if (keyPressed.Key == ConsoleKey.UpArrow && Y > min)
                {
                    Y--;
                    choice--;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow && Y == min)
                {
                    Y = max;
                    choice = 2;
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y < max)
                {
                    Y++;
                    choice++;
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y == max)
                {
                    Y = min;
                    choice = 0;
                }
                else if (keyPressed.Key == ConsoleKey.Enter || keyPressed.Key == ConsoleKey.Spacebar)
                    switch (choice)
                    {
                        case 0:
                            player.MeleeAccuracy++;
                            player.Points--;
                            break;
                        case 1:
                            player.RangedAccuracy++;
                            player.Points--;
                            break;
                        case 2:
                            player.Strength++;
                            player.Points--;
                            break;
                        default:
                            break;
                    }
            }
        }

        public void MenuMove(int min, int max, int numChoices)
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                while (Console.KeyAvailable) { Console.ReadKey(true); }

                if (keyPressed.Key == ConsoleKey.UpArrow && Y > min)
                {
                    Y--;
                    choice--;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow && Y == min)
                {
                    Y = max;
                    choice = numChoices - 1;
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y < max)
                {
                    Y++;
                    choice++;
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y == max)
                {
                    Y = min;
                    choice = 0;
                }
                else if (keyPressed.Key == ConsoleKey.Enter || keyPressed.Key == ConsoleKey.Spacebar)
                {
                    SelectedChoice = choice;
                    ChoiceMade = true;
                    choice = 0;
                }
            }
        }

        public void MenuMove(int min, List<Weapon> weaponList)
        {
            int max = weaponList.Count() + min;
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                while (Console.KeyAvailable) { Console.ReadKey(true); }

                if (keyPressed.Key == ConsoleKey.UpArrow && Y > min)
                {
                    Y--;
                    choice--;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow && Y == min)
                {
                    Y = max;
                    choice = weaponList.Count();
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y < max)
                {
                    Y++;
                    choice++;
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y == max)
                {
                    Y = min;
                    choice = 0;
                }
                else if (keyPressed.Key == ConsoleKey.Enter || keyPressed.Key == ConsoleKey.Spacebar)
                {
                    SelectedChoice = choice;
                    ChoiceMade = true;
                    if (choice > weaponList.Count() - 1)
                        choice = 0;
                    else
                    {
                        SelectedWeapon = weaponList[SelectedChoice];
                        choice = 0;
                    }
                }
            }
        }

        public void MenuMove(int min, List<Item> itemList)
        {
            int max = itemList.Count() + min;
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                while (Console.KeyAvailable) { Console.ReadKey(true); }

                if (keyPressed.Key == ConsoleKey.UpArrow && Y > min)
                {
                    Y--;
                    choice--;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow && Y == min)
                {
                    Y = max;
                    choice = itemList.Count();
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y < max)
                {
                    Y++;
                    choice++;
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y == max)
                {
                    Y = min;
                    choice = 0;
                }
                else if (keyPressed.Key == ConsoleKey.Enter || keyPressed.Key == ConsoleKey.Spacebar)
                {
                    SelectedChoice = choice;
                    ChoiceMade = true;
                    if (choice > itemList.Count() - 1)
                        choice = 0;
                    else
                    {
                        SelectedItem = itemList[SelectedChoice];
                        choice = 0;
                    }
                }
            }
        }
    }
}
