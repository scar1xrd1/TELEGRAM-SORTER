using TelegramSorterSpace;

TelegramSorter telegramSorter = new TelegramSorter();

//telegramSorter.SortByPath("C:\\Users\\Anton\\Downloads\\Telegram Desktop\\");

while (true)
{
    Console.Write("1. Сортировка по пути\n2. Сортировка\n-> ");
    string user = Console.ReadLine(); Console.Clear();

    if (user == "1")
    {
        Console.Write("Введите путь -> ");
        string path = Console.ReadLine(); Console.Clear();
        telegramSorter.SortByPath(path);
    }
    else if (user == "2") telegramSorter.Sort();
}