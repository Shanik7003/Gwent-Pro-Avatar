using System.Collections.Generic;
using System.Linq.Expressions;

public class ASTNodeFactory
{
    private readonly TokenList _tokens;

    public ASTNodeFactory(TokenList tokens)
    {
        _tokens = tokens;
    }

    public IdentifierNode CreateIdentifierNode(string name,bool isDynamic = false,bool isContext = false,bool isCard = false, bool isCardList = false)
    {
        return new IdentifierNode(name,  _tokens.LookAhead().Location,isDynamic ,isContext,isCard,  isCardList);
    }

    public Number CreateNumberNode(double value)
    {
        return new Number(value, _tokens.LookAhead().Location);
    }
    public Text CreateTextNode(string value)
    {
        return new Text(value, _tokens.LookAhead().Location);
    }

    public PropertyAccessNode CreatePropertyAccessNode(IdentifierNode property, ExpressionNode target)
    {
        return new PropertyAccessNode(property, target, _tokens.LookAhead().Location);
    }

    public BinaryOperation CreateBinaryOperationNode(ExpressionNode left, string op, ExpressionNode right, bool isLogicalExp = false, bool isNumericExp = false,bool isConcatenation = false)
    {
        return new BinaryOperation(left, op, right,  _tokens.LookAhead().Location ,isLogicalExp, isNumericExp,isConcatenation);
    }

    public Assignment CreateAssignmentNode(ExpressionNode variable, ExpressionNode value)
    {
        return new Assignment(variable, value, _tokens.LookAhead().Location);
    }

    public Assignment CreateAssignmentNode(ExpressionNode variable)
    {
        return new Assignment(variable, _tokens.LookAhead().Location);
    }

    public MethodCallNode CreateMethodCallNode(IdentifierNode function, ExpressionNode target)
    {
        return new MethodCallNode(function, target, _tokens.LookAhead().Location);
    }

    public MethodCallNode CreateMethodCallNode(IdentifierNode functionName, object paramName, ExpressionNode target)
    {
        return new MethodCallNode(functionName, paramName, target, _tokens.LookAhead().Location);
    }

    public ExpressionMethodCall CreateExpressionMethodCallNode(IdentifierNode functionName, ExpressionNode target)
    {
        return new ExpressionMethodCall(functionName, target, _tokens.LookAhead().Location);
    }

    public ExpressionMethodCall CreateExpressionMethodCallNode(IdentifierNode functionName, object paramName, ExpressionNode target)
    {
        return new ExpressionMethodCall(functionName, paramName, target, _tokens.LookAhead().Location);
    }

    public ForStatement CreateForStatementNode(IdentifierNode variable, IdentifierNode iterable, List<ASTNode> body)
    {
        return new ForStatement(variable, iterable, body, _tokens.LookAhead().Location);
    }

    public WhileStatement CreateWhileStatementNode(ExpressionNode condition, List<ASTNode> body)
    {
        return new WhileStatement(condition, body, _tokens.LookAhead().Location);
    }

    public EffectNode CreateEffectNode(ExpressionNode name, List<ParamNode> parameters, ActionNode action)
    {
        return new EffectNode(name, parameters, action, name.Location);
    }

    public ParamNode CreateParamNode(string name, string type)
    {
        return new ParamNode(name, type, _tokens.LookAhead().Location);
    }

    public CardNode CreateCardNode(ExpressionNode name, ExpressionNode type, ExpressionNode faction, ExpressionNode power, CompilerPosition[] positions, List<EffectInvocationNode> effects)
    {
        return new CardNode(name, type, faction, power, positions, effects, name.Location);
    }

    public EffectInvocationNode CreateEffectInvocationNode(EffectField effectField, SelectorNode? selector, EffectInvocationNode? postAction)
    {
        return new EffectInvocationNode(effectField, selector, postAction, _tokens.LookAhead().Location);
    }

    public EffectField CreateEffectFieldNode(ExpressionNode name, List<CardParam> parameters)
    {
        return new EffectField(name, parameters, _tokens.LookAhead().Location);
    }
    public EffectField CreateEffectFieldNode(IdentifierNode name)
    {
        return new EffectField(name, _tokens.LookAhead().Location);
    }

    public CardParam CreateCardParamNode(IdentifierNode name, ExpressionNode value)
    {
        return new CardParam(name, value, _tokens.LookAhead().Location);
    }

    public SelectorNode CreateSelectorNode(Source source, bool single, MyPredicate predicate)
    {
        return new SelectorNode(source, single, predicate, _tokens.LookAhead().Location);
    }

    public MyPredicate CreateMyPredicateNode(IdentifierNode param, ExpressionNode condition)
    {
        return new MyPredicate(param, condition, _tokens.LookAhead().Location);
    }
    public CompoundAssignmentNode CreateCompoundAssignmentNode(ExpressionNode variable, string op, ExpressionNode value)
    {
        return new CompoundAssignmentNode(variable,op,value,_tokens.LookAhead().Location);
    }
}
