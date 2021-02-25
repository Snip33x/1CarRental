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
    public partial class AddUser : Form
    {
        private readonly CarRentalEntities _db;
        //private ManageUsers _manageUsers;
        public AddUser()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            //_manageUsers = manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = _db.Roles.ToList();
            cbRoles.DataSource = roles;
            cbRoles.ValueMember = "id";
            cbRoles.DisplayMember = "name";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            
            
            var username = tbUsername.Text;
            var roleId = (int)cbRoles.SelectedValue;
            var password = Utils.DefaultHashPassword();
            var user = new User
            {
                username = username,
                password = password,
                isActive = true
            };
            _db.Users.Add(user);
            _db.SaveChanges();

            var userid = user.id;

            var userRole = new UserRole
            {
                roleid = roleId,
                userid = userid
            };

            _db.UserRoles.Add(userRole);
            _db.SaveChanges();

            MessageBox.Show("New User Added SUccessfully");
            //_manageUsers.PopulateGrid();
            Close();
            
            //catch (Exception)
            //{

            //    MessageBox.Show("An Error Has Occured");
            //}
        }
    }
}





/*using System;
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
    public partial class AddUser : Form
    {
        private readonly CarRentalEntities _db;
        public AddUser()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = _db.Roles.ToList();
            cbRoles.DataSource = roles;
            cbRoles.ValueMember = "id";
            cbRoles.DisplayMember = "name";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //combo box, submitowanie nowego usera
                var username = tbUsername.Text;
                var roleId = (int)cbRoles.SelectedValue;
                var password = Utils.DefaultHashPassword();
                //id samo się inkrementuję dzięki db
                var user = new User
                {
                    username = username,
                    password = password,
                    isActive = true
                };
                _db.Users.Add(user);
                _db.SaveChanges();

                var userID = user.id;

                var userRole = new UserRole
                {
                    roleid = roleId,
                    userid = userID
                };

                _db.UserRoles.Add(userRole);
                _db.SaveChanges();

                //// dodawanie w db do User role jaką pełni nowy user na podstawie tego co wybraliśmy w add
                //var userid = user.id;

                //var userRole = new UserRole
                //{
                //    roleid = roleId,
                //    userid = userid
                //};

                //_db.UserRoles.Add(userRole);
                //_db.SaveChanges();

                //MessageBox.Show("New user added sucessfully");
                //Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"An Error Has Occured {ex.Message}");
            }

        }
    }
}
*/