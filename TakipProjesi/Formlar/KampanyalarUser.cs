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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using DevExpress.XtraReports.UI;

namespace TakipProjesi.Formlar
{
    public partial class KampanyalarUser : DevExpress.XtraEditors.XtraUserControl
    {
        public KampanyalarUser()
        {
            InitializeComponent();
        }
        SatisDBEntities2 db = new SatisDBEntities2();

        void Listele()
        {
            var items=(from k in db.Campaigns
                       select new
                       {
                           k.CompaignID,
                           k.CompaignName,
                           k.StartDate,
                           k.EndDate,
                           k.DiscountRate,
                           k.Description
                       }).ToList();
            gridControl1.DataSource = items;
        }
        private void KampanyalarUser_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            
                try
                {
                    int id = (int)gridView1.GetFocusedRowCellValue("CompaignID");

                    if (id == 0)
                    {
                        XtraMessageBox.Show("Silinecek kayıt seçilmedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (var db = new SatisDBEntities2())
                    {
                        var deger = db.Campaigns.Find(id);
                        if (deger == null)
                        {
                            XtraMessageBox.Show("Silinecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        db.Campaigns.Remove(deger);
                        db.SaveChanges();
                    }

                    Listele();

                    XtraMessageBox.Show("Kampanya başarılı bir şekilde silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            

        }
        Campaigns tbl = new Campaigns();
        private void simpleButton1_Click(object sender, EventArgs e)
        {
                try
                {
                    
                    if (string.IsNullOrEmpty(textEdit1.Text))
                    {
                        XtraMessageBox.Show("Lütfen Kampanya Adı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrEmpty(textEdit2.Text) || !DateTime.TryParse(textEdit2.Text, out DateTime startDate))
                    {
                        XtraMessageBox.Show("Lütfen geçerli bir Başlangıç Tarihi girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrEmpty(textEdit3.Text) || !DateTime.TryParse(textEdit3.Text, out DateTime endDate))
                    {
                        XtraMessageBox.Show("Lütfen geçerli bir Bitiş Tarihi girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrEmpty(textEdit4.Text) || !decimal.TryParse(textEdit4.Text, out decimal discountRate))
                    {
                        XtraMessageBox.Show("Lütfen geçerli bir İndirim Oranı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string description = textEdit6.Text;

                    var campaign = new Campaigns
                    {
                        
                        CompaignName = textEdit1.Text,
                        StartDate = startDate,
                        EndDate = endDate,
                        DiscountRate = discountRate,
                        Description = description
                    };

                    db.Campaigns.Add(campaign);
                    db.SaveChanges();

                    XtraMessageBox.Show("Kampanya başarılı bir şekilde kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                int id = (int)gridView1.GetFocusedRowCellValue("CompaignID");

                if (id <= 0)
                {
                    XtraMessageBox.Show("Güncellenecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var db = new SatisDBEntities2())
                {
                    var campaign = db.Campaigns.Find(id);
                    if (campaign == null)
                    {
                        XtraMessageBox.Show("Güncellenecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    campaign.CompaignName = textEdit1.Text;

                    if (string.IsNullOrEmpty(textEdit2.Text) || !DateTime.TryParse(textEdit2.Text, out DateTime startDate))
                    {
                        XtraMessageBox.Show("Lütfen geçerli bir Başlangıç Tarihi girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    campaign.StartDate = startDate;

                    if (string.IsNullOrEmpty(textEdit3.Text) || !DateTime.TryParse(textEdit3.Text, out DateTime endDate))
                    {
                        XtraMessageBox.Show("Lütfen geçerli bir Bitiş Tarihi girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    campaign.EndDate = endDate;

                    if (decimal.TryParse(textEdit4.Text, out decimal discountRate))
                    {
                        campaign.DiscountRate = discountRate;
                    }
                    else
                    {
                        XtraMessageBox.Show("Lütfen geçerli bir İndirim Oranı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    campaign.Description = textEdit6.Text;

                    db.SaveChanges();
                }

                Listele();

                XtraMessageBox.Show("Kampanya başarılı bir şekilde güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            textEdit1.Text = gridView1.GetFocusedRowCellValue("CompaignName").ToString();
            textEdit2.Text = gridView1.GetFocusedRowCellValue("StartDate").ToString();
            textEdit3.Text = gridView1.GetFocusedRowCellValue("EndDate")?.ToString() ?? string.Empty;

            textEdit4.Text = gridView1.GetFocusedRowCellValue("DiscountRate").ToString();
            textEdit6.Text = gridView1.GetFocusedRowCellValue("Description").ToString();
            
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
            DataTable dataTable = GetDataFromGrid(gridControl1); // GridControl'dan veri al

            
            Rapor.Rapor1 report = new Rapor.Rapor1();
            report.SetDataSource(dataTable, "Kampanyalar Rapor");

          
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreview();
        }
    }
}
