using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;

namespace Saltyeyes.Cephalon.Dialogs {
    [Serializable]
    public class ExceptionHandlerDialog<T> : IDialog<T> {
        private readonly Func<IDialog<T>> _makeDialog;
        private readonly bool _displayException;
        private readonly int _stackTraceLength;

        public ExceptionHandlerDialog(Func<IDialog<T>> makeDialog, bool displayException, int stackTraceLength = 500) {
            _makeDialog = makeDialog;
            _displayException = displayException;
            _stackTraceLength = stackTraceLength;
        }

        public async Task StartAsync(IDialogContext context) {
            var dialog = _makeDialog();

            try {
                context.Call<T>(dialog, ResumeAsync);
            } catch (Exception e) {
                if (_displayException)
                    await DisplayException(context, e);
            }
        }

        private async Task ResumeAsync(IDialogContext context, IAwaitable<T> result) {
            try {
                context.Done<T>(await result);
            } catch (Exception e) {
                if (_displayException)
                    await DisplayException(context, e);
            }
        }

        private async Task DisplayException(IDialogContext context, Exception e) {

            var stackTrace = e.StackTrace;
            if (stackTrace.Length > _stackTraceLength)
                stackTrace = stackTrace.Substring(0, _stackTraceLength) + "…";
            stackTrace = stackTrace.Replace(Environment.NewLine, "  \n");

            var message = e.Message.Replace(Environment.NewLine, "  \n");

            var exceptionStr = $"**{message}**  \n\n{stackTrace}";
            var response = context.MakeMessage();
            response.Text = exceptionStr;

            await context.PostAsync(response);
        }
    }
}