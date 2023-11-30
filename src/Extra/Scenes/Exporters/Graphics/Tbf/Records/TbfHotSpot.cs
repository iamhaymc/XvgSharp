using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TbfHotSpot.
  /// </summary>
  public class TbfHotSpot : TbfRecord
  {
    private TbfPoint3 m_HotSpotPt;
    private string m_szHotSpotName;
    private TbfHSType m_HotSpotType;

    public TbfHotSpot()
    {
      m_HotSpotType = TbfHSType.Standard;
    }
    public TbfPoint3 HotSpotPt
    {
      get { return m_HotSpotPt; }
      set { m_HotSpotPt = value; }
    }
    public string HotSpotName
    {
      get { return m_szHotSpotName; }
      set { m_szHotSpotName = value; }
    }
    public TbfHSType HotSpotType
    {
      get { return m_HotSpotType; }
      set { m_HotSpotType = value; }
    }

    public bool WriteTBF(BinaryWriter tbfBinaryWriter)
    {
      bool bRetVal = true;

      if (HotSpotType == TbfHSType.Standard)
      {
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_AUTODIMHS, "HS"))
          return false;
      }
      else if (HotSpotType == TbfHSType.LeaderNote)
      {
        //061306 TGH test to add additional leadernote functions to ddp
        //if(!WriteTbfTagAndData( tbfBinaryWriter, (byte)TagCodes.TR_AUTODIM, "0"))
        //	return false;
        //if(!WriteTbfTagAndData( tbfBinaryWriter, (byte)TagCodes.TR_AUTODIMTYPE, 2))
        //	return false;
        //if(!WriteTbfTagAndData( tbfBinaryWriter, (byte)TagCodes.TR_AUTODIMON, 0))
        //	return false;
        //if(!WriteTbfTagAndData( tbfBinaryWriter, (byte)TagCodes.TR_AUTODIMSYMBOL, HotSpotName))
        //	return false;
        //if(!WriteTbfTagAndData( tbfBinaryWriter, (byte)TagCodes.TR_AUTODIMDESCR, HotSpotName))
        //	return false;
        //if(!WriteTbfTagAndData( tbfBinaryWriter, (byte)TagCodes.TR_AUTODIMVIEWINDEX, 4))//top
        //	return false;
        //if(!WriteTbfTagAndData( tbfBinaryWriter, (byte)TagCodes.TR_AUTODIMVIEWNAME, "_VPTOP"))//top
        //	return false;
        //if(!WriteTbfTagAndData( tbfBinaryWriter, (byte)TagCodes.TR_AUTODIMVIEWCODE, 4))//top
        //	return false;
        //if(!WriteTbfTagAndData( tbfBinaryWriter, (byte)TagCodes.TR_AUTODIMLNOTEAUTO, 1))//auto
        //	return false;
        //if (!WriteTbfTagAndData( tbfBinaryWriter, (byte)TagCodes.TR_END, "AUTODIM" ))
        //	return false;
        ////////////////////////////////////////////////////////////////////////

        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_AUTODIMHSL, "HS"))
          return false;
      }
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_AUTODIMHSNAME, HotSpotName))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_AUTODIMHSLOC, HotSpotPt))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, ""))
        return false;

      return bRetVal;
    }
  }
}
