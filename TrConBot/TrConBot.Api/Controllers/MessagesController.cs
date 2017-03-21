using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using TrConBot.Api.Dialogs;
using TrConBot.Api.Utility;

namespace TrConBot.Api
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<IHttpActionResult> Post([FromBody]Activity activity)
        {
            activity.Locale = "ru-RU";

            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            try
            {
                if (activity.Type == ActivityTypes.Message)
                {
                    var typing = activity.CreateTyping();
                    await connector.Conversations.ReplyToActivityAsync(typing);

                    await Conversation.SendAsync(activity, () => new RootMenuDialog());
                }
                else
                {
                    HandleSystemMessage(activity);
                }
            }
            catch (Exception e)
            {
                var reply = activity.CreateReply(e.ToString().Replace("\n", "\n\n"), MessageLocale.RuRu);
                await connector.Conversations.ReplyToActivityAsync(reply);

                reply = activity.CreateReply("Что-то пошло не так.", MessageLocale.RuRu);
                await connector.Conversations.ReplyToActivityAsync(reply);

                await Task.Delay(2000);
                reply = activity.CreateReply("Пока я восстанавливаюсь, свяжитесь с нами по телефону +7(800)100-22-20", MessageLocale.RuRu);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }

            return Ok();
        }

        private Activity HandleSystemMessage(Activity activity)
        {
            if (activity.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (activity.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (activity.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (activity.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}