using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Drawing;
using DevExpress.XtraPrinting;
using System.Drawing.Printing;

namespace TakipProjesi.Formlar.Rapor
{
    public partial class Rapor1 : DevExpress.XtraReports.UI.XtraReport
    {
        public Rapor1()
        {
            InitializeComponent();
        }

        public void SetDataSource(DataTable dataTable,string rTitle)
        {
            this.DataSource = dataTable;
            this.DataMember = "Rapor1";
            SetupReport(dataTable,rTitle);
        }

        private void SetupReport(DataTable dataTable, string T)
        {
            this.PageWidth = 850; 
            this.PageHeight = 1100;
            this.Margins = new Margins(50, 50, 100, 100); 

            ReportHeaderBand reportHeader = this.Bands["ReportHeader"] as ReportHeaderBand;
            if (reportHeader == null)
            {
                reportHeader = new ReportHeaderBand();
                this.Bands.Add(reportHeader);
            }

            XRLabel reportHeaderLabel = new XRLabel
            {
                
                Text = T,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Navy,
                BoundsF = new RectangleF(0, 0, this.PageWidth - this.Margins.Left - this.Margins.Right, 30),
                TextAlignment = TextAlignment.MiddleCenter,
                BackColor = Color.LightGray
            };
            reportHeader.Controls.Add(reportHeaderLabel);


            PageHeaderBand pageHeader = this.Bands["PageHeader"] as PageHeaderBand;
            if (pageHeader == null)
            {
                pageHeader = new PageHeaderBand();
                this.Bands.Add(pageHeader);
            }

            XRPanel headerPanel = new XRPanel
            {
                BoundsF = new RectangleF(0, 0, this.PageWidth - this.Margins.Left - this.Margins.Right, 30),
                BackColor = Color.LightGray
            };
            pageHeader.Controls.Add(headerPanel);

            float xPosition = 0;
            float columnWidth = (this.PageWidth - this.Margins.Left - this.Margins.Right) / dataTable.Columns.Count;

            foreach (DataColumn column in dataTable.Columns)
            {
                XRLabel headerLabel = new XRLabel
                {
                    Text = column.ColumnName,
                    Font = new Font("Arial", 8, FontStyle.Bold),
                    BoundsF = new RectangleF(xPosition, 0, columnWidth, 30),
                    TextAlignment = TextAlignment.MiddleCenter,
                    BackColor = Color.LightGray,
                    Borders = BorderSide.None 
                };
                headerPanel.Controls.Add(headerLabel);
                xPosition += columnWidth;
            }

            GroupHeaderBand groupHeader = this.Bands["GroupHeader1"] as GroupHeaderBand;
            if (groupHeader == null)
            {
                groupHeader = new GroupHeaderBand();
                this.Bands.Add(groupHeader);
            }
            groupHeader.RepeatEveryPage = true; 

            DetailBand detailBand = this.Bands["Detail"] as DetailBand;
            if (detailBand == null)
            {
                detailBand = new DetailBand();
                this.Bands.Add(detailBand);
            }

            float yPosition = 0;
            float rowHeight = 25;
            detailBand.Controls.Clear();
            xPosition = 0;

            foreach (DataColumn column in dataTable.Columns)
            {
                XRLabel dataLabel = new XRLabel
                {
                    ExpressionBindings = { new ExpressionBinding("BeforePrint", "Text", $"[{column.ColumnName}]") },
                    BoundsF = new RectangleF(xPosition, yPosition, columnWidth, rowHeight),
                    TextAlignment = TextAlignment.MiddleLeft,
                    Borders = BorderSide.None
                };
                detailBand.Controls.Add(dataLabel);
                xPosition += columnWidth;

                
            }
        }
    }
}
