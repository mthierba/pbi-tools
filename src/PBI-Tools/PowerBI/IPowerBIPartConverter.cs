﻿// Copyright (c) Mathias Thierbach
// Licensed under the MIT License. See LICENSE in the project root for license information.

#if NETFRAMEWORK
using Microsoft.PowerBI.Packaging;

namespace PbiTools.PowerBI
{
    /// <summary>
    /// Converts back-and-forth between a Power BI <see cref="IStreamablePowerBIPackagePartContent"/>
    /// and a specific representation of the underlying content as <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">A type that represents a Power BI package part content without any dependency on the Power BI API.</typeparam>
    // ReSharper disable once InconsistentNaming
    public interface IPowerBIPartConverter<T>
    {
        T FromPackagePart(IStreamablePowerBIPackagePartContent part);
        IStreamablePowerBIPackagePartContent ToPackagePart(T content);
    }
}
#endif