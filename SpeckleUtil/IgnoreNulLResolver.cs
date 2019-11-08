﻿using System;
using AutoMapper;

namespace SpeckleUtil
{
  internal class IgnoreNullResolver : IMemberValueResolver<object, object, object, object>
  {
    public object Resolve(object source, object destination, object sourceMember, object destinationMember, ResolutionContext context)
    {
      if (sourceMember is Enum && sourceMember.Equals(GetDefaultValue(sourceMember.GetType())))
      {
        return destinationMember;
      }
      return sourceMember ?? destinationMember;
    }

    private object GetDefaultValue(Type t)
    {
      if (t.IsValueType)
      {
        return Activator.CreateInstance(t);
      }
      else
      {
        return null;
      }
    }
  }
}
