using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class Messenger : IMessenger
    {
        public string GetMessage(long messageId)
        {
            return "";
        }

        public void SendMessage(string message)
        {
            
        }

        public bool TryGetUser(int userId, out User user)
        {
            user = null;
            return false;
        }
    }

    public interface IMessenger
    {
        void SendMessage(string message);
        string GetMessage(long messageId);
        bool TryGetUser(int userId, out User user);
    }

    public class User
    {
    }
}
