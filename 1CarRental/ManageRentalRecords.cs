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
    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities _db; //db connection
        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddRentalRecodrd
            {
                MdiParent = this.MdiParent
            };
            addRentalRecord.Show();

        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value; // 0 row, bo to jest kolekcja w której możesz zaznaczyć więcej niż jeden wiersz, noo i potem chcemy komórkę z Id, 

                // query database for record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.Id == id);// trzeba było przeparsować u góry z (int)



                // lanuch AddEditVehicle window with data
                var addEditRentalRecord = new AddRentalRecodrd(record);
                addEditRentalRecord.MdiParent = this.MdiParent;
                addEditRentalRecord.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.Id == id);

                //delete vehicle from table
                _db.CarRentalRecords.Remove(record);
                _db.SaveChanges();

                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e) // utworzenie to 2x kliknięcie w obszar okienka
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

        private void PopulateGrid()
        {
            var records = _db.CarRentalRecords.Select(q => new
            {
                Customer = q.CustomreName,
                DateOut = q.DateRented,
                DateIn = q.DateReturned,
                id = q.Id,
                q.Cost,
                Car = q.TypesOfCar.Make + " " +q.TypesOfCar.Model //inner join

            }).ToList(); ;

            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateIn"].HeaderText = "Date In";
            gvRecordList.Columns["DateOut"].HeaderText = "Date Out";
            //Hide the column for ID. Changed from the hard coded clumn value to the name to make it more dynamic
            gvRecordList.Columns["id"].Visible = false;
        }
    }
}
