﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.Services
{
    public class MonkeyService
    {
        List<Monkey> monkeyList = new();
        HttpClient httpClient;

        public MonkeyService ()
        {
            this.httpClient = new HttpClient();
        }

        public async Task<List<Monkey>> GetMonkeys ()
        {
            if (monkeyList?.Count > 0)
            {
                return monkeyList;
            }

            HttpResponseMessage response = await httpClient.GetAsync("https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/MonkeysApp/monkeydata.json");

            if (response.IsSuccessStatusCode)
            {
                monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
            }

            return monkeyList;
        }
    }
}
