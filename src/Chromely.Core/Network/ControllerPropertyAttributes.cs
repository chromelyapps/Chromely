// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerPropertyAttribute.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.Core.Network
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpAttribute : Attribute
    {
        public virtual Method Method { get; }
        public string Name { get; set; }
        public string Route { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpGetAttribute : HttpAttribute
    {
        public override Method Method => Method.GET;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPostAttribute : HttpGetAttribute
    {
        public override Method Method => Method.POST;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPutAttribute : HttpGetAttribute
    {
        public override Method Method => Method.PUT;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpDeleteAttribute : HttpGetAttribute
    {
        public override Method Method => Method.DELETE;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpHeadAttribute : HttpGetAttribute
    {
        public override Method Method => Method.HEAD;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpOptionsAttribute : HttpGetAttribute
    {
        public override Method Method => Method.OPTIONS;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPatchAttribute : HttpGetAttribute
    {
        public override Method Method => Method.PATCH;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpMergeAttribute : HttpGetAttribute
    {
        public override Method Method => Method.MERGE;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }
        public string Route { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerPropertyAttribute : Attribute
    {
        public string Name { get; set; }
        public string Route { get; set; }
    }
}