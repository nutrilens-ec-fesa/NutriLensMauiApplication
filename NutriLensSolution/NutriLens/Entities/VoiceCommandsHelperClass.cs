using Android.Content;
using Android.Speech;
using Newtonsoft.Json;

namespace NutriLens.Entities
{
    public static class VoiceCommandsHelperClass
    {
        public static string result;

        public enum MicrophoneState
        {
            Disabled,
            Off,
            On
        }

        public enum StepsMenu
        {
            None = -1
        }

        private static string GetMicrophoneSource(MicrophoneState microphoneState)
        {
            switch (microphoneState)
            {
                case MicrophoneState.Disabled:
                    return "disabled_microphone.png";
                case MicrophoneState.Off:
                    return "microphone_off.png";
                case MicrophoneState.On:
                    return "microphone_on.png";
                default:
                    return string.Empty;
            }
        }

        private static void TurnMicrophoneOn()
        {
            // App.MicrophoneViewModel.MicrophoneImgSource = GetMicrophoneSource(MicrophoneState.On);
        }

        private static void TurnMicrophoneOff()
        {
            // App.MicrophoneViewModel.MicrophoneImgSource = GetMicrophoneSource(MicrophoneState.Off);
        }

        private static async void HearVoiceCommand()
        {
            result = null;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                int tries = 0;
                string value = null;

                SpeechRecognizer speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(Android.App.Application.Context);

                Intent speechRecognizerIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Diga algo...");

                #region Voice Command configuration

                //if (silentTime.IsToggled)
                //    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, int.Parse(silentTimeValue.Text));

                //if (openedTime.IsToggled)
                //    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, int.Parse(openedTimeValue.Text));

                //if (extraPartial.IsToggled)
                //    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraPartialResults, true);

                //if (maxResult.IsToggled)
                //    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                //if (prefferOffline.IsToggled)
                //    speechRecognizerIntent.PutExtra(RecognizerIntent.ExtraPreferOffline, true);

                #endregion


                speechRecognizer.Results += async (sender, e) =>
                {
                    var results = e.Results.Get("results_recognition");
                    string json = JsonConvert.SerializeObject(results);
                    string heardValue = json.Substring(2, json.Length - 4);

                    if (string.IsNullOrEmpty(heardValue))
                    {
                        if (tries < 3)
                        {
                            tries++;
                            await TextToSpeech.SpeakAsync("Não entendi, pode repetir?");
                            speechRecognizer.StartListening(speechRecognizerIntent);
                        }
                        else
                        {
                            result = string.Empty;
                            TurnMicrophoneOff();
                        }

                        Console.WriteLine("Sem resultado");
                    }
                    else
                    {
                        result = heardValue;
                        TurnMicrophoneOff();
                        Console.WriteLine(heardValue);
                    }
                };

                speechRecognizer.Error += async (sender, e) =>
                {
                    Console.WriteLine(e.Error.ToString());

                    if (tries < 3)
                    {
                        tries++;
                        await TextToSpeech.SpeakAsync("Não entendi, pode repetir?");
                        speechRecognizer.StartListening(speechRecognizerIntent);
                    }
                    else
                    {
                        result = string.Empty;
                        TurnMicrophoneOff();
                    }
                };

                speechRecognizer.StartListening(speechRecognizerIntent);

                TurnMicrophoneOn();
            });
        }

        public static async Task<string> GetVoiceCommand()
        {
            HearVoiceCommand();

            while (result == null)
            {
                await Task.Delay(100);
            }

            return result;
        }
    }
}
