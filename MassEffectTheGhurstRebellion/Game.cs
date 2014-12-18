using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;

namespace MassEffectTheGhurstRebellion
{
    class Game
    {
        public Player Player { get; set; }
        public Character Enemy { get; set; }
        public List<Weapon> GameWeapons { get; set; }
        public List<Item> GameItems { get; set; }
        public List<Area> GameAreas { get; set; }
        public Cursor GameCursor { get; set; }

        public Game()
        {
            GameCursor = new Cursor();
            Console.SetWindowSize(101, 30);
            Console.SetBufferSize(101, 30);
            DisplayTitleScreen();
            IntroSequence();
            // testing purposes for combat
            Enemy = new Character("Ghurst Grunt", 100, 100, 50, 50);
            Enemy.WeaponInventory.Add(GameWeapons[2]);
            Enemy.WeaponInventory.Add(GameWeapons[1]);
            Enemy.WeaponInventory.Add(GameWeapons[0]);
            // end testing
            DisplayArea();
        }

        /// <summary>
        /// Displays the intro sequence
        /// </summary>
        void IntroSequence()
        {
            Console.Write("Enter your character's name: ");
            string name = Console.ReadLine();
            bool inputValid = false;
            ConsoleKey input;

            // strips out non-letters from the name
            name = Regex.Replace(name, @"[^A-Za-z]", "");

            // if the string is completely empty, sets the name to the default name of Rex
            if (string.IsNullOrWhiteSpace(name))
                name = "Rex";

            // properly formats the name so the first letter will be capitalized
            name = name.Substring(0, 1).ToUpper() + name.Substring(1, name.Length - 1).ToLower();

            // instantiate the weapons, items, areas, and the player
            CreateWeapons();
            CreateItems();
            CreateAreas();
            Player = new Player("Drau " + name, 100, 100, 50, 50, 6, 0);

            // loops while the player is setting their stats
            do
            {
                inputValid = false;
                // loops while the player's points is more than 0
                while (Player.Points > 0)
                {
                    Console.Clear();
                    DisplayBeginningStatsSelectMenu();
                    GameCursor.Print();
                    TimeInterval();
                    GameCursor.FirstMenuMoveAndSelection(6, 7, Player);
                }
                Console.Clear();
                DisplayBeginningStatsSelectMenu();
                GameCursor.Print();
                Console.ForegroundColor = ConsoleColor.White;
                PrintAtPosition(0, 10, "Are you sure you want to continue with these skills? (Y/N)");
                // this loop only accepts a Y or N input
                do
                {
                    input = Console.ReadKey(true).Key;
                    if (input == ConsoleKey.Y || input == ConsoleKey.N)
                        inputValid = true;
                } while (!inputValid);
                // resets the player's stats if they chose to reset them
                if (input == ConsoleKey.N)
                    ResetStats();
            } while (input == ConsoleKey.N);

            GameCursor.choice = 0;
            GameCursor.Y = 16;
            Console.Clear();
            Player.WeaponInventory.Add(GameWeapons[2]);
        }

        /// <summary>
        /// Resets the player's stats
        /// </summary>
        void ResetStats()
        {
            Player.Strength = 50;
            Player.Dexterity = 50;
            Player.Points = 6;
        }

        /// <summary>
        /// Prints output starting from a specific X and Y coordinate
        /// </summary>
        /// <param name="x">X coordinate to start at</param>
        /// <param name="y">Y coordinate to start at</param>
        /// <param name="text">Text to output</param>
        void PrintAtPosition(int x, int y, string text)
        {
            Console.SetCursorPosition(x, y);
            WordWrap(text);
        }

        /// <summary>
        /// Automatically wraps output to fit into the console window
        /// </summary>
        /// <param name="text">Text to output</param>
        public static void WordWrap(String text)
        {
            // make a string array with the words placed into each element
            String[] words = text.Split(' ');
            StringBuilder buffer = new StringBuilder();

            foreach (String word in words)
            {
                // adds a word to the buffer StringBuilder object
                buffer.Append(word);

                if (buffer.Length >= Console.WindowWidth - 1)
                {
                    // a line of text will be outputted and the buffer cleared
                    String line = buffer.ToString().Substring(0, buffer.Length - word.Length);
                    Console.WriteLine(line);
                    buffer.Clear();
                    // adds the next word to the buffer
                    buffer.Append(word);
                }
                // add a space to the buffer
                buffer.Append(" ");

            }

            Console.WriteLine(buffer.ToString());
        }

        /// <summary>
        /// Sets the time interval to allow the user to move the cursor around in "real time"
        /// </summary>
        void TimeInterval()
        {
            Thread.Sleep(100);
        }

        /// <summary>
        /// Calculates a character's defense based on their strength. A defense of 50 makes the character take the full brunt of the attack. A defense of 1 multiplies the damage by 2
        /// and a defense of 100 halves the damage
        /// </summary>
        /// <param name="strength">The character's strength</param>
        /// <param name="baseDamage">The base damage of the weapon that is being used in the attack</param>
        /// <returns>Returns defense calculation</returns>
        public static int DefenseCalculation(int strength, int baseDamage)
        {
            double maxDamage = (double)baseDamage * 1.5;
            double minDamage = (double)baseDamage / 2;

            return (int)Math.Ceiling(maxDamage - ((maxDamage - minDamage) * ((double)strength / 100)));
        }

        /// <summary>
        /// Calculates a character's melee offense based on their strength. An offense of 50 is normal attack power, an offense of 100 doubles the attack power, and an offense of 1
        /// halves the attack power
        /// </summary>
        /// <param name="strength">The character's strength</param>
        /// <param name="meleeBasePower">The base damage of the character's melee weapon</param>
        /// <returns>Returns new melee weapon power</returns>
        public static int MeleeOffenseCalculation(int strength, int meleeBasePower)
        {
            double minDamage = (double)meleeBasePower / 2;
            double maxDamage = (double)meleeBasePower * 1.5;

            return (int)Math.Floor(minDamage - ((minDamage - maxDamage) * ((double)strength / 100)));
        }

        /// <summary>
        /// Calculates the hit chance
        /// </summary>
        /// <param name="offenderDexterity">The offender's dexterity</param>
        /// <param name="defenderDexterity">The deffender's dexterity</param>
        /// <returns>Returns true if hit, false if miss</returns>
        public static bool HitChance(int offenderDexterity, int defenderDexterity)
        {
            int hitChance = 50 + ((offenderDexterity - defenderDexterity) / 2);
            Random rng = new Random();
            int rand = rng.Next(0, 101);
            if (rand < hitChance)
                return true;
            else
                return false;
        }

        #region Display Functions
        /// <summary>
        /// Function to clear the console and makes the text white
        /// </summary>
        void ClearAndMakeTextWhite()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
        }

        /// <summary>
        /// Displays the title screen
        /// </summary>
        void DisplayTitleScreen()
        {
            Console.WriteLine("\n\n -MMMMMMMN-       `hMMMMMMM+       `sMMMMMN/        `+ydmNNMMMMMMMMMMMMM/  :shmNNNNNMMNNNNMMMNh`");
            Console.WriteLine(" -MMMMMMMMN:     .dMMMMMMMM+      -dMMMMMMMMy`     -NMMMMMmddddddddddddd:`sMMMMMNmddddddddddddy`");
            Console.WriteLine(" -NNNNNNNNNm:   .dNNNNNNNNN/     /mNNmd+yyyys+`    :ooooo/-----------``  `++++++/::::::::--.``");
            Console.WriteLine(" -mmmmmsdmmmd: -dmmmmommmdh:   `+sssso-..oddddd+`  `sdmmmmmmmmmmmmddmmdy:`:ydddddddddddddddmdho`");
            Console.WriteLine(" .ddddd+-ydddh+hhyyy/-ysyyh/  -hdddddddddddddddds.   `-:/+++++++++ohhhhhh-  .://+++++++++shhhhho`");
            Console.WriteLine(" .hhhhh+`.osssssyhh/`:hhhhh:`/hhhhyoooooooooyyyyys-`:+++++++++++++oyyyyyy.`++++++++++++++syyyyy+`");
            Console.WriteLine(" .sssss+` .oyyyyys:  :yyyyy:syyyys:         .osssss/+ssssssssssssssssso+. `yssssssssssssssssso:`");
            Console.WriteLine(" `------`  `-----.   .-----------.           `-----------------......`    `----.....-----...``");
            Console.WriteLine(" `/yhdhhhhh.       -shdhhhhh+`      -shdhhhhh+`     .oydhhhhhs`      -shdhhhhh/     /hhhdmdhhho");
            Console.WriteLine(" oNNdoooooo`      `mNmoooooo:      `mmmoooooo:     `hmmyooooo/`     -mmh.              `hmd`");
            Console.WriteLine(" :yhs+/////`      `dho-......      `hho-......     `oyyo/////:`     .sys:-----.        `shy`");
            Console.WriteLine("  `-::////:`      `//-`            `/:-`             `-::::::-`      `.-::::::-        `:::`");
            Console.WriteLine("\n\n88888 8              .d88b  8                      w      888b.       8          8 8 w             ");
            Console.WriteLine("  8   8d8b. .d88b    8P www 8d8b. 8   8 8d8b d88b w8ww    8  .8 .d88b 88b. .d88b 8 8 w .d8b. 8d8b. ");
            Console.WriteLine("  8   8P Y8 8.dP'    8b  d8 8P Y8 8b d8 8P   `Yb.  8      8wwK' 8.dP' 8  8 8.dP' 8 8 8 8' .8 8P Y8 ");
            Console.WriteLine("  8   8   8 `Y88P    `Y88P' 8   8 `Y8P8 8    Y88P  Y8P    8  Yb `Y88P 88P' `Y88P 8 8 8 `Y8P' 8   8 ");
            Console.WriteLine("                                                                                                   ");

            Console.WriteLine("\n\n\n\n\t\t\t\tPress any key to start . . .");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Displays the battle sequence
        /// </summary>
        void DisplayBattle()
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 16;
            bool choice = false;

            // loops while a choice hasn't been made and both player and enemy are still alive
            while (!choice && Player.HP > 0 && Enemy.HP > 0)
            {
                ClearAndMakeTextWhite();
                DisplayBattleInfo();
                DisplayBattleMenu();
                GameCursor.Print();
                TimeInterval();
                GameCursor.MenuMove(16, 17, 2);
                if (GameCursor.ChoiceMade)
                    choice = true;
            }

            // displays when the enemy dies
            if (Enemy.HP <= 0)
            {
                Console.Clear();
                Console.WriteLine("WINRAR IS YOU.");
                Console.ReadKey();
                return;
            }
            // displays when the player dies
            else if (Player.HP <= 0)
            {
                Console.Clear();
                WordWrap("With one last breath, you collapse against the ground. You lie there motionless as the world around you fades away into darkness. You have died!");
                Console.WriteLine("\nPress any key to continue . . .");
                Console.ReadKey();
                return;
            }

            // the choice that the player chooses from the menu
            switch (GameCursor.SelectedChoice)
            {
                case 0:
                    DisplayBattleAttack();
                    break;
                case 1:
                    Console.WriteLine("This hasn't been implemented yet!");
                    Console.ReadKey();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Displays the attack menu
        /// </summary>
        void DisplayBattleAttack()
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 16;
            bool choice = false;
            while (!choice)
            {
                ClearAndMakeTextWhite();
                DisplayBattleInfo();
                int position = 16;
                // prints each weapon on the screen
                foreach (Weapon weapon in Player.WeaponInventory)
                {
                    PrintAtPosition(6, position, weapon.Name);
                    position++;
                }
                PrintAtPosition(6, position, "Go back");
                // displays the weapon's stats
                if (GameCursor.choice != Player.WeaponInventory.Count())
                {
                    PrintAtPosition(40, 16, Player.WeaponInventory[GameCursor.choice].Name);

                    if (Player.WeaponInventory[GameCursor.choice].IsRanged)
                        PrintAtPosition(40, 17, "Ranged weapon");
                    else
                        PrintAtPosition(40, 17, "Melee weapon");
                    PrintAtPosition(40, 18, String.Format("Power rating: {0}", Player.WeaponInventory[GameCursor.choice].Power));
                }
                GameCursor.Print();
                TimeInterval();
                GameCursor.MenuMove(16, Player.WeaponInventory);
                if (GameCursor.ChoiceMade)
                    choice = true;
            }
            // returns back to the main battle menu
            if (GameCursor.SelectedChoice > Player.WeaponInventory.Count() - 1)
                DisplayBattle();
            else
                BattleSequence();
        }

        /// <summary>
        /// Displays the battle stats
        /// </summary>
        void DisplayBattleInfo()
        {
            Console.WriteLine("You are engaged in a battle!\n\n\n");
            Console.WriteLine(Player.Name);
            Console.WriteLine("Kinetic Barrier: {0}", Player.KineticBarrier);
            Console.WriteLine("HP: {0}\n", Player.HP);
            Console.WriteLine(Enemy.Name);
            Console.WriteLine("Kinetic Barrier: {0}", Enemy.KineticBarrier);
            Console.WriteLine("HP: {0}\n", Enemy.HP);
        }

        /// <summary>
        /// Displays the battle sequence as both characters attack
        /// </summary>
        void BattleSequence()
        {
            ClearAndMakeTextWhite();
            Random rng = new Random();
            Player.Attack(Enemy, Player.WeaponInventory[GameCursor.SelectedChoice]);
            Console.WriteLine();
            Weapon enemyWeapon = Enemy.WeaponInventory[rng.Next(Enemy.WeaponInventory.Count() - 1)];
            Enemy.Attack(Player, enemyWeapon);
            Console.WriteLine("\nPress any key to continue . . .");
            Console.ReadKey();
            DisplayBattle();
        }
        
        /// <summary>
        /// Displays the area information
        /// </summary>
        void DisplayArea()
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 16;
            bool choice = false;
            while (!choice)
            {
                ClearAndMakeTextWhite();
                Console.WriteLine("You're currently in {0}\n\n\n", GameAreas[Player.CurrentArea].Name);
                WordWrap(GameAreas[Player.CurrentArea].AreaDescription);
                DisplayMenu();
                GameCursor.Print();
                TimeInterval();
                GameCursor.MenuMove(16, 18, 3);
                if (GameCursor.ChoiceMade)
                    choice = true;
            }

            // displays the correct next menu dependent on the player's choice
            switch (GameCursor.SelectedChoice)
            {
                case 0:
                    DisplayAreaInvestigation();
                    break;
                case 1:
                    DisplayInventory();
                    break;
                case 2:
                    //Player.CurrentArea = 1;
                    // debug
                    DisplayBattle();
                    // end debug
                    //DisplayArea();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Displays the area investigation menu
        /// </summary>
        void DisplayAreaInvestigation()
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 16;
            bool choice = false;
            while (!choice)
            {
                ClearAndMakeTextWhite();
                Console.WriteLine("You're currently in {0}\n\n\n", GameAreas[Player.CurrentArea].Name);
                WordWrap(GameAreas[Player.CurrentArea].InvestigateDescription);
                DisplayInvestigateMenu();
                GameCursor.Print();
                TimeInterval();
                GameCursor.MenuMove(16, 18, 3);
                if (GameCursor.ChoiceMade)
                    choice = true;
            }

            // goes to the correct next menu dependent on the player's choice
            switch (GameCursor.SelectedChoice)
            {
                case 0:
                    DisplayWeaponsInAreaMenu();
                    break;
                case 1:
                    DisplayItemsInAreaMenu();
                    break;
                case 2:
                    DisplayArea();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// displays the player's inventory
        /// </summary>
        void DisplayInventory()
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 6;
            bool choice = false;
            while (!choice)
            {
                ClearAndMakeTextWhite();
                Console.WriteLine("You're currently in {0}\n\n\n", GameAreas[Player.CurrentArea].Name);
                Console.WriteLine("You're in your inventory.");
                DisplayInventoryMenu();
                GameCursor.Print();
                TimeInterval();
                GameCursor.MenuMove(6, 8, 3);
                if (GameCursor.ChoiceMade)
                    choice = true;
            }

            // displays the next correct menu dependent on the player's choice
            switch (GameCursor.SelectedChoice)
            {
                case 0:
                    DisplayPlayerWeaponsMenu();
                    break;
                case 1:
                    DisplayPlayerItemsMenu();
                    break;
                case 2:
                    DisplayArea();
                    break;
                default:
                    break;
            }
        }

        // various menu choices
        void DisplayMenu()
        {
            PrintAtPosition(6, 16, "Investigate");
            PrintAtPosition(6, 17, "Inventory");
            PrintAtPosition(6, 18, "Exit area");
        }

        void DisplayInvestigateMenu()
        {
            PrintAtPosition(6, 16, "Weapons in this area");
            PrintAtPosition(6, 17, "Items in this area");
            PrintAtPosition(6, 18, "Go back");
        }

        void DisplayInventoryMenu()
        {
            PrintAtPosition(6, 6, "Your weapons");
            PrintAtPosition(6, 7, "Your items");
            PrintAtPosition(6, 8, "Go back");
        }

        void DisplayObjectsInAreaMenu()
        {
            PrintAtPosition(6, 16, "Equip");
            PrintAtPosition(6, 17, "Go back");
        }

        void DisplayObjectsInInventoryMenu()
        {
            PrintAtPosition(6, 16, "Drop");
            PrintAtPosition(6, 17, "Go back");
        }

        void DisplayBattleMenu()
        {
            PrintAtPosition(6, 16, "Attack");
            PrintAtPosition(6, 17, "Inventory");
        }

        /// <summary>
        /// Displays the area's weapons
        /// </summary>
        void DisplayWeaponsInAreaMenu()
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 6;
            bool choice = false;
            while (!choice)
            {
                ClearAndMakeTextWhite();
                Console.WriteLine("You're currently in {0}\n\n\n", GameAreas[Player.CurrentArea].Name);
                if (GameAreas[Player.CurrentArea].WeaponsInArea.Count() == 0)
                    Console.WriteLine("There are no weapons in this area.");
                else
                    Console.WriteLine("These are the weapons in this area:");
                int position = 6;
                foreach (Weapon weapon in GameAreas[Player.CurrentArea].WeaponsInArea)
                {
                    PrintAtPosition(6, position, weapon.Name);
                    position++;
                }
                PrintAtPosition(6, position, "Go back");
                GameCursor.Print();
                TimeInterval();
                GameCursor.MenuMove(6, GameAreas[Player.CurrentArea].WeaponsInArea);
                if (GameCursor.ChoiceMade)
                    choice = true;
            }

            // goes back if a weapon isn't chosen, otherwise a weapon is chosen
            if (GameCursor.SelectedChoice > GameAreas[Player.CurrentArea].WeaponsInArea.Count() - 1)
                DisplayAreaInvestigation();
            else
                DisplayWeaponInfoMenu(GameCursor.SelectedWeapon, true, GameCursor.SelectedChoice);
        }

        /// <summary>
        /// Displays a weapon's information
        /// </summary>
        /// <param name="weapon">The weapon to look at</param>
        /// <param name="fromArea">This judges if the weapon is in the area or the player's inventory</param>
        /// <param name="weaponIndex">The index from the list the weapon is coming from</param>
        void DisplayWeaponInfoMenu(Weapon weapon, bool fromArea, int weaponIndex)
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 16;
            bool choice = false;
            while (!choice)
            {
                ClearAndMakeTextWhite();
                Console.WriteLine("{0}", weapon.Name);
                Console.WriteLine("\n{0}", weapon.Description);
                if (weapon.IsRanged)
                    Console.WriteLine("\nRanged weapon");
                else
                    Console.WriteLine("\nMelee weapon");
                Console.WriteLine("\nPower rating: {0}", weapon.Power);
                if (fromArea)
                    DisplayObjectsInAreaMenu();
                else if (weaponIndex == 0)
                    PrintAtPosition(6, 16, "Go back");
                else
                    DisplayObjectsInInventoryMenu();
                GameCursor.Print();
                TimeInterval();
                if (weaponIndex == 0)
                    GameCursor.MenuMove(16, 16, 1);
                else
                    GameCursor.MenuMove(16, 17, 2);
                if (GameCursor.ChoiceMade)
                    choice = true;
            }
            if (fromArea)
            {
                if (GameCursor.SelectedChoice == 0)
                    DisplayWeaponPickedUp(weapon, weaponIndex);
                else
                    DisplayWeaponsInAreaMenu();
            }
            else
            {
                if (GameCursor.SelectedChoice == 0 && weaponIndex != 0)
                    DisplayWeaponDropped(weapon, weaponIndex);
                else
                    DisplayPlayerWeaponsMenu();
            }
        }

        /// <summary>
        /// Displays the weapon being picked up
        /// </summary>
        /// <param name="weapon">The weapon being picked up</param>
        /// <param name="listIndex">The index value from the list where the weapon is coming from</param>
        void DisplayWeaponPickedUp(Weapon weapon, int listIndex)
        {
            ClearAndMakeTextWhite();
            Console.WriteLine("You've picked up the {0}.\n", weapon.Name);
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey();
            Player.WeaponInventory.Add(GameAreas[Player.CurrentArea].WeaponsInArea[listIndex]);
            GameAreas[Player.CurrentArea].WeaponsInArea.RemoveAt(listIndex);
            DisplayWeaponsInAreaMenu();
        }

        /// <summary>
        /// Displays the weapon being dropped
        /// </summary>
        /// <param name="weapon">THe weapon being dropped</param>
        /// <param name="listIndex">The index value from the list wher the weapon will be dropped from</param>
        void DisplayWeaponDropped(Weapon weapon, int listIndex)
        {
            ClearAndMakeTextWhite();
            Console.WriteLine("You've dropped the {0}.\n", weapon.Name);
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey();
            GameAreas[Player.CurrentArea].WeaponsInArea.Add(Player.WeaponInventory[listIndex]);
            Player.WeaponInventory.RemoveAt(listIndex);
            DisplayPlayerWeaponsMenu();
        }

        /// <summary>
        /// Displays the items that are in the area
        /// </summary>
        void DisplayItemsInAreaMenu()
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 6;
            bool choice = false;
            while (!choice)
            {
                ClearAndMakeTextWhite();
                Console.WriteLine("You're currently in {0}\n\n\n", GameAreas[Player.CurrentArea].Name);
                if (GameAreas[Player.CurrentArea].ItemsInArea.Count() == 0)
                    Console.WriteLine("There are no items in this area.");
                else
                    Console.WriteLine("These are the items in this area:");
                int position = 6;
                foreach (Item item in GameAreas[Player.CurrentArea].ItemsInArea)
                {
                    PrintAtPosition(6, position, item.Name);
                    position++;
                }
                PrintAtPosition(6, position, "Go back");
                GameCursor.Print();
                TimeInterval();
                GameCursor.MenuMove(6, GameAreas[Player.CurrentArea].ItemsInArea);
                if (GameCursor.ChoiceMade)
                    choice = true;
            }

            if (GameCursor.SelectedChoice > GameAreas[Player.CurrentArea].ItemsInArea.Count() - 1)
                DisplayAreaInvestigation();
            else
                DisplayItemInfoMenu(GameCursor.SelectedItem, true, GameCursor.SelectedChoice);
        }

        /// <summary>
        /// Displays the information of a particular item
        /// </summary>
        /// <param name="item">Item to look at</param>
        /// <param name="fromArea">Is the weapon in the area or the player's inventory?</param>
        /// <param name="itemIndex">The index value from the list the weapon is in</param>
        void DisplayItemInfoMenu(Item item, bool fromArea, int itemIndex)
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 16;
            bool choice = false;
            while (!choice)
            {
                ClearAndMakeTextWhite();
                Console.WriteLine("{0}", item.Name);
                Console.WriteLine("\n{0}", item.Description);

                if (fromArea)
                    DisplayObjectsInAreaMenu();
                else
                    DisplayObjectsInInventoryMenu();
                GameCursor.Print();
                TimeInterval();
                GameCursor.MenuMove(16, 17, 2);
                if (GameCursor.ChoiceMade)
                    choice = true;
            }
            if (fromArea)
            {
                if (GameCursor.SelectedChoice == 0)
                    DisplayItemPickedUp(item, itemIndex);
                else
                    DisplayItemsInAreaMenu();
            }
            else
            {
                if (GameCursor.SelectedChoice == 0)
                    DisplayItemDropped(item, itemIndex);
                else
                    DisplayPlayerItemsMenu();
            }
        }

        /// <summary>
        /// Displays an item being picked up
        /// </summary>
        /// <param name="item">The item being picked up</param>
        /// <param name="itemIndex">The index from the list the item is from</param>
        void DisplayItemPickedUp(Item item, int itemIndex)
        {
            ClearAndMakeTextWhite();
            Console.WriteLine("You've picked up the {0}.\n", item.Name);
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey();
            Player.ItemInventory.Add(GameAreas[Player.CurrentArea].ItemsInArea[itemIndex]);
            GameAreas[Player.CurrentArea].ItemsInArea.RemoveAt(itemIndex);
            DisplayItemsInAreaMenu();
        }

        /// <summary>
        /// Displays an item being dropped
        /// </summary>
        /// <param name="item">The item to drop</param>
        /// <param name="listIndex">The index from the list the item is from</param>
        void DisplayItemDropped(Item item, int listIndex)
        {
            ClearAndMakeTextWhite();
            Console.WriteLine("You've dropped the {0}.\n", item.Name);
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey();
            GameAreas[Player.CurrentArea].ItemsInArea.Add(Player.ItemInventory[listIndex]);
            Player.ItemInventory.RemoveAt(listIndex);
            DisplayPlayerItemsMenu();
        }

        /// <summary>
        /// Displays the weapons in the player's inventory
        /// </summary>
        void DisplayPlayerWeaponsMenu()
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 6;
            bool choice = false;
            while (!choice)
            {
                ClearAndMakeTextWhite();
                Console.WriteLine("You're currently in {0}\n\n\n", GameAreas[Player.CurrentArea].Name);
                if (Player.WeaponInventory.Count() == 0)
                    Console.WriteLine("You have no weapons.");
                else
                    Console.WriteLine("These are the weapons in your inventory:");
                int position = 6;
                foreach (Weapon weapon in Player.WeaponInventory)
                {
                    PrintAtPosition(6, position, weapon.Name);
                    position++;
                }
                PrintAtPosition(6, position, "Go back");
                GameCursor.Print();
                TimeInterval();
                GameCursor.MenuMove(6, Player.WeaponInventory);
                if (GameCursor.ChoiceMade)
                    choice = true;
            }

            if (GameCursor.SelectedChoice > Player.WeaponInventory.Count() - 1)
                DisplayInventory();
            else
                DisplayWeaponInfoMenu(GameCursor.SelectedWeapon, false, GameCursor.SelectedChoice);
        }

        /// <summary>
        /// Displays the items in the player's inventory
        /// </summary>
        void DisplayPlayerItemsMenu()
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 6;
            bool choice = false;
            while (!choice)
            {
                ClearAndMakeTextWhite();
                Console.WriteLine("You're currently in {0}\n\n\n", GameAreas[Player.CurrentArea].Name);
                if (Player.ItemInventory.Count() == 0)
                    Console.WriteLine("You have no items.");
                else
                    Console.WriteLine("These are the items in your inventory:");
                int position = 6;
                foreach (Item item in Player.ItemInventory)
                {
                    PrintAtPosition(6, position, item.Name);
                    position++;
                }
                PrintAtPosition(6, position, "Go back");
                GameCursor.Print();
                TimeInterval();
                GameCursor.MenuMove(6, Player.ItemInventory);
                if (GameCursor.ChoiceMade)
                    choice = true;
            }

            if (GameCursor.SelectedChoice > Player.ItemInventory.Count() - 1)
                DisplayInventory();
            else
                DisplayItemInfoMenu(GameCursor.SelectedItem, false, GameCursor.SelectedChoice);

        }

        /// <summary>
        /// Displays the player's stats in the beginning of the game
        /// </summary>
        void DisplayBeginningStatsSelectMenu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            WordWrap(String.Format("You are {0}, a krogan citizen of the Republic of Ghurst on Tuchanka. You start with {1} points that you can spend freely to increase any of your " +
            "stats listed below. Choose carefully, as you won't be able to modify your choice after this screen!", Player.Name, Player.maxPoints));
            Console.WriteLine("\n\tPoints left: {0}", Player.Points);
            PrintAtPosition(6, 6, "Dexterity: " + Player.Dexterity);
            PrintAtPosition(6, 7, "Strength: " + Player.Strength);
        }
        #endregion

        #region Create Functions
        /// <summary>
        /// instantiate the game's weapons
        /// </summary>
        void CreateWeapons()
        {
            GameWeapons = new List<Weapon>()
            { 
                new Weapon("M-300 Claymore", "A very powerful krogan shotgun that pretty much wipes away anything that it's pointed at.", true, 90), // 0
                new Weapon("Graal Spike Thrower", "A long line of krogan weapons to hunt thresher maws.", true, 75), // 1
                new Weapon("Headbutt", "Use your head to attack things!", false, 40) // 2
            };
        }

        /// <summary>
        /// instantiates the game's items
        /// </summary>
        void CreateItems()
        {
            GameItems = new List<Item>() 
            {
                new Item("Medi-gel", "This heals you, stupid.") // 0
            };
        }

        /// <summary>
        /// instantiates the game's areas
        /// </summary>
        void CreateAreas()
        {
            GameAreas = new List<Area>()
            {
                new Area("your house", "You start off at home in your disheveled bed. Sounds of shouting cause you to awaken. As you look out of your window to see what's going on, the signature sound of a Claymore " + 
                    "shotgun fills the air. After a slight pause, more Claymores are shot and then there's silence. You soon see corpses dragged away and people soon continue on with their business. With a yawn and " +
                    "a stretch, you shrug off the occurrence. Time to go about your business.", "Your house is nothing special. Just as most areas of Tuchanka, it's run down, dirty and completely unorganized. It'd " +
                    "probably be a good idea to not forget to bring your Claymore with you before leaving. You never know what you might run in to outside."), // 0
                new Area("Ghurst Square", "As you step outside, you notice fresh trails of orange fluid splattered upon the street. No one seems to bother cleaning it up. You look up to the sky and tap against your " +
                    "chin in thought. What was all of that commotion about, anyway? Maybe you'll find out later as time goes on.", "You look around the area for anything valuable. You see a few varren pups playing " +
                    "in the street with each other. Their owners are talking with each other."), // 1               
            };

            GameAreas[0].WeaponsInArea.Add(GameWeapons[0]);
            GameAreas[0].ItemsInArea.Add(GameItems[0]);
        }
        #endregion
    }
}
