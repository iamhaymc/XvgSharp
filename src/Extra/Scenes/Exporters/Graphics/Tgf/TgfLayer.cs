using System;
using System.IO;
using System.Drawing;
using System.Collections;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TgfLayer.
  /// </summary>
  public class TgfLayer
  {
    private int m_crColor;
    private int m_nLineWeight;
    private int m_nLayerType;
    private ArrayList m_TgfLineArray;
    private ArrayList m_TgfPolylineArray;
    private ArrayList m_TgfEllipseArray;
    private ArrayList m_TgfArcArray;
    private ArrayList m_TgfTextArray;
    private Pen m_layerPen;
    private TgfFile m_TgfFile;

    public TgfLayer(TgfFile parentFile)
    {
      m_TgfFile = parentFile;
      m_TgfLineArray = new ArrayList();
      m_TgfPolylineArray = new ArrayList();
      m_TgfEllipseArray = new ArrayList();
      m_TgfArcArray = new ArrayList();
      m_TgfTextArray = new ArrayList();
    }

    //TGH 033108
    public void ClearAll()
    {
      m_TgfLineArray.Clear();
      m_TgfPolylineArray.Clear();
      m_TgfEllipseArray.Clear();
      m_TgfArcArray.Clear();
      m_TgfTextArray.Clear();
    }
    //--

    public TgfFile ParentTgfFile
    {
      get { return m_TgfFile; }
    }
    // renamed from GetLineWeight & SetLineWeight
    public int LineWeight
    {
      get { return m_nLineWeight; }
      set { m_nLineWeight = value; }
    }
    // renamed from GetColor & SetColor
    public int LayerColor
    {
      get { return m_crColor; }
      set { m_crColor = value; }
    }
    public Pen LayerPen
    {
      get { return m_layerPen; }
    }
    // renamed from SetLayerType
    public int LayerType
    {
      get { return m_nLayerType; }
      set { m_nLayerType = value; }
    }

    public void CreatePen()
    {
      //m_layerPen = new Pen(Color.FromArgb(LayerColor), LineWeight);
      //			m_layerPen = new Pen(Color.FromArgb((Int32)(4278190080 + LayerColor)), LineWeight*3);
      m_layerPen = new Pen(Color.FromArgb((int)(4278190080 + LayerColor)), (LineWeight / m_TgfFile.ScalingFactor) * 3.3f);
      //m_layerPen = new Pen(Color.FromArgb((0,0,0), LineWeight);
    }

    public int GetNumEntities()
    {
      return m_TgfLineArray.Count + m_TgfPolylineArray.Count + m_TgfEllipseArray.Count + m_TgfArcArray.Count + m_TgfTextArray.Count;
    }

    public bool AddLine(double fStartX, double fStartY, double fEndX, double fEndY, int nLineType, double fLtScale)
    {
      TgfLine tgfLine = new TgfLine(this);
      tgfLine.SetCoordinates(fStartX, fStartY, fEndX, fEndY, nLineType, fLtScale);
      m_TgfLineArray.Add(tgfLine);
      return true;
    }
    //public bool AddPolyline(double* pfX, double* pfY, int nCount)
    public bool AddPolyline(double[] fX, double[] fY, int nCount)
    {
      TgfPolyline tgfPolyline = new TgfPolyline(this);
      tgfPolyline.SetCoordinates(fX, fY, nCount);
      m_TgfPolylineArray.Add(tgfPolyline);
      return true;
    }
    public bool AddEllipse(double fUpperLeftX, double fUpperLeftY, double fLowerRightX, double fLowerRightY, double fRotationAngle)
    {
      TgfEllipse tgfEllipse = new TgfEllipse(this);
      tgfEllipse.SetCoordinates(fUpperLeftX, fUpperLeftY, fLowerRightX, fLowerRightY, fRotationAngle);
      m_TgfEllipseArray.Add(tgfEllipse);
      return true;
    }
    //public bool AddArc(double fCenterX, double fCenterY, double fRadius, double fStartAngle, double fSweepAngle, int nLineType, double fLtScale)
    public bool AddArc(double fCenterX, double fCenterY, double fRadius, double fStartAngle, double fSweepAngle, short nLineType, double fLtScale)
    {
      TgfArc tgfArc = new TgfArc(this);
      tgfArc.SetCoordinates(fCenterX, fCenterY, fRadius, fStartAngle, fSweepAngle, nLineType, fLtScale);
      m_TgfArcArray.Add(tgfArc);
      return true;
    }
    //public bool AddText(int nFontType, double fFontSize, int nStyle, int nAlignment, double fOrientation, double fStartX, double fStartY, LPCTSTR szTextString, int nNoteType = 0, long nId = 0)
    public bool AddText(int nFontType, double fFontSize, int nStyle, int nAlignment, double fOrientation, double fStartX, double fStartY, string szTextString, int nNoteType, long nId)
    {
      TgfText tgfText = new TgfText(this);
      tgfText.TextFontType = nFontType;
      tgfText.TextFontSize = fFontSize;
      tgfText.TextFontStyle = nStyle;
      tgfText.TextAlignment = nAlignment;
      tgfText.TextOrientation = fOrientation;
      tgfText.SetCoordinates(fStartX, fStartY);
      if (szTextString != null && szTextString.Length > 0)
        tgfText.TextString = szTextString;
      tgfText.NoteType = nNoteType;
      tgfText.NoteId = nId;
      if (nNoteType == 1)   // (note)
      {
        m_TgfFile.SetNoteText((int)nId, szTextString);
      }
      else if (nNoteType == 2)  // (note list)
      {
        m_TgfFile.SetNoteListText((int)nId, string.Empty);
      }
      m_TgfTextArray.Add(tgfText);
      return true;
    }

    //tgfFile.Write(String.Format("L{0:d}{1:d}{2:d5}{3:d5}{4:d5}{5:d5}{6:d5}\n", nLayerNdx, m_nLineType, (int)(m_fStartX*10000), (int)(m_fStartY*10000), (int)(m_fEndX*10000), (int)(m_fEndY*100), (int)(m_fLtScale*10000)));
    //private bool ReadLine(char* szBuf)
    public bool ReadLine(string szBuf)
    {
      bool bRetVal;
      double fStartX;
      double fStartY;
      double fEndX;
      double fEndY;
      double fLtScale = 1.0;
      int nLineType;
      //char		szTemp[6];

      // set the Line Type
      //			strncpy(szTemp, szBuf, 1);
      //			szTemp[1] = NULL;
      //			nLineType = atoi(szTemp);
      //			szBuf++;
      nLineType = int.Parse(szBuf.Substring(2, 1));

      // set the Start X coordinate
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fStartX = atoi(szTemp) / 10000.0;
      //			szBuf += 5;
      fStartX = int.Parse(szBuf.Substring(3, 5)) / 10000.0;

      // set the Start Y coordinate
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fStartY = atoi(szTemp) / 10000.0;
      //			szBuf += 5;
      fStartY = int.Parse(szBuf.Substring(8, 5)) / 10000.0;

      // set the End X coordinate
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fEndX = atoi(szTemp) / 10000.0;
      //			szBuf += 5;
      fEndX = int.Parse(szBuf.Substring(13, 5)) / 10000.0;

      // set the End Y coordinate
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fEndY = atoi(szTemp) / 10000.0;
      //			szBuf += 5;
      fEndY = int.Parse(szBuf.Substring(18, 5)) / 10000.0;

      // set the line type scaling factor scale
      fLtScale = int.Parse(szBuf.Substring(23, 5)) / 10000.0;

      //			// if line type is not continuous, check to see if a line type scaling factor is included
      //			if (nLineType != (int)LINETYPES.TGF_CONTINUOUSLT)
      //			{
      //				// find the end of the command
      //				char* ptr1 = strchr(szBuf, '\n');
      //				if (ptr1)
      //				{
      //					if (ptr1 == szBuf + 5)
      //					{
      //						// set the LineType scale
      //						strncpy(szTemp, szBuf, 5);
      //						szTemp[5] = NULL;
      //						fLtScale = atoi(szTemp) / 10000.0;
      //					}
      //				}
      //			}

      // create a new line and add it to the layer
      bRetVal = AddLine(fStartX, fStartY, fEndX, fEndY, nLineType, fLtScale);

      return bRetVal;
    }
    //tgfFile.Write(String.Format("P{0:d}{1:d3}", nLayerNdx, m_nCount));
    //repeating...
    //		tgfFile.Write(String.Format("{0:d5}{1:d5}", (int)(m_fX[i]*10000), (int)(m_fY[i]*10000)));
    //tgfFile.Write(String.Format("\n"));
    //private bool ReadPolyline(char* szBuf)
    public bool ReadPolyline(string szBuf)
    {
      bool bRetVal;
      int nCount;
      //			char		szTemp[6];

      // set the count
      //			strncpy(szTemp, szBuf, 3);
      //			szTemp[3] = NULL;
      //			nCount = atoi(szTemp);
      //			szBuf += 3;
      nCount = int.Parse(szBuf.Substring(2, 3));

      // create arrays to hold the x & y values
      double[] fX = new double[nCount];
      double[] fY = new double[nCount];

      int charPos = 5;
      for (int i = 0; i < nCount; i++)
      {
        //				strncpy(szTemp, szBuf, 5);
        //				szTemp[5] = NULL;
        //				pfX[i] = atoi(szTemp) / 10000.0;
        //				szBuf += 5;
        fX[i] = int.Parse(szBuf.Substring(charPos, 5)) / 10000.0;
        charPos += 5;
        //				strncpy(szTemp, szBuf, 5);
        //				szTemp[5] = NULL;
        //				pfY[i] = atoi(szTemp) / 10000.0;
        //				szBuf += 5;
        fY[i] = int.Parse(szBuf.Substring(charPos, 5)) / 10000.0;
        charPos += 5;
      }

      bRetVal = AddPolyline(fX, fY, nCount);
      //			delete pfX;
      //			delete pfY;

      return bRetVal;
    }
    //tgfFile.Write(String.Format("A{0:d}{1:d}{2:d8}{3:d8}{4:d7}{5:d5}{6:d5}{7:d5}\n", nLayerNdx, m_nLineType, (int)(m_fCenterX*10000), (int)(m_fCenterY*10000), (int)(m_fRadius*10000), (int)(m_fStartAngle*100), (int)(m_fSweepAngle*100), (int)(m_fLtScale*10000)));
    //private bool ReadArc(char* szBuf)
    public bool ReadArc(string szBuf)
    {
      bool bRetVal;
      double fCenterX;
      double fCenterY;
      double fRadius;
      double fStartAngle;
      double fSweepAngle;
      double fLtScale = 1;
      short nLineType;
      //			char		szTemp[9];

      // set the Line Type
      //			strncpy(szTemp, szBuf, 1);
      //			szTemp[1] = NULL;
      //			nLineType = atoi(szTemp);
      //			szBuf++;
      nLineType = short.Parse(szBuf.Substring(2, 1));

      // set the Center X coordinate
      //			strncpy(szTemp, szBuf, 8);
      //			szTemp[8] = NULL;
      //			fCenterX = atol(szTemp) / 10000.0;
      //			szBuf += 8;
      fCenterX = int.Parse(szBuf.Substring(3, 8)) / 10000.0;

      // set the Center Y coordinate
      //			strncpy(szTemp, szBuf, 8);
      //			szTemp[8] = NULL;
      //			fCenterY = atol(szTemp) / 10000.0;
      //			szBuf += 8;
      fCenterY = int.Parse(szBuf.Substring(11, 8)) / 10000.0;

      // set the radius
      //			strncpy(szTemp, szBuf, 7);
      //			szTemp[7] = NULL;
      //			fRadius = atol(szTemp) / 10000.0;
      //			szBuf += 7;
      fRadius = int.Parse(szBuf.Substring(19, 7)) / 10000.0;

      // set the start angle
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fStartAngle = atoi(szTemp) / 100.0;
      //			szBuf += 5;
      fStartAngle = int.Parse(szBuf.Substring(26, 5)) / 100.0;

      // set the sweep angle
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fSweepAngle = atoi(szTemp) / 100.0;
      //			szBuf += 5;
      fSweepAngle = int.Parse(szBuf.Substring(31, 5)) / 100.0;

      //			// if line type is not continuous, check to see if a line type scaling factor is included
      //			if (nLineType != (int)LINETYPES.TGF_CONTINUOUSLT)
      //			{
      //				// find the end of the command
      //				char* ptr1 = strchr(szBuf, '\n');
      //				if (ptr1)
      //				{
      //					if (ptr1 == szBuf + 5)
      //					{
      //						// set the End Y coordinate
      //						strncpy(szTemp, szBuf, 5);
      //						szTemp[5] = NULL;
      //						fLtScale = atoi(szTemp) / 10000.0;
      //					}
      //				}
      //			}
      // set the line type scaling factor scale
      fLtScale = int.Parse(szBuf.Substring(36, 5)) / 10000.0;

      // create a new arc and add it to the layer
      bRetVal = AddArc(fCenterX, fCenterY, fRadius, fStartAngle, fSweepAngle, nLineType, fLtScale);

      return bRetVal;
    }
    //tgfFile.Write(String.Format("E{0:d}{1:d5}{2:d5}{3:d5}{4:d5}{5:d5}\n", nLayerNdx, (int)(m_fUpperLeftX*10000), (int)(m_fUpperLeftY*10000), (int)(m_fLowerRightX*10000), (int)(m_fLowerRightY*10000), (int)(m_fRotationAngle*100)));
    //private bool ReadEllipse(char* szBuf)
    public bool ReadEllipse(string szBuf)
    {
      bool bRetVal;
      double fUpperLeftX;
      double fUpperLeftY;
      double fLowerRightX;
      double fLowerRightY;
      double fRotationAngle;
      //			char		szTemp[6];

      // set the upper left X coordinate
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fUpperLeftX = atoi(szTemp) / 10000.0;
      //			szBuf += 5;
      fUpperLeftX = int.Parse(szBuf.Substring(2, 5)) / 10000.0;

      // set the upper left Y coordinate
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fUpperLeftY = atoi(szTemp) / 10000.0;
      //			szBuf += 5;
      fUpperLeftY = int.Parse(szBuf.Substring(7, 5)) / 10000.0;

      // set the lower right X coordinate
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fLowerRightX = atoi(szTemp) / 10000.0;
      //			szBuf += 5;
      fLowerRightX = int.Parse(szBuf.Substring(12, 5)) / 10000.0;

      // set the lower right Y coordinate
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fLowerRightY = atoi(szTemp) / 10000.0;
      //			szBuf += 5;
      fLowerRightY = int.Parse(szBuf.Substring(17, 5)) / 10000.0;

      // set the rotation angle
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fRotationAngle = atoi(szTemp) / 100.0;
      fRotationAngle = int.Parse(szBuf.Substring(22, 5)) / 100.0;

      // create a new ellipse and add it to the layer
      bRetVal = AddEllipse(fUpperLeftX, fUpperLeftY, fLowerRightX, fLowerRightY, fRotationAngle);

      return bRetVal;
    }
    //type 0 - tgfFile.Write(String.Format("T{0:d}{1:d}{2:d4}{3:d2}{4:d}{5:d5}{6:d5}{7:d5}{8}\n", nLayerNdx, m_nFontType, (int)(m_fFontSize*10000), m_nFontStyle, m_nAlignment, (int)(m_fOrientation*100), (int)(m_fStartX*10000), (int)(m_fStartY*10000), m_szTextString));
    //type 1 - tgfFile.Write(String.Format("S{0:d}{1:d}{2:d4}{3:d2}{4:d}{5:d5}{6:d5}{7:d5}{8:d8}{9}\n", nLayerNdx, m_nFontType, (int)(m_fFontSize*10000), m_nFontStyle, m_nAlignment, (int)(m_fOrientation*100), (int)(m_fStartX*10000), (int)(m_fStartY*10000), m_nId, m_szTextString));
    //type 2 - tgfFile.Write(String.Format("D{0:d}{1:d}{2:d4}{3:d2}{4:d}{5:d5}{6:d5}{7:d5}{8:d8}\n", nLayerNdx, m_nFontType, (int)(m_fFontSize*10000), m_nFontStyle, m_nAlignment, (int)(m_fOrientation*100), (int)(m_fStartX*10000), (int)(m_fStartY*10000), m_nId));
    //private bool ReadText(char* szBuf, int nType)
    public bool ReadText(string szBuf, int nType)
    {
      bool bRetVal = true;
      int nFontType;
      double fFontSize;
      int nFontStyle;
      int nAlignment;
      double fOrientation;
      double fStartX;
      double fStartY;
      string szTextString;
      long nId;
      //			char		szTemp[9];
      //			char*		ptr1;

      // set the Font Type
      //			strncpy(szTemp, szBuf, 1);
      //			szTemp[1] = NULL;
      //			nFontType = atoi(szTemp);
      //			szBuf++;
      nFontType = int.Parse(szBuf.Substring(2, 1));

      // set the font size
      //			strncpy(szTemp, szBuf, 4);
      //			szTemp[4] = NULL;
      //			fFontSize = atoi(szTemp) / 10000.0;
      //			szBuf += 4;
      fFontSize = int.Parse(szBuf.Substring(3, 4)) / 10000.0;

      // set the Font Type
      //			strncpy(szTemp, szBuf, 2);
      //			szTemp[2] = NULL;
      //			nFontStyle = atoi(szTemp);
      //			szBuf += 2;
      nFontStyle = int.Parse(szBuf.Substring(7, 2));

      // set the alignment
      //			strncpy(szTemp, szBuf, 1);
      //			szTemp[1] = NULL;
      //			nAlignment = atoi(szTemp);
      //			szBuf++;
      nAlignment = int.Parse(szBuf.Substring(9, 1));

      // set the orientation
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fOrientation = atoi(szTemp) / 100.0;
      //			szBuf += 5;
      fOrientation = int.Parse(szBuf.Substring(10, 5)) / 100.0;

      // set the start X coordinate
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fStartX = atoi(szTemp) / 10000.0;
      //			szBuf += 5;
      fStartX = int.Parse(szBuf.Substring(15, 5)) / 10000.0;

      // set the start Y coordinate
      //			strncpy(szTemp, szBuf, 5);
      //			szTemp[5] = NULL;
      //			fStartY = atoi(szTemp) / 10000.0;
      //			szBuf += 5;
      fStartY = int.Parse(szBuf.Substring(20, 5)) / 10000.0;


      if (nType == 0)
      {
        nId = 0;
        // find the end of the text string
        if (szBuf.Length > 25)
        {
          szTextString = szBuf.Substring(25);
          // create a new text object and add it to the layer
          bRetVal = AddText(nFontType, fFontSize, nFontStyle, nAlignment, fOrientation, fStartX, fStartY, szTextString, nType, nId);
        }
      }
      else if (nType == 1)
      {
        // set the id
        //				strncpy(szTemp, szBuf, 8);
        //				szTemp[8] = NULL;
        //				nId = atol(szTemp);
        //				szBuf += 8;
        nId = int.Parse(szBuf.Substring(25, 8));

        // find the end of the text string
        if (szBuf.Length > 33)
        {
          szTextString = szBuf.Substring(33);
        }
        else
        {
          szTextString = null;
        }
        // create a new text object and add it to the layer
        bRetVal = AddText(nFontType, fFontSize, nFontStyle, nAlignment, fOrientation, fStartX, fStartY, szTextString, nType, nId);

      }
      else // nType == 2
      {
        // set the id
        //				strncpy(szTemp, szBuf, 8);
        //				szTemp[8] = NULL;
        //				nId = atol(szTemp);
        nId = int.Parse(szBuf.Substring(25, 8));
        szTextString = null;

        // create a new text object and add it to the layer
        bRetVal = AddText(nFontType, fFontSize, nFontStyle, nAlignment, fOrientation, fStartX, fStartY, szTextString, nType, nId);
      }

      return bRetVal;
    }
    //private bool WriteEMF(CMetaFileDC* pMetaFile, bool bIsImperial)
    public bool WriteEMF_Geom(Graphics EMFile, bool bIsImperial)
    {
      int i;

      // loop through the array of lines and call the function to write the line to the EMF
      for (i = 0; i < m_TgfLineArray.Count; i++)
      {
        ((TgfLine)m_TgfLineArray[i]).WriteEMF(EMFile);
      }
      // loop through the array of polylines and call the function to write the line to the EMF
      for (i = 0; i < m_TgfPolylineArray.Count; i++)
      {
        ((TgfPolyline)m_TgfPolylineArray[i]).WriteEMF(EMFile);
      }
      // loop through the array of ellipses and call the function to write the ellipse to the EMF
      for (i = 0; i < m_TgfEllipseArray.Count; i++)
      {
        ((TgfEllipse)m_TgfEllipseArray[i]).WriteEMF(EMFile);
      }
      // loop through the array of arcs and call the function to write the arc to the EMF
      for (i = 0; i < m_TgfArcArray.Count; i++)
      {
        ((TgfArc)m_TgfArcArray[i]).WriteEMF(EMFile);
      }
      //			// loop through the array of text strings and call the function to write the text string to the EMF
      //			for (i = 0; i < m_TgfTextArray.Count; i++)
      //			{
      //				((TgfText)m_TgfTextArray[i]).WriteEMF(EMFile, bIsImperial);
      //			}

      return true;
    }

    public bool WriteEMF_Text(Graphics EMFile, bool bIsImperial, float fFontScale)
    {
      // loop through the array of text strings and call the function to write the text string to the EMF
      for (int i = 0; i < m_TgfTextArray.Count; i++)
      {
        ((TgfText)m_TgfTextArray[i]).WriteEMF(EMFile, bIsImperial, fFontScale);
      }

      return true;
    }

    //private bool WriteLayerDef(CFile* pTgfFile, int nLayerNdx)
    public bool WriteLayerDef(StreamWriter tgfFile, int nLayerNdx)
    {
      tgfFile.Write(string.Format("Z{0:d}{1:d2}{2:d10}{3:d4}\n", nLayerNdx, m_nLayerType, m_crColor, m_nLineWeight));
      return true;
    }
    //private bool WriteEntities(CFile* pTgfFile, int nLayerNdx)
    public bool WriteEntities(StreamWriter tgfFile, int nLayerNdx)
    {
      int i;

      for (i = 0; i < m_TgfLineArray.Count; i++)
      {
        ((TgfLine)m_TgfLineArray[i]).WriteTGF(tgfFile, nLayerNdx);
      }
      for (i = 0; i < m_TgfPolylineArray.Count; i++)
      {
        ((TgfPolyline)m_TgfPolylineArray[i]).WriteTGF(tgfFile, nLayerNdx);
      }
      for (i = 0; i < m_TgfEllipseArray.Count; i++)
      {
        ((TgfEllipse)m_TgfEllipseArray[i]).WriteTGF(tgfFile, nLayerNdx);
      }
      for (i = 0; i < m_TgfArcArray.Count; i++)
      {
        ((TgfArc)m_TgfArcArray[i]).WriteTGF(tgfFile, nLayerNdx);
      }
      for (i = 0; i < m_TgfTextArray.Count; i++)
      {
        ((TgfText)m_TgfTextArray[i]).WriteTGF(tgfFile, nLayerNdx);
      }

      return true;
    }

  }
}
