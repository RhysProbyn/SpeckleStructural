﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpeckleCore;
using SpeckleCoreGeometryClasses;

namespace SpeckleElementsClasses
{
  public enum Structural1DElementType
  {
    Generic,
    Column,
    Beam,
    Cantilever,
    Brace
  }

  public enum Structural2DElementType
  {
    Generic,
    Slab,
    Wall
  }

  [Serializable]
  public partial class StructuralNode : SpecklePoint, IStructural
  {
    public override string Type { get => base.Type + "/StructuralNode"; }

    [JsonIgnore]
    private Dictionary<string, object> StructuralProperties
    {
      get
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        if (!base.Properties.ContainsKey("structural"))
          base.Properties["structural"] = new Dictionary<string, object>();

        return base.Properties["structural"] as Dictionary<string, object>;

      }
      set
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        base.Properties["structural"] = value;
      }
    }

    /// <summary>Base SpecklePoint.</summary>
    [JsonIgnore]
    public SpecklePoint basePoint
    {
      get => this as SpecklePoint;
      set => this.Value = value.Value;
    }

    /// <summary>Local axis of the node.</summary>
    [JsonIgnore]
    public StructuralAxis Axis
    {
      get => StructuralProperties.ContainsKey("axis") ? (StructuralProperties["axis"] as StructuralAxis) : null;
      set => StructuralProperties["axis"] = value;
    }

    /// <summary>A list of the X, Y, Z, Rx, Ry, and Rz restraints.</summary>
    [JsonIgnore]
    public StructuralVectorBoolSix Restraint
    {
      get => StructuralProperties.ContainsKey("restraint") ? (StructuralProperties["restraint"] as StructuralVectorBoolSix) : null;
      set => StructuralProperties["restraint"] = value;
    }

    /// <summary>A list of the X, Y, Z, Rx, Ry, and Rz stiffnesses.</summary>
    [JsonIgnore]
    public StructuralVectorSix Stiffness
    {
      get => StructuralProperties.ContainsKey("stiffness") ? (StructuralProperties["stiffness"] as StructuralVectorSix) : null;
      set => StructuralProperties["stiffness"] = value;
    }

    /// <summary>Mass of the node.</summary>
    [JsonIgnore]
    public double Mass
    {
      get => StructuralProperties.ContainsKey("mass") ? ((double)StructuralProperties["mass"]) : 0;
      set => StructuralProperties["mass"] = value;
    }

    /// <summary>Analysis results.</summary>
    [JsonIgnore]
    public Dictionary<string, object> Result
    {
      get => StructuralProperties.ContainsKey("result") ? (StructuralProperties["result"] as Dictionary<string, object>) : null;
      set => StructuralProperties["result"] = value;
    }

    /// <summary>GSA local mesh size around node.</summary>
    [JsonIgnore]
    public double GSALocalMeshSize
    {
      get => StructuralProperties.ContainsKey("gsaLocalMeshSize") ? ((double)StructuralProperties["gsaLocalMeshSize"]) : 0;
      set => StructuralProperties["gsaLocalMeshSize"] = value;
    }
  }

  [Serializable]
  public partial class Structural1DElement : SpeckleLine, IStructural
  {
    public override string Type { get => base.Type + "/Structural1DElement"; }

    [JsonIgnore]
    private Dictionary<string, object> StructuralProperties
    {
      get
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        if (!base.Properties.ContainsKey("structural"))
          base.Properties["structural"] = new Dictionary<string, object>();

        return base.Properties["structural"] as Dictionary<string, object>;

      }
      set
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        base.Properties["structural"] = value;
      }
    }

    /// <summary>Base SpeckleLine.</summary>
    [JsonIgnore]
    public SpeckleLine baseLine
    {
      get => this as SpeckleLine;
      set => this.Value = value.Value;
    }

    /// <summary>Type of 1D element.</summary>
    [JsonIgnore]
    public Structural1DElementType ElementType
    {
      get => StructuralProperties.ContainsKey("elementType") ? (Structural1DElementType)Enum.Parse(typeof(Structural1DElementType), (StructuralProperties["elementType"] as string), true) : Structural1DElementType.Generic;
      set => StructuralProperties["elementType"] = value.ToString();
    }

    /// <summary>Application ID of Structural1DProperty.</summary>
    [JsonIgnore]
    public string PropertyRef
    {
      get => StructuralProperties.ContainsKey("propertyRef") ? (StructuralProperties["propertyRef"] as string) : null;
      set => StructuralProperties["propertyRef"] = value;
    }

    /// <summary>Local axis of 1D element.</summary>
    [JsonIgnore]
    public StructuralVectorThree ZAxis
    {
      get => StructuralProperties.ContainsKey("zAxis") ? (StructuralProperties["zAxis"] as StructuralVectorThree) : null;
      set => StructuralProperties["zAxis"] = value;
    }

    /// <summary>List of X, Y, Z, Rx, Ry, and Rz releases on each end.</summary>
    [JsonIgnore]
    public List<StructuralVectorBoolSix> EndRelease
    {
      get
      {
        if (StructuralProperties.ContainsKey("endRelease"))
        {
          try
          {
            try
            {
              return (List<StructuralVectorBoolSix>)StructuralProperties["endRelease"];
            }
            catch
            {
              return ((List<object>)StructuralProperties["endRelease"]).Select(x => x as StructuralVectorBoolSix).ToList();
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["endRelease"] = value;
    }

    /// <summary>List of X, Y, and, Z offsets on each end.</summary>
    [JsonIgnore]
    public List<StructuralVectorThree> Offset
    {
      get
      {
        if (StructuralProperties.ContainsKey("offset"))
        {
          try
          {
            try
            {
              return (List<StructuralVectorThree>)StructuralProperties["offset"];
            }
            catch
            {
              return ((List<object>)StructuralProperties["offset"]).Select(x => x as StructuralVectorThree).ToList();
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["offset"] = value;
    }

    /// <summary>GSA target mesh size.</summary>
    [JsonIgnore]
    public double GSAMeshSize
    {
      get => StructuralProperties.ContainsKey("gsaMeshSize") ? ((double)StructuralProperties["gsaMeshSize"]) : 0;
      set => StructuralProperties["gsaMeshSize"] = value;
    }

    /// <summary>GSA dummy status.</summary>
    [JsonIgnore]
    public bool GSADummy
    {
      get => StructuralProperties.ContainsKey("gsaDummy") ? ((bool)StructuralProperties["gsaDummy"]) : false;
      set => StructuralProperties["gsaDummy"] = value;
    }

    /// <summary>Vertex location of results.</summary>
    [JsonIgnore]
    public List<double> ResultVertices
    {
      get
      {
        if (StructuralProperties.ContainsKey("resultVertices"))
        {
          try
          {
            try
            {
              return (List<double>)StructuralProperties["resultVertices"];
            }
            catch
            {
              this.ResultVertices = ((List<object>)StructuralProperties["resultVertices"]).Select(x => Convert.ToDouble(x)).ToList();
              return this.ResultVertices;
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["resultVertices"] = value;
    }

    /// <summary>Analysis results.</summary>
    [JsonIgnore]
    public Dictionary<string, object> Result
    {
      get => StructuralProperties.ContainsKey("result") ? (StructuralProperties["result"] as Dictionary<string, object>) : null;
      set => StructuralProperties["result"] = value;
    }
  }

  [Serializable]
  public partial class Structural1DSpring : SpeckleLine, IStructural
  {
    public override string Type { get => base.Type + "/Structural1DSpring"; }

    [JsonIgnore]
    private Dictionary<string, object> StructuralProperties
    {
      get
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        if (!base.Properties.ContainsKey("structural"))
          base.Properties["structural"] = new Dictionary<string, object>();

        return base.Properties["structural"] as Dictionary<string, object>;

      }
      set
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        base.Properties["structural"] = value;
      }
    }

    /// <summary>Base SpeckleLine.</summary>
    [JsonIgnore]
    public SpeckleLine baseLine
    {
      get => this as SpeckleLine;
      set => this.Value = value.Value;
    }
    
    /// <summary>Application ID of StructuralSpringProperty.</summary>
    [JsonIgnore]
    public string PropertyRef
    {
      get => StructuralProperties.ContainsKey("propertyRef") ? (StructuralProperties["propertyRef"] as string) : null;
      set => StructuralProperties["propertyRef"] = value;
    }

    /// <summary>Local axis of 1D spring.</summary>
    [JsonIgnore]
    public StructuralVectorThree ZAxis
    {
      get => StructuralProperties.ContainsKey("zAxis") ? (StructuralProperties["zAxis"] as StructuralVectorThree) : null;
      set => StructuralProperties["zAxis"] = value;
    }
    
    /// <summary>GSA dummy status.</summary>
    [JsonIgnore]
    public bool GSADummy
    {
      get => StructuralProperties.ContainsKey("gsaDummy") ? ((bool)StructuralProperties["gsaDummy"]) : false;
      set => StructuralProperties["gsaDummy"] = value;
    }

    /// <summary>Vertex location of results.</summary>
    [JsonIgnore]
    public List<double> ResultVertices
    {
      get
      {
        if (StructuralProperties.ContainsKey("resultVertices"))
        {
          try
          {
            try
            {
              return (List<double>)StructuralProperties["resultVertices"];
            }
            catch
            {
              this.ResultVertices = ((List<object>)StructuralProperties["resultVertices"]).Select(x => Convert.ToDouble(x)).ToList();
              return this.ResultVertices;
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["resultVertices"] = value;
    }

    /// <summary>Analysis results.</summary>
    [JsonIgnore]
    public Dictionary<string, object> Result
    {
      get => StructuralProperties.ContainsKey("result") ? (StructuralProperties["result"] as Dictionary<string, object>) : null;
      set => StructuralProperties["result"] = value;
    }
  }

  [Serializable]
  public partial class Structural1DElementPolyline : SpecklePolyline, IStructural
  {
    public override string Type { get => base.Type + "/Structural1DElementPolyline"; }

    [JsonIgnore]
    private Dictionary<string, object> StructuralProperties
    {
      get
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        if (!base.Properties.ContainsKey("structural"))
          base.Properties["structural"] = new Dictionary<string, object>();

        return base.Properties["structural"] as Dictionary<string, object>;

      }
      set
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        base.Properties["structural"] = value;
      }
    }

    /// <summary>Application ID of elements to reference from other objects.</summary>
    [JsonIgnore]
    public List<string> ElementApplicationId
    {
      get
      {
        if (StructuralProperties.ContainsKey("elementApplicationId"))
        {
          try
          {
            try
            {
              return (List<string>)StructuralProperties["elementApplicationId"];
            }
            catch
            {
              this.ElementApplicationId = ((List<object>)StructuralProperties["elementApplicationId"]).Select(x => Convert.ToString(x)).ToList();
              return this.ElementApplicationId;
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["elementApplicationId"] = value;
    }

    /// <summary>Base SpecklePolyline.</summary>
    [JsonIgnore]
    public SpecklePolyline basePolyline
    {
      get => this as SpecklePolyline;
      set
      {
        this.Value = value.Value;
        this.Closed = value.Closed;
        this.Domain = value.Domain;
      }
    }

    /// <summary>Type of 1D element.</summary>
    [JsonIgnore]
    public Structural1DElementType ElementType
    {
      get => StructuralProperties.ContainsKey("elementType") ? (Structural1DElementType)Enum.Parse(typeof(Structural1DElementType), (StructuralProperties["elementType"] as string), true) : Structural1DElementType.Generic;
      set => StructuralProperties["elementType"] = value.ToString();
    }

    /// <summary>Application ID of Structural1DProperty.</summary>
    [JsonIgnore]
    public string PropertyRef
    {
      get => StructuralProperties.ContainsKey("propertyRef") ? (StructuralProperties["propertyRef"] as string) : null;
      set => StructuralProperties["propertyRef"] = value;
    }

    /// <summary>Local Z axis of 1D elements.</summary>
    [JsonIgnore]
    public List<StructuralVectorThree> ZAxis
    {
      get
      {
        if (StructuralProperties.ContainsKey("zAxis"))
        {
          try
          {
            try
            {
              return (List<StructuralVectorThree>)StructuralProperties["zAxis"];
            }
            catch
            {
              this.ZAxis = ((List<object>)StructuralProperties["zAxis"]).Select(x => x as StructuralVectorThree).ToList();
              return this.ZAxis;
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["zAxis"] = value;
    }

    /// <summary>List of X, Y, Z, Rx, Ry, and Rz releases of each node.</summary>
    [JsonIgnore]
    public List<StructuralVectorBoolSix> EndRelease
    {
      get
      {
        if (StructuralProperties.ContainsKey("endRelease"))
        {
          try
          {
            try
            {
              return (List<StructuralVectorBoolSix>)StructuralProperties["endRelease"];
            }
            catch
            {
              this.EndRelease = ((List<object>)StructuralProperties["endRelease"]).Select(x => x as StructuralVectorBoolSix).ToList();
              return this.EndRelease;
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["endRelease"] = value;
    }

    /// <summary>List of X, Y, Z, Rx, Ry, and Rz offsets of each node.</summary>
    [JsonIgnore]
    public List<StructuralVectorThree> Offset
    {
      get
      {
        if (StructuralProperties.ContainsKey("offset"))
        {
          try
          {
            try
            {
              return (List<StructuralVectorThree>)StructuralProperties["offset"];
            }
            catch
            {
              this.Offset = ((List<object>)StructuralProperties["offset"]).Select(x => x as StructuralVectorThree).ToList();
              return this.Offset;
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["offset"] = value;
    }

    /// <summary>GSA target mesh size.</summary>
    [JsonIgnore]
    public double GSAMeshSize
    {
      get => StructuralProperties.ContainsKey("gsaMeshSize") ? ((double)StructuralProperties["gsaMeshSize"]) : 0;
      set => StructuralProperties["gsaMeshSize"] = value;
    }

    /// <summary>GSA dummy status.</summary>
    [JsonIgnore]
    public bool GSADummy
    {
      get => StructuralProperties.ContainsKey("gsaDummy") ? ((bool)StructuralProperties["gsaDummy"]) : false;
      set => StructuralProperties["gsaDummy"] = value;
    }

    /// <summary>Vertex location of results.</summary>
    [JsonIgnore]
    public List<double> ResultVertices
    {
      get
      {
        if (StructuralProperties.ContainsKey("resultVertices"))
        {
          try
          {
            try
            {
              return (List<double>)StructuralProperties["resultVertices"];
            }
            catch
            {
              this.ResultVertices = ((List<object>)StructuralProperties["resultVertices"]).Select(x => Convert.ToDouble(x)).ToList();
              return this.ResultVertices;
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["resultVertices"] = value;
    }

    /// <summary>Analysis results.</summary>
    [JsonIgnore]
    public Dictionary<string, object> Result
    {
      get => StructuralProperties.ContainsKey("result") ? (StructuralProperties["result"] as Dictionary<string, object>) : null;
      set => StructuralProperties["result"] = value;
    }
  }

  [Serializable]
  public partial class Structural2DElement : SpeckleMesh, IStructural
  {
    public override string Type { get => base.Type + "/Structural2DElement"; }

    [JsonIgnore]
    private Dictionary<string, object> StructuralProperties
    {
      get
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        if (!base.Properties.ContainsKey("structural"))
          base.Properties["structural"] = new Dictionary<string, object>();

        return base.Properties["structural"] as Dictionary<string, object>;

      }
      set
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        base.Properties["structural"] = value;
      }
    }

    /// <summary>Base SpeckleMesh.</summary>
    [JsonIgnore]
    public SpeckleMesh baseMesh
    {
      get => this as SpeckleMesh;
      set
      {
        this.Vertices = value.Vertices;
        this.Faces = value.Faces;
        this.Colors = value.Colors;
        this.TextureCoordinates = value.TextureCoordinates;
      }
    }

    /// <summary>Type of 2D element.</summary>
    [JsonIgnore]
    public Structural2DElementType ElementType
    {
      get => StructuralProperties.ContainsKey("elementType") ? (Structural2DElementType)Enum.Parse(typeof(Structural2DElementType), (StructuralProperties["elementType"] as string), true) : Structural2DElementType.Generic;
      set => StructuralProperties["elementType"] = value.ToString();
    }

    /// <summary>Application ID of Structural2DProperty.</summary>
    [JsonIgnore]
    public string PropertyRef
    {
      get => StructuralProperties.ContainsKey("propertyRef") ? (StructuralProperties["propertyRef"] as string) : null;
      set => StructuralProperties["propertyRef"] = value;
    }

    /// <summary>Local axis of 2D element.</summary>
    [JsonIgnore]
    public StructuralAxis Axis
    {
      get => StructuralProperties.ContainsKey("axis") ? (StructuralProperties["axis"] as StructuralAxis) : null;
      set => StructuralProperties["axis"] = value;
    }

    /// <summary>Offset of 2D element.</summary>
    [JsonIgnore]
    public double Offset
    {
      get => StructuralProperties.ContainsKey("offset") ? ((double)StructuralProperties["offset"]) : 0;
      set => StructuralProperties["offset"] = value;
    }

    /// <summary>GSA target mesh size.</summary>
    [JsonIgnore]
    public double GSAMeshSize
    {
      get => StructuralProperties.ContainsKey("gsaMeshSize") ? ((double)StructuralProperties["gsaMeshSize"]) : 0;
      set => StructuralProperties["gsaMeshSize"] = value;
    }

    /// <summary>GSA dummy status.</summary>
    [JsonIgnore]
    public bool GSADummy
    {
      get => StructuralProperties.ContainsKey("gsaDummy") ? ((bool)StructuralProperties["gsaDummy"]) : false;
      set => StructuralProperties["gsaDummy"] = value;
    }

    /// <summary>Analysis results.</summary>
    [JsonIgnore]
    public Dictionary<string, object> Result
    {
      get => StructuralProperties.ContainsKey("result") ? (StructuralProperties["result"] as Dictionary<string, object>) : null;
      set => StructuralProperties["result"] = value;
    }
  }

  [Serializable]
  public partial class Structural2DElementMesh : SpeckleMesh, IStructural
  {
    public override string Type { get => base.Type + "/Structural2DElementMesh"; }

    [JsonIgnore]
    private Dictionary<string, object> StructuralProperties
    {
      get
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        if (!base.Properties.ContainsKey("structural"))
          base.Properties["structural"] = new Dictionary<string, object>();

        return base.Properties["structural"] as Dictionary<string, object>;

      }
      set
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        base.Properties["structural"] = value;
      }
    }

    /// <summary>Application ID of elements to reference from other objects.</summary>
    [JsonIgnore]
    public List<string> ElementApplicationId
    {
      get
      {
        if (StructuralProperties.ContainsKey("elementApplicationId"))
        {
          try
          {
            try
            {
              return (List<string>)StructuralProperties["elementApplicationId"];
            }
            catch
            {
              this.ElementApplicationId = ((List<object>)StructuralProperties["elementApplicationId"]).Select(x => Convert.ToString(x)).ToList();
              return this.ElementApplicationId;
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["elementApplicationId"] = value;
    }

    /// <summary>Base SpeckleMesh.</summary>
    [JsonIgnore]
    public SpeckleMesh baseMesh
    {
      get => this as SpeckleMesh;
      set
      {
        this.Vertices = value.Vertices;
        this.Faces = value.Faces;
        this.Colors = value.Colors;
        this.TextureCoordinates = value.TextureCoordinates;
      }
    }

    /// <summary>Type of 2D element.</summary>
    [JsonIgnore]
    public Structural2DElementType ElementType
    {
      get => StructuralProperties.ContainsKey("elementType") ? (Structural2DElementType)Enum.Parse(typeof(Structural2DElementType), (StructuralProperties["elementType"] as string), true) : Structural2DElementType.Generic;
      set => StructuralProperties["elementType"] = value.ToString();
    }

    /// <summary>Application ID of Structural2DProperty.</summary>
    [JsonIgnore]
    public string PropertyRef
    {
      get => StructuralProperties.ContainsKey("propertyRef") ? (StructuralProperties["propertyRef"] as string) : null;
      set => StructuralProperties["propertyRef"] = value;
    }

    /// <summary>Local axis of each 2D element.</summary>
    [JsonIgnore]
    public List<StructuralAxis> Axis
    {
      get
      {
        if (StructuralProperties.ContainsKey("axis"))
        {
          try
          {
            try
            {
              return (List<StructuralAxis>)StructuralProperties["axis"];
            }
            catch
            {
              return ((List<object>)StructuralProperties["axis"]).Select(x => x as StructuralAxis).ToList();
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["axis"] = value;
    }

    /// <summary>Offset of easch 2D element.</summary>
    [JsonIgnore]
    public List<double> Offset
    {
      get
      {
        if (StructuralProperties.ContainsKey("offset"))
        {
          try
          {
            try
            {
              return (List<double>)StructuralProperties["offset"];
            }
            catch
            {
              this.Offset = ((List<object>)StructuralProperties["offset"]).Select(x => Convert.ToDouble(x)).ToList();
              return this.Offset;
            }
          }
          catch
          { return null; }
        }
        else
          return null;
      }
      set => StructuralProperties["offset"] = value;
    }

    /// <summary>GSA target mesh size.</summary>
    [JsonIgnore]
    public double GSAMeshSize
    {
      get => StructuralProperties.ContainsKey("gsaMeshSize") ? ((double)StructuralProperties["gsaMeshSize"]) : 0;
      set => StructuralProperties["gsaMeshSize"] = value;
    }

    /// <summary>GSA dummy status.</summary>
    [JsonIgnore]
    public bool GSADummy
    {
      get => StructuralProperties.ContainsKey("gsaDummy") ? ((bool)StructuralProperties["gsaDummy"]) : false;
      set => StructuralProperties["gsaDummy"] = value;
    }

    /// <summary>Analysis results.</summary>
    [JsonIgnore]
    public Dictionary<string, object> Result
    {
      get => StructuralProperties.ContainsKey("result") ? (StructuralProperties["result"] as Dictionary<string, object>) : null;
      set => StructuralProperties["result"] = value;
    }
  }

  [Serializable]
  public partial class Structural2DVoid : SpeckleMesh, IStructural
  {
    public override string Type { get => base.Type + "/Structural2DVoid"; }

    [JsonIgnore]
    private Dictionary<string, object> StructuralProperties
    {
      get
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        if (!base.Properties.ContainsKey("structural"))
          base.Properties["structural"] = new Dictionary<string, object>();

        return base.Properties["structural"] as Dictionary<string, object>;

      }
      set
      {
        if (base.Properties == null)
          base.Properties = new Dictionary<string, object>();

        base.Properties["structural"] = value;
      }
    }

    /// <summary>Base SpeckleMesh.</summary>
    [JsonIgnore]
    public SpeckleMesh baseMesh
    {
      get => this as SpeckleMesh;
      set
      {
        this.Vertices = value.Vertices;
        this.Faces = value.Faces;
        this.Colors = value.Colors;
        this.TextureCoordinates = value.TextureCoordinates;
      }
    }
  }
}