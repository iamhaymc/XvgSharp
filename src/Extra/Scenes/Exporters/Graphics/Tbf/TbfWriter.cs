using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace Trane.Submittals.Pipeline
{
  public interface ITbfWriter
  {
    bool AddLine(float fStartX, float fStartY, float fStartZ, float fEndX, float fEndY, float fEndZ, short nLineType, float fLineTypeScale);
    bool AddCircle(float fCenterX, float fCenterY, float fCenterZ, float fRadius, short nLineType, float fLineTypeScale, bool bTransform, string szTransMatrix);
    bool AddArc(float fCenterX, float fCenterY, float fCenterZ, float fRadius, float fStartAngle, float fEndAngle, short nLineType, float fLineTypeScale, bool bTransform, string szTransMatrix);
    bool AddText(float fStartX, float fStartY, float fStartZ, float fHeight, float fRotationAngle, string szTextString);
    bool Add3DSolid(TbfSolidType type, float fStartX, float fStartY, float fStartZ, float fEndX, float fEndY, float fEndZ, float fVertexX, float fVertexY, float fVertexZ, bool bHideStartToVertex, bool bHideVertexToEnd, bool bHideEndToStart);
    bool Add3DSolid(TbfSolidType type, float fStartX, float fStartY, float fStartZ, float fEndX, float fEndY, float fEndZ, bool bTransform, string szTransMatrix);
    bool Add3DSolid(TbfSolidType type, float fCenterX, float fCenterY, float fCenterZ, float fRadius);
    bool Add3DSolid(TbfSolidType type, float fCenterX, float fCenterY, float fCenterZ, float fRadius, float fVertexX, float fVertexY, float fVertexZ, bool bTransform, string szTransMatrix);
    bool Add3DSolid(TbfSolidType type, float fCenterX, float fCenterY, float fCenterZ, float fRadius, float fHeight, bool bTransform, string szTransMatrix);
    bool Add3DSolid(TbfSolidType type, float fCenterX, float fCenterY, float fCenterZ, float fRadius, float fHeight, float fVertexX, float fVertexY, float fVertexZ, bool bTransform, string szTransMatrix);
    bool Add3DSolid(TbfSolidType type, float fCenterX, float fCenterY, float fCenterZ, float fRadius, float fHeight, float fEndRadius, bool bCone, bool bTransform, string szTransMatrix);
    bool AddHotspot(short tbfHSType, float fStartX, float fStartY, float fStartZ, string szHotSpotName);
    bool Add3dArc(float fStartX, float fStartY, float fStartZ, float fEndX, float fEndY, float fEndZ, float fVertexX, float fVertexY, float fVertexZ, short nLineType, float fLineTypeScale, float fNormalX, float fNormalY, float fNormalZ);
    bool Add3dCircle(float fStartX, float fStartY, float fStartZ, float fEndX, float fEndY, float fEndZ, float fVertexX, float fVertexY, float fVertexZ, short nLineType, float fLineTypeScale, float fNormalX, float fNormalY, float fNormalZ);
    int CountEntities();
    bool WriteTbf(string filePath, string entryName);
  }

  public class TbfWriter : ITbfWriter
  {
    private int m_nTbfVersion;
    private ArrayList m_LineArray;
    private ArrayList m_CircleArray;
    private ArrayList m_ArcArray;
    private ArrayList m_TextArray;
    private ArrayList m_3DSolidArray;
    private ArrayList m_HotSpotArray;
    private ArrayList m_3DArcArray;
    private ArrayList m_3DCircleArray;
    private TbfLineTypeDefs m_LineTypeDefArray;

    public TbfWriter()
    {
      m_LineArray = new ArrayList();
      m_CircleArray = new ArrayList();
      m_ArcArray = new ArrayList();
      m_TextArray = new ArrayList();
      m_3DSolidArray = new ArrayList();
      m_HotSpotArray = new ArrayList();
      m_3DArcArray = new ArrayList();
      m_3DCircleArray = new ArrayList();
      m_LineTypeDefArray = new TbfLineTypeDefs();
    }

    public int CountEntities()
    {
      return m_LineArray.Count + m_CircleArray.Count + m_ArcArray.Count + m_TextArray.Count + m_3DSolidArray.Count + m_HotSpotArray.Count + m_3DArcArray.Count + m_3DCircleArray.Count;
    }

    public bool WriteTbf(string filePath, string entryName)
    {
      bool bRetVal = true;
      using (MemoryStream tbfStream = new MemoryStream((int)10000))
      using (BinaryWriter tbfBinaryWriter = new BinaryWriter(tbfStream))
      {
        int i;

        TbfPoint3 p1;
        p1.X = p1.Y = p1.Z = 0.0f;
        TbfPoint3 p2;
        p2.X = p2.Y = p2.Z = 10.0f;

        TbfHeader tbfHeader = new TbfHeader();
        bRetVal = tbfHeader.WriteTbfHeader(tbfBinaryWriter, p1, p2, m_LineTypeDefArray);

        // loop through the arrays of entities to write each entity to the TBF
        for (i = 0; i < m_LineArray.Count && bRetVal; i++)
        {
          bRetVal = ((TbfLine)m_LineArray[i]).WriteTBF(tbfBinaryWriter);
        }
        for (i = 0; i < m_CircleArray.Count && bRetVal; i++)
        {
          bRetVal = ((TbfCircle)m_CircleArray[i]).WriteTBF(tbfBinaryWriter);
        }
        for (i = 0; i < m_ArcArray.Count && bRetVal; i++)
        {
          bRetVal = ((TbfArc)m_ArcArray[i]).WriteTBF(tbfBinaryWriter);
        }
        for (i = 0; i < m_TextArray.Count && bRetVal; i++)
        {
          bRetVal = ((TbfText)m_TextArray[i]).WriteTBF(tbfBinaryWriter);
        }
        for (i = 0; i < m_3DSolidArray.Count && bRetVal; i++)
        {
          bRetVal = ((Tbf3DSolid)m_3DSolidArray[i]).WriteTBF(tbfBinaryWriter);
        }
        for (i = 0; i < m_HotSpotArray.Count && bRetVal; i++)
        {
          bRetVal = ((TbfHotSpot)m_HotSpotArray[i]).WriteTBF(tbfBinaryWriter);
        }
        for (i = 0; i < m_3DArcArray.Count && bRetVal; i++)
        {
          bRetVal = ((Tbf3dArc)m_3DArcArray[i]).WriteTBF(tbfBinaryWriter);
        }
        for (i = 0; i < m_3DCircleArray.Count && bRetVal; i++)
        {
          bRetVal = ((Tbf3dCircle)m_3DCircleArray[i]).WriteTBF(tbfBinaryWriter);
        }

        if (File.Exists(filePath))
          File.Delete(filePath);

        if (CountEntities() != 0 && tbfStream.Length < int.MaxValue)
        {
          tbfStream.Position = 0;
          FsZip.EntryOut(filePath, entryName, tbfStream);
          //using (ClrZip tbfZip = ClrZip.OpenWrite(filePath))
          //{
          //  using (ClrZipEntry tbfEntry = tbfZip.OpenEntryByName("TBF_DDP"))
          //  {
          //    tbfEntry.Write(tbfStream);
          //  }
          //}
        }
      }
      return bRetVal;
    }

    public bool AddLine(float fStartX, float fStartY, float fStartZ, float fEndX, float fEndY, float fEndZ, short nLineType, float fLineTypeScale)
    {
      TbfLine tbfLine = new TbfLine();
      tbfLine.StartPt = new TbfPoint3(fStartX, fStartY, fStartZ);
      tbfLine.EndPt = new TbfPoint3(fEndX, fEndY, fEndZ);
      if ((TbfLineType)nLineType == TbfLineType.Continuous)
      {
        tbfLine.LineTypeName = "CONTINUOUS";
      }
      else
      {
        string lineTypeName;
        m_LineTypeDefArray.AddDefinition((TbfLineType)nLineType, fLineTypeScale, out lineTypeName);
        tbfLine.LineTypeName = lineTypeName;
      }
      m_LineArray.Add(tbfLine);
      return true;
    }
    public bool AddCircle(float fCenterX, float fCenterY, float fCenterZ, float fRadius, short nLineType, float fLineTypeScale, bool bTransform, string szTransMatrix)
    {
      TbfCircle tbfCircle = new TbfCircle();
      tbfCircle.CenterPt = new TbfPoint3(fCenterX, fCenterY, fCenterZ);
      tbfCircle.Radius = fRadius;
      if (bTransform)
        tbfCircle.TransformationMatrix = szTransMatrix;
      if ((TbfLineType)nLineType == TbfLineType.Continuous)
      {
        tbfCircle.LineTypeName = "CONTINUOUS";
      }
      else
      {
        string lineTypeName;
        m_LineTypeDefArray.AddDefinition((TbfLineType)nLineType, fLineTypeScale, out lineTypeName);
        tbfCircle.LineTypeName = lineTypeName;
      }
      m_CircleArray.Add(tbfCircle);
      return true;
    }
    public bool AddArc(float fCenterX, float fCenterY, float fCenterZ, float fRadius, float fStartAngle, float fEndAngle, short nLineType, float fLineTypeScale, bool bTransform, string szTransMatrix)
    {
      TbfArc tbfArc = new TbfArc();
      tbfArc.CenterPt = new TbfPoint3(fCenterX, fCenterY, fCenterZ);
      tbfArc.Radius = fRadius;
      tbfArc.StartAngle = fStartAngle;
      tbfArc.EndAngle = fEndAngle;
      if (bTransform)
        tbfArc.TransformationMatrix = szTransMatrix;
      if ((TbfLineType)nLineType == TbfLineType.Continuous)
      {
        tbfArc.LineTypeName = "CONTINUOUS";
      }
      else
      {
        string lineTypeName;
        m_LineTypeDefArray.AddDefinition((TbfLineType)nLineType, fLineTypeScale, out lineTypeName);
        tbfArc.LineTypeName = lineTypeName;
      }
      m_ArcArray.Add(tbfArc);
      return true;
    }
    public bool AddText(float fStartX, float fStartY, float fStartZ, float fHeight, float fRotationAngle, string szTextString)
    {
      TbfText tbfText = new TbfText();
      tbfText.StartPt = new TbfPoint3(fStartX, fStartY, fStartZ);
      tbfText.Height = fHeight;
      tbfText.RotationAngle = fRotationAngle;
      tbfText.TextString = szTextString;
      m_TextArray.Add(tbfText);
      return true;
    }
    // overloaded for the parameters of a Face
    public bool Add3DSolid(TbfSolidType type, float fStartX, float fStartY, float fStartZ, float fEndX, float fEndY, float fEndZ, float fVertexX, float fVertexY, float fVertexZ, bool bHideStartToVertex, bool bHideVertexToEnd, bool bHideEndToStart)
    {
      Tbf3DSolid tbf3DSolid = new Tbf3DSolid();
      tbf3DSolid.Type = type;
      tbf3DSolid.StartPt = new TbfPoint3(fStartX, fStartY, fStartZ);
      tbf3DSolid.EndPt = new TbfPoint3(fEndX, fEndY, fEndZ);
      tbf3DSolid.Vertex = new TbfPoint3(fVertexX, fVertexY, fVertexZ);
      tbf3DSolid.HideStartToVertex = bHideStartToVertex;
      tbf3DSolid.HideVertexToEnd = bHideVertexToEnd;
      tbf3DSolid.HideEndToStart = bHideEndToStart;
      m_3DSolidArray.Add(tbf3DSolid);
      return true;
    }
    // overloaded for the parameters of a box and a wedge
    public bool Add3DSolid(TbfSolidType type, float fStartX, float fStartY, float fStartZ, float fEndX, float fEndY, float fEndZ, bool bTransform, string szTransMatrix)
    {
      Tbf3DSolid tbf3DSolid = new Tbf3DSolid();
      tbf3DSolid.Type = type;
      tbf3DSolid.StartPt = new TbfPoint3(fStartX, fStartY, fStartZ);
      tbf3DSolid.EndPt = new TbfPoint3(fEndX, fEndY, fEndZ);
      if (bTransform)
        tbf3DSolid.TransformationMatrix = szTransMatrix;
      m_3DSolidArray.Add(tbf3DSolid);
      return true;
    }
    // overloaded for the parameters of a Sphere
    public bool Add3DSolid(TbfSolidType type, float fCenterX, float fCenterY, float fCenterZ, float fRadius)
    {
      Tbf3DSolid tbf3DSolid = new Tbf3DSolid();
      tbf3DSolid.Type = type;
      tbf3DSolid.CenterPt = new TbfPoint3(fCenterX, fCenterY, fCenterZ);
      tbf3DSolid.Radius = fRadius;
      m_3DSolidArray.Add(tbf3DSolid);
      return true;
    }
    // overloaded for the parameters of a Half Sphere
    public bool Add3DSolid(TbfSolidType type, float fCenterX, float fCenterY, float fCenterZ, float fRadius, float fVertexX, float fVertexY, float fVertexZ, bool bTransform, string szTransMatrix)
    {
      Tbf3DSolid tbf3DSolid = new Tbf3DSolid();
      tbf3DSolid.Type = type;
      tbf3DSolid.CenterPt = new TbfPoint3(fCenterX, fCenterY, fCenterZ);
      tbf3DSolid.Radius = fRadius;
      tbf3DSolid.Vertex = new TbfPoint3(fVertexX, fVertexY, fVertexZ);
      if (bTransform)
        tbf3DSolid.TransformationMatrix = szTransMatrix;
      m_3DSolidArray.Add(tbf3DSolid);
      return true;
    }
    // overloaded for the parameters of a Cylinder
    public bool Add3DSolid(TbfSolidType type, float fCenterX, float fCenterY, float fCenterZ, float fRadius, float fHeight, bool bTransform, string szTransMatrix)
    {
      Tbf3DSolid tbf3DSolid = new Tbf3DSolid();
      tbf3DSolid.Type = type;
      tbf3DSolid.CenterPt = new TbfPoint3(fCenterX, fCenterY, fCenterZ);
      tbf3DSolid.Radius = fRadius;
      tbf3DSolid.Height = fHeight;
      if (bTransform)
        tbf3DSolid.TransformationMatrix = szTransMatrix;
      m_3DSolidArray.Add(tbf3DSolid);
      return true;
    }
    // overloaded for the parameters of a Half Cylinder
    public bool Add3DSolid(TbfSolidType type, float fCenterX, float fCenterY, float fCenterZ, float fRadius, float fHeight, float fVertexX, float fVertexY, float fVertexZ, bool bTransform, string szTransMatrix)
    {
      Tbf3DSolid tbf3DSolid = new Tbf3DSolid();
      tbf3DSolid.Type = type;
      tbf3DSolid.CenterPt = new TbfPoint3(fCenterX, fCenterY, fCenterZ);
      tbf3DSolid.Radius = fRadius;
      tbf3DSolid.Height = fHeight;
      tbf3DSolid.Vertex = new TbfPoint3(fVertexX, fVertexY, fVertexZ);
      if (bTransform)
        tbf3DSolid.TransformationMatrix = szTransMatrix;
      m_3DSolidArray.Add(tbf3DSolid);
      return true;
    }
    // overloaded for the parameters of a Cone (bool bCone is a bogus parameter, used to make the parameter list different than the box)
    public bool Add3DSolid(TbfSolidType type, float fCenterX, float fCenterY, float fCenterZ, float fRadius, float fHeight, float fEndRadius, bool bCone, bool bTransform, string szTransMatrix)
    {
      Tbf3DSolid tbf3DSolid = new Tbf3DSolid();
      tbf3DSolid.Type = type;
      tbf3DSolid.CenterPt = new TbfPoint3(fCenterX, fCenterY, fCenterZ);
      tbf3DSolid.Radius = fRadius;
      tbf3DSolid.Height = fHeight;
      tbf3DSolid.EndRadius = fEndRadius;
      if (bTransform)
        tbf3DSolid.TransformationMatrix = szTransMatrix;
      m_3DSolidArray.Add(tbf3DSolid);
      return true;
    }
    public bool AddHotspot(short tbfHSType, float fStartX, float fStartY, float fStartZ, string szHotSpotName)
    {
      TbfHotSpot tbfHotSpot = new TbfHotSpot();
      tbfHotSpot.HotSpotPt = new TbfPoint3(fStartX, fStartY, fStartZ);
      tbfHotSpot.HotSpotName = szHotSpotName;
      tbfHotSpot.HotSpotType = (TbfHSType)tbfHSType;
      m_HotSpotArray.Add(tbfHotSpot);
      return true;
    }
    public bool AddHotspot(TbfHSType tbfHSType, float fStartX, float fStartY, float fStartZ, string szHotSpotName)
    {
      TbfHotSpot tbfHotSpot = new TbfHotSpot();
      tbfHotSpot.HotSpotPt = new TbfPoint3(fStartX, fStartY, fStartZ);
      tbfHotSpot.HotSpotName = szHotSpotName;
      tbfHotSpot.HotSpotType = tbfHSType;
      m_HotSpotArray.Add(tbfHotSpot);
      return true;
    }
    public bool Add3dArc(float fStartX, float fStartY, float fStartZ, float fEndX, float fEndY, float fEndZ, float fVertexX, float fVertexY, float fVertexZ, short nLineType, float fLineTypeScale, float fNormalX, float fNormalY, float fNormalZ)
    {
      Tbf3dArc tbf3dArc = new Tbf3dArc();
      tbf3dArc.StartPt = new TbfPoint3(fStartX, fStartY, fStartZ);
      tbf3dArc.EndPt = new TbfPoint3(fEndX, fEndY, fEndZ);
      tbf3dArc.VertexPt = new TbfPoint3(fVertexX, fVertexY, fVertexZ);
      tbf3dArc.NormalVector = new TbfPoint3(fNormalX, fNormalY, fNormalZ);
      if ((TbfLineType)nLineType == TbfLineType.Continuous)
      {
        tbf3dArc.LineTypeName = "CONTINUOUS";
      }
      else
      {
        string lineTypeName;
        m_LineTypeDefArray.AddDefinition((TbfLineType)nLineType, fLineTypeScale, out lineTypeName);
        tbf3dArc.LineTypeName = lineTypeName;
      }
      m_3DArcArray.Add(tbf3dArc);
      return true;
    }
    public bool Add3dCircle(float fStartX, float fStartY, float fStartZ, float fEndX, float fEndY, float fEndZ, float fVertexX, float fVertexY, float fVertexZ, short nLineType, float fLineTypeScale, float fNormalX, float fNormalY, float fNormalZ)
    {
      Tbf3dCircle tbf3dCircle = new Tbf3dCircle();
      tbf3dCircle.StartPt = new TbfPoint3(fStartX, fStartY, fStartZ);
      tbf3dCircle.EndPt = new TbfPoint3(fEndX, fEndY, fEndZ);
      tbf3dCircle.VertexPt = new TbfPoint3(fVertexX, fVertexY, fVertexZ);
      tbf3dCircle.NormalVector = new TbfPoint3(fNormalX, fNormalY, fNormalZ);
      if ((TbfLineType)nLineType == TbfLineType.Continuous)
      {
        tbf3dCircle.LineTypeName = "CONTINUOUS";
      }
      else
      {
        string lineTypeName;
        m_LineTypeDefArray.AddDefinition((TbfLineType)nLineType, fLineTypeScale, out lineTypeName);
        tbf3dCircle.LineTypeName = lineTypeName;
      }
      m_3DCircleArray.Add(tbf3dCircle);
      return true;
    }
  }
}
