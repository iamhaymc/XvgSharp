using System;
using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TbfArc.
  /// </summary>
  public class TbfArc : TbfRecord
  {
    private TbfPoint3 m_CenterPt;
    private float m_Radius;
    private float m_StartAngle;
    private float m_EndAngle;
    private bool m_bTransform;
    private string m_szTransMatrix;
    private string m_LineTypeName;

    public TbfArc()
    {
      m_bTransform = false;
    }
    public TbfPoint3 CenterPt
    {
      get { return m_CenterPt; }
      set { m_CenterPt = value; }
    }
    public float Radius
    {
      get { return m_Radius; }
      set { m_Radius = value; }
    }
    public float StartAngle
    {
      get { return m_StartAngle; }
      // convert angle to radians
      set { m_StartAngle = value; }
    }
    public float EndAngle
    {
      get { return m_EndAngle; }
      // convert angle to radians
      set { m_EndAngle = value; }
    }
    public string TransformationMatrix
    {
      get { return m_szTransMatrix; }
      set
      {
        m_bTransform = true;
        m_szTransMatrix = value;
      }
    }
    public bool IsTransformed
    {
      get { return m_bTransform; }
    }
    public string LineTypeName
    {
      get { return m_LineTypeName; }
      set { m_LineTypeName = value; }
    }

    public bool WriteTBF(BinaryWriter tbfBinaryWriter)
    {
      TbfPoint3 p0, p1, p2;
      float angle_m;
      //			/* START */
      //			Point3D p0 = new Point3D(CenterPt.X + Radius, CenterPt.Y, CenterPt.Z);
      //			/* END */
      //			Point3D p1 = new Point3D(CenterPt.X - Radius, CenterPt.Y, CenterPt.Z);
      //			/* MID */
      //			Point3D p2 = new Point3D(CenterPt.X, CenterPt.Y + Radius, CenterPt.Z);

      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LINE, "0"))
        return false;

      /* LTYPE */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPE, LineTypeName))
        return false;

      /* START POINT */
      p0.X = (float)(CenterPt.X + (Radius * Math.Cos(StartAngle)));
      p0.Y = (float)(CenterPt.Y + (Radius * Math.Sin(StartAngle)));
      p0.Z = CenterPt.Z;

      /* END POINT */
      p2.X = (float)(CenterPt.X + (Radius * Math.Cos(EndAngle)));
      p2.Y = (float)(CenterPt.Y + (Radius * Math.Sin(EndAngle)));
      p2.Z = CenterPt.Z;

      /* ARC MID POINT */
      if (StartAngle < EndAngle)
      {
        angle_m = (float)((StartAngle + EndAngle) / 2.0);
      }
      else
      {
        // This is a special case. Usually occurs when arc crosses 0, but
        // can also occur if end angle == 0. If this occurs, angle_m==0
        // produces end pt & mid pt at same (X,Y,Z).  Not Good!
        // Adding 360 deg to start angle accurately produces mid-angle:
        angle_m = (float)((StartAngle + EndAngle + (2 * Math.PI)) / 2.0);
      }

      p1.X = (float)(CenterPt.X + (Radius * Math.Cos(angle_m)));
      p1.Y = (float)(CenterPt.Y + (Radius * Math.Sin(angle_m)));
      p1.Z = CenterPt.Z;

      /* OUTPUT POINTS */
      /* MOVETO */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_MOVETO, p0))
        return false;
      /* ARCTO */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_ARCTO, p1))
        return false;
      /* DRAWTO */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, p2))
        return false;
      /* TR_END */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, ""))
        return false;

      return true;
    }

  }
}
