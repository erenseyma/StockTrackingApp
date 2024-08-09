using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
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
    public partial class SiparislerUser : DevExpress.XtraEditors.XtraUserControl
    {
        public SiparislerUser()
        {
            InitializeComponent();
        }
        SatisDBEntities2 db = new SatisDBEntities2();
        void Listele()
        {
            var items = (from s in db.OrdersTBL
                         select new
                         {
                             s.OrderID,
                             s.CustomerID,
                             s.OrderDate,
                             s.TotalAmount,
                             s.OrderStatus,
                             s.ShippingDate,
                             s.TrackingNumber
                         }


                        ).ToList();
            gridControl1.DataSource = items;
        }
        private void SiparislerUser_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void textEdit5_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (int)gridView1.GetFocusedRowCellValue("OrderID");
                if (id <= 0)
                {
                    XtraMessageBox.Show("Silinecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                using (var db = new SatisDBEntities2())
                {
                    var order = db.OrdersTBL.Find(id);
                    if (order == null)
                    {
                        XtraMessageBox.Show("Silinecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    db.OrdersTBL.Remove(order);
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textEdit1.Text) || !int.TryParse(textEdit1.Text, out int customerId))
                {
                    XtraMessageBox.Show("Lütfen geçerli bir Müşteri ID girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(textEdit2.Text) || !DateTime.TryParse(textEdit2.Text, out DateTime orderDate))
                {
                    XtraMessageBox.Show("Lütfen geçerli bir Sipariş Tarihi girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(textEdit3.Text) || !decimal.TryParse(textEdit3.Text, out decimal totalAmount))
                {
                    XtraMessageBox.Show("Lütfen geçerli bir Toplam Tutar girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string orderStatus = textEdit4.Text;

                DateTime? shippingDate = null;
                if (!string.IsNullOrEmpty(textEdit5.Text) && DateTime.TryParse(textEdit5.Text, out DateTime parsedShippingDate))
                {
                    shippingDate = parsedShippingDate;
                }

                string trackingNumber = textEdit6.Text;



                var order = new OrdersTBL
                {
                    CustomerID = customerId,
                    OrderDate = orderDate,
                    TotalAmount = totalAmount,
                    OrderStatus = orderStatus,
                    ShippingDate = shippingDate,
                    TrackingNumber = trackingNumber
                };

                db.OrdersTBL.Add(order);
                db.SaveChanges();

                XtraMessageBox.Show("Sipariş başarılı bir şekilde kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                int id = (int)gridView1.GetFocusedRowCellValue("OrderID");

                if (id <= 0)
                {
                    XtraMessageBox.Show("Güncellenecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var db = new SatisDBEntities2())
                {
                    var order = db.OrdersTBL.Find(id);
                    if (order == null)
                    {
                        XtraMessageBox.Show("Güncellenecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrEmpty(textEdit2.Text) || !DateTime.TryParse(textEdit2.Text, out DateTime orderDate))
                    {
                        XtraMessageBox.Show("Lütfen geçerli bir Sipariş Tarihi girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    order.CustomerID = int.Parse(textEdit1.Text);
                    order.OrderDate = orderDate;
                    order.TotalAmount = decimal.Parse(textEdit3.Text);
                    order.OrderStatus = textEdit4.Text;
                    order.ShippingDate = DateTime.Parse(textEdit5.Text);
                    order.TrackingNumber = textEdit6.Text;

                    db.SaveChanges();
                }

                Listele();

                XtraMessageBox.Show("Sipariş başarılı bir şekilde güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var value1 = gridView1.GetFocusedRowCellValue("CustmerID");
            textEdit1.Text = value1 != null ? value1.ToString() : string.Empty;
            textEdit2.Text = gridView1.GetFocusedRowCellValue("OrderDate").ToString();
            textEdit3.Text = gridView1.GetFocusedRowCellValue("TotalAmount")?.ToString();

            textEdit4.Text = gridView1.GetFocusedRowCellValue("OrderStatus").ToString();
            var value = gridView1.GetFocusedRowCellValue("ShippingDate");
            textEdit5.Text = value != null ? value.ToString() : string.Empty;

            textEdit6.Text = gridView1.GetFocusedRowCellValue("TrackingNumber").ToString();


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
                report.SetDataSource(dataTable, "Siparişler  Rapor");

                // Raporu göster
                ReportPrintTool printTool = new ReportPrintTool(report);
                printTool.ShowPreview();
        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }
    } 
}
