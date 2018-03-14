using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using PorterStemmer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using WekaClassifier;
using WekaClassifier.Enums;
using MARC2.Model;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

namespace MARC2
{
    /// <summary>
    /// Interaction logic for Classify.xaml
    /// </summary>
    public partial class ClassifyNFRPage : Page
    {
        public MyViewModel Model { get; set; }
        public bool changeInProgress = false;

        string exceptionMessage;

        List<string> allNFRReviews = new List<string>();
        List<string> allNFRClassification = new List<string>();
        
        int currentReviewIndex = 0;
        List<string> filteredReviews;
        TextFilterType txtfilterType = TextFilterType.NoFilter;

        bool CITCheckboxCheckedState;
        bool DITCheckboxCheckedState;
        bool NoSWCheckboxCheckedState;
        bool STCheckboxCheckedState;

        bool NBCheckboxCheckedState;
        bool SVMCheckboxCheckedState;

        bool DTCheckboxCheckedState;
        bool CTCheckboxCheckedState;

        /// <summary>
        /// Classify Page accepts model with MyViewModel type 
        /// </summary>
        /// <param name="model"></param>
        public ClassifyNFRPage(MyViewModel model)
        {
            InitializeComponent();

            // Initialize Model 
            Model = model;
            this.DataContext = this;

            //Initialize Checkbox checked state
            NBCheckbox.IsChecked = true;
            DTCheckbox.IsChecked = true;
            DITCheckbox.IsChecked = true;

            dependabilityHeader.Header = Model.CurrentSource.Replace("Imported Reviews", "Dependability");
            performanceHeader.Header = Model.CurrentSource.Replace("Imported Reviews", "Performance");
            supportabilityHeader.Header = Model.CurrentSource.Replace("Imported Reviews", "Supportability");
            usabilityHeader.Header = Model.CurrentSource.Replace("Imported Reviews", "Usability");

            PopulateViewFromModel();
        }


        /// <summary>
        /// Populate View from the data retrieved from the Model
        /// </summary>
        private void PopulateViewFromModel()
        {
            List<ReviewItem> items = new List<ReviewItem>();

            if (Model.DependabilityList != null)
            {
                foreach (var item in Model.DependabilityList)
                {
                    items.Add(new ReviewItem() { Review = item });
                }
                dependabilityListbox.ItemsSource = items;
                noDependabilityTextBlock.Visibility = items.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
            }

            items = new List<ReviewItem>();
            if (Model.PerformanceList != null)
            {
                foreach (var item in Model.PerformanceList)
                {
                    items.Add(new ReviewItem() { Review = item });
                }
                performanceListbox.ItemsSource = items;
                noPerformanceTextBlock.Visibility = items.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
            }

            items = new List<ReviewItem>();
            if (Model.SupportabilityList != null)
            {
                foreach (var item in Model.SupportabilityList)
                {
                    items.Add(new ReviewItem() { Review = item });
                }
                supportabilityListbox.ItemsSource = items;
                noSupportabilityTextBlock.Visibility = items.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
            }

            items = new List<ReviewItem>();
            if (Model.UsabilityList != null)
            {
                foreach (var item in Model.UsabilityList)
                {
                    items.Add(new ReviewItem() { Review = item });
                }
                usabilityListbox.ItemsSource = items;
                noUsabilityTextBlock.Visibility = items.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
            }

            //Update Pie Chart for Dependability, Performance, Supportability and Usability
            try
            {
                if (Model.ClassfyNFRReviewsResultsCollection != null && Model.ClassfyNFRReviewsResultsCollection.Count > 0)
                    Model.ClassfyNFRReviewsResultsCollection.Clear();

                Model.ClassfyNFRReviewsResultsCollection.Add(
                    new PieSeries
                    {
                        Title = "Dependability",
                        Values = new ChartValues<ObservableValue> { new ObservableValue(Model.DependabilityList != null ? Model.DependabilityList.Count : 0) },
                        DataLabels = true
                    });
                Model.ClassfyNFRReviewsResultsCollection.Add(
                    new PieSeries
                    {
                        Title = "Performance",
                        Values = new ChartValues<ObservableValue> { new ObservableValue(Model.PerformanceList != null ? Model.PerformanceList.Count : 0) },
                        DataLabels = true
                    });
                Model.ClassfyNFRReviewsResultsCollection.Add(
                    new PieSeries
                    {
                        Title = "Supportability",
                        Values = new ChartValues<ObservableValue> { new ObservableValue(Model.SupportabilityList != null ? Model.SupportabilityList.Count : 0) },
                        DataLabels = true
                    });
                Model.ClassfyNFRReviewsResultsCollection.Add(
                   new PieSeries
                   {
                       Title = "Usability",
                       Values = new ChartValues<ObservableValue> { new ObservableValue(Model.UsabilityList != null ? Model.UsabilityList.Count : 0) },
                       DataLabels = true
                   });


                progressBarContainer.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Approach Checkbox event handler for BOW and BOF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ITApproachCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (!changeInProgress)
            {
                changeInProgress = true;
                DITCheckbox.IsChecked = false;
                CITCheckbox.IsChecked = false;
                (sender as CheckBox).IsChecked = true;
                changeInProgress = false;
            }
        }

        /// <summary>
        /// Filter Checkbox event handler for Stopwords Removal and Stemming
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            //No need to make sure only one filter is selected.
        }

        /// <summary>
        /// Training Checkbox event handler for Custom Training and Default Training
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainingCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (!changeInProgress)
            {
                changeInProgress = true;
                CTCheckbox.IsChecked = false;
                DTCheckbox.IsChecked = false;
                (sender as CheckBox).IsChecked = true;
                changeInProgress = false;
            }
        }

        /// <summary>
        /// Classifier Checkbox event handler for NB, SVM, and RF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClassifierCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (!changeInProgress)
            {
                changeInProgress = true;
                NBCheckbox.IsChecked = false;
                SVMCheckbox.IsChecked = false;

                (sender as CheckBox).IsChecked = true;
                changeInProgress = false;
            }
        }

        /// <summary>
        /// Classify Reviews Button Click Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void classifyButton_Click(object sender, RoutedEventArgs e)
        {
            progressBarContainer.Visibility = Visibility.Visible;

            var userReviews = Model.ReviewList;
            CITCheckboxCheckedState = CITCheckbox.IsChecked ?? false;
            DITCheckboxCheckedState = DITCheckbox.IsChecked ?? false;
            NoSWCheckboxCheckedState = NoSWCheckbox.IsChecked ?? false;
            STCheckboxCheckedState = STCheckbox.IsChecked ?? false;

            NBCheckboxCheckedState = NBCheckbox.IsChecked ?? false;
            SVMCheckboxCheckedState = SVMCheckbox.IsChecked ?? false;

            DTCheckboxCheckedState = DTCheckbox.IsChecked ?? false;
            CTCheckboxCheckedState = CTCheckbox.IsChecked ?? false;


            //Check For Custom training file option checked and file not selected
            if (CTCheckboxCheckedState && browseCustomTrainingFileTextbox.Text == "")
            {
                progressBarContainer.Visibility = Visibility.Hidden;
                MessageBox.Show("Custom training file field empty.");
            }
            else
            {
                var CTFilePath = browseCustomTrainingFileTextbox.Text;
                if (userReviews.Count != 0)
                {
                    var bwClassifyAllAndExport = new BackgroundWorker();
                    bwClassifyAllAndExport.DoWork += (o, args)
                        => classifyAllReviews
                        (
                            userReviews,
                            CTCheckboxCheckedState ? CTFilePath : null,
                            SVMCheckboxCheckedState ? ClassifierName.SupportVectorMachine : ClassifierName.NaiveBayes
                            );
                    bwClassifyAllAndExport.RunWorkerCompleted += (o, args) => classifyAllAndExportUpdateControl();
                    bwClassifyAllAndExport.RunWorkerAsync();
                }
            }



        }

        /// <summary>
        /// Classify all and Export Update Control
        /// </summary>
        /// <param name="trainingFilePath"></param>
        private void classifyAllAndExportUpdateControl()
        {
            List<string> dependabilityReviews = new List<string>();
            List<string> performanceReviews = new List<string>();
            List<string> supportabilityReviews = new List<string>();
            List<string> usabilityReviews = new List<string>();

            if (allNFRClassification != null && allNFRClassification.Count > 0)
            {
                for (int i = 0; i < allNFRClassification.Count; i++)
                {
                    if (allNFRClassification[i].Contains("Dep"))
                    {
                        dependabilityReviews.Add(allNFRReviews[i]);
                    }
                    if (allNFRClassification[i].Contains("Per"))
                    {
                        performanceReviews.Add(allNFRReviews[i]);
                    }
                    if (allNFRClassification[i].Contains("Sup"))
                    {
                        supportabilityReviews.Add(allNFRReviews[i]);
                    }
                    if (allNFRClassification[i].Contains("Usa"))
                    {
                        usabilityReviews.Add(allNFRReviews[i]);
                    }
                }
            }


            var dependabilityReviewsTemp = dependabilityReviews.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            var performanceReviewsTemp = performanceReviews.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            var supportabilityReviewsTemp = supportabilityReviews.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            var usabilityReviewsTemp = usabilityReviews.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            Model.DependabilityList = dependabilityReviewsTemp;
            Model.PerformanceList = performanceReviewsTemp;
            Model.SupportabilityList = supportabilityReviewsTemp;
            Model.UsabilityList = usabilityReviewsTemp;

            PopulateViewFromModel();
        }

        /// <summary>
        /// Classify all user reviews and export. Takes in a list of user reviews and training File path
        /// </summary>
        /// <param name="userReviews"></param>
        /// <param name="trainingFilePath"></param>
        private void classifyAllReviews(List<string> userReviews, string trainingFilePath, ClassifierName classifierName)
        {
            ResolveTextFilterType();
            WekaClassifier.WekaClassifier classifier;
            allNFRClassification = new List<string>();

            filteredReviews = new List<string>();
            foreach (string review in userReviews)
            {
                filteredReviews.Add(FilterText(review));
            }

            try
            {
                classifier = new WekaClassifier.WekaClassifier(filteredReviews, trainingFilePath, Directory.GetCurrentDirectory(), classifierName, txtfilterType, ClassificationScheme.Binary);
                allNFRClassification = classifier.predictedLabel;
                
                //Instead of adding filtered text add the original review.
                var temp = new List<string>();
                foreach (var item in classifier.predictedData)
                {
                    for (int i = 0; i < userReviews.Count; i++)
                    {
                        if (FilterText(userReviews[i]) == item)
                        {
                            temp.Add(userReviews[i]);
                            break;
                        }
                    }
                }
                allNFRReviews = temp;
            }
            catch (Exception e)
            {
                exceptionMessage = e.ToString();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void ResolveTextFilterType()
        {
            if (NoSWCheckboxCheckedState == false && STCheckboxCheckedState == false)
            {
                txtfilterType = TextFilterType.NoFilter;
            }
            else if (NoSWCheckboxCheckedState == true && STCheckboxCheckedState == true)
            {
                txtfilterType = TextFilterType.StopwordsRemovalStemming;
            }
            else if (NoSWCheckboxCheckedState == true)
            {
                txtfilterType = TextFilterType.StopwordsRemoval;
            }
            else if (STCheckboxCheckedState == true)
            {
                txtfilterType = TextFilterType.Stemming;
            }
        }


        /// <summary>
        /// Method to filter input text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string FilterText(string text)
        {
            var currDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);

            // Combine the base folder with your specific folder....
            string specificFolder = System.IO.Path.Combine(currDir, "MARC 3.0");

            // Check if folder exists and if not, create it
            if (!Directory.Exists(specificFolder))
                Directory.CreateDirectory(specificFolder);


            text.Replace('.', ' ');
            if (NoSWCheckboxCheckedState)
            {
                StopWordRemoval.StopWordRemoval temp = new StopWordRemoval.StopWordRemoval(text, specificFolder);
                text = temp.output;
            }


            if (STCheckboxCheckedState)
            {
                string[] words = text.Split(' ');
                string finalStemOutput = "";
                foreach (string word in words)
                {
                    Stemmer temp = new Stemmer();
                    temp.add(word.ToCharArray(), word.Length);
                    temp.stem();
                    var stemOutput = temp.ToString();
                    finalStemOutput += stemOutput + " ";
                }
                text = finalStemOutput;
            }
            text = RemoveSpecialCharacters(text);
            return text;
        }


        /// <summary>
        /// Remove special characters from input string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                // if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ' || c == '-')
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// Browse Custom Training File Button Click Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseCustomTrainingFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();


            //fdlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\InputData\\TrainingDatasets";
            fdlg.Filter = "Arff Files (*.arff)|*.arff";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == true)
            {
                browseCustomTrainingFileTextbox.Text = fdlg.FileName;
            }
        }

        /// <summary>
        /// Event handler for Exporting Classification Results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportClassificationResults_button(object sender, RoutedEventArgs e)
        {
            if (Model.BugReportList == null || Model.BugReportList.Count == 0)
            {
                MessageBox.Show("One or more list may be empty.");
            }
            else if (Model.UserRequirementList == null || Model.UserRequirementList.Count == 0)
            {
                MessageBox.Show("One or more list may be empty.");
            }

            try
            {
                if (Model.BugReportList.Count != 0 || Model.UserRequirementList.Count != 0)
                {
                    var outputDialogFolder = ShowSelectOutputFolderDialog();
                    if (outputDialogFolder != null)
                    {
                        ExportClassificationResults(outputDialogFolder);
                        Process.Start("explorer.exe", outputDialogFolder);
                    }
                }
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// Show Select Output Folder Dialog
        /// </summary>
        /// <returns></returns>
        private string ShowSelectOutputFolderDialog()
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "MARC 3.0 : Select Directory To Save Classification Results";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = Directory.GetCurrentDirectory();

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = Directory.GetCurrentDirectory();
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dlg.FileName;
            }
            return null;
        }

        /// <summary>
        /// Main Method to Export Classification Results
        /// </summary>
        /// <param name="outputFolder"></param>
        private void ExportClassificationResults(string outputFolder)
        {
            //Write Bug Reports to OutputFolder
            using (var brWriter = new StreamWriter(outputFolder + @"\Bug Reports.txt"))
            {
                if (Model.BugReportList != null && Model.BugReportList.Count > 0)
                {
                    foreach (var item in Model.BugReportList)
                    {
                        brWriter.WriteLine(item);
                    }
                }
            }

            //Write User Requierments to OutputFolder
            using (var urWriter = new StreamWriter(outputFolder + @"\User Requirement.txt"))
            {
                if (Model.UserRequirementList != null && Model.UserRequirementList.Count > 0)
                {
                    foreach (var item in Model.UserRequirementList)
                    {
                        urWriter.WriteLine(item);
                    }
                }
            }

            //Write Miscellaneous to OutputFolder
            using (var otWriter = new StreamWriter(outputFolder + @"\Miscellaneous.txt"))
            {
                if (Model.MiscellaneousList != null && Model.MiscellaneousList.Count > 0)
                {
                    foreach (var item in Model.MiscellaneousList)
                    {
                        otWriter.WriteLine(item);
                    }
                }
            }
        }



        /// <summary>
        /// Browse Custom Indicator Terms List Folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseCITFileButton_Click(object sender, RoutedEventArgs e)
        {

        }


        #region Mouse Scroll Handlers

        /// <summary>
        /// Vertical Scroll Event Handler for Dependability ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void instScroll_Loaded(object sender, RoutedEventArgs e)
        {
            dependabilityListbox.AddHandler(MouseWheelEvent, new RoutedEventHandler(MyMouseWheelH), true);
        }


        /// <summary>
        /// Vertical Scroll Initiator for Dependability Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyMouseWheelH(object sender, RoutedEventArgs e)
        {
            MouseWheelEventArgs eargs = (MouseWheelEventArgs)e;
            double x = (double)eargs.Delta;
            double y = instScroll.VerticalOffset;
            instScroll.ScrollToVerticalOffset(y - x);
        }


        /// <summary>
        /// Vertical Scroll Initiator for Performance Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyMouseWheelH2(object sender, RoutedEventArgs e)
        {
            MouseWheelEventArgs eargs = (MouseWheelEventArgs)e;
            double x = (double)eargs.Delta;
            double y = instScroll2.VerticalOffset;
            instScroll2.ScrollToVerticalOffset(y - x);
        }


        /// <summary>
        /// Vertical Scroll Handler for Performance Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void instScroll2_Loaded(object sender, RoutedEventArgs e)
        {
            performanceListbox.AddHandler(MouseWheelEvent, new RoutedEventHandler(MyMouseWheelH2), true);
        }


        /// <summary>
        /// Vertical Scroll Handler for Supportability Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void instScroll3_Loaded(object sender, RoutedEventArgs e)
        {
            supportabilityListbox.AddHandler(MouseWheelEvent, new RoutedEventHandler(MyMouseWheelH3), true);
        }


        /// <summary>
        /// Vertical Scroll Initiator for Supportability Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyMouseWheelH3(object sender, RoutedEventArgs e)
        {
            MouseWheelEventArgs eargs = (MouseWheelEventArgs)e;
            double x = (double)eargs.Delta;
            double y = instScroll3.VerticalOffset;
            instScroll3.ScrollToVerticalOffset(y - x);
        }

        /// <summary>
        /// Vertical Scroll Handler for Usability Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void instScroll4_Loaded(object sender, RoutedEventArgs e)
        {
            usabilityListbox.AddHandler(MouseWheelEvent, new RoutedEventHandler(MyMouseWheelH4), true);
        }

        /// <summary>
        /// Vertical Scroll Initiator for Usability Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyMouseWheelH4(object sender, RoutedEventArgs e)
        {
            MouseWheelEventArgs eargs = (MouseWheelEventArgs)e;
            double x = (double)eargs.Delta;
            double y = instScroll4.VerticalOffset;
            instScroll4.ScrollToVerticalOffset(y - x);
        }


        #endregion Mouse Scroll Handlers


    }
}
