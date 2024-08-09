using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TakipProjesi.Entity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TakipProjesi.Formlar
{
    public partial class MusterilerUser : DevExpress.XtraEditors.XtraUserControl
    {
        public MusterilerUser()
        {
            InitializeComponent();
        }
        SatisDBEntities2 db = new SatisDBEntities2();

        void Listele()
        {
            var items = (from m in db.CustomerTBL
                         select new
                         {
                             m.CustomerID,
                             m.FirstName,
                             m.LastName,
                             m.Email,
                             m.Phone,
                             m.Address,
                             m.Country,
                             m.RegistirationDate
                         }).ToList();
            gridControl1.DataSource = items;
        }
        private void MusterilerUser_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            
                try
                {
                    int id = (int)gridView1.GetFocusedRowCellValue("CustomerID");

                    if (id <= 0)
                    {
                        XtraMessageBox.Show("Silinecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (var db = new SatisDBEntities2())
                    {
                        var deger = db.CustomerTBL.Find(id);
                        if (deger == null)
                        {
                            XtraMessageBox.Show("Silinecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // İlgili siparişleri ve satışlarda silinecek
                        var orders = db.OrdersTBL.Where(o => o.CustomerID == id).ToList();
                        var sales = db.SalesTBL.Where(s => s.CustomerID == id).ToList();

                        db.OrdersTBL.RemoveRange(orders);
                        db.SalesTBL.RemoveRange(sales);

                        db.CustomerTBL.Remove(deger);
                        db.SaveChanges();
                    }

                    Listele();

                    XtraMessageBox.Show("Müşteri başarılı bir şekilde silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
                try
                {

                    if (string.IsNullOrEmpty(textEdit1.Text))
                    {
                        XtraMessageBox.Show("Lütfen Ad girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrEmpty(textEdit2.Text))
                    {
                        XtraMessageBox.Show("Lütfen Soyad girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrEmpty(textEdit3.Text))
                    {
                        XtraMessageBox.Show("Lütfen E-posta girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrEmpty(textEdit4.Text))
                    {
                        XtraMessageBox.Show("Lütfen Telefon numarası girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string address = textEdit5.Text;

                    string country = textEdit6.Text;


                    var customer = new CustomerTBL
                    {
                        FirstName = textEdit1.Text,
                        LastName = textEdit2.Text,
                        Email = textEdit3.Text,
                        Phone = textEdit4.Text,
                        Address = address,
                        Country = country,
                        RegistirationDate = DateTime.Now
                    };

                    db.CustomerTBL.Add(customer);
                    db.SaveChanges();

                    XtraMessageBox.Show("Müşteri başarılı bir şekilde kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Listele();
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
                try
                {
                    int id = (int)gridView1.GetFocusedRowCellValue("CustomerID");

                    if (id <= 0)
                    {
                        XtraMessageBox.Show("Güncellenecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (var db = new SatisDBEntities2())
                    {
                        var customer = db.CustomerTBL.Find(id);
                        if (customer == null)
                        {
                            XtraMessageBox.Show("Güncellenecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        customer.FirstName = textEdit1.Text;
                        customer.LastName = textEdit2.Text;
                        customer.Email = textEdit3.Text;
                        customer.Phone = textEdit4.Text;
                        customer.Address = textEdit5.Text;
                        customer.Country = textEdit6.Text;

                        

                        db.SaveChanges();
                    }

                    
                    Listele();

                    XtraMessageBox.Show("Müşteri başarılı bir şekilde güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            textEdit1.Text = gridView1.GetFocusedRowCellValue("FirstName").ToString();
            textEdit2.Text = gridView1.GetFocusedRowCellValue("LastName").ToString();
            textEdit3.Text = gridView1.GetFocusedRowCellValue("Email")?.ToString() ?? string.Empty;

            textEdit4.Text = gridView1.GetFocusedRowCellValue("Phone").ToString();
            textEdit5.Text = gridView1.GetFocusedRowCellValue("Address").ToString();
            textEdit6.Text = gridView1.GetFocusedRowCellValue("Country").ToString();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            DataTable dataTable = GetDataFromGrid(gridControl1); 

            Rapor.Rapor1 report = new Rapor.Rapor1();
            report.SetDataSource(dataTable,"Müşteriler Rapor");

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreview();
        }
        DataTable GetDataFromGrid(GridControl gridControl)
        {
            GridView gridView = gridControl.MainView as GridView;
            if (gridView != null)
            {
                DataTable dataTable = new DataTable();

                foreach (GridColumn column in gridView.Columns)
                {
                    dataTable.Columns.Add(column.FieldName);
                }


                for (int i = 0; i < gridView.RowCount; i++)
                {
                    DataRow row = dataTable.NewRow();
                    for (int j = 0; j < gridView.Columns.Count; j++)
                    {
                        row[j] = gridView.GetRowCellValue(i, gridView.Columns[j]);
                    }
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
            else
            {
                throw new InvalidOperationException("hata.");
            }
        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textEdit4_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit5_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl6_Click(object sender, EventArgs e)
        {

        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }
    }
}
