using System.Collections;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Summary description for TbfLineTypeDefArray.
  /// </summary>
  public class TbfLineTypeDefs
  {
    private bool m_StandardHiddenLTD;
    private bool m_StandardCenterLTD;
    private bool m_StandardPhantomLTD;
    private ArrayList m_HiddenLTDArray;    // array of the scale that this LTD is set to
    private ArrayList m_CenterLTDArray;    // array of the scale that this LTD is set to
    private ArrayList m_PhantomLTDArray;   // array of the scale that this LTD is set to

    public bool StandardHiddenLTD
    {
      get { return m_StandardHiddenLTD; }
      set { m_StandardHiddenLTD = value; }
    }
    public bool StandardCenterLTD
    {
      get { return m_StandardCenterLTD; }
      set { m_StandardCenterLTD = value; }
    }
    public bool StandardPhantomLTD
    {
      get { return m_StandardPhantomLTD; }
      set { m_StandardPhantomLTD = value; }
    }
    public int HiddenLTDCount
    {
      get { return m_HiddenLTDArray.Count; }
    }
    public int CenterLTDCount
    {
      get { return m_CenterLTDArray.Count; }
    }
    public int PhantomLTDCount
    {
      get { return m_PhantomLTDArray.Count; }
    }
    public float HiddenLTDScale(int nNdx)
    {
      return (float)m_HiddenLTDArray[nNdx];
    }
    public float CenterLTDScale(int nNdx)
    {
      return (float)m_CenterLTDArray[nNdx];
    }
    public float PhantomLTDScale(int nNdx)
    {
      return (float)m_PhantomLTDArray[nNdx];
    }

    public TbfLineTypeDefs()
    {
      m_HiddenLTDArray = new ArrayList();
      m_CenterLTDArray = new ArrayList();
      m_PhantomLTDArray = new ArrayList();
    }
    public void AddDefinition(TbfLineType lineType, float lineTypeScale, out string lineTypeName)
    {
      if (lineType == TbfLineType.Hidden)
        lineTypeName = "HIDDEN";
      else if (lineType == TbfLineType.Center)
        lineTypeName = "CENTER";
      else //if (lineType == TbfLineType.Phantom)
        lineTypeName = "PHANTOM";

      // determine if this definition already exists
      if (!DoesDefinitionExist(lineType, lineTypeScale, ref lineTypeName))
      {
        if (lineTypeScale == 1.0f)
        {
          if (lineType == TbfLineType.Hidden)
            StandardHiddenLTD = true;
          else if (lineType == TbfLineType.Center)
            StandardCenterLTD = true;
          else //if (lineType == TbfLineType.Phantom)
            StandardPhantomLTD = true;
        }
        else
        {
          int nNdx;
          // if definition does not exist, add it
          if (lineType == TbfLineType.Hidden)
          {
            nNdx = m_HiddenLTDArray.Add(lineTypeScale);
            lineTypeName += nNdx.ToString();
          }
          else if (lineType == TbfLineType.Center)
          {
            nNdx = m_CenterLTDArray.Add(lineTypeScale);
            lineTypeName += nNdx.ToString();
          }
          else //if (lineType == TbfLineType.Phantom)
          {
            nNdx = m_PhantomLTDArray.Add(lineTypeScale);
            lineTypeName += nNdx.ToString();
          }
        }
      }
    }
    public bool DoesDefinitionExist(TbfLineType lineType, float lineTypeScale, ref string lineTypeName)
    {
      bool retVal = false;

      if (lineTypeScale == 1.0f)
      {
        if (lineType == TbfLineType.Hidden)
          retVal = StandardHiddenLTD;
        else if (lineType == TbfLineType.Center)
          retVal = StandardCenterLTD;
        else //if (lineType == TbfLineType.Phantom)
          retVal = StandardPhantomLTD;
      }
      else
      {
        ArrayList definitionArray;
        if (lineType == TbfLineType.Hidden)
          definitionArray = m_HiddenLTDArray;
        else if (lineType == TbfLineType.Center)
          definitionArray = m_CenterLTDArray;
        else //if (lineType == TbfLineType.Phantom)
          definitionArray = m_PhantomLTDArray;
        for (int i = 0; i < definitionArray.Count; i++)
        {
          if ((float)definitionArray[i] == lineTypeScale)
          {
            retVal = true;
            lineTypeName += i.ToString();
          }
        }
      }

      return retVal;
    }
  }
}
