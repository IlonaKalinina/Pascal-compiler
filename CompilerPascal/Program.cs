﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CompilerPascal
{
    class Program
    {
        public static bool eof = false;
         static void Main(string[] args)
         {
             try
             {
                 if (args.Contains("lex"))
                 {
                     var lexer = new Lexer.Lexer(args[0]);
                     //var lexer = new Lexer.Lexer("../../../../CompilerPascal.Test/Tests/LexerTests/Files/1_1.in");

                     eof = false;
                     while (!eof)
                     {
                         var token = lexer.GetLexem();

                         if (token != null)
                         {
                             if (token.categoryLexeme != "error" && token.categoryLexeme != "comments")
                             {
                                 if (token.categoryLexeme == "End File")
                                 {
                                     Console.Write($"{token.categoryLexeme}");
                                     eof = true;
                                 }
                                 else
                                 {
                                     Console.WriteLine($"{token.numberLine} {token.numberSymbol} {token.categoryLexeme} {token.valueLexema} {token.initialLexema}");
                                 }
                             }
                             else if (token.categoryLexeme == "error")
                             {
                                 Console.WriteLine($"{token.valueLexema}");
                             }
                         }
                         else
                         {
                             eof = true;
                         }
                     }
                 }
                 if (args.Contains("pars"))
                 {
                    Parser.Parser P = new Parser.Parser(args[0]);

                    List<string> answer = new List<string>();

                    RunTree(P.doParse);
                }
             }
             catch
             {
                 Console.WriteLine("File not found");
             }
         }

        static List<string> answerList = new List<string>();

        static string lineAnswer = null;
        static string lineBuf = null;

        static int numNode = 0;
        static int numChildren = -1;
        static void RunTree(Parser.Node doParse)
        {
            lineBuf = null;
            for (int i = 0; i < numNode; i++)
            {
                if (i == numNode - 1)
                {
                    if (numChildren == 0)
                    {
                        lineBuf += "├───";
                    }
                    else if (numChildren == 1)
                    {
                        lineBuf += "└───";
                    }
                }
                else
                {
                    lineBuf += "    ";
                }
            }
            lineAnswer = lineBuf + doParse.value;
            Console.WriteLine(lineAnswer);
            answerList.Add(lineAnswer);

            if (doParse.children != null)
            {
                numNode++;
                numChildren = 0;
                RunTree(doParse.children[0]);
                numChildren = -1;

                if (doParse.children.Count > 1)
                {
                    numChildren = 1;
                    RunTree(doParse.children[1]);
                    numChildren = -1;
                }
                numNode--;
            }
            return;
        }
    }
}