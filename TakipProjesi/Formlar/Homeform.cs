using DevExpress.DashboardWin.Native;
using DevExpress.XtraEditors;
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
    public partial class Homeform : DevExpress.XtraEditors.XtraUserControl
    {
        public Homeform()
        {
            InitializeComponent();
        }

        SatisDBEntities2 db=new SatisDBEntities2();
        private void Homeform_Load(object sender, EventArgs e)
        {


            Dashboard1 dashboard = new Dashboard1();
            dashboardViewer1.Dashboard = dashboard;
        }
    }
}
