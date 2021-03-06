﻿using System;
using System.Collections.Generic;
using System.Linq;
using SpeckleCore;
using SpeckleGSAInterfaces;
using SpeckleStructuralClasses;

namespace SpeckleStructuralGSA
{
  [GSAObject("LOAD_2D_THERMAL.2", new string[] { }, "loads", true, true, new Type[] { typeof(GSA2DElement), typeof(GSA2DMember), typeof(GSALoadCase) }, new Type[] { typeof(GSA2DElement), typeof(GSA2DMember), typeof(GSALoadCase) })]
  public class GSA2DThermalLoading : IGSASpeckleContainer
  {
    public int GSAId { get; set; }
    public string GWACommand { get; set; }
    public List<string> SubGWACommand { get; set; } = new List<string>();
    public dynamic Value { get; set; } = new Structural2DThermalLoad();

    public void ParseGWACommand(List<GSA2DElement> e2Ds, List<GSA2DMember> m2Ds)
    {
      if (this.GWACommand == null)
        return;

      var obj = new Structural2DThermalLoad();

      var pieces = this.GWACommand.ListSplit("\t");

      var counter = 1; // Skip identifier
      
      obj.Name = pieces[counter++];
      obj.ApplicationId = HelperClass.GetApplicationId(this.GetGSAKeyword(), this.GSAId);

      var elementList = pieces[counter++];

      obj.ElementRefs = new List<string>();

      if (Initialiser.Settings.TargetLayer == GSATargetLayer.Analysis)
      {
        var elementId = Initialiser.Interface.ConvertGSAList(elementList, SpeckleGSAInterfaces.GSAEntity.ELEMENT);
        foreach (var id in elementId)
        {
          IGSASpeckleContainer elem = e2Ds.Where(e => e.GSAId == id).FirstOrDefault();

          if (elem == null)
            continue;

          obj.ElementRefs.Add((elem.Value as SpeckleObject).ApplicationId);
          this.SubGWACommand.Add(elem.GWACommand);
        }
      }
      else
      {
        var groupIds = HelperClass.GetGroupsFromGSAList(elementList).ToList();
        foreach (var id in groupIds)
        {
          var memb2Ds = m2Ds.Where(m => m.Group == id);

          obj.ElementRefs.AddRange(memb2Ds.Select(m => (string)m.Value.ApplicationId));
          this.SubGWACommand.AddRange(memb2Ds.Select(m => m.GWACommand));
        }
      }

      obj.LoadCaseRef = HelperClass.GetApplicationId(typeof(GSALoadCase).GetGSAKeyword(), Convert.ToInt32(pieces[counter++]));

      var loadingType = pieces[counter++];

      switch (loadingType)
      {
        case "CONS":
          obj.TopTemperature = Convert.ToDouble(pieces[counter++]);
          obj.BottomTemperature = obj.TopTemperature;
          break;
        case "DZ":
          obj.TopTemperature = Convert.ToDouble(pieces[counter++]);
          obj.BottomTemperature = Convert.ToDouble(pieces[counter++]);
          break;
        case "GEN":
          // GENERALIZE THIS TO AN AVERAGE
          for (var i = 0; i < 3; i++)
          { 
            obj.TopTemperature += Convert.ToDouble(pieces[counter++]);
            obj.BottomTemperature += Convert.ToDouble(pieces[counter++]);
          }
          obj.TopTemperature /= 4;
          obj.BottomTemperature /= 4;
          break;
      }

      this.Value = obj;
    }

    public string SetGWACommand()
    {
      if (this.Value == null)
        return "";

      var loading = this.Value as Structural2DThermalLoad;

      var keyword = typeof(GSA2DThermalLoading).GetGSAKeyword();

      var index = Initialiser.Cache.ResolveIndex(typeof(GSA2DThermalLoading).GetGSAKeyword(), loading.ApplicationId);

      var targetString = " ";

      if (loading.ElementRefs != null && loading.ElementRefs.Count() > 0)
      {
        if (Initialiser.Settings.TargetLayer == GSATargetLayer.Analysis)
        {
          var e2DIndices = Initialiser.Cache.LookupIndices(typeof(GSA2DElement).GetGSAKeyword(), loading.ElementRefs).Where(x => x.HasValue).Select(x => x.Value).ToList();
          var e2DMeshIndices = Initialiser.Cache.LookupIndices(typeof(GSA2DElementMesh).GetGSAKeyword(), loading.ElementRefs).Where(x => x.HasValue).Select(x => x.Value).ToList();
          targetString = string.Join(" ",
            e2DIndices.Select(x => x.ToString())
            .Concat(e2DMeshIndices.Select(x => "G" + x.ToString()))
          );
        }
        else if (Initialiser.Settings.TargetLayer == GSATargetLayer.Design)
        {
          var m2DIndices = Initialiser.Cache.LookupIndices(typeof(GSA2DMember).GetGSAKeyword(), loading.ElementRefs).Where(x => x.HasValue).Select(x => x.Value).ToList();
          targetString = string.Join(" ",
            m2DIndices.Select(x => "G" + x.ToString()));
        }
      }

      var loadCaseRef = Initialiser.Cache.LookupIndex(typeof(GSALoadCase).GetGSAKeyword(), loading.LoadCaseRef);

      var loadingName = string.IsNullOrEmpty(loading.Name) ? " " : loading.Name;

      var ls = new List<string>
        {
          "SET_AT",
          index.ToString(),
          keyword + ":" + HelperClass.GenerateSID(loading),
          loadingName, // Name
          targetString, //Elements
					(loadCaseRef.HasValue) ? loadCaseRef.Value.ToString() : "1",
        };

      if (loading.TopTemperature == loading.BottomTemperature)
      {
        ls.Add("CONS");
        ls.Add(loading.TopTemperature.ToString());
      }
      else
      {
        ls.Add("DZ");
        ls.Add(loading.TopTemperature.ToString());
        ls.Add(loading.BottomTemperature.ToString());
      }

      return (string.Join("\t", ls));
    }
  }

  public static partial class Conversions
  {
    public static string ToNative(this Structural2DThermalLoad load)
    {
      return new GSA2DThermalLoading() { Value = load }.SetGWACommand();
    }

    public static SpeckleObject ToSpeckle(this GSA2DThermalLoading dummyObject)
    {
      var newLines = ToSpeckleBase<GSA2DThermalLoading>();

      var loads = new List<GSA2DThermalLoading>();
      var e2Ds = new List<GSA2DElement>();
      var m2Ds = new List<GSA2DMember>();

      if (Initialiser.Settings.TargetLayer == GSATargetLayer.Analysis)
      {
        e2Ds = Initialiser.GSASenderObjects[typeof(GSA2DElement)].Cast<GSA2DElement>().ToList();
      }
      else if (Initialiser.Settings.TargetLayer == GSATargetLayer.Design)
      {
        m2Ds = Initialiser.GSASenderObjects[typeof(GSA2DMember)].Cast<GSA2DMember>().ToList();
      }

      foreach (var k in newLines.Keys)
      {
        var load = new GSA2DThermalLoading() { GSAId = k, GWACommand = newLines[k] };
        load.ParseGWACommand(e2Ds, m2Ds);
        loads.Add(load);
      }

      Initialiser.GSASenderObjects[typeof(GSA2DThermalLoading)].AddRange(loads);

      return (loads.Count() > 0 ) ? new SpeckleObject() : new SpeckleNull();
    }
  }
}
