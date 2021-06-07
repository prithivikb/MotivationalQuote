// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Windows;
using TextCopy;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCardsBot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace Microsoft.BotBuilderSamples
{
    public class MainDialog : ComponentDialog
    {
        protected readonly ILogger _logger;

       
        public object DataFormats { get; private set; }

       

        public MainDialog(ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _logger = logger;

            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                 SelectEmotionAsync,
                 ChoiceCardStepAsync,
                 ShowCardStepAsync,
                 CardReactionAsync,
                 CardReactionActionAsync,
                 UserActivityAsync,
                 ActivityStatusAsync,
                 ActivityRemindAsync,
                 WaitAsync,
                 waitRespondAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        // 1. Prompts the user if the user is not in the middle of a dialog.
        // 2. Re-prompts the user when an invalid input is received.
        private async Task<DialogTurnResult> SelectEmotionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MainDialog.SelectEmotionAsync");
            chatbotResponse input = new chatbotResponse();
            string respone = input.botresponse("Feeling");

            var Emotion = new PromptOptions()
            {
                

                Prompt = MessageFactory.Text(respone),
                RetryPrompt = MessageFactory.Text("That was not a valid choice."),
                Choices = GetEmotionChoices(),
                
            };


            return await stepContext.PromptAsync(nameof(ChoicePrompt), Emotion, cancellationToken);
        }

      
        private async Task<DialogTurnResult> ChoiceCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MainDialog.ChoiceCardStepAsync");

            // Create the PromptOptions which contain the prompt and re-prompt messages.
            // PromptOptions also contains the list of choices available to the user.
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("I Got Something to Motivate You  !!! Would you like to see it"),
                RetryPrompt = MessageFactory.Text("That was not a valid choice, please select a card or number from 1 to 9."),
                Choices = GetChoices(),
            };


            
            // Prompt the user with the configured PromptOptions.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        // Send a Rich Card response to the user based on their choice.
        // This method is only called when a valid prompt response is parsed from the user's response to the ChoicePrompt.
        private async Task<DialogTurnResult> ShowCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MainDialog.ShowCardStepAsync");
            
            // Cards are sent as Attachments in the Bot Framework.
            // So we need to create a list of attachments for the reply activity.
            var attachments = new List<Attachment>();
            
            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);
            
            // Decide which type of card(s) we are going to show the user

            if (((FoundChoice)stepContext.Result).Value == "Yes")
            {
                reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment());
                await stepContext.Context.SendActivityAsync(reply, cancellationToken);

               

                return await stepContext.NextAsync(null, cancellationToken);
            }
            else
            {
            return await   stepContext.EndDialogAsync();
            }

         /*   switch (((FoundChoice)stepContext.Result).Value)
            {
                case "Yes":
                    // Display an Adaptive Card
                    reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment());
                    //reply.RepromptDialogAsync();
                    break;
                case "No":
                    stepContext.EndDialogAsync();
                    break;
                default:
                    stepContext.EndDialogAsync();
                    //reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment());
                    break;
            }

            // Send the card(s) to the user as an attachment to the activity
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            // Give the user instructions about what to do next
            // await stepContext.Context.SendActivityAsync(MessageFactory.Text("Type anything to see another card."), cancellationToken);
            // _logger.LogInformation("MainDialog.ShowCardStepAsync");


            //var activeDialog = stepContext.ActiveDialog;

            return await stepContext.NextAsync(null, cancellationToken);*/
        }
        private async Task<DialogTurnResult> CardReactionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MainDialog.CardReactionAsync");

            
            var options = new PromptOptions()
            {
               
                Choices = GetQuotesChoices(),
            };
            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }
        private async Task<DialogTurnResult> CardReactionActionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            CardActionmethod copy12 = new CardActionmethod();
            string quote = copy12.Share();
            likedislike like = new likedislike();
            if (((FoundChoice)stepContext.Result).Value == "Share")
            {
                
                await ClipboardService.SetTextAsync(quote);

                return await stepContext.NextAsync(null, cancellationToken);
            }
            if (((FoundChoice)stepContext.Result).Value == "I like It")
            {
                
                like.like(quote);


                return await stepContext.NextAsync(null, cancellationToken);
            }
            if (((FoundChoice)stepContext.Result).Value == "I dont Like It")
            {

                like.Dislike(quote);


                return await stepContext.NextAsync(null, cancellationToken);
            }
            else
            { 
            return await stepContext.NextAsync(null, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> UserActivityAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("you have activity coming up .the Sense of completing it might make you feel better"), cancellationToken);
           // Thread.Sleep(50000);
            var Activity = new PromptOptions()
            {
                
                Choices = GetActivityChoices(),

            };

           // await stepContext.PromptAsync(nameof(ChoicePrompt), Activity, cancellationToken);

            return await stepContext.PromptAsync(nameof(ChoicePrompt), Activity, cancellationToken);
        }

        private async Task<DialogTurnResult> ActivityStatusAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            if (((FoundChoice)stepContext.Result).Value == "Start Activity Now")
            {
                chatbotResponse input = new chatbotResponse();
                string respone = input.botresponse("Start_Activity_Now");
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Great!!! Let Me Know when you Complete the Activity !!!"), cancellationToken);

                return await stepContext.EndDialogAsync();
            }
            if (((FoundChoice)stepContext.Result).Value == "Remind Me Later")
            {

                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Achievers Don't Procastinate... But I Can Remind you in a while to complete the activity"), cancellationToken);
                var Activity = new PromptOptions()
                {

                    Choices = GetRemindChoices(),
                    
            };

               


                return await stepContext.PromptAsync(nameof(ChoicePrompt), Activity, cancellationToken);
            }
            if (((FoundChoice)stepContext.Result).Value == "Completed")
            {

                chatbotResponse input = new chatbotResponse();
                string respone = input.botresponse("Completed");

                await stepContext.Context.SendActivityAsync(MessageFactory.Text(respone), cancellationToken);

                return await stepContext.EndDialogAsync();
            }


                return await stepContext.EndDialogAsync();
        }
        private async Task<DialogTurnResult> ActivityRemindAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (((FoundChoice)stepContext.Result).Value == "15 Mins")
            {
                chatbotResponse input = new chatbotResponse();
                string respone = input.botresponse("Remind_me_later");
                DateTime time = DateTime.Now;
                DateTime  time2 = time.AddMinutes(15) ;

               respone += String.Format("{0:hh: mm:ss tt}", time2);

        await stepContext.Context.SendActivityAsync(MessageFactory.Text(respone), cancellationToken);
                //Thread.Sleep(1000);
                return await stepContext.NextAsync("15", cancellationToken);



            }
            if (((FoundChoice)stepContext.Result).Value == "30 Mins")
            {
                chatbotResponse input = new chatbotResponse();
                string respone = input.botresponse("Remind_me_later");
                DateTime time = DateTime.Now;
                DateTime time2 = time.AddMinutes(30);

                respone += String.Format("{0:hh: mm:ss tt}", time2);

                await stepContext.Context.SendActivityAsync(MessageFactory.Text(respone), cancellationToken); ; ;
                return await stepContext.NextAsync("30", cancellationToken);


            }
            if (((FoundChoice)stepContext.Result).Value == "1 Hour")
            {
                chatbotResponse input = new chatbotResponse();
                string respone = input.botresponse("Remind_me_later");
                DateTime time = DateTime.Now;
                DateTime time2 = time.AddHours(1);

                respone += String.Format("{0:hh: mm:ss tt}", time2);

                await stepContext.Context.SendActivityAsync(MessageFactory.Text(respone), cancellationToken);
                return await stepContext.NextAsync("60", cancellationToken);


            }

            return await stepContext.EndDialogAsync();

        }
        private async Task<DialogTurnResult> WaitAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Task task = Task.Run(() => RF(stepContext, cancellationToken));
            var x = stepContext.Result as string ;
         //   await stepContext.Context.SendActivityAsync(MessageFactory.Text(x), cancellationToken);
            if ( x == "15")
            {
                //Thread.Sleep(10000);
                
               // RF(stepContext, cancellationToken);
                return await stepContext.NextAsync(null, cancellationToken);
            }
            else
            { 
                return await stepContext.NextAsync(null, cancellationToken);
            }

        }
        private async Task<DialogTurnResult> waitRespondAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Remainder for the Activity"), cancellationToken);
            return await stepContext.EndDialogAsync();
        }
        #region Bot Options
        private IList<Choice> GetChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Yes", Synonyms = new List<string>() { "Yes" } },
                new Choice() { Value = "No", Synonyms = new List<string>() { "No" } },
                
            };

            return cardOptions;
        }
         
        private IList<Choice> GetEmotionChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "anger", Synonyms = new List<string>() { "anger" } },
                new Choice() { Value = "Fear", Synonyms = new List<string>() { "Fear" } },
                new Choice() { Value = "Sad", Synonyms = new List<string>() { "Sad" } },
                new Choice() { Value = "disgust", Synonyms = new List<string>() { "disgust" } },
                new Choice() { Value = "Happy", Synonyms = new List<string>() { "Hapiness" } },
                           };

            return cardOptions;
        }

        private IList<Choice> GetActivityChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Start Activity Now", Synonyms = new List<string>() { "Start Activity Now" } },
                new Choice() { Value = "Remind Me Later", Synonyms = new List<string>() { "Remind Me Later" } },
                new Choice() { Value = "Completed", Synonyms = new List<string>() { "Completed" } },
    


            };

            return cardOptions;
        }

        private IList<Choice> GetRemindChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "15 Mins", Synonyms = new List<string>() { "15 Mins" } },
                new Choice() { Value = "30 Mins", Synonyms = new List<string>() { "30 Mins" } },
                new Choice() { Value = "1 Hour", Synonyms = new List<string>() { "1 Hour" } },
            };

            return cardOptions;
        }

        private IList<Choice> GetQuotesChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "I like It", Synonyms = new List<string>() { "I like It" } },
                new Choice() { Value = "I dont Like It", Synonyms = new List<string>() { "I dont Like It" } },
                new Choice() { Value = "Share", Synonyms = new List<string>() { "Share It With a Friend" } },
            };

            return cardOptions;
        }
        #endregion
        private async void RF(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Thread.Sleep(5000);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Notification"), cancellationToken);
            //return Task.CompletedTask;
        }
    }
}
