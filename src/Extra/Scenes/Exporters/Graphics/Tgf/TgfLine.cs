using System;
using System.Drawing;
using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TgfLine.
  /// </summary>
  public class TgfLine
  {
    private int m_nLineType;
    private TgfLayer m_TgfLayer;
    private double m_fStartX;
    private double m_fStartY;
    private double m_fEndX;
    private double m_fEndY;
    private double m_fLtScale;

    public TgfLine(TgfLayer parentLayer)
    {
      m_TgfLayer = parentLayer;
    }

    public void SetCoordinates(double fStartX, double fStartY, double fEndX, double fEndY, int nLineType, double fLtScale)
    {
      m_fStartX = fStartX;
      m_fStartY = fStartY;
      m_fEndX = fEndX;
      m_fEndY = fEndY;
      m_nLineType = nLineType;
      m_fLtScale = fLtScale;
    }
    //void WriteTGF(CFile* pTgfFile, int nLayerNdx)
    public void WriteTGF(StreamWriter tgfFile, int nLayerNdx)
    {
      tgfFile.Write(string.Format("L{0:d}{1:d}{2:d5}{3:d5}{4:d5}{5:d5}{6:d5}\n", nLayerNdx, m_nLineType, (int)(m_fStartX * 10000), (int)(m_fStartY * 10000), (int)(m_fEndX * 10000), (int)(m_fEndY * 10000), (int)(m_fLtScale * 10000)));
    }
    //void WriteEMF(CMetaFileDC* pMetaFile)
    public void WriteEMF(Graphics EMFile)
    {
      // Draw the line
      switch (m_nLineType)
      {
        case (int)TGFLINETYPES.CONTINUOUS:
          WriteOutLineSegment(EMFile, m_fStartX, m_fStartY, m_fEndX, m_fEndY);
          break;
        case (int)TGFLINETYPES.HIDDEN:
          WriteOutHiddenLineSegments(EMFile);
          break;
        case (int)TGFLINETYPES.CENTER:
          WriteOutCenteredLineSegments(EMFile);
          break;
        case (int)TGFLINETYPES.PHANTOM:
          WriteOutPhantomLineSegments(EMFile);
          break;
      }
    }
    //void WriteOutHiddenLineSegments(CMetaFileDC* pMetaFile)
    void WriteOutHiddenLineSegments(Graphics EMFile)
    {
      // calc deltas and line angle
      double dDeltaX = (m_fEndX - m_fStartX); //fabs
      double dDeltaY = (m_fEndY - m_fStartY); //fabs
      double dLineAngle = Math.Atan2(dDeltaY, dDeltaX);

      // calc cos and sin of angle for use later.
      double cosAngle = Math.Cos(dLineAngle);
      double sinAngle = Math.Sin(dLineAngle);

      // calc line distance  h^2 = x^2 + y^2.
      double LineLength = Math.Sqrt(Math.Pow(dDeltaX, 2) + Math.Pow(dDeltaY, 2));

      // calc min dist for hidden pattern.
      double mindist = .375 * m_TgfLayer.ParentTgfFile.DefaultLineTypeScale * m_fLtScale;

      double DashLength = (mindist * .6667);
      double SpaceLength = (mindist * .3333);

      //calc number of cycles
      int numCycles = (int)(LineLength / mindist);

      if (numCycles == 0)
        WriteOutLineSegment(EMFile, m_fStartX, m_fStartY, m_fEndX, m_fEndY);
      else
      {

        //calc end dash length  = (total length - (min length * cycles)) - space) divided in 2)
        double EndDashLength = (((LineLength - (mindist * (numCycles - 1))) - SpaceLength) / 2);

        // first segment.
        double CurX = m_fStartX;
        double CurY = m_fStartY;
        double vertX = m_fStartX + (cosAngle * EndDashLength);
        double vertY = m_fStartY + (sinAngle * EndDashLength);

        WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);

        for (int i = 0; i < numCycles - 1; i++)
        {
          //next segment.
          CurX = vertX + (cosAngle * SpaceLength);
          CurY = vertY + (sinAngle * SpaceLength);
          vertX = CurX + (cosAngle * DashLength);
          vertY = CurY + (sinAngle * DashLength);

          WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);
        }

        //last segment.
        CurX = vertX + (cosAngle * SpaceLength);
        CurY = vertY + (sinAngle * SpaceLength);
        vertX = CurX + (cosAngle * EndDashLength);
        vertY = CurY + (sinAngle * EndDashLength);

        WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);
      }
    }
    //void WriteOutCenteredLineSegments(CMetaFileDC* pMetaFile)
    void WriteOutCenteredLineSegments(Graphics EMFile)
    {
      // calc deltas and line angle
      double dDeltaX = (m_fEndX - m_fStartX); //fabs
      double dDeltaY = (m_fEndY - m_fStartY); //fabs
      double dLineAngle = Math.Atan2(dDeltaY, dDeltaX);

      // calc cos and sin of angle for use later.
      double cosAngle = Math.Cos(dLineAngle);
      double sinAngle = Math.Sin(dLineAngle);

      // calc line distance  h^2 = x^2 + y^2.
      double LineLength = Math.Sqrt(Math.Pow(dDeltaX, 2) + Math.Pow(dDeltaY, 2));

      // calc min dist for hidden pattern.
      double mindist = 2.000 * m_TgfLayer.ParentTgfFile.DefaultLineTypeScale * m_fLtScale;

      double DashLength = (mindist * .625);     //5/8
      double ShortDashLength = (mindist * .125);   //1/8
      double SpaceLength = (mindist * .125);    //1/8

      //calc number of cycles
      int numCycles = (int)(LineLength / mindist);

      if (numCycles == 0)
        WriteOutLineSegment(EMFile, m_fStartX, m_fStartY, m_fEndX, m_fEndY);
      else
      {

        //calc end dash length  = (total length - (min length * cycles)) - (space-shortdash-space)) divided in 2)
        double EndDashLength = (((LineLength - (mindist * (numCycles - 1))) - (SpaceLength * 3)) / 2);

        // first segment.
        double CurX = m_fStartX;
        double CurY = m_fStartY;
        double vertX = m_fStartX + (cosAngle * EndDashLength);
        double vertY = m_fStartY + (sinAngle * EndDashLength);

        WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);

        //first short segment.
        CurX = vertX + (cosAngle * SpaceLength);
        CurY = vertY + (sinAngle * SpaceLength);
        vertX = CurX + (cosAngle * ShortDashLength);
        vertY = CurY + (sinAngle * ShortDashLength);

        WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);

        for (int i = 0; i < numCycles - 1; i++)
        {
          //next segment.
          CurX = vertX + (cosAngle * SpaceLength);
          CurY = vertY + (sinAngle * SpaceLength);
          vertX = CurX + (cosAngle * DashLength);
          vertY = CurY + (sinAngle * DashLength);

          WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);

          //short segment.
          CurX = vertX + (cosAngle * SpaceLength);
          CurY = vertY + (sinAngle * SpaceLength);
          vertX = CurX + (cosAngle * ShortDashLength);
          vertY = CurY + (sinAngle * ShortDashLength);

          WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);

        }

        //last segment.
        CurX = vertX + (cosAngle * SpaceLength);
        CurY = vertY + (sinAngle * SpaceLength);
        vertX = CurX + (cosAngle * EndDashLength);
        vertY = CurY + (sinAngle * EndDashLength);

        WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);
      }
    }
    //void WriteOutPhantomLineSegments(CMetaFileDC* pMetaFile)
    void WriteOutPhantomLineSegments(Graphics EMFile)
    {
      // calc deltas and line angle
      double dDeltaX = (m_fEndX - m_fStartX); //fabs
      double dDeltaY = (m_fEndY - m_fStartY); //fabs
      double dLineAngle = Math.Atan2(dDeltaY, dDeltaX);

      // calc cos and sin of angle for use later.
      double cosAngle = Math.Cos(dLineAngle);
      double sinAngle = Math.Sin(dLineAngle);

      // calc line distance  h^2 = x^2 + y^2.
      double LineLength = Math.Sqrt(Math.Pow(dDeltaX, 2) + Math.Pow(dDeltaY, 2));

      //TGH 021309 if entity ltscale can not be set to zero, treat in equations a 1.0
      if (m_fLtScale == 0.0)
      {
        m_fLtScale = 1;
      }

      // calc min dist for hidden pattern.
      double mindist = 2.500 * m_TgfLayer.ParentTgfFile.DefaultLineTypeScale * m_fLtScale;

      double DashLength = (mindist * .500);     //5/10
      double ShortDashLength = (mindist * .100);   //1/10
      double SpaceLength = (mindist * .100);    //1/10

      //calc number of cycles
      int numCycles = (int)(LineLength / mindist);

      if (numCycles == 0)
        WriteOutLineSegment(EMFile, m_fStartX, m_fStartY, m_fEndX, m_fEndY);
      else
      {

        //calc end dash length  = (total length - (min length * cycles)) - (space-shdash-space-shdash-space)) divided in 2)
        double EndDashLength = (((LineLength - (mindist * (numCycles - 1))) - (SpaceLength * 5)) / 2);

        // first segment.
        double CurX = m_fStartX;
        double CurY = m_fStartY;
        double vertX = m_fStartX + (cosAngle * EndDashLength);
        double vertY = m_fStartY + (sinAngle * EndDashLength);

        WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);

        //first short segment1.
        CurX = vertX + (cosAngle * SpaceLength);
        CurY = vertY + (sinAngle * SpaceLength);
        vertX = CurX + (cosAngle * ShortDashLength);
        vertY = CurY + (sinAngle * ShortDashLength);

        WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);

        //first short segment2.
        CurX = vertX + (cosAngle * SpaceLength);
        CurY = vertY + (sinAngle * SpaceLength);
        vertX = CurX + (cosAngle * ShortDashLength);
        vertY = CurY + (sinAngle * ShortDashLength);

        WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);

        for (int i = 0; i < numCycles - 1; i++)
        {
          //next segment.
          CurX = vertX + (cosAngle * SpaceLength);
          CurY = vertY + (sinAngle * SpaceLength);
          vertX = CurX + (cosAngle * DashLength);
          vertY = CurY + (sinAngle * DashLength);

          WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);

          //short segment1.
          CurX = vertX + (cosAngle * SpaceLength);
          CurY = vertY + (sinAngle * SpaceLength);
          vertX = CurX + (cosAngle * ShortDashLength);
          vertY = CurY + (sinAngle * ShortDashLength);

          WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);

          //short segment2.
          CurX = vertX + (cosAngle * SpaceLength);
          CurY = vertY + (sinAngle * SpaceLength);
          vertX = CurX + (cosAngle * ShortDashLength);
          vertY = CurY + (sinAngle * ShortDashLength);

          WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);
        }

        //last segment.
        CurX = vertX + (cosAngle * SpaceLength);
        CurY = vertY + (sinAngle * SpaceLength);
        vertX = CurX + (cosAngle * EndDashLength);
        vertY = CurY + (sinAngle * EndDashLength);

        WriteOutLineSegment(EMFile, CurX, CurY, vertX, vertY);
      }
    }

    //void WriteOutLineSegment(CMetaFileDC* pMetaFile, double fStartX, double fStartY, double fEndX, double fEndY)
    void WriteOutLineSegment(Graphics EMFile, double fStartX, double fStartY, double fEndX, double fEndY)
    {
      EMFile.DrawLine(m_TgfLayer.LayerPen, (float)fStartX, (float)fStartY, (float)fEndX, (float)fEndY);
    }
  }
}
