﻿using System;
using System.Collections.Generic;

namespace CompilerPascal
{
    public partial class Parser
    {
        public List<NodeDescriptions> ParseDescription()
        {
            List<NodeDescriptions> types = new List<NodeDescriptions>();
            while (Expect(KeyWord.VAR, KeyWord.CONST, KeyWord.TYPE, KeyWord.FUNCTION))
            {
                switch (currentLex.Value)
                {
                    case KeyWord.VAR:
                        types.Add(ParseVarDescription());
                        break;
                    case KeyWord.CONST:
                        types.Add(ParseConstDescription());
                        break;
                    case KeyWord.TYPE:
                        types.Add(ParseTypeDescription());
                        break;
                    case KeyWord.FUNCTION:
                        types.Add(ParseFunctionDescription());
                        break;
                }
            }
            return types;
        }

        public NodeDescriptions ParseVarDescription()
        {
            List<VarDeclarationNode> body = new List<VarDeclarationNode>();

            currentLex = lexer.GetLexem();
            RequireType(LexemaType.IDENTIFIER);

            while (ExpectType(LexemaType.IDENTIFIER))
            {
                body.Add(ParseVariableDescription());
                Require(Separator.Semiсolon); // ;
            }
            return new VarTypesNode(body);
        }

        public NodeDescriptions ParseConstDescription()
        {
            List<ConstDeclarationNode> body = new List<ConstDeclarationNode>();

            currentLex = lexer.GetLexem();
            RequireType(LexemaType.IDENTIFIER);

            while (ExpectType(LexemaType.IDENTIFIER))
            {
                string name = (string)currentLex.Value;
                symTableStack.Check(name);

                currentLex = lexer.GetLexem();
                Require(Operator.Equal); // =

                NodeExpression value;
                value = ParseExpression();
                Require(Separator.Semiсolon); // ;

                SymVarConst varConst = new SymVarConst(name, value.GetCachedType(), value);
                symTableStack.Add(name, varConst);

                body.Add(new ConstDeclarationNode(varConst, value));
            }
            return new ConstTypesNode(body);
        }

        public NodeDescriptions ParseTypeDescription()
        {
            List<DeclarationNode> body = new List<DeclarationNode>();

            currentLex = lexer.GetLexem();
            RequireType(LexemaType.IDENTIFIER);

            while (ExpectType(LexemaType.IDENTIFIER))
            {
                string nameType;
                nameType = (string)currentLex.Value;

                currentLex = lexer.GetLexem();
                Require(Operator.Equal); // =

                SymbolType type;
                type = ParseType();

                SymTypeAlias typeAlias = new SymTypeAlias(type.ToString(), type);
                symTableStack.Add(nameType, type);

                TypeDeclarationNode res;
                res = new TypeDeclarationNode(nameType, typeAlias);
                body.Add(res);

                Require(Separator.Semiсolon); // ;
            }
            return new TypeTypesNode(body);
        }

        public VarDeclarationNode ParseVariableDescription()
        {
            List<string>    names = new List<string>();
            List<SymbolVar> vars  = new List<SymbolVar>();
            NodeExpression  value = null;
            SymbolType type;

            while (ExpectType(LexemaType.IDENTIFIER))
            {
                names.Add((string)currentLex.Value);
                symTableStack.Check(names[^1]);

                currentLex = lexer.GetLexem();
                NotRequireType(LexemaType.IDENTIFIER);

                if (Expect(Separator.Comma))
                {
                    currentLex = lexer.GetLexem();
                    RequireType(LexemaType.IDENTIFIER);
                }
            }
            Require(Operator.Colon);
            type = ParseType();

            if (Expect(Operator.Equal))
            {
                if (names.Count > 1)
                {
                    throw new Except(currentLex.Line_number, currentLex.Symbol_number, "Only one variable can be initialized");
                }
                currentLex = lexer.GetLexem();
                value = ParseExpression();
            }

            foreach (string name in names)
            {
                SymbolVar var = new SymbolVar(name, type);
                symTableStack.Add(name, var);
                vars.Add(var);
            }
            return new VarDeclarationNode(vars, type, value);
        }

        public FuncTypesNode ParseFunctionDescription()
        {
            string name;
            List<VarDeclarationNode> param = new List<VarDeclarationNode>();
            List<NodeDescriptions> locals;

            currentLex = lexer.GetLexem();
            RequireType(LexemaType.IDENTIFIER);
            name = (string)currentLex.Value;
            currentLex = lexer.GetLexem();

            SymbolType type;

            if (Expect(Separator.OpenBracket))
            {
                currentLex = lexer.GetLexem();
                while (!Expect(Separator.CloseBracket))
                {
                    param.Add(ParseVariableDescription());
                    if (Expect(Separator.Semiсolon))
                    {
                        currentLex = lexer.GetLexem();
                    }
                }
                Require(Separator.CloseBracket);
            }

            Require(Operator.Colon);
            type = ParseType();
            Require(Separator.Semiсolon);

            locals = ParseDescription();

            Require(KeyWord.BEGIN);
            BlockStmt body = ParseBlock();
            Require(Separator.Semiсolon);

            return new FuncTypesNode(name, param, locals, body, type);
        }
    }
}
