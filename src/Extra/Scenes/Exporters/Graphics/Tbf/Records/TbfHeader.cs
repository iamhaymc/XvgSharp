using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TbfHeader.
  /// </summary>
  public class TbfHeader : TbfRecord
  {
    public TbfHeader()
    {
    }

    internal bool WriteTbfHeader(BinaryWriter tbfBinaryWriter, TbfPoint3 lmin, TbfPoint3 lmax, TbfLineTypeDefs lineTypeDefArray)
    {
      TbfPoint3 point1;
      TbfPoint3 point2;

      /* DRAWDEF */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWDEF))
        return false;
      /* VERSION */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TBFVERSION, string.Format("{0:f1}", (float)TbfVersionType.CURRENT)))
        return false;
      /* DATA EXTENTS */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_EXTENTS, lmin, lmax))
        return false;
      /* END DRAWDEF*/
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "DRAWDEF"))
        return false;

      //OUTPUT CURRENT VIEW//
      // VIEWDEF
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_VIEWDEF, "Current"))
        return false;
      // EYE
      point1.X = 16.65f;
      point1.Y = 10.53f;
      point1.Z = 1;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_EYE, point1))
        return false;
      // OBJ
      point1.X = 17.65f;
      point1.Y = 11.53f;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_OBJ, point1))
        return false;
      // NORM
      point1.X = -1.57735026918963f;
      point1.Y = -1.57735026918963f;
      point1.Z = 0.57735026918963f;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_NORM, point1))
        return false;
      // MINMAX
      point1.X = -27.838269653969f;
      point1.Y = -23.113425721873f;
      point1.Z = -1000000000.0000f;
      point2.X = 27.838269653969f;
      point2.Y = 23.113425721873f;
      point2.Z = 1.7303187567613f;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_MINMAX, point1, point2))
        return false;
      // TYPE
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TYPE, "O"))
        return false;
      // ANGLE
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TYPE, 0.0000f))
        return false;
      // AXON
      point1.X = 0.0f;
      point1.Y = 0.0f;
      point1.Z = 0.0f;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_AXON, point1))
        return false;
      // VIEWPORT
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_VIEWPORT, 0.0000f, 0.0000f, 0.0000f, 0.0000f))
        return false;
      // VIEWROTATE
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_VIEWROTATE, (int)0))
        return false;
      // TURN OFF LAYERS (JUST PAPERSPACE)
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LAYEROFF, "PAPERSPACE"))
        return false;
      // END CURRENT VIEW
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "VIEWDEF"))
        return false;

      //OUTPUT TOP VIEW//
      // VIEWDEF
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_VIEWDEF, "TOP"))
        return false;
      // EYE
      point1.X = 7.93714f;
      point1.Y = 4.5f;
      point1.Z = 1f;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_EYE, point1))
        return false;
      // OBJ
      point1.X = 7.93714f;
      point1.Y = 4.5f;
      point1.Z = 0f;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_OBJ, point1))
        return false;
      // NORM
      point1.X = 0;
      point1.Y = 0;
      point1.Z = 1;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_NORM, point1))
        return false;
      // MINMAX
      point1.X = -100;
      point1.Y = -100;
      point1.Z = -1000000000.0000f;
      point2.X = 100;
      point2.Y = 100;
      point2.Z = 100;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_MINMAX, point1, point2))
        return false;
      // TYPE
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_TYPE, "O"))
        return false;
      // ANGLE
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_ANG, 0.0000f))
        return false;
      // AXON
      point1.X = 0.0f;
      point1.Y = 0.0f;
      point1.Z = 0.0f;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_AXON, point1))
        return false;
      // VIEWPORT
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_VIEWPORT, 0.0000f, 0.0000f, 0.0000f, 0.0000f))
        return false;
      // VIEWROTATE
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_VIEWROTATE, (int)0))
        return false;
      // TURN OFF LAYERS (JUST PAPERSPACE)
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_VIEWROTATE, "PAPERSPACE"))
        return false;
      // END CURRENT VIEW
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "VIEWDEF"))
        return false;

      //OUTPUT LAYERS
      //Layer Paperspace
      /* LAYERDEF */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LAYERDEF, "PAPERSPACE"))
        return false;
      /* STATUS */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LAYERSTATUS, (int)64))
        return false;
      /* COLOR */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_COLOR, (int)7))
        return false;
      /* LTYPE */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPE, "CONTINUOUS"))
        return false;
      /* END */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "LAYERDEF"))
        return false;

      //Layer '0'
      /* LAYERDEF */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LAYERDEF, "0"))
        return false;
      /* STATUS */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LAYERSTATUS, (int)0))
        return false;
      /* COLOR */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_COLOR, (int)7))
        return false;
      /* LTYPE */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPE, "CONTINUOUS"))
        return false;
      /* END */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "LAYERDEF"))
        return false;

      // Output Line Type Definition - CONTINUOUS
      /* LINETYPEDEF */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LINETYPEDEF, "CONTINUOUS"))
        return false;
      /* SCALE */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPESCALE, 1.0000f))
        return false;
      /* SIZE */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPESIZE, 0.0000f))
        return false;
      /* END */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "LINETYPEDEF"))
        return false;

      // Output Line Type Definition - Standard HIDDEN
      if (lineTypeDefArray.StandardHiddenLTD)
      {
        /* LINETYPEDEF */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LINETYPEDEF, "HIDDEN"))
          return false;
        /* SCALE */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPESCALE, 1.0000f))
          return false;

        if (!WriteHiddenLTDAttributes(tbfBinaryWriter))
          return false;

        /* END */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "LINETYPEDEF"))
          return false;
      }
      // Output Line Type Definitions - Custom HIDDEN
      for (int i = 0; i < lineTypeDefArray.HiddenLTDCount; i++)
      {
        /* LINETYPEDEF */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LINETYPEDEF, "HIDDEN" + i.ToString()))
          return false;
        /* SCALE */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPESCALE, lineTypeDefArray.HiddenLTDScale(i)))
          return false;

        if (!WriteHiddenLTDAttributes(tbfBinaryWriter))
          return false;

        /* END */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "LINETYPEDEF"))
          return false;
      }

      // Output Line Type Definition - Standard CENTER
      if (lineTypeDefArray.StandardCenterLTD)
      {
        /* LINETYPEDEF */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LINETYPEDEF, "CENTER"))
          return false;
        /* SCALE */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPESCALE, 1.0000f))
          return false;

        if (!WriteCenterLTDAttributes(tbfBinaryWriter))
          return false;

        /* END */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "LINETYPEDEF"))
          return false;
      }
      // Output Line Type Definitions - Custom CENTER
      for (int i = 0; i < lineTypeDefArray.HiddenLTDCount; i++)
      {
        /* LINETYPEDEF */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LINETYPEDEF, "CENTER" + i.ToString()))
          return false;
        /* SCALE */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPESCALE, lineTypeDefArray.HiddenLTDScale(i)))
          return false;

        if (!WriteCenterLTDAttributes(tbfBinaryWriter))
          return false;

        /* END */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "LINETYPEDEF"))
          return false;
      }

      // Output Line Type Definition - Standard PHANTOM
      if (lineTypeDefArray.StandardHiddenLTD)
      {
        /* LINETYPEDEF */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LINETYPEDEF, "PHANTOM"))
          return false;
        /* SCALE */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPESCALE, 1.0000f))
          return false;

        if (!WritePhantomLTDAttributes(tbfBinaryWriter))
          return false;

        /* END */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "LINETYPEDEF"))
          return false;
      }
      // Output Line Type Definitions - Custom HIDDEN
      for (int i = 0; i < lineTypeDefArray.HiddenLTDCount; i++)
      {
        /* LINETYPEDEF */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LINETYPEDEF, "PHANTOM" + i.ToString()))
          return false;
        /* SCALE */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPESCALE, lineTypeDefArray.HiddenLTDScale(i)))
          return false;

        if (!WritePhantomLTDAttributes(tbfBinaryWriter))
          return false;

        /* END */
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "LINETYPEDEF"))
          return false;
      }

      // Define HS Symbol
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_SYMBOLDEF, "HS"))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_POLYGON, "0"))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_COLOR, (int)1))
        return false;
      point1.X = 0;
      point1.Y = -4.768774032592773f;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_RINGFROM, point1))
        return false;
      point1.X = -4.768774032592773f;
      point1.Y = 0;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, point1))
        return false;
      point1.X = 0;
      point1.Y = 4.768774032592773f;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, point1))
        return false;
      point1.X = 4.768774032592773f;
      point1.Y = 0;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, point1))
        return false;
      point1.X = 0;
      point1.Y = -4.768774032592773f;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_ENDDRAW, point1))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, ""))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "SymbolDef: HS"))
        return false;

      // Define HS Leader Note Symbol
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_SYMBOLDEF, "HSL"))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_POLYGON, "0"))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_COLOR, (int)1))
        return false;
      point1.X = -15.19177913665771f;
      point1.Y = -5.063926696777344f;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_RINGFROM, point1))
        return false;
      point1.X = -15.19177913665771f;
      point1.Y = 5.063926696777344f;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, point1))
        return false;
      point1.X = -5.063926696777344f;
      point1.Y = 5.063926696777344f;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, point1))
        return false;
      point1.X = 0;
      point1.Y = 0;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, point1))
        return false;
      point1.X = -5.063926696777344f;
      point1.Y = -5.063926696777344f;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, point1))
        return false;
      point1.X = -15.19177913665771f;
      point1.Y = -5.063926696777344f;
      point1.Z = 0;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_ENDDRAW, point1))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, ""))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, "SymbolDef: HSL"))
        return false;

      //SymbolDef, "HSL",
      //    Polygon, "0",
      //        Color, 1,
      //        RingFrom, -15.19177913665771, -5.063926696777344, 0,
      //        DrawTo, -15.19177913665771, 5.063926696777344, 0,
      //        DrawTo, -5.063926696777344, 5.063926696777344, 0,
      //        DrawTo, 0, 0, 0,
      //        DrawTo, -5.063926696777344, -5.063926696777344, 0,
      //        EndDraw, -15.19177913665771, -5.063926696777344, 0,
      //    End, "",
      //End, "SymbolDef: HSL",
      //SymbolDef, "HS",
      //    Polygon, "0",
      //        Color, 1,
      //        RingFrom, 0, -4.768774032592773, 0,
      //        DrawTo, -4.768774032592773, 0, 0,
      //        DrawTo, 0, 4.768774032592773, 0,
      //        DrawTo, 4.768774032592773, 0, 0,
      //        EndDraw, 0, -4.768774032592773, 0,
      //    End, "",
      //End, "SymbolDef: HS",

      return true;
    }
    private bool WriteHiddenLTDAttributes(BinaryWriter tbfBinaryWriter)
    {
      /* SIZE */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPESIZE, 0.375f))
        return false;
      /* Line Type On */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEON, 0.25f))
        return false;
      /* Line Type Off */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEOFF, -0.125f))
        return false;

      return true;
    }
    private bool WriteCenterLTDAttributes(BinaryWriter tbfBinaryWriter)
    {
      /* SIZE */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPESIZE, 2.0f))
        return false;
      /* Line Type On */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEON, 1.25f))
        return false;
      /* Line Type Off */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEOFF, -0.25f))
        return false;
      /* Line Type On */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEON, 0.25f))
        return false;
      /* Line Type Off */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEOFF, -0.25f))
        return false;

      return true;
    }
    private bool WritePhantomLTDAttributes(BinaryWriter tbfBinaryWriter)
    {
      /* SIZE */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPESIZE, 2.5f))
        return false;
      /* Line Type On */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEON, 1.25f))
        return false;
      /* Line Type Off */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEOFF, -0.25f))
        return false;
      /* Line Type On */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEON, 0.25f))
        return false;
      /* Line Type Off */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEOFF, -0.25f))
        return false;
      /* Line Type On */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEON, 0.25f))
        return false;
      /* Line Type Off */
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_LTYPEOFF, -0.25f))
        return false;

      return true;
    }
  }
}
