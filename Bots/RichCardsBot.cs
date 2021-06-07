// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace Microsoft.BotBuilderSamples
{
    // RichCardsBot prompts a user to select a Rich Card and then returns the card
    // that matches the user's selection.
    public class RichCardsBot : DialogBot<MainDialog>
    {
        public class userdata
        {
            public string UserName { get; set; }
            public string Occupation { get; set; }
        }
        public RichCardsBot(ConversationState conversationState, UserState userState, MainDialog dialog, ILogger<DialogBot<MainDialog>> logger)
            : base(conversationState, userState, dialog, logger)
        {
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            StreamReader streamReader = new StreamReader(@"C:\Users\kbpri\source\repos\botbuilder-samples\samples\csharp_dotnetcore\06.using-cards\user.json");
            StreamReader r = streamReader;

            string json1 = r.ReadToEnd();
            List<userdata> items = JsonSerializer.Deserialize<List<userdata>>(json1);
            var name = items[0].UserName;
            foreach (var member in membersAdded)
            {
                // Greet anyone that was not the target (recipient) of this message.
                // To learn more about Adaptive Cards, see https://aka.ms/msbot-adaptivecards for more details.
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    var reply = MessageFactory.Text("Hey "+name+" !!!"+"\t\t \n "
                        +"                     "
                        + "\n Please type anything to get started.");

                    await turnContext.SendActivityAsync(reply, cancellationToken);
                }
            }
        }
    }
}
