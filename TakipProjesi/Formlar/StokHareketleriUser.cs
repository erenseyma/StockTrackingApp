using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Filtering.Templates;
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
using static DevExpress.XtraEditors.Mask.MaskSettings;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TakipProjesi.Formlar
{
    public partial class StokHareketleriUser : DevExpress.XtraEditors.XtraUserControl
    {
        public StokHareketleriUser()
        {
            InitializeComponent();
        }
        SatisDBEntities2 db=new SatisDBEntities2();

        void Listele()
        {
            var items=(from st in db.InvetoryTransactionsTBL
                       select new
                       {
                           st.TransactionID,
                           st.ProductID,
                           st.TransactionDate,
                           st.Quantity,
                           st.TransactionTyppe
                       }).ToList();
            gridControl1.DataSource = items;
        }

        private void StokHareketleriUser_Load(object sender, EventArgs e)
        {
            Listele();
        }
        InvetoryTransactionsTBL tbl = new InvetoryTransactionsTBL();
        private void BtnEkle_Click(object sender, EventArgs e)
        {

                try
                {
                    
                    if (string.IsNullOrEmpty(txtad.Text) || !int.TryParse(txtad.Text, out int productId))
                    {
                        XtraMessageBox.Show("Lütfen geçerli bir Ürün ID girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrEmpty(txtmik.Text) || !int.TryParse(txtmik.Text, out int quantity))
                    {
                        XtraMessageBox.Show("Lütfen geçerli bir Miktar girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string transactionType = txtistur.Text;

                    var inventoryTransaction = new InvetoryTransactionsTBL
                    {
                        ProductID = productId,
                        TransactionDate = DateTime.Now,
                        Quantity = quantity,
                        TransactionTyppe = transactionType
                    };
                    
                    db.InvetoryTransactionsTBL.Add(inventoryTransaction);

                
                    var product = db.ProductTBL.SingleOrDefault(s => s.ProductID == productId );
                    if (product != null)
                    {
                       product.StockQuantity +=quantity;
                    }

                    

            
                db.SaveChanges();

                    XtraMessageBox.Show("Stok işlemi başarılı bir şekilde kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Listele();
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
           

        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (int)gridView1.GetFocusedRowCellValue("TransactionID");

                if (id <= 0)
                {
                    XtraMessageBox.Show("Silinecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var db = new SatisDBEntities2())
                {
                    var transaction = db.InvetoryTransactionsTBL.Find(id);
                    if (transaction == null)
                    {
                        XtraMessageBox.Show("Silinecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    
                    db.InvetoryTransactionsTBL.Remove(transaction);
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

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (int)gridView1.GetFocusedRowCellValue("TransactionID");

                if (id <= 0)
                {
                    XtraMessageBox.Show("Güncellenecek kayıt seçilmedi veya geçersiz ID.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var db = new SatisDBEntities2())
                {
                    var deger = db.InvetoryTransactionsTBL.Find(id);
                    if (deger == null)
                    {
                        XtraMessageBox.Show("Güncellenecek kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    deger.ProductID = int.Parse(txtad.Text);
                    deger.TransactionDate = DateTime.Now; 
                    deger.Quantity = int.Parse(txtmik.Text);
                    deger.TransactionTyppe = txtistur.Text; 

                    db.SaveChanges();
                }

                Listele();

                XtraMessageBox.Show("Envanter işlemi başarılı bir şekilde güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            txtad.Text = gridView1.GetFocusedRowCellValue("ProductID").ToString();
            txtmik.Text = gridView1.GetFocusedRowCellValue("Quantity").ToString();
            txtistur.Text = gridView1.GetFocusedRowCellValue("TransactionTyppe")?.ToString() ?? string.Empty;
        }
    }
    }
