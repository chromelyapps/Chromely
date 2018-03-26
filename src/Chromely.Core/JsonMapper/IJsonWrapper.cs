// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IJsonWrapper.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// The authors disclaim copyright to this source code. For more details, see
// the COPYING file included with this distribution @ https://github.com/lbv/litjson
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
//
// This is a port of LitJson to.NET Standard for Chromely.Mostly provided as-is. 
// For more info: https://github.com/lbv/litjson
// </note>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable All
namespace LitJson
{
    using System.Collections;
    using System.Collections.Specialized;

    /// <summary>
    /// The json type.
    /// </summary>
    public enum JsonType
    {
        None,
        Object,
        Array,
        String,
        Int,
        Long,
        Double,
        Boolean
    }

    /// <summary>
    /// The JsonWrapper interface.
    ///  Interface that represents a type capable of handling all kinds of JSON
    ///   data.This is mainly used when mapping objects through JsonMapper, and
    ///   it's implemented by JsonData
    /// </summary>
    public interface IJsonWrapper : IList, IOrderedDictionary
    {
        bool IsArray { get; }
        bool IsBoolean { get; }
        bool IsDouble { get; }
        bool IsInt { get; }
        bool IsLong { get; }
        bool IsObject { get; }
        bool IsString { get; }

        bool GetBoolean();
        double GetDouble();
        int GetInt();
        JsonType GetJsonType();
        long GetLong();
        string GetString();

        void SetBoolean(bool val);
        void SetDouble(double val);
        void SetInt(int val);
        void SetJsonType(JsonType type);
        void SetLong(long val);
        void SetString(string val);

        string ToJson();
        void ToJson(JsonWriter writer);
    }
}