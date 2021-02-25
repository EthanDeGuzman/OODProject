using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using Newtonsoft.Json;

namespace OODProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Create List
        List<Company> allCompanies = new List<Company>();

        public MainWindow()
        {
            InitializeComponent();
        }

        // When window is loaded execute this
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] Sort = { "Name", "Year" };
            cbxSort.ItemsSource = Sort;

            //Code for setting up the clock
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render); // Prioritizes rendering the clock on the window
            timer.Tick += new EventHandler(timer_Tick); // Constantly updates the clock
            timer.Interval = new TimeSpan(0, 0, 1); // Go up every 1 second
            timer.Start(); //Starts the clock

            using (StreamReader sr = new StreamReader(@"../Company.json"))
            {
                string json = sr.ReadToEnd();

                //Convert JSON File to a List
                allCompanies = JsonConvert.DeserializeObject<List<Company>>(json);

                //Display List to List Box
                lstCompany.ItemsSource = allCompanies;
            }

            //Code to Write to JSON
            //string json = JsonConvert.SerializeObject(allCompanies, Formatting.Indented);
            //using(StreamWriter sw = new StreamWriter(@"../Company.json"))
            //{
            //    sw.Write(json);
            //}

        }

        //Displays The Time on the window and in an hour/minutes/seconds/format
        public void timer_Tick(object sender, EventArgs e)
        {
            tblkClock.Text = DateTime.Now.ToString(@"hh\:mm\:ss");
        }

        private void cbxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstCompany.ItemsSource = null;
            string selectedSort = cbxSort.SelectedItem as string;

            // Sort Company by Name or Year 
            if (selectedSort == "Name")
                allCompanies.Sort();
            else
            {
                SortByYearByAscendingOrder eAsc = new SortByYearByAscendingOrder();
                allCompanies.Sort(eAsc);
            }
                
            lstCompany.ItemsSource = allCompanies;
        }

        //IComparer Interface
        public class SortByYearByAscendingOrder : IComparer<Company>
        {
            public int Compare(Company x, Company y)
            {
                if (x.YearFormed > y.YearFormed) return 1;
                else if (x.YearFormed < y.YearFormed) return -1;
                else return 0;
            }
        }

        //Selection Change method for listbox
        private void lstCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Company selectedCompany = lstCompany.SelectedItem as Company;

            if (selectedCompany != null)
            {
                lstGames.ItemsSource = selectedCompany.GamesList;
                tblkCompany.Text = string.Format($"Company Name: {selectedCompany.CompanyName}" + $"\nYear Formed: {selectedCompany.YearFormed}" + $"\nFounders: {selectedCompany.Founders}");
            }
        }

    }
}
