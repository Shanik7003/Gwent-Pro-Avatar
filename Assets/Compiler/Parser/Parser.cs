using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;

class Parser
{
    public static TokenList Tokens { get; private set; }
    public static ASTNodeFactory NodeFactory {get; set; }
    private List<CompilingError> parsingErrors;

    public Parser(TokenList tokens, List<CompilingError> errors)
    {
        Tokens = tokens;
        parsingErrors = errors;
        NodeFactory = new(Tokens);
    }

    // Método para agregar errores
    private void AddParsingError(CodeLocation location, string message)
    {
        parsingErrors.Add(new CompilingError(location, ErrorCode.ParsingError, message));
    }

    public RootNode ParseCode()
    {
        RootNode root = new();
        while (!Tokens.End)
        {
            try
            {
                if (Tokens.MatchValue("effect"))
                {
                    EffectNode effect = ParseEffect();
                    if (effect != null)
                    {
                        root.Effects.Add(effect);
                    }
                }
                else if (Tokens.MatchValue("card"))
                {
                    CardNode card = ParseCard();
                    if (card != null)
                    {
                        root.Cards.Add(card);
                    }
                }
                else 
                {
                    Tokens.Next();
                }
            }
            catch (Exception ex)
            {
                AddParsingError(Tokens.LookAhead().Location,ex.Message);
            }

        }
        return root;
    }

    public CardNode? ParseCard()
    {
        try
        {
            Tokens.Expect("{");
            Engine.CardType type = ParseType();
            Tokens.Expect(",");
            IdentifierNode name = ParseNameField();
            Tokens.Expect(",");
            Engine.Faction faction = ParseFaction();
            Tokens.Expect(",");
            int power = ParsePower();
            Tokens.Expect(",");
            CompilerPosition[] range = ParseRange();
            Tokens.Expect(",");
            List<EffectInvocationNode> effects = ParseOnActivationBody();
            Tokens.Expect("}");
            return NodeFactory.CreateCardNode(name, type, faction, power, range, effects);
        }
        catch (Exception ex)
        {
            AddParsingError(Tokens.LookAhead().Location,ex.Message);
            return null; 
        }
    }

    public Engine.CardType ParseType()
    {
        Tokens.Expect("Type");
        Tokens.Expect(":");
        var type = Tokens.Expect(TokenType.Text).Value;
        if (Enum.TryParse(type, out Engine.CardType cardType))
        {
            return cardType;
        }
        else
        {
            AddParsingError(Tokens.LookAhead().Location,  $"Tipo de carta inválido: {type}");
            return default;
        }
    }

    public Engine.Faction ParseFaction()
    {
        Tokens.Expect("Faction");
        Tokens.Expect(":");
        var faction = Tokens.Expect(TokenType.Text).Value;
        if (Enum.TryParse(faction, out Engine.Faction factionType))
        {
            return factionType;
        }
        else
        {
            AddParsingError(Tokens.LookAhead().Location,  $"Facción inválida: {faction}");
            return default;
        }
    }

    public int ParsePower()
    {
        Tokens.Expect("Power");
        Tokens.Expect(":");
        var power = Tokens.Expect(TokenType.Number).Value;
        if (int.TryParse(power, out int result))
        {
            return result;
        }
        else
        {
            AddParsingError(Tokens.LookAhead().Location,  $"Valor de poder inválido: {power}");
            return 0;
        }
    }

    public CompilerPosition[] ParseRange()
    {
        Tokens.Expect("Range");
        Tokens.Expect(":");
        Tokens.Expect("[");
        List<CompilerPosition> range = new List<CompilerPosition>();
        while (Tokens.LookAhead().Value != "]")
        {
            var position = Tokens.Expect(TokenType.Text).Value;
            if (Enum.TryParse(position, out CompilerPosition pos))
            {
                range.Add(pos);
            }
            else
            {
                AddParsingError(Tokens.LookAhead().Location,  $"Posición inválida: {position}");
            }
            if (Tokens.LookAhead().Value == ",")
            {
                Tokens.Next();
            }
            if (Tokens.End)
            {
                AddParsingError(Tokens.LookAhead().Location,  " Expected ']' ");
                break;
            }
        }
        Tokens.Expect("]");
        return range.ToArray();
    }

    public List<EffectInvocationNode> ParseOnActivationBody()
    {
        List<EffectInvocationNode> effects = new List<EffectInvocationNode>();
        Tokens.Expect("OnActivation");
        Tokens.Expect(":");
        Tokens.Expect("[");
        if (Tokens.LookAhead().Value == "{")
        {
            effects.Add(ParseEffectInvocation());
            while (Tokens.LookAhead().Value == ",")
            {
                Tokens.Expect(",");
                effects.Add(ParseEffectInvocation());
            }
            Tokens.Expect("]");
        }
        return effects;
    }

    public EffectInvocationNode ParseEffectInvocation()
    {
        SelectorNode? selector = null;
        EffectInvocationNode? postAction = null;
        Tokens.Expect("{");
        EffectField effectfield = ParseEffectField();
        while (Tokens.LookAhead().Value != "}")
        {
            selector = ParseSelectorField();
            postAction = ParsePostAction();
            if (Tokens.End)
            {
                AddParsingError(Tokens.LookAhead().Location,  " Expected '}' ");
                break;
            }
        }
        Tokens.Expect("}");
        EffectInvocationNode effect = NodeFactory.CreateEffectInvocationNode(effectfield,selector,postAction);
        return effect;
    }

    public EffectField ParseEffectField()
    {
        Tokens.Expect("Effect");
        Tokens.Expect(":");
        if (Tokens.LookAhead().Type == TokenType.Text)
        {
            IdentifierNode Name = NodeFactory.CreateIdentifierNode(Tokens.LookAhead().Value);
            Tokens.Next();
            return NodeFactory.CreateEffectFieldNode(Name);
        }
        Tokens.Expect("{");
        IdentifierNode name = ParseNameField();
        List<CardParam> cardParams = new List<CardParam>();
        while (Tokens.LookAhead().Value != "}")
        {
            Tokens.Expect(",");
            cardParams.Add(ParseCardParam());
            if (Tokens.End)
            {
                AddParsingError(Tokens.LookAhead().Location,  " Expected '}' ");
                break;
            }
        }
        Tokens.Expect("}");
        EffectField effectField = NodeFactory.CreateEffectFieldNode(name,cardParams);
        return effectField;
    }

    public CardParam ParseCardParam()
    {
        IdentifierNode name = NodeFactory.CreateIdentifierNode(Tokens.Expect(TokenType.Identifier).Value);
        Tokens.Expect(":");
        object value = ConvertString(Tokens.LookAhead().Value);
        Tokens.Next();//consume el value
        return NodeFactory.CreateCardParamNode(name, value);
    }
    public static object ConvertString(string input)
    {
        if (int.TryParse(input, out int intValue))
        {
            return intValue; // Devuelve un int si es convertible
        }
        else if (bool.TryParse(input, out bool boolValue))
        {
            return boolValue; // Devuelve un bool si es convertible
        }
        else
        {
            return input; // Devuelve el string original si no es convertible a int ni a bool
        }
    }

    public SelectorNode? ParseSelectorField()
    {
        if (Tokens.LookAhead().Value ==",") Tokens.Next();
        if (Tokens.LookAhead().Value == "Selector")
        {
            Tokens.Expect("Selector");
            Tokens.Expect(":");
            Tokens.Expect("{");
            Source source = ParseSource();
            bool single = ParseSingle();
            MyPredicate predicate  = ParsePredicate();
            Tokens.Expect("}");
            return NodeFactory.CreateSelectorNode(source,single,predicate);
        }
        return null;
    }

    public Source ParseSource()
    {
        Tokens.Expect("Source");
        Tokens.Expect(":");
        var source = Tokens.Expect(TokenType.Text).Value;
        if (Enum.TryParse(source, out Source result))
        {
            return result;
        }
        else
        {
            AddParsingError(Tokens.LookAhead().Location,  $"Fuente inválida: {source}");
            return default;
        }
    }

    public bool ParseSingle()
    {
        if (Tokens.LookAhead().Value == ",") Tokens.Next();
        if (Tokens.LookAhead().Value == "Single")
        {
            Tokens.Expect("Single");
            Tokens.Expect(":");
            string single = Tokens.Expect(TokenType.Keyword).Value;
            if (single == "true" || single == "false") return single=="true";
            else throw new ArgumentException($"Invalid single : {single}");
        }
        else return false;
    }

    public MyPredicate ParsePredicate()
    {
        if (Tokens.LookAhead().Value == ",") Tokens.Next();
        Tokens.Expect("Predicate");
        Tokens.Expect(":");
        Tokens.Expect("(");
        IdentifierNode param = NodeFactory.CreateIdentifierNode(Tokens.Expect(TokenType.Identifier).Value);
        Tokens.Expect(")");
        Tokens.Expect("=>");
        var condition = ParseExpression();
        return NodeFactory.CreateMyPredicateNode(param,condition);
    }

    public EffectInvocationNode? ParsePostAction()
    {
        if (Tokens.LookAhead().Value ==",") Tokens.Next();
        if (Tokens.LookAhead().Value == "PostAction")
        {
            Tokens.Expect("PostAction");
            Tokens.Expect(":");
            EffectInvocationNode postAction = ParseEffectInvocation();
            return postAction;
        }
        return null;
    }

    public EffectNode ParseEffect()
    {
        try
        {
            Tokens.Expect("{");
            var effectBody = ParseEffectBody();
            Tokens.Expect("}");
            return effectBody;
        }
        catch (Exception ex)
        {
            AddParsingError(Tokens.LookAhead().Location,  $"Error al parsear el efecto: {ex.Message}");
            return null;
        }
    }

    public EffectNode ParseEffectBody()
    {
        var name = ParseNameField();
        Tokens.Expect(",");
        List<ParamNode> parameters = new List<ParamNode>();
        if (Tokens.LookAhead().Value == "Params")
        {
            parameters = ParseParamsField();
        }

        var action = ParseActionField();
        return NodeFactory.CreateEffectNode(name, parameters, action);
    }

    public IdentifierNode ParseNameField()
    {
    
        Tokens.Expect("Name");
        Tokens.Expect(":");
        IdentifierNode name = NodeFactory.CreateIdentifierNode(Tokens.Expect(TokenType.Text).Value);
        return name;
    }

    public List<ParamNode> ParseParamsField()
    {
        Tokens.Expect("Params");
        Tokens.Expect(":");
        Tokens.Expect("{");

        var paramList = new List<ParamNode>();
        while (Tokens.LookAhead().Value != TokenValues.ClosedCurlyBraces)
        {
            var param = ParseParam();
            paramList.Add(param);
            if (Tokens.LookAhead().Type == TokenType.Comma)
            {
                Tokens.Next(); // consume ','
            }
            if (Tokens.End)
            {
                AddParsingError(Tokens.LookAhead().Location,  " Expected '}' ");
                break;
            }
        }
        Tokens.Expect("}");
        Tokens.Expect(",");
        return paramList;
    }

    public ParamNode ParseParam()
    {
    
        var name = Tokens.Expect(TokenType.Identifier).Value;
        Tokens.Expect(":");
        var type = Tokens.Expect(TokenType.Type).Value;
        return NodeFactory.CreateParamNode(name, type);
    }

    public ActionNode ParseActionField()
    {
    
        Tokens.Expect("Action");
        Tokens.Expect(":");
        return ParseFunction();
    }

    public ActionNode ParseFunction()
    {
        Tokens.Expect("(");
        IdentifierNode targets = NodeFactory.CreateIdentifierNode(Tokens.Expect(TokenType.Identifier).Value, false,false,false,true);
        Tokens.Expect(",");
        IdentifierNode context = NodeFactory.CreateIdentifierNode(Tokens.Expect(TokenType.Identifier).Value, false, true);
        Tokens.Expect(")");
        Tokens.Expect("=>");
        Tokens.Expect("{");

        var statements = new List<StatementNode>();
        while (Tokens.LookAhead().Value != "}")
        {
            statements.Add(ParseStatement());
            if (Tokens.End)
            {
                AddParsingError(Tokens.LookAhead().Location,  " Expected '}' ");
                break;
            }
        }

        Tokens.Expect("}");
        return new ActionNode(targets, context, statements);
    }

    public StatementNode ParseStatement()
    {
        if (Tokens.LookAhead().Value == "for")
        {
            return ParseForStatement();
        }
        else if (Tokens.LookAhead().Value == "while")
        {
            return ParseWhileStatement();
        }
        else if (Tokens.LookAhead().Type == TokenType.Identifier)
        {
            return ParseAssignment();
        }
        else
        {
            AddParsingError(Tokens.LookAhead().Location,  $"Token inesperado: {Tokens.LookAhead().Value}");
            return null;
        }
    }

    public ForStatement ParseForStatement()
    {
        Tokens.Expect("for");
        var variable = NodeFactory.CreateIdentifierNode(Tokens.Expect(TokenType.Identifier).Value, false,false,true);
        Tokens.Expect("in");
        IdentifierNode iterable = NodeFactory.CreateIdentifierNode(Tokens.Expect(TokenType.Identifier).Value);

        if (Tokens.LookAhead().Value != "{")
        {
            var body = new List<ASTNode> { ParseStatement() };
            return NodeFactory.CreateForStatementNode(variable, iterable, body);
        }
        else
        {
            Tokens.Expect("{");
            var body = new List<ASTNode>();
            while (Tokens.LookAhead().Value != "}")
            {
                body.Add(ParseStatement());
                if (Tokens.End)
                {
                    AddParsingError(Tokens.LookAhead().Location,  " Expected '}' ");
                    break;
                }
            }
            Tokens.Expect("}");
            Tokens.Expect(";");
            return NodeFactory.CreateForStatementNode(variable, iterable, body);
        }
    }

    public WhileStatement ParseWhileStatement()
    {
        Tokens.Expect("while");
        Tokens.Expect("(");
        var condition = ParseExpression();
        Tokens.Expect(")");
        if (Tokens.LookAhead().Value != "{")
        {
            var body = new List<ASTNode> { ParseStatement() };
            return NodeFactory.CreateWhileStatementNode(condition, body);
        }
        else
        {
            Tokens.Expect("{");
            var body = new List<ASTNode>();
            while (Tokens.LookAhead().Type != TokenType.ClosedCurlyBrace)
            {
                body.Add(ParseStatement());
                if (Tokens.End)
                {
                    AddParsingError(Tokens.LookAhead().Location,  " Expected '}' ");
                    break;
                }
            }
            Tokens.Expect("}");
            return NodeFactory.CreateWhileStatementNode(condition, body);
        }
    }

    public AssignmentOrMethodCall ParseAssignment()
    {
        ExpressionNode variable = ParseExpression();
        var op = Tokens.LookAhead().Value;
        if (op == "+=" || op == "-=" || op == "*=" || op == "/=")
        {
            Tokens.Next();
            var value = ParseExpression();
            Tokens.Expect(";");
            return NodeFactory.CreateCompoundAssignmentNode(variable, op, value);
        }
        else if (Tokens.LookAhead().Value == "(")
        {
            return ParseMethodCall(variable as PropertyAccessNode);
        }
        else
        {
            Tokens.Expect("=");
            var value = ParseExpression();
            if (Tokens.LookAhead().Value == "(")
            {
                return NodeFactory.CreateAssignmentNode(variable, ParseExpressionMethodCall(value as PropertyAccessNode));
            }
            else
            {
                Tokens.Expect(";");
                return NodeFactory.CreateAssignmentNode(variable, value);
            }
        }
    }

    public MethodCallNode ParseMethodCall(PropertyAccessNode call)
    {
    
        Tokens.Next();
        if (Tokens.CanLookAhead() && Tokens.LookAhead().Type == TokenType.Identifier)
        {
            var param = NodeFactory.CreateIdentifierNode(Tokens.Expect(TokenType.Identifier).Value);
            Tokens.Expect(")");
            Tokens.Expect(";");
            return NodeFactory.CreateMethodCallNode(call.Property, param, call.Target);
        }
        Tokens.Expect(")");
        Tokens.Expect(";");
        return NodeFactory.CreateMethodCallNode(call.Property, call.Target);
    }

    public ExpressionMethodCall ParseExpressionMethodCall(PropertyAccessNode call)
    {
    
        Tokens.Next();
        if (Tokens.CanLookAhead() && Tokens.LookAhead().Type == TokenType.Identifier)
        {
            var param = NodeFactory.CreateIdentifierNode(Tokens.Expect(TokenType.Identifier).Value);
            Tokens.Expect(")");
            Tokens.Expect(";");
            return NodeFactory.CreateExpressionMethodCallNode(call.Property, param, call.Target);
        }
        Tokens.Expect(")");
        Tokens.Expect(";");
        return NodeFactory.CreateExpressionMethodCallNode(call.Property, call.Target);
    }

    #region ParseEspression

    public ExpressionNode ParseExpression()
    {
        return ParseLogicalTerm();
    }

    public ExpressionNode ParseLogicalTerm()
    {
        var node = ParseLogicalFactor();
        while (Tokens.CanLookAhead() && Tokens.LookAhead().Value == "||")
        {
            var op = Tokens.LookAhead().Value;
            Tokens.Next();
            var right = ParseLogicalFactor();
            node = NodeFactory.CreateBinaryOperationNode(node, op, right,true);
        }
        return node;
    }

    public ExpressionNode ParseLogicalFactor()
    {
        var node = ParseEqualityExpr();
        while (Tokens.CanLookAhead() && Tokens.LookAhead().Value == "&&")
        {
            var op = Tokens.LookAhead().Value;
            Tokens.Next();
            var right = ParseEqualityExpr();
            node = NodeFactory.CreateBinaryOperationNode(node, op, right,true);
        }
        return node;
    }

    public ExpressionNode ParseEqualityExpr()
    {
        var node = ParseRelationalExpr();
        while (Tokens.CanLookAhead() && (Tokens.LookAhead().Value == "==" || Tokens.LookAhead().Value == "!="))
        {
            var op = Tokens.LookAhead().Value;
            Tokens.Next();
            var right = ParseRelationalExpr();
            node = NodeFactory.CreateBinaryOperationNode(node, op, right,true);
        }
        return node;
    }

    public ExpressionNode ParseRelationalExpr()
    {
        var node = ParseAdditiveExpr();
        while (Tokens.CanLookAhead() && (Tokens.LookAhead().Value == "<" || Tokens.LookAhead().Value == ">" || Tokens.LookAhead().Value == "<=" || Tokens.LookAhead().Value == ">="))
        {
            var op = Tokens.LookAhead().Value;
            Tokens.Next();
            var right = ParseAdditiveExpr();
            node = NodeFactory.CreateBinaryOperationNode(node, op, right,true);
        }
        return node;
    }

    public ExpressionNode ParseAdditiveExpr()
    {
        var node = ParseMultiplicativeExpr();
        while (Tokens.CanLookAhead() && (Tokens.LookAhead().Value == "+" || Tokens.LookAhead().Value == "-"))
        {
            var op = Tokens.LookAhead().Value;
            Tokens.Next();
            var right = ParseMultiplicativeExpr();
            node = NodeFactory.CreateBinaryOperationNode(node, op, right,false,true);
        }
        return node;
    }

    public ExpressionNode ParseMultiplicativeExpr()
    {
        var node = ParsePrimary();
        while (Tokens.CanLookAhead() && (Tokens.LookAhead().Value == "*" || Tokens.LookAhead().Value == "/"))
        {
            var op = Tokens.LookAhead().Value;
            Tokens.Next();
            var right = ParsePrimary();
            node = NodeFactory.CreateBinaryOperationNode(node, op, right,false,true);
        }
        return node;
    }

    public ExpressionNode ParsePrimary()
    {
        var token = Tokens.LookAhead();

        if (token.Type == TokenType.Number)
        {
            Tokens.Next(); // Consume el número
            return NodeFactory.CreateNumberNode(double.Parse(token.Value));
        }

        if (token.Type == TokenType.Identifier || token.Type == TokenType.Text)
        {
            ExpressionNode target = NodeFactory.CreateIdentifierNode(Tokens.LookAhead().Value);
            Tokens.Next(); // Consume el identificador o el texto

            while (Tokens.CanLookAhead() && Tokens.LookAhead().Value == ".")
            {
                Tokens.Next(); // Consume '.'
                var property = NodeFactory.CreateIdentifierNode(Tokens.LookAhead().Value);
                Tokens.Next();
                target = NodeFactory.CreatePropertyAccessNode(property, target);
            }
            return target;
        }

        if (token.Value == "(")
        {
            Tokens.Next(); // Consume '('
            var expression = ParseExpression();
            Tokens.Expect(")"); // Espera y consume ')'
            return expression;
        }

        AddParsingError(Tokens.LookAhead().Location,  $"Token inesperado: {token.Value}");
        return null;
    }

    #endregion
}