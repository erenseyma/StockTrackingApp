using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TakipProjesi.Entity;
using TakipProjesi.Formlar.Rapor;

namespace TakipProjesi.Formlar
{
    public partial class UrunlerUser : DevExpress.XtraEditors.XtraUserControl
    {
        public UrunlerUser()
        {
            InitializeComponent();
        }
        SatisDBEntities2 db = new SatisDBEntities2();
        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {

            GridView view = sender as GridView;
            if (view == null) return;

            if (e.RowHandle < 0) return;

            var stockQuantityObject = view.GetRowCellValue(e.RowHandle, view.Columns["StockQuantity"]);

            if (stockQuantityObject != null && int.TryParse(stockQuantityObject.ToString(), out int stockQuantity))
            {
                if (stockQuantity <= 20)
                {
                    e.Appearance.BackColor = Color.Salmon;
                    e.Appearance.BackColor2 = Color.SeaShell;
                    e.HighPriority = true; 
                }
            }

        }
        void Listele()
        {

            var items = (from u in db.ProductTBL
                         select new
                         {
                             u.ProductID,
                             u.ProductName,
                             u.Marka,
                             u.CategoryID,
                             u.Price,
                             u.StockQuantity,
                             u.Desccription
                         }
                         ).ToList();
            gridControl1.DataSource = items;
        }

        private void UrunlerUser_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (int)gridView1.GetFocusedRowCellValue("ProductID");

                if (id <= 0)
                {
                    XtraMessageBox.Show("Silinecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var db = new SatisDBEntities2())
                {
                    var product = db.ProductTBL.Find(id);
                    if (product == null)
                    {
                        XtraMessageBox.Show("Silinecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    bool hasUnapprovedSales = db.SalesTBL.Any(s => s.ProductID == id && s.Status == "Onaylanmadı");
                    if (hasUnapprovedSales)
                    {
                        XtraMessageBox.Show("Ürün onaylanmamış satışlara sahip olduğu için silinemiyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    db.ProductTBL.Remove(product);
                    db.SaveChanges();
                }

                Listele();

                XtraMessageBox.Show("Kayıt başarılı bir şekilde silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var productName = gridView1.GetFocusedRowCellValue("ProductName")?.ToString() ?? string.Empty;
            textEdit1.Text = productName;
            
            textEdit2.Text = gridView1.GetFocusedRowCellValue("Marka")?.ToString() ?? string.Empty;
            textEdit3.Text = gridView1.GetFocusedRowCellValue("CategoryID")?.ToString() ?? string.Empty;
            textEdit4.Text = gridView1.GetFocusedRowCellValue("Price")?.ToString() ?? string.Empty;
            textEdit5.Text = gridView1.GetFocusedRowCellValue("StockQuantity")?.ToString() ?? string.Empty;
            textEdit6.Text = gridView1.GetFocusedRowCellValue("Description")?.ToString() ?? string.Empty;

        }

        private void textEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textEdit1.Text))
                {
                    XtraMessageBox.Show("Lütfen Ürün Adı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(textEdit2.Text))
                {
                    XtraMessageBox.Show("Lütfen Marka girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(textEdit3.Text) || !int.TryParse(textEdit3.Text, out int categoryId))
                {
                    XtraMessageBox.Show("Lütfen geçerli bir Kategori ID girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(textEdit4.Text) || !decimal.TryParse(textEdit4.Text, out decimal price))
                {
                    XtraMessageBox.Show("Lütfen geçerli bir Fiyat girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(textEdit5.Text) || !int.TryParse(textEdit5.Text, out int stockQuantity))
                {
                    XtraMessageBox.Show("Lütfen geçerli bir Stok Miktarı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string description = textEdit6.Text;

                var product = new ProductTBL
                {
                    ProductName = textEdit1.Text,
                    Marka = textEdit2.Text,
                    CategoryID = categoryId,
                    Price = price,
                    StockQuantity = stockQuantity,
                    Desccription = description 
                };

                db.ProductTBL.Add(product);
                db.SaveChanges();

                XtraMessageBox.Show("Ürün başarılı bir şekilde kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);


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
                int id = (int)gridView1.GetFocusedRowCellValue("ProductID");

                if (id <= 0)
                {
                    XtraMessageBox.Show("Güncellenecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var db = new SatisDBEntities2())
                {
                    var deger = db.ProductTBL.Find(id);
                    if (deger == null)
                    {
                        XtraMessageBox.Show("Güncellenecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    deger.ProductName = textEdit1.Text;
                    deger.Marka = textEdit2.Text;
                    deger.CategoryID = int.Parse(textEdit3.Text);
                    deger.Price = decimal.Parse(textEdit4.Text);
                    deger.StockQuantity = int.Parse(textEdit5.Text);
                    deger.Desccription = textEdit6.Text;

                    db.SaveChanges();
                }

                Listele();

                XtraMessageBox.Show("Ürün başarılı bir şekilde güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

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
                throw new InvalidOperationException("GridControl's MainView is not a GridView.");
            }
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            DataTable dataTable = GetDataFromGrid(gridControl1);

            Rapor.Rapor1 report = new Rapor.Rapor1();
            report.SetDataSource(dataTable,"Ürünler Raporu");

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreview();
        }
    }
}
