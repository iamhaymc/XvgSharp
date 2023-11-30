using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for Tbf3dArc.
  /// </summary>
  public class Tbf3dArc : TbfRecord
  {
    private TbfPoint3 m_StartPt;
    private TbfPoint3 m_EndPt;
    private TbfPoint3 m_VertexPt;
    private string m_LineTypeName;
    private TbfPoint3 m_NormalVector;

    public Tbf3dArc()
    {
    }
    public TbfPoint3 StartPt
    {
      get { return m_StartPt; }
      set { m_StartPt = value; }
    }
    public TbfPoint3 VertexPt
    {
      get { return m_VertexPt; }
      set { m_VertexPt = value; }
    }
    public TbfPoint3 EndPt
    {
      get { return m_EndPt; }
      set { m_EndPt = value; }
    }
    public TbfPoint3 NormalVector
    {
      get { return m_NormalVector; }
      set { m_NormalVector = value; }
    }
    public string LineTypeName
    {
      get { return m_LineTypeName; }
      set { m_LineTypeName = value; }
    }

    public bool WriteTBF(BinaryWriter tbfBinaryWriter)
    {
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LINE, "0"))
        return false;

      /* LTYPE */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPE, LineTypeName))
        return false;

      // PUSH
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_PUSH))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_PREUCS, NormalVector))
        return false;

      /* OUTPUT POINTS */
      /* MOVETO */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_MOVETO, StartPt))
        return false;
      /* ARCTO */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_ARCTO, VertexPt))
        return false;
      /* DRAWTO */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, EndPt))
        return false;

      // POP
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_POP))
        return false;

      /* TR_END */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, ""))
        return false;


      return true;
    }

  }
}
