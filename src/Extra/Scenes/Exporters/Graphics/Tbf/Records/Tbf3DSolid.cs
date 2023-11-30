using System;
using System.IO;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for Tbf3DSolid.
  /// </summary>
  public class Tbf3DSolid : TbfRecord
  {
    private TbfSolidType m_solidType;
    private TbfPoint3 m_StartPt;
    private TbfPoint3 m_EndPt;
    private TbfPoint3 m_CenterPt;
    private float m_Radius;
    private float m_Height;
    private float m_EndRadius;
    private TbfPoint3 m_Vertex;
    private bool m_HideStartToVertex;
    private bool m_HideVertexToEnd;
    private bool m_HideEndToStart;
    private bool m_bTransform;
    private string m_szTransMatrix;

    public Tbf3DSolid()
    {
      m_bTransform = false;
    }
    public TbfSolidType Type
    {
      get { return m_solidType; }
      set { m_solidType = value; }
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
    public float Radius
    {
      get { return m_Radius; }
      set { m_Radius = value; }
    }
    public float Height
    {
      get { return m_Height; }
      set { m_Height = value; }
    }
    public TbfPoint3 CenterPt
    {
      get { return m_CenterPt; }
      set { m_CenterPt = value; }
    }
    public float EndRadius
    {
      get { return m_EndRadius; }
      set { m_EndRadius = value; }
    }
    public TbfPoint3 Vertex
    {
      get { return m_Vertex; }
      set { m_Vertex = value; }
    }
    public bool HideStartToVertex
    {
      get { return m_HideStartToVertex; }
      set { m_HideStartToVertex = value; }
    }
    public bool HideVertexToEnd
    {
      get { return m_HideVertexToEnd; }
      set { m_HideVertexToEnd = value; }
    }
    public bool HideEndToStart
    {
      get { return m_HideEndToStart; }
      set { m_HideEndToStart = value; }
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

    public bool WriteTBF(BinaryWriter tbfBinaryWriter)
    {
      bool bRetVal = false;

      switch (Type)
      {
        case TbfSolidType.Face:
          bRetVal = CreateOutputFace(tbfBinaryWriter, StartPt, Vertex, EndPt, HideStartToVertex, HideVertexToEnd, HideEndToStart);
          break;
        case TbfSolidType.Cylinder:
          bRetVal = CreateOutputCylinder(tbfBinaryWriter);
          break;
        case TbfSolidType.Box:
          bRetVal = CreateOutputBox(tbfBinaryWriter);
          break;
        default:
          break;
      }

      return bRetVal;
    }
    //////////////////////////////////////////////////////
    //outputs a 3dFace
    bool CreateOutputFace(BinaryWriter tbfBinaryWriter, TbfPoint3 p1, TbfPoint3 p2, TbfPoint3 p3, bool hideP1toP2, bool hideP2toP3, bool hideP3toP1)
    {
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_POLYGON, "0"))
        return false;

      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_RINGFROM, p1))
        return false;

      if (hideP1toP2)
      {
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_SOFTTO, p2))
          return false;
      }
      else
      {
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, p2))
          return false;
      }

      if (hideP2toP3)
      {
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_SOFTTO, p3))
          return false;
      }
      else
      {
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, p3))
          return false;
      }

      if (hideP3toP1)
      {
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_ENDSOFT, p1))
          return false;
      }
      else
      {
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_ENDDRAW, p1))
          return false;
      }

      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, ""))
        return false;

      return true;
    }
    //////////////////////////////////////////////////////
    //outputs a 3dBox - box must have a normal of (0,0,1)
    bool CreateOutputBox(BinaryWriter tbfBinaryWriter)
    //void CreateOutputBox(FILE* fp, AcGePoint3d Locpt, double Xlen, double Ylen, double Zlen)
    {
      TbfPoint3[] CornerPt = new TbfPoint3[8];

      CornerPt[0].X = EndPt.X;
      CornerPt[0].Y = StartPt.Y;
      CornerPt[0].Z = EndPt.Z;
      //v0[0] = Locpt.x + Xlen;
      //v0[1] = Locpt.y;
      //v0[2] = Locpt.z + Zlen;

      CornerPt[1] = EndPt;
      //v1[0] = Locpt.x + Xlen;
      //v1[1] = Locpt.y + Ylen;
      //v1[2] = Locpt.z + Zlen;

      CornerPt[2].X = StartPt.X;
      CornerPt[2].Y = EndPt.Y;
      CornerPt[2].Z = EndPt.Z;
      //v2[0] = Locpt.x;
      //v2[1] = Locpt.y + Ylen;
      //v2[2] = Locpt.z + Zlen;

      CornerPt[3].X = StartPt.X;
      CornerPt[3].Y = StartPt.Y;
      CornerPt[3].Z = EndPt.Z;
      //v3[0] = Locpt.x;
      //v3[1] = Locpt.y;
      //v3[2] = Locpt.z + Zlen;

      CornerPt[4].X = EndPt.X;
      CornerPt[4].Y = StartPt.Y;
      CornerPt[4].Z = StartPt.Z;
      //v4[0] = Locpt.x + Xlen;
      //v4[1] = Locpt.y;
      //v4[2] = Locpt.z;

      CornerPt[5].X = EndPt.X;
      CornerPt[5].Y = EndPt.Y;
      CornerPt[5].Z = StartPt.Z;
      //v5[0] = Locpt.x + Xlen;
      //v5[1] = Locpt.y + Ylen;
      //v5[2] = Locpt.z;

      CornerPt[6].X = StartPt.X;
      CornerPt[6].Y = EndPt.Y;
      CornerPt[6].Z = StartPt.Z;
      //v6[0] = Locpt.x;
      //v6[1] = Locpt.y + Ylen;
      //v6[2] = Locpt.z;

      CornerPt[7] = StartPt;
      //v7[0] = Locpt.x;
      //v7[1] = Locpt.y;
      //v7[2] = Locpt.z;

      //OutputBox(tbfBinaryWriter, v0, v1, v2, v3, v4, v5, v6, v7);
      //OutputBox(tbfBinaryWriter, v0, EndPt, v2, v3, v4, v5, v6, StartPt);
      if (!OutputBox(tbfBinaryWriter, CornerPt))
        return false;
      else
        return true;
    }
    //////////////////////////////////////////////////////
    //outputs a 3dCylinder
    bool CreateOutputCylinder(BinaryWriter tbfBinaryWriter)
    {
      bool bRetVal = true;
      TbfPoint3 TopCenterPt = CenterPt;

      TopCenterPt.Z = CenterPt.Z + Height;

      TbfCircle tbfCircle = new TbfCircle();
      tbfCircle.CenterPt = CenterPt;
      tbfCircle.Radius = Radius;
      tbfCircle.WriteTBF(tbfBinaryWriter); //draw bottom circle
      tbfCircle.CenterPt = TopCenterPt;
      tbfCircle.Radius = Radius;
      tbfCircle.WriteTBF(tbfBinaryWriter); //draw top circle

      TbfPoint3[] Bpts = new TbfPoint3[16];
      TbfPoint3[] Tpts = new TbfPoint3[16];
      int i;

      // create a loop that calcs point on circle.
      ///use 22.5 degrees because it is 1/16 of cicle
      for (i = 0; i < 16; i++)
      {
        Bpts[i] = UtilPolar(CenterPt, i * Math.PI / 8, Radius);
        Tpts[i] = UtilPolar(TopCenterPt, i * Math.PI / 8, Radius);
      }

      //base circle
      for (i = 0; i < 15 && bRetVal; i++)
      {
        bRetVal = Output3SidedFace(tbfBinaryWriter, Bpts[i], Tpts[i], Tpts[i + 1]); //tri 1
        if (bRetVal)
          bRetVal = Output3SidedFace(tbfBinaryWriter, Bpts[i], Bpts[i + 1], Tpts[i + 1]); //tri 2
      }
      //finish last edge with 4 edged polygon.
      if (bRetVal)
        bRetVal = Output3SidedFace(tbfBinaryWriter, Bpts[i], Tpts[0], Tpts[15]); //tri 1
      if (bRetVal)
        bRetVal = Output3SidedFace(tbfBinaryWriter, Bpts[i], Bpts[15], Tpts[15]); //tri 2

      return bRetVal;
    }
    bool OutputBox(BinaryWriter tbfBinaryWriter, TbfPoint3[] CornerPt)
    //bool OutputBox(BinaryWriter tbfBinaryWriter, ads_point v0,ads_point v1,ads_point v2,ads_point v3,
    //	ads_point v4,ads_point v5,ads_point v6,ads_point v7)
    {
      //face1
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[0], CornerPt[1], CornerPt[2]))//tri 1
        return false;
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[0], CornerPt[3], CornerPt[2]))//tri 2
        return false;
      //face2
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[3], CornerPt[2], CornerPt[6]))//tri 3
        return false;
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[3], CornerPt[7], CornerPt[6]))//tri 4
        return false;
      //face3
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[0], CornerPt[1], CornerPt[5]))//tri 5
        return false;
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[0], CornerPt[4], CornerPt[5]))//tri 6
        return false;
      //face4
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[1], CornerPt[5], CornerPt[6]))//tri 7
        return false;
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[1], CornerPt[2], CornerPt[6]))//tri 8
        return false;
      //face5
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[0], CornerPt[4], CornerPt[7]))//tri 9
        return false;
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[0], CornerPt[3], CornerPt[7]))//tri 10
        return false;
      //face6
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[4], CornerPt[5], CornerPt[6]))//tri 11
        return false;
      if (!Output3SidedFace(tbfBinaryWriter, CornerPt[4], CornerPt[7], CornerPt[6]))//tri 12
        return false;

      return true;
    }
    bool Output3SidedFace(BinaryWriter tbfBinaryWriter, TbfPoint3 p1, TbfPoint3 p2, TbfPoint3 p3)
    {
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_POLYGON, "0"))
        return false;
      if (!Draw3DFaceEdge(tbfBinaryWriter, p1, false, true, false))
        return false;
      if (!Draw3DFaceEdge(tbfBinaryWriter, p2, false, false, false))
        return false;
      if (!Draw3DFaceEdge(tbfBinaryWriter, p3, false, false, false))
        return false;
      if (!Draw3DFaceEdge(tbfBinaryWriter, p3, true, false, false))
        return false;
      if (!Draw3DFaceEdge(tbfBinaryWriter, p1, true, false, true))
        return false;
      if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_END, ""))
        return false;

      return true;
    }
    // Output a 3dface edge according to its visibility.
    bool Draw3DFaceEdge(BinaryWriter tbfBinaryWriter, TbfPoint3 p0, bool bInvisible, bool bStart, bool bEnd)
    {
      if (bStart)
      {
        if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_RINGFROM, p0))
          return false;
      }
      else if (bEnd)
      {
        if (bInvisible)
        {
          if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_ENDSOFT, p0))
            return false;
        }
        else
        {
          if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_ENDDRAW, p0))
            return false;
        }
      }
      else
      {
        if (bInvisible)
        {
          if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_SOFTTO, p0))
            return false;
        }
        else
        {
          if (!WriteTbfTagAndData(tbfBinaryWriter, (byte)TbfTagCode.TR_DRAWTO, p0))
            return false;
        }
      }
      return true;
    }
    // calculates a new point based on start point, angle (in radians) and distance.
    TbfPoint3 UtilPolar(TbfPoint3 StartPt, double fAngleRadians, double fDistance)
    {
      TbfPoint3 endpt;

      endpt.X = (float)(StartPt.X + Math.Cos(fAngleRadians) * fDistance);
      endpt.Y = (float)(StartPt.Y + Math.Sin(fAngleRadians) * fDistance);
      endpt.Z = StartPt.Z;

      return endpt;
    }
  }
}
