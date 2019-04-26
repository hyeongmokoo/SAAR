using System;

namespace VisUncertainty
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            //Ensures that any ESRI libraries that have been used are unloaded in the correct order. 
            //Failure to do this may result in random crashes on exit due to the operating system unloading 
            //the libraries in the incorrect order. 
            ESRI.ArcGIS.ADF.COMSupport.AOUninitialize.Shutdown();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.programPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutSAARToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFieldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fieldCalculatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataTransformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmHistogram = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmBoxplot = new System.Windows.Forms.ToolStripMenuItem();
            this.violinPlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmQQplot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmScatterplot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.moranScatterplotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.conditionedMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spatialWeightsMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stmenuSample = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDescriptiveStatistics = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.spatialAutocorrelationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localSpatialAutocorrelationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.reducedSecondMomentMeasureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.variogramCloudToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clustogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spatialCorrelogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spatialCorrelogramLocalVerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.globalBivariateSpatialAssociationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bivariateSpatialAssociationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regressionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linearRegressionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generalizedLinearModelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spatialRegressionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.eigenvectorSpatialFilteringToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.spatiallyVaryingCoefficientsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncertaintyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.proportionalSymbolWithSaturateAndTransparencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.choroplethMapWithGraduatedColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graduatedSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dynamicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gliderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blinkingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classSeparabilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classificationOptimizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.theRoubustnessOfChoroplethMapClassificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteSensingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.parallelCoordinatePlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clusteringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hierarchicalClusteringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createSpatialWeightMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createFlowLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mLClassificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLayoutViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leesToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bivariateLISAMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bivariateSpatialQuadrantMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bivariateSpatialProbabilityMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bivariateSpatialSignificanceMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.bivariateSpatialRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bivariateSpatialClusterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bivariateSpatialOutlierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.globalBivariateSpatialAssociationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tempToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geocodingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncernSAMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusBarXY = new System.Windows.Forms.ToolStripStatusLabel();
            this.cdColor = new System.Windows.Forms.ColorDialog();
            this.ofdAddData = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.dataToolStripMenuItem,
            this.graphsToolStripMenuItem,
            this.stmenuSample,
            this.regressionToolStripMenuItem,
            this.uncertaintyToolStripMenuItem,
            this.ToolStripMenuItem,
            this.printToolStripMenuItem,
            this.leesToolsToolStripMenuItem,
            this.tempToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(859, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "Lee\'s tools";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewDoc,
            this.menuOpenDoc,
            this.menuSaveDoc,
            this.menuSaveAs,
            this.menuSeparator,
            this.programPropertiesToolStripMenuItem,
            this.aboutSAARToolStripMenuItem,
            this.menuExitApp});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "File";
            // 
            // menuNewDoc
            // 
            this.menuNewDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuNewDoc.Image")));
            this.menuNewDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuNewDoc.Name = "menuNewDoc";
            this.menuNewDoc.Size = new System.Drawing.Size(177, 22);
            this.menuNewDoc.Text = "New Document";
            this.menuNewDoc.Click += new System.EventHandler(this.menuNewDoc_Click);
            // 
            // menuOpenDoc
            // 
            this.menuOpenDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuOpenDoc.Image")));
            this.menuOpenDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuOpenDoc.Name = "menuOpenDoc";
            this.menuOpenDoc.Size = new System.Drawing.Size(177, 22);
            this.menuOpenDoc.Text = "Open Document...";
            this.menuOpenDoc.Click += new System.EventHandler(this.menuOpenDoc_Click);
            // 
            // menuSaveDoc
            // 
            this.menuSaveDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuSaveDoc.Image")));
            this.menuSaveDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuSaveDoc.Name = "menuSaveDoc";
            this.menuSaveDoc.Size = new System.Drawing.Size(177, 22);
            this.menuSaveDoc.Text = "SaveDocument";
            this.menuSaveDoc.Click += new System.EventHandler(this.menuSaveDoc_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(177, 22);
            this.menuSaveAs.Text = "Save As...";
            this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
            // 
            // menuSeparator
            // 
            this.menuSeparator.Name = "menuSeparator";
            this.menuSeparator.Size = new System.Drawing.Size(174, 6);
            // 
            // programPropertiesToolStripMenuItem
            // 
            this.programPropertiesToolStripMenuItem.Name = "programPropertiesToolStripMenuItem";
            this.programPropertiesToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.programPropertiesToolStripMenuItem.Text = "Program Properties";
            this.programPropertiesToolStripMenuItem.Click += new System.EventHandler(this.programPropertiesToolStripMenuItem_Click);
            // 
            // aboutSAARToolStripMenuItem
            // 
            this.aboutSAARToolStripMenuItem.Name = "aboutSAARToolStripMenuItem";
            this.aboutSAARToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.aboutSAARToolStripMenuItem.Text = "About SAAR";
            this.aboutSAARToolStripMenuItem.Click += new System.EventHandler(this.aboutSAARToolStripMenuItem_Click);
            // 
            // menuExitApp
            // 
            this.menuExitApp.Name = "menuExitApp";
            this.menuExitApp.Size = new System.Drawing.Size(177, 22);
            this.menuExitApp.Text = "Exit";
            this.menuExitApp.Click += new System.EventHandler(this.menuExitApp_Click);
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFieldToolStripMenuItem,
            this.deleteFieldsToolStripMenuItem,
            this.fieldCalculatorToolStripMenuItem,
            this.dataTransformationToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // addFieldToolStripMenuItem
            // 
            this.addFieldToolStripMenuItem.Name = "addFieldToolStripMenuItem";
            this.addFieldToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.addFieldToolStripMenuItem.Text = "Add Field";
            this.addFieldToolStripMenuItem.Click += new System.EventHandler(this.addFieldToolStripMenuItem_Click);
            // 
            // deleteFieldsToolStripMenuItem
            // 
            this.deleteFieldsToolStripMenuItem.Name = "deleteFieldsToolStripMenuItem";
            this.deleteFieldsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.deleteFieldsToolStripMenuItem.Text = "Delete Fields";
            this.deleteFieldsToolStripMenuItem.Click += new System.EventHandler(this.deleteFieldsToolStripMenuItem_Click);
            // 
            // fieldCalculatorToolStripMenuItem
            // 
            this.fieldCalculatorToolStripMenuItem.Name = "fieldCalculatorToolStripMenuItem";
            this.fieldCalculatorToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.fieldCalculatorToolStripMenuItem.Text = "Field Calculator";
            this.fieldCalculatorToolStripMenuItem.Click += new System.EventHandler(this.fieldCalculatorToolStripMenuItem_Click);
            // 
            // dataTransformationToolStripMenuItem
            // 
            this.dataTransformationToolStripMenuItem.Name = "dataTransformationToolStripMenuItem";
            this.dataTransformationToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.dataTransformationToolStripMenuItem.Text = "Data Transformation";
            this.dataTransformationToolStripMenuItem.Click += new System.EventHandler(this.dataTransformationToolStripMenuItem_Click);
            // 
            // graphsToolStripMenuItem
            // 
            this.graphsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmHistogram,
            this.tsmBoxplot,
            this.violinPlotToolStripMenuItem,
            this.tsmQQplot,
            this.toolStripSeparator3,
            this.tsmScatterplot,
            this.toolStripSeparator4,
            this.moranScatterplotToolStripMenuItem,
            this.conditionedMapToolStripMenuItem,
            this.spatialWeightsMatrixToolStripMenuItem});
            this.graphsToolStripMenuItem.Name = "graphsToolStripMenuItem";
            this.graphsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.graphsToolStripMenuItem.Text = "Explore";
            // 
            // tsmHistogram
            // 
            this.tsmHistogram.Name = "tsmHistogram";
            this.tsmHistogram.Size = new System.Drawing.Size(237, 22);
            this.tsmHistogram.Text = "Histogram";
            this.tsmHistogram.Click += new System.EventHandler(this.tsmHistogram_Click);
            // 
            // tsmBoxplot
            // 
            this.tsmBoxplot.Name = "tsmBoxplot";
            this.tsmBoxplot.Size = new System.Drawing.Size(237, 22);
            this.tsmBoxplot.Text = "Boxplot";
            this.tsmBoxplot.Click += new System.EventHandler(this.tsmBoxplot_Click);
            // 
            // violinPlotToolStripMenuItem
            // 
            this.violinPlotToolStripMenuItem.Name = "violinPlotToolStripMenuItem";
            this.violinPlotToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.violinPlotToolStripMenuItem.Text = "Violin plot";
            this.violinPlotToolStripMenuItem.Click += new System.EventHandler(this.violinPlotToolStripMenuItem_Click);
            // 
            // tsmQQplot
            // 
            this.tsmQQplot.Name = "tsmQQplot";
            this.tsmQQplot.Size = new System.Drawing.Size(237, 22);
            this.tsmQQplot.Text = "Quantile-comparison plot";
            this.tsmQQplot.Click += new System.EventHandler(this.tsmQQplot_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(234, 6);
            // 
            // tsmScatterplot
            // 
            this.tsmScatterplot.Name = "tsmScatterplot";
            this.tsmScatterplot.Size = new System.Drawing.Size(237, 22);
            this.tsmScatterplot.Text = "Scatterplot";
            this.tsmScatterplot.Click += new System.EventHandler(this.tsmScatterplot_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(234, 6);
            // 
            // moranScatterplotToolStripMenuItem
            // 
            this.moranScatterplotToolStripMenuItem.Name = "moranScatterplotToolStripMenuItem";
            this.moranScatterplotToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.moranScatterplotToolStripMenuItem.Text = "Moran Scatterplot";
            this.moranScatterplotToolStripMenuItem.Click += new System.EventHandler(this.moranScatterplotToolStripMenuItem_Click);
            // 
            // conditionedMapToolStripMenuItem
            // 
            this.conditionedMapToolStripMenuItem.Name = "conditionedMapToolStripMenuItem";
            this.conditionedMapToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.conditionedMapToolStripMenuItem.Text = "Conditioned Choropleth Maps";
            this.conditionedMapToolStripMenuItem.Click += new System.EventHandler(this.conditionedMapToolStripMenuItem_Click);
            // 
            // spatialWeightsMatrixToolStripMenuItem
            // 
            this.spatialWeightsMatrixToolStripMenuItem.Name = "spatialWeightsMatrixToolStripMenuItem";
            this.spatialWeightsMatrixToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.spatialWeightsMatrixToolStripMenuItem.Text = "Spatial Composition";
            this.spatialWeightsMatrixToolStripMenuItem.Click += new System.EventHandler(this.spatialWeightsMatrixToolStripMenuItem_Click);
            // 
            // stmenuSample
            // 
            this.stmenuSample.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmDescriptiveStatistics,
            this.toolStripSeparator1,
            this.spatialAutocorrelationToolStripMenuItem,
            this.localSpatialAutocorrelationToolStripMenuItem,
            this.toolStripSeparator2,
            this.reducedSecondMomentMeasureToolStripMenuItem,
            this.variogramCloudToolStripMenuItem,
            this.clustogramToolStripMenuItem,
            this.spatialCorrelogramToolStripMenuItem,
            this.spatialCorrelogramLocalVerToolStripMenuItem,
            this.toolStripSeparator6,
            this.globalBivariateSpatialAssociationToolStripMenuItem,
            this.bivariateSpatialAssociationToolStripMenuItem});
            this.stmenuSample.Name = "stmenuSample";
            this.stmenuSample.Size = new System.Drawing.Size(62, 20);
            this.stmenuSample.Text = "Analysis";
            // 
            // tsmDescriptiveStatistics
            // 
            this.tsmDescriptiveStatistics.Name = "tsmDescriptiveStatistics";
            this.tsmDescriptiveStatistics.Size = new System.Drawing.Size(294, 22);
            this.tsmDescriptiveStatistics.Text = "Descriptive Statistics";
            this.tsmDescriptiveStatistics.Click += new System.EventHandler(this.tsmDescriptiveStatistics_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(291, 6);
            // 
            // spatialAutocorrelationToolStripMenuItem
            // 
            this.spatialAutocorrelationToolStripMenuItem.Name = "spatialAutocorrelationToolStripMenuItem";
            this.spatialAutocorrelationToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.spatialAutocorrelationToolStripMenuItem.Text = "Global Spatial Autocorrelation";
            this.spatialAutocorrelationToolStripMenuItem.Click += new System.EventHandler(this.spatialAutocorrelationToolStripMenuItem_Click);
            // 
            // localSpatialAutocorrelationToolStripMenuItem
            // 
            this.localSpatialAutocorrelationToolStripMenuItem.Name = "localSpatialAutocorrelationToolStripMenuItem";
            this.localSpatialAutocorrelationToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.localSpatialAutocorrelationToolStripMenuItem.Text = "Local Spatial Autocorrelation (Univariate)";
            this.localSpatialAutocorrelationToolStripMenuItem.Click += new System.EventHandler(this.localSpatialAutocorrelationToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(291, 6);
            // 
            // reducedSecondMomentMeasureToolStripMenuItem
            // 
            this.reducedSecondMomentMeasureToolStripMenuItem.Name = "reducedSecondMomentMeasureToolStripMenuItem";
            this.reducedSecondMomentMeasureToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.reducedSecondMomentMeasureToolStripMenuItem.Text = "Reduced Second Moment Measure";
            this.reducedSecondMomentMeasureToolStripMenuItem.Visible = false;
            this.reducedSecondMomentMeasureToolStripMenuItem.Click += new System.EventHandler(this.reducedSecondMomentMeasureToolStripMenuItem_Click);
            // 
            // variogramCloudToolStripMenuItem
            // 
            this.variogramCloudToolStripMenuItem.Name = "variogramCloudToolStripMenuItem";
            this.variogramCloudToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.variogramCloudToolStripMenuItem.Text = "Variogram Cloud";
            this.variogramCloudToolStripMenuItem.Click += new System.EventHandler(this.variogramCloudToolStripMenuItem_Click);
            // 
            // clustogramToolStripMenuItem
            // 
            this.clustogramToolStripMenuItem.Name = "clustogramToolStripMenuItem";
            this.clustogramToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.clustogramToolStripMenuItem.Text = "Co/Clustogram";
            this.clustogramToolStripMenuItem.Visible = false;
            this.clustogramToolStripMenuItem.Click += new System.EventHandler(this.clustogramToolStripMenuItem_Click);
            // 
            // spatialCorrelogramToolStripMenuItem
            // 
            this.spatialCorrelogramToolStripMenuItem.Name = "spatialCorrelogramToolStripMenuItem";
            this.spatialCorrelogramToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.spatialCorrelogramToolStripMenuItem.Text = "Spatial Correlogram (Global)";
            this.spatialCorrelogramToolStripMenuItem.Visible = false;
            this.spatialCorrelogramToolStripMenuItem.Click += new System.EventHandler(this.spatialCorrelogramToolStripMenuItem_Click);
            // 
            // spatialCorrelogramLocalVerToolStripMenuItem
            // 
            this.spatialCorrelogramLocalVerToolStripMenuItem.Name = "spatialCorrelogramLocalVerToolStripMenuItem";
            this.spatialCorrelogramLocalVerToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.spatialCorrelogramLocalVerToolStripMenuItem.Text = "Spatial Correlogram";
            this.spatialCorrelogramLocalVerToolStripMenuItem.Click += new System.EventHandler(this.spatialCorrelogramLocalVerToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(291, 6);
            // 
            // globalBivariateSpatialAssociationToolStripMenuItem
            // 
            this.globalBivariateSpatialAssociationToolStripMenuItem.Name = "globalBivariateSpatialAssociationToolStripMenuItem";
            this.globalBivariateSpatialAssociationToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.globalBivariateSpatialAssociationToolStripMenuItem.Text = "Global Bivariate Spatial Association";
            this.globalBivariateSpatialAssociationToolStripMenuItem.Click += new System.EventHandler(this.globalBivariateSpatialAssociationToolStripMenuItem_Click);
            // 
            // bivariateSpatialAssociationToolStripMenuItem
            // 
            this.bivariateSpatialAssociationToolStripMenuItem.Name = "bivariateSpatialAssociationToolStripMenuItem";
            this.bivariateSpatialAssociationToolStripMenuItem.Size = new System.Drawing.Size(294, 22);
            this.bivariateSpatialAssociationToolStripMenuItem.Text = "Local Bivariate Spatial Association";
            this.bivariateSpatialAssociationToolStripMenuItem.Click += new System.EventHandler(this.bivariateSpatialAssociationToolStripMenuItem_Click);
            // 
            // regressionToolStripMenuItem
            // 
            this.regressionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.linearRegressionToolStripMenuItem,
            this.generalizedLinearModelsToolStripMenuItem,
            this.spatialRegressionToolStripMenuItem1,
            this.eigenvectorSpatialFilteringToolStripMenuItem1,
            this.spatiallyVaryingCoefficientsToolStripMenuItem});
            this.regressionToolStripMenuItem.Name = "regressionToolStripMenuItem";
            this.regressionToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.regressionToolStripMenuItem.Text = "Regression";
            // 
            // linearRegressionToolStripMenuItem
            // 
            this.linearRegressionToolStripMenuItem.Name = "linearRegressionToolStripMenuItem";
            this.linearRegressionToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.linearRegressionToolStripMenuItem.Text = "Linear Regression";
            this.linearRegressionToolStripMenuItem.Click += new System.EventHandler(this.linearRegressionToolStripMenuItem_Click);
            // 
            // generalizedLinearModelsToolStripMenuItem
            // 
            this.generalizedLinearModelsToolStripMenuItem.Name = "generalizedLinearModelsToolStripMenuItem";
            this.generalizedLinearModelsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.generalizedLinearModelsToolStripMenuItem.Text = "Generalized Linear Models";
            this.generalizedLinearModelsToolStripMenuItem.Click += new System.EventHandler(this.generalizedLinearModelsToolStripMenuItem_Click);
            // 
            // spatialRegressionToolStripMenuItem1
            // 
            this.spatialRegressionToolStripMenuItem1.Name = "spatialRegressionToolStripMenuItem1";
            this.spatialRegressionToolStripMenuItem1.Size = new System.Drawing.Size(231, 22);
            this.spatialRegressionToolStripMenuItem1.Text = "Spatial Autoregression";
            this.spatialRegressionToolStripMenuItem1.Click += new System.EventHandler(this.spatialRegressionToolStripMenuItem1_Click);
            // 
            // eigenvectorSpatialFilteringToolStripMenuItem1
            // 
            this.eigenvectorSpatialFilteringToolStripMenuItem1.Name = "eigenvectorSpatialFilteringToolStripMenuItem1";
            this.eigenvectorSpatialFilteringToolStripMenuItem1.Size = new System.Drawing.Size(231, 22);
            this.eigenvectorSpatialFilteringToolStripMenuItem1.Text = "Eigenvector Spatial Filtering";
            this.eigenvectorSpatialFilteringToolStripMenuItem1.Click += new System.EventHandler(this.eigenvectorSpatialFilteringToolStripMenuItem1_Click);
            // 
            // spatiallyVaryingCoefficientsToolStripMenuItem
            // 
            this.spatiallyVaryingCoefficientsToolStripMenuItem.Name = "spatiallyVaryingCoefficientsToolStripMenuItem";
            this.spatiallyVaryingCoefficientsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.spatiallyVaryingCoefficientsToolStripMenuItem.Text = "Spatially Varying Coefficients";
            this.spatiallyVaryingCoefficientsToolStripMenuItem.Click += new System.EventHandler(this.spatiallyVaryingCoefficientsToolStripMenuItem_Click);
            // 
            // uncertaintyToolStripMenuItem
            // 
            this.uncertaintyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.visualizationToolStripMenuItem,
            this.dynamicToolStripMenuItem,
            this.classificationToolStripMenuItem,
            this.remoteSensingToolStripMenuItem,
            this.clusteringToolStripMenuItem});
            this.uncertaintyToolStripMenuItem.Name = "uncertaintyToolStripMenuItem";
            this.uncertaintyToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.uncertaintyToolStripMenuItem.Text = "Uncertainty";
            // 
            // visualizationToolStripMenuItem
            // 
            this.visualizationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.proportionalSymbolWithSaturateAndTransparencyToolStripMenuItem,
            this.choroplethMapWithGraduatedColorToolStripMenuItem,
            this.graduatedSymbolToolStripMenuItem});
            this.visualizationToolStripMenuItem.Name = "visualizationToolStripMenuItem";
            this.visualizationToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.visualizationToolStripMenuItem.Text = "Visualization (Static)";
            // 
            // proportionalSymbolWithSaturateAndTransparencyToolStripMenuItem
            // 
            this.proportionalSymbolWithSaturateAndTransparencyToolStripMenuItem.Name = "proportionalSymbolWithSaturateAndTransparencyToolStripMenuItem";
            this.proportionalSymbolWithSaturateAndTransparencyToolStripMenuItem.Size = new System.Drawing.Size(317, 22);
            this.proportionalSymbolWithSaturateAndTransparencyToolStripMenuItem.Text = "Proportional Symbol with Color combination";
            this.proportionalSymbolWithSaturateAndTransparencyToolStripMenuItem.Click += new System.EventHandler(this.proportionalSymbolWithSaturateAndTransparencyToolStripMenuItem_Click);
            // 
            // choroplethMapWithGraduatedColorToolStripMenuItem
            // 
            this.choroplethMapWithGraduatedColorToolStripMenuItem.Name = "choroplethMapWithGraduatedColorToolStripMenuItem";
            this.choroplethMapWithGraduatedColorToolStripMenuItem.Size = new System.Drawing.Size(317, 22);
            this.choroplethMapWithGraduatedColorToolStripMenuItem.Text = "Choropleth Map with Overlaid Symbol";
            this.choroplethMapWithGraduatedColorToolStripMenuItem.Click += new System.EventHandler(this.choroplethMapWithGraduatedColorToolStripMenuItem_Click);
            // 
            // graduatedSymbolToolStripMenuItem
            // 
            this.graduatedSymbolToolStripMenuItem.Name = "graduatedSymbolToolStripMenuItem";
            this.graduatedSymbolToolStripMenuItem.Size = new System.Drawing.Size(317, 22);
            this.graduatedSymbolToolStripMenuItem.Text = "Composite Symbols";
            this.graduatedSymbolToolStripMenuItem.Click += new System.EventHandler(this.graduatedSymbolToolStripMenuItem_Click);
            // 
            // dynamicToolStripMenuItem
            // 
            this.dynamicToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gliderToolStripMenuItem,
            this.blinkingToolStripMenuItem});
            this.dynamicToolStripMenuItem.Name = "dynamicToolStripMenuItem";
            this.dynamicToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.dynamicToolStripMenuItem.Text = "Dynamic Visualization";
            // 
            // gliderToolStripMenuItem
            // 
            this.gliderToolStripMenuItem.Name = "gliderToolStripMenuItem";
            this.gliderToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.gliderToolStripMenuItem.Text = "Slider";
            this.gliderToolStripMenuItem.Click += new System.EventHandler(this.gliderToolStripMenuItem_Click);
            // 
            // blinkingToolStripMenuItem
            // 
            this.blinkingToolStripMenuItem.Name = "blinkingToolStripMenuItem";
            this.blinkingToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.blinkingToolStripMenuItem.Text = "Blinking";
            this.blinkingToolStripMenuItem.Click += new System.EventHandler(this.blinkingToolStripMenuItem_Click);
            // 
            // classificationToolStripMenuItem
            // 
            this.classificationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.classSeparabilityToolStripMenuItem,
            this.classificationOptimizationToolStripMenuItem,
            this.theRoubustnessOfChoroplethMapClassificationToolStripMenuItem});
            this.classificationToolStripMenuItem.Name = "classificationToolStripMenuItem";
            this.classificationToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.classificationToolStripMenuItem.Text = "Classification";
            // 
            // classSeparabilityToolStripMenuItem
            // 
            this.classSeparabilityToolStripMenuItem.Name = "classSeparabilityToolStripMenuItem";
            this.classSeparabilityToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
            this.classSeparabilityToolStripMenuItem.Text = "Class Separability";
            this.classSeparabilityToolStripMenuItem.Click += new System.EventHandler(this.classSeparabilityToolStripMenuItem_Click);
            // 
            // classificationOptimizationToolStripMenuItem
            // 
            this.classificationOptimizationToolStripMenuItem.Name = "classificationOptimizationToolStripMenuItem";
            this.classificationOptimizationToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
            this.classificationOptimizationToolStripMenuItem.Text = "Optimized Classification with Uncertainty";
            this.classificationOptimizationToolStripMenuItem.Click += new System.EventHandler(this.classificationOptimizationToolStripMenuItem_Click);
            // 
            // theRoubustnessOfChoroplethMapClassificationToolStripMenuItem
            // 
            this.theRoubustnessOfChoroplethMapClassificationToolStripMenuItem.Name = "theRoubustnessOfChoroplethMapClassificationToolStripMenuItem";
            this.theRoubustnessOfChoroplethMapClassificationToolStripMenuItem.Size = new System.Drawing.Size(296, 22);
            this.theRoubustnessOfChoroplethMapClassificationToolStripMenuItem.Text = "Evaluation";
            this.theRoubustnessOfChoroplethMapClassificationToolStripMenuItem.Visible = false;
            this.theRoubustnessOfChoroplethMapClassificationToolStripMenuItem.Click += new System.EventHandler(this.theRoubustnessOfChoroplethMapClassificationToolStripMenuItem_Click);
            // 
            // remoteSensingToolStripMenuItem
            // 
            this.remoteSensingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.parallelCoordinatePlotToolStripMenuItem});
            this.remoteSensingToolStripMenuItem.Name = "remoteSensingToolStripMenuItem";
            this.remoteSensingToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.remoteSensingToolStripMenuItem.Text = "Remote Sensing";
            this.remoteSensingToolStripMenuItem.Visible = false;
            // 
            // parallelCoordinatePlotToolStripMenuItem
            // 
            this.parallelCoordinatePlotToolStripMenuItem.Name = "parallelCoordinatePlotToolStripMenuItem";
            this.parallelCoordinatePlotToolStripMenuItem.Size = new System.Drawing.Size(320, 22);
            this.parallelCoordinatePlotToolStripMenuItem.Text = "Uncertainty Visualization in Classified RS data";
            this.parallelCoordinatePlotToolStripMenuItem.Click += new System.EventHandler(this.parallelCoordinatePlotToolStripMenuItem_Click);
            // 
            // clusteringToolStripMenuItem
            // 
            this.clusteringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hierarchicalClusteringToolStripMenuItem});
            this.clusteringToolStripMenuItem.Name = "clusteringToolStripMenuItem";
            this.clusteringToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.clusteringToolStripMenuItem.Text = "Clustering";
            this.clusteringToolStripMenuItem.Visible = false;
            // 
            // hierarchicalClusteringToolStripMenuItem
            // 
            this.hierarchicalClusteringToolStripMenuItem.Name = "hierarchicalClusteringToolStripMenuItem";
            this.hierarchicalClusteringToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.hierarchicalClusteringToolStripMenuItem.Text = "Hierarchical Clustering";
            this.hierarchicalClusteringToolStripMenuItem.Click += new System.EventHandler(this.hierarchicalClusteringToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem
            // 
            this.ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createSpatialWeightMatrixToolStripMenuItem,
            this.createFlowLinesToolStripMenuItem,
            this.mLClassificationToolStripMenuItem});
            this.ToolStripMenuItem.Name = "ToolStripMenuItem";
            this.ToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.ToolStripMenuItem.Text = "Tools";
            // 
            // createSpatialWeightMatrixToolStripMenuItem
            // 
            this.createSpatialWeightMatrixToolStripMenuItem.Name = "createSpatialWeightMatrixToolStripMenuItem";
            this.createSpatialWeightMatrixToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.createSpatialWeightMatrixToolStripMenuItem.Text = "Create Spatial Weights Matrix";
            this.createSpatialWeightMatrixToolStripMenuItem.Click += new System.EventHandler(this.createSpatialWeightMatrixToolStripMenuItem_Click);
            // 
            // createFlowLinesToolStripMenuItem
            // 
            this.createFlowLinesToolStripMenuItem.Name = "createFlowLinesToolStripMenuItem";
            this.createFlowLinesToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.createFlowLinesToolStripMenuItem.Text = "Create Flow Lines";
            this.createFlowLinesToolStripMenuItem.Click += new System.EventHandler(this.createFlowLinesToolStripMenuItem_Click);
            // 
            // mLClassificationToolStripMenuItem
            // 
            this.mLClassificationToolStripMenuItem.Name = "mLClassificationToolStripMenuItem";
            this.mLClassificationToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.mLClassificationToolStripMenuItem.Text = "ML Classification";
            this.mLClassificationToolStripMenuItem.Visible = false;
            this.mLClassificationToolStripMenuItem.Click += new System.EventHandler(this.mLClassificationToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openLayoutViewToolStripMenuItem});
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.printToolStripMenuItem.Text = "Layout";
            // 
            // openLayoutViewToolStripMenuItem
            // 
            this.openLayoutViewToolStripMenuItem.Name = "openLayoutViewToolStripMenuItem";
            this.openLayoutViewToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.openLayoutViewToolStripMenuItem.Text = "Open Layout View";
            this.openLayoutViewToolStripMenuItem.Click += new System.EventHandler(this.openLayoutViewToolStripMenuItem_Click);
            // 
            // leesToolsToolStripMenuItem
            // 
            this.leesToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bivariateLISAMapsToolStripMenuItem,
            this.bivariateSpatialQuadrantMapsToolStripMenuItem,
            this.bivariateSpatialProbabilityMapsToolStripMenuItem,
            this.bivariateSpatialSignificanceMapsToolStripMenuItem,
            this.toolStripSeparator5,
            this.bivariateSpatialRangeToolStripMenuItem,
            this.bivariateSpatialClusterToolStripMenuItem,
            this.bivariateSpatialOutlierToolStripMenuItem,
            this.toolStripSeparator7,
            this.globalBivariateSpatialAssociationToolStripMenuItem1});
            this.leesToolsToolStripMenuItem.Name = "leesToolsToolStripMenuItem";
            this.leesToolsToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.leesToolsToolStripMenuItem.Text = "LARRY";
            // 
            // bivariateLISAMapsToolStripMenuItem
            // 
            this.bivariateLISAMapsToolStripMenuItem.Name = "bivariateLISAMapsToolStripMenuItem";
            this.bivariateLISAMapsToolStripMenuItem.Size = new System.Drawing.Size(335, 22);
            this.bivariateLISAMapsToolStripMenuItem.Text = "Bivariate LISA Maps";
            this.bivariateLISAMapsToolStripMenuItem.Click += new System.EventHandler(this.bivariateLISAMapsToolStripMenuItem_Click);
            // 
            // bivariateSpatialQuadrantMapsToolStripMenuItem
            // 
            this.bivariateSpatialQuadrantMapsToolStripMenuItem.Name = "bivariateSpatialQuadrantMapsToolStripMenuItem";
            this.bivariateSpatialQuadrantMapsToolStripMenuItem.Size = new System.Drawing.Size(335, 22);
            this.bivariateSpatialQuadrantMapsToolStripMenuItem.Text = "Bivariate Spatial Quadrant Maps";
            this.bivariateSpatialQuadrantMapsToolStripMenuItem.Click += new System.EventHandler(this.bivariateSpatialQuadrantMapsToolStripMenuItem_Click);
            // 
            // bivariateSpatialProbabilityMapsToolStripMenuItem
            // 
            this.bivariateSpatialProbabilityMapsToolStripMenuItem.Name = "bivariateSpatialProbabilityMapsToolStripMenuItem";
            this.bivariateSpatialProbabilityMapsToolStripMenuItem.Size = new System.Drawing.Size(335, 22);
            this.bivariateSpatialProbabilityMapsToolStripMenuItem.Text = "Bivariate Spatial Probability Maps";
            this.bivariateSpatialProbabilityMapsToolStripMenuItem.Click += new System.EventHandler(this.bivariateSpatialProbabilityMapsToolStripMenuItem_Click);
            // 
            // bivariateSpatialSignificanceMapsToolStripMenuItem
            // 
            this.bivariateSpatialSignificanceMapsToolStripMenuItem.Name = "bivariateSpatialSignificanceMapsToolStripMenuItem";
            this.bivariateSpatialSignificanceMapsToolStripMenuItem.Size = new System.Drawing.Size(335, 22);
            this.bivariateSpatialSignificanceMapsToolStripMenuItem.Text = "Bivariate Spatial Significant Quadrant Maps";
            this.bivariateSpatialSignificanceMapsToolStripMenuItem.Click += new System.EventHandler(this.bivariateSpatialSignificanceMapsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(332, 6);
            // 
            // bivariateSpatialRangeToolStripMenuItem
            // 
            this.bivariateSpatialRangeToolStripMenuItem.Name = "bivariateSpatialRangeToolStripMenuItem";
            this.bivariateSpatialRangeToolStripMenuItem.Size = new System.Drawing.Size(335, 22);
            this.bivariateSpatialRangeToolStripMenuItem.Text = "Bivariate Spatial Range Maps";
            this.bivariateSpatialRangeToolStripMenuItem.Click += new System.EventHandler(this.bivariateSpatialRangeToolStripMenuItem_Click);
            // 
            // bivariateSpatialClusterToolStripMenuItem
            // 
            this.bivariateSpatialClusterToolStripMenuItem.Name = "bivariateSpatialClusterToolStripMenuItem";
            this.bivariateSpatialClusterToolStripMenuItem.Size = new System.Drawing.Size(335, 22);
            this.bivariateSpatialClusterToolStripMenuItem.Text = "Bivariate Spatial Cluster Maps";
            this.bivariateSpatialClusterToolStripMenuItem.Click += new System.EventHandler(this.bivariateSpatialClusterToolStripMenuItem_Click);
            // 
            // bivariateSpatialOutlierToolStripMenuItem
            // 
            this.bivariateSpatialOutlierToolStripMenuItem.Name = "bivariateSpatialOutlierToolStripMenuItem";
            this.bivariateSpatialOutlierToolStripMenuItem.Size = new System.Drawing.Size(335, 22);
            this.bivariateSpatialOutlierToolStripMenuItem.Text = "Bivariate Spatial Outlier Maps";
            this.bivariateSpatialOutlierToolStripMenuItem.Click += new System.EventHandler(this.bivariateSpatialOutlierToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(332, 6);
            // 
            // globalBivariateSpatialAssociationToolStripMenuItem1
            // 
            this.globalBivariateSpatialAssociationToolStripMenuItem1.Name = "globalBivariateSpatialAssociationToolStripMenuItem1";
            this.globalBivariateSpatialAssociationToolStripMenuItem1.Size = new System.Drawing.Size(335, 22);
            this.globalBivariateSpatialAssociationToolStripMenuItem1.Text = "Global Bivariate Spatial Autocorrelation Statistics";
            this.globalBivariateSpatialAssociationToolStripMenuItem1.Click += new System.EventHandler(this.globalBivariateSpatialAssociationToolStripMenuItem1_Click);
            // 
            // tempToolStripMenuItem
            // 
            this.tempToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.geocodingToolStripMenuItem,
            this.uncernSAMToolStripMenuItem});
            this.tempToolStripMenuItem.Name = "tempToolStripMenuItem";
            this.tempToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.tempToolStripMenuItem.Text = "Temp";
            this.tempToolStripMenuItem.Visible = false;
            // 
            // geocodingToolStripMenuItem
            // 
            this.geocodingToolStripMenuItem.Name = "geocodingToolStripMenuItem";
            this.geocodingToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.geocodingToolStripMenuItem.Text = "Geocoding";
            this.geocodingToolStripMenuItem.Click += new System.EventHandler(this.geocodingToolStripMenuItem_Click);
            // 
            // uncernSAMToolStripMenuItem
            // 
            this.uncernSAMToolStripMenuItem.Name = "uncernSAMToolStripMenuItem";
            this.uncernSAMToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.uncernSAMToolStripMenuItem.Text = "Uncern SAM";
            this.uncernSAMToolStripMenuItem.Click += new System.EventHandler(this.uncernSAMToolStripMenuItem_Click);
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(206, 52);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(653, 512);
            this.axMapControl1.TabIndex = 2;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnMouseUp += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseUpEventHandler(this.axMapControl1_OnMouseUp);
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);
            this.axMapControl1.OnMapReplaced += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMapReplacedEventHandler(this.axMapControl1_OnMapReplaced);
            this.axMapControl1.SizeChanged += new System.EventHandler(this.axMapControl1_SizeChanged);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 24);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(859, 28);
            this.axToolbarControl1.TabIndex = 3;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.axTOCControl1.Location = new System.Drawing.Point(3, 52);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(203, 512);
            this.axTOCControl1.TabIndex = 5;
            this.axTOCControl1.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl1_OnMouseDown);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(466, 278);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 5;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 52);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 534);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarXY});
            this.statusStrip1.Location = new System.Drawing.Point(3, 564);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(856, 22);
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusBar1";
            // 
            // statusBarXY
            // 
            this.statusBarXY.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.statusBarXY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusBarXY.Name = "statusBarXY";
            this.statusBarXY.Size = new System.Drawing.Size(53, 17);
            this.statusBarXY.Text = "Test 123";
            // 
            // ofdAddData
            // 
            this.ofdAddData.Filter = "Shapefile|*.shp";
            this.ofdAddData.Multiselect = true;
            this.ofdAddData.Title = "Add shapefile";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 586);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Untitled";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuNewDoc;
        private System.Windows.Forms.ToolStripMenuItem menuOpenDoc;
        private System.Windows.Forms.ToolStripMenuItem menuSaveDoc;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem menuExitApp;
        private System.Windows.Forms.ToolStripSeparator menuSeparator;
        public ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        public ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusBarXY;
        private System.Windows.Forms.ToolStripMenuItem stmenuSample;
        private System.Windows.Forms.ToolStripMenuItem tsmDescriptiveStatistics;
        private System.Windows.Forms.ToolStripMenuItem uncertaintyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmHistogram;
        private System.Windows.Forms.ToolStripMenuItem tsmBoxplot;
        private System.Windows.Forms.ToolStripMenuItem tsmScatterplot;
        private System.Windows.Forms.ToolStripMenuItem tsmQQplot;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createSpatialWeightMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visualizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graduatedSymbolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem proportionalSymbolWithSaturateAndTransparencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem choroplethMapWithGraduatedColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dynamicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blinkingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gliderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLayoutViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFieldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fieldCalculatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFieldsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem spatialAutocorrelationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem moranScatterplotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem classificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem classSeparabilityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localSpatialAutocorrelationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem theRoubustnessOfChoroplethMapClassificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem classificationOptimizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataTransformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reducedSecondMomentMeasureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem variogramCloudToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remoteSensingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parallelCoordinatePlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spatialCorrelogramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem violinPlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem programPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem conditionedMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFlowLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spatialCorrelogramLocalVerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mLClassificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bivariateSpatialAssociationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem globalBivariateSpatialAssociationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem clustogramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regressionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem linearRegressionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generalizedLinearModelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spatialRegressionToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem eigenvectorSpatialFilteringToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem leesToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bivariateSpatialClusterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bivariateSpatialOutlierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bivariateSpatialRangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem globalBivariateSpatialAssociationToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem bivariateLISAMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem bivariateSpatialQuadrantMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bivariateSpatialProbabilityMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bivariateSpatialSignificanceMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem tempToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem geocodingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncernSAMToolStripMenuItem;
        private System.Windows.Forms.ColorDialog cdColor;
        public System.Windows.Forms.OpenFileDialog ofdAddData;
        private System.Windows.Forms.ToolStripMenuItem spatialWeightsMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clusteringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hierarchicalClusteringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spatiallyVaryingCoefficientsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutSAARToolStripMenuItem;
    }
}

