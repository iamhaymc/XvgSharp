using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TbfLine.
  /// </summary>
  public class TbfLine : TbfRecord
  {
    private TbfPoint3 m_StartPt;
    private TbfPoint3 m_EndPt;
    private string m_LineTypeName;

    public TbfLine()
    {
    }

    public TbfPoint3 StartPt
    {
      get { return m_StartPt; }
      set { m_StartPt = value; }
    }
    public TbfPoint3 EndPt
    {
      get { return m_EndPt; }
      set { m_EndPt = value; }
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

      /* MOVETO */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_MOVETO, StartPt))
        return false;

      /* DRAWTO */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, EndPt))
        return false;

      /* TR_END */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, ""))
        return false;

      return true;
    }
  }
}
