using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace PhoneListWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetData();
        }

        public void GetData()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44340/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("api/contacts").Result;

            if (response.IsSuccessStatusCode)
            {
                var users = response.Content.ReadAsAsync<IEnumerable<Contacts>>().Result;
                contactgrid.ItemsSource = users;
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44340/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var contact = new Contacts();

            contact.Name = txtName.Text;
            contact.Number = txtPhone.Text;

            int number;
            bool parseNumber = Int32.TryParse(contact.Number, out number);
            bool validateNumber = number.ToString().Length == 9;

            if (string.IsNullOrWhiteSpace(contact.Name) || string.IsNullOrWhiteSpace(contact.Number))
            {
                MessageBox.Show("Must fill in all fields");
                return;
            }
            else if (!parseNumber)
            {
                MessageBox.Show("The number can only contain numbers");
                return;
            }
            else if (!validateNumber)
            {
                MessageBox.Show("The number must be 9 characters");
                return;
            }
            
            

            var response = client.PostAsJsonAsync("api/contacts", contact).Result;

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Contact Added");
                txtName.Text = "";
                txtPhone.Text = "";
                GetData();
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44340/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var name = txtSearch.Text.Trim();

            var url = "api/contacts/findContactByName?name=" + name;

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var contacts = response.Content.ReadAsAsync<IEnumerable<Contacts>>().Result;
                int contactCount = contacts.Count();
                if(contactCount == 0)
                {
                    MessageBox.Show("No contacts found");
                    return;
                }
                else
                {
                    contactgrid.ItemsSource = contacts;
                    txtSearch.Text = "Name";
                }
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44340/");

            var id = txtDelete.Text.Trim();

            var url = "api/contacts/" + id;

            HttpResponseMessage response = client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Contact Deleted");
                txtDelete.Text = "Id";
                GetData();
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }
    }
}
