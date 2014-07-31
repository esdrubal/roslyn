﻿' Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Collections.Immutable
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Symbols
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.CodeAnalysis
    ''' <summary>
    ''' Addition Visual Basic syntax extension methods.
    ''' </summary>
    Public Module VisualBasicExtensions
        ''' <summary>
        ''' Returns SyntaxKind for SyntaxTrivia nodes.
        ''' </summary> 
        <Extension>
        Public Function VisualBasicKind(trivia As SyntaxTrivia) As SyntaxKind
            Return If(trivia.Language Is LanguageNames.VisualBasic, CType(trivia.RawKind, SyntaxKind), SyntaxKind.None)
        End Function

        ''' <summary>
        ''' Returns SyntaxKind for SyntaxToken from RawKind property.
        ''' </summary>       
        <Extension>
        Public Function VisualBasicKind(token As SyntaxToken) As SyntaxKind
            Return If(token.Language Is LanguageNames.VisualBasic, CType(token.RawKind, SyntaxKind), SyntaxKind.None)
        End Function

        ''' <summary>
        ''' Returns SyntaxKind for SyntaxToken from RawContextualKind.
        ''' </summary>
        <Extension>
        Public Function VisualBasicContextualKind(token As SyntaxToken) As SyntaxKind
            Return If(token.Language Is LanguageNames.VisualBasic, CType(token.RawContextualKind, SyntaxKind), SyntaxKind.None)
        End Function

        ''' <summary>
        ''' Returns SyntaxKind for SyntaxNode from RawKind property.
        ''' </summary>
        <Extension>
        Public Function VisualBasicKind(node As SyntaxNode) As SyntaxKind
            Return If(node.Language Is LanguageNames.VisualBasic, CType(node.RawKind, SyntaxKind), SyntaxKind.None)
        End Function

        ''' <summary>
        ''' Returns SyntaxKind for SyntaxNodeOrToken from RawKind property.
        ''' </summary>        
        <Extension>
        Public Function VisualBasicKind(nodeOrToken As SyntaxNodeOrToken) As SyntaxKind
            Return If(nodeOrToken.Language Is LanguageNames.VisualBasic, CType(nodeOrToken.RawKind, SyntaxKind), SyntaxKind.None)
        End Function

        ''' <summary>
        ''' Determines if SyntaxTrivia is a specified kind.
        ''' </summary>        
        '''<param name="trivia">The Source SyntaxTrivia.</param>
        ''' <param name="kind">The SyntaxKind to test for.</param>
        <Extension>
        Public Function IsKind(trivia As SyntaxTrivia, kind As SyntaxKind) As Boolean
            Return trivia.VisualBasicKind = kind
        End Function

        ''' <summary>
        ''' Determines if SyntaxToken is a specified kind.
        ''' </summary>
        '''<param name="token">The Source SyntaxToken.</param>
        ''' <param name="kind">The SyntaxKind to test for.</param>
        <Extension>
        Public Function IsKind(token As SyntaxToken, kind As SyntaxKind) As Boolean
            Return token.VisualBasicKind = kind
        End Function

        ''' <summary>
        ''' Determines if SyntaxToken is a specified kind.
        ''' </summary>
        '''<param name="token">The Source SyntaxToken.</param>
        ''' <param name="kind">The SyntaxKind to test for.</param>
        ''' <returns>A boolean value if token is of specified kind; otherwise false.</returns>
        <Extension>
        Public Function IsContextualKind(token As SyntaxToken, kind As SyntaxKind) As Boolean
            Return token.VisualBasicContextualKind = kind
        End Function

        ''' <summary>
        ''' Determines if SyntaxNode is a specified kind.
        ''' </summary>
        ''' <param name="node">The Source SyntaxNode.</param>
        ''' <param name="kind">The SyntaxKind to test for.</param>
        ''' <returns>A boolean value if node is of specified kind; otherwise false.</returns>
        <Extension>
        Public Function IsKind(node As SyntaxNode, kind As SyntaxKind) As Boolean
            Return node IsNot Nothing AndAlso node.VisualBasicKind = kind
        End Function

        ''' <summary>
        ''' Determines if a SyntaxNodeOrToken is a specified kind.
        ''' </summary>
        ''' <param name="nodeOrToken">The source SyntaxNodeOrToke.</param>
        ''' <param name="kind">The SyntaxKind to test for.</param>
        ''' <returns>A boolean value if nodeoOrToken is of specified kind; otherwise false.</returns>
        <Extension>
        Public Function IsKind(nodeOrToken As SyntaxNodeOrToken, kind As SyntaxKind) As Boolean
            Return nodeOrToken.VisualBasicKind = kind
        End Function

        ''' <summary>
        ''' Tests whether a list contains tokens of a particular kind.
        ''' </summary>
        ''' <param name="kind">The <see cref="VisualBasic.SyntaxKind"/> to test for.</param>
        ''' <returns>Returns true if the list contains a token which matches <paramref name="kind"/></returns>
        <Extension>
        Public Function Any(list As SyntaxTriviaList, kind As SyntaxKind) As Boolean
            For Each trivia In list
                If trivia.IsKind(kind) Then
                    Return True
                End If
            Next
            Return False
        End Function

        ''' <summary>
        ''' Tests whether a list contains tokens of a particular kind.
        ''' </summary>
        ''' <param name="kind">The <see cref="VisualBasic.SyntaxKind"/> to test for.</param>
        ''' <returns>Returns true if the list contains a token which matches <paramref name="kind"/></returns>
        <Extension>
        Public Function Any(list As SyntaxTokenList, kind As SyntaxKind) As Boolean
            For Each token In list
                If token.IsKind(kind) Then
                    Return True
                End If
            Next
            Return False
        End Function

        <Extension>
        Friend Function FirstOrDefault(list As SyntaxTokenList, kind As SyntaxKind) As SyntaxToken
            If list.Count > 0 Then
                For Each element In list
                    If element.IsKind(kind) Then
                        Return element
                    End If
                Next
            End If
            Return Nothing
        End Function

        <Extension>
        Friend Function First(list As SyntaxTokenList, kind As SyntaxKind) As SyntaxToken
            If list.Count > 0 Then
                For Each element In list
                    If element.IsKind(kind) Then
                        Return element
                    End If
                Next
            End If
            Throw New InvalidOperationException()
        End Function


    End Module
End Namespace

Namespace Microsoft.CodeAnalysis.VisualBasic
    Public Module VisualBasicExtensions

        <Extension>
        Friend Function GetLocation(syntaxReference As SyntaxReference) As Location
            Dim tree = TryCast(syntaxReference.SyntaxTree, VisualBasicSyntaxTree)
            If syntaxReference.SyntaxTree IsNot Nothing Then
                If tree.IsEmbeddedSyntaxTree Then
                    Return New EmbeddedTreeLocation(tree.GetEmbeddedKind, syntaxReference.Span)
                ElseIf tree.IsMyTemplate Then
                    Return New MyTemplateLocation(tree, syntaxReference.Span)
                End If
            End If
            Return New SourceLocation(syntaxReference)
        End Function

        <Extension>
        Friend Function IsMyTemplate(syntaxTree As SyntaxTree) As Boolean
            Dim vbTree = TryCast(syntaxTree, VisualBasicSyntaxTree)
            Return vbTree IsNot Nothing AndAlso vbTree.IsMyTemplate
        End Function

        <Extension>
        Friend Function HasReferenceDirectives(syntaxTree As SyntaxTree) As Boolean
            Dim vbTree = TryCast(syntaxTree, VisualBasicSyntaxTree)
            Return vbTree IsNot Nothing AndAlso vbTree.HasReferenceDirectives
        End Function

        <Extension>
        Friend Function IsAnyPreprocessorSymbolDefined(syntaxTree As SyntaxTree, conditionalSymbolNames As IEnumerable(Of String), atNode As SyntaxNodeOrToken) As Boolean
            Dim vbTree = TryCast(syntaxTree, VisualBasicSyntaxTree)
            Return vbTree IsNot Nothing AndAlso vbTree.IsAnyPreprocessorSymbolDefined(conditionalSymbolNames, atNode)
        End Function

        <Extension>
        Friend Function GetVisualBasicSyntax(syntaxReference As SyntaxReference, Optional cancellationToken As CancellationToken = Nothing) As VisualBasicSyntaxNode
            Return DirectCast(syntaxReference.GetSyntax(cancellationToken), VisualBasicSyntaxNode)
        End Function

        <Extension>
        Friend Function GetVisualBasicRoot(syntaxTree As SyntaxTree, Optional cancellationToken As CancellationToken = Nothing) As VisualBasicSyntaxNode
            Return DirectCast(syntaxTree.GetRoot(cancellationToken), VisualBasicSyntaxNode)
        End Function

        <Extension>
        Friend Function GetPreprocessingSymbolInfo(syntaxTree As SyntaxTree, identifierNode As IdentifierNameSyntax) As VisualBasicPreprocessingSymbolInfo
            Dim vbTree = DirectCast(syntaxTree, VisualBasicSyntaxTree)
            Return vbTree.GetPreprocessingSymbolInfo(identifierNode)
        End Function

        <Extension>
        Friend Function Errors(trivia As SyntaxTrivia) As InternalSyntax.SyntaxDiagnosticInfoList
            Return New InternalSyntax.SyntaxDiagnosticInfoList(DirectCast(trivia.UnderlyingNode, InternalSyntax.VisualBasicSyntaxNode))
        End Function

        <Extension>
        Friend Function Errors(token As SyntaxToken) As InternalSyntax.SyntaxDiagnosticInfoList
            Return New InternalSyntax.SyntaxDiagnosticInfoList(DirectCast(token.Node, InternalSyntax.VisualBasicSyntaxNode))
        End Function

        <Extension>
        Friend Function GetSyntaxErrors(token As SyntaxToken, tree As SyntaxTree) As ReadOnlyCollection(Of Diagnostic)
            Return VisualBasicSyntaxNode.DoGetSyntaxErrors(tree, token)
        End Function

        <Extension>
        Friend Function AddError(node As GreenNode, diagnostic As DiagnosticInfo) As GreenNode
            Dim green = TryCast(node, InternalSyntax.VisualBasicSyntaxNode)
            Return green.AddError(diagnostic)
        End Function

        ''' <summary>
        ''' Checks to see if SyntaxToken is a bracketed identifier.
        ''' </summary>
        ''' <param name="token">The source SyntaxToken.</param>
        ''' <returns>A boolean value, True if token represents a bracketed Identifier.</returns>
        <Extension>
        Public Function IsBracketed(token As SyntaxToken) As Boolean
            If token.IsKind(SyntaxKind.IdentifierToken) Then
                Return DirectCast(token.Node, InternalSyntax.IdentifierTokenSyntax).IsBracketed
            End If
            Return False
        End Function

        ''' <summary>
        ''' Returns the Type character for a given syntax token.  This returns type character for Indentifiers or Integer, Floating Point or Decimal Literals.
        ''' Examples: Dim a$   or Dim l1 = 1L
        ''' </summary>
        ''' <param name="token">The source SyntaxToken.</param>
        ''' <returns>A type character used for the specific Internal Syntax Token Types.</returns>
        <Extension>
        Public Function GetTypeCharacter(token As SyntaxToken) As TypeCharacter
            Select Case token.VisualBasicKind()
                Case SyntaxKind.IdentifierToken
                    Dim id = DirectCast(token.Node, InternalSyntax.IdentifierTokenSyntax)
                    Return id.TypeCharacter

                Case SyntaxKind.IntegerLiteralToken
                    Dim literal = DirectCast(token.Node, InternalSyntax.IntegerLiteralTokenSyntax)
                    Return literal.TypeSuffix

                Case SyntaxKind.FloatingLiteralToken
                    Dim literal = DirectCast(token.Node, InternalSyntax.FloatingLiteralTokenSyntax)
                    Return literal.TypeSuffix

                Case SyntaxKind.DecimalLiteralToken
                    Dim literal = DirectCast(token.Node, InternalSyntax.DecimalLiteralTokenSyntax)
                    Return literal.TypeSuffix
            End Select

            Return Nothing
        End Function

        ''' <summary>
        ''' The source token base for Integer literals.  Base can be Decimal, Hex or Octal.
        ''' </summary>
        ''' <param name="token">The source SyntaxToken.</param>
        ''' <returns>An instance representing the integer literal base.</returns>
        <Extension>
        Public Function GetBase(token As SyntaxToken) As LiteralBase?
            If token.IsKind(SyntaxKind.IntegerLiteralToken) Then
                Dim tk = DirectCast(token.Node, InternalSyntax.IntegerLiteralTokenSyntax)
                Return tk.Base
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Determines if the token represents a reserved or contextual keyword
        ''' </summary>
        ''' <param name="token">The source SyntaxToken.</param>
        ''' <returns>A boolean value True if token is a keyword.</returns>
        <Extension>
        Public Function IsKeyword(token As SyntaxToken) As Boolean
            Return SyntaxFacts.IsKeywordKind(token.VisualBasicKind())
        End Function

        ''' <summary>
        ''' Determines if the token represents a reserved keyword
        ''' </summary>
        ''' <param name="token">The source SyntaxToken.</param>
        ''' <returns>A boolean value True if token is a reserved keyword.</returns>
        <Extension>
        Public Function IsReservedKeyword(token As SyntaxToken) As Boolean
            Return SyntaxFacts.IsReservedKeyword(token.VisualBasicKind())
        End Function

        ''' <summary>
        ''' Determines if the token represents a contextual keyword
        ''' </summary>
        ''' <returns>A boolean value True if token is a contextual keyword.</returns>
        <Extension>
        Public Function IsContextualKeyword(token As SyntaxToken) As Boolean
            Return SyntaxFacts.IsContextualKeyword(token.VisualBasicKind())
        End Function

        ''' <summary>
        ''' Determines if the token  represents a preprocessor keyword
        ''' </summary>
        ''' <param name="token">The source SyntaxToken.</param>
        ''' <returns> A boolean value True if token is a pre processor keyword.</returns>
        <Extension>
        Public Function IsPreprocessorKeyword(token As SyntaxToken) As Boolean
            Return SyntaxFacts.IsPreprocessorKeyword(token.VisualBasicKind())
        End Function

        ''' <summary>
        ''' Returns the Identifiertext for a specified SyntaxToken.
        ''' </summary>
        <Extension>
        Public Function GetIdentifierText(token As SyntaxToken) As String
            Return If(token.Node IsNot Nothing,
                        If(token.IsKind(SyntaxKind.IdentifierToken),
                            DirectCast(token.Node, InternalSyntax.IdentifierTokenSyntax).IdentifierText,
                            token.ToString()),
                        String.Empty)
        End Function

        ''' <summary>
        ''' Insert one or more tokens in the list at the specified index.
        ''' </summary>
        ''' <returns>A new list with the tokens inserted.</returns>
        <Extension>
        Public Function Insert(list As SyntaxTokenList, index As Integer, ParamArray items As SyntaxToken()) As SyntaxTokenList
            If index < 0 OrElse index > list.Count Then
                Throw New ArgumentOutOfRangeException("index")
            End If

            If items Is Nothing Then
                Throw New ArgumentNullException("items")
            End If

            If list.Count = 0 Then
                Return SyntaxFactory.TokenList(items)
            Else
                Dim builder = New Syntax.SyntaxTokenListBuilder(list.Count + items.Length)
                If index > 0 Then
                    builder.Add(list, 0, index)
                End If

                builder.Add(items)

                If index < list.Count Then
                    builder.Add(list, index, list.Count - index)
                End If

                Return builder.ToList()
            End If
        End Function

        ''' <summary>
        ''' Add one or more tokens to the end of the list.
        ''' </summary>
        ''' <returns>A new list with the tokens added.</returns>
        <Extension>
        Public Function Add(list As SyntaxTokenList, ParamArray items As SyntaxToken()) As SyntaxTokenList
            Return Insert(list, list.Count, items)
        End Function

        ''' <summary>
        '''  Replaces trivia on a specified SyntaxToken.
        ''' </summary>
        ''' <param name="token">The source SyntaxToken to change trivia on.</param>
        ''' <param name="oldTrivia">The original Trivia.</param>
        ''' <param name="newTrivia">The updated Trivia.</param>
        ''' <returns>The updated SyntaxToken with replaced trivia.</returns>
        <Extension>
        Public Function ReplaceTrivia(token As SyntaxToken, oldTrivia As SyntaxTrivia, newTrivia As SyntaxTrivia) As SyntaxToken
            Return SyntaxReplacer.Replace(token, trivia:={oldTrivia}, computeReplacementTrivia:=Function(o, r) newTrivia)
        End Function

        ''' <summary>
        '''  Replaces trivia on a specified SyntaxToken.
        ''' </summary>
        <Extension>
        Public Function ReplaceTrivia(token As SyntaxToken, trivia As IEnumerable(Of SyntaxTrivia), computeReplacementTrivia As Func(Of SyntaxTrivia, SyntaxTrivia, SyntaxTrivia)) As SyntaxToken
            Return SyntaxReplacer.Replace(token, trivia:=trivia, computeReplacementTrivia:=computeReplacementTrivia)
        End Function

        <Extension>
        Friend Function AsSeparatedList(Of TOther As SyntaxNode)(list As SyntaxNodeOrTokenList) As SeparatedSyntaxList(Of TOther)
            Dim builder = SeparatedSyntaxListBuilder(Of TOther).Create
            For Each i In list
                Dim node = i.AsNode
                If node IsNot Nothing Then
                    builder.Add(DirectCast(DirectCast(node, SyntaxNode), TOther))
                Else
                    builder.AddSeparator(i.AsToken)
                End If
            Next
            Return builder.ToList
        End Function

        ''' <summary>
        ''' Gets the DirectiveTriviaSyntax items for a specified SytaxNode with optional filtering.
        ''' </summary>
        ''' <param name="node">The source SyntaxNode.</param>
        ''' <param name="filter">The optional DirectiveTriva Syntax filter predicate.</param>
        ''' <returns>A list of DirectiveTriviaSyntax items</returns>
        <Extension>
        Public Function GetDirectives(node As SyntaxNode, Optional filter As Func(Of DirectiveTriviaSyntax, Boolean) = Nothing) As IList(Of DirectiveTriviaSyntax)
            Return DirectCast(node, VisualBasicSyntaxNode).GetDirectives(filter)
        End Function


        ''' <summary>
        ''' Gets the first DirectiveTriviaSyntax item for a specified SyntaxNode.
        ''' </summary>
        ''' <param name="node">The source SyntaxNode.</param>
        ''' <param name="predicate">The optional DirectiveTriviaSyntax filter predicate.</param>
        ''' <returns>The first DirectiveSyntaxTrivia item.</returns>
        <Extension> Public Function GetFirstDirective(node As SyntaxNode, Optional predicate As Func(Of DirectiveTriviaSyntax, Boolean) = Nothing) As DirectiveTriviaSyntax
            Return DirectCast(node, VisualBasicSyntaxNode).GetFirstDirective(predicate)
        End Function

        ''' <summary>
        ''' Gets the last DirectiveTriviaSyntax item for a specified SyntaxNode.
        ''' </summary>
        ''' <param name="node">The source node</param>
        ''' <param name="predicate">The optional DirectiveTriviaSyntax filter predicate.</param>
        ''' <returns>The last DirectiveSyntaxTrivia item.</returns>
        <Extension>
        Public Function GetLastDirective(node As SyntaxNode, Optional predicate As Func(Of DirectiveTriviaSyntax, Boolean) = Nothing) As DirectiveTriviaSyntax
            Return DirectCast(node, VisualBasicSyntaxNode).GetLastDirective(predicate)
        End Function

        ''' <summary>
        ''' Gets the root CompilationUnitSytax for a specified SyntaxTree.
        ''' </summary>
        ''' <param name="tree">The source SyntaxTree.</param>
        ''' <returns>A CompilationUnitSyntax.</returns>
        <Extension>
        Public Function GetCompilationUnitRoot(tree As SyntaxTree) As CompilationUnitSyntax
            Return DirectCast(tree.GetRoot(), CompilationUnitSyntax)
        End Function

        ''' <summary>
        '''  Gets the reporting state for a warning at a given source location based on warning directives.
        ''' </summary>
        <Extension>
        Friend Function GetWarningState(tree As SyntaxTree, id As String, position As Integer) As ReportDiagnostic
            Return DirectCast(tree, VisualBasicSyntaxTree).GetWarningState(id, position)
        End Function

#Region "Symbols"
        ''' <summary>
        ''' Determines if symbol is Shared.
        ''' </summary>
        ''' <param name="symbol">The source symbol.</param>
        ''' <returns>A boolean value, True if symbol is Shared; otherwise False.</returns>
        <Extension>
        Public Function IsShared(symbol As ISymbol) As Boolean
            Return symbol.IsStatic
        End Function

        <Extension>
        Public Function IsOverrides(symbol As ISymbol) As Boolean
            Return symbol.IsOverride
        End Function

        <Extension>
        Public Function IsOverridable(symbol As ISymbol) As Boolean
            Return symbol.IsVirtual
        End Function

        <Extension>
        Public Function IsNotOverridable(symbol As ISymbol) As Boolean
            Return symbol.IsSealed
        End Function

        <Extension>
        Public Function IsMustOverride(symbol As ISymbol) As Boolean
            Return symbol.IsAbstract
        End Function

        <Extension>
        Public Function IsMe(parameterSymbol As IParameterSymbol) As Boolean
            Return parameterSymbol.IsThis
        End Function

        <Extension>
        Public Function IsOverloads(methodSymbol As IMethodSymbol) As Boolean
            Dim vbmethod = TryCast(methodSymbol, MethodSymbol)
            Return vbmethod IsNot Nothing AndAlso vbmethod.IsOverloads
        End Function

        <Extension>
        Public Function IsOverloads(propertySymbol As IPropertySymbol) As Boolean
            Dim vbprop = TryCast(propertySymbol, PropertySymbol)
            Return vbprop IsNot Nothing AndAlso vbprop.IsOverloads
        End Function

        <Extension>
        Public Function IsDefault(propertySymbol As IPropertySymbol) As Boolean
            Dim vbprop = TryCast(propertySymbol, PropertySymbol)
            Return vbprop IsNot Nothing AndAlso vbprop.IsDefault
        End Function

        <Extension>
        Public Function HandledEvents(methodSymbol As IMethodSymbol) As ImmutableArray(Of HandledEvent)
            Dim vbmethod = TryCast(methodSymbol, MethodSymbol)
            Return vbmethod.HandledEvents
        End Function

        <Extension>
        Public Function IsFor(localSymbol As ILocalSymbol) As Boolean
            Dim vblocal = TryCast(localSymbol, LocalSymbol)
            Return vblocal IsNot Nothing AndAlso vblocal.IsFor
        End Function

        <Extension>
        Public Function IsForEach(localSymbol As ILocalSymbol) As Boolean
            Dim vblocal = TryCast(localSymbol, LocalSymbol)
            Return vblocal IsNot Nothing AndAlso vblocal.IsForEach
        End Function

        <Extension>
        Public Function IsCatch(localSymbol As ILocalSymbol) As Boolean
            Dim vblocal = TryCast(localSymbol, LocalSymbol)
            Return vblocal IsNot Nothing AndAlso vblocal.IsCatch
        End Function

        <Extension>
        Public Function AssociatedField(eventSymbol As IEventSymbol) As IFieldSymbol
            Dim vbevent = TryCast(eventSymbol, EventSymbol)
            Return If(vbevent IsNot Nothing, vbevent.AssociatedField, Nothing)
        End Function

        <Extension>
        Public Function HasAssociatedField(eventSymbol As IEventSymbol) As Boolean
            Dim vbevent = TryCast(eventSymbol, EventSymbol)
            Return vbevent IsNot Nothing AndAlso vbevent.HasAssociatedField
        End Function

        <Extension>
        Public Function GetFieldAttributes(eventSymbol As IEventSymbol) As ImmutableArray(Of AttributeData)
            Dim vbevent = TryCast(eventSymbol, EventSymbol)
            If vbevent IsNot Nothing Then
                Return StaticCast(Of AttributeData).From(vbevent.GetFieldAttributes())
            Else
                Return ImmutableArray(Of AttributeData).Empty
            End If
        End Function

        <Extension>
        Public Function IsImplicitlyDeclared(eventSymbol As IEventSymbol) As Boolean
            Dim vbevent = TryCast(eventSymbol, EventSymbol)
            Return vbevent IsNot Nothing AndAlso vbevent.IsImplicitlyDeclared
        End Function

        ''' <summary>
        ''' Gets all module members in a namespace.
        ''' </summary>
        ''' <param name="[namespace]">The source namespace symbol.</param>
        ''' <returns>An ImmutableArray of NamedTypeSymbol for all module members in namespace.</returns>
        <Extension>
        Public Function GetModuleMembers([namespace] As INamespaceSymbol) As ImmutableArray(Of INamedTypeSymbol)
            Dim symbol = TryCast([namespace], NamespaceSymbol)
            If symbol IsNot Nothing Then
                Return StaticCast(Of INamedTypeSymbol).From(symbol.GetModuleMembers())
            Else
                Return ImmutableArray.Create(Of INamedTypeSymbol)()
            End If
        End Function

        ''' <summary>
        ''' Gets all module members in a specified namespace.
        ''' </summary>
        ''' <param name="[namespace]">The source namespace symbol.</param>
        ''' <param name="name">The name of the namespace.</param>
        ''' <returns>An ImmutableArray of NamedTypeSymbol for all module members in namespace.</returns>
        <Extension>
        Public Function GetModuleMembers([namespace] As INamespaceSymbol, name As String) As ImmutableArray(Of INamedTypeSymbol)
            Dim symbol = TryCast([namespace], NamespaceSymbol)
            If symbol IsNot Nothing Then
                Return StaticCast(Of INamedTypeSymbol).From(symbol.GetModuleMembers(name))
            Else
                Return ImmutableArray.Create(Of INamedTypeSymbol)()
            End If
        End Function
#End Region

#Region "Compilation"
        ''' <summary>
        ''' Gets the Semantic Model OptionStrict property.
        ''' </summary>
        ''' <param name="semanticModel">A source Semantic model object.</param>
        ''' <returns>The OptionStrict object for the semantic model instance OptionStrict property, otherise Null if semantic model is Null. </returns>
        <Extension>
        Public Function OptionStrict(semanticModel As SemanticModel) As OptionStrict
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.OptionStrict
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the Semantic Model OptionInfer property.
        ''' </summary>
        ''' <param name="semanticModel">A source Semantic model object.</param>
        ''' <returns>A boolean values, for the semantic model instance OptionInfer property. otherise Null if semantic model is Null. </returns>
        <Extension>
        Public Function OptionInfer(semanticModel As SemanticModel) As Boolean
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.OptionInfer
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the Semantic Model OptionExplicit property.
        ''' </summary>
        ''' <param name="semanticModel">A source Semantic model object.</param>
        ''' <returns>A boolean values, for the semantic model instance OptionExplicit property. otherise Null if semantic model is Null. </returns>
        <Extension>
        Public Function OptionExplicit(semanticModel As SemanticModel) As Boolean
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.OptionExplicit
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the Semantic Model OptionCompareText property.
        ''' </summary>
        ''' <param name="semanticModel">A source Semantic model object.</param>
        ''' <returns>A boolean values, for the semantic model instance OptionCompareText property. otherise Null if semantic model is Null. </returns>
        <Extension>
        Public Function OptionCompareText(semanticModel As SemanticModel) As Boolean
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.OptionCompareText
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the compilation RootNamespace property.
        ''' </summary>      
        ''' <param name="compilation">A source Compilation object.</param>
        ''' <returns>A NamespaceSymbol instance, for the compilation instance RootNamespace property. otherwise Null if compilation instance is Null. </returns>
        <Extension>
        Public Function RootNamespace(compilation As Compilation) As INamespaceSymbol
            Dim vbcomp = TryCast(compilation, VisualBasicCompilation)
            If vbcomp IsNot Nothing Then
                Return vbcomp.RootNamespace
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the compilation AliasImports property.
        ''' </summary>
        ''' <param name="compilation">A source Compilation object.</param>
        ''' <returns>An ImmutableArray of AliasSymbol, from the compilation instance AliasImports property; otherwise an empty ImmutableArray if compilation instance is Null.</returns>
        <Extension>
        Public Function AliasImports(compilation As Compilation) As ImmutableArray(Of IAliasSymbol)
            Dim vbcomp = TryCast(compilation, VisualBasicCompilation)
            If vbcomp IsNot Nothing Then
                Return StaticCast(Of IAliasSymbol).From(vbcomp.AliasImports)
            Else
                Return ImmutableArray.Create(Of IAliasSymbol)()
            End If
        End Function

        ''' <summary>
        '''  Gets the compilation MemberImports property.
        ''' </summary>
        ''' <param name="compilation">A source Compilation object.</param>
        ''' <returns>An ImmutableArray of NamespaceOrTypeSymbol, from the compilation instance MemberImports property; otherwise an empty ImmutableArray if compilation instance is Null.</returns>
        <Extension>
        Public Function MemberImports(compilation As Compilation) As ImmutableArray(Of INamespaceOrTypeSymbol)
            Dim vbcomp = TryCast(compilation, VisualBasicCompilation)
            If vbcomp IsNot Nothing Then
                Return StaticCast(Of INamespaceOrTypeSymbol).From(vbcomp.MemberImports)
            Else
                Return ImmutableArray.Create(Of INamespaceOrTypeSymbol)()
            End If
        End Function

        ''' <summary>
        ''' Determines what kind of conversion there is between the specified types.
        ''' </summary>
        ''' <param name="compilation">A source Compilation object.</param>
        ''' <param name="source">A source Typesymbol</param>
        ''' <param name="destination">A destination Typesymbol</param>
        ''' <returns>A Conversion instance, representing the kind of conversion between the two type symbols; otherwise Null if compilation instance is Null.</returns>
        <Extension>
        Public Function ClassifyConversion(compilation As Compilation, source As ITypeSymbol, destination As ITypeSymbol) As Conversion
            Dim vbcomp = TryCast(compilation, VisualBasicCompilation)
            If vbcomp IsNot Nothing Then
                Return vbcomp.ClassifyConversion(DirectCast(source, TypeSymbol), DirectCast(destination, TypeSymbol))
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the special type symbol in current compilation.
        ''' </summary>
        ''' <param name="compilation">A source Compilation object.</param>
        ''' <param name="typeId">The SpecialType to get.</param>
        ''' <returns>A NamedTypeSymbol for the specified type in compilation; Null if compilation is Null.</returns>
        <Extension>
        Public Function GetSpecialType(compilation As Compilation, typeId As SpecialType) As INamedTypeSymbol
            Dim vbcomp = TryCast(compilation, VisualBasicCompilation)
            If vbcomp IsNot Nothing Then
                Return vbcomp.GetSpecialType(typeId)
            Else
                Return Nothing
            End If
        End Function
#End Region

#Region "SemanticModel"
        ''' <summary>
        ''' Determines what kind of conversion there is between the expression syntax and a specified type.
        ''' </summary>
        ''' <param name="semanticModel">A source semantic model.</param>
        ''' <param name="expression">A source expression syntax.</param>
        ''' <param name="destination">A destination TypeSymbol.</param>
        ''' <returns>A Conversion instance, representing the kind of conversion between the expression and type symbol; otherwise Null if semantic model instance is Null.</returns>     
        <Extension>
        Public Function ClassifyConversion(semanticModel As SemanticModel, expression As ExpressionSyntax, destination As ITypeSymbol) As Conversion
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.ClassifyConversion(expression, DirectCast(destination, TypeSymbol))
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Determines what kind of conversion there is between the expression syntax and a specified type.
        ''' </summary>
        ''' <param name="semanticModel">A source semantic model.</param>
        ''' <param name="position">A position within the expression syntax.</param>
        ''' <param name="expression">A source expression syntax.</param>
        ''' <param name="destination">A destination TypeSymbol.</param>
        ''' <returns>A Conversion instance, representing the kind of conversion between the expression and type symbol; otherwise Null if semantic model instance is Null.</returns>
        <Extension>
        Public Function ClassifyConversion(semanticModel As SemanticModel, position As Integer, expression As ExpressionSyntax, destination As ITypeSymbol) As Conversion
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.ClassifyConversion(position, expression, DirectCast(destination, TypeSymbol))
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding symbol for a specified identifier.
        ''' </summary>
        ''' <param name="semanticModel">A source semantic model.</param>
        ''' <param name="identifierSyntax">A IdentiferSyntax object.</param>
        ''' <param name="cancellationToken">A cancellation token.</param>
        ''' <returns>A symbol, for the specified identifier; otherwise Null if semantic model is Null. </returns>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, identifierSyntax As ModifiedIdentifierSyntax, Optional cancellationToken As CancellationToken = Nothing) As ISymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(identifierSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding PropertySymbol for a specified FieldInitializerSyntax.
        ''' </summary>
        ''' <param name="semanticModel">A source semantic model.</param>
        ''' <param name="fieldInitializerSyntax">A fieldInitizerSyntax object.</param>
        ''' <param name="cancellationToken">A cancellation token.</param>
        ''' <returns>A PropertySymbol. Null if semantic model is null.</returns>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, fieldInitializerSyntax As FieldInitializerSyntax, Optional cancellationToken As CancellationToken = Nothing) As IPropertySymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(fieldInitializerSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding  NamedTypeSymbol for a specified  AnonymousObjectCreationExpressionSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, anonymousObjectCreationExpressionSyntax As AnonymousObjectCreationExpressionSyntax, Optional cancellationToken As CancellationToken = Nothing) As INamedTypeSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(anonymousObjectCreationExpressionSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding RangeVariableSymbol for a specified ExpressionRangeVariableSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, rangeVariableSyntax As ExpressionRangeVariableSyntax, Optional cancellationToken As CancellationToken = Nothing) As IRangeVariableSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(rangeVariableSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding RangeVariableSymbol for a specified CollectionRangeVariableSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, rangeVariableSyntax As CollectionRangeVariableSyntax, Optional cancellationToken As CancellationToken = Nothing) As IRangeVariableSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(rangeVariableSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding RangeVariableSymbol for a specified AggregationRangeVariableSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, rangeVariableSyntax As AggregationRangeVariableSyntax, Optional cancellationToken As CancellationToken = Nothing) As IRangeVariableSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(rangeVariableSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding LabelSymbol for a specified LabelStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As LabelStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As ILabelSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding FieldSymbol for a specified EnumMemberDeclarationSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As EnumMemberDeclarationSyntax, Optional cancellationToken As CancellationToken = Nothing) As IFieldSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding NamedTypeSymbol for a specified TypeStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As TypeStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As INamedTypeSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding NamedTypeSymbol for a specified TypeBlockSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As TypeBlockSyntax, Optional cancellationToken As CancellationToken = Nothing) As INamedTypeSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding NamedTypeSymbol for a specified EnumStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As EnumStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As INamedTypeSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding NamedTypeSymbol for a specified EnumBlockSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As EnumBlockSyntax, Optional cancellationToken As CancellationToken = Nothing) As INamedTypeSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding NamespaceSymbol for a specified NamespaceStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As NamespaceStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As INamespaceSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding NamespaceSymbol for a specified NamespaceBlockSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As NamespaceBlockSyntax, Optional cancellationToken As CancellationToken = Nothing) As INamespaceSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding ParameterSymbol for a specified ParameterSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, parameter As ParameterSyntax, Optional cancellationToken As CancellationToken = Nothing) As IParameterSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(parameter, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding TypeParameterSymbol Symbol for a specified TypeParameterSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, typeParameter As TypeParameterSyntax, Optional cancellationToken As CancellationToken = Nothing) As ITypeParameterSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(typeParameter, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding NamedTypeSymbol for a specified DelegateStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As DelegateStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As INamedTypeSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding MethodSymbol for a specified SubNewStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As SubNewStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As IMethodSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding MethodSymbol for a specified MethodStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As MethodStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As IMethodSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding symbol for a specified  DeclareStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As DeclareStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As IMethodSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding MethodSymbol for a specified OperatorStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As OperatorStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As IMethodSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding MethodSymbol for a specified MethodBlockBaseSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As MethodBlockBaseSyntax, Optional cancellationToken As CancellationToken = Nothing) As IMethodSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding PropertySymbol for a specified PropertyStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As PropertyStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As IPropertySymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding EventSymbol for a specified EventStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As EventStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As IEventSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding PropertySymbol for a specified PropertyBlockSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As PropertyBlockSyntax, Optional cancellationToken As CancellationToken = Nothing) As IPropertySymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding EventSymbol for a specified EventBlockSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As EventBlockSyntax, Optional cancellationToken As CancellationToken = Nothing) As IEventSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding LocalSymbol for a specified CatchStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As CatchStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As ILocalSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding MethodSymbol for a specified AccessorStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As AccessorStatementSyntax, Optional cancellationToken As CancellationToken = Nothing) As IMethodSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding AliasSymbol for a specified AliasImportsClauseSyntax.
        ''' </summary>
        <Extension>
        Public Function GetDeclaredSymbol(semanticModel As SemanticModel, declarationSyntax As AliasImportsClauseSyntax, Optional cancellationToken As CancellationToken = Nothing) As IAliasSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetDeclaredSymbol(declarationSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding ForEachStatementInfo containing semantic info for a specified ForEachStatementSyntax.
        ''' </summary>
        <Extension>
        Public Function GetForEachStatementInfo(semanticModel As SemanticModel, node As ForEachStatementSyntax) As ForEachStatementInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetForEachStatementInfo(node)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding ForEachStatementInfo containing semantic info for a specified ForBlockSyntax.
        ''' </summary>
        <Extension>
        Public Function GetForEachStatementInfo(semanticModel As SemanticModel, node As ForBlockSyntax) As ForEachStatementInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetForEachStatementInfo(node)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding AwaitExpressionInfo containing semantic info for a specified AwaitExpressionSyntax.
        ''' </summary>
        <Extension>
        Public Function GetAwaitExpressionInfo(semanticModel As SemanticModel, awaitExpression As AwaitExpressionSyntax, Optional cancellationToken As CancellationToken = Nothing) As AwaitExpressionInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetAwaitExpressionInfo(awaitExpression, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' If the given node is within a preprocessing directive, gets the preprocessing symbol info for it.
        ''' </summary>
        <Extension>
        Public Function GetPreprocessingSymbolInfo(semanticModel As SemanticModel, node As IdentifierNameSyntax) As PreprocessingSymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetPreprocessingSymbolInfo(node)
            Else
                Return PreprocessingSymbolInfo.None
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding SymbolInfo containing semantic info for a specified ExpressionSyntax.
        ''' </summary>
        <Extension>
        Public Function GetSymbolInfo(semanticModel As SemanticModel, expression As ExpressionSyntax, Optional cancellationToken As CancellationToken = Nothing) As SymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSymbolInfo(expression, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Returns what 'Add' method symbol(s), if any, corresponds to the given expression syntax 
        ''' within <see cref="ObjectCollectionInitializerSyntax.Initializer"/>.
        ''' </summary>
        <Extension>
        Public Function GetCollectionInitializerSymbolInfo(semanticModel As SemanticModel, expression As ExpressionSyntax, Optional cancellationToken As CancellationToken = Nothing) As SymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetCollectionInitializerSymbolInfo(expression, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding SymbolInfo containing semantic info for a specified CrefReferenceSyntax.
        ''' </summary>
        <Extension>
        Public Function GetSymbolInfo(semanticModel As SemanticModel, crefReference As CrefReferenceSyntax, Optional cancellationToken As CancellationToken = Nothing) As SymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSymbolInfo(crefReference, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding SymbolInfo containing semantic info for a specified AttributeSyntax.
        ''' </summary>
        <Extension>
        Public Function GetSymbolInfo(semanticModel As SemanticModel, attribute As AttributeSyntax, Optional cancellationToken As CancellationToken = Nothing) As SymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSymbolInfo(attribute, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding SymbolInfo containing semantic info for a specified AttributeSyntax.
        ''' </summary>
        <Extension>
        Public Function GetSpeculativeSymbolInfo(semanticModel As SemanticModel, position As Integer, expression As ExpressionSyntax, bindingOption As SpeculativeBindingOption) As SymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSpeculativeSymbolInfo(position, expression, bindingOption)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding SymbolInfo containing semantic info for specified  AttributeSyntax at a given position, used in Semantic Info for items not appearing in source code.
        ''' </summary>
        <Extension>
        Public Function GetSpeculativeSymbolInfo(semanticModel As SemanticModel, position As Integer, attribute As AttributeSyntax) As SymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSpeculativeSymbolInfo(position, attribute)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding TypeInfo containing semantic info for a specified ExpressionSyntax.
        ''' </summary>
        <Extension>
        Public Function GetConversion(semanticModel As SemanticModel, expression As SyntaxNode, Optional cancellationToken As CancellationToken = Nothing) As Conversion
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetConversion(expression, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        <Extension>
        Public Function GetSpeculativeConversion(semanticModel As SemanticModel, position As Integer, expression As ExpressionSyntax, bindingOption As SpeculativeBindingOption) As Conversion
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSpeculativeConversion(position, expression, bindingOption)
            Else
                Return Nothing
            End If
        End Function

        <Extension>
        Public Function GetTypeInfo(semanticModel As SemanticModel, expression As ExpressionSyntax, Optional cancellationToken As CancellationToken = Nothing) As TypeInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetTypeInfo(expression, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding TypeInfo  containing semantic info for a speculating an ExpressionSyntax at a given position, used in Semantic Info for items not appearing in source code.
        ''' </summary>
        <Extension>
        Public Function GetSpeculativeTypeInfo(semanticModel As SemanticModel, position As Integer, expression As ExpressionSyntax, bindingOption As SpeculativeBindingOption) As TypeInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSpeculativeTypeInfo(position, expression, bindingOption)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding TypeInfo containing semantic info for a specified AttributeSyntax.
        ''' </summary>
        <Extension>
        Public Function GetTypeInfo(semanticModel As SemanticModel, attribute As AttributeSyntax, Optional cancellationToken As CancellationToken = Nothing) As TypeInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetTypeInfo(attribute, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding ImmutableArray of Symbols for a specified ExpressionSyntax.
        ''' </summary>
        <Extension>
        Public Function GetMemberGroup(semanticModel As SemanticModel, expression As ExpressionSyntax, Optional cancellationToken As CancellationToken = Nothing) As ImmutableArray(Of ISymbol)
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetMemberGroup(expression, cancellationToken)
            Else
                Return ImmutableArray.Create(Of ISymbol)
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding ImmutableArray of Symbols for a speculating an ExpressionSyntax at a given position, used in Semantic Info for items not appearing in source code.
        ''' </summary>
        <Extension>
        Public Function GetSpeculativeMemberGroup(semanticModel As SemanticModel, position As Integer, expression As ExpressionSyntax) As ImmutableArray(Of ISymbol)
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSpeculativeMemberGroup(position, expression)
            Else
                Return ImmutableArray.Create(Of ISymbol)
            End If
        End Function

        ''' <summary>
        ''' Gets the corresponding ImmutableArray of Symbols for a specified AttributeSyntax.
        ''' </summary>
        <Extension>
        Public Function GetMemberGroup(semanticModel As SemanticModel, attribute As AttributeSyntax, Optional cancellationToken As CancellationToken = Nothing) As ImmutableArray(Of ISymbol)
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetMemberGroup(attribute, cancellationToken)
            Else
                Return ImmutableArray.Create(Of ISymbol)
            End If
        End Function

        ''' <summary>
        ''' If "nameSyntax" resolves to an alias name, return the AliasSymbol corresponding
        ''' to A. Otherwise return null.
        ''' </summary>
        <Extension>
        Public Function GetAliasInfo(semanticModel As SemanticModel, nameSyntax As IdentifierNameSyntax, Optional cancellationToken As CancellationToken = Nothing) As IAliasSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetAliasInfo(nameSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Binds the name in the context of the specified location and sees if it resolves to an
        ''' alias name. If it does, return the AliasSymbol corresponding to it. Otherwise, return null.
        ''' </summary>
        <Extension>
        Public Function GetSpeculativeAliasInfo(semanticModel As SemanticModel, position As Integer, nameSyntax As IdentifierNameSyntax, bindingOption As SpeculativeBindingOption) As IAliasSymbol
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSpeculativeAliasInfo(position, nameSyntax, bindingOption)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Returns information about methods associated with CollectionRangeVariableSyntax.
        ''' </summary>
        <Extension>
        Public Function GetCollectionRangeVariableSymbolInfo(
            semanticModel As SemanticModel,
            variableSyntax As CollectionRangeVariableSyntax,
            Optional cancellationToken As CancellationToken = Nothing
        ) As CollectionRangeVariableSymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetCollectionRangeVariableSymbolInfo(variableSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Returns information about methods associated with AggregateClauseSyntax.
        ''' </summary>
        <Extension>
        Public Function GetAggregateClauseSymbolInfo(
            semanticModel As SemanticModel,
            aggregateSyntax As AggregateClauseSyntax,
            Optional cancellationToken As CancellationToken = Nothing
        ) As AggregateClauseSymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetAggregateClauseSymbolInfo(aggregateSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' DistinctClauseSyntax -       Returns Distinct method associated with DistinctClauseSyntax.
        ''' 
        ''' WhereClauseSyntax -          Returns Where method associated with WhereClauseSyntax.
        ''' 
        ''' PartitionWhileClauseSyntax - Returns TakeWhile/SkipWhile method associated with PartitionWhileClauseSyntax.
        ''' 
        ''' PartitionClauseSyntax -      Returns Take/Skip method associated with PartitionClauseSyntax.
        ''' 
        ''' GroupByClauseSyntax -        Returns GroupBy method associated with GroupByClauseSyntax.
        ''' 
        ''' JoinClauseSyntax -           Returns Join/GroupJoin method associated with JoinClauseSyntax/GroupJoinClauseSyntax.
        ''' 
        ''' SelectClauseSyntax -         Returns Select method associated with SelectClauseSyntax, if needed.
        ''' 
        ''' FromClauseSyntax -           Returns Select method associated with FromClauseSyntax, which has only one 
        '''                              CollectionRangeVariableSyntax and is the only query clause within 
        '''                              QueryExpressionSyntax. NotNeeded SymbolInfo otherwise. 
        '''                              The method call is injected by the compiler to make sure that query is translated to at 
        '''                              least one method call. 
        ''' 
        ''' LetClauseSyntax -            NotNeeded SymbolInfo.
        ''' 
        ''' OrderByClauseSyntax -        NotNeeded SymbolInfo.
        ''' 
        ''' AggregateClauseSyntax -      Empty SymbolInfo. GetAggregateClauseInfo should be used instead.
        ''' </summary>
        <Extension>
        Public Function GetSymbolInfo(
            semanticModel As SemanticModel,
            clauseSyntax As QueryClauseSyntax,
            Optional cancellationToken As CancellationToken = Nothing
        ) As SymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSymbolInfo(clauseSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Returns Select method associated with ExpressionRangeVariableSyntax within a LetClauseSyntax, if needed.
        ''' NotNeeded SymbolInfo otherwise.
        ''' </summary>
        <Extension>
        Public Function GetSymbolInfo(
            semanticModel As SemanticModel,
            variableSyntax As ExpressionRangeVariableSyntax,
            Optional cancellationToken As CancellationToken = Nothing
        ) As SymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSymbolInfo(variableSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Returns aggregate function associated with FunctionAggregationSyntax.
        ''' </summary>
        <Extension>
        Public Function GetSymbolInfo(
            semanticModel As SemanticModel,
            functionSyntax As FunctionAggregationSyntax,
            Optional cancellationToken As CancellationToken = Nothing
        ) As SymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSymbolInfo(functionSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Returns OrdrBy/OrderByDescending/ThenBy/ThenByDescending method associated with OrderingSyntax.
        ''' </summary>
        <Extension>
        Public Function GetSymbolInfo(
            semanticModel As SemanticModel,
            orderingSyntax As OrderingSyntax,
            Optional cancellationToken As CancellationToken = Nothing
        ) As SymbolInfo
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.GetSymbolInfo(orderingSyntax, cancellationToken)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Analyze control-flow within a part of a method body.
        ''' </summary>
        <Extension>
        Public Function AnalyzeControlFlow(semanticModel As SemanticModel, firstStatement As StatementSyntax, lastStatement As StatementSyntax) As ControlFlowAnalysis
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.AnalyzeControlFlow(firstStatement, lastStatement)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Analyze control-flow within a part of a method body.
        ''' </summary>
        <Extension>
        Public Function AnalyzeControlFlow(semanticModel As SemanticModel, statement As StatementSyntax) As ControlFlowAnalysis
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.AnalyzeControlFlow(statement)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Analyze data-flow within an expression. 
        ''' </summary>
        <Extension>
        Public Function AnalyzeDataFlow(semanticModel As SemanticModel, expression As ExpressionSyntax) As DataFlowAnalysis
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.AnalyzeDataFlow(expression)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Analyze data-flow within a set of contiguous statements.
        ''' </summary>
        <Extension>
        Public Function AnalyzeDataFlow(semanticModel As SemanticModel, firstStatement As StatementSyntax, lastStatement As StatementSyntax) As DataFlowAnalysis
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.AnalyzeDataFlow(firstStatement, lastStatement)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Analyze data-flow within a statement.
        ''' </summary>
        <Extension>
        Public Function AnalyzeDataFlow(semanticModel As SemanticModel, statement As StatementSyntax) As DataFlowAnalysis
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.AnalyzeDataFlow(statement)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Gets the SemanticModel for a MethodBlockBaseSyntax at a given position, used in Semantic Info for items not appearing in source code.
        ''' </summary>
        <Extension>
        Public Function TryGetSpeculativeSemanticModelForMethodBody(semanticModel As SemanticModel, position As Integer, method As MethodBlockBaseSyntax, <Out> ByRef speculativeModel As SemanticModel) As Boolean
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.TryGetSpeculativeSemanticModelForMethodBody(position, method, speculativeModel)
            Else
                speculativeModel = Nothing
                Return False
            End If
        End Function

        ''' <summary>
        ''' Gets the SemanticModel for a RangeArgumentSyntax at a given position, used in Semantic Info for items not appearing in source code.
        ''' </summary>
        <Extension>
        Public Function TryGetSpeculativeSemanticModel(semanticModel As SemanticModel, position As Integer, rangeArgument As RangeArgumentSyntax, <Out> ByRef speculativeModel As SemanticModel) As Boolean
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.TryGetSpeculativeSemanticModel(position, rangeArgument, speculativeModel)
            Else
                speculativeModel = Nothing
                Return False
            End If
        End Function

        ''' <summary>
        ''' Gets the SemanticModel for a ExecutableStatementSyntax at a given position, used in Semantic Info for items not appearing in source code.
        ''' </summary>
        <Extension>
        Public Function TryGetSpeculativeSemanticModel(semanticModel As SemanticModel, position As Integer, statement As ExecutableStatementSyntax, <Out> ByRef speculativeModel As SemanticModel) As Boolean
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.TryGetSpeculativeSemanticModel(position, statement, speculativeModel)
            Else
                speculativeModel = Nothing
                Return False
            End If
        End Function

        ''' <summary>
        ''' Gets the SemanticModel for a EqualsValueSyntax at a given position, used in Semantic Info for items not appearing in source code.
        ''' </summary>
        <Extension>
        Public Function TryGetSpeculativeSemanticModel(semanticModel As SemanticModel, position As Integer, initializer As EqualsValueSyntax, <Out> ByRef speculativeModel As SemanticModel) As Boolean
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.TryGetSpeculativeSemanticModel(position, initializer, speculativeModel)
            Else
                speculativeModel = Nothing
                Return False
            End If
        End Function

        ''' <summary>
        ''' Gets the SemanticModel for a AttributeSyntax at a given position, used in Semantic Info for items not appearing in source code.
        ''' </summary>
        <Extension>
        Public Function TryGetSpeculativeSemanticModel(semanticModel As SemanticModel, position As Integer, attribute As AttributeSyntax, <Out> ByRef speculativeModel As SemanticModel) As Boolean
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.TryGetSpeculativeSemanticModel(position, attribute, speculativeModel)
            Else
                speculativeModel = Nothing
                Return False
            End If
        End Function

        ''' <summary>
        ''' Gets the SemanticModel for a TypeSyntax at a given position, used in Semantic Info for items not appearing in source code.
        ''' </summary>
        <Extension>
        Public Function TryGetSpeculativeSemanticModel(semanticModel As SemanticModel, position As Integer, type As TypeSyntax, <Out> ByRef speculativeModel As SemanticModel, Optional bindingOption As SpeculativeBindingOption = SpeculativeBindingOption.BindAsExpression) As Boolean
            Dim vbmodel = TryCast(semanticModel, VisualBasicSemanticModel)
            If vbmodel IsNot Nothing Then
                Return vbmodel.TryGetSpeculativeSemanticModel(position, type, speculativeModel, bindingOption)
            Else
                speculativeModel = Nothing
                Return False
            End If
        End Function

#End Region

    End Module

End Namespace
