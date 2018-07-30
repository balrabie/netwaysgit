using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureCognitiveServices.Models;
using Microsoft.CognitiveServices.Speech;


namespace AzureCognitiveServices
{
    /// <summary>
    /// Class that manConverts audio files (PCM or WAV with sampling rate of 16kHz) into text
    /// </summary>
    public class SpeechToTextManager
    {
        public string RecognizedText { get; private set; } = null;

        const string SamplePath = @"D:\Users\bahid\Desktop\sample.wav";

        public async Task<SpeechToTextDto> RecognizeSpeechAsync(string path)
        {
            // Creates an instance of a speech factory with specified
            // subscription key and service region.
            var factory = SpeechFactory.FromSubscription(subscriptionKey, region);

            var stopRecognition = new TaskCompletionSource<int>();

            using (var recognizer = factory.CreateSpeechRecognizerWithFileInput(path))
            {
                // Subscribes to events.
                recognizer.IntermediateResultReceived += (s, e) => {
                    Console.WriteLine($"\n    Partial result: {e.Result.Text}.");
                };

                recognizer.FinalResultReceived += (s, e) => {
                    var result = e.Result;
                    Console.WriteLine($"Recognition status: {result.RecognitionStatus.ToString()}");
                    switch (result.RecognitionStatus)
                    {
                        case RecognitionStatus.Recognized:
                            Console.WriteLine($"\n    Final result: Text: {result.Text}, Offset: {result.OffsetInTicks}, Duration: {result.Duration}.");
                            RecognizedText = result.Text;
                            break;
                        case RecognitionStatus.InitialSilenceTimeout:
                            Console.WriteLine("The start of the audio stream contains only silence, and the service timed out waiting for speech.\n");
                            //RecognizedText = null;
                            break;
                        case RecognitionStatus.InitialBabbleTimeout:
                            Console.WriteLine("The start of the audio stream contains only noise, and the service timed out waiting for speech.\n");
                            //RecognizedText = null;
                            break;
                        case RecognitionStatus.NoMatch:
                            Console.WriteLine("The speech was detected in the audio stream, but no words from the target language were matched. Possible reasons could be wrong setting of the target language or wrong format of audio stream.\n");
                            //RecognizedText = null;
                            break;
                        case RecognitionStatus.Canceled:
                            Console.WriteLine($"There was an error, reason: {result.RecognitionFailureReason}");
                            //RecognizedText = null;
                            break;
                    }
                };

                recognizer.RecognitionErrorRaised += (s, e) => {
                    Console.WriteLine($"\n    An error occurred. Status: {e.Status.ToString()}, FailureReason: {e.FailureReason}");
                    stopRecognition.TrySetResult(0);
                };

                recognizer.OnSessionEvent += (s, e) => {
                    Console.WriteLine($"\n    Session event. Event: {e.EventType.ToString()}.");
                    // Stops recognition when session stop is detected.
                    if (e.EventType == SessionEventType.SessionStoppedEvent)
                    {
                        Console.WriteLine($"\nStop recognition.");
                        stopRecognition.TrySetResult(0);
                    }
                };

                // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
                await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                // Waits for completion.
                // Use Task.WaitAny to keep the task rooted.
                Task.WaitAny(new[] { stopRecognition.Task });

                // Stops recognition.
                await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

                return new SpeechToTextDto { RecognizedText = this.RecognizedText };
            }
        }


        const string subscriptionKey = "832d6aa5cc8746e78d78999bee92bdbe"; // 4980653010e04dcda27415e95ee32fe9
        // 832d6aa5cc8746e78d78999bee92bdbe

        const string region = "westus";

    }
}
