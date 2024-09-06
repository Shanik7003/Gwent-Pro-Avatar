/* Lexical analysis. Allows to split a raw Text representing the program into 
   the first abstract elements (tokens). */
using System;
using System.Collections.Generic;

public class Compiling
{
    private static LexicalAnalyzer? __LexicalProcess;
    public static LexicalAnalyzer Lexical
    {
        get
        {
            if (__LexicalProcess == null)
            {
                __LexicalProcess = new LexicalAnalyzer();

                // Registrar operadores según el nuevo DSL
                __LexicalProcess.RegisterOperator(".",  TokenValues.Dot);
                __LexicalProcess.RegisterOperator("{",  TokenValues.OpenCurlyBraces);
                __LexicalProcess.RegisterOperator("}",  TokenValues.ClosedCurlyBraces);
                __LexicalProcess.RegisterOperator("(",  TokenValues.OpenBracket);
                __LexicalProcess.RegisterOperator(")",  TokenValues.ClosedBracket);
                __LexicalProcess.RegisterOperator(":",  TokenValues.Colon);
                __LexicalProcess.RegisterOperator(";",  TokenValues.StatementSeparator);
                __LexicalProcess.RegisterOperator(",",  TokenValues.ValueSeparator);
                __LexicalProcess.RegisterOperator("=",  TokenValues.Assign);
                __LexicalProcess.RegisterOperator("+",  TokenValues.Add); 
                __LexicalProcess.RegisterOperator("-",  TokenValues.Sub); 
                __LexicalProcess.RegisterOperator("*",  TokenValues.Mul); 
                __LexicalProcess.RegisterOperator("/",  TokenValues.Div); 
                __LexicalProcess.RegisterOperator("==", TokenValues.LogicEqual);
                __LexicalProcess.RegisterOperator("+=", TokenValues.AddAssign);
                __LexicalProcess.RegisterOperator("-=", TokenValues.SubstAssign);
                __LexicalProcess.RegisterOperator("*=", TokenValues.MultAssign);
                __LexicalProcess.RegisterOperator("/=", TokenValues.DivideAssign);
                __LexicalProcess.RegisterOperator("++", TokenValues.Increment);
                __LexicalProcess.RegisterOperator("--", TokenValues.DeIncrement);
                __LexicalProcess.RegisterOperator("=>", TokenValues.Arrow);
                __LexicalProcess.RegisterOperator(">", TokenValues.GreaterThan);
                __LexicalProcess.RegisterOperator("<", TokenValues.LessThan);
                __LexicalProcess.RegisterOperator("<=", TokenValues.LessEqualThan);
                __LexicalProcess.RegisterOperator(">=", TokenValues.GreaterEqualThan);
                __LexicalProcess.RegisterOperator("[", TokenValues.OpenSquareBraket);
                __LexicalProcess.RegisterOperator("]", TokenValues.ClosedSquareBraket);
                __LexicalProcess.RegisterOperator("@", TokenValues.Concatenation);
                __LexicalProcess.RegisterOperator("@@", TokenValues.ConcatenationWithSpace);

                // Registrar palabras clave según el nuevo DSL

                __LexicalProcess.RegisterKeyword("effect", TokenValues.effect);                
                __LexicalProcess.RegisterKeyword("card", TokenValues.card);
                __LexicalProcess.RegisterKeyword("false", TokenValues.False);
                __LexicalProcess.RegisterKeyword("true", TokenValues.True);
                __LexicalProcess.RegisterKeyword("for", TokenValues.For);  
                __LexicalProcess.RegisterKeyword("while", TokenValues.While);
                __LexicalProcess.RegisterKeyword("Name", TokenValues.Name);
                __LexicalProcess.RegisterKeyword("Params", TokenValues.Params);
                __LexicalProcess.RegisterKeyword("Action", TokenValues.Action);
                __LexicalProcess.RegisterKeyword("Type", TokenValues.Type);
                __LexicalProcess.RegisterKeyword("Faction", TokenValues.Faction);
                __LexicalProcess.RegisterKeyword("Range", TokenValues.Range);
                __LexicalProcess.RegisterKeyword("OnActivation", TokenValues.OnActivation);
                __LexicalProcess.RegisterKeyword("Selector", TokenValues.Selector);
                __LexicalProcess.RegisterKeyword("Source", TokenValues.Source);
                __LexicalProcess.RegisterKeyword("Single", TokenValues.Single);
                __LexicalProcess.RegisterKeyword("Predicate", TokenValues.Predicate);
                __LexicalProcess.RegisterKeyword("PostAction", TokenValues.PostAction);

                //types
                __LexicalProcess.RegisterType("String", TokenValues.stringType);
                __LexicalProcess.RegisterType("Bool", TokenValues.boolType);
                __LexicalProcess.RegisterType("Number", TokenValues.intType);

                // Registrar delimitadores de texto
                __LexicalProcess.RegisterText("\"", "\"");
            }

            return __LexicalProcess;
        }
    }
}

