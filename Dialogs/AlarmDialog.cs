using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Saltyeyes.Cephalon {
    [Serializable]
    public class AlarmDialog : IDialog<object> {
        public async Task StartAsync(IDialogContext context) {
            var response = context.MakeMessage();
            response.Text = "Hello, I'm the alarm dialog. I'm interrupting your conversation to ask you a question. Type \"done\" to resume";

            await context.PostAsync(response);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result) {
            var response = context.MakeMessage();
            if ((await result).Text == "done") {
                response.Text = "Great, back to the original conversation!";
                await context.PostAsync(response);
                context.Done(String.Empty); //Finish this dialog
            } else {
                response.Text = "I'm still on the survey until you type \"done\"";
                await context.PostAsync(response);
                context.Wait(MessageReceivedAsync); //Not done yet
            }
        }
    }
}