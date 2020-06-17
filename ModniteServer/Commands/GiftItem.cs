using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModniteServer.Commands
{
    public sealed class GiftItem : IUserCommand
    {
        public string Description => "Gifts you an existing item";
        public string ExampleArgs => "player123 itemID";
        public string Args => "<userid> <itemID>";
        public void Handle(string[] args)
        {
            
        }
    }
}
