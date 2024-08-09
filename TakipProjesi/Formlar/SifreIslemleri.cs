using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TakipProjesi.Entity;

namespace TakipProjesi.Formlar
{
    public partial class SifreIslemleri : DevExpress.XtraEditors.XtraUserControl
    {
        private MainForm _mainform;
        public SifreIslemleri(MainForm mainform)
        {
            InitializeComponent();
            _mainform=mainform;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string oldPassword = textEdit2.Text;
            string newPassword = textEdit3.Text;
            string confirmPassword = textEdit4.Text;

            if (newPassword != confirmPassword)
            {
                labelControl4.Text = "Yeni şifreler eşleşmiyor.";
                return;
            }

            bool isChanged = ChangePassword(oldPassword, newPassword);
            if (isChanged)
            {
                labelControl4.ForeColor = System.Drawing.Color.Green;
                labelControl4.Text = "Şifre başarıyla değiştirildi.";
            }
            else
            {
                ChangePassword(oldPassword, newPassword);
                labelControl4.Text = "Eski şifre yanlış veya şifre değiştirme başarısız oldu.";
            }
        }

        private bool ChangePassword(string oldPassword, string newPassword)
        {
            int userId = 3; 

            string oldPasswordHash = ComputeSha256Hash(oldPassword);
            string newPasswordHash = ComputeSha256Hash(newPassword);

            try
            {
                using (var context = new SatisDBEntities2())
                {
                    var user = context.Users.SingleOrDefault(u => u.UserID == userId && u.PasswordHash == oldPasswordHash);
                    if (user == null)
                    {
                        return false; 
                    }

                    user.PasswordHash =newPasswordHash;
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return false;
            }
        }
        private void textEdit3_EditValueChanged(object sender, EventArgs e)
        {

        }
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void SifreIslemleri_Load(object sender, EventArgs e)
        {

        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int id = 1;
            string ad = textEdit1.Text;
            string soyad = textEdit6.Text;
            string numara = textEdit5.Text;

            using (var context = new SatisDBEntities2())
            {

                var existingUser = context.UserInfo.SingleOrDefault(u => u.UserID == id);

                //if (existingUser != null)
                //{
                    existingUser.UserName = ad;
                    existingUser.UserLast = soyad;
                    existingUser.Phone = numara;

                    context.SaveChanges();
                    _mainform.UpdateStatusBar(ad, soyad);
                    MessageBox.Show("Kişi başarıyla güncellendi.");
                //}
            }




        }
        

    }
}
