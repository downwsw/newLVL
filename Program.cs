using System;

namespace HomeworkMoney
{
    // Класс для нашей ошибки "Банкрут"
    // Наследуемся от ApplicationException, как в задании
    class BankruptException : ApplicationException
    {
        public BankruptException(string message) : base(message)
        {
        }
    }

    class Money
    {
        // Поля делаем public, чтобы было проще (хотя профи делают private)
        public int Hryvnia { get; set; }
        public int Kopecks { get; set; }

        public Money(int h, int k)
        {
            Hryvnia = h;
            Kopecks = k;
            Normalize(); // Сразу приводим в порядок (чтобы не было 120 копеек)
        }

        // Вспомогательный метод, чтобы пересчитать копейки в гривны
        private void Normalize()
        {
            if (Kopecks >= 100)
            {
                Hryvnia += Kopecks / 100;
                Kopecks = Kopecks % 100;
            }
        }

        // Метод для получения общей суммы в копейках (так легче считать)
        public int ToTotalKopecks()
        {
            return Hryvnia * 100 + Kopecks;
        }

        // Перегрузка +
        public static Money operator +(Money m1, Money m2)
        {
            int total = m1.ToTotalKopecks() + m2.ToTotalKopecks();
            return new Money(0, total);
        }

        // Перегрузка -
        public static Money operator -(Money m1, Money m2)
        {
            int total = m1.ToTotalKopecks() - m2.ToTotalKopecks();
            if (total < 0)
            {
                throw new BankruptException("Ошибка: Банкрут! Сумма стала отрицательной.");
            }
            return new Money(0, total);
        }

        // Перегрузка * (умножение на целое число)
        public static Money operator *(Money m, int multiplier)
        {
            int total = m.ToTotalKopecks() * multiplier;
            return new Money(0, total);
        }

        // Перегрузка / (деление на целое число)
        public static Money operator /(Money m, int divider)
        {
            if (divider == 0) throw new DivideByZeroException("На ноль делить нельзя!");
            
            int total = m.ToTotalKopecks() / divider;
            return new Money(0, total);
        }

        // Перегрузка ++ (добавляем 1 копейку)
        public static Money operator ++(Money m)
        {
            int total = m.ToTotalKopecks() + 1;
            return new Money(0, total);
        }

        // Перегрузка -- (убираем 1 копейку)
        public static Money operator --(Money m)
        {
            int total = m.ToTotalKopecks() - 1;
            if (total < 0)
            {
                throw new BankruptException("Ошибка: Банкрут при уменьшении на копейку!");
            }
            return new Money(0, total);
        }

        // Операторы сравнения
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

        // Это нужно, чтобы не ругался компилятор на == и !=
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // Чтобы красиво выводить на экран
        public override string ToString()
        {
            return $"{Hryvnia} грн. {Kopecks:D2} коп.";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Для вывода кириллицы в консоли
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Демонстрация класса Money ===");
            
            // Создаем начальную сумму
            Money myMoney = new Money(10, 50); // 10 грн 50 коп

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
                        myMoney = myMoney - sub; // Тут может вылететь Банкрут!
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
                        myMoney--; // Тут тоже может быть Банкрут
                    }
                    else if (choice == "7")
                    {
                        Console.Write("Введите гривны для сравнения: ");
                        int h = int.Parse(Console.ReadLine());
                        int k = 0; // упростим, без копеек
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