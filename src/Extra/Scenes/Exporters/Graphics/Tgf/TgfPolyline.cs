using System;
using System.Drawing;
using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TgfPolyline.
  /// </summary>
  public class TgfPolyline
  {
    private TgfLayer m_TgfLayer;
    double[] m_fX;
    double[] m_fY;
    int m_nCount;

    public TgfPolyline(TgfLayer parentLayer)
    {
      m_TgfLayer = parentLayer;
    }
    //void SetCoordinates(double* pfX, double* pfY, int nCount)
    public void SetCoordinates(double[] pfX, double[] pfY, int nCount)
    {
      m_nCount = nCount;
      m_fX = new double[m_nCount];
      m_fY = new double[m_nCount];

      for (int i = 0; i < m_nCount; i++)
      {
        m_fX[i] = pfX[i];
        m_fY[i] = pfY[i];
      }
    }
    //void WriteTGF(CFile* pTgfFile, int nLayerNdx)
    public void WriteTGF(StreamWriter tgfFile, int nLayerNdx)
    {
      tgfFile.Write(string.Format("P{0:d}{1:d3}", nLayerNdx, m_nCount));
      for (int i = 0; i < m_nCount; i++)
      {
        tgfFile.Write(string.Format("{0:d5}{1:d5}", (int)(m_fX[i] * 10000), (int)(m_fY[i] * 10000)));
      }
      tgfFile.Write(string.Format("\n"));
    }
    //void WriteEMF(CMetaFileDC* pMetaFile)
    public void WriteEMF(Graphics EMFile)
    {
      PointF[] linePoint;

      // Convert the points
      linePoint = new PointF[m_nCount];
      for (int i = 0; i < m_nCount; i++)
      {
        linePoint[i].X = (float)m_fX[i];
        linePoint[i].Y = (float)m_fY[i];
      }
      // Draw the polyline
      EMFile.DrawLines(m_TgfLayer.LayerPen, linePoint);
    }
  }
}
