using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace WS
{
    public class ChatHub : Hub
    {
        public void Send()
        {
            WSServer.Instance.SendMessage(Clients);
        }
    }

    public sealed class WSServer
    {
        private WSServer() { }

        private static WSServer instance = null;
        private static bool isRunning = false;
        Random rd = new Random();
        int i = 0;

        public static WSServer Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new WSServer();
                }

                return instance;
            }
        }

        public void SendMessage(Microsoft.AspNet.SignalR.Hubs.IHubCallerConnectionContext<dynamic> clients)
        {
            if (!isRunning)
            {
                isRunning = true;

                while (true)
                {
                    List<ItemModel> items = new List<ItemModel>();

                    string[] ids = { "A", "B", "C", "D", "E" };

                    foreach (string id in ids)
                    {
                        items.Add(new ItemModel()
                        {
                            ID = id,
                            Count = rd.Next(1, 1000)
                        }); ;
                    }

                    // Call the broadcastMessage method to update clients.
                    clients.All.broadcastMessage(JsonConvert.SerializeObject(items));
                    Thread.Sleep(1000);

                    Debug.WriteLine(i);
                    i++;
                }
            }
        }
    }


    public class ItemModel
    {
        public string ID { get; set; }
        public int Count { get; set; }
    }
}