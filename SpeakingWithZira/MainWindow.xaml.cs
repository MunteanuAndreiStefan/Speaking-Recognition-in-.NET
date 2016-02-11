using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Globalization;

namespace SpeakingWithZira
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpeechRecognitionEngine recognitionEngine;
        SpeechSynthesizer synthesizer;
        public MainWindow()
        {
            InitializeComponent();
            SpeechRecognition_Initialize();
            TextToSpeech_Initialize();
            this.Hide();
        }

        private void SpeechRecognition_Initialize()
        {
            recognitionEngine = new SpeechRecognitionEngine();
            Choices commands = new Choices();
            string[] choices = {"hi zira", "how are you today?", "i feel sick", "good bye zira", "yes" };
            commands.Add(choices);
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commands);
            Grammar grammar = new Grammar(grammarBuilder);
            recognitionEngine.LoadGrammarAsync(grammar);
            recognitionEngine.SetInputToDefaultAudioDevice();

            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
            recognitionEngine.SpeechRecognized += recognitionEngine_SpeechRecognized;
        }

        void recognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string ssmlText = "";
            switch(e.Result.Text)
            {
                case "hi zira":
                    BuildTheAnswer(ref ssmlText, "Hello", "+50Hz", "0.8");
                    BuildTheAnswer(ref ssmlText, "there", "30Hz", "0.8");
                break;
                case "how are you today?":
                    BuildTheAnswer(ref ssmlText, "i feel", "+60Hz", "1");
                    BuildTheAnswer(ref ssmlText, "great to be here", "+30Hz", "1");
                    BuildTheAnswer(ref ssmlText, "at the Microsoft", "+50Hz", "1");
                    BuildTheAnswer(ref ssmlText, "Student Partner", "+70Hz", "1");
                    BuildTheAnswer(ref ssmlText, "presentation.", "+10Hz", "1");
                    BuildTheAnswer(ref ssmlText, "what about you?", "+5Hz", "0.8");
                    break;
                case "i feel sick":
                    BuildTheAnswer(ref ssmlText, "oh no", "+50Hz", "0.4");
                    break;
                case "good bye zira":
                    BuildTheAnswer(ref ssmlText, "have a", "x-high", "0.5");
                    BuildTheAnswer(ref ssmlText, "good", "x-high", "0.3");
                    BuildTheAnswer(ref ssmlText, "day", "x-high", "0.4");
                    Application.Current.Shutdown();
                    break;
                case "yes":
                    BuildTheAnswer(ref ssmlText,"yes","default","1");                    
                    break;
                default:
                    return;
            }
            Answer(ssmlText);
        }

        private void TextToSpeech_Initialize()
        {
            synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();
            synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult,0, CultureInfo.CurrentCulture);            
        }

        private void BuildTheAnswer(ref string ssmlText, string textToBeAppended, string pitch, string rate)
        {
            ssmlText += "<prosody pitch='" + pitch + "' rate='" + rate + "'>" + textToBeAppended + "</prosody>";
        }

        private void Answer(string ssmlText)
        {
            PromptBuilder builder = new PromptBuilder();
            builder.AppendSsmlMarkup(ssmlText);
            synthesizer.Speak(builder);
        }
    }
}
