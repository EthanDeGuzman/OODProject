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
        List<Company> filteredCompanies = new List<Company>();

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

        //Search Bar
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            filteredCompanies.Clear();

            lstCompany.ItemsSource = null;
            string search = tbxSearch.Text.ToLower();

            if (search == null)
            {
                lstCompany.ItemsSource = allCompanies;   
            }
            else if (Int32.TryParse(search, out int year)) // Checks if user is searching for a specific year and parses it into an int
            {
                foreach (var company in allCompanies)
                {
                    if (company.YearFormed.Equals(year))
                    {
                        filteredCompanies.Add(company);
                    }
                }
            }
            else
            {
                foreach (var company in allCompanies)
                {
                    if (company.CompanyName.ToLower().Contains(search))
                    {
                        filteredCompanies.Add(company);
                    }
                }
            }
            
            lstCompany.ItemsSource = filteredCompanies;
            tbxSearch.Text = "";
        }

        private void btnCompany_Click(object sender, RoutedEventArgs e)
        {
            lblCompany.Visibility = Visibility.Hidden;
            btnCompany.Visibility = Visibility.Hidden;
            btnGame.Visibility = Visibility.Hidden;

            ShowAddCompany();
        }

        private void btnGame_Click(object sender, RoutedEventArgs e)
        {
            lblCompany.Visibility = Visibility.Hidden;
            btnCompany.Visibility = Visibility.Hidden;
            btnGame.Visibility = Visibility.Hidden;

            ShowAddGame();
        }

        //Method to Show textbox and labels needed to add a company
        private void ShowAddCompany()
        {
            lblAddCompany.Visibility = Visibility.Visible;
            lblCompanyName.Visibility = Visibility.Visible;
            lblYearFormed.Visibility = Visibility.Visible;
            lblFounders.Visibility = Visibility.Visible;

            tbxCompanyName.Visibility = Visibility.Visible;
            tbxYearFormed.Visibility = Visibility.Visible;
            tbxFounders.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
        }

        //Method to show textbox and labels needed to add a game
        private void ShowAddGame()
        {
            lblAddGame.Visibility = Visibility.Visible;
            lblGameName.Visibility = Visibility.Visible;
            lblYearReleased.Visibility = Visibility.Visible;
            lblPrice.Visibility = Visibility.Visible;
            lblRating.Visibility = Visibility.Visible;
            lblDescription.Visibility = Visibility.Visible;

            tbxGameName.Visibility = Visibility.Visible;
            tbxYearReleased.Visibility = Visibility.Visible;
            tbxPrice.Visibility = Visibility.Visible;
            tbxRating.Visibility = Visibility.Visible;
            tbxDescription.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
        }
    }
}
