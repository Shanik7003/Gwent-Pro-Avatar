using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;

class Parser
{
    public TokenList Tokens { get; private set; }
    private List<CompilingError> parsingErrors;

    public Parser(TokenList tokens, List<CompilingError> errors)
    {
        Tokens = tokens;
        parsingErrors = errors;
    }

    // Método para agregar errores
    private void AddParsingError(CodeLocation location, ErrorCode code, string message)
    {
        parsingErrors.Add(new CompilingError(location, code, message));
    }

    public RootNode ParseCode()
    {
        RootNode root = new RootNode();
        while (!Tokens.End)
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
                AddParsingError(Tokens.LookAhead().Location, ErrorCode.SyntaxError, "Se esperaba 'effect' o 'card'.");
                break;
            }
        }
        return root;
    }

    public CardNode ParseCard()
    {
        Tokens.Expect("{");
        CardType type = ParseType();
        Tokens.Expect(",");
        string name = ParseNameField();
        Tokens.Expect(",");
        Faction faction = ParseFaction();
        Tokens.Expect(",");
        int power = ParsePower();
        Tokens.Expect(",");
        Position[] range = ParseRange();
        Tokens.Expect(",");
        List<EffectInvocationNode> effects = ParseOnActivationBody();
        Tokens.Expect("}");
        return new CardNode(name, type, faction, power, range, effects);
    }

    public CardType ParseType()
    {
        Tokens.Expect("Type");
        Tokens.Expect(":");
        var type = Tokens.Expect(TokenType.Text).Value;
        if (Enum.TryParse(type, out CardType cardType))
        {
            return cardType;
        }
        else
        {
            AddParsingError(Tokens.LookAhead().Location, ErrorCode.Invalid, $"Tipo de carta inválido: {type}");
            return default;
        }
    }

    public Faction ParseFaction()
    {
        Tokens.Expect("Faction");
        Tokens.Expect(":");
        var faction = Tokens.Expect(TokenType.Text).Value;
        if (Enum.TryParse(faction, out Faction factionType))
        {
            return factionType;
        }
        else
        {
            AddParsingError(Tokens.LookAhead().Location, ErrorCode.Invalid, $"Facción inválida: {faction}");
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
            AddParsingError(Tokens.LookAhead().Location, ErrorCode.Invalid, $"Valor de poder inválido: {power}");
            return 0;
        }
    }

    public Position[] ParseRange()
    {
        Tokens.Expect("Range");
        Tokens.Expect(":");
        Tokens.Expect("[");
        List<Position> range = new List<Position>();
        while (Tokens.LookAhead().Value != "]")
        {
            var position = Tokens.Expect(TokenType.Text).Value;
            if (Enum.TryParse(position, out Position pos))
            {
                range.Add(pos);
            }
            else
            {
                AddParsingError(Tokens.LookAhead().Location, ErrorCode.Invalid, $"Posición inválida: {position}");
            }
            if (Tokens.LookAhead().Value == ",")
            {
                Tokens.Next();
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
        }
        Tokens.Expect("}");
        EffectInvocationNode effect = new EffectInvocationNode(effectfield,selector,postAction);
        return effect;
    }

    public EffectField ParseEffectField()
    {
        Tokens.Expect("Effect");
        Tokens.Expect(":");
        if (Tokens.LookAhead().Type == TokenType.Text)
        {
            string Name = Tokens.LookAhead().Value;
            Tokens.Next();
            return new EffectField(Name);
        }
        Tokens.Expect("{");
        string name = ParseNameField();
        List<CardParam> cardParams = [];
        while (Tokens.LookAhead().Value != "}")
        {
            Tokens.Expect(",");
            cardParams.Add(ParseCardParam());
        }
        Tokens.Expect("}");
        EffectField effectField = new EffectField(name,cardParams);
        return effectField;
    }

    public CardParam ParseCardParam()
    {
        string name = Tokens.Expect(TokenType.Identifier).Value;
        Tokens.Expect(":");
        string value = Tokens.LookAhead().Value;
        Tokens.Next();//consume el value
        return new CardParam(name, value);
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
            return new SelectorNode(source,single,predicate);
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
            AddParsingError(Tokens.LookAhead().Location, ErrorCode.Invalid, $"Fuente inválida: {source}");
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
        string param = Tokens.Expect(TokenType.Identifier).Value;
        Tokens.Expect(")");
        Tokens.Expect("=>");
        var condition = ParseExpression();
        return new MyPredicate(param,condition);
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
            AddParsingError(Tokens.LookAhead().Location, ErrorCode.SyntaxError, $"Error al parsear el efecto: {ex.Message}");
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
        return new EffectNode(name, parameters, action);
    }

    public string ParseNameField()
    {
        Tokens.Expect("Name");
        Tokens.Expect(":");
        var name = Tokens.Expect(TokenType.Text).Value;
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
        return new ParamNode(name, type);
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

        IdentifierNode targets = new(Tokens.Expect(TokenType.Identifier).Value, true);
        Tokens.Expect(",");
        IdentifierNode context = new(Tokens.Expect(TokenType.Identifier).Value, true, true);
        Tokens.Expect(")");
        Tokens.Expect("=>");
        Tokens.Expect("{");

        var statements = new List<StatementNode>();
        while (Tokens.LookAhead().Value != "}")
        {
            statements.Add(ParseStatement());
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
            AddParsingError(Tokens.LookAhead().Location, ErrorCode.SyntaxError, $"Token inesperado: {Tokens.LookAhead().Value}");
            return null;
        }
    }

    public ForStatement ParseForStatement()
    {
        Tokens.Expect("for");
        var variable = new IdentifierNode(Tokens.Expect(TokenType.Identifier).Value, true);
        Tokens.Expect("in");
        IdentifierNode iterable = new IdentifierNode(Tokens.Expect(TokenType.Identifier).Value);

        if (Tokens.LookAhead().Value != "{")
        {
            var body = new List<ASTNode> { ParseStatement() };
            return new ForStatement(variable, iterable, body);
        }
        else
        {
            Tokens.Expect("{");
            var body = new List<ASTNode>();
            while (Tokens.LookAhead().Value != "}")
            {
                body.Add(ParseStatement());
            }
            Tokens.Expect("}");
            Tokens.Expect(";");
            return new ForStatement(variable, iterable, body);
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
            return new WhileStatement(condition, body);
        }
        else
        {
            Tokens.Expect("{");
            var body = new List<ASTNode>();
            while (Tokens.LookAhead().Type != TokenType.ClosedCurlyBrace)
            {
                body.Add(ParseStatement());
            }
            Tokens.Expect("}");
            return new WhileStatement(condition, body);
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
            return new CompoundAssignmentNode(variable, op, value);
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
                return new Assignment(variable, ParseExpressionMethodCall(value as PropertyAccessNode));
            }
            else
            {
                Tokens.Expect(";");
                return new Assignment(variable, value);
            }
        }
    }

    public MethodCallNode ParseMethodCall(PropertyAccessNode call)
    {
        Tokens.Next();
        if (Tokens.CanLookAhead() && Tokens.LookAhead().Type == TokenType.Identifier)
        {
            var param = Tokens.Expect(TokenType.Identifier);
            Tokens.Expect(")");
            Tokens.Expect(";");
            return new MethodCallNode(call.Property.Name, param.Value, call.Target);
        }
        Tokens.Expect(")");
        Tokens.Expect(";");
        return new MethodCallNode(call.Property.Name, call.Target);
    }

    public ExpressionMethodCall ParseExpressionMethodCall(PropertyAccessNode call)
    {
        Tokens.Next();
        if (Tokens.CanLookAhead() && Tokens.LookAhead().Type == TokenType.Identifier)
        {
            var param = Tokens.Expect(TokenType.Identifier);
            Tokens.Expect(")");
            Tokens.Expect(";");
            return new ExpressionMethodCall(call.Property.Name, param.Value, call.Target);
        }
        Tokens.Expect(")");
        Tokens.Expect(";");
        return new ExpressionMethodCall(call.Property.Name, call.Target);
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
            node = new BinaryOperation(node, op, right);
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
            node = new BinaryOperation(node, op, right);
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
            node = new BinaryOperation(node, op, right);
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
            node = new BinaryOperation(node, op, right);
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
            node = new BinaryOperation(node, op, right);
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
            node = new BinaryOperation(node, op, right);
        }
        return node;
    }

    public ExpressionNode ParsePrimary()
    {
        var token = Tokens.LookAhead();

        if (token.Type == TokenType.Number)
        {
            Tokens.Next(); // Consume el número
            return new Number(double.Parse(token.Value));
        }

        if (token.Type == TokenType.Identifier || token.Type == TokenType.Text)
        {
            ExpressionNode target = new IdentifierNode(Tokens.LookAhead().Value);
            Tokens.Next(); // Consume el identificador o el texto

            while (Tokens.CanLookAhead() && Tokens.LookAhead().Value == ".")
            {
                Tokens.Next(); // Consume '.'
                string property = Tokens.LookAhead().Value;
                Tokens.Next();
                target = new PropertyAccessNode(property, target);
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

        AddParsingError(Tokens.LookAhead().Location, ErrorCode.SyntaxError, $"Token inesperado: {token.Value}");
        return null;
    }

    #endregion
}
