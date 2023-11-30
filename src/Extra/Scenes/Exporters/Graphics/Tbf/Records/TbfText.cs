using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TbfText.
  /// </summary>
  public class TbfText : TbfRecord
  {
    private TbfPoint3 m_StartPt;
    private float m_Height;
    private float m_RotationAngle;
    private string m_TextString;

    public TbfText()
    {
    }
    public TbfPoint3 StartPt
    {
      get { return m_StartPt; }
      set { m_StartPt = value; }
    }
    public float Height
    {
      get { return m_Height; }
      set { m_Height = value; }
    }
    public float RotationAngle
    {
      get { return m_RotationAngle; }
      // convert angle to radians
      set { m_RotationAngle = value; }
    }
    public string TextString
    {
      get { return m_TextString; }
      set { m_TextString = value; }
    }

    public bool WriteTBF(BinaryWriter tbfBinaryWriter)
    {
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TEXT, "0"))
        return false;

      /* MOVETO */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_MOVETO, StartPt))
        return false;

      /* TEXTSIZE */
      // default the ratio to 1f
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TEXTSIZE, Height, 1f))
        return false;

      /* TEXTSTYLE */
      // default the style to "STD"
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TEXTSTYLE, "STD"))
        return false;

      /* TEXTFONT */
      // default the font to "ROMANS"
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TEXTFONT, "ROMANS"))
        return false;

      /* TEXTANG */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TEXTANG, RotationAngle))
        return false;

      /* TEXTJUST */
      // default the justification to "BL" (bottom left)
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TEXTJUST, "BL"))
        return false;

      /* TR_TEXTSKEW */
      // default the skew to 0
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TEXTSKEW, 0f))
        return false;

      /* TEXTSTRING */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TEXTSTRING, TextString))
        return false;

      /* TR_END */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, ""))
        return false;

      return true;
    }
  }
}
