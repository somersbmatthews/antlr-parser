using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace antlr_parser.Antlr4Impl.Java
{
    public static class AntlrParseJava
    {
        public static IEnumerable<ClassInfo> OuterClassInfosFromJavaSource(string source, string filePath)
        {
            try
            {
                char[] codeArray = source.ToCharArray();
                AntlrInputStream inputStream = new AntlrInputStream(codeArray, codeArray.Length);

                JavaLexer lexer = new JavaLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                JavaParser parser = new JavaParser(commonTokenStream);

                parser.RemoveErrorListeners();
                parser.AddErrorListener(new ErrorListener()); // add ours

                // a compilation unit is the highest level container -> start there
                CompilationUnitListener compilationUnitListener = new CompilationUnitListener(filePath);
                parser.compilationUnit().EnterRule(compilationUnitListener);
                return compilationUnitListener.OuterClassInfos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}