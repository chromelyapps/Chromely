// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.NativeHosts;

public class GListUtil
{
    private readonly IntPtr _list;
    public GListUtil(IntPtr list)
    {
        _list = list;
    }

    public int Length
    {
        get
        {
            return g_list_length(_list);
        }
    }

    public void Free()
    {
        if (_list != IntPtr.Zero)
            g_list_free(_list);
    }

    public IntPtr GetItem(int nth)
    {
        if (_list != IntPtr.Zero)
            return g_list_nth_data(_list, (uint)nth);

        return IntPtr.Zero;
    }

    #region DLLIMPORTS GlibLib

    [DllImport(Library.GlibLib, CallingConvention = CallingConvention.Cdecl)]
    static extern int g_list_length(IntPtr l);

    [DllImport(Library.GlibLib, CallingConvention = CallingConvention.Cdecl)]
    static extern void g_list_free(IntPtr l);

    [DllImport(Library.GlibLib, CallingConvention = CallingConvention.Cdecl)]
    static extern IntPtr g_list_nth_data(IntPtr l, uint n);

    #endregion
}