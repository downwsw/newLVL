using System;

namespace HomeworkMoney
{
    class BankruptException : ApplicationException
    {
        public BankruptException(string message) : base(message)
        {
        }
    }

    class Money
    {
        public int Hryvnia { get; set; }
        public int Kopecks { get; set; }

        public Money(int h, int k)
        {
            Hryvnia = h;
            Kopecks = k;
            Normalize();
        }

        private void Normalize()
        {
            if (Kopecks >= 100)
            {
                Hryvnia += Kopecks / 100;
                Kopecks = Kopecks % 100;
            }
        }

        public int ToTotalKopecks()
        {
            return Hryvnia * 100 + Kopecks;
        }

        public static Money operator +(Money m1, Money m2)
        {
            int total = m1.ToTotalKopecks() + m2.ToTotalKopecks();
            return new Money(0, total);
        }

        public static Money operator -(Money m1, Money m2)
        {
            int total = m1.ToTotalKopecks() - m2.ToTotalKopecks();
            if (total < 0)
            {
                throw new BankruptException("Ошибка: Банкрут! Сумма стала отрицательной.");
            }
            return new Money(0, total);
        }

        public static Money operator *(Money m, int multiplier)
        {
            int total = m.ToTotalKopecks() * multiplier;
            return new Money(0, total);
        }

        public static Money operator /(Money m, int divider)
        {
            if (divider == 0) throw new DivideByZeroException("На ноль делить нельзя!");
            
            int total = m.ToTotalKopecks() / divider;
            return new Money(0, total);
        }

        public static Money operator ++(Money m)
        {
            int total = m.ToTotalKopecks() + 1;
            return new Money(0, total);
        }

        public static Money operator --(Money m)
        {
            int total = m.ToTotalKopecks() - 1;
            if (total < 0)
            {
                throw new BankruptException("Ошибка: Банкрут при уменьшении на копейку!");
            }
            return new Money(0, total);
        }

        public static bool operator >(Money m1, Money m2)
        {
            return m1.ToTotalKopecks() > m2.ToTotalKopecks();
        }

        public static bool operator <(Money m1, Money m2)
        {
            return m1.ToTotalKopecks() < m2.ToTotalKopecks();
        }

        public static bool operator ==(Money m1, Money m2)
        {
            return m1.ToTotalKopecks() == m2.ToTotalKopecks();
        }

        public static bool operator !=(Money m1, Money m2)
        {
            return m1.ToTotalKopecks() != m2.ToTotalKopecks();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Hryvnia} грн. {Kopecks:D2} коп.";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Демонстрация класса Money ===");
            
            Money myMoney = new Money(10, 50); 

            while (true)
            {
                Console.WriteLine($"\nТекущий баланс: {myMoney}");
                Console.WriteLine("1. Добавить сумму (+)");
                Console.WriteLine("2. Отнять сумму (-)");
                Console.WriteLine("3. Умножить на число (*)");
                Console.WriteLine("4. Разделить на число (/)");
                Console.WriteLine("5. ++ (плюс копейка)");
                Console.WriteLine("6. -- (минус копейка)");
                Console.WriteLine("7. Сравнить с другой суммой");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                try
                {
                    if (choice == "0") break;

                    else if (choice == "1")
                    {
                        Console.Write("Введите гривны: ");
                        int h = int.Parse(Console.ReadLine());
                        Console.Write("Введите копейки: ");
                        int k = int.Parse(Console.ReadLine());
                        Money add = new Money(h, k);
                        myMoney = myMoney + add;
                    }
                    else if (choice == "2")
                    {
                        Console.Write("Введите гривны: ");
                        int h = int.Parse(Console.ReadLine());
                        Console.Write("Введите копейки: ");
                        int k = int.Parse(Console.ReadLine());
                        Money sub = new Money(h, k);
                        myMoney = myMoney - sub; 
                    }
                    else if (choice == "3")
                    {
                        Console.Write("На сколько умножить: ");
                        int x = int.Parse(Console.ReadLine());
                        myMoney = myMoney * x;
                    }
                    else if (choice == "4")
                    {
                        Console.Write("На сколько разделить: ");
                        int x = int.Parse(Console.ReadLine());
                        myMoney = myMoney / x;
                    }
                    else if (choice == "5")
                    {
                        myMoney++;
                    }
                    else if (choice == "6")
                    {
                        myMoney--; 
                    }
                    else if (choice == "7")
                    {
                        Console.Write("Введите гривны для сравнения: ");
                        int h = int.Parse(Console.ReadLine());
                        int k = 0;
                        Money compare = new Money(h, k);
                        if (myMoney > compare) Console.WriteLine("Ваша сумма больше.");
                        else if (myMoney < compare) Console.WriteLine("Ваша сумма меньше.");
                        else Console.WriteLine("Суммы равны.");
                    }
                }
                catch (BankruptException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n!!! {ex.Message} !!!");
                    Console.WriteLine("Операция отменена.");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка ввода: " + ex.Message);
                }
            }
        }
    }
}
