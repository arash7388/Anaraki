namespace MehranPack
{
    partial class ReportW
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportW));
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group2 = new Telerik.Reporting.Group();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule5 = new Telerik.Reporting.Drawing.StyleRule();
            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.reportNameTextBox = new Telerik.Reporting.TextBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.partNoCaptionTextBox = new Telerik.Reporting.TextBox();
            this.partNoDataTextBox = new Telerik.Reporting.TextBox();
            this.wIDCaptionTextBox = new Telerik.Reporting.TextBox();
            this.wIDDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.operatorNameCaptionTextBox = new Telerik.Reporting.TextBox();
            this.operatorNameDataTextBox = new Telerik.Reporting.TextBox();
            this.colorNameCaptionTextBox = new Telerik.Reporting.TextBox();
            this.colorNameDataTextBox = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.pCodeDataTextBox = new Telerik.Reporting.TextBox();
            this.pNameDataTextBox = new Telerik.Reporting.TextBox();
            this.groupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.groupFooterSection = new Telerik.Reporting.GroupFooterSection();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionString = "Anaraki";
            this.sqlDataSource1.Name = "sqlDataSource1";
            this.sqlDataSource1.SelectCommand = resources.GetString("sqlDataSource1.SelectCommand");
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.714D);
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.714D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = false;
            // 
            // pageHeader
            // 
            this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.714D);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.reportNameTextBox});
            this.pageHeader.Name = "pageHeader";
            // 
            // reportNameTextBox
            // 
            this.reportNameTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.053D), Telerik.Reporting.Drawing.Unit.Cm(0.053D));
            this.reportNameTextBox.Name = "reportNameTextBox";
            this.reportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.788D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.reportNameTextBox.StyleName = "PageInfo";
            this.reportNameTextBox.Value = "ReportW";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.714D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.currentTimeTextBox,
            this.pageInfoTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.053D), Telerik.Reporting.Drawing.Unit.Cm(0.053D));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.368D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.474D), Telerik.Reporting.Drawing.Unit.Cm(0.053D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.368D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.586D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox,
            this.partNoCaptionTextBox,
            this.partNoDataTextBox,
            this.wIDCaptionTextBox,
            this.wIDDataTextBox,
            this.textBox1,
            this.textBox2,
            this.operatorNameCaptionTextBox,
            this.operatorNameDataTextBox,
            this.colorNameCaptionTextBox,
            this.colorNameDataTextBox});
            this.reportHeader.Name = "reportHeader";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.894D), Telerik.Reporting.Drawing.Unit.Cm(0.986D));
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "ReportW";
            // 
            // partNoCaptionTextBox
            // 
            this.partNoCaptionTextBox.CanGrow = true;
            this.partNoCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.143D), Telerik.Reporting.Drawing.Unit.Cm(0.986D));
            this.partNoCaptionTextBox.Name = "partNoCaptionTextBox";
            this.partNoCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.631D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.partNoCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.partNoCaptionTextBox.StyleName = "Caption";
            this.partNoCaptionTextBox.Value = "Part No:";
            // 
            // partNoDataTextBox
            // 
            this.partNoDataTextBox.CanGrow = true;
            this.partNoDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.828D), Telerik.Reporting.Drawing.Unit.Cm(0.986D));
            this.partNoDataTextBox.Name = "partNoDataTextBox";
            this.partNoDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.631D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.partNoDataTextBox.StyleName = "Data";
            this.partNoDataTextBox.Value = "= Fields.PartNo";
            // 
            // wIDCaptionTextBox
            // 
            this.wIDCaptionTextBox.CanGrow = true;
            this.wIDCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.512D), Telerik.Reporting.Drawing.Unit.Cm(0.986D));
            this.wIDCaptionTextBox.Name = "wIDCaptionTextBox";
            this.wIDCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.631D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.wIDCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.wIDCaptionTextBox.StyleName = "Caption";
            this.wIDCaptionTextBox.Value = "WID:";
            // 
            // wIDDataTextBox
            // 
            this.wIDDataTextBox.CanGrow = true;
            this.wIDDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.196D), Telerik.Reporting.Drawing.Unit.Cm(0.986D));
            this.wIDDataTextBox.Name = "wIDDataTextBox";
            this.wIDDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.631D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.wIDDataTextBox.StyleName = "Data";
            this.wIDDataTextBox.Value = "= Fields.WID";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.88D), Telerik.Reporting.Drawing.Unit.Cm(0.986D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.631D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "Date Date:";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.564D), Telerik.Reporting.Drawing.Unit.Cm(0.986D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.631D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.textBox2.StyleName = "Data";
            this.textBox2.Value = "= Fields.Date.Date";
            // 
            // operatorNameCaptionTextBox
            // 
            this.operatorNameCaptionTextBox.CanGrow = true;
            this.operatorNameCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.248D), Telerik.Reporting.Drawing.Unit.Cm(0.986D));
            this.operatorNameCaptionTextBox.Name = "operatorNameCaptionTextBox";
            this.operatorNameCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.631D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.operatorNameCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.operatorNameCaptionTextBox.StyleName = "Caption";
            this.operatorNameCaptionTextBox.Value = "Operator Name:";
            // 
            // operatorNameDataTextBox
            // 
            this.operatorNameDataTextBox.CanGrow = true;
            this.operatorNameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.8D), Telerik.Reporting.Drawing.Unit.Cm(0.986D));
            this.operatorNameDataTextBox.Name = "operatorNameDataTextBox";
            this.operatorNameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.631D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.operatorNameDataTextBox.StyleName = "Data";
            this.operatorNameDataTextBox.Value = "= Fields.OperatorName";
            // 
            // colorNameCaptionTextBox
            // 
            this.colorNameCaptionTextBox.CanGrow = true;
            this.colorNameCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.484D), Telerik.Reporting.Drawing.Unit.Cm(0.986D));
            this.colorNameCaptionTextBox.Name = "colorNameCaptionTextBox";
            this.colorNameCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.631D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.colorNameCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.colorNameCaptionTextBox.StyleName = "Caption";
            this.colorNameCaptionTextBox.Value = "Color Name:";
            // 
            // colorNameDataTextBox
            // 
            this.colorNameDataTextBox.CanGrow = true;
            this.colorNameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.168D), Telerik.Reporting.Drawing.Unit.Cm(0.986D));
            this.colorNameDataTextBox.Name = "colorNameDataTextBox";
            this.colorNameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.631D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.colorNameDataTextBox.StyleName = "Data";
            this.colorNameDataTextBox.Value = "= Fields.ColorName";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.714D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pCodeDataTextBox,
            this.pNameDataTextBox});
            this.detail.Name = "detail";
            // 
            // pCodeDataTextBox
            // 
            this.pCodeDataTextBox.CanGrow = true;
            this.pCodeDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.053D), Telerik.Reporting.Drawing.Unit.Cm(0.053D));
            this.pCodeDataTextBox.Name = "pCodeDataTextBox";
            this.pCodeDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.368D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.pCodeDataTextBox.StyleName = "Data";
            this.pCodeDataTextBox.Value = "= Fields.PCode";
            // 
            // pNameDataTextBox
            // 
            this.pNameDataTextBox.CanGrow = true;
            this.pNameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.474D), Telerik.Reporting.Drawing.Unit.Cm(0.053D));
            this.pNameDataTextBox.Name = "pNameDataTextBox";
            this.pNameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.368D), Telerik.Reporting.Drawing.Unit.Cm(0.6D));
            this.pNameDataTextBox.StyleName = "Data";
            this.pNameDataTextBox.Value = "= Fields.PName";
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(2.5D);
            this.groupHeaderSection.Name = "groupHeaderSection";
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(2.5D);
            this.groupFooterSection.Name = "groupFooterSection";
            // 
            // ReportW
            // 
            this.DataSource = this.sqlDataSource1;
            group1.GroupFooter = this.labelsGroupFooterSection;
            group1.GroupHeader = this.labelsGroupHeaderSection;
            group1.Name = "labelsGroup";
            group2.GroupFooter = this.groupFooterSection;
            group2.GroupHeader = this.groupHeaderSection;
            group2.Groupings.Add(new Telerik.Reporting.Grouping("[=Fields.PName]"));
            group2.Name = "group";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.labelsGroupHeaderSection,
            this.labelsGroupFooterSection,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.detail,
            this.groupHeaderSection,
            this.groupFooterSection});
            this.Name = "ReportW";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule2.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(238)))), ((int)(((byte)(232)))));
            styleRule2.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            styleRule2.Style.Font.Name = "Verdana";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(18D);
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule3.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            styleRule3.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(222)))), ((int)(((byte)(209)))));
            styleRule3.Style.Font.Name = "Verdana";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule4.Style.Font.Name = "Verdana";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule5.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule5.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            styleRule5.Style.Font.Name = "Verdana";
            styleRule5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            styleRule5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4,
            styleRule5});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(16.894D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource sqlDataSource1;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.TextBox reportNameTextBox;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.TextBox partNoCaptionTextBox;
        private Telerik.Reporting.TextBox partNoDataTextBox;
        private Telerik.Reporting.TextBox wIDCaptionTextBox;
        private Telerik.Reporting.TextBox wIDDataTextBox;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox operatorNameCaptionTextBox;
        private Telerik.Reporting.TextBox operatorNameDataTextBox;
        private Telerik.Reporting.TextBox colorNameCaptionTextBox;
        private Telerik.Reporting.TextBox colorNameDataTextBox;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox pCodeDataTextBox;
        private Telerik.Reporting.TextBox pNameDataTextBox;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
    }
}