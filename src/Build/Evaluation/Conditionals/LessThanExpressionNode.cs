﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

#nullable disable

namespace Microsoft.Build.Evaluation
{
    /// <summary>
    /// Compares for left &lt; right
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal sealed class LessThanExpressionNode : NumericComparisonExpressionNode
    {
        /// <summary>
        /// Compare numerically
        /// </summary>
        /// <param name="left">left input</param>
        /// <param name="right">right input</param>
        /// <returns>true if right is higher than left, false otherwise</returns>
        protected override bool Compare(double left, double right)
        {
            return left < right;
        }

        /// <summary>
        /// Compare Versions. This is only intended to compare version formats like "A.B.C.D" which can otherwise not be compared numerically
        /// </summary>
        /// <param name="left">left version</param>
        /// <param name="right">right version</param>
        /// <returns>true if right version is higher than left, false otherwise</returns>
        protected override bool Compare(Version left, Version right)
        {
            return left < right;
        }

        /// <summary>
        /// Compare mixed numbers and Versions
        /// </summary>
        /// <param name="left">left version</param>
        /// <param name="right">right number</param>
        /// <returns>true if right is higher than major version of left, false otherwise</returns>
        protected override bool Compare(Version left, double right)
        {
            if (left.Major != right)
            {
                return left.Major < right;
            }

            // If they have same "major" number, then that means we are comparing something like "6.X.Y.Z" to "6". Version treats the objects with more dots as
            // "larger" regardless of what those dots are (e.g. 6.0.0.0 > 6 is a true statement)
            return false;
        }

        /// <summary>
        /// Compare mixed numbers and Versions
        /// </summary>
        /// <param name="left">left number</param>
        /// <param name="right">right version</param>
        /// <returns>true if the major version of right is higher than left, false otherwise</returns>
        protected override bool Compare(double left, Version right)
        {
            if (right.Major != left)
            {
                return left < right.Major;
            }

            // If they have same "major" number, then that means we are comparing something like "6.X.Y.Z" to "6". Version treats the objects with more dots as
            // "larger" regardless of what those dots are (e.g. 6.0.0.0 > 6 is a true statement)
            return true;
        }

        internal override string DebuggerDisplay => $"(< {LeftChild.DebuggerDisplay} {RightChild.DebuggerDisplay})";
    }
}
