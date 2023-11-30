using System;
using System.Drawing;
using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TgfArc.
  /// </summary>
  public class TgfArc
  {
    private TgfLayer m_TgfLayer;
    private short m_nLineType;
    private double m_fLtScale;
    private double m_fCenterX;
    private double m_fCenterY;
    private double m_fRadius;
    private double m_fStartAngle;
    private double m_fSweepAngle;
    private double m_fUpperLeftX;       // X coordinate of upper left corner of bounding rectangle
    private double m_fUpperLeftY;       // Y coordinate of upper left corner of bounding rectangle
    private double m_fLowerRightX;         // X coordinate of lower right corner of bounding rectangle
    private double m_fLowerRightY;         // Y coordinate of lower right corner of bounding rectangle

    public TgfArc(TgfLayer parentLayer)
    {
      m_TgfLayer = parentLayer;
    }
    public void SetCoordinates(double fCenterX, double fCenterY, double fRadius, double fStartAngle, double fSweepAngle, short nLineType, double fLtScale)
    {
      m_fCenterX = fCenterX;
      m_fCenterY = fCenterY;
      m_fRadius = fRadius;
      m_fStartAngle = fStartAngle;
      //m_fAltStartAngle = m_TgfLayer.ParentTgfFile.ConvertAngle(360 - fStartAngle - fSweepAngle);
      if (fSweepAngle >= 360.0)
      {
        m_fSweepAngle = 360.0;
        m_fStartAngle = 0;
      }
      else
        m_fSweepAngle = fSweepAngle;

      m_nLineType = nLineType;
      m_fLtScale = fLtScale;

      m_fUpperLeftX = m_fCenterX - m_fRadius;
      m_fUpperLeftY = m_fCenterY + m_fRadius;
      m_fLowerRightX = m_fCenterX + m_fRadius;
      m_fLowerRightY = m_fCenterY - m_fRadius;
    }
    //private void WriteEMF(CMetaFileDC *pMetaFile);
    public void WriteEMF(Graphics EMFile)
    {
      switch (m_nLineType)
      {
        case (int)TGFLINETYPES.CONTINUOUS:
          WriteOutArcSegment(EMFile, m_fStartAngle, m_fSweepAngle);
          break;
        case (int)TGFLINETYPES.HIDDEN:
          if (m_fSweepAngle < 360.0)
            WriteOutHiddenArcSegments(EMFile);
          else
            WriteOutHiddenCircle(EMFile);
          break;
        case (int)TGFLINETYPES.CENTER:
          if (m_fSweepAngle < 360.0)
            WriteOutCenteredArcSegments(EMFile);
          else
            WriteOutCenteredCircle(EMFile);
          break;
        case (int)TGFLINETYPES.PHANTOM:
          if (m_fSweepAngle < 360.0)
            WriteOutPhantomArcSegments(EMFile);
          else
            WriteOutPhantomCircle(EMFile);
          break;
      }
    }
    //private void WriteTGF(CFile* pTgfFile, int nLayerNdx)
    public void WriteTGF(StreamWriter tgfFile, int nLayerNdx)
    {
      //szOutput.Format("A%d%d%+08d%+08d%07d%05d%05d%05d\n", nLayerNdx, m_nLineType, (int)(m_fCenterX*10000), (int)(m_fCenterY*10000), (int)(m_fRadius*10000), (int)(m_fStartAngle*100), (int)(m_fSweepAngle*100), (int)(m_fLtScale*10000));
      tgfFile.Write(string.Format("A{0:d}{1:d}{2}{3:d7}{4}{5:d7}{6:d7}{7:d5}{8:d5}{9:d5}\n", nLayerNdx, m_nLineType, m_fCenterX > 0 ? "+" : "-", Math.Abs((int)(m_fCenterX * 10000)), m_fCenterY > 0 ? "+" : "-", Math.Abs((int)(m_fCenterY * 10000)), (int)(m_fRadius * 10000), (int)(m_fStartAngle * 100), (int)(m_fSweepAngle * 100), (int)(m_fLtScale * 10000)));
    }
    //void WriteOutHiddenArcSegments(CMetaFileDC* pMetaFile)
    void WriteOutHiddenArcSegments(Graphics EMFile)
    {
      // get arc length
      // calc cord length (sweep angle / 360) * 2Pi * Radius.
      double dCordLength = (m_fSweepAngle / 360) * (2 * Math.PI * m_fRadius);

      // if arc is not to small then add it to TGF file
      if (dCordLength > 0.0001)
      {
        // calc min dist for hidden pattern.
        double mindist = .375 * m_TgfLayer.ParentTgfFile.DefaultLineTypeScale * m_fLtScale;

        double DashLength = (mindist * .6667);
        double SpaceLength = (mindist * .3333);

        //calc number of cycles
        int numCycles = (int)(dCordLength / mindist);

        if (numCycles == 0)
          WriteOutArcSegment(EMFile, m_fStartAngle, m_fSweepAngle);
        else
        {
          double dStartAngle = m_fStartAngle;

          //calc end dash length  = (total length - (min length * cycles)) - space) divided in 2)
          double EndDashLength = (((dCordLength - (mindist * (numCycles - 1))) - SpaceLength) / 2);

          //convert cords to angles.
          double EndDashAngle = (EndDashLength * 360) / (2 * Math.PI * m_fRadius);
          double DashAngle = (DashLength * 360) / (2 * Math.PI * m_fRadius);
          double SpaceAngle = (SpaceLength * 360) / (2 * Math.PI * m_fRadius);

          // first segment.
          WriteOutArcSegment(EMFile, dStartAngle, EndDashAngle);
          dStartAngle = dStartAngle + EndDashAngle;

          for (int i = 0; i < numCycles - 1; i++)
          {
            //next segment.
            dStartAngle = dStartAngle + SpaceAngle;
            WriteOutArcSegment(EMFile, dStartAngle, DashAngle);
            dStartAngle = dStartAngle + DashAngle;
          }

          //last segment.
          dStartAngle = dStartAngle + SpaceAngle;
          double dSweepAngle = m_fSweepAngle - dStartAngle;
          WriteOutArcSegment(EMFile, dStartAngle, EndDashAngle);
        }
      }
    }
    //void WriteOutCenteredArcSegments(CMetaFileDC* pMetaFile)
    void WriteOutCenteredArcSegments(Graphics EMFile)
    {
      // get arc length
      // calc cord length (sweep angle / 360) * 2Pi * Radius.
      double dCordLength = (m_fSweepAngle / 360) * (2 * Math.PI * m_fRadius);

      // if arc is not to small then add it to TGF file
      if (dCordLength > 0.0001)
      {
        // calc min dist for hidden pattern.
        double mindist = 2.000 * m_TgfLayer.ParentTgfFile.DefaultLineTypeScale * m_fLtScale;

        double DashLength = (mindist * .625);     //5/8
        double ShortDashLength = (mindist * .125);   //1/8
        double SpaceLength = (mindist * .125);    //1/8

        //calc number of cycles
        int numCycles = (int)(dCordLength / mindist);

        if (numCycles == 0)
          WriteOutArcSegment(EMFile, m_fStartAngle, m_fSweepAngle);
        else
        {
          double dStartAngle = m_fStartAngle;

          //calc end dash length  = (total length - (min length * cycles)) - space) divided in 2)
          double EndDashLength = (((dCordLength - (mindist * (numCycles - 1))) - (SpaceLength * 3)) / 2);

          //convert cords to angles.
          double EndDashAngle = (EndDashLength * 360) / (2 * Math.PI * m_fRadius);
          double DashAngle = (DashLength * 360) / (2 * Math.PI * m_fRadius);
          double SpaceAngle = (SpaceLength * 360) / (2 * Math.PI * m_fRadius);
          double ShDashAngle = SpaceAngle;

          // first segment.
          WriteOutArcSegment(EMFile, dStartAngle, EndDashAngle);
          if ((dStartAngle = dStartAngle + EndDashAngle) > 360)
            dStartAngle = dStartAngle - 360;

          // first short segment
          if ((dStartAngle = dStartAngle + SpaceAngle) > 360)
            dStartAngle = dStartAngle - 360;
          WriteOutArcSegment(EMFile, dStartAngle, ShDashAngle);
          if ((dStartAngle = dStartAngle + ShDashAngle) > 360)
            dStartAngle = dStartAngle - 360;

          for (int i = 0; i < numCycles - 1; i++)
          {
            //next segment.
            if ((dStartAngle = dStartAngle + SpaceAngle) > 360)
              dStartAngle = dStartAngle - 360;
            WriteOutArcSegment(EMFile, dStartAngle, DashAngle);
            if ((dStartAngle = dStartAngle + DashAngle) > 360)
              dStartAngle = dStartAngle - 360;

            // first short segment
            if ((dStartAngle = dStartAngle + SpaceAngle) > 360)
              dStartAngle = dStartAngle - 360;
            WriteOutArcSegment(EMFile, dStartAngle, ShDashAngle);
            if ((dStartAngle = dStartAngle + ShDashAngle) > 360)
              dStartAngle = dStartAngle - 360;
          }

          //last segment.
          if ((dStartAngle = dStartAngle + SpaceAngle) > 360)
            dStartAngle = dStartAngle - 360;
          double dSweepAngle = m_fSweepAngle - dStartAngle;
          WriteOutArcSegment(EMFile, dStartAngle, EndDashAngle);
        }
      }
    }
    //void WriteOutPhantomArcSegments(CMetaFileDC* pMetaFile)
    void WriteOutPhantomArcSegments(Graphics EMFile)
    {
      // get arc length
      // calc cord length (sweep angle / 360) * 2Pi * Radius.
      double dCordLength = (m_fSweepAngle / 360) * (2 * Math.PI * m_fRadius);

      // if arc is not to small then add it to TGF file
      if (dCordLength > 0.0001)
      {
        // calc min dist for hidden pattern.
        double mindist = 2.500 * m_TgfLayer.ParentTgfFile.DefaultLineTypeScale * m_fLtScale;

        double DashLength = (mindist * .500);     //5/10
        double ShortDashLength = (mindist * .100);   //1/10
        double SpaceLength = (mindist * .100);    //1/10

        //calc number of cycles
        int numCycles = (int)(dCordLength / mindist);

        if (numCycles == 0)
          WriteOutArcSegment(EMFile, m_fStartAngle, m_fSweepAngle);
        else
        {
          double dStartAngle = m_fStartAngle;

          //calc end dash length  = (total length - (min length * cycles)) - space) divided in 2)
          double EndDashLength = (((dCordLength - (mindist * (numCycles - 1))) - (SpaceLength * 5)) / 2);

          //convert cords to angles.
          double EndDashAngle = (EndDashLength * 360) / (2 * Math.PI * m_fRadius);
          double DashAngle = (DashLength * 360) / (2 * Math.PI * m_fRadius);
          double SpaceAngle = (SpaceLength * 360) / (2 * Math.PI * m_fRadius);
          double ShDashAngle = SpaceAngle;

          // first segment.
          WriteOutArcSegment(EMFile, dStartAngle, EndDashAngle);
          if ((dStartAngle = dStartAngle + EndDashAngle) > 360)
            dStartAngle = dStartAngle - 360;

          // first short segment1
          if ((dStartAngle = dStartAngle + SpaceAngle) > 360)
            dStartAngle = dStartAngle - 360;
          WriteOutArcSegment(EMFile, dStartAngle, ShDashAngle);
          if ((dStartAngle = dStartAngle + ShDashAngle) > 360)
            dStartAngle = dStartAngle - 360;

          // first short segment2
          if ((dStartAngle = dStartAngle + SpaceAngle) > 360)
            dStartAngle = dStartAngle - 360;
          WriteOutArcSegment(EMFile, dStartAngle, ShDashAngle);
          if ((dStartAngle = dStartAngle + ShDashAngle) > 360)
            dStartAngle = dStartAngle - 360;

          for (int i = 0; i < numCycles - 1; i++)
          {
            //next segment.
            if ((dStartAngle = dStartAngle + SpaceAngle) > 360)
              dStartAngle = dStartAngle - 360;
            WriteOutArcSegment(EMFile, dStartAngle, DashAngle);
            if ((dStartAngle = dStartAngle + DashAngle) > 360)
              dStartAngle = dStartAngle - 360;

            // short segment1
            if ((dStartAngle = dStartAngle + SpaceAngle) > 360)
              dStartAngle = dStartAngle - 360;
            WriteOutArcSegment(EMFile, dStartAngle, ShDashAngle);
            if ((dStartAngle = dStartAngle + ShDashAngle) > 360)
              dStartAngle = dStartAngle - 360;

            // short segment2
            if ((dStartAngle = dStartAngle + SpaceAngle) > 360)
              dStartAngle = dStartAngle - 360;
            WriteOutArcSegment(EMFile, dStartAngle, ShDashAngle);
            if ((dStartAngle = dStartAngle + ShDashAngle) > 360)
              dStartAngle = dStartAngle - 360;
          }

          //last segment.
          if ((dStartAngle = dStartAngle + SpaceAngle) > 360)
            dStartAngle = dStartAngle - 360;
          double dSweepAngle = m_fSweepAngle - dStartAngle;
          WriteOutArcSegment(EMFile, dStartAngle, EndDashAngle);
        }
      }
    }
    //void WriteOutHiddenCircle(CMetaFileDC* pMetaFile)
    void WriteOutHiddenCircle(Graphics EMFile)
    {
      if (m_fRadius > 0.0001)
      {
        // break circle into arcs.

        // calc circle circumference.
        double circum = 2 * Math.PI * m_fRadius;

        // calc min dist for hidden pattern.
        double mindist = .375 * m_TgfLayer.ParentTgfFile.DefaultLineTypeScale * m_fLtScale;

        //calc actual cycle length
        double cycleLength = circum / mindist;

        //calc number of cycles
        int numCycles = (int)Math.Round(cycleLength);

        // if zero cycles draw circle.
        if (cycleLength < 1) //0
          WriteOutArcSegment(EMFile, 0.0, 360.0);
        else if (cycleLength < 2.5) //1 or 2 cycles
        {
          WriteOutArcSegment(EMFile, 0.0, 120.0);
          WriteOutArcSegment(EMFile, 180.0, 120.0);
        }
        else  // >2
        {
          // simplified calc.
          double curAngle = 0;
          double Dash_Angle = (360.0f / numCycles) * 0.6667;
          double spaceAngle = (360.0f / numCycles) * 0.3333;

          for (int i = 0; i < numCycles; i++)
          {
            //draw arc
            WriteOutArcSegment(EMFile, curAngle, Dash_Angle);
            curAngle += Dash_Angle;

            //skip space
            curAngle += spaceAngle;
          }
        }
      }
    }
    //void WriteOutCenteredCircle(CMetaFileDC* pMetaFile)
    void WriteOutCenteredCircle(Graphics EMFile)
    {
      if (m_fRadius > 0.0001)
      {
        // break circle into arcs.

        // calc circle circumference.
        double circum = 2 * Math.PI * m_fRadius;

        // calc min dist for hidden pattern.
        double mindist = 2.000 * m_TgfLayer.ParentTgfFile.DefaultLineTypeScale * m_fLtScale;

        //calc actual cycle length
        double cycleLength = circum / mindist;

        //calc number of cycles
        int numCycles = (int)Math.Round(cycleLength);

        // if zero cycles draw circle.
        if (cycleLength < 1) //0
          WriteOutArcSegment(EMFile, 0.0, 360.0);
        else if (cycleLength < 2.5) //1 or 2 cycles
        {
          WriteOutArcSegment(EMFile, 0.0, 112.5);
          WriteOutArcSegment(EMFile, 135.0, 22.5);
          WriteOutArcSegment(EMFile, 180.0, 112.5);
          WriteOutArcSegment(EMFile, 315.0, 22.5);
        }
        else
        {
          // simplified calc.
          double curAngle = 0;
          double Dash_Angle = (360.0f / numCycles) * 0.625;  //5/8
          double spaceAngle = (360.0f / numCycles) * 0.125;  //1/8

          for (int i = 0; i < numCycles; i++)
          {
            //draw long dash
            WriteOutArcSegment(EMFile, curAngle, Dash_Angle);
            curAngle += Dash_Angle;

            //skip space
            curAngle += spaceAngle;

            //draw short dash
            WriteOutArcSegment(EMFile, curAngle, spaceAngle);
            curAngle += spaceAngle;

            //skip space
            curAngle += spaceAngle;
          }
        }
      }
    }
    //void WriteOutPhantomCircle(CMetaFileDC* pMetaFile)
    void WriteOutPhantomCircle(Graphics EMFile)
    {
      if (m_fRadius > 0.0001)
      {
        // break circle into arcs.

        // calc circle circumference.
        double circum = 2 * Math.PI * m_fRadius;

        // calc min dist for hidden pattern.
        double mindist = 2.500 * m_TgfLayer.ParentTgfFile.DefaultLineTypeScale * m_fLtScale;

        //calc actual cycle length
        double cycleLength = circum / mindist;

        //calc number of cycles
        int numCycles = (int)Math.Round(cycleLength);

        // if zero cycles draw circle.
        if (cycleLength < 1) //0
          WriteOutArcSegment(EMFile, 0.0, 360.0);
        else if (cycleLength < 2.5)   //1 or 2 cycles
        {
          WriteOutArcSegment(EMFile, 0.0, 90.0);
          WriteOutArcSegment(EMFile, 108.0, 18.0);
          WriteOutArcSegment(EMFile, 144.0, 18.0);

          WriteOutArcSegment(EMFile, 180.0, 90.0);
          WriteOutArcSegment(EMFile, 288.0, 18.0);
          WriteOutArcSegment(EMFile, 324.0, 18.0);
        }
        else
        {
          // simplified calc.
          double curAngle = 0;
          double Dash_Angle = (360.0f / numCycles) * 0.500;  //5/10
          double spaceAngle = (360.0f / numCycles) * 0.100;  //1/10

          for (int i = 0; i < numCycles; i++)
          {
            //draw long dash
            WriteOutArcSegment(EMFile, curAngle, Dash_Angle);
            curAngle += Dash_Angle;

            //skip space
            curAngle += spaceAngle;

            //draw short dash
            WriteOutArcSegment(EMFile, curAngle, spaceAngle);
            curAngle += spaceAngle;

            //skip space
            curAngle += spaceAngle;

            //draw short dash
            WriteOutArcSegment(EMFile, curAngle, spaceAngle);
            curAngle += spaceAngle;

            //skip space
            curAngle += spaceAngle;
          }
        }
      }
    }
    //void WriteOutArcSegment(CMetaFileDC* pMetaFile, double fStartAngle, double fSweepAngle)
    private void WriteOutArcSegment(Graphics EMFile, double fStartAngle, double fSweepAngle)
    {
      // Draw the arc
      RectangleF test = m_TgfLayer.ParentTgfFile.CorrectRectangle((float)m_fUpperLeftX,
          (float)m_fUpperLeftY,
          (float)m_fLowerRightX,
          (float)m_fLowerRightY);
      EMFile.DrawArc(m_TgfLayer.LayerPen, m_TgfLayer.ParentTgfFile.CorrectRectangle((float)m_fUpperLeftX,
          (float)m_fUpperLeftY,
          (float)m_fLowerRightX,
          (float)m_fLowerRightY), (float)fStartAngle, (float)fSweepAngle);
    }



  }


}
