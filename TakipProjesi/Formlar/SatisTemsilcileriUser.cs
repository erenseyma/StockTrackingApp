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

namespace TakipProjesi.Formlar
{
    public partial class SatisTemsilcileriUser : DevExpress.XtraEditors.XtraUserControl
    {
        public SatisTemsilcileriUser()
        {
            InitializeComponent();
        }
        SatisDBEntities2 db = new SatisDBEntities2();

        void Listele()
        {
            var items=(from sa in db.SalesRepresentativeTBL 
                       select new
                       {
                           sa.SalesRepID,
                           sa.FirstName,
                           sa.LastName,
                           sa.Email,
                           sa.Phone,
                           sa.HireDate,
                           sa.Region
                       }).ToList();
            gridControl1.DataSource = items;
        }
        private void SatisTemsilcileriUser_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
                try
                {
                    int id = (int)gridView1.GetFocusedRowCellValue("SalesRepID");

                    if (id <= 0)
                    {
                        XtraMessageBox.Show("Silinecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (var db = new SatisDBEntities2())
                    {
                        var deger = db.SalesRepresentativeTBL.Find(id);
                        if (deger == null)
                        {
                            XtraMessageBox.Show("Silinecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        db.SalesRepresentativeTBL.Remove(deger);
                        db.SaveChanges();
                    }

                    Listele();

                    XtraMessageBox.Show("Satış Temsilcisi başarılı bir şekilde silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                    if (string.IsNullOrEmpty(textEdit5.Text) || !DateTime.TryParse(textEdit5.Text, out DateTime hireDate))
                    {
                        XtraMessageBox.Show("Lütfen geçerli bir İşe Başlama Tarihi girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string region = textEdit6.Text;

                    var salesRep = new SalesRepresentativeTBL
                    {
                        FirstName = textEdit1.Text,
                        LastName = textEdit2.Text,
                        Email = textEdit3.Text,
                        Phone = textEdit4.Text,
                        HireDate = hireDate,
                        Region = region
                    };

                    db.SalesRepresentativeTBL.Add(salesRep);
                    db.SaveChanges();

                    XtraMessageBox.Show("Satış Temsilcisi başarılı bir şekilde kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    int id = (int)gridView1.GetFocusedRowCellValue("SalesRepID"); 

                    if (id <= 0)
                    {
                        XtraMessageBox.Show("Güncellenecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (var db = new SatisDBEntities2())
                    {
                        var salesRep = db.SalesRepresentativeTBL.Find(id);
                        if (salesRep == null)
                        {
                            XtraMessageBox.Show("Güncellenecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        salesRep.FirstName = textEdit1.Text;
                        salesRep.LastName = textEdit2.Text;
                        salesRep.Email = textEdit3.Text;
                        salesRep.Phone = textEdit4.Text;

                        if (string.IsNullOrEmpty(textEdit5.Text) || !DateTime.TryParse(textEdit5.Text, out DateTime hireDate))
                        {
                            XtraMessageBox.Show("Lütfen geçerli bir İşe Başlama Tarihi girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        salesRep.HireDate = hireDate;

                        salesRep.Region = textEdit6.Text;
                        db.SaveChanges();
                    }

                    
                    Listele();

                    XtraMessageBox.Show("Satış Temsilcisi başarılı bir şekilde güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            textEdit5.Text = gridView1.GetFocusedRowCellValue("HireDate").ToString();
            textEdit6.Text = gridView1.GetFocusedRowCellValue("Region").ToString();
        }
        DataTable GetDataFromGrid(GridControl gridControl)
        {
            GridView gridView = gridControl.MainView as GridView;
            if (gridView != null)
            {
                DataTable dataTable = new DataTable();

                // GridView sütunlarını DataTable'a ekleme
                foreach (GridColumn column in gridView.Columns)
                {
                    dataTable.Columns.Add(column.FieldName);
                }

                // GridView'dan DataTable'a veri ekleme
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
                throw new InvalidOperationException("GridControl's MainView is not a GridView.");
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            DataTable dataTable = GetDataFromGrid(gridControl1);

            Rapor.Rapor1 report = new Rapor.Rapor1();
            report.SetDataSource(dataTable, "Satış Temsilcileri Raporu");
             
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreview();
        }
    }
}
