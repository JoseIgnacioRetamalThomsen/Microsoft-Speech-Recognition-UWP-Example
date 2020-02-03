using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.SpeechRecognition;
using Windows.Storage;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpeachOne
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SpeechRecognizer sR;
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            sR = new SpeechRecognizer();
            //set time

            //load grammar file
            var gf = await StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///grammar.xml"));

            //add the constraint to the speach recogniser
            sR.Constraints.Add(new SpeechRecognitionGrammarFileConstraint(gf));

            var result = await sR.CompileConstraintsAsync();
            if(result.Status == SpeechRecognitionResultStatus.Success)
            {
                while (true)
                {
                    SpeechRecognitionResult srr = await sR.RecognizeAsync();
                    // use the semantic interpretation engine
                    // to get the  commands

                    string myCommand = srr.SemanticInterpretation.Properties["command"].Single();
                    string ruleID = srr.RulePath[0];
                    
                    var md = new Windows.UI.Popups.MessageDialog(myCommand, "User said this " + ruleID);
                    await md.ShowAsync();
                }
            }



        }
    }
}
