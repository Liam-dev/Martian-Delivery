using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartianDelivery
{
    class Order
    {
        public string initialConversation;
        public string completedConversation;

        public List<string> items;

        public int reward;

        public Order(string alien, int order)
        {
            File file = new File();

            string path = "res://orders/" + alien + "/" + order.ToString() + ".txt";
            if (file.FileExists(path))
            {
                file.Open(path, File.ModeFlags.Read);
                string[] data = file.GetAsText().Split("\n".ToCharArray());
                file.Close();

                items = data[0].Split(",".ToCharArray()).ToList();
                reward = int.Parse(data[1]);

                initialConversation = alien + " : " + data[2];
                completedConversation = alien + " : " + data[3];
            }          
        }
    }

}