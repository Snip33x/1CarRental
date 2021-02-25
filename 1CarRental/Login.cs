using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1CarRental
{
    public partial class Login : Form
    {
        private readonly CarRentalEntities _db;
        public Login()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SHA256 sha = SHA256.Create();

                var username = tbUsername.Text.Trim(); //usuwamy spacje trimem
                var password = tbPassword.Text;



                var hashed_password = Utils.HashPassword(password); // ciągniemy z klasy cały proces szyfrowania

                var user = _db.Users.FirstOrDefault(q => q.username == username && q.password == hashed_password && q.isActive == true); //lambdą iterujemy po rekordach w db
                
                if (user == null) //if user doesn't exists
                {
                    MessageBox.Show("Please provide valid info");
                }
                else
                {
                    //var role = user.UserRoles.FirstOrDefault(); //bo chce tylko jedno  
                    //var roleShortName = role.Role.shortname;
                    //Close(); całkiem zamknie, Hide(); to będzie nadal działał w backgoundzie
                    var mainWindow = new MainWindow(this, user);  //roleShortName - jak chceliśmy tylko jedno
                    mainWindow.Show();
                    Hide(); // cały czas login jest w backgroundzie, i zamyka się razem z mainwindowem 
                    
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Something went wrong. Please try again");
            }
        }
    }
}
