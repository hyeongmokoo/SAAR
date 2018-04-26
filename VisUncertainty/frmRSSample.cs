using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using RDotNet;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms.DataVisualization.Charting;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.esriSystem;


namespace VisUncertainty
{
    public partial class frmRSSample : Form
    {
        private MainForm m_pForm;
        private REngine m_pEngine;
        private int m_intColumnCnt;

        private int m_intTotalNSeries;

        IRasterProps m_pRasterProps;

        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        //Check These variables later!
        private List<List<int>> m_lstPtsIdContainer;
        private List<int> m_lstPtSeriesID;
        private List<int[]> m_lstIDsValues;

        private ClassificationResults[,] m_arrClsResults;
        private double[] m_Entropies;
        private double[] m_laggedEntropies;

        #region Variables for Drawing Rectangle
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;
        #endregion

        public frmRSSample()
        {
            InitializeComponent();

            m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            m_pEngine = m_pForm.pEngine;
            m_pEngine.Evaluate("library(e1071)");

            m_pActiveView = m_pForm.axMapControl1.ActiveView;
            m_pSnippet = new clsSnippet();
            string strStartPath = m_pForm.strPath;
            txtInputRaster.Text = strStartPath + @"\SampleData\RS\Denton\DentonL7.img";
            txtInputSigFile.Text = strStartPath + @"\SampleData\RS\Denton\sig_l7.gsg";
        }

        #region matrix algebra

        static double[][] MatrixInverse(double[][] matrix)
        {
            int n = matrix.Length;
            double[][] result = MatrixDuplicate(matrix);

            int[] perm;
            int toggle;
            double[][] lum = MatrixDecompose(matrix, out perm,
              out toggle);
            if (lum == null)
                throw new Exception("Unable to compute inverse");

            double[] b = new double[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0;
                    else
                        b[j] = 0.0;
                }

                double[] x = HelperSolve(lum, b); // 

                for (int j = 0; j < n; ++j)
                    result[j][i] = x[j];
            }
            return result;
        }

        static double[] HelperSolve(double[][] luMatrix, double[] b)
        {
            // before calling this helper, permute b using the perm array
            // from MatrixDecompose that generated luMatrix
            int n = luMatrix.Length;
            double[] x = new double[n];
            b.CopyTo(x, 0);

            for (int i = 1; i < n; ++i)
            {
                double sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1][n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum / luMatrix[i][i];
            }

            return x;
        }


        static double[] MatrixVectorProduct(double[][] matrix, double[] vector)
        {
            // result of multiplying an n x m matrix by a m x 1 
            // column vector (yielding an n x 1 column vector)
            int mRows = matrix.Length; int mCols = matrix[0].Length;
            int vRows = vector.Length;
            if (mCols != vRows)
                throw new Exception("Non-conformable matrix and vector");
            double[] result = new double[mRows];
            for (int i = 0; i < mRows; ++i)
                for (int j = 0; j < mCols; ++j)
                    result[i] += matrix[i][j] * vector[j];
            return result;
        }

        static double ProductFunction(double[] vector1, double[][] matrix, double[] vector2)
        {
            // result of multiplying an n x m matrix by a m x 1 
            // column vector (yielding an n x 1 column vector)
            int mRows = matrix.Length; int mCols = matrix[0].Length;
            int vRows = vector1.Length;
            if (mCols != mRows)
                throw new Exception("Non-conformable matrix and vector");
            double[] result1 = new double[mRows];
            for (int i = 0; i < mRows; ++i)
                for (int j = 0; j < mCols; ++j)
                    result1[i] += matrix[j][i] * vector1[j];

            double result = 0;
            for (int i = 0; i < mCols; i++)
                result += result1[i] * vector2[i];

            return result;
        }

        static double MatrixDeterminant(double[][] matrix)
        {
            int[] perm;
            int toggle;
            double[][] lum = MatrixDecompose(matrix, out perm, out toggle);
            if (lum == null)
                throw new Exception("Unable to compute MatrixDeterminant");
            double result = toggle;
            for (int i = 0; i < lum.Length; ++i)
                result *= lum[i][i];
            return result;
        }

        static double[][] MatrixDecompose(double[][] matrix, out int[] perm, out int toggle)
        {
            // Doolittle LUP decomposition with partial pivoting.
            // rerturns: result is L (with 1s on diagonal) and U;
            // perm holds row permutations; toggle is +1 or -1 (even or odd)
            int rows = matrix.Length;
            int cols = matrix[0].Length; // assume square
            if (rows != cols)
                throw new Exception("Attempt to decompose a non-square m");

            int n = rows; // convenience

            double[][] result = MatrixDuplicate(matrix);

            perm = new int[n]; // set up row permutation result
            for (int i = 0; i < n; ++i) { perm[i] = i; }

            toggle = 1; // toggle tracks row swaps.
            // +1 -greater-than even, -1 -greater-than odd. used by MatrixDeterminant

            for (int j = 0; j < n - 1; ++j) // each column
            {
                double colMax = Math.Abs(result[j][j]); // find largest val in col
                int pRow = j;
                //for (int i = j + 1; i less-than n; ++i)
                //{
                //  if (result[i][j] greater-than colMax)
                //  {
                //    colMax = result[i][j];
                //    pRow = i;
                //  }
                //}

                // reader Matt V needed this:
                for (int i = j + 1; i < n; ++i)
                {
                    if (Math.Abs(result[i][j]) > colMax)
                    {
                        colMax = Math.Abs(result[i][j]);
                        pRow = i;
                    }
                }
                // Not sure if this approach is needed always, or not.

                if (pRow != j) // if largest value not on pivot, swap rows
                {
                    double[] rowPtr = result[pRow];
                    result[pRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[pRow]; // and swap perm info
                    perm[pRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                // --------------------------------------------------
                // This part added later (not in original)
                // and replaces the 'return null' below.
                // if there is a 0 on the diagonal, find a good row
                // from i = j+1 down that doesn't have
                // a 0 in column j, and swap that good row with row j
                // --------------------------------------------------

                if (result[j][j] == 0.0)
                {
                    // find a good row to swap
                    int goodRow = -1;
                    for (int row = j + 1; row < n; ++row)
                    {
                        if (result[row][j] != 0.0)
                            goodRow = row;
                    }

                    if (goodRow == -1)
                        throw new Exception("Cannot use Doolittle's method");

                    // swap rows so 0.0 no longer on diagonal
                    double[] rowPtr = result[goodRow];
                    result[goodRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[goodRow]; // and swap perm info
                    perm[goodRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }
                // --------------------------------------------------
                // if diagonal after swap is zero . .
                //if (Math.Abs(result[j][j]) less-than 1.0E-20) 
                //  return null; // consider a throw

                for (int i = j + 1; i < n; ++i)
                {
                    result[i][j] /= result[j][j];
                    for (int k = j + 1; k < n; ++k)
                    {
                        result[i][k] -= result[i][j] * result[j][k];
                    }
                }


            } // main j column loop

            return result;
        } // MatrixDecompose

        static double[][] MatrixDuplicate(double[][] matrix)
        {
            // allocates/creates a duplicate of a matrix.
            double[][] result = MatrixCreate(matrix.Length, matrix[0].Length);
            for (int i = 0; i < matrix.Length; ++i) // copy the values
                for (int j = 0; j < matrix[i].Length; ++j)
                    result[i][j] = matrix[i][j];
            return result;
        }

        static double[][] MatrixCreate(int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            return result;
        }

        static double[] VectorSubtract(double[] fromVector, double[] toVector)
        {
            int intLength = fromVector.Length;
            if(intLength != toVector.Length)
                throw new Exception("Lengths are different");
            double[] result = new double[intLength];

            for (int i = 0; i < intLength; i++)
                result[i] = fromVector[i] - toVector[i];

            return result;

        }

        #endregion

        private class SignatureRegions
        {
            public double[][] covariance;
            public double[] Means;
            public double Determinant;
            public double[][] InverseCov;
            public double FirstTerm;
            public int ClassID;
            public int CellCount;
            public string ClassName;
        }

        private class ClassificationResults
        {
            public List<double> Likelihood;
            public double SumLikelihood;
            public double[] ProbVector;
            public int Class;
            public double RelativeAccuracy;
            public double ShannonEntropy;
            public double[] DNs;
            public int UID;
            public double LaggedShannonEntropy;
        }


        private void button1_Click(object sender, EventArgs e)
        {

            frmProgress pfrmProgress = new frmProgress();
            pfrmProgress.lblStatus.Text = "Caculation";
            pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
            pfrmProgress.Show();

            int intLyrCnt = 0; 
            int intClsCnt = 0;

            double dblSameProb = Math.Log(0.25);

            #region Load Signature file
            //string strSigpath = "E:\\Dropbox\\Projects\\2014\\NIH_withDrChun_DrGriffith\\Programmings\\VisUncertainty\\VisUncertainty\\bin\\Debug\\SampleData\\RS\\l7_collin_2001.gsg";
            string strSigpath = txtInputSigFile.Text;

            string[] lines = System.IO.File.ReadAllLines(strSigpath);

            //Get Class and Layer Count from Signature file
            int intSigLineIDX = 0;
            string strLine = null;
            bool blnFirstLine = true;
            do
            {
                intSigLineIDX++;
                blnFirstLine = false;
                strLine = lines[intSigLineIDX];

                if (strLine == "")
                    blnFirstLine = true;
                else if (strLine[0]!=' ')
                    blnFirstLine = true;
                
            } while (blnFirstLine);

            int intItemCount = 0;
            int intSigCharIdx = 0;
            while (intItemCount < 3)
            {
                strLine = strLine.TrimStart();

                if (strLine[intSigCharIdx] == ' ')
                {
                    if (intItemCount == 1)
                        intClsCnt =Convert.ToInt32(strLine.Substring(0,intSigCharIdx));
                    else if(intItemCount == 2)
                        intLyrCnt = Convert.ToInt32(strLine.Substring(0,intSigCharIdx));;

                    strLine = strLine.Substring(intSigCharIdx).TrimStart();
                    intSigCharIdx = -1;
                    intItemCount++;
                }
                intSigCharIdx++;
            }

            //Get Mean, covariance, and other information
            List<SignatureRegions> lstSigRegion = new List<SignatureRegions>();
            int intCollCnt = 0;

            while (intCollCnt < intClsCnt)
            {

                SignatureRegions pSigRegion = new SignatureRegions();

                //Basic Info
                intSigLineIDX = intSigLineIDX + 4;

                int intColItem = 0;
                strLine = lines[intSigLineIDX].TrimStart(); ;
                
                intSigCharIdx = 0;
                while (intColItem < 2)
                {

                    if (strLine[intSigCharIdx] == ' ')
                    {
                        if (intColItem == 0)
                            pSigRegion.ClassID = Convert.ToInt32(strLine.Substring(0, intSigCharIdx));
                        else if (intColItem == 1)
                        {
                            pSigRegion.CellCount = Convert.ToInt32(strLine.Substring(0, intSigCharIdx));
                            pSigRegion.ClassName = strLine.Substring(intSigCharIdx).TrimStart();
                        }

                        intColItem++;
                        strLine = strLine.Substring(intSigCharIdx).TrimStart();
                        intSigCharIdx = -1;

                    }
                    intSigCharIdx++;
                }
                
                //Mean Vector
                intSigCharIdx = 0;
                intSigLineIDX = intSigLineIDX + 3;
                strLine = lines[intSigLineIDX].TrimStart();
                double[] dblMeans = new double[intLyrCnt];
                intColItem = 0;
                while (intColItem < intLyrCnt-1)
                {

                    if (strLine[intSigCharIdx] == ' ')
                    {
                        if (intColItem == intLyrCnt-2)
                        {
                            dblMeans[intColItem] = Convert.ToDouble(strLine.Substring(0, intSigCharIdx));
                            dblMeans[intColItem + 1] = Convert.ToDouble(strLine.Substring(intSigCharIdx).TrimStart());
                        }
                        else
                            dblMeans[intColItem] = Convert.ToDouble(strLine.Substring(0, intSigCharIdx));

                        intColItem++;
                        strLine = strLine.Substring(intSigCharIdx).TrimStart();
                        intSigCharIdx = -1;

                    }
                    intSigCharIdx++;
                }
                pSigRegion.Means = dblMeans;

                intSigCharIdx = 0;
                intSigLineIDX = intSigLineIDX + 1;
                double[][] dblCovariance = new double[intLyrCnt][]; 
                for (int i = 0; i < intLyrCnt; i++)
                {
                    intSigLineIDX++;
                    strLine = lines[intSigLineIDX].TrimStart();
                    dblCovariance[i] = new double[intLyrCnt];

                    intColItem = 0;
                    while (intColItem < intLyrCnt)
                    {
                        if (strLine[intSigCharIdx] == ' ')
                        {
                            if (intColItem == 0)
                            {
                                //Remove First Column
                            }
                            else if (intColItem == intLyrCnt - 1)
                            {
                                dblCovariance[i][intColItem-1] = Convert.ToDouble(strLine.Substring(0, intSigCharIdx));
                                dblCovariance[i][intColItem] = Convert.ToDouble(strLine.Substring(intSigCharIdx).TrimStart());
                            }
                            else
                                dblCovariance[i][intColItem-1] = Convert.ToDouble(strLine.Substring(0, intSigCharIdx));

                            intColItem++;
                            strLine = strLine.Substring(intSigCharIdx).TrimStart();
                            intSigCharIdx = -1;

                        }
                        intSigCharIdx++;
                    }

                }
                pSigRegion.covariance = dblCovariance;
                lstSigRegion.Add(pSigRegion);
                intCollCnt++;
            }


            #endregion

            //List<SignatureRegions> lstSigRegion = new List<SignatureRegions>();

            //int[,] arrClsColor = new int[4, intClsCnt];
            //arrClsColor[0, 0] = 0; arrClsColor[1, 0] = 0; arrClsColor[2, 0] = 92; arrClsColor[3, 0] = 230;
            //arrClsColor[0, 1] = 1; arrClsColor[1, 1] = 76; arrClsColor[2, 1] = 0; arrClsColor[3, 1] = 115;
            //arrClsColor[0, 2] = 2; arrClsColor[1, 2] = 38; arrClsColor[2, 2] = 115; arrClsColor[3, 2] = 0;
            //arrClsColor[0, 3] = 3; arrClsColor[1, 3] = 230; arrClsColor[2, 3] = 0; arrClsColor[3, 3] = 0;

            int[,] arrClsColor = new int[4, intClsCnt];
            arrClsColor[0, 0] = 0; arrClsColor[1, 0] = 0; arrClsColor[2, 0] = 92; arrClsColor[3, 0] = 230;
            arrClsColor[0, 1] = 1; arrClsColor[1, 1] = 77; arrClsColor[2, 1] = 193; arrClsColor[3, 1] = 44;
            arrClsColor[0, 2] = 2; arrClsColor[1, 2] = 195; arrClsColor[2, 2] = 14; arrClsColor[3, 2] = 3;


            for (int i = 0; i < intClsCnt; i++)
            {
                lstSigRegion[i].Determinant = MatrixDeterminant(lstSigRegion[i].covariance);
                lstSigRegion[i].InverseCov = MatrixInverse(lstSigRegion[i].covariance);
                lstSigRegion[i].FirstTerm = Math.Pow(2 * Math.PI, (double)intLyrCnt / (double)2.0) * Math.Pow(lstSigRegion[i].Determinant, (double)0.5);
            }

            //#endregion

            //string strOriRSTpath = "E:\\Dropbox\\Projects\\2014\\NIH_withDrChun_DrGriffith\\Programmings\\VisUncertainty\\VisUncertainty\\bin\\Debug\\SampleData\\RS\\Mask\\l7_mask_2001.img";
            //string strOriRSTpath = "D:\\Dropbox\\Projects\\2014\\NIH_withDrChun_DrGriffith\\Programmings\\VisUncertainty\\VisUncertainty\\bin\\Debug\\SampleData\\RS\\Mask\\l7_mask_2001.img";
            string strOriRSTpath = txtInputRaster.Text;

            string strExtentFileSource = System.IO.Path.GetDirectoryName(strOriRSTpath);
            string strExtentFileName = System.IO.Path.GetFileName(strOriRSTpath);

            IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
            IRasterWorkspace rasterWorkspace = (IRasterWorkspace)
                workspaceFactory.OpenFromFile(strExtentFileSource, 0);

            //Open a file raster dataset. 
            IRasterDataset2 rasterDataset = (IRasterDataset2)rasterWorkspace.OpenRasterDataset(strExtentFileName);
            IRaster2 pOriRaster2 = (IRaster2)rasterDataset.CreateFullRaster();
            IRasterBandCollection bands = (IRasterBandCollection)pOriRaster2;
            double[] dblMaximums = new double[intLyrCnt];
            for (int i = 0; i < intLyrCnt; i++)
            {
                IRasterBand rasterBand = bands.Item(i);
                dblMaximums[i] = rasterBand.Statistics.Maximum;
            }
            int intMaximum = Convert.ToInt32(dblMaximums.Max());
            //IRaster2 pOriRaster2 = (IRaster2)m_pOriRstLayer.Raster;
            //Create a raster cursor with a system-optimized pixel block size by passing a null.

            // Get NoDataValues
            m_pRasterProps = (IRasterProps)pOriRaster2;
            System.Array pOriNoData = (System.Array)m_pRasterProps.NoDataValue;
            
            
            IPnt Oriblocksize = new PntClass();
            Oriblocksize.SetCoords(m_pRasterProps.Width, m_pRasterProps.Height);
            // Create a raster cursor with a system-optimized pixel block size by passing a null.

            IRasterCursor pOriRstCursor = pOriRaster2.CreateCursorEx(Oriblocksize);
            //IRasterCursor pOriRstCursor = pOriRaster2.CreateCursorEx(null);
            IPixelBlock3 pixelblock3 = (IPixelBlock3)pOriRstCursor.PixelBlock;


            System.Array[] OriPixels = new System.Array[intLyrCnt];
            //UInt16[][,] intArry = new UInt16[intLyrCnt][,];
            for (int i = 0; i < intLyrCnt; i++)
            {
                OriPixels[i] = (System.Array)pixelblock3.get_PixelData(i);
            }


            //Compare the Arrays and Draw
            int intBlockwidth = m_pRasterProps.Width;
            int intBlockHeight = m_pRasterProps.Height;

            ISpatialReference sr = new UnknownCoordinateSystemClass();

            m_arrClsResults = new ClassificationResults[intBlockwidth, intBlockHeight];


            #region MLC Classficiation
            
            if (cboClassificationMethod.Text == "Maximum likelihood Classifier")
            {
                int[,] ClassifiedResult = new int[intBlockwidth, intBlockHeight];
                int[] priorClssCnt = new int[intClsCnt];

                int intUID = 0;
                for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                {
                    //ClassifiedResult[rowIndex] = new int[intBlockwidth];

                    for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
                    {
                        double[] dblX = new double[intLyrCnt];

                        for (int i = 0; i < intLyrCnt; i++)
                        {
                            dblX[i] = Convert.ToDouble(OriPixels[i].GetValue(colIndex, rowIndex));
                        }

                        List<double> lstMLs = new List<double>();

                        for (int i = 0; i < intClsCnt; i++)
                        {
                            double[] diff = VectorSubtract(dblX, lstSigRegion[i].Means);
                            double dblProduct = ProductFunction(diff, lstSigRegion[i].InverseCov, diff);
                            //lstMLs.Add(dblSameProb - (0.5 * lstSigRegion[i].logDeterminant) - (0.5 * dblProduct));
                            lstMLs.Add(Math.Exp(-0.5 * dblProduct));
                        }
                        double dblMaxML = lstMLs.Max();
                        int intClass = lstMLs.IndexOf(dblMaxML);
                        double dblSum = lstMLs.Sum();

                        //Assign Values MLC Results Class to check the probability vector
                        m_arrClsResults[colIndex, rowIndex] = new ClassificationResults();
                        m_arrClsResults[colIndex, rowIndex].UID = intUID;
                        m_arrClsResults[colIndex, rowIndex].DNs = dblX;
                        m_arrClsResults[colIndex, rowIndex].Likelihood = lstMLs;
                        m_arrClsResults[colIndex, rowIndex].Class = intClass;
                        m_arrClsResults[colIndex, rowIndex].SumLikelihood = dblSum;

                        //Cal Prior Probility  
                        priorClssCnt[intClass]++;

                        //Save Result for Raster Format;
                        ClassifiedResult[colIndex, rowIndex] = intClass;
                    }
                }

                double[] priorProb = new double[intClsCnt];

                for (int i = 0; i < intClsCnt; i++)
                {
                    //priorProb[i] = (double)priorClssCnt[i] / priorClssCnt.Sum();//With Different Weight
                    priorProb[i] = (double)1 / (double)intClsCnt;
                }

                for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
                    {
                        double[] dblProducts = new double[intClsCnt];
                        List<double> dblLikelihood = m_arrClsResults[colIndex, rowIndex].Likelihood;
                        for (int i = 0; i < intClsCnt; i++)
                        {
                            dblProducts[i] = dblLikelihood[i] * priorProb[i];
                        }
                        double dblSumProduct = dblProducts.Sum();
                        double[] dblProbVector = new double[intClsCnt];
                        double dblShannonEntropy = 0;
                        for (int i = 0; i < intClsCnt; i++)
                        {
                            dblProbVector[i] = dblProducts[i] / dblSumProduct;
                            if (dblProbVector[i] != 0)
                                dblShannonEntropy += dblProbVector[i] * Math.Log(dblProbVector[i], 2);
                        }
                        m_arrClsResults[colIndex, rowIndex].ProbVector = dblProbVector;

                        double[] sortedProbVector = new double[intClsCnt];

                        System.Array.Copy(dblProbVector, sortedProbVector, intClsCnt);

                        System.Array.Sort(sortedProbVector);
                        m_arrClsResults[colIndex, rowIndex].RelativeAccuracy = sortedProbVector[intClsCnt - 1] - sortedProbVector[intClsCnt - 2];

                        m_arrClsResults[colIndex, rowIndex].ShannonEntropy = ((double)(-1) * dblShannonEntropy) / (Math.Log(intLyrCnt, 2));
                    }
                }




                if (txtClassifiedResult.Text != "")
                {
                    //Save Results
                    IRasterDataset pRstDSMLC = CreateRasterDataset2(strExtentFileSource, "mlc", sr, m_pRasterProps.Extent.XMin, m_pRasterProps.Extent.YMin, m_pRasterProps.Width, m_pRasterProps.Height,
                        m_pRasterProps.MeanCellSize().X, ClassifiedResult); 
                }

                float[][,] clsProbMatResult = new float[4][,];

                for (int i = 0; i < intClsCnt; i++)
                {
                    clsProbMatResult[i] = new float[intBlockwidth, intBlockHeight];
                    for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                    {
                        for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
                        {
                            clsProbMatResult[i][colIndex, rowIndex] = Convert.ToSingle(m_arrClsResults[colIndex, rowIndex].ProbVector[i]);

                        }
                    }

                    if (txtFuzzyOuput.Text != "")
                    {
                        CreateRasterDataset3(strExtentFileSource, "prob" + i.ToString(), sr, m_pRasterProps.Extent.XMin, m_pRasterProps.Extent.YMin, m_pRasterProps.Width, m_pRasterProps.Height,
        m_pRasterProps.MeanCellSize().X, clsProbMatResult[i]);
                    }
                }

                float[,] clsEntropy = new float[intBlockwidth, intBlockHeight];

                for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
                    {
                        clsEntropy[colIndex, rowIndex] = Convert.ToSingle(m_arrClsResults[colIndex, rowIndex].ShannonEntropy);

                    }
                }

                //Calculate Lagged Shannon Entropy
                int intNeigborCnt = 1;
                for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
                    {
                        double dblSumSE = 0;
                        int intCnt = 0;

                        for (int v = (-1) * intNeigborCnt; v <= intNeigborCnt; v++)
                        {
                            for (int h = (-1) * intNeigborCnt; h <= intNeigborCnt; h++)
                            {
                                int LaggedVIdx = rowIndex + v;
                                int LaggedHIdx = colIndex + h;
                                if (LaggedVIdx >= 0 && LaggedVIdx < intBlockHeight && LaggedHIdx >= 0 && LaggedHIdx < intBlockwidth)
                                {
                                    if (LaggedVIdx != rowIndex || LaggedHIdx != colIndex)
                                    {
                                        dblSumSE += m_arrClsResults[LaggedHIdx, LaggedVIdx].ShannonEntropy;
                                        intCnt++;
                                    }
                                }
                            }
                        }
                        m_arrClsResults[colIndex, rowIndex].LaggedShannonEntropy = dblSumSE / (double)intCnt;
                    }
                }
                if (txtEntropyOutput.Text != "")
                {
                    CreateRasterDataset3(strExtentFileSource, "ment", sr, m_pRasterProps.Extent.XMin, m_pRasterProps.Extent.YMin, m_pRasterProps.Width, m_pRasterProps.Height,
        m_pRasterProps.MeanCellSize().X, clsEntropy);
                }
            }
            #endregion

            #region Write txt for Test
            //            string strOutputName = "E:\\Dropbox\\Projects\\2014\\NIH_withDrChun_DrGriffith\\Programmings\\VisUncertainty\\VisUncertainty\\bin\\Debug\\SampleData\\RS\\Mask\\SampleData.txt";
//            System.IO.StreamWriter pSW = new System.IO.StreamWriter(strOutputName);

//            StringBuilder plotCommmand = new StringBuilder();
//            for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
//            {
//                ClassifiedResult[rowIndex] = new int[intBlockwidth];

//                for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
//                {
//                    for (int i = 0; i < intLyrCnt; i++)
//                    {
//                         plotCommmand.Append(OriPixels[i].GetValue(colIndex, rowIndex).ToString() + ", ");
//                    }
//                    pSW.WriteLine(plotCommmand.ToString());
//                    plotCommmand.Clear();
//                }
//            }
           

//            pSW.Close();
            //            pSW.Dispose();

            #endregion

            #region Fuzzy c means clustering in R
            if (cboClassificationMethod.Text == "Supervised Fuzzy C-Means")
            {
                double[,] DNs = new double[intBlockHeight * intBlockwidth, intLyrCnt];

                int intDNIdx = 0;
                for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
                    {
                        double[] arrX = new double[intLyrCnt];

                        for (int i = 0; i < intLyrCnt; i++)
                        {
                            double dblx = Convert.ToDouble(OriPixels[i].GetValue(colIndex, rowIndex));
                            DNs[intDNIdx, i] = dblx;
                            arrX[i] = dblx;
                        }
                        m_arrClsResults[colIndex, rowIndex] = new ClassificationResults();
                        m_arrClsResults[colIndex, rowIndex].UID = intDNIdx;
                        m_arrClsResults[colIndex, rowIndex].DNs = arrX;
                        intDNIdx++;
                    }
                }
                NumericMatrix nmDNs = m_pEngine.CreateNumericMatrix(DNs);
                m_pEngine.SetSymbol("x.sample", nmDNs);

                double[,] Centers = new double[intClsCnt, intLyrCnt];

                for (int i = 0; i < intClsCnt; i++)
                {
                    for (int j = 0; j < intLyrCnt; j++)
                    {
                        Centers[i, j] = lstSigRegion[i].Means[j];
                    }
                }
                NumericMatrix nmCenters = m_pEngine.CreateNumericMatrix(Centers);
                m_pEngine.SetSymbol("sig.center", nmCenters);
                m_pEngine.Evaluate("cl <- cmeans(x.sample, centers = sig.center, 100, verbose = F, method='cmeans')");
                int[] ClusterResult = m_pEngine.Evaluate("cl$cluster").AsInteger().ToArray();
                NumericMatrix MembershipResult = m_pEngine.Evaluate("cl$membership").AsNumericMatrix();


                //Save Classified Results
                int[,] clsMatResult = new int[intBlockwidth, intBlockHeight];

                intDNIdx = 0;
                for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
                    {
                        clsMatResult[colIndex, rowIndex] = ClusterResult[intDNIdx] - 1;

                        m_arrClsResults[colIndex, rowIndex].Class = ClusterResult[intDNIdx] - 1;

                        double[] dblProbVector = new double[intClsCnt];
                        double dblShannonEntropy = 0;
                        for (int i = 0; i < intClsCnt; i++)
                        {
                            dblProbVector[i] = MembershipResult[intDNIdx, i];
                            if (dblProbVector[i] != 0)
                                dblShannonEntropy += dblProbVector[i] * Math.Log(dblProbVector[i], 2);
                        }
                        m_arrClsResults[colIndex, rowIndex].ProbVector = dblProbVector;

                        double[] sortedProbVector = new double[intClsCnt];

                        System.Array.Copy(dblProbVector, sortedProbVector, intClsCnt);

                        System.Array.Sort(sortedProbVector);
                        m_arrClsResults[colIndex, rowIndex].RelativeAccuracy = sortedProbVector[intClsCnt - 1] - sortedProbVector[intClsCnt - 2];

                        m_arrClsResults[colIndex, rowIndex].ShannonEntropy = ((double)(-1) * dblShannonEntropy) / (Math.Log(intClsCnt, 2));

                        intDNIdx++;
                    }
                }

                //Calculate Lagged Shannon Entropy
                int intNeigborCnt = 1;
                for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
                    {
                        double dblSumSE = 0;
                        int intCnt = 0;

                        for (int v = (-1) * intNeigborCnt; v <= intNeigborCnt; v++)
                        {
                            for (int h = (-1) * intNeigborCnt; h <= intNeigborCnt; h++)
                            {
                                int LaggedVIdx = rowIndex + v;
                                int LaggedHIdx = colIndex + h;
                                if (LaggedVIdx >= 0 && LaggedVIdx < intBlockHeight && LaggedHIdx >= 0 && LaggedHIdx < intBlockwidth)
                                {
                                    if (LaggedVIdx != rowIndex || LaggedHIdx != colIndex)
                                    {
                                        dblSumSE += m_arrClsResults[LaggedHIdx, LaggedVIdx].ShannonEntropy;
                                        intCnt++;
                                    }
                                }
                            }
                        }
                        m_arrClsResults[colIndex, rowIndex].LaggedShannonEntropy = dblSumSE / (double)intCnt;
                    }
                }
                if (txtClassifiedResult.Text != "")
                {
                    IRasterDataset pRstDSFCM = CreateRasterDataset2(strExtentFileSource, "fcm", sr, m_pRasterProps.Extent.XMin, m_pRasterProps.Extent.YMin, m_pRasterProps.Width, m_pRasterProps.Height,
                        m_pRasterProps.MeanCellSize().X, clsMatResult);
                }

                if (txtFuzzyOuput.Text != "")
                {
                    //Save Classified Results
                    float[][,] clsProbMatResult = new float[intClsCnt][,];
                    intDNIdx = 0;
                    for (int i = 0; i < intClsCnt; i++)
                    {
                        clsProbMatResult[i] = new float[intBlockwidth, intBlockHeight];
                        for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                        {
                            for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
                            {
                                clsProbMatResult[i][colIndex, rowIndex] = Convert.ToSingle(m_arrClsResults[colIndex, rowIndex].ProbVector[i]);

                            }
                        }
                        CreateRasterDataset3(strExtentFileSource, "fm" + i.ToString(), sr, m_pRasterProps.Extent.XMin, m_pRasterProps.Extent.YMin, m_pRasterProps.Width, m_pRasterProps.Height,
        m_pRasterProps.MeanCellSize().X, clsProbMatResult[i]);
                    }
                }

                if (txtEntropyOutput.Text != "")
                {
                    float[,] clsEntropy = new float[intBlockwidth, intBlockHeight];

                    for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                    {
                        for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
                        {
                            clsEntropy[colIndex, rowIndex] = Convert.ToSingle(m_arrClsResults[colIndex, rowIndex].ShannonEntropy);

                        }
                    }
                    CreateRasterDataset3(strExtentFileSource, "fent", sr, m_pRasterProps.Extent.XMin, m_pRasterProps.Extent.YMin, m_pRasterProps.Width, m_pRasterProps.Height,
        m_pRasterProps.MeanCellSize().X, clsEntropy);
                }
            }
            #endregion


            List<int>[][] lstDN = new List<int>[intLyrCnt][];
            for (int j = 0; j < intLyrCnt; j++)
            {
                lstDN[j] = new List<int>[intClsCnt];
            }

            for (int j = 0; j < intLyrCnt; j++)
            {
                for (int i = 0; i < intClsCnt; i++)
                    lstDN[j][i] = new List<int>();

            }

            List<int>[][] lstLyrIDs = new List<int>[intLyrCnt][];
            for (int j = 0; j < intLyrCnt; j++)
            {
                lstLyrIDs[j] = new List<int>[intClsCnt];
            }

            for (int j = 0; j < intLyrCnt; j++)
            {
                for (int i = 0; i < intClsCnt; i++)
                    lstLyrIDs[j][i] = new List<int>();

            }

            //Uncertainty Layer Settings
            //System.Array[] ProbPixels = null;
            //int intClsCnt = intClsCnt;
            List<int>[][] lstProb = null;
            List<int>[][] lstProbIDs = null;

            lstProb = new List<int>[intClsCnt][];

            for (int j = 0; j < intClsCnt; j++)
            {
                lstProb[j] = new List<int>[intClsCnt];
            }

            for (int j = 0; j < intClsCnt; j++)
            {
                for (int i = 0; i < intClsCnt; i++)
                    lstProb[j][i] = new List<int>();

            }


            lstProbIDs = new List<int>[intClsCnt][];

            for (int j = 0; j < intClsCnt; j++)
            {
                lstProbIDs[j] = new List<int>[intClsCnt];
            }

            for (int j = 0; j < intClsCnt; j++)
            {
                for (int i = 0; i < intClsCnt; i++)
                    lstProbIDs[j][i] = new List<int>();

            }


            //Store ID and Values for Brushing and Linking
            m_lstIDsValues = new List<int[]>();


            //For New Chart
            List<int>[] lstNewProb = new List<int>[intClsCnt];
            List<int>[] lstNewID = new List<int>[intClsCnt];
            for (int j = 0; j < intClsCnt; j++)
            {
                lstNewProb[j] = new List<int>();

                lstNewID[j] = new List<int>();
            }

            //For Entropy Moran Scatter plot
            m_Entropies = new double[intBlockHeight * intBlockwidth];
            m_laggedEntropies = new double[intBlockHeight * intBlockwidth];
            int[] EntUIDs = new int[intBlockHeight * intBlockwidth];


            int intID = 0;

            //Collect data for visualization
            for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
            {
                for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                {
                    //int intClass = Convert.ToInt32(pClspixels.GetValue(colIndex, rowIndex));
                    int intClass = m_arrClsResults[colIndex, rowIndex].Class;

                    int[] arrValues = new int[intLyrCnt + intClsCnt + 3];

                    arrValues[0] = colIndex;
                    arrValues[1] = rowIndex;
                    arrValues[2] = intClass;

                    m_Entropies[intID] = m_arrClsResults[colIndex, rowIndex].ShannonEntropy;
                    m_laggedEntropies[intID] = m_arrClsResults[colIndex, rowIndex].LaggedShannonEntropy;
                    EntUIDs[intID] = m_arrClsResults[colIndex, rowIndex].UID;

                    for (int i = 0; i < intClsCnt; i++)
                    {
                              //          lstNewProb[i].Add(Convert.ToInt32(arrClsResults[colIndex, rowIndex].ProbVector[i] * (double)255));
                              //lstNewID[i].Add(arrClsResults[colIndex, rowIndex].UID);
                        if (arrClsColor[0, i] == intClass)
                        {

                            //Get DNs
                            for (int j = 0; j < intLyrCnt; j++)
                            {
                                //if (colIndex == 0 && rowIndex == 241)
                                //    j = j;
                                int intValue = Convert.ToInt32(OriPixels[j].GetValue(colIndex, rowIndex));
                                lstDN[j][i].Add(intValue);
                                lstLyrIDs[j][i].Add(intID);
                                arrValues[j + 3] = intValue;

                            }

                            for (int j = 0; j < intClsCnt; j++)
                            {
                                int intValue = Convert.ToInt32(m_arrClsResults[colIndex, rowIndex].ProbVector[j] * (double)intMaximum);
                                lstProb[j][i].Add(intValue);
                                lstProbIDs[j][i].Add(intID);
                                arrValues[j + 3 + intLyrCnt] = intValue;
                            }

                        }
                    }

                    m_lstIDsValues.Add(arrValues);
                    intID++;
                    if (m_lstIDsValues.Count != intID)
                        MessageBox.Show("Diff"); //For deburgging

                }
            }


            pChart.Series.Clear();



            m_lstPtsIdContainer = new List<List<int>>();
            m_lstPtSeriesID = new List<int>();


            //////////////////////////////////////////////
            #region Draw Violin plot with DN

            List<List<int>> lstPtsIdContainerDN = new List<List<int>>();
            List<int> lstPtSeriesIDDN = new List<int>(); ;

            frmVisDN pfrmVisDN = new frmVisDN();
            Chart pDNChart = pfrmVisDN.pChart;
            pfrmVisDN.Show();

            for (int i = 0; i < intClsCnt; i++)
            {
                Color FillColor = Color.FromArgb(arrClsColor[1, i], arrClsColor[2, i], arrClsColor[3, i]);

                for (int j = 0; j < intLyrCnt; j++)
                {
                    List<int> lstTarget = lstDN[j][i]; ;

                    List<int> lstIDs = lstLyrIDs[j][i]; ;

                    List<int> sortedTarget = new List<int>(lstTarget);

                    //to prevent overlapping of Boxplots
                    //double dblPlotHalfWidth = 0.05;
                    double dblPlotHalfWidth = 0.08; //Adjust spacing
                    double dblMin = (dblPlotHalfWidth * (intClsCnt - 1)) * (-1);
                    double dblRefXvalue = j + (dblMin + (i * (dblPlotHalfWidth * 2)));

                    //Find Max and Min
                    double[] adblStats = BoxPlotStats(sortedTarget);
                    bool blnDrawViolin = true;
                    if (adblStats[0] == adblStats[4])
                        blnDrawViolin = false;

                    //Restrict Plot min and max
                    if (adblStats[0] < 0)
                        adblStats[0] = 0;
                    if (adblStats[4] > 255)
                        adblStats[4] = 255;

                    string strNumbering = i.ToString() + "_" + j.ToString();

                    double dblXmin = dblRefXvalue - (dblPlotHalfWidth / 2);
                    double dblXmax = dblRefXvalue + (dblPlotHalfWidth / 2);

                    if (blnDrawViolin)
                    {
                        //Draw Lines
                        AddLineSeries(pDNChart, "m_" + strNumbering, Color.Red, 1, ChartDashStyle.Dash, dblRefXvalue - dblPlotHalfWidth, dblRefXvalue + dblPlotHalfWidth, adblStats[2], adblStats[2]);
                        AddLineSeries(pDNChart, "v_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblRefXvalue, dblRefXvalue, adblStats[1], adblStats[3]);
                        //AddLineSeries(pChart, "v2_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblRefXvalue, dblRefXvalue, adblStats[3], adblStats[4]);
                        AddLineSeries(pDNChart, "h1_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblXmin, dblXmax, adblStats[1], adblStats[1]);
                        AddLineSeries(pDNChart, "h2_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblXmin, dblXmax, adblStats[3], adblStats[3]);
                    }
                    else
                    {
                        //Draw Outliers
                        var pMedPt = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "mpt_" + strNumbering,
                            Color = Color.Red,
                            BorderColor = FillColor,
                            IsVisibleInLegend = false,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                            MarkerStyle = MarkerStyle.Circle,
                            MarkerSize = 2,
                        };

                        pDNChart.Series.Add(pMedPt);
                        pMedPt.Points.AddXY(dblRefXvalue, adblStats[2]);
                    }

                    if (blnDrawViolin)
                    {
                        //Draw Violin Plot
                        var pviolin = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "vio1_" + strNumbering,
                            Color = FillColor,
                            BorderColor = FillColor,
                            IsVisibleInLegend = false,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line

                        };
                        pDNChart.Series.Add(pviolin);

                        double[,] adblViolinStats = ViolinPlot(sortedTarget, 4);

                        int intChartLenth = (adblViolinStats.Length) / 2;


                        for (int k = 0; k < intChartLenth; k++)
                        {
                            if (adblViolinStats[k, 0] > adblStats[0] && adblViolinStats[k, 0] < adblStats[4])
                                pviolin.Points.AddXY(dblRefXvalue - adblViolinStats[k, 1], adblViolinStats[k, 0]);
                        }
                        pviolin.Points.AddXY(dblRefXvalue, adblStats[4]);
                        for (int k = intChartLenth - 1; k >= 0; k--)
                            if (adblViolinStats[k, 0] > adblStats[0] && adblViolinStats[k, 0] < adblStats[4])
                                pviolin.Points.AddXY(dblRefXvalue + adblViolinStats[k, 1], adblViolinStats[k, 0]);
                        pviolin.Points.AddXY(dblRefXvalue, adblStats[0]);
                    }



                    //Draw Outliers
                    var pOutlier = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "out_" + strNumbering,
                        Color = FillColor,
                        BorderColor = FillColor,
                        IsVisibleInLegend = false,
                        ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                        MarkerStyle = MarkerStyle.Circle,
                        MarkerSize = 1,
                    };

                    pDNChart.Series.Add(pOutlier);
                    lstPtSeriesIDDN.Add(pDNChart.Series.Count - 1); //Add Series ID for brushing and linking

                    List<int> lstPTsIds = new List<int>();
                    int intListCnt = lstTarget.Count;
                    for (int k = 0; k < intListCnt; k++)
                    {
                        if (lstTarget[k] < adblStats[0] || lstTarget[k] > adblStats[4])
                        {
                            pOutlier.Points.AddXY(dblRefXvalue, lstTarget[k]);
                            lstPTsIds.Add(lstIDs[k]);
                            //if (m_lstIDsValues[lstIDs[k]][3 + i] != lstTarget[k])
                            //    MessageBox.Show("ddd");
                        }
                    }
                    lstPtsIdContainerDN.Add(lstPTsIds);
                }

            }
            //m_intTotalNSeries = pDNChart.Series.Count;
            int intTotalNSeriesDN = pDNChart.Series.Count;

            //Chart Setting
            pDNChart.ChartAreas[0].AxisY.Minimum = 0;
            pDNChart.ChartAreas[0].AxisY.Maximum = 255;

            pDNChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            pDNChart.ChartAreas[0].AxisY.Title = "DN and Probability";

            //pChart.ChartAreas[0].AxisY.IsLabelAutoFit = true;
            //if (chkAddProb.Checked)
            //{
            //    pChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            //    pChart.ChartAreas[0].AxisY.Title = "DN and Probability";
            //}
            //else
            //    pChart.ChartAreas[0].AxisY.Title = "DN";

            pDNChart.ChartAreas[0].AxisX.Maximum = intLyrCnt - 0.5;
            pDNChart.ChartAreas[0].AxisX.Minimum = -0.5;

            pDNChart.ChartAreas[0].AxisX.Title = "Layers";

            pDNChart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            pDNChart.ChartAreas[0].AxisX.CustomLabels.Clear();

            for (int j = 0; j < intLyrCnt; j++)
            {
                System.Windows.Forms.DataVisualization.Charting.CustomLabel pcutsomLabel = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
                pcutsomLabel.FromPosition = j - 0.5;
                pcutsomLabel.ToPosition = j + 0.5;
                pcutsomLabel.Text = "DN of Layer " + j.ToString() + "(0-255)";
                pDNChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
            }



            #endregion

            
            
            
            
            #region draw violin chart for uncertainty (with Classes)
            List<List<int>> lstPtsIdContainerUncern = new List<List<int>>();
            List<int> lstPtSeriesIDUncern = new List<int>(); ;
            
            frmVisProbEnt pfrmVisUncern = new frmVisProbEnt();
            pfrmVisUncern.Show();

            Chart pUncernChart = pfrmVisUncern.pChart;

            for (int i = 0; i < intClsCnt; i++)
            {
                Color FillColor = Color.FromArgb(arrClsColor[1, i], arrClsColor[2, i], arrClsColor[3, i]);

                for (int j = 0; j < intClsCnt; j++)
                {
                    List<int> lstTarget = lstProb[j][i];

                    List<int> lstIDs = lstProbIDs[j][i];

                    List<int> sortedTarget = new List<int>(lstTarget);

                    //to prevent overlapping of Boxplots
                    //double dblPlotHalfWidth = 0.05;
                    double dblPlotHalfWidth = 0.08; //Adjust spacing
                    double dblMin = (dblPlotHalfWidth * (intClsCnt - 1)) * (-1);
                    double dblRefXvalue = j + (dblMin + (i * (dblPlotHalfWidth * 2)));

                    //Find Max and Min
                    double[] adblStats = BoxPlotStats(sortedTarget);
                    bool blnDrawViolin = true;
                    if (adblStats[0] == adblStats[4])
                        blnDrawViolin = false;

                    //Restrict Plot min and max
                    if (adblStats[0] < 0)
                        adblStats[0] = 0;
                    if (adblStats[4] > 255)
                        adblStats[4] = 255;

                    string strNumbering = i.ToString() + "_" + j.ToString();

                    double dblXmin = dblRefXvalue - (dblPlotHalfWidth / 2);
                    double dblXmax = dblRefXvalue + (dblPlotHalfWidth / 2);

                    if (blnDrawViolin)
                    {
                        //Draw Lines
                        AddLineSeries(pUncernChart, "m_" + strNumbering, Color.Red, 1, ChartDashStyle.Dash, dblRefXvalue - dblPlotHalfWidth, dblRefXvalue + dblPlotHalfWidth, adblStats[2], adblStats[2]);
                        AddLineSeries(pUncernChart, "v_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblRefXvalue, dblRefXvalue, adblStats[1], adblStats[3]);
                        //AddLineSeries(pChart, "v2_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblRefXvalue, dblRefXvalue, adblStats[3], adblStats[4]);
                        AddLineSeries(pUncernChart, "h1_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblXmin, dblXmax, adblStats[1], adblStats[1]);
                        AddLineSeries(pUncernChart, "h2_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblXmin, dblXmax, adblStats[3], adblStats[3]);
                    }
                    else
                    {
                        //Draw Outliers
                        var pMedPt = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "mpt_" + strNumbering,
                            Color = Color.Red,
                            BorderColor = FillColor,
                            IsVisibleInLegend = false,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                            MarkerStyle = MarkerStyle.Circle,
                            MarkerSize = 2,
                        };

                        pUncernChart.Series.Add(pMedPt);
                        pMedPt.Points.AddXY(dblRefXvalue, adblStats[2]);
                    }

                    if (blnDrawViolin)
                    {
                        //Draw Violin Plot
                        var pviolin = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "vio1_" + strNumbering,
                            Color = FillColor,
                            BorderColor = FillColor,
                            IsVisibleInLegend = false,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line

                        };
                        pUncernChart.Series.Add(pviolin);

                        double[,] adblViolinStats = ViolinPlot(sortedTarget, 4);

                        int intChartLenth = (adblViolinStats.Length) / 2;


                        for (int k = 0; k < intChartLenth; k++)
                        {
                            if (adblViolinStats[k, 0] > adblStats[0] && adblViolinStats[k, 0] < adblStats[4])
                                pviolin.Points.AddXY(dblRefXvalue - adblViolinStats[k, 1], adblViolinStats[k, 0]);
                        }
                        pviolin.Points.AddXY(dblRefXvalue, adblStats[4]);
                        for (int k = intChartLenth - 1; k >= 0; k--)
                            if (adblViolinStats[k, 0] > adblStats[0] && adblViolinStats[k, 0] < adblStats[4])
                                pviolin.Points.AddXY(dblRefXvalue + adblViolinStats[k, 1], adblViolinStats[k, 0]);
                        pviolin.Points.AddXY(dblRefXvalue, adblStats[0]);
                    }



                    //Draw Outliers
                    var pOutlier = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "out_" + strNumbering,
                        Color = FillColor,
                        BorderColor = FillColor,
                        IsVisibleInLegend = false,
                        ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                        MarkerStyle = MarkerStyle.Circle,
                        MarkerSize = 1,
                    };

                    pUncernChart.Series.Add(pOutlier);
                    lstPtSeriesIDUncern.Add(pUncernChart.Series.Count - 1); //Add Series ID for brushing and linking
                    
                    List<int> lstPTsIds = new List<int>();
                    int intListCnt = lstTarget.Count;
                    for (int k = 0; k < intListCnt; k++)
                    {
                        if (lstTarget[k] < adblStats[0] || lstTarget[k] > adblStats[4])
                        {
                            pOutlier.Points.AddXY(dblRefXvalue, lstTarget[k]);
                            lstPTsIds.Add(lstIDs[k]);
                            //if (m_lstIDsValues[lstIDs[k]][3 + i] != lstTarget[k])
                            //    MessageBox.Show("ddd");
                        }
                    }
                    lstPtsIdContainerUncern.Add(lstPTsIds);
                }

            }
            int intTotalNSeriesUncern = pUncernChart.Series.Count;

            //Chart Setting
            pUncernChart.ChartAreas[0].AxisY.Minimum = 0;
            pUncernChart.ChartAreas[0].AxisY.Maximum = 255;

            pUncernChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            pUncernChart.ChartAreas[0].AxisY.Title = "DN and Probability";

            pUncernChart.ChartAreas[0].AxisX.Maximum = intClsCnt - 0.5;
            pUncernChart.ChartAreas[0].AxisX.Minimum = -0.5;

            pUncernChart.ChartAreas[0].AxisX.Title = "Layers";

            pUncernChart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            pUncernChart.ChartAreas[0].AxisX.CustomLabels.Clear();

            for (int j = 0; j < intClsCnt; j++)
            {
                System.Windows.Forms.DataVisualization.Charting.CustomLabel pcutsomLabel = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
                pcutsomLabel.FromPosition = j - 0.5;
                pcutsomLabel.ToPosition = j + 0.5;
                pcutsomLabel.Text = "Prob of Class " + (j).ToString() + "(0-100)";

                pUncernChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
            }
            
            //Assign required variables for Brushing and linking
            pfrmVisDN.m_pUncernChart = pUncernChart;
            pfrmVisDN.m_pActiveView = m_pActiveView;
            pfrmVisDN.m_intLyrCnt = intLyrCnt;
            pfrmVisDN.m_intClsCnt = intClsCnt;
            pfrmVisDN.m_intTotalNSeries = intTotalNSeriesDN;
            pfrmVisDN.m_intTotalNSeriesUncern = intTotalNSeriesUncern;
            pfrmVisDN.m_lstPtsIdContainer = lstPtsIdContainerDN;
            pfrmVisDN.m_lstPtSeriesID = lstPtSeriesIDDN;
            pfrmVisDN.m_pRasterProps = m_pRasterProps;
            pfrmVisDN.m_lstIDsValues = m_lstIDsValues;


            pfrmVisUncern.m_pDNChart = pDNChart;
            pfrmVisUncern.m_pActiveView = m_pActiveView;
            pfrmVisUncern.m_intLyrCnt = intLyrCnt;
            pfrmVisUncern.m_intClsCnt = intClsCnt;
            pfrmVisUncern.m_intTotalNSeries = intTotalNSeriesUncern;
            pfrmVisUncern.m_intTotalNSeriesDN = intTotalNSeriesDN;
            pfrmVisUncern.m_lstPtsIdContainer = lstPtsIdContainerUncern;
            pfrmVisUncern.m_lstPtSeriesID = lstPtSeriesIDUncern;
            pfrmVisUncern.m_pRasterProps = m_pRasterProps;
            pfrmVisUncern.m_lstIDsValues = m_lstIDsValues;
            
            #endregion

            #region Draw MoranScatterplot for Entropy
            frmVisEntropy pfrmVisEntropy = new frmVisEntropy();
            Chart pMCChart = pfrmVisEntropy.pChart;

            pMCChart.ChartAreas[0].AxisX.IsStartedFromZero = false;
            pMCChart.ChartAreas[0].AxisX.IsMarginVisible = true;

            pMCChart.ChartAreas[0].AxisY.IsStartedFromZero = false;

            pfrmVisEntropy.Text = "Moran Scatter Plot of Entropy";
            pMCChart.Series.Clear();
            System.Drawing.Color pMarkerColor = System.Drawing.Color.Blue;
            var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Points",
                Color = pMarkerColor,
                BorderColor = pMarkerColor,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                MarkerSize = 1,

            };

            pMCChart.Series.Add(seriesPts);

            int intTotalCnt =  m_Entropies.Length;
            for (int j = 0; j < intTotalCnt; j++)
                seriesPts.Points.AddXY(m_Entropies[j], m_laggedEntropies[j]);

            

            var VLine = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "VLine",
                Color = System.Drawing.Color.Black,
                BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash,
                //BorderColor = System.Drawing.Color.Black,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            };
            pMCChart.Series.Add(VLine);

            VLine.Points.AddXY(m_Entropies.Average(), m_laggedEntropies.Min());
            VLine.Points.AddXY(m_Entropies.Average(), m_laggedEntropies.Max());

            var HLine = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "HLine",
                Color = System.Drawing.Color.Black,
                BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash,
                //BorderColor = System.Drawing.Color.Black,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            };
            pMCChart.Series.Add(HLine);

            HLine.Points.AddXY(m_Entropies.Min(), m_laggedEntropies.Average());
            HLine.Points.AddXY(m_Entropies.Max(), m_laggedEntropies.Average());

            pMCChart.ChartAreas[0].AxisY.Minimum = 0;

            pfrmVisEntropy.m_Entropies = m_Entropies;
            pfrmVisEntropy.m_laggedEntropies = m_laggedEntropies;
            pfrmVisEntropy.m_EntUID = EntUIDs;
            pfrmVisEntropy.m_pActiveView = m_pActiveView;
            pfrmVisEntropy.m_intLyrCnt = intLyrCnt;
            pfrmVisEntropy.m_intClsCnt = intClsCnt;
            pfrmVisEntropy.m_intTotalNSeriesDN = intTotalNSeriesDN;
            pfrmVisEntropy.m_intTotalNSeriesUncern = intTotalNSeriesUncern;
            pfrmVisEntropy.m_pDNChart = pDNChart;
            pfrmVisEntropy.m_pUncernChart = pUncernChart;
            pfrmVisEntropy.m_lstIDsValues = m_lstIDsValues;
            pfrmVisEntropy.m_pRasterProps = m_pRasterProps;
            pfrmVisEntropy.intOriPtsSizeOnLine = pDNChart.Series[lstPtSeriesIDDN[0]].MarkerSize;

            pfrmVisEntropy.Show();
            #endregion


            #region draw violin chart for uncertainty (without Classes)
            //for (int i = 0; i < intClsCnt; i++)
            //{
            //    Color FillColor = Color.FromArgb(arrClsColor[1, i], arrClsColor[2, i], arrClsColor[3, i]);

            //    List<int> lstTarget = lstNewProb[i];

            //    List<int> lstIDs = lstNewID[i];

            //    List<int> sortedTarget = new List<int>(lstTarget);

            //    //to prevent overlapping of Boxplots
            //    double dblPlotHalfWidth = 0.05;
            //    double dblMin = (dblPlotHalfWidth * (intClsCnt - 1)) * (-1);
            //    //double dblRefXvalue = i + (dblMin + (i * (dblPlotHalfWidth * 2)));

            //    double dblRefXvalue = i;

            //    //Find Max and Min
            //    double[] adblStats = BoxPlotStats(sortedTarget);
            //    bool blnDrawViolin = true;
            //    if (adblStats[0] == adblStats[4])
            //        blnDrawViolin = false;

            //    //Restrict Plot min and max
            //    if (adblStats[0] < 0)
            //        adblStats[0] = 0;
            //    if (adblStats[4] > 255)
            //        adblStats[4] = 255;

            //    string strNumbering = i.ToString();

            //    double dblXmin = dblRefXvalue - (dblPlotHalfWidth / 2);
            //    double dblXmax = dblRefXvalue + (dblPlotHalfWidth / 2);

            //    if (blnDrawViolin)
            //    {

            //        //Draw Lines
            //        AddLineSeries(pUncernChart, "m_" + strNumbering, Color.Red, 1, ChartDashStyle.Dash, dblRefXvalue - dblPlotHalfWidth, dblRefXvalue + dblPlotHalfWidth, adblStats[2], adblStats[2]);
            //        AddLineSeries(pUncernChart, "v_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblRefXvalue, dblRefXvalue, adblStats[1], adblStats[3]);
            //        //AddLineSeries(pChart, "v2_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblRefXvalue, dblRefXvalue, adblStats[3], adblStats[4]);
            //        AddLineSeries(pUncernChart, "h1_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblXmin, dblXmax, adblStats[1], adblStats[1]);
            //        AddLineSeries(pUncernChart, "h2_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblXmin, dblXmax, adblStats[3], adblStats[3]);
            //    }
            //    else
            //    {
            //        //Draw Outliers
            //        var pMedPt = new System.Windows.Forms.DataVisualization.Charting.Series
            //        {
            //            Name = "mpt_" + strNumbering,
            //            Color = Color.Red,
            //            BorderColor = FillColor,
            //            IsVisibleInLegend = false,
            //            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
            //            MarkerStyle = MarkerStyle.Circle,
            //            MarkerSize = 2,
            //        };

            //        pUncernChart.Series.Add(pMedPt);
            //        pMedPt.Points.AddXY(dblRefXvalue, adblStats[2]);
            //    }

            //    if (blnDrawViolin)
            //    {
            //        //Draw Violin Plot
            //        var pviolin = new System.Windows.Forms.DataVisualization.Charting.Series
            //        {
            //            Name = "vio1_" + strNumbering,
            //            Color = FillColor,
            //            BorderColor = FillColor,
            //            IsVisibleInLegend = false,
            //            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line

            //        };
            //        pUncernChart.Series.Add(pviolin);

            //        double[,] adblViolinStats = ViolinPlot(sortedTarget, 4);

            //        int intChartLenth = (adblViolinStats.Length) / 2;


            //        for (int k = 0; k < intChartLenth; k++)
            //        {
            //            if (adblViolinStats[k, 0] > adblStats[0] && adblViolinStats[k, 0] < adblStats[4])
            //                pviolin.Points.AddXY(dblRefXvalue - adblViolinStats[k, 1], adblViolinStats[k, 0]);
            //        }
            //        pviolin.Points.AddXY(dblRefXvalue, adblStats[4]);
            //        for (int k = intChartLenth - 1; k >= 0; k--)
            //            if (adblViolinStats[k, 0] > adblStats[0] && adblViolinStats[k, 0] < adblStats[4])
            //                pviolin.Points.AddXY(dblRefXvalue + adblViolinStats[k, 1], adblViolinStats[k, 0]);
            //        pviolin.Points.AddXY(dblRefXvalue, adblStats[0]);
            //    }



            //    //Draw Outliers
            //    var pOutlier = new System.Windows.Forms.DataVisualization.Charting.Series
            //    {
            //        Name = "out_" + strNumbering,
            //        Color = FillColor,
            //        BorderColor = FillColor,
            //        IsVisibleInLegend = false,
            //        ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
            //        MarkerStyle = MarkerStyle.Circle,
            //        MarkerSize = 1,
            //    };

            //    pUncernChart.Series.Add(pOutlier);
            //    m_lstPtSeriesID.Add(pUncernChart.Series.Count - 1); //Add Series ID for brushing and linking

            //    List<int> lstPTsIds = new List<int>();
            //    int intListCnt = lstTarget.Count;
            //    for (int k = 0; k < intListCnt; k++)
            //    {
            //        if (lstTarget[k] < adblStats[0] || lstTarget[k] > adblStats[4])
            //        {
            //            pOutlier.Points.AddXY(dblRefXvalue, lstTarget[k]);
            //            lstPTsIds.Add(lstIDs[k]);
            //            //if (m_lstIDsValues[lstIDs[k]][3 + i] != lstTarget[k])
            //            //    MessageBox.Show("ddd");
            //        }
            //    }
            //    m_lstPtsIdContainer.Add(lstPTsIds);


            //}
            //m_intTotalNSeries = pDNChart.Series.Count;

            ////Chart Setting
            //pDNChart.ChartAreas[0].AxisY.Minimum = 0;
            //pDNChart.ChartAreas[0].AxisY.Maximum = 255;

            //pDNChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            //pDNChart.ChartAreas[0].AxisY.Title = "DN and Probability";

            ////pChart.ChartAreas[0].AxisY.IsLabelAutoFit = true;
            ////if (chkAddProb.Checked)
            ////{
            ////    pChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            ////    pChart.ChartAreas[0].AxisY.Title = "DN and Probability";
            ////}
            ////else
            ////    pChart.ChartAreas[0].AxisY.Title = "DN";

            //pUncernChart.ChartAreas[0].AxisX.Maximum = intClsCnt - 0.5;
            //pUncernChart.ChartAreas[0].AxisX.Minimum = -0.5;

            //pUncernChart.ChartAreas[0].AxisX.Title = "Layers";

            //pUncernChart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            //pUncernChart.ChartAreas[0].AxisX.CustomLabels.Clear();

            //for (int j = 0; j < intClsCnt; j++)
            //{
            //    System.Windows.Forms.DataVisualization.Charting.CustomLabel pcutsomLabel = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            //    pcutsomLabel.FromPosition = j - 0.5;
            //    pcutsomLabel.ToPosition = j + 0.5;

            //    if (j < intLyrCnt)
            //        pcutsomLabel.Text = "DN of Layer " + j.ToString() + "(0-255)";
            //    else
            //        pcutsomLabel.Text = "Prob of Class " + (j - intLyrCnt).ToString() + "(0-100)";

            //    //if (j < intLyrCnt)
            //    //    pcutsomLabel.Text = "DN of " + lstLayers.CheckedItems[j].ToString() + "(0-255)";
            //    //else
            //    //    pcutsomLabel.Text = "Prob " + lstClasses.Items[j - intLyrCnt].ToString() + "(0-100)";

            //    pUncernChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
            //}


            #endregion

            #region Draw Violin Plot on one chart
            pChart.Series.Clear();
            m_intColumnCnt = intLyrCnt + intClsCnt;
            for (int i = 0; i < intClsCnt; i++)
            {
                Color FillColor = Color.FromArgb(arrClsColor[1, i], arrClsColor[2, i], arrClsColor[3, i]);

                for (int j = 0; j < m_intColumnCnt; j++)
                {
                    List<int> lstTarget;
                    if (j < intLyrCnt)
                        lstTarget = lstDN[j][i];
                    else
                        lstTarget = lstProb[j - intLyrCnt][i];

                    List<int> lstIDs;
                    if (j < intLyrCnt)
                        lstIDs = lstLyrIDs[j][i];
                    else
                        lstIDs = lstProbIDs[j - intLyrCnt][i];

                    List<int> sortedTarget = new List<int>(lstTarget);

                    //to prevent overlapping of Boxplots
                    //double dblPlotHalfWidth = 0.05;
                    double dblPlotHalfWidth = 0.08; //Adjust spacing
                    double dblMin = (dblPlotHalfWidth * (intClsCnt - 1)) * (-1);
                    double dblRefXvalue = j + (dblMin + (i * (dblPlotHalfWidth * 2)));

                    //Find Max and Min
                    double[] adblStats = BoxPlotStats(sortedTarget);
                    bool blnDrawViolin = true;
                    if (adblStats[0] == adblStats[4])
                        blnDrawViolin = false;

                    //Restrict Plot min and max
                    if (adblStats[0] < 0)
                        adblStats[0] = 0;
                    if (adblStats[4] > intMaximum)
                        adblStats[4] = intMaximum;

                    string strNumbering = i.ToString() + "_" + j.ToString();

                    double dblXmin = dblRefXvalue - (dblPlotHalfWidth / 2);
                    double dblXmax = dblRefXvalue + (dblPlotHalfWidth / 2);

                    if (blnDrawViolin)
                    {
                        //Draw Lines
                        AddLineSeries(pChart, "m_" + strNumbering, Color.Red, 1, ChartDashStyle.Dash, dblRefXvalue - dblPlotHalfWidth, dblRefXvalue + dblPlotHalfWidth, adblStats[2], adblStats[2]);
                        AddLineSeries(pChart, "v_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblRefXvalue, dblRefXvalue, adblStats[1], adblStats[3]);
                        //AddLineSeries(pChart, "v2_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblRefXvalue, dblRefXvalue, adblStats[3], adblStats[4]);
                        AddLineSeries(pChart, "h1_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblXmin, dblXmax, adblStats[1], adblStats[1]);
                        AddLineSeries(pChart, "h2_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblXmin, dblXmax, adblStats[3], adblStats[3]);
                    }
                    else
                    {
                        //Draw Outliers
                        var pMedPt = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "mpt_" + strNumbering,
                            Color = Color.Red,
                            BorderColor = FillColor,
                            IsVisibleInLegend = false,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                            MarkerStyle = MarkerStyle.Circle,
                            MarkerSize = 2,
                        };

                        pChart.Series.Add(pMedPt);
                        pMedPt.Points.AddXY(dblRefXvalue, adblStats[2]);
                    }

                    if (blnDrawViolin)
                    {
                        //Draw Violin Plot
                        var pviolin = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "vio1_" + strNumbering,
                            Color = FillColor,
                            BorderColor = FillColor,
                            IsVisibleInLegend = false,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line

                        };
                        pChart.Series.Add(pviolin);

                        double[,] adblViolinStats = ViolinPlot(sortedTarget, 4);

                        int intChartLenth = (adblViolinStats.Length) / 2;


                        for (int k = 0; k < intChartLenth; k++)
                        {
                            if (adblViolinStats[k, 0] > adblStats[0] && adblViolinStats[k, 0] < adblStats[4])
                                pviolin.Points.AddXY(dblRefXvalue - adblViolinStats[k, 1], adblViolinStats[k, 0]);
                        }
                        pviolin.Points.AddXY(dblRefXvalue, adblStats[4]);
                        for (int k = intChartLenth - 1; k >= 0; k--)
                            if (adblViolinStats[k, 0] > adblStats[0] && adblViolinStats[k, 0] < adblStats[4])
                                pviolin.Points.AddXY(dblRefXvalue + adblViolinStats[k, 1], adblViolinStats[k, 0]);
                        pviolin.Points.AddXY(dblRefXvalue, adblStats[0]);
                    }



                    //Draw Outliers
                    var pOutlier = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "out_" + strNumbering,
                        Color = FillColor,
                        BorderColor = FillColor,
                        IsVisibleInLegend = false,
                        ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                        MarkerStyle = MarkerStyle.Circle,
                        MarkerSize = 1,
                    };

                    pChart.Series.Add(pOutlier);
                    m_lstPtSeriesID.Add(pChart.Series.Count - 1); //Add Series ID for brushing and linking

                    List<int> lstPTsIds = new List<int>();
                    int intListCnt = lstTarget.Count;
                    for (int k = 0; k < intListCnt; k++)
                    {
                        if (lstTarget[k] < adblStats[0] || lstTarget[k] > adblStats[4])
                        {
                            pOutlier.Points.AddXY(dblRefXvalue, lstTarget[k]);
                            lstPTsIds.Add(lstIDs[k]);
                            //if (m_lstIDsValues[lstIDs[k]][3 + i] != lstTarget[k])
                            //    MessageBox.Show("ddd");
                        }
                    }
                    m_lstPtsIdContainer.Add(lstPTsIds);
                }

            }
            m_intTotalNSeries = pChart.Series.Count;

            //Chart Setting
            pChart.ChartAreas[0].AxisY.Minimum = 0;
            pChart.ChartAreas[0].AxisY.Maximum = intMaximum;

            pChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            pChart.ChartAreas[0].AxisY.Title = "DN and Probability";

            //pChart.ChartAreas[0].AxisY.IsLabelAutoFit = true;
            //if (chkAddProb.Checked)
            //{
            //    pChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            //    pChart.ChartAreas[0].AxisY.Title = "DN and Probability";
            //}
            //else
            //    pChart.ChartAreas[0].AxisY.Title = "DN";

            pChart.ChartAreas[0].AxisX.Maximum = m_intColumnCnt - 0.5;
            pChart.ChartAreas[0].AxisX.Minimum = -0.5;

            pChart.ChartAreas[0].AxisX.Title = "Layers";

            pChart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            pChart.ChartAreas[0].AxisX.CustomLabels.Clear();

            for (int j = 0; j < m_intColumnCnt; j++)
            {
                System.Windows.Forms.DataVisualization.Charting.CustomLabel pcutsomLabel = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
                pcutsomLabel.FromPosition = j - 0.5;
                pcutsomLabel.ToPosition = j + 0.5;

                if (j < intLyrCnt)
                    pcutsomLabel.Text = "DN of Layer " + j.ToString() + "(0-"+intMaximum.ToString()+")";
                else
                    pcutsomLabel.Text = "Prob of Class " + (j - intLyrCnt).ToString() + "(0-100)";

                //if (j < intLyrCnt)
                //    pcutsomLabel.Text = "DN of " + lstLayers.CheckedItems[j].ToString() + "(0-255)";
                //else
                //    pcutsomLabel.Text = "Prob " + lstClasses.Items[j - intLyrCnt].ToString() + "(0-100)";

                pChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
            }

            #endregion


            #region Draw Histogram of Entropy
            //double dblBinCnt = Math.Ceiling(Math.Pow(Entropies.Length, 1.0f / 3.0f) * 2.0);
            //IDataHistogram pDataHistogram = new DataHistogramClass();

            //pDataHistogram.SetData(Entropies);

            //IHistogram pHistogram = (IHistogram)pDataHistogram;

            //object xVals, frqs;
            //pHistogram.GetHistogram(out xVals, out frqs);

            //IClassifyGEN pClassifyGEN = new EqualIntervalClass();

            //int intBreakCount = Convert.ToInt32(dblBinCnt);
            //pClassifyGEN.Classify(xVals, frqs, intBreakCount);

            //double[] cb = (double[])pClassifyGEN.ClassBreaks;
            //Double[] vecMids = new Double[intBreakCount];
            //for (int j = 0; j < cb.Length - 1; j++)
            //{
            //    vecMids[j] = (cb[j + 1] + cb[j]) / 2.0F;

            //}

            //Double[] vecCounts = new Double[intBreakCount];


            //for (int j = 0; j < intTotalCnt; j++)
            //{
            //    for (int k = 0; k < intBreakCount; k++)
            //    {
            //        if (Entropies[j] >= cb[k] && Entropies[j] <= cb[k + 1])
            //            vecCounts[k]++;
            //    }
            //}

            //frmHistResults pfrmTemp = new frmHistResults();
            //pfrmTemp.Text = "Histogram of Entorpy";
            //pfrmTemp.pChart.Series.Clear();

            //var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
            //{
            //    Name = "Series1",
            //    Color = System.Drawing.Color.White,
            //    BorderColor = System.Drawing.Color.Black,
            //    IsVisibleInLegend = false,
            //    IsXValueIndexed = true,
            //    ChartType = SeriesChartType.Column,

            //};

            //pfrmTemp.pChart.Series.Add(series1);

            //int intNBins = vecMids.Length;

            //for (int j = 0; j < intNBins; j++)
            //{
            //    series1.Points.AddXY(vecMids[j], vecCounts[j]);
            //}

            //pfrmTemp.pChart.Series[0]["PointWidth"] = "1";
            //pfrmTemp.pChart.ChartAreas[0].AxisX.Title = "Entropy";
            //pfrmTemp.pChart.ChartAreas[0].AxisY.Title = "Frequency";

            //pfrmTemp.pChart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;

            //pfrmTemp.dblBreaks = cb;
            //pfrmTemp.m_pForm = m_pForm;
            //pfrmTemp.pActiveView = m_pActiveView;
            ////pfrmTemp.pFLayer = pFLayer;
            ////pfrmTemp.strFieldName = strFieldName;
            //pfrmTemp.intNBins = intNBins;
            //pfrmTemp.vecCounts = vecCounts;
            //pfrmTemp.vecMids = vecMids;

            //double dblInterval = cb[1] - cb[0];
            //int intDecimalPlaces = 1;
            //string strDecimalPlaces = "N" + intDecimalPlaces.ToString();
            //for (int n = 0; n < cb.Length; n++)
            //{
            //    CustomLabel pcutsomLabel = new CustomLabel();
            //    pcutsomLabel.FromPosition = n;
            //    pcutsomLabel.ToPosition = n + 1;
            //    pcutsomLabel.Text = cb[n].ToString(strDecimalPlaces);

            //    pfrmTemp.pChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
            //}

            //pfrmTemp.Show();

            #endregion
            pfrmProgress.Close();
      
        }
        public static void CreateRasterDataset3(string Path, string FileName, ISpatialReference sr, double dblXmin, double dblYmin, int intXCount, int intYCount, double dblCellSize, float[,] arrRKernel)
        {
            //try
            //{
            //Create raster workspace. This example also works with any other workspaces that support raster data such as a file geodatabase (FGDB) workspace.
            //Access the workspace and the SDE workspace.
            IRasterWorkspace2 rasterWs = OpenRasterWorkspace(Path);
            //Define the spatial reference of the raster dataset.

            //Define the origin for the raster dataset, which is the lower left corner of the raster.
            IPoint origin = new PointClass();
            origin.PutCoords(dblXmin, dblYmin);
            //Define the dimension of the raster dataset.
            int NumBand = 1; // This is the number of bands the raster dataset contains.

            //Create a raster dataset in grid format.
            IRasterDataset rasterDataset = rasterWs.CreateRasterDataset(FileName + "tp",
              "GRID", origin, intXCount, intYCount, dblCellSize, dblCellSize, NumBand,
              rstPixelType.PT_FLOAT, sr, true); // Float Type raster
            //Get the raster band.
            IRasterBandCollection rasterBands = (IRasterBandCollection)rasterDataset;
            IRasterBand rasterBand;
            IRasterProps rasterProps;
            rasterBand = rasterBands.Item(0);
            rasterProps = (IRasterProps)rasterBand;
            //Set NoData if necessary. For a multiband image, NoData value needs to be set for each band.
            rasterProps.NoDataValue = -999;
            //Create a raster from the dataset.
            IRaster raster = rasterDataset.CreateDefaultRaster();

            //Create a pixel block.
            IPnt blocksize = new PntClass();
            blocksize.SetCoords(intXCount, intYCount);
            IPixelBlock3 pixelblock = raster.CreatePixelBlock(blocksize) as IPixelBlock3;

            //Populate some pixel values to the pixel block.
            System.Array pixels;
            pixels = (System.Array)pixelblock.get_PixelData(0);
            for (int i = 0; i < intXCount; i++)
            {
                for (int j = 0; j < intYCount; j++)
                {
                    //pixels.SetValue(Convert.ToByte(arrRKernel[i, (intYCount - 1) - j]), i, j); // Transpose array to Raster
                    pixels.SetValue(Convert.ToSingle(arrRKernel[i, j]), i, j); // Transpose array to Raster
                }
            }
            pixelblock.set_PixelData(0, (System.Array)pixels);


            //Define the location that the upper left corner of the pixel block is to write.
            IPnt upperLeft = new PntClass();
            upperLeft.SetCoords(0, 0);

            //Write the pixel block.
            IRasterEdit rasterEdit = (IRasterEdit)raster;
            rasterEdit.Write(upperLeft, (IPixelBlock)pixelblock);

            //Release rasterEdit explicitly.
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rasterEdit);
            ISaveAs pSaveAs = rasterDataset as ISaveAs;
            pSaveAs.SaveAs(FileName, (IWorkspace)rasterWs, "GRID");

            //    IRasterDataset pRasterDataset = pKernelResult.RasterDataset;
            IDataset pDataSetforTempRaster = rasterDataset as IDataset;
            pDataSetforTempRaster.Delete();

            //IRasterDataset pRstDs = rasterWs.OpenRasterDataset(FileName);

            //return pRstDs;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    return null;
            //}
        }
        public static IRasterDataset CreateRasterDataset2(string Path, string FileName, ISpatialReference sr, double dblXmin, double dblYmin, int intXCount, int intYCount, double dblCellSize, int[,] arrRKernel)
        {
            //try
            //{
            //Create raster workspace. This example also works with any other workspaces that support raster data such as a file geodatabase (FGDB) workspace.
            //Access the workspace and the SDE workspace.
            IRasterWorkspace2 rasterWs = OpenRasterWorkspace(Path);
            //Define the spatial reference of the raster dataset.

            //Define the origin for the raster dataset, which is the lower left corner of the raster.
            IPoint origin = new PointClass();
            origin.PutCoords(dblXmin, dblYmin);
            //Define the dimension of the raster dataset.
            int NumBand = 1; // This is the number of bands the raster dataset contains.

            //Create a raster dataset in grid format.
            IRasterDataset rasterDataset = rasterWs.CreateRasterDataset(FileName + "tp",
              "GRID", origin, intXCount, intYCount, dblCellSize, dblCellSize, NumBand,
              rstPixelType.PT_UCHAR, sr, true); // Float Type raster
            //Get the raster band.
            IRasterBandCollection rasterBands = (IRasterBandCollection)rasterDataset;
            IRasterBand rasterBand;
            IRasterProps rasterProps;
            rasterBand = rasterBands.Item(0);
            rasterProps = (IRasterProps)rasterBand;
            //Set NoData if necessary. For a multiband image, NoData value needs to be set for each band.
            rasterProps.NoDataValue = -999;
            //Create a raster from the dataset.
            IRaster raster = rasterDataset.CreateDefaultRaster();

            //Create a pixel block.
            IPnt blocksize = new PntClass();
            blocksize.SetCoords(intXCount, intYCount);
            IPixelBlock3 pixelblock = raster.CreatePixelBlock(blocksize) as IPixelBlock3;

            //Populate some pixel values to the pixel block.
            System.Array pixels;
            pixels = (System.Array)pixelblock.get_PixelData(0);
            for (int i = 0; i < intXCount; i++)
            {
                for (int j = 0; j < intYCount; j++)
                {
                    //pixels.SetValue(Convert.ToByte(arrRKernel[i, (intYCount - 1) - j]), i, j); // Transpose array to Raster
                    pixels.SetValue(Convert.ToByte(arrRKernel[i, j]), i, j); // Transpose array to Raster
                }
            }
            pixelblock.set_PixelData(0, (System.Array)pixels);


            //Define the location that the upper left corner of the pixel block is to write.
            IPnt upperLeft = new PntClass();
            upperLeft.SetCoords(0, 0);

            //Write the pixel block.
            IRasterEdit rasterEdit = (IRasterEdit)raster;
            rasterEdit.Write(upperLeft, (IPixelBlock)pixelblock);

            //Release rasterEdit explicitly.
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rasterEdit);
            ISaveAs pSaveAs = rasterDataset as ISaveAs;
            pSaveAs.SaveAs(FileName, (IWorkspace)rasterWs, "GRID");

            //    IRasterDataset pRasterDataset = pKernelResult.RasterDataset;
            IDataset pDataSetforTempRaster = rasterDataset as IDataset;
            pDataSetforTempRaster.Delete();

            IRasterDataset pRstDs = rasterWs.OpenRasterDataset(FileName);

            return pRstDs;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    return null;
            //}
        }
        public static IRasterWorkspace2 OpenRasterWorkspace(string PathName)
        {
            //This function opens a raster workspace.
            try
            {
                IWorkspaceFactory workspaceFact = new RasterWorkspaceFactoryClass();
                return workspaceFact.OpenFromFile(PathName, 0) as IRasterWorkspace2;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        private double[,] ViolinPlot(List<int> plstTarget, double dblBandwidth)
        {
            plstTarget.Sort();
            int intLength = plstTarget.Count;
            int intMax = plstTarget[intLength - 1];
            int intMin = plstTarget[0];
            int intRange = intMax - intMin;

            double[,] adblResult = new double[intRange + 1, 2];

            for (int i = 0; i < (intRange + 1); i++)
            {
                int intX = intMin + i;
                adblResult[i, 0] = intX;

                int j = 0;
                double dblU = (intX - plstTarget[j]) / dblBandwidth;
                double dblK = 0;

                while (dblU > -1) //Apply While to improve efficienty, it can apply only sorted lst or arry
                {
                    //Apply Epanechnikov
                    if (dblU < 1)
                    {
                        dblK += 0.75 * (1 - Math.Pow(dblU, 2));
                    }
                    j++;
                    if (j < intLength)
                        dblU = (intX - plstTarget[j]) / dblBandwidth;
                    else
                        dblU = -1;
                }
                //adblResult[i, 1] = dblK / (dblBandwidth * intLength);
                adblResult[i, 1] = dblK / (dblBandwidth * intLength)*1.5;//Highlight difference for presentation 1011_HK
            }

            return adblResult;

        }

        private double[] BoxPlotStats(List<int> plstTarget)
        {
            plstTarget.Sort();

            double[] adblStats = new double[5];
            //double[] BPStats = new double[5];

            //adblStats[0] = adblTarget.Min();
            //adblStats[4] = adblTarget.Max();
            int intLength = plstTarget.Count;

            adblStats[2] = GetMedian(plstTarget);
            //Get 1st and 3rd Quantile
            if (intLength % 2 == 0)
            {
                int newLength = intLength / 2;
                List<int> lowSubset = plstTarget.GetRange(0, newLength);
                List<int> upperSubset = plstTarget.GetRange(newLength, newLength);

                adblStats[1] = GetMedian(lowSubset);
                adblStats[3] = GetMedian(upperSubset);
            }
            else
            {
                int newLength = (intLength - 1) / 2;
                List<int> lowSubset = plstTarget.GetRange(0, newLength);
                List<int> upperSubset = plstTarget.GetRange(newLength + 1, newLength);

                adblStats[1] = GetMedian(lowSubset);
                adblStats[3] = GetMedian(upperSubset);

            }
            double dblIQR = adblStats[3] - adblStats[1];
            adblStats[0] = adblStats[1] - (1.5 * dblIQR);
            adblStats[4] = adblStats[3] + (1.5 * dblIQR);
            return adblStats;
        }

        private double GetMedian(List<int> sortedArray)
        {
            double dblMedian = 0;
            int intLength = sortedArray.Count;

            if (intLength % 2 == 0)
            {
                // count is even, average two middle elements
                double a = sortedArray[intLength / 2 - 1];
                double b = sortedArray[intLength / 2];
                return dblMedian = (a + b) / 2;
            }
            else
            {
                // count is odd, return the middle element
                return dblMedian = sortedArray[(intLength - 1) / 2];
            }
        }

        private void AddLineSeries(Chart pChart, string strSeriesName, System.Drawing.Color FillColor, int intWidth, ChartDashStyle BorderDash, double dblXMin, double dblXMax, double dblYMin, double dblYMax)
        {
            //try
            //{
            var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = strSeriesName,
                Color = FillColor,
                //BorderColor = Color.Black,
                BorderWidth = intWidth,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                BorderDashStyle = BorderDash,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

            };

            pChart.Series.Add(pSeries);

            pSeries.Points.AddXY(dblXMin, dblYMin);
            pSeries.Points.AddXY(dblXMax, dblYMax);
            //}
            //catch (Exception ex)
            //{
            //    frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
            //    return;
            //}

        }

        private void pChart_MouseDown(object sender, MouseEventArgs e)
        {
            _canDraw = true;
            _startX = e.X;
            _startY = e.Y;
        }

        private void pChart_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (!_canDraw) return;

                int x = Math.Min(_startX, e.X);
                int y = Math.Min(_startY, e.Y);

                int width = Math.Max(_startX, e.X) - Math.Min(_startX, e.X);

                int height = Math.Max(_startY, e.Y) - Math.Min(_startY, e.Y);
                _rect = new Rectangle(x, y, width, height);
                Refresh();

                Pen pen = new Pen(Color.Cyan, 1);
                pGraphics = pChart.CreateGraphics();
                pGraphics.DrawRectangle(pen, _rect);
                pGraphics.Dispose();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void pChart_MouseUp(object sender, MouseEventArgs e)
        {
            //Clear previous selection
            m_pActiveView.GraphicsContainer.DeleteAllElements();
            while (pChart.Series.Count != m_intTotalNSeries)
                pChart.Series.RemoveAt(pChart.Series.Count - 1);

            _canDraw = false;

            HitTestResult result = pChart.HitTest(e.X, e.Y);



            int dblOriPtsSize = pChart.Series[m_lstPtSeriesID[0]].MarkerSize;
            int intTotalSriCount = m_lstPtSeriesID.Count;

            int intSelLinesCount = 0;

            List<int> lstColIdx = new List<int>();
            List<int> lstRowIdx = new List<int>();

            for (int i = 0; i < intTotalSriCount; i++)
            {
                int intSeriesID = m_lstPtSeriesID[i];
                int intPtsCount = pChart.Series[intSeriesID].Points.Count;

                for (int j = 0; j < intPtsCount; j++)
                {
                    int intX = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(pChart.Series[intSeriesID].Points[j].XValue);
                    int intY = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(pChart.Series[intSeriesID].Points[j].YValues[0]);

                    System.Drawing.Point SelPts = new System.Drawing.Point(intX, intY);

                    if (_rect.Contains(SelPts))
                    {
                        double dblXOffset = (pChart.Series[intSeriesID].Points[j].XValue) - Math.Round(pChart.Series[intSeriesID].Points[j].XValue, 0);
                        System.Drawing.Color pMarkerColor = System.Drawing.Color.Cyan;
                        var seriesLines = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "SelLines" + intSelLinesCount.ToString(),
                            Color = pMarkerColor,
                            BorderColor = pMarkerColor,
                            IsVisibleInLegend = false,
                            IsXValueIndexed = false,
                            //ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                            MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                            MarkerSize = dblOriPtsSize * 2

                        };
                        intSelLinesCount++;
                        pChart.Series.Add(seriesLines);

                        int intSelLocationIdx = m_lstPtsIdContainer[i][j];
                        int[] dblSelValues = m_lstIDsValues[intSelLocationIdx];
                        for (int k = 0; k < m_intColumnCnt; k++)
                        {
                            int intYvalue = m_lstIDsValues[intSelLocationIdx][3 + k];
                            seriesLines.Points.AddXY(k + dblXOffset, intYvalue);
                        }

                        DrawPointsOnActiveView(m_lstIDsValues[intSelLocationIdx][0], m_lstIDsValues[intSelLocationIdx][1], m_pRasterProps, m_pActiveView);
                        lstColIdx.Add(m_lstIDsValues[intSelLocationIdx][0]);
                        lstRowIdx.Add(m_lstIDsValues[intSelLocationIdx][1]);
                    }

                }
            }
            frmVisEntropy pfrmVisEntropy = System.Windows.Forms.Application.OpenForms["frmVisEntropy"] as frmVisEntropy;
            BrushPointsOnMCPlot(lstColIdx, lstRowIdx, m_pRasterProps, pfrmVisEntropy, m_Entropies, m_laggedEntropies);
            
            if (intSelLinesCount == 0)
            {
                m_pActiveView.GraphicsContainer.DeleteAllElements();
                m_pActiveView.Refresh();
            }
        }
        private void BrushPointsOnMCPlot(List<int> colindex, List<int> rowindex, IRasterProps pRasterProps, frmVisEntropy pfrmVisEntropy, double[] Entropies, double[] laggedEntropies)
        {
            
            int intLength = colindex.Count;
            Chart pVisEntChart = pfrmVisEntropy.pChart;
            int intLastSeriesIdx = pVisEntChart.Series.Count - 1;

            //Remove Previous Selection
            if (pVisEntChart.Series[intLastSeriesIdx].Name == "SelPoints")
                pVisEntChart.Series.RemoveAt(intLastSeriesIdx);

            if (intLength == 0)
                return;

            int dblOriPtsSize = pVisEntChart.Series[0].MarkerSize;

            System.Drawing.Color pMarkerColor = System.Drawing.Color.Cyan;
            var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "SelPoints",
                Color = pMarkerColor,
                BorderColor = pMarkerColor,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                MarkerSize = dblOriPtsSize * 2

            };

            pVisEntChart.Series.Add(seriesPts);

            for (int i = 0; i < intLength; i++)
            {
                int intSelID = (colindex[i] * pRasterProps.Height) + rowindex[i];
                seriesPts.Points.AddXY(Entropies[intSelID], laggedEntropies[intSelID]);
            }

        }
        private void DrawPointsOnActiveView(int colindex, int rowindex, IRasterProps pRasterProps, IActiveView ActiveView)
        {
            IGraphicsContainer pGraphicContainer = ActiveView.GraphicsContainer;

            IRgbColor pRgbColor = m_pSnippet.getRGB(0, 255, 255);

            //ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
            //pSimpleLineSymbol.Width = 2;
            //pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            //pSimpleLineSymbol.Color = pRgbColor;

            ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
            pSimpleMarkerSymbol.Size = 8;
            pSimpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            pSimpleMarkerSymbol.Color = pRgbColor;

            double dblX = 0, dblY = 0;
            double dblCellSize = Convert.ToDouble(pRasterProps.MeanCellSize().X);
            dblX = pRasterProps.Extent.XMin + (dblCellSize / 2) + dblCellSize * colindex;
            dblY = pRasterProps.Extent.YMax - (dblCellSize / 2) - dblCellSize * rowindex;

            IPoint pPoint = new PointClass();
            pPoint.X = dblX;
            pPoint.Y = dblY;

            IElement pElement = new MarkerElementClass();

            IMarkerElement pMarkerElement = (IMarkerElement)pElement;
            pMarkerElement.Symbol = pSimpleMarkerSymbol;
            pElement.Geometry = pPoint;

            pGraphicContainer.AddElement(pElement, 0);

            ActiveView.Refresh();
            //for (int i = 0; i < intLstCnt; i++)
            //{
            //    int intIdx = lstIndices[i];
            //    double[] arrSelValue = arrValue[intIdx];
            //    //drawing a polyline
            //    IPoint FromP = new PointClass();
            //    FromP.X = arrSelValue[0]; FromP.Y = arrSelValue[1];

            //    IPoint ToP = new PointClass();
            //    ToP.X = arrSelValue[2]; ToP.Y = arrSelValue[3];

            //    IPolyline polyline = new PolylineClass();
            //    IPointCollection pointColl = polyline as IPointCollection;
            //    pointColl.AddPoint(FromP);
            //    pointColl.AddPoint(ToP);

            //    IElement pElement = new LineElementClass();

            //    ILineElement pLineElement = (ILineElement)pElement;
            //    pLineElement.Symbol = pSimpleLineSymbol;
            //    pElement.Geometry = polyline;

            //    pGraphicContainer.AddElement(pElement, 0);
            //}

            //pActiveView.Refresh();

        }
        
    }
}
