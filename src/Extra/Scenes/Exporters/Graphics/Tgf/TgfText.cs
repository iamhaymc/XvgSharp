using System;
using System.Drawing;
using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TgfText.
  /// </summary>
  public class TgfText
  {
    private const bool haloBorder = false;
    private const bool haloOn = true;
    private const bool xyPositionMarker = false;
    private TgfLayer m_TgfLayer;
    private double m_fStartX;
    private double m_fStartY;
    private int m_nFontType;
    private double m_fFontSize;
    private int m_nFontStyle;
    private int m_nNoteType;
    private int m_nAlignment;
    private double m_fOrientation;      // max precision is one-tenth of a degree
    private double m_fAltOrientation;
    private long m_nId;
    private string m_szTextString;
    private Font m_Font;
    private SolidBrush m_Brush;
    private StringFormat drawFormat;

    public TgfText(TgfLayer parentLayer)
    {
      m_TgfLayer = parentLayer;

      drawFormat = new StringFormat(StringFormat.GenericTypographic);

      //drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
      //drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft | StringFormatFlags.DisplayFormatControl;
      //drawFormat.FormatFlags = StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;
      //			drawFormat.Alignment = StringAlignment.Far;
      drawFormat.LineAlignment = StringAlignment.Far;
      drawFormat.Alignment = StringAlignment.Near;
    }
    public void SetCoordinates(double fStartX, double fStartY)
    {
      m_fStartX = fStartX;
      m_fStartY = fStartY;
    }
    //void SetFontType(int nFontType)
    public int TextFontType
    {
      get { return m_nFontType; }
      set
      {
        m_nFontType = value;
        if (m_nFontType < (int)TGFFONTTYPES.ARIAL || m_nFontType > (int)TGFFONTTYPES.TIMES)
          m_nFontType = (int)TGFFONTTYPES.ARIAL;
      }
    }
    //void SetFontSize(double fFontSize)
    public double TextFontSize
    {
      get { return m_fFontSize; }
      set { m_fFontSize = value; }
    }
    //void SetFontStyle(int nFontStyle)
    public int TextFontStyle
    {
      get { return m_nFontStyle; }
      set { m_nFontStyle = value; }
    }
    //void SetAlignment(int nAlignment)
    public int TextAlignment
    {
      get { return m_nAlignment; }
      set { m_nAlignment = value; }
    }
    //void SetOrientation(double fOrientation)
    public double AltTextOrientation
    {
      get { return m_fAltOrientation; }
    }
    public double TextOrientation
    {
      get { return m_fOrientation; }
      set
      {
        m_fOrientation = value;
        m_fAltOrientation = 360 - m_fOrientation;
        if (m_fAltOrientation >= 360.0)
          m_fAltOrientation -= 360.0;
      }
    }
    //void SetText(LPCTSTR szText)
    public string TextString
    {
      get { return m_szTextString; }
      set { m_szTextString = value; }
    }
    //void SetNoteType(int nNoteType)
    public int NoteType
    {
      get { return m_nNoteType; }
      set { m_nNoteType = value; }
    }
    //void SetId(long nId)
    public long NoteId
    {
      get { return m_nId; }
      set { m_nId = value; }
    }

    //void WriteTGF(CFile* pTgfFile, int nLayerNdx)
    public void WriteTGF(StreamWriter tgfFile, int nLayerNdx)
    {
      if (m_nNoteType == 0)
      {
        tgfFile.Write(string.Format("T{0:d}{1:d}{2:d4}{3:d2}{4:d}{5:d5}{6:d5}{7:d5}{8}\n", nLayerNdx, m_nFontType, (int)(m_fFontSize * 10000), m_nFontStyle, m_nAlignment, (int)(TextOrientation * 100), (int)(m_fStartX * 10000), (int)(m_fStartY * 10000), m_szTextString));
      }
      else if (m_nNoteType == 1)
      {
        //szOutput.Format("S%d%d%04d%02d%d%05d%05d%05d%08ld%s\n", nLayerNdx, m_nFontType, (int)(m_fFontSize*10000), m_nFontStyle, m_nAlignment, (int)(TextOrientation*100), (int)(m_fStartX*10000), (int)(m_fStartY*10000), m_nId, m_szTextString);
        tgfFile.Write(string.Format("S{0:d}{1:d}{2:d4}{3:d2}{4:d}{5:d5}{6:d5}{7:d5}{8:d8}{9}\n", nLayerNdx, m_nFontType, (int)(m_fFontSize * 10000), m_nFontStyle, m_nAlignment, (int)(TextOrientation * 100), (int)(m_fStartX * 10000), (int)(m_fStartY * 10000), m_nId, m_szTextString));
      }
      else // m_nNoteType == 2
      {
        //szOutput.Format("D%d%d%04d%02d%d%05d%05d%05d%08ld\n", nLayerNdx, m_nFontType, (int)(m_fFontSize*10000), m_nFontStyle, m_nAlignment, (int)(TextOrientation*100), (int)(m_fStartX*10000), (int)(m_fStartY*10000), m_nId);
        tgfFile.Write(string.Format("D{0:d}{1:d}{2:d4}{3:d2}{4:d}{5:d5}{6:d5}{7:d5}{8:d8}\n", nLayerNdx, m_nFontType, (int)(m_fFontSize * 10000), m_nFontStyle, m_nAlignment, (int)(TextOrientation * 100), (int)(m_fStartX * 10000), (int)(m_fStartY * 10000), m_nId));
      }
    }
    //void WriteEMF(CMetaFileDC *pMetaFile, bool bIsImperial)
    public void WriteEMF(Graphics EMFile, bool bIsImperial)
    {
      WriteEMF(EMFile, bIsImperial, 1.0f);
    }

    public void WriteEMF(Graphics EMFile, bool bIsImperial, float fHeightScale)
    {
      SetEMFFont(EMFile, fHeightScale);
      ParseText(EMFile, bIsImperial);
    }


    void SetEMFFont(Graphics EMFile, float fHeightScale)
    {
      // set font name
      string szFontName = "";
      if (m_nFontType == (int)TGFFONTTYPES.ARIAL)
        szFontName = "Arial";
      else if (m_nFontType == (int)TGFFONTTYPES.TIMES)
        szFontName = "Times New Roman";

      // set font styles
      FontStyle fontStyle = new FontStyle();
      if ((m_nFontStyle & (int)TGFFONTSTYLES.BOLD) == (int)TGFFONTSTYLES.BOLD)
        fontStyle = FontStyle.Bold;
      if ((m_nFontStyle & (int)TGFFONTSTYLES.ITALIC) == (int)TGFFONTSTYLES.ITALIC)
        fontStyle |= FontStyle.Italic;
      if ((m_nFontStyle & (int)TGFFONTSTYLES.UNDERLINE) == (int)TGFFONTSTYLES.UNDERLINE)
        fontStyle |= FontStyle.Underline;

      m_Font = new Font(szFontName, (float)TextFontSize * fHeightScale, fontStyle, GraphicsUnit.Point);
      m_Brush = new SolidBrush(Color.FromArgb(m_TgfLayer.LayerColor));
    }

    void ParseText(Graphics EMFile, bool bIsImperial)
    {
      string szTextOut;
      string szTextOut2;

      if (m_nNoteType == 1)
      {
        szTextOut = (string)m_TgfLayer.ParentTgfFile.NoteIdHashtable[(int)m_nId];

        if ((szTextOut == null || szTextOut.Length == 0) && !m_TgfLayer.ParentTgfFile.PipelineInterface)
          //			szTextOut = m_szTextString;
          szTextOut = string.Format("Note - {0:d}", m_nId);
      }
      else if (m_nNoteType == 2)
      {
        szTextOut = (string)m_TgfLayer.ParentTgfFile.NoteListIdHashtable[(int)m_nId];

        if ((szTextOut == null || szTextOut.Length == 0) && !m_TgfLayer.ParentTgfFile.PipelineInterface)
          //			szTextOut = m_szTextString;
          szTextOut = string.Format("Note List - {0:d}", m_nId);
      }
      else
        szTextOut = m_szTextString;

      if (szTextOut != null && szTextOut.Length > 0)
      {
        bool bLineRemain = true;
        int nLineNum = 0;
        int nLastPos = 0;
        int nPos;

        // loop though all lines of text
        while (bLineRemain)
        {
          nPos = szTextOut.IndexOf("\\P", nLastPos);
          if (nPos == -1)
          {
            bLineRemain = false;
            if (nLastPos == 0)
              szTextOut2 = szTextOut;
            else
              szTextOut2 = szTextOut.Substring(nLastPos);
          }
          else
          {
            szTextOut2 = szTextOut.Substring(nLastPos, nPos - nLastPos);
          }

          // replace any of the UOM fields (i.e. <<5.125~ln>> = 5 1/8")
          TextFormat.ReplaceStringUOMs(ref szTextOut2, bIsImperial);

          OutputText(EMFile, nLineNum, szTextOut2);

          nLastPos = nPos + 2;
          nLineNum++;
        }
      }
    }
    //void PositionText(CMetaFileDC* pMetaFile, int nLineNum, CString* szTextOut)
    void OutputText(Graphics EMFile, int nLineNum, string szTextOut)
    {
      float drawPosX, drawPosY, haloPosX, haloPosY;
      float fAngleSin, fAngleCos;
      SizeF haloSize;

      DetermineTextPosition(EMFile, nLineNum, out fAngleSin, out fAngleCos, out drawPosX, out drawPosY, out haloPosX, out haloPosY, out haloSize, szTextOut);

      if (haloOn)
      {
        CreateHalo(EMFile, haloPosX, haloPosY, fAngleSin, fAngleCos, haloSize);
      }
      // the reason we flip the Y axis back is that otherwise the font gets distorted
      EMFile.ScaleTransform(1.0f, -1.0f);

      //			if (szTextOut.IndexOf("50") != -1)
      //				MessageBox.Show(szTextOut + "\n" + drawPosX + "\n" + -drawPosY + "\n" + drawFormat.Alignment + "\n" + drawFormat.FormatFlags + "\n" + drawFormat.LineAlignment + "\n" + drawFormat.Trimming + "\n" + drawFormat.DigitSubstitutionLanguage + "\n" + drawFormat.DigitSubstitutionMethod + "\n");

      if (m_TgfLayer.ParentTgfFile.TopssCitrixMode)
      {
        EMFile.TranslateTransform(drawPosX, -drawPosY);
        EMFile.RotateTransform((float)AltTextOrientation);
        EMFile.DrawString(szTextOut, m_Font, m_Brush, 0.0f, 0.0f, drawFormat);
        m_TgfLayer.ParentTgfFile.SetBaseTranslation(EMFile);
      }
      else
      {
        if (AltTextOrientation != 0)
        {
          EMFile.TranslateTransform(drawPosX, -drawPosY);
          EMFile.RotateTransform((float)AltTextOrientation);
          EMFile.DrawString(szTextOut, m_Font, m_Brush, 0.0f, 0.0f, drawFormat);
          m_TgfLayer.ParentTgfFile.SetBaseTranslation(EMFile);
        }
        else
        {
          EMFile.DrawString(szTextOut, m_Font, m_Brush, drawPosX, -drawPosY, drawFormat);
          EMFile.ScaleTransform(1.0f, -1.0f);
        }
      }
      //			if (debugMode)
      //			{
      //				Pen haloPen = new Pen(Color.FromArgb(0,0, 255), 1/m_TgfLayer.ParentTgfFile.ScalingFactor);
      //				EMFile.DrawLine(haloPen, (float)m_fStartX, (float)m_fStartY - .02f, (float)m_fStartX, (float)m_fStartY + .02f);
      //				EMFile.DrawLine(haloPen, (float)m_fStartX - .02f, (float)m_fStartY, (float)m_fStartX + .02f, (float)m_fStartY);
      //				haloPen.Dispose();
      //			}
    }
    void DetermineTextPosition(Graphics EMFile, int nLineNum, out float fAngleSin, out float fAngleCos, out float drawPosX, out float drawPosY, out float haloPosX, out float haloPosY, out SizeF haloSize, string szTextOut)
    {
      // Calculate increment for each line
      double fRadians = (AltTextOrientation / 180.0) * 3.1415926535;
      //	int nDx = sin(fRadians) * nLineNum * (l_LogFont.lfHeight + l_LogFont.lfHeight/10);
      fAngleSin = (float)Math.Sin(fRadians);
      float nDx = fAngleSin * nLineNum * m_Font.GetHeight();
      fAngleCos = (float)Math.Cos(fRadians);
      float nDy = fAngleCos * nLineNum * m_Font.GetHeight();

      // Calculate location for each line
      drawPosX = (float)m_fStartX - nDx;
      drawPosY = (float)m_fStartY - nDy;

      haloPosX = drawPosX;
      haloPosY = drawPosY;

      if (!m_TgfLayer.ParentTgfFile.TopssCitrixMode)
      {
        drawPosX -= (.22f * fAngleSin) * m_Font.GetHeight();
        drawPosY -= (.22f * fAngleCos) * m_Font.GetHeight();
      }

      haloSize = EMFile.MeasureString(szTextOut, m_Font, 10, drawFormat);
      haloPosX -= fAngleSin * haloSize.Height * 0.20f;
      haloPosY -= fAngleCos * haloSize.Height * 0.20f;
      if (TextAlignment == (int)TGFALIGNMENTSTYLES.RIGHT)
      {
        drawPosX = drawPosX + (fAngleCos * -haloSize.Width);
        drawPosY = drawPosY - (fAngleSin * -haloSize.Width);
      }
      else if (TextAlignment == (int)TGFALIGNMENTSTYLES.CENTER)
      {
        drawPosX = drawPosX - (fAngleCos * 0.5f * haloSize.Width);
        drawPosY = drawPosY + (fAngleSin * 0.5f * haloSize.Width);
      }
    }
    // The backround is set to transparent, so we will need to create our own halos for each text box.
    // We do this because the natural text halos are not exactly correct, and halo printing problems do occur.
    //void (CMetaFileDC* pMetaFile, int nXX, int nYY, double fAngleSin, double fAngleCos, TEXTMETRIC* pTextMetrics, CString* szTextOut)
    void CreateHalo(Graphics EMFile, float nXX, float nYY, double fAngleSin, double fAngleCos, SizeF haloSize)
    {
      PointF[] ptClip = new PointF[4];    //POINTS for PolyGon() to clip lines

      if (m_nAlignment == (int)TGFALIGNMENTSTYLES.LEFT)
      {
        // calculate the coordinates of the polygon
        ptClip[0].X = (float)(nXX);
        ptClip[0].Y = (float)(nYY);
        ptClip[1].X = (float)(nXX + (fAngleCos * haloSize.Width));
        ptClip[1].Y = (float)(nYY - (fAngleSin * haloSize.Width));
        ptClip[2].X = (float)(nXX + (fAngleCos * haloSize.Width) + (fAngleSin * haloSize.Height));
        ptClip[2].Y = (float)(nYY - (fAngleSin * haloSize.Width) + (fAngleCos * haloSize.Height));
        ptClip[3].X = (float)(nXX + (fAngleSin * haloSize.Height));
        ptClip[3].Y = (float)(nYY + (fAngleCos * haloSize.Height));
      }
      else if (m_nAlignment == (int)TGFALIGNMENTSTYLES.RIGHT)
      {
        // calculate the coordinates of the polygon
        ptClip[0].X = (float)(nXX);
        ptClip[0].Y = (float)(nYY);
        ptClip[1].X = (float)(nXX + (fAngleCos * -haloSize.Width));
        ptClip[1].Y = (float)(nYY - (fAngleSin * -haloSize.Width));
        ptClip[2].X = (float)(nXX + (fAngleCos * -haloSize.Width) + (fAngleSin * haloSize.Height));
        ptClip[2].Y = (float)(nYY - (fAngleSin * -haloSize.Width) + (fAngleCos * haloSize.Height));
        ptClip[3].X = (float)(nXX + (fAngleSin * haloSize.Height));
        ptClip[3].Y = (float)(nYY + (fAngleCos * haloSize.Height));
      }
      else // (m_nAlignment == (int)TGFALIGNMENTSTYLES.CENTER)
      {
        nXX = nXX - (float)(fAngleCos * 0.5 * haloSize.Width);
        nYY = nYY + (float)(fAngleSin * 0.5 * haloSize.Width);
        ptClip[0].X = (float)(nXX);
        ptClip[0].Y = (float)(nYY);
        ptClip[1].X = (float)(nXX + (fAngleCos * haloSize.Width));
        ptClip[1].Y = (float)(nYY - (fAngleSin * haloSize.Width));
        ptClip[2].X = (float)(nXX + (fAngleCos * haloSize.Width) + (fAngleSin * haloSize.Height));
        ptClip[2].Y = (float)(nYY - (fAngleSin * haloSize.Width) + (fAngleCos * haloSize.Height));
        ptClip[3].X = (float)(nXX + (fAngleSin * haloSize.Height));
        ptClip[3].Y = (float)(nYY + (fAngleCos * haloSize.Height));
      }

      Color color;
      //	draw border of clip polygon or not.
      if (haloBorder)
      {
        // Draw a red border
        color = Color.FromArgb(0, 0, 255);
      }
      else
      {
        // White border (won't show up)
        color = Color.FromArgb(255, 255, 255);
      }

      using (Pen haloPen = new Pen(color, 1 / m_TgfLayer.ParentTgfFile.ScalingFactor))
      {
        // Draw the text halo poly clip!
        using (Brush haloBrush = new SolidBrush(Color.FromArgb(255, 255, 255)))
        {
          EMFile.FillPolygon(haloBrush, ptClip);
          EMFile.DrawPolygon(haloPen, ptClip);

          if (xyPositionMarker)
          {
            // draw blue + at XY point
            haloPen.Color = Color.FromArgb(0, 0, 255);
            EMFile.DrawLine(haloPen, nXX, nYY - .05f, nXX, nYY + .05f);
            EMFile.DrawLine(haloPen, nXX - .05f, nYY, nXX + .05f, nYY);

            haloPen.Color = Color.FromArgb(0, 255, 255);
            EMFile.DrawLine(haloPen, (float)(m_fStartX), (float)(m_fStartY - .05), (float)(m_fStartX), (float)(m_fStartY + .05));
            EMFile.DrawLine(haloPen, (float)(m_fStartX - .05), (float)(m_fStartY), (float)(m_fStartX + .05), (float)(m_fStartY));
          }
        }
      }
    }
  }
}
