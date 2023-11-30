using System;
using System.Drawing;
using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TgfEllipse.
  /// </summary>
  public class TgfEllipse
  {
    private TgfLayer m_TgfLayer;
    private float m_fUpperLeftX;
    private float m_fUpperLeftY;
    private float m_fLowerRightX;
    private float m_fLowerRightY;
    private float m_fRotationAngle;

    public TgfEllipse(TgfLayer parentLayer)
    {
      m_TgfLayer = parentLayer;
    }
    //void WriteTGF(CFile* pTgfFile, int nLayerNdx)
    public void WriteTGF(StreamWriter tgfFile, int nLayerNdx)
    {
      tgfFile.Write(string.Format("E{0:d}{1:d5}{2:d5}{3:d5}{4:d5}{5:d5}\n", nLayerNdx, (int)(m_fUpperLeftX * 10000), (int)(m_fUpperLeftY * 10000), (int)(m_fLowerRightX * 10000), (int)(m_fLowerRightY * 10000), (int)(m_fRotationAngle * 100)));
    }
    public void SetCoordinates(double fUpperLeftX, double fUpperLeftY, double fLowerRightX, double fLowerRightY, double fRotationAngle)
    {
      m_fUpperLeftX = (float)fUpperLeftX;
      m_fUpperLeftY = (float)fUpperLeftY;
      m_fLowerRightX = (float)fLowerRightX;
      m_fLowerRightY = (float)fLowerRightY;
      m_fRotationAngle = (float)fRotationAngle;
    }
    //void WriteEMF(CMetaFileDC *pMetaFile)
    public void WriteEMF(Graphics EMFile)
    {
      float centerPtX = (m_fUpperLeftX + m_fLowerRightX) / 2.0f;
      float centerPtY = (m_fUpperLeftY + m_fLowerRightY) / 2.0f;

      if (m_fRotationAngle != 0)
      {
        EMFile.TranslateTransform(centerPtX, centerPtY);
        EMFile.RotateTransform(m_fRotationAngle);
        EMFile.DrawEllipse(m_TgfLayer.LayerPen, m_fUpperLeftX - centerPtX, m_fLowerRightY - centerPtY, Math.Abs(m_fUpperLeftX - m_fLowerRightX), Math.Abs(m_fUpperLeftY - m_fLowerRightY));
        m_TgfLayer.ParentTgfFile.SetBaseTranslation(EMFile);
      }
      else
        EMFile.DrawEllipse(m_TgfLayer.LayerPen, m_fUpperLeftX, m_fLowerRightY, Math.Abs(m_fUpperLeftX - m_fLowerRightX), Math.Abs(m_fUpperLeftY - m_fLowerRightY));
    }
  }
}
