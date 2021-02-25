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
    public partial class AddRentalRecodrd : Form
    {
        private bool isEditMode;
        // linijka niżej to private property
        private readonly CarRentalEntities _db;  // - wszystko Entity =Jednostka, istota  

        public AddRentalRecodrd()
        {
            InitializeComponent();
            lblTitle.Text = "Add New Rental Record";
            this.Text = "Add New Rental Record"; //nazwa okienka
            isEditMode = false;
            _db = new CarRentalEntities(); // to jest podejrzewam połączenie z db
        }

        public AddRentalRecodrd(CarRentalRecord recordToEdit)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";
            if (recordToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();
                PopulateFields(recordToEdit);
            }
        }

        private void PopulateFields(CarRentalRecord recordToEdit)
        {
            tbCustomerName.Text = recordToEdit.CustomreName;
            dtRented.Value = (DateTime)recordToEdit.DateRented; // allow null
            dtReturned.Value = (DateTime)recordToEdit.DateReturned;
            tbCost.Text = recordToEdit.Cost.ToString();
            lblRecordId.Text = recordToEdit.Id.ToString(); // ukryty label, bo potrzebujemy ID
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = tbCustomerName.Text;
                var dateOut = dtRented.Value;
                var dateIn = dtReturned.Value;
                double cost = Convert.ToDouble(tbCost.Text); //albo parse

                var carType = TypeofCarCombo.Text;
                var isValid = true;
                var errorMessage = "";

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += "Error: Please enter missing data.\n\r";
                    //MessageBox.Show("Please enter missing data.");
                }

                if (dateOut > dateIn)
                {
                    isValid = false;
                    errorMessage += "Error: Illegal date selection.\n\r";
                    //MessageBox.Show("Illegal date selection");
                }

                //isValid == true;
                if (isValid)
                {
                    //Declare an object of the record to be added
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode) //jeżeli jesteśmy w Edit Modzie
                    {
                        //If in edit mode, then get the ID and retrieve the record from the database and place
                        //the result in the record object
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = _db.CarRentalRecords.FirstOrDefault(Queryable => Queryable.Id == id);
                    }
                    //populate record object with valuse from the form
                    // WKŁADAMY DANE DO BAZY DANYCH
                    rentalRecord.CustomreName = customerName;
                    rentalRecord.DateRented = dateOut;
                    rentalRecord.DateReturned = dateIn;
                    rentalRecord.Cost = (decimal)cost;   //zmieniamy double na decimala
                    rentalRecord.TypeOfCarID = (int)TypeofCarCombo.SelectedValue; //z combo fielda bierzemy Id tej marki samochodu  - rzutujemy że chcemy inta
                    //ten rentalRecord istnieje w if - a ten poniżej w else - to są dwa różne

                    //if not in edit mode, then add the record object ot the database
                    if (!isEditMode)
                        _db.CarRentalRecords.Add(rentalRecord);
                    //Save Changes made to the entity       
                    _db.SaveChanges();

                    // Message Box bo submicie
                    MessageBox.Show($"Thank you for Renting {customerName}" +
                        $"\nDate Rented: {dateOut}" +
                        $"\nDate Returned: {dateIn}" +
                        $"\nCost: {cost}" +
                        $"\nType of Car: {carType}");
                    Close();
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {   // Slecet * from TypesOfCars
            //var cars = carRentalEntities.TypesOfCars.ToList();

            var cars = _db.TypesOfCars
                .Select(q => new { Id = q.Id, Name = q.Make + " " + q.Model }).ToList();

            TypeofCarCombo.DisplayMember = "Name";  //Name to nazwa kolumny w db
            TypeofCarCombo.ValueMember = "Id";  // bez tego określone rzutowanie jest niepoprawne kiedy jest próba wrzucania do db
            TypeofCarCombo.DataSource = cars;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); // koleś dał tu vara ale u mnie psuje kod // bo nie dawał pierwszego słowa klasy
            mainWindow.Show(); // odpalamy nowe okienko - przypisane do buttona - bez ifa możemy otwierać wiele okienek
        }
    }
}
