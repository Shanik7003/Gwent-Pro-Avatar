using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

public abstract class ASTNode : IVisitable
{
    public CodeLocation Location {get; set;}
    public abstract void PrintMermaid(StringBuilder sb, string parentId);

    public abstract void Accept(ASTVisitor visitor);
}

public abstract class ExpressionNode : ASTNode, IVisitable
{
    public bool IsLogicalExp{ get; set ;}
    public bool IsNumericExp{ get; set; }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class RootNode : ASTNode, IVisitable
{
    public List<EffectNode> Effects { get; }
    public List<CardNode> Cards { get; }

    public RootNode()
    {
        Effects = new List<EffectNode>();
        Cards = new List<CardNode>();
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"Root\"]");
        if (!string.IsNullOrEmpty(parentId))
        {
            sb.AppendLine($"{parentId} --> {nodeId}");
        }

        foreach (var effect in Effects)
        {
            effect.PrintMermaid(sb, nodeId);
        }

        foreach (var card in Cards)
        {
            card.PrintMermaid(sb, nodeId);
        }
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class EffectNode : ASTNode, IVisitable
{
    public IdentifierNode Name { get; }
    public List<ParamNode> Params { get; }
    public ActionNode Action { get; }

    public EffectNode(IdentifierNode name, List<ParamNode> @params, ActionNode action,CodeLocation location)
    {
        Name = name;
        Params = @params;
        Action = action;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"Effect: {Name}\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        foreach (var param in Params)
        {
            param.PrintMermaid(sb, nodeId);
        }

        Action.PrintMermaid(sb, nodeId);
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class ParamNode : ASTNode, IVisitable
{
    public string Name { get; }
    public string Type { get; }

    public ParamNode(string name, string type, CodeLocation location)
    {
        Name = name;
        Type = type;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"Param: {Name} : {Type}\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class ActionNode : ASTNode, IVisitable
{
    public List<StatementNode> Statements { get; }
    public IdentifierNode Targets { get; }
    public IdentifierNode Context { get; }

    public ActionNode(IdentifierNode targets, IdentifierNode context, List<StatementNode> statements)
    {
        Targets = targets;
        Statements = statements;
        Context = context;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"Action\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        Targets.PrintMermaid(sb, nodeId);
        Context.PrintMermaid(sb, nodeId);
        foreach (var statement in Statements)
        {
            statement.PrintMermaid(sb, nodeId);
        }
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public abstract class StatementNode : ASTNode{}

public abstract class AssignmentOrMethodCall : StatementNode{}

public class Assignment : AssignmentOrMethodCall, IVisitable
{
    public ExpressionNode Variable { get; }
    public ExpressionNode? Value { get; }

    public Assignment(ExpressionNode variable, ExpressionNode value,CodeLocation location)
    {
        Variable = variable;
        Value = value;
        Location = location;
    }

    public Assignment(ExpressionNode variable, ExpressionNode value)
    {
        Variable = variable;
        Value = value;
    }

    public Assignment(ExpressionNode variable,CodeLocation location)
    {
        Variable = variable;
        Value = null;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"Assignment\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        Variable.PrintMermaid(sb, nodeId);
        Value?.PrintMermaid(sb, nodeId);
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class MethodCallNode : AssignmentOrMethodCall, IVisitable
{
    public IdentifierNode Funtion { get; }
    public ExpressionNode Target { get; }
    public IdentifierNode? Param { get; }

    public MethodCallNode(IdentifierNode funtion, IdentifierNode param, ExpressionNode target, CodeLocation location)
    {
        Funtion = funtion;
        Param = param;
        Target = target;
        Location = location;
    }

    public MethodCallNode( IdentifierNode funtion, ExpressionNode target,CodeLocation location)
    {
        Funtion = funtion;     
        Param = null;
        Target = target;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"CallFuntionNode: {Funtion.Name}\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        Funtion.PrintMermaid(sb, nodeId);
        Param?.PrintMermaid(sb, nodeId);
        Target.PrintMermaid(sb, nodeId);
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}
public class ExpressionMethodCall : ExpressionNode,IVisitable
{
    public IdentifierNode Funtion { get; }
    public ExpressionNode Target { get; }
    public IdentifierNode? Param { get; }

    public ExpressionMethodCall(IdentifierNode funtion, IdentifierNode param, ExpressionNode target,CodeLocation location)
    {
        Funtion = funtion;
        Param = param;
        Target = target;
        Location = location;
    }

    public ExpressionMethodCall(IdentifierNode funtion, ExpressionNode target,CodeLocation location)
    {
        Funtion = funtion;
        Param = null;
        Target = target;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"CallFuntionNode: {Funtion.Name}\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        Funtion.PrintMermaid(sb, nodeId);
        Param?.PrintMermaid(sb, nodeId);
        Target.PrintMermaid(sb, nodeId);
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class ForStatement : StatementNode, IVisitable
{
    public IdentifierNode Variable { get; }
    public IdentifierNode Iterable { get; }
    public List<ASTNode> Body { get; }

    public ForStatement(IdentifierNode variable, IdentifierNode iterable, List<ASTNode> body,CodeLocation location)
    { 
        Variable = variable;
        Iterable = iterable;
        Body = body;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"For: {Variable.Name} in Iterable\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        Variable.PrintMermaid(sb, nodeId);
        Iterable.PrintMermaid(sb, nodeId);
        foreach (var statement in Body)
        {
            statement.PrintMermaid(sb, nodeId);
        }
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class WhileStatement : StatementNode, IVisitable
{
    public ExpressionNode Condition { get; }
    public List<ASTNode> Body { get; }

    public WhileStatement(ExpressionNode condition, List<ASTNode> body,CodeLocation location)
    {
        Condition = condition;
        Body = body;
        Location = Location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"While\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        Condition.PrintMermaid(sb, nodeId);
        foreach (var statement in Body)
        {
            statement.PrintMermaid(sb, nodeId);
        }
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class Number : ExpressionNode, IVisitable
{
    public double Value { get; }
    public Number(double value, CodeLocation location)
    {
        Value = value;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"Number: {Value}\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class IdentifierNode : ExpressionNode, IVisitable
{
    public string Name { get; }
    public bool IsDynamic { get; set; }
    public bool IsContext { get; set; }
    public bool IsCardList { get; set; }
    public bool IsCard { get; set; }

    public IdentifierNode(string name, CodeLocation location,bool isDynamic = false,bool isContext = false,bool isCard = false, bool isCardList = false)
    {
        Name = name;
        IsDynamic = isDynamic;
        IsContext = isContext;
        IsCard = isCard;
        IsCardList = isCardList;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"Identifier: {Name}\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class PropertyAccessNode : ExpressionNode, IVisitable
{
    public IdentifierNode Property { get; }
    public ExpressionNode Target { get; }

    public PropertyAccessNode(IdentifierNode property, ExpressionNode target,CodeLocation location)
    {
        Property = property;
        Target = target;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        // Obtener un identificador único para el nodo PropertyAccessNode
        string nodeId = $"{GetHashCode()}";
        
        // Añadir el nodo PropertyAccessNode al gráfico
        sb.AppendLine($"{nodeId}[\"PropertyAccessNode\"]");
        
        // Conectar el nodo actual con su nodo padre
        sb.AppendLine($"{parentId} --> {nodeId}");

        // Imprimir el nodo Property
        string propertyId = $"{Property.GetHashCode()}";
        sb.AppendLine($"{propertyId}[\"Property: {Property.Name}\"]");
        sb.AppendLine($"{nodeId} --> {propertyId}");
        Property.PrintMermaid(sb, propertyId);

        // Imprimir el nodo Target
        string targetId = $"{Target.GetHashCode()}";
        sb.AppendLine($"{targetId}[\"Target\"]");
        sb.AppendLine($"{nodeId} --> {targetId}");
        Target.PrintMermaid(sb, targetId);
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class BinaryOperation : ExpressionNode, IVisitable
{
    public ExpressionNode Left { get; }
    public string Operator { get; }
    public ExpressionNode Right { get; }

    public BinaryOperation(ExpressionNode left, string op, ExpressionNode right,CodeLocation location, bool isLogicalExp = false , bool isNumericExp = false)
    {
        Left = left;
        Operator = op;
        Right = right;
        IsLogicalExp = isLogicalExp;
        IsNumericExp = isNumericExp;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"BinaryOperation: {Operator}\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        Left.PrintMermaid(sb, nodeId);
        Right.PrintMermaid(sb, nodeId);
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class CompoundAssignmentNode : Assignment
{
    public string Operator { get; }

    public CompoundAssignmentNode(ExpressionNode variable, string op, ExpressionNode value, CodeLocation location) : base(variable,value)
    {
        Operator = op;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"CompoundAssignment: {Operator}\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        Variable.PrintMermaid(sb, nodeId);
        Value.PrintMermaid(sb, nodeId);
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class CardNode : ASTNode, IVisitable
{
    public IdentifierNode Name { get; }
    public CardType Type { get; }
    public Faction Faction { get; }
    public int Power { get; }
    public Position[] Position { get; }
    public List<EffectInvocationNode> EffectList { get; }

    public CardNode(IdentifierNode name, CardType type, Faction faction, int power, Position[] position, List<EffectInvocationNode> effectList,CodeLocation location)
    {
        Name = name;
        Type = type;
        Faction = faction;
        Power = power;
        Position = position;
        EffectList = effectList;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"Card: {Name}\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        // Crear nodos para las propiedades y conectar con el nodo de la carta
        string typeId = $"{Type.GetHashCode()}";
        sb.AppendLine($"{typeId}[\"Type: {Type}\"]");
        sb.AppendLine($"{nodeId} --> {typeId}");

        string factionId = $"{Faction.GetHashCode()}";
        sb.AppendLine($"{factionId}[\"Faction: {Faction}\"]");
        sb.AppendLine($"{nodeId} --> {factionId}");

        string powerId = $"{Power.GetHashCode()}";
        sb.AppendLine($"{powerId}[\"Power: {Power}\"]");
        sb.AppendLine($"{nodeId} --> {powerId}");

        foreach (var position in Position)
        {
            string positionId = $"{position.GetHashCode()}";
            sb.AppendLine($"{positionId}[\"Position: {position}\"]");
            sb.AppendLine($"{nodeId} --> {positionId}");
        }

        foreach (var effect in EffectList)
        {
            effect.PrintMermaid(sb, nodeId);
        }
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class EffectInvocationNode : ASTNode, IVisitable
{
    public EffectField EffectField { get; }
    public SelectorNode? Selector { get; }
    public EffectInvocationNode? PostAction { get; }

    public EffectInvocationNode(EffectField effectField, SelectorNode? selector, EffectInvocationNode? postAction,CodeLocation location)
    {
        EffectField = effectField;
        Selector = selector;
        PostAction = postAction;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"EffectInvocation\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        EffectField.PrintMermaid(sb, nodeId);
        Selector?.PrintMermaid(sb, nodeId);
        PostAction?.PrintMermaid(sb, nodeId);
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class EffectField : ASTNode, IVisitable
{
    public IdentifierNode Name { get; }
    public List<CardParam>? Params { get; }

    public EffectField(IdentifierNode name,CodeLocation location)
    {
        Name = name;
        Params = null;
        Location = location;
    }

    public EffectField(IdentifierNode name, List<CardParam>? param, CodeLocation location)
    {
        Name = name;
        Params = param;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"EffectField: {Name}\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        if (Params == null) return;

        foreach (var param in Params)
        {
            param.PrintMermaid(sb, nodeId);
        }
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class CardParam : ASTNode, IVisitable
{
    public IdentifierNode Name { get; }
    public object Value { get; }

    public CardParam(IdentifierNode name, object value,CodeLocation location)
    {
        Name = name;
        Value = value;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"CardParam\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        Name.PrintMermaid(sb, nodeId);
        string valueId = $"{Value.GetHashCode()}";
        sb.AppendLine($"{valueId}[\"Value: {Value}\"]");
        sb.AppendLine($"{nodeId} --> {valueId}");
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class SelectorNode : ASTNode, IVisitable
{
    public Source Source { get; }
    public bool Single { get; }
    public MyPredicate? Predicate { get; }

    public SelectorNode(Source source, bool single, MyPredicate predicate,CodeLocation location)
    {
        Source = source;
        Single = single;
        Predicate = predicate;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"Selector\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        string sourceId = $"{Source.GetHashCode()}";
        sb.AppendLine($"{sourceId}[\"Source: {Source}\"]");
        sb.AppendLine($"{nodeId} --> {sourceId}");

        string singleId = $"{Single.GetHashCode()}";
        sb.AppendLine($"{singleId}[\"Single: {Single}\"]");
        sb.AppendLine($"{nodeId} --> {singleId}");

        Predicate?.PrintMermaid(sb, nodeId);
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class MyPredicate : ASTNode, IVisitable
{
    public IdentifierNode Param { get; }
    public ExpressionNode Condition { get; }

    public MyPredicate(IdentifierNode param, ExpressionNode condition,CodeLocation location)
    {
        Param = param;
        Param.IsCard = true;
        Condition = condition;
        Location = location;
    }

    public override void PrintMermaid(StringBuilder sb, string parentId)
    {
        string nodeId = $"{GetHashCode()}";
        sb.AppendLine($"{nodeId}[\"Predicate\"]");
        sb.AppendLine($"{parentId} --> {nodeId}");

        string paramId = $"{Param.GetHashCode()}";
        sb.AppendLine($"{paramId}[\"Param: {Param}\"]");
        sb.AppendLine($"{nodeId} --> {paramId}");


        Condition.PrintMermaid(sb, nodeId);
    }

    public override void Accept(ASTVisitor visitor)
    {
        visitor.Visit(this);
    }
}
