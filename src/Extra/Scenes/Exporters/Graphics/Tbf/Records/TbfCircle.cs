using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TbfCircle.
  /// </summary>
  public class TbfCircle : TbfRecord
  {
    private TbfPoint3 m_CenterPt;
    private float m_Radius;
    private bool m_bTransform;
    private string m_szTransMatrix;
    private string m_LineTypeName;

    public TbfCircle()
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
      //			/* START */
      //			Point3D p0 = new Point3D(CenterPt.X + Radius, CenterPt.Y, CenterPt.Z);
      //			/* END */
      //			Point3D p1 = new Point3D(CenterPt.X - Radius, CenterPt.Y, CenterPt.Z);
      //			/* MID */
      //			Point3D p2 = new Point3D(CenterPt.X, CenterPt.Y + Radius, CenterPt.Z);

      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_POLYGON, "0"))
        return false;

      /* LTYPE */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPE, LineTypeName))
        return false;

      /* OUTPUT POINTS */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_RINGFROM, new TbfPoint3(CenterPt.X + Radius, CenterPt.Y, CenterPt.Z)))
        return false;

      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_CIRCLETO, new TbfPoint3(CenterPt.X - Radius, CenterPt.Y, CenterPt.Z)))
        return false;

      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, new TbfPoint3(CenterPt.X, CenterPt.Y + Radius, CenterPt.Z)))
        return false;

      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_ENDDRAW, new TbfPoint3(CenterPt.X + Radius, CenterPt.Y, CenterPt.Z)))
        return false;

      /* TR_END */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, ""))
        return false;

      return true;
    }
  }
}
