using DevExpress.Charts.Native;
using DevExpress.Utils.About;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
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
using DevExpress.XtraReports.UI;

namespace TakipProjesi.Formlar
{
    public partial class SatislarUser : DevExpress.XtraEditors.XtraUserControl
    {
        public SatislarUser()
        {
            InitializeComponent();

        }
        SatisDBEntities2 db = new SatisDBEntities2();
        void Listele()
        {
            var items =(from s in db.SalesTBL
                        select new
                        {
                            s.SaleID,
                            s.CustomerID,
                            s.ProductID,
                            s.SaleDate,
                            s.Quantity,
                            s.TotalAmount,
                            s.PaymentMethod,
                            s.ShippingAddress,
                            s.Status,
                            s.SalesRapID
                        }
                        
                        ).ToList();
            gridControl1.DataSource = items;
        }

        private void SatislarUser_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

                try
                {
                    int id = (int)gridView1.GetFocusedRowCellValue("SaleID");

                    if (id <= 0)
                    {
                        XtraMessageBox.Show("Silinecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (var db = new SatisDBEntities2())
                    {
                        var deger = db.SalesTBL.Find(id);
                        if (deger == null)
                        {
                            XtraMessageBox.Show("Silinecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    var orders = db.OrdersTBL.Where(o => o.CustomerID == id).ToList();

                    db.OrdersTBL.RemoveRange(orders);


                    

                    db.SalesTBL.Remove(deger);
                        db.SaveChanges();
                    }

                    Listele();

                    XtraMessageBox.Show("Satış başarılı bir şekilde silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            


        }
        SalesTBL tbl = new SalesTBL();
        private void simpleButton1_Click(object sender, EventArgs e)//ekle
        {
                try
                {
                    using (var db = new SatisDBEntities2())
                    {
                        if (!int.TryParse(textEdit1.Text, out int customerID))
                        {
                            XtraMessageBox.Show("Geçersiz Müşteri ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!int.TryParse(textEdit2.Text, out int productID))
                        {
                            XtraMessageBox.Show("Geçersiz Ürün ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!DateTime.TryParse(textEdit3.Text, out DateTime saleDate))
                        {
                            XtraMessageBox.Show("Geçersiz Satış Tarihi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!int.TryParse(textEdit4.Text, out int quantity))
                        {
                            XtraMessageBox.Show("Geçersiz Miktar.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!decimal.TryParse(textEdit5.Text, out decimal totalAmount))
                        {
                            XtraMessageBox.Show("Geçersiz Toplam Tutar.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!int.TryParse(textEdit11.Text, out int salesRepID))
                        {
                            XtraMessageBox.Show("Geçersiz Satış Temsilcisi ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var sale = new SalesTBL
                        {
                            CustomerID = customerID,
                            ProductID = productID,
                            SaleDate = saleDate,
                            Quantity = quantity,
                            TotalAmount = totalAmount,
                            PaymentMethod = textEdit8.Text,
                            ShippingAddress = textEdit9.Text,
                            Status = textEdit10.Text,
                            SalesRapID = salesRepID
                        };
    
                        db.SalesTBL.Add(sale);
                        var stock = db.ProductTBL.SingleOrDefault(s => s.ProductID == productID);

                        if (stock != null)
                        {
                            stock.StockQuantity -= quantity;
                        }
                            
                        db.SaveChanges();

                        if (sale.Status == "Onaylandı")
                        {
                            var order = new OrdersTBL
                            {
                                CustomerID = sale.CustomerID,
                                OrderDate = saleDate,
                                TotalAmount = sale.TotalAmount,
                                OrderStatus = "Yolda",
                                ShippingDate = DateTime.Now, // Başlangıçta boş olabilir
                                TrackingNumber = GenerateRandomTrackingNumber() // 9 haneli rastgele takip numarası
                            };

                            db.OrdersTBL.Add(order);
                            db.SaveChanges();
                        var product = db.ProductTBL.Find(sale.ProductID);
                        if (product != null)
                        {
                            product.StockQuantity -= sale.Quantity;
                            db.SaveChanges();
                        }
                    }
                    }

                    Listele();

                    XtraMessageBox.Show("Satış ve ilgili sipariş başarılı bir şekilde eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

           


            string GenerateRandomTrackingNumber()
            {
                Random random = new Random();
                return random.Next(100000000, 999999999).ToString();
            }


        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
                try
                {

                    int id = (int)gridView1.GetFocusedRowCellValue("SaleID");


                    if (id <= 0)
                    {
                        XtraMessageBox.Show("Güncellenecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    using (var db = new SatisDBEntities2())
                    {

                        var sale = db.SalesTBL.Find(id);
                        if (sale == null)
                        {
                            XtraMessageBox.Show("Güncellenecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        sale.CustomerID = int.Parse(textEdit1.Text);
                        sale.ProductID = int.Parse(textEdit2.Text);
                        if (string.IsNullOrEmpty(textEdit3.Text) || !DateTime.TryParse(textEdit3.Text, out DateTime saleDate))
                        {
                            XtraMessageBox.Show("Lütfen geçerli bir Satış Tarihi girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        sale.SaleDate = saleDate;
                        sale.Quantity = int.Parse(textEdit4.Text);
                        sale.TotalAmount = int.Parse(textEdit5.Text);
                        sale.PaymentMethod = textEdit8.Text;
                        sale.ShippingAddress = textEdit9.Text;
                        sale.Status = textEdit10.Text;
                        sale.SalesRapID = int.Parse(textEdit11.Text);
                        db.SaveChanges();
                        
                    

                }


                    Listele();

                    XtraMessageBox.Show("Satış başarılı bir şekilde güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {

                    XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            textEdit1.Text = gridView1.GetFocusedRowCellValue("CustomerID").ToString();
            textEdit2.Text = gridView1.GetFocusedRowCellValue("ProductID").ToString();
            textEdit3.Text = gridView1.GetFocusedRowCellValue("SaleDate")?.ToString() ?? string.Empty;

            textEdit4.Text = gridView1.GetFocusedRowCellValue("Quantity").ToString();
            textEdit5.Text = gridView1.GetFocusedRowCellValue("TotalAmount").ToString();
            textEdit8.Text = gridView1.GetFocusedRowCellValue("PaymentMethod").ToString();

            textEdit9.Text = gridView1.GetFocusedRowCellValue("ShippingAddress").ToString();
            textEdit10.Text = gridView1.GetFocusedRowCellValue("Status").ToString();
            textEdit11.Text = gridView1.GetFocusedRowCellValue("SalesRapID").ToString();


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
            DataTable dataTable = GetDataFromGrid(gridControl1); // GridControl'dan veri al

            // Raporu oluştur ve veri kaynağını ayarla
            Rapor.Rapor1 report = new Rapor.Rapor1();
            report.SetDataSource(dataTable, "Satış  Raporu");

            // Raporu göster
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreview();
        }
    }
}
