using BT.Dto;
using IdentityModel.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BT.API.Hubs
{
    public class ChatHub:Hub
    {
        private readonly IDistributedCache distributedCache;

        public ChatHub(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }



        public async Task SendMessage(string user, string message,string img)
        {
            ChatDto chatDto = new ChatDto() { Name = user,Msg=message,Img = img, SendTime= DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString() };

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions() { SlidingExpiration=TimeSpan.FromHours(2)};


            string json = await distributedCache.GetStringAsync("chat");
            if (string.IsNullOrEmpty(json))
            {
                List<ChatDto> chatDtos = new List<ChatDto>();
                chatDtos.Add(chatDto);
                await distributedCache.SetStringAsync("chat",JsonSerializer.Serialize(chatDtos), options);
            }
            else
            {
                var list = JsonSerializer.Deserialize<List<ChatDto>>(json);
                list.Add(chatDto);
                await distributedCache.SetStringAsync("chat", JsonSerializer.Serialize(list), options);

            }

            await Clients.All.SendAsync("MsgList", user, message, img,DateTime.Now.ToLongDateString()+ DateTime.Now.ToLongTimeString());
        }

    }
}
