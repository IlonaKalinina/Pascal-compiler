using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompilerPascal.Test
{
    [TestClass]
    public class CompilerTester
    {
        public const string TestsDirectory = @"..\..\..\Tests";

        public static IEnumerable<object[]> LexerTestMethodInput
        {
            get
            {
                var res = new List<object[]>();
                var directories = new DirectoryInfo(TestsDirectory).GetDirectories();
                foreach (var directory in directories)
                {
                    if (directory.Name == "LexerTests")
                    {
                        var key = "";
                        try
                        {
                            key = new StreamReader(directory.FullName + "\\.key").ReadToEnd();
                        }
                        catch (FileNotFoundException e)
                        {
                            Console.WriteLine(e);
                            continue;
                        }

                        var directoryInfo = new DirectoryInfo(directory.FullName + "\\Files");
                        var files = directoryInfo.GetFiles("*.in");
                        res.AddRange(files.Select(file => new object[]
                            {file.Directory + "\\" + Path.GetFileNameWithoutExtension(file.Name), key}));
                    }
                }
                return res;
            }
        }
        public static IEnumerable<object[]> SimpleParsTestMethodInput
        {
            get
            {
                var res = new List<object[]>();
                var directories = new DirectoryInfo(TestsDirectory).GetDirectories();
                foreach (var directory in directories)
                {
                    if (directory.Name == "SimpleParsTests")
                    {
                        var key = "";
                        try
                        {
                            key = new StreamReader(directory.FullName + "\\.key").ReadToEnd();
                        }
                        catch (FileNotFoundException e)
                        {
                            Console.WriteLine(e);
                            continue;
                        }

                        var directoryInfo = new DirectoryInfo(directory.FullName + "\\Files");
                        var files = directoryInfo.GetFiles("*.in");
                        res.AddRange(files.Select(file => new object[]
                            {file.Directory + "\\" + Path.GetFileNameWithoutExtension(file.Name), key}));
                    }
                }
                return res;
            }
        }
        public static IEnumerable<object[]> ParserTestMethodInput
        {
            get
            {
                var res = new List<object[]>();
                var directories = new DirectoryInfo(TestsDirectory).GetDirectories();
                foreach (var directory in directories)
                {
                    if (directory.Name == "ParserTests")
                    {
                        var key = "";
                        try
                        {
                            key = new StreamReader(directory.FullName + "\\.key").ReadToEnd();
                        }
                        catch (FileNotFoundException e)
                        {
                            Console.WriteLine(e);
                            continue;
                        }
                        var directoryInfo = new DirectoryInfo(directory.FullName + "\\Files");
                        var files = directoryInfo.GetFiles("*.in");
                        res.AddRange(files.Select(file => new object[]
                            {file.Directory + "\\" + Path.GetFileNameWithoutExtension(file.Name), key}));
                    }
                }
                return res;
            }
        }
        public static IEnumerable<object[]> SemanticAnalysisTestMethodInput
        {
            get
            {
                var res = new List<object[]>();
                var directories = new DirectoryInfo(TestsDirectory).GetDirectories();
                foreach (var directory in directories)
                {
                    if (directory.Name == "SemanticAnalysisTests")
                    {
                        var key = "";
                        try
                        {
                            key = new StreamReader(directory.FullName + "\\.key").ReadToEnd();
                        }
                        catch (FileNotFoundException e)
                        {
                            Console.WriteLine(e);
                            continue;
                        }

                        var directoryInfo = new DirectoryInfo(directory.FullName + "\\Files");
                        var files = directoryInfo.GetFiles("*.in");
                        res.AddRange(files.Select(file => new object[]
                            {file.Directory + "\\" + Path.GetFileNameWithoutExtension(file.Name), key}));
                    }
                }
                return res;
            }
        }


        [TestMethod]
        [DynamicData(nameof(LexerTestMethodInput))]
        public void LexerTests(string fileName, string compileKeys)
        {
            var outputs = GetOutput.RunAndGetOutputs(fileName, compileKeys);
            Assert.AreEqual(outputs.Item2, outputs.Item1);
        }
        [TestMethod]
        [DynamicData(nameof(SimpleParsTestMethodInput))]
        public void SimpleParsTests(string fileName, string compileKeys)
        {
            var outputs = GetOutput.RunAndGetOutputs(fileName, compileKeys);
            Assert.AreEqual(outputs.Item2, outputs.Item1);
        }
        [TestMethod]
        [DynamicData(nameof(ParserTestMethodInput))]
        public void ParserTests(string fileName, string compileKeys)
        {
            var outputs = GetOutput.RunAndGetOutputs(fileName, compileKeys);
            Assert.AreEqual(outputs.Item2, outputs.Item1);
        }
        [TestMethod]
        [DynamicData(nameof(SemanticAnalysisTestMethodInput))]
        public void SemanticAnalysisTests(string fileName, string compileKeys)
        {
            var outputs = GetOutput.RunAndGetOutputs(fileName, compileKeys);
            Assert.AreEqual(outputs.Item2, outputs.Item1);
        }
    }
}