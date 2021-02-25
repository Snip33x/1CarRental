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
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing; // do autorefresha
        private readonly CarRentalEntities _db;
        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null) // do autorefresha tu nie było niczego // it can be equal to null
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            this.Text = "Add New Vehicle"; //nazwa okienka
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing; // do autorefresha
            _db = new CarRentalEntities();
        }

        public AddEditVehicle(TypesOfCar carEdit, ManageVehicleListing manageVehicleListing = null) //overloading  -- TypesOfCar represents the data from db, and carEdit is obeject that has the data that we're going to be editing in the form
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            this.Text = "Edit Vehicle";
            _manageVehicleListing = manageVehicleListing; // do autorefresha
            if (carEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();
                PopulateFields(carEdit);
            }
                
        }

        private void PopulateFields(TypesOfCar car)
        {
            lblId.Text = car.Id.ToString(); // pokazujemy jakie Id jest aktualnie zmieniane
            tbMake.Text = car.Make;
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLicensePlateNumber.Text = car.LicensePlateNumber;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(isEditMode)
            {
                try
                {
                    // Edit Code here
                    var id = int.Parse(lblId.Text); // bo to text, ale my chcemy inta
                    var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);
                    car.Model = tbModel.Text;
                    car.Make = tbMake.Text;
                    car.VIN = tbVIN.Text;
                    car.Year = int.Parse(tbYear.Text);
                    car.LicensePlateNumber = tbLicensePlateNumber.Text;

                    // // usuwamy to bo autorefresh i wyrzucamy poza elsa savechangesa
                    //_db.SaveChanges();
                    //MessageBox.Show("Edytowano rekord");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else
            {
                //Add Code here

                var newCar = new TypesOfCar
                {
                    LicensePlateNumber = tbLicensePlateNumber.Text,
                    Make = tbMake.Text,
                    Model = tbModel.Text,
                    VIN = tbVIN.Text,
                    Year = int.Parse(tbYear.Text)
                };
                //add it to db
                _db.TypesOfCars.Add(newCar);

            }
            _db.SaveChanges();
            _manageVehicleListing.PopulateGrid(); //  autorefresh
            MessageBox.Show("Dodano nowy rekord");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); // this represents AddEditVehicle() window , .Close is opposite of .Show
        }
    }
}
