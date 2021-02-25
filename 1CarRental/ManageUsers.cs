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
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities _db;
        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities(); //initialize db inside of constructor
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser();
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value; // 0 row, bo to jest kolekcja w której możesz zaznaczyć więcej niż jeden wiersz, noo i potem chcemy komórkę z Id, 

                // query database for record
                var user = _db.Users.FirstOrDefault(q => q.id == id);// trzeba było przeparsować u góry z (int)
                //var genericPassword = "Password123"; - tak mieliśmy zanim nie zrobiliśmy w utility aby był hardcoed password
                var hashed_password = Utils.DefaultHashPassword(); //żeby defaultowe hasło po resecie też było szyfrowane
                user.password = hashed_password;
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s Password has been reset!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value; // 0 row, bo to jest kolekcja w której możesz zaznaczyć więcej niż jeden wiersz, noo i potem chcemy komórkę z Id, 

                // query database for record
                var user = _db.Users.FirstOrDefault(q => q.id == id);// trzeba było przeparsować u góry z (int)
                user.isActive = false;
                
                //if (user.isActive == true)
                //  user.isActive = false;
                //else
                //  user.isActive = true;
                user.isActive = user.isActive == true ? false : true ;
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s Active status has been changed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
