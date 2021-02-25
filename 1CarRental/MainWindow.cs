using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1CarRental
{
    public partial class MainWindow : Form
    {
        private Login _login;
        public string _roleName; // main window will have a pulbicy accessible property called a Rolename wchoch will allow any window that needs to know what rule is the current loged person in 
        public User _user; //property - te trzy
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(Login login, User user) //now i can do what i want with login
        {
            InitializeComponent();
            _login = login;
            _user = user;
            _roleName = user.UserRoles.FirstOrDefault().Role.shortname; // firstOr Default - only one instance of an object ==  one result
            
    }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRentalRecodrd addRentalRecodrdForm = new AddRentalRecodrd();
            addRentalRecodrdForm.ShowDialog(); // teraz już nie pojawi nam się okienko w okienku głownym, ale za to dopóki go nie zamkniemy, to nic innego nie da się zrobić
            addRentalRecodrdForm.MdiParent = this;  // mdi apperance expects some form or some object of a form to be assigned // to sprawia że okienko pojawi się w głownym i nie będzie się dało nim wyjechać poza
            //addRentalRecodrdForm.Show();   // this == this class jak się najedzie na this to podświetla nazwę klasy
        }

        private void manageVehiceleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var OpenForms = Application.OpenForms.Cast<Form>(); // give me a list of open forms
            var isOpen = OpenForms.Any(q => q.Name == "ManageVehicleListing");
            
            if (!isOpen)
            {
                var vehicleListing = new ManageVehicleListing();
                vehicleListing.MdiParent = this;
                vehicleListing.Show();
            }


        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var OpenForms = Application.OpenForms.Cast<Form>(); // give me a list of open forms
            var isOpen = OpenForms.Any(q => q.Name == "ManageRentalRecords"); // tu musi być podana w cudzysłowie nazwa klasy
            Console.WriteLine(OpenForms);

            if (!isOpen)
            {
                var manageRentalRecords = new ManageRentalRecords();
                manageRentalRecords.MdiParent = this;
                manageRentalRecords.Show();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Close(); // zamykamy login formę która jest ukryta, bo tak będzie cały czas włączona w tle
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var OpenForms = Application.OpenForms.Cast<Form>(); // give me a list of open forms
            var isOpen = OpenForms.Any(q => q.Name == "ManageUsers"); // tu musi być podana w cudzysłowie nazwa klasy // niekoniecznie, bo nie działa na usersach XXX
            
            if (!isOpen)
            {
                var manageUsers = new ManageUsers();
                manageUsers.MdiParent = this;
                manageUsers.Show();
            }
        }

        private void MainWindow_Load(object sender, EventArgs e) // chowamy przyciski dla nie adminów
        {
            var username = _user.username;
            tsiLoginText.Text = $"Logged In As: {username}";
            if(_roleName != "admin")
            {
                manageUsersToolStripMenuItem.Visible = false; //Enabled żeby całkiem wyrzucić
            }
        }
    }
}
