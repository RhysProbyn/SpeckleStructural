﻿using System;
using System.Collections.Generic;
using System.Linq;
using SpeckleCore;
using SpeckleGSAInterfaces;
using SpeckleStructuralClasses;

namespace SpeckleStructuralGSA
{
  [GSAObject("USER_VEHICLE.1", new string[] { }, "misc", true, true, new Type[] { }, new Type[] { })]
  public class GSABridgeVehicle : IGSASpeckleContainer
  {
    public int GSAId { get; set; }
    public string GWACommand { get; set; }
    public List<string> SubGWACommand { get; set; } = new List<string>();
    public dynamic Value { get; set; } = new StructuralBridgeVehicle();

    public void ParseGWACommand()
    {
      if (this.GWACommand == null)
        return;

      var obj = new StructuralBridgeVehicle();

      var pieces = this.GWACommand.ListSplit("\t");

      var counter = 1; // Skip identifier

      this.GSAId = Convert.ToInt32(pieces[counter++]);
      obj.ApplicationId = HelperClass.GetApplicationId(this.GetGSAKeyword(), this.GSAId);
      obj.Name = pieces[counter++].Trim(new char[] { '"' });

      //TO DO : replace these defaults with the real thing
      obj.Width = 0;

      this.Value = obj;
    }

    public string SetGWACommand()
    {
      if (this.Value == null)
        return "";

      var destType = typeof(GSABridgeVehicle);

      var vehicle = this.Value as StructuralBridgeVehicle;

      var keyword = destType.GetGSAKeyword();

      var index = Initialiser.Cache.ResolveIndex(keyword, vehicle.ApplicationId);

      //The width parameter is intentionally not being used here as the meaning doesn't map to the y coordinate parameter of the ASSEMBLY keyword
      //It is therefore to be ignored here for GSA purposes.

      var ls = new List<string>
        {
          "SET",
          keyword + ":" + HelperClass.GenerateSID(vehicle),
          index.ToString(),
          string.IsNullOrEmpty(vehicle.Name) ? "" : vehicle.Name,
          vehicle.Width.ToString(),
          vehicle.Axles.Count().ToString()
      };

      foreach (var axle in vehicle.Axles)
      {
        ls.AddRange(new[] {
          axle.Position.ToString(),
          axle.WheelOffset.ToString(),
          axle.LeftWheelLoad.ToString(),
          axle.RightWheelLoad.ToString() });
      }

      return (string.Join("\t", ls));
    }
  }

  public static partial class Conversions
  {
    public static string ToNative(this StructuralBridgeVehicle assembly)
    {
      return new GSABridgeVehicle() { Value = assembly }.SetGWACommand();
    }

    public static SpeckleObject ToSpeckle(this GSABridgeVehicle dummyObject)
    {
      var newLines = ToSpeckleBase<GSABridgeVehicle>();

      //Get all relevant GSA entities in this entire model
      var alignments = new List<GSABridgeVehicle>();

      foreach (var p in newLines.Values)
      {
        var alignment = new GSABridgeVehicle() { GWACommand = p };
        //Pass in ALL the nodes and members - the Parse_ method will search through them
        alignment.ParseGWACommand();
        alignments.Add(alignment);
      }

      Initialiser.GSASenderObjects[typeof(GSABridgeVehicle)].AddRange(alignments);

      return (alignments.Count() > 0) ? new SpeckleObject() : new SpeckleNull();
    }
  }
}
