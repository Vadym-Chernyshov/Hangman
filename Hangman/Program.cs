using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Hangman
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.GetEncoding(1251);

            bool win = false;
            //створюємо базу наших слів(взяв назви творів Шевченка)
            List<string> dbOfRandomWords = ["відьма", "варнак","утоплена","тризна","тополя",
                                            "перебендя", "неофіти", "наймичка","лілея","іржавець"];           
            //обираємо наше слово
            Random randomForGame = new();
            string randomWord = dbOfRandomWords[randomForGame.Next(0, dbOfRandomWords.Count)];

            //перші повідомлення
            Console.WriteLine($"Вітаємо! Спробуйте вгадати зашифроване слово!\n" +
                              $"Кількість літер у слові: {randomWord.Length}\n" +
                              $"Кількість можливих невірних спроб: {randomWord.Length}");

            // для виведення вже відкритих букв для користувача 
            string fillWord = new('*', randomWord.Length); //слово для заповнення
            char[] fillWordInArray = fillWord.ToCharArray();

            //так можна отримати кількість унікальних символів в слові
            int uniqueCount = randomWord.Distinct().Count();

            //логіка гри, так як  ми маємо зворотний відлік то думаю краще використати зворотній цикл
            for (int i = randomWord.Length; i > 0; i--)
            {
                //якщо кількість спроб менша за кількість унікальних символів
                //це вже програш
                if (uniqueCount > i)
                {
                    break;
                }

                Console.Write("Введіть вашу літеру: ");
                string? userInput = Console.ReadLine();                

                Console.WriteLine();
                if (char.TryParse(userInput, out char inputLetter) && //перевірка на парсинг
                    char.IsLetter(inputLetter) && //перевірка на букву
                    randomWord.Contains(char.ToLower(inputLetter)) && //перевірка на наявність у слові
                    !fillWord.Contains(char.ToLower(inputLetter))) //перевірка на повторення
                {
                    for (int j = 0; j < randomWord.Length; j++)
                    {
                        //заміняємо наші невідомі букви на вгадані літери
                        if (randomWord[j] == char.ToLower(inputLetter))
                        {
                            fillWordInArray[j] = randomWord[j];
                            uniqueCount--; //зменшуємо кількість унікальних символів
                        }
                        fillWord = new string(fillWordInArray);
                    }
                    //перевірка на перемогу
                    if (fillWord == randomWord)
                    {
                        win = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Така літера є у слові! Залишилось спроб: {i - 1}");
                        Console.WriteLine();
                        Console.WriteLine($"\t\t\t{fillWord}"); //виводимо гравцеві частину з вгаданими літерами
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine($"Такої літери немає або її вже відкрили! Залишилось спроб: {i - 1}");
                    Console.WriteLine();
                    Console.WriteLine($"\t\t\t{fillWord}");
                    Console.WriteLine();
                }
            }
            Console.WriteLine(win?
                $"Вітаємо, ви вгадали слово! Зашифроване слово: {randomWord}." :
                $"Ви програли! Зашифроване слово: {randomWord}.");
        }
    }
}
