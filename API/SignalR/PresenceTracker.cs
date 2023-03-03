using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> onlineUsers =
        new Dictionary<string, List<string>>();

        public Task<bool> userConnected(string userName, string connectionId)

        {
            bool isOnline=false;
            lock (onlineUsers)
            {
                if (onlineUsers.ContainsKey(userName))
                {
                    onlineUsers[userName].Add(connectionId);
                }
                else
                {
                    onlineUsers.Add(userName, new List<string> { connectionId });
                    isOnline=true;
                }
            }
            return Task.FromResult(isOnline);
        }


        public Task<bool> userDisconnected(string userName, string connectionId)
        {
            bool isOffline=false;
            lock (onlineUsers)
            { 
                if (!onlineUsers.ContainsKey(userName)) return Task.FromResult(isOffline);
                onlineUsers[userName].Remove(connectionId);
                if (onlineUsers[userName].Count == 0)
                {
                    onlineUsers.Remove(userName);
                    isOffline=true;
                }
            }
            return Task.FromResult(isOffline);
        }
        public Task<string[]> getOnlineUsers()
        {
            string[] onlineU;
            lock (onlineUsers)
            {
                onlineU = onlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }
            return Task.FromResult(onlineU);
        }


        public Task<List<string>> getConnectionsForUser(string username)
        {
            List<string> connectionIds;
            lock (onlineUsers)
            {
                connectionIds = onlineUsers.GetValueOrDefault(username);


            }
            return Task.FromResult(connectionIds);
        }
    }
}