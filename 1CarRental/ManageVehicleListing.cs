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
    public partial class ManageVehicleListing : Form
    {
        private readonly CarRentalEntities _db; //taki styl nazywania _db     //readonly bo to będzie tylko zczytywać, tak to nie konieczne
        public ManageVehicleListing()
        {
            InitializeComponent();
            _db = new CarRentalEntities(); // initialize db
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                
            }

        }
        public void PopulateGrid()
        {
            //Kiedy forma się załaduję - to ja chcę...

            //var cars = _db.TypesOfCars.ToList(); // to jest tak jakby Select * from - i przez to dorzuca nam kolumnę która jest foreign key

            //var cars = _db.TypesOfCars.Select(q => new { ID = q.Id, Name = q.Make }).ToList(); // WAŻNE!! To jest Lambda - literka q nie ma znacznia może być inna, liczy się => --- łączymy się z db - potem budujemy objekt - ID i Name to aliasy i można je nazywać w inny sposób

            // dobrze jest takie dane wyciągać jako listy  -- jak nie miałem to błąd wyrzucało
            //Select id as ID, name as Name from TypesOfCars

            var cars = _db.TypesOfCars
                .Select(q => new {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicensePlate = q.LicensePlateNumber,
                    q.Id // tak podpowiada Visual studio, że nie trzeba deklarować, ale tak wiadomo o co chodzi


                }).ToList();


            gvVehicleList.DataSource = cars;
            gvVehicleList.Columns[4].HeaderText = "License Plate Number"; // oryginalne było sklejone i brzydko wyglądało
            gvVehicleList.Columns[5].Visible = false; //chowamy id, ale będziemy z niego korzystać, w ADD i Edit
            //gvVehicleList.Columns[0].HeaderText = "ID"; //nazywamy kolumny jak chcemy
            //gvVehicleList.Columns[1].HeaderText = "Name";
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            AddEditVehicle addEditVehicle = new AddEditVehicle(this); // this do autorefresha
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value; // 0 row, bo to jest kolekcja w której możesz zaznaczyć więcej niż jeden wiersz, noo i potem chcemy komórkę z Id, 

                // query database for record
                var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);// trzeba było przeparsować u góry z (int)
                // lanuch AddEditVehicle wilndow with data
                var addEditVehicle = new AddEditVehicle(car, this); // this do autorefresha
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {

            // delejt jest gówniany bo jak usuwamy typ samochodu to wtedy usuwa wszystkie rekkordy z db z tym typem samochodu
            // jak dodamy w MS SQl w opjach foreignkey Relationship delete rule cascade bądz inny
            try
            {
                // get Id of selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);



                DialogResult dr = MessageBox.Show("Are you sure you want to delete record?", "Delete",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                
                if (dr == DialogResult.Yes)
                {
                    //delete vehicle from table
                    _db.TypesOfCars.Remove(car);
                    _db.SaveChanges();
                }
                PopulateGrid(); // autorefresh

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Simple Refresh Option
            PopulateGrid();
        }
    }
}
