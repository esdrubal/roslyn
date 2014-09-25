﻿' Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Runtime.CompilerServices

Namespace Microsoft.CodeAnalysis.VisualBasic
    Friend Module SyntaxKindExtensions

        ''' <summary>
        ''' Determine if the given <see cref="SyntaxKind"/> array contains the given kind.
        ''' </summary>
        ''' <param name="kinds">Array to search</param>
        ''' <param name="kind">Sought value</param>
        ''' <returns>True if <paramref name="kinds"/> contains the value <paramref name="kind"/>.</returns>
        ''' <remarks>PERF: Not using Array.IndexOf here because it results in a call to IndexOf on the default EqualityComparer for SyntaxKind. The default comparer for SyntaxKind is
        ''' the ObjectEqualityComparer which results in boxing allocations.</remarks>
        <Extension()>
        Public Function Contains(kinds As SyntaxKind(), kind As SyntaxKind) As Boolean
            For Each k In kinds
                If k = kind Then Return True
            Next
            Return False
        End Function

    End Module
End Namespace

