using System;
using System.Collections.Generic;
using System.Text;

public abstract class ASTNode : IVisitable
{
    public abstract void PrintMermaid(StringBuilder sb, string parentId);

    public abstract void Accept(ASTVisitor visitor);

    public List<ASTNode> Children { get; set; }
}

public abstract class ExpressionNode : ASTNode, IVisitable
{
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
        Children = new List<ASTNode>();
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
    public string Name { get; }
    public List<ParamNode> Params { get; }
    public ActionNode Action { get; }

    public EffectNode(string name, List<ParamNode> @params, ActionNode action)
    {
        Name = name;
        Params = @params;
        Action = action;
        Children = new List<ASTNode>();
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

    public ParamNode(string name, string type)
    {
        Name = name;
        Type = type;
        Children = new List<ASTNode>();
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

    public ActionNode(string targets, string context, List<StatementNode> statements)
    {
        Targets = new IdentifierNode(targets);
        Statements = statements;
        Context = new IdentifierNode(context);
        Children = new List<ASTNode>();
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

public abstract class StatementNode : ASTNode
{
}

public abstract class AssignmentOrMethodCall : StatementNode
{
}

public class Assignment : AssignmentOrMethodCall, IVisitable
{
    public ExpressionNode Variable { get; }
    public ExpressionNode? Value { get; }

    public Assignment(ExpressionNode variable, ExpressionNode value)
    {
        Variable = variable;
        Value = value;
        Children = new List<ASTNode>();
    }

    public Assignment(ExpressionNode variable)
    {
        Variable = variable;
        Value = null;
        Children = new List<ASTNode>();
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

    public MethodCallNode(string funtion, string param, ExpressionNode target)
    {
        Funtion = new IdentifierNode(funtion);
        Param = new IdentifierNode(param);
        Target = target;
        Children = new List<ASTNode>();
    }

    public MethodCallNode(string funtion, ExpressionNode target)
    {
        Funtion = new IdentifierNode(funtion);
        Param = null;
        Target = target;
        Children = new List<ASTNode>();
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
    public ExpressionNode Iterable { get; }
    public List<ASTNode> Body { get; }

    public ForStatement(IdentifierNode variable, ExpressionNode iterable, List<ASTNode> body)
    {
        Variable = variable;
        Iterable = iterable;
        Body = body;
        Children = new List<ASTNode>();
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

    public WhileStatement(ExpressionNode condition, List<ASTNode> body)
    {
        Condition = condition;
        Body = body;
        Children = new List<ASTNode>();
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
    public Number(double value)
    {
        Value = value;
        Children = new List<ASTNode>();
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
    public IdentifierNode(string name)
    {
        Name = name;
        Children = new List<ASTNode>();
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

    public PropertyAccessNode(string property, ExpressionNode target)
    {
        Property = new IdentifierNode(property);
        Target = target;
        Children = new List<ASTNode>();
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

    public BinaryOperation(ExpressionNode left, string op, ExpressionNode right)
    {
        Left = left;
        Operator = op;
        Right = right;
        Children = new List<ASTNode>();
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

    public CompoundAssignmentNode(ExpressionNode variable, string op, ExpressionNode value) : base(variable, value)
    {
        Operator = op;
        Children = new List<ASTNode>();
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
    public string Name { get; }
    public CardType Type { get; }
    public Faction Faction { get; }
    public int Power { get; }
    public Position[] Position { get; }
    public List<EffectInvocationNode> EffectList { get; }

    public CardNode(string name, CardType type, Faction faction, int power, Position[] position, List<EffectInvocationNode> effectList)
    {
        Name = name;
        Type = type;
        Faction = faction;
        Power = power;
        Position = position;
        EffectList = effectList;
        Children = new List<ASTNode>();
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

    public EffectInvocationNode(EffectField effectField, SelectorNode? selector, EffectInvocationNode? postAction)
    {
        EffectField = effectField;
        Selector = selector;
        PostAction = postAction;
        Children = new List<ASTNode>();
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
    public string Name { get; }
    public List<CardParam>? Params { get; }

    public EffectField(string name)
    {
        Name = name;
        Params = null;
        Children = new List<ASTNode>();
    }

    public EffectField(string name, List<CardParam>? param)
    {
        Name = name;
        Params = param;
        Children = new List<ASTNode>();
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
    public string Value { get; }

    public CardParam(string name, string value)
    {
        Name = new IdentifierNode(name);
        Value = value;
        Children = new List<ASTNode>();
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

    public SelectorNode(Source source, bool single, MyPredicate predicate)
    {
        Source = source;
        Single = single;
        Predicate = predicate;
        Children = new List<ASTNode>();
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
    public string Param { get; }
    public ExpressionNode Condition { get; }

    public MyPredicate(string param, ExpressionNode condition)
    {
        Param = param;
        Condition = condition;
        Children = new List<ASTNode>();
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
