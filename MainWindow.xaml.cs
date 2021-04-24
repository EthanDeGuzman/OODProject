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

/*
    Student Name: Ethan De Guzman
    Student Id: S00199053
    Github Link: 
    Extra Feature: Implementation of IComparer Method
*/

namespace OODProject
{
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

            // Try Catch for exception handling when dealing with JSON
            try
            {
                using (StreamReader sr = new StreamReader(@"../Company.json"))
                {
                    string json = sr.ReadToEnd();

                    //Convert JSON File to a List
                    allCompanies = JsonConvert.DeserializeObject<List<Company>>(json);

                    //Display List to List Box
                    lstCompany.ItemsSource = allCompanies;
                }
            }
            catch (FileNotFoundException error)
            {
                Console.WriteLine($"The file was not found: '{error}'");
            }
            catch (DirectoryNotFoundException error)
            {
                Console.WriteLine($"The directory was not found: '{error}'");
            }
            catch (IOException error)
            {
                Console.WriteLine($"The file could not be opened: '{error}'");
            }

        }

        //Displays The Time on the window and in an hour/minutes/seconds/format
        public void timer_Tick(object sender, EventArgs e)
        {
            tblkClock.Text = DateTime.Now.ToString(@"hh\:mm\:ss");
        }

        //IComparer Interface for Extra Feature
        public class SortByYearByAscendingOrder : IComparer<Company>
        {
            public int Compare(Company x, Company y)
            {
                if (x.YearFormed > y.YearFormed) return 1;
                else if (x.YearFormed < y.YearFormed) return -1;
                else return 0;
            }
        }

        //Method to Write to JSON File
        public void writeJSON()
        {
            //Code to Write to JSON with exception handling
            try
            {
                string json = JsonConvert.SerializeObject(allCompanies, Formatting.Indented);
                using (StreamWriter sw = new StreamWriter(@"../Company.json"))
                {
                    sw.Write(json);
                }
            }
            catch (FileNotFoundException error)
            {
                Console.WriteLine($"The file was not found: '{error}'");
            }
            catch (DirectoryNotFoundException error)
            {
                Console.WriteLine($"The directory was not found: '{error}'");
            }
            catch (IOException error)
            {
                Console.WriteLine($"The file could not be opened: '{error}'");
            }
        }

        //Method for Temp Fix to refresh all List Boxes
        public void refreshList()
        {
            // Temp Fix for List boxes breaking when its out of sync
            lstCompanyAdd.ItemsSource = null;
            lstCompanyAdd.ItemsSource = allCompanies;

            lstCompany.ItemsSource = null;
            lstCompany.ItemsSource = allCompanies;

            lstCompanyGameTab.ItemsSource = null;
            lstCompanyGameTab.ItemsSource = allCompanies;

            lstCompanyDelete.ItemsSource = null;
            lstCompanyDelete.ItemsSource = allCompanies;

            lstCompanyUpdateTab.ItemsSource = null;
            lstCompanyUpdateTab.ItemsSource = allCompanies;
        }

        /*=======================================================================
                         All Methods for Button Clicks in Each Tab
          =======================================================================*/
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
                foreach (var company in allCompanies) // Search for the name of the company
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
        
        //Add Button for Company which adds the new data to the JSON
        private void btnAddCompany_Click(object sender, RoutedEventArgs e)
        {
                string Name = tbxCompanyName.Text;
                int yearFormed = int.Parse(tbxYearFormed.Text);
                string founders = tbxFounders.Text;

                Company tempCompany = new Company(Name, yearFormed, founders);

                allCompanies.Add(tempCompany);

                tbxCompanyName.Text = "";
                tbxYearFormed.Text = "";
                tbxFounders.Text = "";

            writeJSON(); // Calls write code for JSON

            refreshList(); // Refreshes the List
        }

        //Add Button for Games which adds the game to the game list of the selected company
        private void btnAddGame_Click(object sender, RoutedEventArgs e)
        {
            Company tempCompany = lstCompanyGameTab.SelectedItem as Company;

            string name = tbxGameName.Text;
            int yearReleased = int.Parse(tbxYearReleased.Text);
            double price = double.Parse(tbxPrice.Text);
            string description = tbxDescription.Text;
            string avgRating = tbxRating.Text;

            Games tempGame = new Games(name, yearReleased, price, description, avgRating);

            tempCompany.GamesList.Add(tempGame);

            tbxGameName.Text = "";
            tbxYearReleased.Text = "";
            tbxPrice.Text = "";
            tbxDescription.Text = "";
            tbxRating.Text = "";

            writeJSON();
        }

        //Delete button for removing data
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Company selectedCompany = lstCompanyDelete.SelectedItem as Company;
            if (selectedCompany != null)
            {
                allCompanies.Remove(selectedCompany);
            }

            writeJSON();

            lstCompanyDelete.ItemsSource = allCompanies;
            lstGamesDelete.ItemsSource = null;

            refreshList();
        }

        //Save button for update to save new data
        private void btnUpdateSave_Click(object sender, RoutedEventArgs e)
        {
            Company selectedCompany = lstCompanyUpdateTab.SelectedItem as Company;

            if (selectedCompany != null)
            {
                selectedCompany.CompanyName = tbxUpdateCompanyName.Text;
                selectedCompany.YearFormed = int.Parse(tbxUpdateYear.Text);
                selectedCompany.Founders = tbxUpdateFounders.Text;
            }
            allCompanies.Remove(selectedCompany);

            allCompanies.Add(selectedCompany);

            writeJSON();

            tbxUpdateCompanyName.Text = "";
            tbxUpdateYear.Text = "";
            tbxUpdateFounders.Text = "";

            refreshList();
        }

        /*=======================================================================
                         All Methods for Selection Change in Each Tab
          =======================================================================*/
        //Selection Change to populate and Sort List box on Info Tab
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

        //Selection Change method for listbox for Games List in Info Tab
        private void lstCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Company selectedCompany = lstCompany.SelectedItem as Company;

            if (selectedCompany != null)
            {
                lstGames.ItemsSource = selectedCompany.GamesList;
                tblkCompany.Text = string.Format($"Company Name: {selectedCompany.CompanyName}" + $"\nYear Formed: {selectedCompany.YearFormed}" + $"\nFounders: {selectedCompany.Founders}");
            }
        }

        //Selection Change to populate Text Boxes when a company is selected on Update Tab
        private void lstCompanyUpdateTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Company selectedCompany = lstCompanyUpdateTab.SelectedItem as Company;

            if (selectedCompany != null)
            {
                tbxUpdateCompanyName.Text = selectedCompany.CompanyName;
                tbxUpdateYear.Text = selectedCompany.YearFormed.ToString();
                tbxUpdateFounders.Text = selectedCompany.Founders;
            }
        }

        //Selection Change to populate List box for Games List on Delete Tab
        private void lstCompanyDelete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Company selectedCompany = lstCompanyDelete.SelectedItem as Company;

            if (selectedCompany != null)
            {
                lstGamesDelete.ItemsSource = selectedCompany.GamesList;
            }
        }

        /*=======================================================================
                         All Methods to Load List boxes in each Tab
          =======================================================================*/
        //Loads List box of Company to Add Company Tab
        private void addCompanyTab_Loaded(object sender, RoutedEventArgs e)
        {
            lstCompanyAdd.ItemsSource = allCompanies;
        }

        //Loads List box of Company to Add Game Tab
        private void AddGame_Loaded(object sender, RoutedEventArgs e)
        {
            lstCompanyGameTab.ItemsSource = allCompanies;
        }

        //Populates List box in Update Tab
        private void UpdateTab_Loaded(object sender, RoutedEventArgs e)
        {
            lstCompanyUpdateTab.ItemsSource = allCompanies;
        }

        //Loads all companies to the list box in Delete Tab
        private void DeleteTab_Loaded(object sender, RoutedEventArgs e)
        {
            lstCompanyDelete.ItemsSource = allCompanies;
        }
    }
}
