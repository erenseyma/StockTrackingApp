using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TakipProjesi.Entity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TakipProjesi.Formlar
{
    public partial class LoginForm : DevExpress.XtraEditors.XtraForm
    {
        public LoginForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(simpleButton1_KeyDown);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string email = textEdit1.Text;
            string password = textEdit2.Text;

            if (AuthenticateUser(email, password))
            {
                labelControl4.ForeColor = System.Drawing.Color.Green;
                labelControl4.Text = "Giriş başarılı.";
               
                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                labelControl4.Text = "Email veya şifre yanlış.";
            }
        }

        private bool AuthenticateUser(string email, string password)
        {

            string passwordHash = ComputeSha256Hash(password);

            try
            {
                using (var context = new SatisDBEntities2())
                {
                    return context.Users.Any(u => u.Email == email && u.PasswordHash == passwordHash);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return false;
            }



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

        private void simpleButton1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton1.PerformClick();
                e.Handled = true;
            }
        }
    }
}