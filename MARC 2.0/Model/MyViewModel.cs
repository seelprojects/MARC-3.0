﻿using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MARC2.Model
{
    public class MyViewModel : INotifyPropertyChanged
    {

        public SeriesCollection ImportedReviewsCollection { get; set; }
        public SeriesCollection ClassfyReviewsResultsCollection { get; set; }
        

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MyViewModel()
        {
            //Initialize Import Page Pie Chart
            ImportedReviewsCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "No Reviews Imported",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(0) },
                    DataLabels = true
                }
                
            };

            //Initialize Classify Page Pie Chart
            ClassfyReviewsResultsCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Bug Reports",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(0) },
                    DataLabels = true
                },
                 new PieSeries
                {
                    Title = "User Requirements",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(0) },
                    DataLabels = true
                },
                  new PieSeries
                {
                    Title = "Miscellaneous",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(0) },
                    DataLabels = true
                }
            };
        }

        private List<string> appList;
        public List<string> AppList
        {
            get { return appList; }
            set
            {
                if (value != appList)
                {
                    appList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("AppList"));
                }
            }
        }

        private List<string> reviewList;
        public List<string> ReviewList
        {
            get { return reviewList; }
            set
            {
                if (value != reviewList)
                {
                    reviewList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("AppList"));
                }
            }
        }


        private List<string> bugReportList;
        public List<string> BugReportList
        {
            get { return bugReportList; }
            set
            {
                if (value != bugReportList)
                {
                    bugReportList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("BugReportList"));
                }
            }
        }

        private List<string> userRequirementList;
        public List<string> UserRequirementList
        {
            get { return userRequirementList; }
            set
            {
                if (value != userRequirementList)
                {
                    userRequirementList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("UserRequirementList"));
                }
            }
        }

        private List<string> miscellaneousList;
        public List<string> MiscellaneousList
        {
            get { return miscellaneousList; }
            set
            {
                if (value != miscellaneousList)
                {
                    miscellaneousList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("MiscellaneousList"));
                }
            }
        }


        private List<string> bugReportSummaryList;
        public List<string> BugReportSummaryList
        {
            get { return bugReportSummaryList; }
            set
            {
                if (value != bugReportSummaryList)
                {
                    bugReportSummaryList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("BugReportSummaryList"));
                }
            }
        }

        private List<string> userRequirementsSummaryList;
        public List<string> UserRequirementsSummaryList
        {
            get { return userRequirementsSummaryList; }
            set
            {
                if (value != userRequirementsSummaryList)
                {
                    userRequirementsSummaryList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("UserRequirementsSummaryList"));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

    
}