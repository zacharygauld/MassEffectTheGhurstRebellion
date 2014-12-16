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
            Enemy = new Character("Ghurst Grunt", 100, 100, 50, 50, 50);
            Enemy.WeaponInventory.Add(GameWeapons[2]);
            DisplayArea();
        }

        void IntroSequence()
        {
            Console.Write("Enter your character's name: ");
            string name = Console.ReadLine();
            bool inputValid = false;
            ConsoleKey input;
            if (string.IsNullOrWhiteSpace(name))
                name = "Rex";
            else
                name = Regex.Replace(name, "[^A-Za-z]+", "");

            name = name.Substring(0, 1).ToUpper() + name.Substring(1, name.Length - 1).ToLower();
            Player = new Player("Drau " + name, 100, 100, 50, 50, 50, 6, 0);
            CreateWeapons();
            CreateItems();
            CreateAreas();
            do
            {
                inputValid = false;
                ResetStats(name);
                while (Player.Points > 0)
                {
                    Console.Clear();
                    DisplayBeginningStatsSelectMenu();
                    GameCursor.Print();
                    TimeInterval();
                    GameCursor.FirstMenuMoveAndSelection(6, 8, Player);
                }
                Console.Clear();
                DisplayBeginningStatsSelectMenu();
                GameCursor.Print();
                Console.ForegroundColor = ConsoleColor.White;
                PrintAtPosition(0, 10, "Are you sure you want to continue with these skills? (Y/N)");
                do
                {
                    input = Console.ReadKey(true).Key;
                    if (input == ConsoleKey.Y || input == ConsoleKey.N)
                        inputValid = true;
                } while (!inputValid);
            } while (input == ConsoleKey.N);
            GameCursor.choice = 0;
            GameCursor.Y = 16;
            Console.Clear();
            Player.WeaponInventory.Add(GameWeapons[2]);
        }
        
        void ResetStats(string name)
        {
            Player = new Player("Drau " + name, 100, 100, 50, 50, 50, 6, 0);
        }
            
        void PrintAtPosition(int x, int y, string text)
        {
            Console.SetCursorPosition(x, y);
            WordWrap(text);
        }

        public static void WordWrap(String text)
        {
            String[] words = text.Split(' ');
            StringBuilder buffer = new StringBuilder();

            foreach (String word in words)
            {
                buffer.Append(word);

                if (buffer.Length >= Console.WindowWidth - 1)
                {
                    String line = buffer.ToString().Substring(0, buffer.Length - word.Length);
                    Console.WriteLine(line);
                    buffer.Clear();
                    buffer.Append(word);
                }

                buffer.Append(" ");

            }

            Console.WriteLine(buffer.ToString());
        }

        void TimeInterval()
        {
            Thread.Sleep(100);
        }

        #region Display Functions
        void ClearAndMakeTextWhite()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
        }

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

        void DisplayBattle()
        {
            GameCursor.ChoiceMade = false;
            GameCursor.Y = 16;
            bool choice = false;
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

            if (Enemy.HP <= 0)
            {
                Console.Clear();
                Console.WriteLine("WINRAR IS YOU.");
                Console.ReadKey();
                return;
            }
            else if (Player.HP <= 0)
            {
                Console.Clear();
                WordWrap("With one last breath, you collapse against the ground. You lie there motionless as the world around you fades away into darkness. You have died!");
                Console.WriteLine("\nPress any key to continue . . .");
                Console.ReadKey();
                return;
            }

            switch (GameCursor.SelectedChoice)
            {
                case 0:
                    DisplayBattleAttack();
                    break;
                case 1:
                    break;
                default:
                    break;
            }
        }

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
                foreach (Weapon weapon in Player.WeaponInventory)
                {
                    PrintAtPosition(6, position, weapon.Name);
                    position++;
                }
                PrintAtPosition(6, position, "Go back");
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

            if (GameCursor.SelectedChoice > Player.WeaponInventory.Count() - 1)
                DisplayBattle();
            else
                BattleSequence();
        }

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

        void BattleSequence()
        {
            ClearAndMakeTextWhite();
            Random rng = new Random();
            Player.AttackEnemy(Enemy, Player.WeaponInventory[GameCursor.SelectedChoice]);
            Console.WriteLine();
            Weapon enemyWeapon = Enemy.WeaponInventory[rng.Next(Enemy.WeaponInventory.Count() - 1)];
            Enemy.AttackPlayer(Player, enemyWeapon);
            Console.WriteLine("\nPress any key to continue . . .");
            Console.ReadKey();
            DisplayBattle();
        }

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

            switch (GameCursor.SelectedChoice)
            {
                case 0:
                    DisplayAreaInvestigation();
                    break;
                case 1:
                    DisplayInventory();
                    break;
                case 2:
                    Player.CurrentArea = 1;
                    //DisplayArea();
                    DisplayBattle();
                    break;
                default:
                    break;
            }
        }

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

            if (GameCursor.SelectedChoice > GameAreas[Player.CurrentArea].WeaponsInArea.Count() - 1)
                DisplayAreaInvestigation();
            else
                DisplayWeaponInfoMenu(GameCursor.SelectedWeapon, true, GameCursor.SelectedChoice);
        }

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
        void DisplayBeginningStatsSelectMenu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            WordWrap(String.Format("You are {0}, a krogan citizen of the Republic of Ghurst on Tuchanka. You start with {1} points that you can spend freely to increase any of your " +
            "stats listed below. Choose carefully, as you won't be able to modify your choice after this screen!", Player.Name, Player.maxPoints));
            Console.WriteLine("\n\tPoints left: {0}", Player.Points);
            PrintAtPosition(6, 6, "Melee Accuracy: " + Player.MeleeAccuracy);
            PrintAtPosition(6, 7, "Ranged Accuracy: " + Player.RangedAccuracy);
            PrintAtPosition(6, 8, "Strength: " + Player.Strength);
        }
        #endregion

        #region Create Functions
        void CreateWeapons()
        {
            GameWeapons = new List<Weapon>()
            { 
                new Weapon("M-300 Claymore", "A very powerful krogan shotgun that pretty much wipes away anything that it's pointed at.", true, 90), // 0
                new Weapon("Graal Spike Thrower", "A long line of krogan weapons to hunt thresher maws.", true, 75), // 1
                new Weapon("Headbutt", "Use your head to attack things!", false, 40) // 2
            };
        }

        void CreateItems()
        {
            GameItems = new List<Item>() 
            {
                new Item("Medi-gel", "This heals you, stupid.") // 0
            };
        }

        void CreateAreas()
        {
            GameAreas = new List<Area>()
            {
                new Area("your house", "You start off at home in your disheveled bed. Sounds of shouting cause you to awaken. As you look out of your window to see what's going on, " + 
                "the signature sound of a Claymore shotgun fills the air. After a slight pause, more Claymores are shot and then there's silence. You soon see corpses dragged away and people soon continue on " +
                "with their business. With a yawn and a stretch, you shrug off the occurrence. Time to go about your business.", "Your house is nothing special. Just as most areas of Tuchanka, it's run down, dirty " +
                "and completely unorganized. It'd probably be a good idea to not forget to bring your Claymore with you before leaving. You never know what you might run in to outside."), // 0
                new Area("Test", "Nothing to describe here! It's just a test area.", "Test investigation stuff. LOL!") // 1
            };

            GameAreas[0].WeaponsInArea.Add(GameWeapons[0]);
            GameAreas[0].ItemsInArea.Add(GameItems[0]);
        }
        #endregion
    }
}
