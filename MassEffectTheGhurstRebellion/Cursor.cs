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

        /// <summary>
        /// Prints the cursor at a specified area of the screen according to the cursor's X and Y values
        /// </summary>
        public void Print()
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Icon);
        }

        /// <summary>
        /// Moves the cursor for the first menu of the game
        /// </summary>
        /// <param name="min">Lowest Y coordinate the cursor can move to</param>
        /// <param name="max">Highest X coordinate the cursor can move to</param>
        /// <param name="player">Passes in player to allow it to be affected</param>
        public void FirstMenuMoveAndSelection(int min, int max, Player player)
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                while (Console.KeyAvailable) { Console.ReadKey(true); }

                // moves the cursor up if it's not already at the top
                if (keyPressed.Key == ConsoleKey.UpArrow && Y > min)
                {
                    Y--;
                    choice--;
                }
                // moves the cursor down to the bottom choice if the cursor is at the top
                else if (keyPressed.Key == ConsoleKey.UpArrow && Y == min)
                {
                    Y = max;
                    choice = 1;
                }
                // moves the cursor down if it's not already at the bottom
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y < max)
                {
                    Y++;
                    choice++;
                }
                // moves the cursor up to the top choice if the cursor is at the bottom
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y == max)
                {
                    Y = min;
                    choice = 0;
                }
                // depending on the current choice, pressing enter or spacebar will affect the player's stats
                else if (keyPressed.Key == ConsoleKey.Enter || keyPressed.Key == ConsoleKey.Spacebar)
                    switch (choice)
                    {
                        case 0:
                            player.Dexterity++;
                            player.Points--;
                            break;
                        case 1:
                            player.Strength++;
                            player.Points--;
                            break;
                        default:
                            break;
                    }
            }
        }

        /// <summary>
        /// Moves the cursor for the various in-game menus
        /// </summary>
        /// <param name="min">Lowest Y coordinate the cursor can move to</param>
        /// <param name="max">Highest Y coordinate the cursor can move to</param>
        /// <param name="numChoices">Number of menu choices</param>
        public void MenuMove(int min, int max, int numChoices)
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                while (Console.KeyAvailable) { Console.ReadKey(true); }

                // moves the cursor up if it's not already at the top
                if (keyPressed.Key == ConsoleKey.UpArrow && Y > min)
                {
                    Y--;
                    choice--;
                }
                // moves the cursor down to the bottom choice if the cursor is at the top. numChoice is subtracted by one as the choice starts at 0
                else if (keyPressed.Key == ConsoleKey.UpArrow && Y == min)
                {
                    Y = max;
                    choice = numChoices - 1;
                }
                // moves the cursor down if it's not already at the bottom
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y < max)
                {
                    Y++;
                    choice++;
                }
                // moves the cursor up to the top choice if the cursor is at the bottom
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y == max)
                {
                    Y = min;
                    choice = 0;
                }
                // sets the cursor object's SelectedChoice to choice and resets the choice back to 0
                else if (keyPressed.Key == ConsoleKey.Enter || keyPressed.Key == ConsoleKey.Spacebar)
                {
                    SelectedChoice = choice;
                    ChoiceMade = true;
                    choice = 0;
                }
            }
        }

        /// <summary>
        /// Moves the cursor for the various in-game menus dealing with weapons
        /// </summary>
        /// <param name="min">Lowest Y coordinate the cursor can move to</param>
        /// <param name="weaponList">Highest Y coordinate the cursor can move to is dependent on the weaponList</param>
        public void MenuMove(int min, List<Weapon> weaponList)
        {
            // max Y coordinate is calculated
            int max = weaponList.Count() + min;
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                while (Console.KeyAvailable) { Console.ReadKey(true); }

                // moves the cursor up if it's not already at the top
                if (keyPressed.Key == ConsoleKey.UpArrow && Y > min)
                {
                    Y--;
                    choice--;
                }
                // moves the cursor down to the bottom choice if the cursor is at the top
                else if (keyPressed.Key == ConsoleKey.UpArrow && Y == min)
                {
                    Y = max;
                    choice = weaponList.Count();
                }
                // moves the cursor down if it's not already at the bottom
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y < max)
                {
                    Y++;
                    choice++;
                }
                // moves the cursor up to the top choice if the cursor is at the bottom
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y == max)
                {
                    Y = min;
                    choice = 0;
                }
                // sets the cursor object's SelectedChoice to choice and resets the choice back to 0. If the cursor has selected a weapon, set the SelectedWeapon to the weapon in the weaponList
                // at the SelectedChoice index
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

        /// <summary>
        /// Moves the cursor for the various in-game menus dealing with items
        /// </summary>
        /// <param name="min">Lowest Y coordinate the cursor can move to</param>
        /// <param name="itemList">Highest Y coordinate the cursor can move to is dependent on the itemList</param>
        public void MenuMove(int min, List<Item> itemList)
        {
            // max Y coordinate is calculated
            int max = itemList.Count() + min;
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                while (Console.KeyAvailable) { Console.ReadKey(true); }

                // moves the cursor up if it's not already at the top
                if (keyPressed.Key == ConsoleKey.UpArrow && Y > min)
                {
                    Y--;
                    choice--;
                }
                // moves the cursor down to the bottom choice if the cursor is at the top
                else if (keyPressed.Key == ConsoleKey.UpArrow && Y == min)
                {
                    Y = max;
                    choice = itemList.Count();
                }
                // moves the cursor down if it's not already at the bottom
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y < max)
                {
                    Y++;
                    choice++;
                }
                // moves the cursor up to the top choice if the cursor is at the bottom
                else if (keyPressed.Key == ConsoleKey.DownArrow && Y == max)
                {
                    Y = min;
                    choice = 0;
                }
                // sets the cursor object's SelectedChoice to choice and resets the choice back to 0. If the cursor has selected an item, set the SelectedItem to the item in the itemList
                // at the SelectedChoice index
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
