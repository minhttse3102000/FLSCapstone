using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;

namespace BEAPICapstoneProjectFLS.Services
{
    public static class SendFirebaseMessaging
    {
        public static async Task<string> SendNotification(string uid, string title, string body, string channel)
        {

            try
            {
                var message = new Message()
                {
                    Data = new Dictionary<string, string>()
                    {
                        ["channel"] = channel
                    },
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },

                    Topic = uid
                };
                var messaging = FirebaseMessaging.DefaultInstance;
                return await messaging.SendAsync(message);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
