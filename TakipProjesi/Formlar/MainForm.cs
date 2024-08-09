using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Tab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakipProjesi.Formlar
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public MainForm()
        {
            InitializeComponent();
            OpenUpdateForm();
        }

        private void BtnUrun_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.UrunlerUser urunler = new Formlar.UrunlerUser();
            //panelControl1.Controls.Clear();
            //urunler.Dock = DockStyle.Fill;
            //panelControl1.Controls.Add(urunler);

            TabPage tabPage = new TabPage("Ürünler");

            urunler.Dock = DockStyle.Fill;
            tabPage.Controls.Add(urunler);

            
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
            //Formlar.UrunlerUser urun =new Formlar.UrunlerUser();
            //urun.Show();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.MusterilerUser musteriler = new Formlar.MusterilerUser();
            TabPage tabPage = new TabPage("Müşteriler");

            musteriler.Dock = DockStyle.Fill;
            tabPage.Controls.Add(musteriler);

            //tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.SiparislerUser siparisler =new Formlar.SiparislerUser();
            TabPage tabPage = new TabPage("Siparişler");

            siparisler.Dock = DockStyle.Fill;
            tabPage.Controls.Add(siparisler);

            //tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.SatislarUser satislar=new Formlar.SatislarUser();
            TabPage tabPage = new TabPage("Satışlar");

            satislar.Dock = DockStyle.Fill;
            tabPage.Controls.Add(satislar);

            
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.KampanyalarUser kampanyalar =new Formlar.KampanyalarUser();
            TabPage tabPage = new TabPage("Kampanyalar");

            kampanyalar.Dock = DockStyle.Fill;
            tabPage.Controls.Add(kampanyalar);

            
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.SatisTemsilcileriUser satisTemsilcileri =new Formlar.SatisTemsilcileriUser();
            TabPage tabPage = new TabPage("Satış Temsilcileri");

            satisTemsilcileri.Dock = DockStyle.Fill;
            tabPage.Controls.Add(satisTemsilcileri);

            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {

            Formlar.StokHareketleriUser st=new StokHareketleriUser();
            TabPage tabPage = new TabPage("Stok Hareketleri");

            st.Dock = DockStyle.Fill;
            tabPage.Controls.Add(st);

            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            tabControl1.TabPages.Clear();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            tabControl1.TabPages.Clear();
            Formlar.DashboardUser dash =new Formlar.DashboardUser();
            TabPage tabPage = new TabPage("Dashboard");

            dash.Dock = DockStyle.Fill;
            tabPage.Controls.Add(dash);

            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;

        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            tabControl1.TabPages.Clear();
            Formlar.RaporlamaUser r=new Formlar.RaporlamaUser();

            TabPage tabPage = new TabPage("Rapor Sihirbazı");

            r.Dock = DockStyle.Fill;
            tabPage.Controls.Add(r);

            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
        }
        
        private void BtnHome_ItemClick(object sender, ItemClickEventArgs e)
        {
            tabControl1.TabPages.Clear();
            Formlar.Homeform frm1=new Formlar.Homeform();
            TabPage tabPage = new TabPage("Hızlı Ekran");
            
                frm1.Dock = DockStyle.Fill;
                tabPage.Controls.Add(frm1);
                tabControl1.TabPages.Add(tabPage);
                tabControl1.SelectedTab = tabPage;
            
        }
        
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.SifreIslemleri frm1=new Formlar.SifreIslemleri(this);
            TabPage tabPage = new TabPage("Password");
            frm1.Dock = DockStyle.Fill;
            tabPage.Controls.Add(frm1);
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;


        }
            private void OpenUpdateForm()
            {
                SifreIslemleri updateForm = new SifreIslemleri(this);
                updateForm.Show();
            }
        public void UpdateStatusBar(string ad, string soyad)
        {
            this.barStaticItem4.Caption = "Ad: " + ad;
            barStaticItem6.Caption = "Soyad: " + soyad;
        }


        
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Formlar.Homeform frm1 = new Formlar.Homeform();
            TabPage tabPage = new TabPage("Hızlı Ekran");

            frm1.Dock = DockStyle.Fill;
            tabPage.Controls.Add(frm1);
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectedTab = tabPage;
            barStaticItem4.Caption = "Kullanıcı:    " + "Şeyma";
            barStaticItem6.Caption = "Eren";
        }
    }
}