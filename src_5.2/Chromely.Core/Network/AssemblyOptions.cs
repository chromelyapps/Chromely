// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public class AssemblyOptions
{
    /// <summary>
    /// Gets or sets the target assembly.
    /// </summary>
    public Assembly? TargetAssembly { get; set; }

    /// <summary>
    /// Gets or sets the default namespace.
    /// </summary>
    public string? DefaultNamespace { get; set; }

    /// <summary>
    /// Gets or sets the root folder.
    /// </summary>
    public string? RootFolder { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="AssemblyOptions"/>.
    /// </summary>
    /// <param name="targetAssemblyName">Target assembly full name.</param>
    /// <param name="defaultNamespace">The default name space if there is any.</param>
    /// <param name="rootFolder">The root folder if there is any.</param>
    public AssemblyOptions(string targetAssemblyName, string? defaultNamespace = null, string? rootFolder = null)
    {
        TargetAssembly = AssemblyOptions.LoadAssembly(targetAssemblyName);
        DefaultNamespace = defaultNamespace;
        RootFolder = rootFolder;

        if (TargetAssembly is not null && string.IsNullOrWhiteSpace(DefaultNamespace))
        {
            DefaultNamespace = TargetAssembly.GetName().Name;
        }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="AssemblyOptions"/>.
    /// </summary>
    /// <param name="target">The target assembly.</param>
    /// <param name="defaultNamespace">The default name space if there is any.</param>
    /// <param name="rootFolder">The root folder if there is any.</param>
    public AssemblyOptions(Assembly target, string? defaultNamespace = null, string? rootFolder = null)
    {
        TargetAssembly = target;
        DefaultNamespace = defaultNamespace;
        RootFolder = rootFolder;

        if (TargetAssembly is not null && string.IsNullOrWhiteSpace(DefaultNamespace))
        {
            DefaultNamespace = TargetAssembly.GetName().Name;
        }
    }

    private static Assembly? LoadAssembly(string targetAssemblyName)
    {
        try
        {
            return Assembly.LoadFrom(targetAssemblyName);
        }
        catch { }

        return default;
    }
}