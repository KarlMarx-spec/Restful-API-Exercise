using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Client
    {
        static HttpClient client = new HttpClient();

        public static async Task<Customer> GetCustomerAsync(int id)
        {
            Customer customer = null;
            var response = await client.GetAsync(client.BaseAddress + "/" + id.ToString());
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<Result>(result);
                if (res.statusCode == 200)
                {
                    customer = res.value;
                    return customer;
                }
                else if (res.statusCode == 404)
                {
                    Console.WriteLine("Запись с таким  Id отсутствует");
                    return null;
                }
                return null;
            }
            else
            {
                Console.WriteLine("Неизвестная ошибка");
                return null;
            }
        }

        public static async Task<bool?> CreateCustomerAsync(Customer customer)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                client.BaseAddress, customer);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<Result>(result);
                if (res.statusCode == 200)
                {
                    return true;
                }
                else if (res.statusCode == 409)
                {
                    return false;
                }
                return null;
            }
            else
                return null;
        }

        public static void SetRoute()
        {
            client.BaseAddress = new Uri("https://localhost:7174/customers");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task Work()
        {
            string str = "";
            while (str != "exit")
            {
                str = Console.ReadLine();
                if (str == "search")
                {
                    int n = 0;

                    Console.WriteLine("Введите id для поиска");
                    str = Console.ReadLine();
                    Int32.TryParse(str, out n);
                    if (n == 0)
                    {
                        Console.WriteLine("Повторите ввод");
                    }
                    else
                    {
                        var customer = await GetCustomerAsync(n);
                        if (customer is null)
                        {
                            Console.WriteLine("Повторите ввод");
                        }
                        else
                        {
                            Console.WriteLine("ID: " + customer.Id);
                            Console.WriteLine("FirstName: " + customer.FirstName);
                            Console.WriteLine("LastName: " + customer.LastName);
                        }
                    }
                }
                else if (str == "add")
                {
                    int n = 0;

                    Console.WriteLine("Заполните поля");
                    Console.WriteLine("Id");
                    str = Console.ReadLine();
                    Int32.TryParse(str, out n);
                    if (n <= 0)
                    {
                        Console.WriteLine("Повторите ввод");
                    }
                    else
                    {
                        Console.WriteLine("FirstName");
                        var firstName = Console.ReadLine();
                        Console.WriteLine("LastName");
                        var lastName = Console.ReadLine();
                        var result = await CreateCustomerAsync(new Customer(n, firstName, lastName));
                        if (result is true)
                        {
                            Console.WriteLine("Успешно");
                        }
                        else if (result is false)
                        {
                            Console.WriteLine("Конфликт: сущность с таким ID существует. Повторите ввод");
                        }
                        else
                        {
                            Console.WriteLine("Неизвестная ошибка. Повторите ввод");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Повторите ввод требуемой операции");
                }
            }
        }

    }
}
