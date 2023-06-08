namespace Gladiators
{
    class Programm
    {
        static void Main()
        {
            Menu menu = new Menu();
            menu.Run();
        }
    }

    class Menu
    {
        private const string MenuChooseGladiators = "1";
        private const string MenuShowAllDescription = "2";
        private const string MenuFight = "3";
        private const string MenuExit = "0";
        private Arena _arena = new Arena();

        public void Run()
        {
            bool isExit = false;
            string userInput;

            while (isExit == false)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine(MenuChooseGladiators + " - Выбрать гладиатора");
                Console.WriteLine(MenuShowAllDescription + " - Характеристики бойцов");
                Console.WriteLine(MenuFight + " - Бой!");
                Console.WriteLine(MenuExit + " - Выход");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case MenuChooseGladiators:
                        _arena.AddGladiatorToArena(ChooseGladiator());
                        break;

                    case MenuShowAllDescription:
                        ShowAllGladiators();
                        break;

                    case MenuFight:
                        _arena.PerformFight();
                        break;

                    case MenuExit:
                        isExit = true;
                        break;
                }
            }
        }

        private static Gladiator ChooseGladiator()
        {
            Gladiator[] _gladiators = new Gladiator[]
            {
                new Fighter(),
                new Rouge(),
                new Knight(),
                new Cleric(),
                new Doppelganger(),
            };

            for (int i = 0; i < _gladiators.Length; i++)
            {
                Console.WriteLine($"{i+1} - {_gladiators[i].СharClass}");
            }

            int gladiatorNumber = int.Parse(Console.ReadLine()) - 1;
            
            return _gladiators[gladiatorNumber];
         }

        private void ShowAllGladiators()
        {
            Console.WriteLine("Список гладиаторов:");

            Gladiator[] _gladiators = new Gladiator[]
            {
                new Fighter(),
                new Rouge(),
                new Knight(),
                new Cleric(),
                new Doppelganger(),
            };

            for (int i = 0; i < _gladiators.Length; i++)
            {
                Console.Write($"{_gladiators[i].СharClass} - "); _gladiators[i].ShowDescription();
                _gladiators[i].ShowShortСharacteristics();
                Console.WriteLine("-------------------------------------------------------------------------------------------------------");
            }
        }
    }

    class Arena
    {
        private Gladiator _gladiator1;
        private Gladiator _gladiator2;
        private bool _isFightReady;

        public void AddGladiatorToArena(Gladiator gladiator)
        {
            if (_gladiator1 == null)
            {
                _gladiator1 = gladiator;
                Console.WriteLine($"Выбран {_gladiator1.СharClass} {_gladiator1.Name}");
            }
            else if (_gladiator1 != null && _gladiator2 == null)
            {
                _gladiator2 = gladiator;
                Console.WriteLine($"Выбран {_gladiator2.СharClass} {_gladiator2.Name}");
                Console.WriteLine($"Идущие на смерть {_gladiator1.СharClass} {_gladiator1.Name} и {_gladiator2.СharClass} {_gladiator2.Name} приветствуют тебя!");
                _isFightReady = true;
            }
            else
            {
                Console.WriteLine("На арене уже 2 гладиатора, начните бой!");
            }
        }
        
        public void PerformFight()
        {
            if (_isFightReady == true)
            {
                ChooseFirstGladiator();

                while (_gladiator1.Health > 0 && _gladiator2.Health > 0)
                {
                    _gladiator1.ShowStatus();
                    Console.Write(" | ");
                    _gladiator2.ShowStatus();
                    Console.WriteLine();

                    _gladiator2.TakeDamage(_gladiator1.DealDamage());
                    _gladiator1.TakeDamage(_gladiator2.DealDamage());
                    Console.WriteLine("\n---------------------------------------------------------------------------------------");
                }

                DecideWin(_gladiator1, _gladiator2);
                _isFightReady = false;
            }
            else
            {
                Console.WriteLine("Для боя нужно 2 гладиатора");
            }
        }

        private void ChooseFirstGladiator()
        {
            Gladiator tempGladiator = _gladiator1;

            if (_gladiator1.Initiative < _gladiator2.Initiative)
            {
                _gladiator1 = tempGladiator;
                _gladiator1 = _gladiator2;
                _gladiator2 = tempGladiator;
            }
        }

        private void DecideWin(Gladiator gladiator1, Gladiator gladiator2)
        {
            if (gladiator1.Health > 0 && gladiator2.Health < 0)
            {
                Console.WriteLine($"Победил {gladiator1.СharClass} {gladiator1.Name}! {gladiator2.СharClass} {gladiator2.Name} - повержен!");
            }
            else if (gladiator2.Health > 0 && gladiator1.Health < 0)
            {
                Console.WriteLine($"Победил {gladiator2.СharClass} {gladiator2.Name}! {gladiator1.СharClass} {gladiator1.Name} - повержен!");
            }
            else
            {
                Console.WriteLine("Нет победителя, оба погибли.");
            }
        }
    }

    class Gladiator
    {
        protected int MaxHealth;
        protected int Armor;
        protected int MaxArmor;
        protected int HitDamage;
        protected int BaseDamage = 15;
        protected static Random Random = new Random();

        public Gladiator()
        {
            Name = GetName();
            Health = 1000;
            MaxHealth = 1000;
            Armor = 30;
            MaxArmor = 60;
            HitDamage = 100;
            Initiative = 0;
        }

        public string Name { get; protected set; }
        public string СharClass { get; protected set; }
        public int Initiative { get; protected set; }
        public int Health { get; protected set; }
        public string Description { get; protected set; }

        public virtual int DealDamage()
        {
            int alternateDamage = UseAttackAbility();

            double minDamageMod = 0.8;
            double maxDamageMod = 1.2;
            double damageMod = minDamageMod + (Random.NextDouble() * (maxDamageMod - minDamageMod));

            if (alternateDamage != 0)
            {
                int damage = alternateDamage;
                Console.Write($"{СharClass} {Name} Пытается нанести {damage} урона | ");
                return damage;
            }
            else
            {
                int damage = (int)(HitDamage * damageMod);
                Console.Write($"{СharClass} {Name} Пытается нанести {damage} урона | ");
                return damage;
            }
        }

        public virtual void TakeDamage(int damage)
        {
            UseDefenceAbility();

            int reducedDamage = damage - Armor;

            if (reducedDamage < BaseDamage)
            {
                reducedDamage = BaseDamage;
            }
            else
            {
                Health -= reducedDamage;
            }

            Console.WriteLine($"{СharClass} {Name} получает {reducedDamage} урона");
        }

        public void ShowStatus()
        {
            Console.Write($"{СharClass} {Name} - Здоровье: {Health}, Броня: {Armor}");
        }

        public void ShowDescription()
        {
            Console.WriteLine(Description);
        }

        public void ShowShortСharacteristics()
        {
            Console.WriteLine($"Урон - {HitDamage} | Здоровье - {Health} | Макс здоровье - {MaxHealth} | Броня - {Armor} | Макс Броня - {MaxArmor} | Инициатива - {Initiative}");
        }

        protected virtual void UseDefenceAbility()
        {
        }

        protected virtual int UseAttackAbility()
        {
            return 0;
        }

        private string GetName()
        {
            string[] fantasyNames = new string[]
        {
    "Aethelind",
    "Brynhildr",
    "Caelan",
    "Delphinia",
    "Eldric",
    "Faldir",
    "Galadria",
    "Hadriel",
    "Ithilwen",
    "Jareth",
    "Kaelyn",
    "Lysander",
    "Morgana",
    "Niamh",
    "Orion",
    "Persephone",
    "Quillan",
    "Rhiannon",
    "Seraphina",
    "Thaddeus",
    "Ursula",
    "Vespera",
    "Wyndham",
    "Xanthia",
    "Yvaine",
    "Zephyr",
    "Arabelle",
    "Bastian",
    "Celestia",
    "Dorian"
        };

            string name = fantasyNames[Random.Next(0, fantasyNames.Length - 1)];
            return name;
        }
    }

    class Fighter : Gladiator
    {
        public Fighter()
        {
            СharClass = "Fighter";
            Description = "Сбалансированный боец со случайным начальным уроном, который останется постоянным, каждый удар";
            HitDamage = Random.Next(100, 120);
            Armor = 20;
            Initiative = 5;
        }

        public override int DealDamage()
        {
            Console.Write($"{СharClass} {Name} Пытается нанести {HitDamage} урона | ");
            return HitDamage;
        }
    }

    class Rouge : Gladiator
    {
        public Rouge()
        {
            СharClass = "Rouge";
            Description = "Вор. Боец с небольшим базовым уроном, но возможностью нанести критический удар или полностью уклониться от атаки";
            HitDamage = 70;
            Armor = 5;
            Initiative = 10;
        }

        public override void TakeDamage(int damage)
        {
            bool isDoged = IsDoge();
            int reducedDamage = damage - Armor;

            if (isDoged)
            {
                Console.WriteLine($"{СharClass} {Name} Уклонился от атаки!");
            }
            else
            {
                if (reducedDamage < BaseDamage)
                {
                    reducedDamage = BaseDamage;
                }
                else
                {
                    Health -= reducedDamage;
                }

                Console.WriteLine($"{СharClass} {Name} получает {reducedDamage} урона");
            }
        }

        protected override int UseAttackAbility()
        {
            return UseCriticalHit();
        }

        private int UseCriticalHit()
        {
            double critMultiplier = 3;
            int critChance = 25;
            int baseDamage = HitDamage;

            if (Random.Next(0, 100) < critChance)
            {
                baseDamage = Convert.ToInt32(Math.Round(baseDamage * critMultiplier));
                Console.WriteLine($"{Name} наносит критический удар!");
            }

            return baseDamage;
        }

        private bool IsDoge()
        {
            int dogeChance = 20;
            bool isDoged = false;

            if (Random.Next(0, 100) < dogeChance)
            {
                isDoged = true;
            }
            else
            {
                isDoged = false;
            }

            return isDoged;
        }
    }

    class Knight : Gladiator
    {
        private int _shieldArmor;

        public Knight()
        {
            СharClass = "Knight";
            Description = "Рыцарь. Весь в броне и со щитом, которым может воспользоваться в любой момент и увеличить свою броню";
            Armor = 30;
            MaxArmor = 80;
        }

        protected override void UseDefenceAbility()
        {
            if (_shieldArmor > 0)
            {
                Armor -= _shieldArmor;
            }

            _shieldArmor = RiseShield();

            if (Armor + _shieldArmor < MaxArmor)
            {
                Armor += _shieldArmor;
            }
            else
            {
                Armor = MaxArmor;

                Console.WriteLine("Броня максимальна!");
            }
        }

        private int RiseShield()
        {
            int getArmorChance = 30;
            int addArmor = 20;
            int extraArmor = 0;

            if (Random.Next(0, 100) < getArmorChance)
            {
                extraArmor += addArmor;
                Console.WriteLine($"{СharClass} {Name} Поднял щит, добавлено {extraArmor} брони!");
            }

            return extraArmor;
        }
    }

    class Cleric : Gladiator
    {
        public Cleric()
        {
            СharClass = "Cleric";
            Description = "Боевой храмовник. Может вылечить себя после удара";
            MaxHealth = 1300;
            Armor = 30;
            Initiative = 3;
        }

        protected override void UseDefenceAbility()
        {
            int healAmmount = Heal();

            if (Health + healAmmount > MaxHealth)
            {
                Health = MaxHealth;
            }
            else
            {
                Health += healAmmount;
            }
        }

        private int Heal()
        {
            int getHealthChance = 20;
            int addHealth = 50;
            int extraHealth = 0;

            if (Random.Next(0, 100) <= getHealthChance)
            {
                extraHealth += addHealth;
                Console.WriteLine($"{СharClass} {Name} восстанавливает: " + addHealth + " здоровья!");
            }

            return extraHealth;
        }
    }

    class Doppelganger : Gladiator
    {
        public Doppelganger()
        {
            СharClass = "Doppelganger";
            Description = "Странная раздвоенная сущность. Может, как нанести двойной урон, или разделить полученный между сущностями";
            HitDamage = 60;
            Armor = 15;
            Initiative = 7;
        }

        public override void TakeDamage(int damage)
        {
            int healthBeforeDamage = Health;
            damage = DevideDamage(damage);

            if (damage <= Armor)
            {
                Health -= DevideDamage(BaseDamage);
            }
            else
            {
                Health = Math.Max(0, Health - (damage - Armor));
            }

            Console.WriteLine($"{СharClass} {Name} Получает " + (healthBeforeDamage - Health) + " урона");
        }

        protected override int UseAttackAbility()
        {
            return DoubleHit();
        }

        private int DoubleHit()
        {
            int hitMultiplier = 2;
            int doubleHitChance = 50;
            int baseDamage = HitDamage;

            if (Random.Next(0, 100) < doubleHitChance)
            {
                baseDamage = baseDamage * hitMultiplier;
                Console.WriteLine($"{СharClass} {Name} Наносит двойной удар!");
            }

            return baseDamage;
        }

        private int DevideDamage(int damage)
        {
            int devideDamageChance = 50;
            int devideDamageBy = 2;

            if (Random.Next(0, 100) <= devideDamageChance)
            {
                damage = damage / devideDamageBy;
                Console.WriteLine($"{СharClass} {Name} Разделяет урон между своими сущностями!");
            }

            return damage;
        }
    }
}