// See https://aka.ms/new-console-template for more information
using ConsoleApp1;

Client.SetRoute();
Console.WriteLine("Введите search для поиска, add для создания, exit для выхода");


await Client.Work();

