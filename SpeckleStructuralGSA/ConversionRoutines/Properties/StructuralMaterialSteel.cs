﻿using System;
using System.Collections.Generic;
using System.Linq;
using SpeckleCore;
using SpeckleGSAInterfaces;
using SpeckleStructuralClasses;

namespace SpeckleStructuralGSA
{
  [GSAObject("MAT_STEEL.3", new string[] { }, "properties", true, true, new Type[] { }, new Type[] { })]
  public class GSAMaterialSteel : IGSASpeckleContainer
  {
    public int GSAId { get; set; }
    public string GWACommand { get; set; }
    public List<string> SubGWACommand { get; set; } = new List<string>();
    public dynamic Value { get; set; } = new StructuralMaterialSteel();

    public void ParseGWACommand()
    {
      if (this.GWACommand == null)
        return;

      var obj = new StructuralMaterialSteel();

      var pieces = this.GWACommand.ListSplit("\t");

      var counter = 1; // Skip identifier
      this.GSAId = Convert.ToInt32(pieces[counter++]);
      obj.ApplicationId = Initialiser.Indexer.GetApplicationId(this.GetGSAKeyword(), this.GSAId);
      counter++; // MAT.8
      obj.Name = pieces[counter++].Trim(new char[] { '"' });
      counter++; // Unlocked
      obj.YoungsModulus = Convert.ToDouble(pieces[counter++]);
      obj.PoissonsRatio = Convert.ToDouble(pieces[counter++]);
      obj.ShearModulus = Convert.ToDouble(pieces[counter++]);
      obj.Density = Convert.ToDouble(pieces[counter++]);
      obj.CoeffThermalExpansion = Convert.ToDouble(pieces[counter++]);

      // Failure strain is found before MAT_CURVE_PARAM.2
      var strainIndex = Array.FindIndex(pieces, x => x.StartsWith("MAT_CURVE_PARAM"));
      if (strainIndex > 0)
        obj.MaxStrain = Convert.ToDouble(pieces[strainIndex - 1]);

      // Skip to last fourth to last
      counter = pieces.Count() - 4;
      obj.YieldStrength = Convert.ToDouble(pieces[counter++]);
      obj.UltimateStrength = Convert.ToDouble(pieces[counter++]);

      this.Value = obj;
    }

    public string SetGWACommand()
    {
      if (this.Value == null)
        return "";

      var mat = this.Value as StructuralMaterialSteel;

      var keyword = typeof(GSAMaterialSteel).GetGSAKeyword();

      var index = Initialiser.Indexer.ResolveIndex(typeof(GSAMaterialSteel).GetGSAKeyword(), typeof(GSAMaterialSteel).Name, mat.ApplicationId);

      // TODO: This function barely works.
      var ls = new List<string>
      {
        "SET",
        "MAT_STEEL.3" + ":" + HelperClass.GenerateSID(mat),
        index.ToString(),
        "MAT.8",
        mat.Name == null || mat.Name == "" ? " " : mat.Name,
        "YES", // Unlocked
        mat.YoungsModulus.ToString(), // E
        mat.PoissonsRatio.ToString(), // nu
        mat.ShearModulus.ToString(), // G
        mat.Density.ToString(), // rho
        mat.CoeffThermalExpansion.ToString(), // alpha
        "MAT_ANAL.1",
        "0", // TODO: What is this?
        "Steel",
        "-268435456", // TODO: What is this?
        "MAT_ELAS_ISO",
        "6", // TODO: What is this?
        mat.YoungsModulus.ToString(), // E
        mat.PoissonsRatio.ToString(), // nu
        mat.Density.ToString(), // rho
        mat.CoeffThermalExpansion.ToString(), // alpha
        mat.ShearModulus.ToString(), // G
        "0", // TODO: What is this?
        "0", // TODO: What is this?
        "0", // TODO: What is this?
        "0", // TODO: What is this?
        "0", // TODO: What is this?
        "0", // TODO: What is this?
        "0", // TODO: What is this?
        mat.MaxStrain.ToString(), // Ultimate strain
        "MAT_CURVE_PARAM.2",
        "",
        "UNDEF",
        "1", // Material factor on strength
        "1", // Material factor on elastic modulus
        "MAT_CURVE_PARAM.2",
        "",
        "UNDEF",
        "1", // Material factor on strength
        "1", // Material factor on elastic modulus
        "0", // Cost
        mat.YieldStrength.ToString(), // Yield strength
        mat.UltimateStrength.ToString(), // Ultimate strength
        "0", // Perfectly plastic strain limit
        "0" // Hardening modulus
      };

      return (string.Join("\t", ls));
    }
  }

  public static partial class Conversions
  {
    public static string ToNative(this StructuralMaterialSteel mat)
    {
      return new GSAMaterialSteel() { Value = mat }.SetGWACommand();
    }

    public static SpeckleObject ToSpeckle(this GSAMaterialSteel dummyObject)
    {
      var newLines = ToSpeckleBase<GSAMaterialSteel>();

      var materials = new List<GSAMaterialSteel>();

      foreach (var p in newLines)
      {
        try
        {
          var mat = new GSAMaterialSteel() { GWACommand = p };
          mat.ParseGWACommand();
          materials.Add(mat);
        }
        catch { }
      }

      Initialiser.GSASenderObjects[typeof(GSAMaterialSteel)].AddRange(materials);

      return (materials.Count() > 0) ? new SpeckleObject() : new SpeckleNull();
    }
  }
}
