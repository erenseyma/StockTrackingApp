using DevExpress.DashboardCommon;
using DevExpress.DashboardWin;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
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

namespace TakipProjesi.Formlar
{
    public partial class DashboardUser : DevExpress.XtraEditors.XtraUserControl
    {
        public DashboardUser()
        {
            InitializeComponent();
        }

        private void DashboardUser_Load(object sender, EventArgs e)
        {
            var dashboard = new DevExpress.DashboardCommon.Dashboard();
            MsSqlConnectionParameters connectionParameters = new MsSqlConnectionParameters(
                "SEYMA\\SQLEXPRESS", "SatisDB", "sa", "Bm337991", MsSqlAuthorizationType.SqlServer);

            //    // SqlDataSource oluştur
            //    SqlDataSource sqlDataSource = new SqlDataSource(connectionParameters);

            //    // Özel SQL sorgusu oluştur
            //    CustomSqlQuery query = new CustomSqlQuery();
            //    query.Name = "customQuery";
            //    query.Sql = "SELECT * FROM ProductTBL"; // Gerçek SQL sorgunuzu buraya yazın

            //    // Sorguyu veri kaynağına ekle
            //    sqlDataSource.Queries.Add(query);
            //    sqlDataSource.Fill();

            //    // SqlDataSource'u DashboardSqlDataSource'a dönüştür
            //    DashboardSqlDataSource dashboardSqlDataSource = new DashboardSqlDataSource("SQL Data Source", sqlDataSource);

            //    // Veri kaynağını dashboard'a ekle
            //    dashboard.DataSources.Add(dashboardSqlDataSource);

            //    // Dashboard'u Dashboard Designer'a atayın
            //    dashboardDesigner1.Dashboard = dashboard;

            //    // Dashboard Designer'ı kullanıcı kontrolüne ekle
            //    this.Controls.Add(dashboardDesigner1);
            //    dashboardDesigner1.Dock = DockStyle.Fill;
        }
    }
}
