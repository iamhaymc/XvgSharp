using System;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace Trane.Submittals.Pipeline
{
  public enum TGFLAYERTYPES
  {
    GEOMETRIC = 1,    // Geometric (Construction) Layer
    DIMENSION,        // Dimension Layer (all dimension lines and text)
    TEXT,          // Text Layer (all leader note lines and text)
    TEMPLATE       // Template Layer (for any entities to be displayed on a template only)
  }
  public enum TGFLINETYPES
  {
    CONTINUOUS = 1,   // Normal Unbroken Line Type
    HIDDEN,           // Hidden Line Type (uniformly broken)
    CENTER,           // Center Line Type
    PHANTOM           // Phantom Line Type
  }
  public enum TGFFONTTYPES
  {
    ARIAL = 1,        // Arial Font
    TIMES          // Times New Roman Font
  }
  public enum TGFFONTSTYLES
  {
    NORMAL = 0, // Normal Text
    BOLD = 2,   // Bolded Text
    ITALIC = 4, // Italicize Text
    UNDERLINE = 8     // Underlined Text
  }
  public enum TGFALIGNMENTSTYLES
  {
    RIGHT = 1,
    LEFT,
    CENTER
  }
  public enum TGFVERSION  // see the bottom of this file for more information on the TGF formats
  {
    V1 = 1,
    V2,
    V3 = 3,           // 12/2003 version change to handle storing larger Arc Center Points, also handles negative values,
                      //         an arc can have a center point which exists way outside of the paperspace
    CURRENT = V3
  }

  public interface ITgfFile
  {
    float ScalingFactor { get; }
    double DrawingHeight { get; set; }
    double DrawingWidth { get; set; }
    double DefaultLineTypeScale { get; set; }
    bool PipelineInterface { get; set; }
    bool TopssCitrixMode { get; set; }
    bool Landscape { get; set; }
    int TgfVersion { get; set; }

    bool CreateNewFile(string szFileName, double fDrawingWidth, double fDrawingHeight, double fDefLtScale);
    bool OutputToTGF();
    int AddLayer(int nLayerType, int crColor, int nLineWeight);
    bool LoadFromFile(string szFileName);
    bool OutputToEMF(string szEmfFileName, bool bIsImperial);
    bool LoadFromPtr(string szTgfData);
    bool AddLine(int nLayerNdx, double fStartX, double fStartY, double fEndX, double fEndY, int nLineType, double fLtScale);
    bool AddPolyline(int nLayerNdx, double[] fX, double[] fY, int nCount);
    bool AddEllipse(int nLayerNdx, double fUpperLeftX, double fUpperLeftY, double fLowerRightX, double fLowerRightY, double fRotationAngle);
    bool AddArc(int nLayerNdx, double fCenterX, double fCenterY, double fRadius, double fStartAngle, double fSweepAngle, short nLineType, double fLtScale);
    bool AddCircle(int nLayerNdx, double fCenterX, double fCenterY, double fRadius, short nLineType, double fLtScale);
    bool AddPoint(int nLayerNdx, double fCenterX, double fCenterY);
    bool AddText(int nLayerNdx, int nFontType, double fFontSize, int nStyle, int nAlignment, double fOrientation, double fStartX, double fStartY, string szTextString);
    bool AddRText(int nLayerNdx, int nFontType, double fFontSize, int nStyle, int nAlignment, double fOrientation, double fStartX, double fStartY, int nNoteType, long nId, string szTextString);
    bool MovetoNextNoteEntry(bool bReset);
    int CurrentNoteId { get; }
    bool MovetoNextNoteListEntry(bool bReset);
    int CurrentNoteListId { get; }
    void SetNoteText(int nId, string szNote);
    void SetNoteListText(int nId, string szNote);
  }
  public class TgfFile : ITgfFile
  {
    public ILogger Logger { get; set; }
    const float scalingFactor = 1000.0f;
    private StreamWriter m_TgfStreamWriter;
    private string m_TgfFileName;
    private int m_nTgfVersion;
    private double m_fDrawingHeight;
    private double m_fDrawingWidth;
    private double m_fDefLtScale;
    private bool m_bPLInterface;
    private bool m_bIsLandscape;
    private bool topssCitrixMode;
    private Hashtable m_NoteIdMap;
    private Hashtable m_NoteListIdMap;
    private ArrayList m_TgfLayerArray;
    private IDictionaryEnumerator m_NoteListEnumerator;
    private IDictionaryEnumerator m_NoteEnumerator;

    /**********************************************************************************************************
         Constructor
    **********************************************************************************************************/
    public TgfFile(ILogger logger)
    {
      Logger = logger;
      m_NoteIdMap = new Hashtable();
      m_NoteListIdMap = new Hashtable();
      m_TgfLayerArray = new ArrayList();

      this.m_bPLInterface = false;
      this.topssCitrixMode = false;
      TgfVersion = 0;
    }

    //replaces void SetSize(double fDrawingHeight, double fDrawingWidth, double fDefLtScale)
    public float ScalingFactor
    {
      get { return scalingFactor; }
    }
    // replaces double GetDrawingHeight()
    public double DrawingHeight
    {
      get { return m_fDrawingHeight; }
      set { m_fDrawingHeight = value; }
    }
    // replaces double GetDrawingWidth()
    public double DrawingWidth
    {
      get { return m_fDrawingWidth; }
      set { m_fDrawingWidth = value; }
    }
    // replaces public double GetLtScale()
    public double DefaultLineTypeScale
    {
      get { return m_fDefLtScale; }
      set { m_fDefLtScale = value; }
    }
    // replaces IsPipelineInterface and SetPipelineInterface
    public bool PipelineInterface
    {
      get { return m_bPLInterface; }
      set { m_bPLInterface = value; }
    }
    public bool TopssCitrixMode
    {
      get { return this.topssCitrixMode; }
      set { this.topssCitrixMode = value; }
    }
    // replaces IsLandscape
    public bool Landscape   // indicates whether this drawing is in landscape format (if so, it will need to be rotated)
    {
      get { return m_bIsLandscape; }
      set { m_bIsLandscape = value; }
    }
    public bool MovetoNextNoteEntry(bool bReset)
    {
      if (bReset)
      {
        m_NoteEnumerator = m_NoteIdMap.GetEnumerator();
        m_NoteEnumerator.Reset();
      }
      return m_NoteEnumerator.MoveNext();
    }
    public int CurrentNoteId
    {
      get { return (int)m_NoteEnumerator.Key; }
    }
    public Hashtable NoteIdHashtable
    {
      get { return m_NoteIdMap; }
    }
    public bool MovetoNextNoteListEntry(bool bReset)
    {
      if (bReset)
      {
        m_NoteListEnumerator = m_NoteListIdMap.GetEnumerator();
        m_NoteListEnumerator.Reset();
      }
      return m_NoteListEnumerator.MoveNext();
    }
    public int CurrentNoteListId
    {
      get { return (int)m_NoteListEnumerator.Key; }
    }
    public Hashtable NoteListIdHashtable
    {
      get { return m_NoteListIdMap; }
    }
    // renamed from GetTgfVersion
    public int TgfVersion
    {
      get { return m_nTgfVersion; }
      set { m_nTgfVersion = value; }
    }

    public bool CreateNewFile(string szFileName, double fDrawingWidth, double fDrawingHeight, double fDefLtScale)
    {
      bool bRetVal = true;

      m_TgfFileName = szFileName;
      try
      {
        //TGH 033108
        m_NoteIdMap.Clear();
        m_NoteListIdMap.Clear();

        foreach (TgfLayer t in m_TgfLayerArray)
        {
          t.ClearAll();
        }
        m_TgfLayerArray.Clear();
        //--

        m_TgfStreamWriter = new StreamWriter(m_TgfFileName);
        DrawingHeight = fDrawingHeight;
        DrawingWidth = fDrawingWidth;
        DefaultLineTypeScale = fDefLtScale;
        if (DrawingHeight == 7.4 && DrawingWidth == 8.8)
          Landscape = true;
        else
          Landscape = false;
      }
      catch
      {
        bRetVal = false;
      }

      return bRetVal;
    }
    public bool OutputToTGF()
    {
      if (m_TgfStreamWriter != null)
        return WriteTGF();
      else
        return false;
    }

    private bool WriteTGF()
    {
      int nNumEntities = 0;
      int nNumLayers = 0;
      bool bRetVal = true;
      int i;

      // loop through the array of layers and check each layer for entities
      for (i = 0; i < m_TgfLayerArray.Count && bRetVal; i++)
      {
        if (((TgfLayer)m_TgfLayerArray[i]).GetNumEntities() > 0)
        {
          nNumEntities += ((TgfLayer)m_TgfLayerArray[i]).GetNumEntities();
          nNumLayers++;
        }
      }

      // if at least one entity is define on one of the layers
      if (nNumEntities > 0)
      {
        // write out the TGF file header
        //szHeader = String.Format("TGF%02d%05d%05d%d%05d\n", (int)TGFVERSION.CURRENT, (int)(GetDrawingWidth()*10000), (int)(GetDrawingHeight()*10000), nNumLayers, (int)(m_fDefLtScale*10000));
        m_TgfStreamWriter.Write(string.Format("TGF{0:d2}{1:d5}{2:d5}{3:d}{4:d5}\n", (int)TGFVERSION.CURRENT, (int)(DrawingWidth * 10000), (int)(DrawingHeight * 10000), nNumLayers, (int)(m_fDefLtScale * 10000)));

        // loop through the array of layers and write out the layer definition to the file
        int nLayerNdx = 0;
        for (i = 0; i < m_TgfLayerArray.Count && bRetVal; i++)
        {
          if (((TgfLayer)m_TgfLayerArray[i]).GetNumEntities() > 0)
          {
            bRetVal = ((TgfLayer)m_TgfLayerArray[i]).WriteLayerDef(m_TgfStreamWriter, ++nLayerNdx);
          }
        }
        // loop through the array of layers and write out the entities of each layer to the file
        nLayerNdx = 0;
        for (i = 0; i < m_TgfLayerArray.Count && bRetVal; i++)
        {
          if (((TgfLayer)m_TgfLayerArray[i]).GetNumEntities() > 0)
          {
            bRetVal = ((TgfLayer)m_TgfLayerArray[i]).WriteEntities(m_TgfStreamWriter, ++nLayerNdx);
          }
        }
      }

      try
      {
        // close the file
        m_TgfStreamWriter.Close();
        if (nNumEntities == 0)
        {
          // remove the file
          if (File.Exists(m_TgfFileName))
            File.Delete(m_TgfFileName);
          bRetVal = false;
        }
      }
      catch
      {
        bRetVal = false;
      }

      return bRetVal;
    }
    //public int DefineLayer(int nLayerType, Int32 crColor, int nLineWeight)
    public int AddLayer(int nLayerType, int crColor, int nLineWeight)
    {
      TgfLayer tgfLayer = new TgfLayer(this);
      tgfLayer.LayerType = nLayerType;
      tgfLayer.LayerColor = crColor;
      tgfLayer.LineWeight = nLineWeight;
      tgfLayer.CreatePen();
      return m_TgfLayerArray.Add(tgfLayer);
    }
    private object GetLayer(int nLayerNdx)
    {
      if (nLayerNdx >= 0 && nLayerNdx < m_TgfLayerArray.Count)
        return m_TgfLayerArray[nLayerNdx];
      //if (nLayerNdx > 0 && nLayerNdx <= m_TgfLayerArray.Count)
      //	return m_TgfLayerArray[nLayerNdx-1];
      else
        return null;
    }
    public bool LoadFromFile(string szFileName)
    {
      return ReadFile(szFileName);
    }
    public bool OutputToEMF(string szEmfFileName, bool bIsImperial)
    {
      return WriteEMF(szEmfFileName, bIsImperial);
    }
    public bool LoadFromPtr(string szTgfData)
    {
      return ReadTgf(szTgfData);
    }
    // fLtScale=1 is default
    public bool AddLine(int nLayerNdx, double fStartX, double fStartY, double fEndX, double fEndY, int nLineType, double fLtScale)
    {
      TgfLayer tgfLayer = (TgfLayer)GetLayer(nLayerNdx);
      if (tgfLayer != null)
        return tgfLayer.AddLine(fStartX, fStartY, fEndX, fEndY, nLineType, fLtScale);
      else
        return false;
    }
    //public bool AddPolyline(int nLayerNdx, double* pfX, double* pfY, int nCount)
    public bool AddPolyline(int nLayerNdx, double[] fX, double[] fY, int nCount)
    {
      TgfLayer tgfLayer = (TgfLayer)GetLayer(nLayerNdx);
      if (tgfLayer != null)
        return tgfLayer.AddPolyline(fX, fY, nCount);
      else
        return false;
    }
    public bool AddEllipse(int nLayerNdx, double fUpperLeftX, double fUpperLeftY, double fLowerRightX, double fLowerRightY, double fRotationAngle)
    {
      while (fRotationAngle > 360)
        fRotationAngle -= 360;

      TgfLayer tgfLayer = (TgfLayer)GetLayer(nLayerNdx);
      if (tgfLayer != null)
        return tgfLayer.AddEllipse(fUpperLeftX, fUpperLeftY, fLowerRightX, fLowerRightY, fRotationAngle);
      else
        return false;
    }
    // fLtScale=1 is default
    public bool AddArc(int nLayerNdx, double fCenterX, double fCenterY, double fRadius, double fStartAngle, double fSweepAngle, short nLineType, double fLtScale)
    {
      while (fStartAngle > 360)
        fStartAngle -= 360;
      if (fSweepAngle > 360)
        fSweepAngle = 360;

      TgfLayer tgfLayer = (TgfLayer)GetLayer(nLayerNdx);
      if (tgfLayer != null)
        return tgfLayer.AddArc(fCenterX, fCenterY, fRadius, fStartAngle, fSweepAngle, nLineType, fLtScale);
      else
        return false;
    }
    // fLtScale=1 is default
    public bool AddCircle(int nLayerNdx, double fCenterX, double fCenterY, double fRadius, short nLineType, double fLtScale)
    {
      TgfLayer tgfLayer = (TgfLayer)GetLayer(nLayerNdx);
      if (tgfLayer != null)
        return tgfLayer.AddArc(fCenterX, fCenterY, fRadius, 0, 360, nLineType, fLtScale);
      else
        return false;
    }
    public bool AddPoint(int nLayerNdx, double fCenterX, double fCenterY)
    {
      TgfLayer tgfLayer = (TgfLayer)GetLayer(nLayerNdx);
      if (tgfLayer != null)
        return tgfLayer.AddArc(fCenterX, fCenterY, 0.005, 0, 360, (int)TGFLINETYPES.CONTINUOUS, 1);
      else
        return false;
    }
    public bool AddText(int nLayerNdx, int nFontType, double fFontSize, int nStyle, int nAlignment, double fOrientation, double fStartX, double fStartY, string szTextString)
    {
      TgfLayer tgfLayer = (TgfLayer)GetLayer(nLayerNdx);
      if (tgfLayer != null)
        return tgfLayer.AddText(nFontType, fFontSize, nStyle, nAlignment, fOrientation, fStartX, fStartY, szTextString, 0, 0);
      else
        return false;
    }
    // szTextString=NULL is default
    public bool AddRText(int nLayerNdx, int nFontType, double fFontSize, int nStyle, int nAlignment, double fOrientation, double fStartX, double fStartY, int nNoteType, long nId, string szTextString)
    {
      TgfLayer tgfLayer = (TgfLayer)GetLayer(nLayerNdx);
      if (tgfLayer != null)
        return tgfLayer.AddText(nFontType, fFontSize, nStyle, nAlignment, fOrientation, fStartX, fStartY, szTextString, nNoteType, nId);
      else
        return false;
    }

    public void SetNoteText(int nId, string szNote)
    {
      //m_NoteIdMap.Add(nId, szNote);
      m_NoteIdMap[nId] = szNote;
    }
    public void SetNoteListText(int nId, string szNote)
    {
      //m_NoteListIdMap.Add(nId, null);
      m_NoteListIdMap[nId] = szNote;
    }

    public float ConvertFontPointSize(double fFontSize)
    {
      //			return (float)(((fFontSize*100.0) * 10.0));
      return (float)(((fFontSize * 1.0) * 1.0));
    }
    public RectangleF CorrectRectangle(float x1, float y1, float x2, float y2)
    {
      return RectangleF.FromLTRB(Math.Min(x1, x2), Math.Min(y1, y2), Math.Max(x1, x2), Math.Max(y1, y2));
    }
    public void SetBaseTranslation(Graphics metaGraphic)
    {
      metaGraphic.ResetTransform();
      if (Landscape)
      {
        //				metaGraphic.TranslateTransform((float)DrawingHeight, 0.0f);
        metaGraphic.TranslateTransform((float)(DrawingHeight * scalingFactor), (float)(DrawingWidth * scalingFactor));
        metaGraphic.ScaleTransform(scalingFactor, -scalingFactor);
        metaGraphic.RotateTransform((float)90);
      }
      else
      {
        metaGraphic.TranslateTransform(0.0f, (float)(DrawingHeight * scalingFactor));
        metaGraphic.ScaleTransform(scalingFactor, -scalingFactor);
      }
    }

    //"TGF{0:d2}{1:d5}{2:d5}{3:d}{4:d5}\n", (int)TGFVERSION.CURRENT, (int)(DrawingWidth*10000), (int)(DrawingHeight*10000), nNumLayers, (int)(m_fDefLtScale*10000)));
    private bool ReadTgf(string szBuf)
    {
      bool bRetVal = false;
      int headerEndPos = 0;

      szBuf = szBuf.Replace("\r\n", "\n");
      // read header (the first 3 characters need to be TGF)
      if (szBuf.Substring(0, 3).CompareTo("TGF") == 0)
      {
        // find the end of the header
        int lfPos = headerEndPos = szBuf.IndexOf('\n');
        if (headerEndPos == 21 || headerEndPos == 22 || headerEndPos == 23)
        //char* ptr1 = strchr(szBuf, '\n');
        //if (ptr1)
        {
          short nNumLayers = 0;
          // read the header data
          bRetVal = ReadHeader(szBuf, headerEndPos, ref nNumLayers);
          if (bRetVal)
          {
            // read layers
            //char* ptr2 = ptr1;
            for (short i = 0; i < nNumLayers && bRetVal; i++)
            {
              //							ptr1 = ++ptr2;
              //							// find the end of the layer
              //							ptr2 = strchr(ptr1, '\n');
              //							if (ptr2)
              //							{
              //								// read the layer data
              //								bRetVal = ReadLayer(ptr1, i);
              //							}
              // read the layer data
              bRetVal = ReadLayer(szBuf.Substring(headerEndPos + 1 + (i * 19), 19), i);
            }
            int startPos = headerEndPos + 1 + (nNumLayers * 19);
            while (szBuf.Length > startPos && bRetVal && lfPos > -1)
            {
              //							ptr1 = ++ptr2;
              //							// find the end of the layer
              //							ptr2 = strchr(ptr1, '\n');
              //							if (ptr2)
              //							{
              //								// read the command
              //								bRetVal = ReadCommand(ptr1);
              //							}
              // find the end of the command
              lfPos = szBuf.IndexOf('\n', startPos);
              if (lfPos > startPos)
              {
                // read the command
                bRetVal = ReadCommand(szBuf.Substring(startPos, lfPos - startPos));
                startPos = lfPos + 1;
              }
            }
          }
        }
      }

      return bRetVal;
    }
    //private bool ReadCommand(char* szBuf)
    private bool ReadCommand(string szBuf)
    {
      bool bRetVal = false;

      // set the command name
      char cCommand = szBuf[0];
      // set the layer index
      short nLayerNdx = short.Parse(szBuf.Substring(1, 1));

      TgfLayer tgfLayer = (TgfLayer)GetLayer(nLayerNdx - 1);
      if (tgfLayer != null)
      {
        //				szBuf += 2;
        // read header to determine what type of command it is
        if (cCommand == 'L')
        {
          bRetVal = tgfLayer.ReadLine(szBuf);
        }
        else if (cCommand == 'A')
        {
          bRetVal = tgfLayer.ReadArc(szBuf);
        }
        else if (cCommand == 'T')
        {
          bRetVal = tgfLayer.ReadText(szBuf, 0);
        }
        else if (cCommand == 'S')
        {
          bRetVal = tgfLayer.ReadText(szBuf, 1);
        }
        else if (cCommand == 'D')
        {
          bRetVal = tgfLayer.ReadText(szBuf, 2);
        }
        else if (cCommand == 'E')
        {
          bRetVal = tgfLayer.ReadEllipse(szBuf);
        }
        else if (cCommand == 'P')
        {
          bRetVal = tgfLayer.ReadPolyline(szBuf);
        }
      }

      return bRetVal;
    }
    // "Z{0:d}{1:d2}{2:d10}{3:d4}\n", nLayerNdx, m_nLayerType, m_crColor, m_nLineWeight));
    //private bool ReadLayer(char* szBuf, int nLayerNum)
    private bool ReadLayer(string szBuf, short nLayerNum)
    {
      bool bRetVal = false;
      int nLayerNdx;
      //char		szTemp[11];

      // read header (the first character needs to be Z)
      //if (strncmp(szBuf, "Z", 1) == 0)
      if (szBuf[0] == 'Z')
      {
        //				// move the pointer to the start of the layer index
        //				szBuf++;

        // set the Layer Index
        //				strncpy(szTemp, szBuf, 1);
        //				szTemp[1] = NULL;
        //				nLayerNdx = atoi(szTemp);
        //				szBuf++;
        nLayerNdx = short.Parse(szBuf.Substring(1, 1));

        if (nLayerNdx == nLayerNum + 1)
        {
          // create a new layer
          TgfLayer tgfLayer = new TgfLayer(this);

          // set the Layer Type
          //					strncpy(szTemp, szBuf, 2);
          //					szTemp[2] = NULL;
          //					nLayerType = atoi(szTemp);
          //					szBuf += 2;
          tgfLayer.LayerType = short.Parse(szBuf.Substring(2, 2));

          // set the Layer color
          //					strncpy(szTemp, szBuf, 10);
          //					szTemp[10] = NULL;
          //					nLayerColor = atol(szTemp);
          //					szBuf += 10;
          tgfLayer.LayerColor = int.Parse(szBuf.Substring(4, 10));

          // set the Layer line weight
          //					strncpy(szTemp, szBuf, 4);
          //					szTemp[4] = NULL;
          //					nLineWeight = atoi(szTemp);
          tgfLayer.LineWeight = int.Parse(szBuf.Substring(14, 4));

          tgfLayer.CreatePen();
          m_TgfLayerArray.Add(tgfLayer);

          bRetVal = true;
        }
      }

      return bRetVal;
    }
    // "TGF{0:d2}{1:d5}{2:d5}{3:d}{4:d5}\n", (int)TGFVERSION.CURRENT, (int)(DrawingWidth*10000), (int)(DrawingHeight*10000), nNumLayers, (int)(m_fDefLtScale*10000)));
    //private bool ReadHeader(string szBuf, int* nNumLayers)
    private bool ReadHeader(string szBuf, int headerEndPos, ref short nNumLayers)
    {
      bool bRetVal = false;
      //			char	szTemp[6];

      //			// move the pointer to the start of the version
      //			szBuf += 3;

      // set the version
      //			strncpy(szTemp, szBuf, 2);
      //			szTemp[2] = NULL;
      //			m_nTgfVersion = atoi(szTemp);
      //			szBuf += 2;
      TgfVersion = int.Parse(szBuf.Substring(3, 2));

      if (TgfVersion >= (int)TGFVERSION.V3)  // not supporting anything before version 3
      {

        // set the drawing width
        //			strncpy(szTemp, szBuf, 5);
        //			szTemp[5] = NULL;
        //			double fDrawingWidth = atoi(szTemp) / 10000.0;
        //			szBuf += 5;
        DrawingWidth = int.Parse(szBuf.Substring(5, 5)) / 10000.0;

        // set the drawing height
        //			strncpy(szTemp, szBuf, 5);
        //			szTemp[5] = NULL;
        //			double fDrawingHeight = atoi(szTemp) / 10000.0;
        //			szBuf += 5;
        DrawingHeight = int.Parse(szBuf.Substring(10, 5)) / 10000.0;

        // set the layer count
        //			strncpy(szTemp, szBuf, 1);
        //			szTemp[1] = NULL;
        //			*nNumLayers = atoi(szTemp);
        //			szBuf++;
        nNumLayers = short.Parse(szBuf.Substring(15, 1));

        // set the Default Line Type Scale
        //			strncpy(szTemp, szBuf, 5);
        //			szTemp[5] = NULL;
        //			double fDefLtScale = atoi(szTemp) / 10000.0;
        DefaultLineTypeScale = int.Parse(szBuf.Substring(16, headerEndPos - 16)) / 10000.0;

        if (DrawingHeight == 7.4 && DrawingWidth == 8.8)
          Landscape = true;
        else
          Landscape = false;

        bRetVal = true;
      }

      return bRetVal;
    }
    private bool ReadFile(string szFileName)
    {
      bool bRetVal = false;

      try
      {
        // Create an instance of StreamReader to read from a file.
        // The using statement also closes the StreamReader.
        using (StreamReader sr = new StreamReader(szFileName))
        {
          // read the entire file into the string buffer
          string szBuf = sr.ReadToEnd();
          bRetVal = ReadTgf(szBuf);
        }
      }
      catch (Exception ex)
      {
        Logger.Error(ex, $"Failure to read TGF file {szFileName}");
      }

      return bRetVal;
    }
    private bool WriteEMF(string szEmfFileName, bool bIsImperial)
    {
      bool bRetVal = true;

      // note: we add .01 inch to the drawing size because if we don't, the right-most,
      //       and bottom-most pixels get cropped by MS Word

      IntPtr hDC = Win32.GetDC(IntPtr.Zero);
      int m_nWidthMM;
      int m_nHeightMM;
      int m_nWidthPels;
      int m_nHeightPels;
      m_nWidthMM = Win32.GetDeviceCaps(hDC, Win32.HORZSIZE);
      m_nHeightMM = Win32.GetDeviceCaps(hDC, Win32.VERTSIZE);
      m_nWidthPels = Win32.GetDeviceCaps(hDC, Win32.HORZRES);
      m_nHeightPels = Win32.GetDeviceCaps(hDC, Win32.VERTRES);

      //NOTE: The calculation for the EMF bounding rectangle will be in millimeters. The
      //file is always generated as a portrait sized image, even though landscape mode
      //turns it on its side to render the drawing. Because of that, the width of the emf
      //rectangle will be the drawing height (7.4 in) in landscape mode and the drawing
      //width (7.4 in) in portrait mode. In either case, the calculation for width should
      //use the screen DC width dimensions, and the same goes for the height calculation.
      Rectangle EmfBounds;
      if (Landscape)
      {
        EmfBounds = Rectangle.FromLTRB(
             0,
             0,
             (int)((scalingFactor * (DrawingHeight + 0.01) * m_nWidthMM) / m_nWidthPels),
             (int)((scalingFactor * (DrawingWidth + 0.01) * m_nHeightMM) / m_nHeightPels));
      }
      else
      {
        EmfBounds = Rectangle.FromLTRB(
             0,
             0,
             (int)((scalingFactor * (DrawingWidth + 0.01) * m_nWidthMM) / m_nWidthPels),
             (int)((scalingFactor * (DrawingHeight + 0.01) * m_nHeightMM) / m_nHeightPels));
      }

      using (Metafile metaFile = new Metafile(szEmfFileName, hDC, EmfBounds,
           MetafileFrameUnit.Millimeter, EmfType.EmfOnly,
           string.Format("Trane Graphic File Version {0}\0", TgfVersion)))
      {

        using (Graphics metaGraphic = Graphics.FromImage(metaFile))
        {

          SetBaseTranslation(metaGraphic);
          // Clip everything outside of the drawing height and width
          Region clipRegion = new Region(new RectangleF(0, 0, (float)DrawingWidth, (float)DrawingHeight));
          metaGraphic.SetClip(clipRegion, CombineMode.Replace);

          // scale font down to 96 dpi
          float fFontScale = 1.0f;
          if (!this.TopssCitrixMode &&
               96.0f - metaGraphic.DpiY < -0.1f)
          {
            fFontScale = 96.0f / metaGraphic.DpiY;
          }

          // loop through the array of layers - Geom
          for (int i = 0; i < m_TgfLayerArray.Count && bRetVal; i++)
          {
            bRetVal = ((TgfLayer)m_TgfLayerArray[i]).WriteEMF_Geom(metaGraphic, bIsImperial);
          }

          // loop through the array of layers text
          for (int i = 0; i < m_TgfLayerArray.Count && bRetVal; i++)
          {
            bRetVal = ((TgfLayer)m_TgfLayerArray[i]).WriteEMF_Text(metaGraphic, bIsImperial, fFontScale);
          }
        }
      }

      // Release handle to temporary device context.
      Win32.ReleaseDC(IntPtr.Zero, hDC);

      return bRetVal;
    }
  }
  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  //////// Interop functions
  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  internal class Win32
  {
    internal const int HORZSIZE = 4;    /* Horizontal size in millimeters	*/
    internal const int VERTSIZE = 6;    /* Vertical size in millimeters		*/
    internal const int HORZRES = 8;     /* Horizontal width in pixels		*/
    internal const int VERTRES = 10; /* Vertical height in pixels		*/
    internal const int LOGPIXELSX = 88; /* Logical pixels/inch in X			*/
    internal const int LOGPIXELSY = 90; /* Logical pixels/inch in Y			*/
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    internal static extern IntPtr GetDC(IntPtr hWnd);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    internal static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
    [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
    internal static extern int GetDeviceCaps(IntPtr hDC, int id);

  }
}


/* TGF Version 3 format
 * TGF Header - "TGF{0:d2}{1:d5}{2:d5}{3:d}{4:d5}\n", (int)TGFVERSION.CURRENT, (int)(DrawingWidth*10000), (int)(DrawingHeight*10000), nNumLayers, (int)(m_fDefLtScale*10000)));
 * Layer - "Z{0:d}{1:d2}{2:d10}{3:d4}\n", nLayerNdx, m_nLayerType, m_crColor, m_nLineWeight));
 * Line - "L{0:d}{1:d}{2:d5}{3:d5}{4:d5}{5:d5}{6:d5}\n", nLayerNdx, m_nLineType, (int)(m_fStartX*10000), (int)(m_fStartY*10000), (int)(m_fEndX*10000), (int)(m_fEndY*100), (int)(m_fLtScale*10000)));
 * Polyline - "P{0:d}{1:d3}{2:d5}{3:d5}\n", nLayerNdx, m_nCount, (int)(m_fX[i]*10000), (int)(m_fY[i]*10000)));
 * Arc - "A{0:d}{1:d}{2:d8}{3:d8}{4:d7}{5:d5}{6:d5}{7:d5}\n", nLayerNdx, m_nLineType, (int)(m_fCenterX*10000), (int)(m_fCenterY*10000), (int)(m_fRadius*10000), (int)(m_fStartAngle*100), (int)(m_fSweepAngle*100), (int)(m_fLtScale*10000)));
 * Ellipse - "E{0:d}{1:d5}{2:d5}{3:d5}{4:d5}{5:d5}\n", nLayerNdx, (int)(m_fUpperLeftX*10000), (int)(m_fUpperLeftY*10000), (int)(m_fLowerRightX*10000), (int)(m_fLowerRightY*10000), (int)(m_fRotationAngle*100)));
 * Standard Text (Type 0) - "T{0:d}{1:d}{2:d4}{3:d2}{4:d}{5:d5}{6:d5}{7:d5}{8}\n", nLayerNdx, m_nFontType, (int)(m_fFontSize*10000), m_nFontStyle, m_nAlignment, (int)(m_fOrientation*100), (int)(m_fStartX*10000), (int)(m_fStartY*10000), m_szTextString));
 * Static Note (Type 1) - "S{0:d}{1:d}{2:d4}{3:d2}{4:d}{5:d5}{6:d5}{7:d5}{8:d8}{9}\n", nLayerNdx, m_nFontType, (int)(m_fFontSize*10000), m_nFontStyle, m_nAlignment, (int)(m_fOrientation*100), (int)(m_fStartX*10000), (int)(m_fStartY*10000), m_nId, m_szTextString));
 * Dynamic Note (Type 2) - "D{0:d}{1:d}{2:d4}{3:d2}{4:d}{5:d5}{6:d5}{7:d5}{8:d8}\n", nLayerNdx, m_nFontType, (int)(m_fFontSize*10000), m_nFontStyle, m_nAlignment, (int)(m_fOrientation*100), (int)(m_fStartX*10000), (int)(m_fStartY*10000), m_nId));
 */
