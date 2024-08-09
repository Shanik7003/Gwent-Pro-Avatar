using System;
using System.Globalization;
public class Symbol
{
    public string Name { get; }
    public Type Type { get; }
    public bool IsFunction { get; }
    public List<Type> Parameters { get; }

    public Symbol(string name, Type type, bool isFunction = false, List<Type> parameters = null)
    {
        Name = name;
        Type = type;
        IsFunction = isFunction;
        Parameters = parameters ?? new List<Type>();
    }

    public override string ToString()
    {
        return $"Symbol(Name: {Name}, Type: {Type}, IsFunction: {IsFunction}, Parameters: {string.Join(", ", Parameters)})";
    }
}

public class SymbolTable
{
    private Dictionary<string, Symbol> symbols;
    public SymbolTable Parent { get; private set; }

    public SymbolTable(SymbolTable parent = null)
    {
        symbols = new Dictionary<string, Symbol>();
        Parent = parent;
    }

    public void AddSymbol(string name, Symbol symbol)
    {
        if (!symbols.ContainsKey(name))
        {
            symbols[name] = symbol;
        }
        else
        {
            Console.WriteLine($"Error: El símbolo '{name}' ya está declarado en este alcance.");
        }
    }

    public Symbol? GetSymbol(string name)
    {
        if (symbols.TryGetValue(name, out var symbol))
        {
            return symbol;
        }
        else if (Parent != null)
        {
            return Parent.GetSymbol(name); // Buscar en el alcance padre
        }
        return null;
    }

    public bool ContainsSymbol(string name)
    {
        if (symbols.ContainsKey(name))
        {
            return true;
        }
        else if (Parent != null)
        {
            return Parent.ContainsSymbol(name); // Buscar en el alcance padre
        }
        return false;
    }

    // Método para entrar en un nuevo alcance
    public SymbolTable EnterScope()
    {
        return new SymbolTable(this);
    }

    // Método para salir del alcance actual
    public SymbolTable ExitScope()
    {
        return Parent ?? this; // Volver al padre, si no existe, quedarse en el mismo alcance
    }

    public void PrintActualSymbolTable()
    {
        Console.WriteLine("Símbolos en el alcance actual:");
        foreach (var kvp in symbols)
        {
            Console.WriteLine($"  {kvp.Value}");
        }
        Parent?.PrintActualSymbolTable();
    }

    public void PrintAllScopes()
    {
        Console.WriteLine("Todos los Alcances:");
        var currentScope = this;
        while (currentScope != null)
        {
            foreach (var kvp in currentScope.symbols)
            {
                Console.WriteLine($"  {kvp.Value}");
            }
            Console.WriteLine("---");
            currentScope = currentScope.Parent;
        }
    }
}

public interface IVisitable
{
    void Accept(ASTVisitor visitor);
}

// Clase abstracta que representa el visitante
public abstract class ASTVisitor
{
    // Definir métodos para cada tipo de nodo que quieras visitar

    public abstract void Visit(RootNode node);
    public abstract void Visit(Assignment node);
    public abstract void Visit(MethodCallNode node);
    public abstract void Visit(ExpressionNode node);
    public abstract void Visit(PropertyAccessNode node);
    public abstract void Visit(EffectNode node);
    public abstract void Visit(ParamNode node);
    public abstract void Visit(ActionNode node);
    public abstract void Visit(ForStatement node);
    public abstract void Visit(WhileStatement node);
    public abstract void Visit(BinaryOperation node);
    public abstract void Visit(IdentifierNode node);
    public abstract void Visit(Number node);
    public abstract void Visit(CardNode node);
    public abstract void Visit(EffectInvocationNode node);
    public abstract void Visit(EffectField node);
    public abstract void Visit(CardParam node);
    public abstract void Visit(SelectorNode node);
    public abstract void Visit(MyPredicate node);
    
    // Agrega más métodos para otros tipos de nodos según sea necesario

    // Opcional: Proporcionar una implementación predeterminada
    protected virtual void DefaultVisit(ASTNode node)
    {
        Console.WriteLine($"Visiting node of type {node.GetType().Name}");
    }
}

public class SemanticVisitor : ASTVisitor
{
    private SymbolTable currentSymbolTable;

    public SemanticVisitor()
    {
        currentSymbolTable = new SymbolTable();
    }

    private void ReportError(string message)
    {
        Console.WriteLine($"Semantic Error: {message}");
    }

    private string EvaluateType(ExpressionNode expression)
    {
        switch (expression)
        {
            case Number number:
                return "int"; // o "float" si corresponde
            case IdentifierNode identifierNode:
                var symbol = currentSymbolTable.GetSymbol(identifierNode.Name);
                if (symbol != null)
                {
                    return symbol.Type.ToString();
                }
                else
                {
                    ReportError($"Variable '{identifierNode.Name}' is not declared");
                    return "unknown";
                }
            case BinaryOperation binaryOperation:
                var leftType = EvaluateType(binaryOperation.Left);
                var rightType = EvaluateType(binaryOperation.Right);
                if (leftType == rightType)
                {
                    return leftType;
                }
                else
                {
                    ReportError($"Type mismatch in binary operation: {leftType} and {rightType}");
                    return "unknown";
                }
            case PropertyAccessNode propertyAccess:
                // Evaluar el tipo de la propiedad accedida
                var targetType = EvaluateType(propertyAccess.Target);
                // Asumir que la propiedad es de tipo "int" para este ejemplo
                return "int";

            default:
                ReportError($"Unknown expression type: {expression.GetType().Name}");
                return "unknown";
        }
    }

    private bool TypesAreCompatible(string type1, string type2)
    {
        return type1 == type2; // Placeholder para lógica más compleja
    }

    public override void Visit(RootNode node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();

        foreach (var effect in node.Effects)
        {
            effect.Accept(this);
        }

        foreach (var card in node.Cards)
        {
            card.Accept(this);
        }

        currentSymbolTable = currentSymbolTable.ExitScope();
    }

    public override void Visit(Assignment node)
    {
        var varName = (node.Variable as IdentifierNode)?.Name;
        var exprType = EvaluateType(node.Value);

        if (!currentSymbolTable.ContainsSymbol(varName))
        {
            ReportError($"Variable '{varName}' is not declared");
            return;
        }

        var varSymbol = currentSymbolTable.GetSymbol(varName);
        if (!TypesAreCompatible(varSymbol.Type.ToString(), exprType))
        {
            ReportError($"Type mismatch: cannot assign '{exprType}' to '{varSymbol.Type}'");
        }
    }

    public override void Visit(MethodCallNode node)
    {
        var functionName = node.Funtion.Name;
        if (!currentSymbolTable.ContainsSymbol(functionName))
        {
            ReportError($"Function '{functionName}' is not declared");
            return;
        }

        var functionSymbol = currentSymbolTable.GetSymbol(functionName);
        if (!functionSymbol.IsFunction)
        {
            ReportError($"Symbol '{functionName}' is not a function");
            return;
        }

        // Verificar los parámetros de la función aquí
    }

    public override void Visit(EffectNode node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();

        foreach (var param in node.Params)
        {
            param.Accept(this);
        }

        node.Action.Accept(this);

        currentSymbolTable = currentSymbolTable.ExitScope();
    }

    public override void Visit(ParamNode node)
    {
        var paramName = node.Name;
        var paramType = node.Type;

        if (currentSymbolTable.ContainsSymbol(paramName))
        {
            ReportError($"Parameter '{paramName}' is already declared");
        }
        else
        {
            currentSymbolTable.AddSymbol(paramName, new Symbol(paramName, Type.GetType(paramType)));
        }
    }

    public override void Visit(ActionNode node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();

        node.Targets.Accept(this);
        node.Context.Accept(this);

        foreach (var statement in node.Statements)
        {
            statement.Accept(this);
        }

        currentSymbolTable = currentSymbolTable.ExitScope();
    }

    public override void Visit(ForStatement node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();

        node.Variable.Accept(this);
        node.Iterable.Accept(this);

        foreach (var statement in node.Body)
        {
            statement.Accept(this);
        }

        currentSymbolTable = currentSymbolTable.ExitScope();
    }

    public override void Visit(WhileStatement node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();

        node.Condition.Accept(this);

        foreach (var statement in node.Body)
        {
            statement.Accept(this);
        }

        currentSymbolTable = currentSymbolTable.ExitScope();
    }

    public override void Visit(BinaryOperation node)
    {
        var leftType = EvaluateType(node.Left);
        var rightType = EvaluateType(node.Right);

        if (!TypesAreCompatible(leftType, rightType))
        {
            ReportError($"Type mismatch in binary operation: {leftType} and {rightType}");
        }

        node.Left.Accept(this);
        node.Right.Accept(this);
    }

    public override void Visit(IdentifierNode node)
    {
        if (!currentSymbolTable.ContainsSymbol(node.Name))
        {
            ReportError($"Variable '{node.Name}' is not declared");
        }
    }

    public override void Visit(Number node)
    {
        // Nothing to check for a number literal
    }

    public override void Visit(PropertyAccessNode node)
    {
        node.Target.Accept(this);
        node.Property.Accept(this);
    }

    public override void Visit(CardNode node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();

        foreach (var effect in node.EffectList)
        {
            effect.Accept(this);
        }

        currentSymbolTable = currentSymbolTable.ExitScope();
    }

    public override void Visit(EffectInvocationNode node)
    {
        node.EffectField.Accept(this);
        if (node.Selector != null)
        {
            node.Selector.Accept(this);
        }
        if (node.PostAction != null)
        {
            node.PostAction.Accept(this);
        }
    }

    public override void Visit(EffectField node)
    {
        if (node.Params != null)
        {
            foreach (var param in node.Params)
            {
                param.Accept(this);
            }
        }
    }

    public override void Visit(CardParam node)
    {
        node.Name.Accept(this);
    }

    public override void Visit(SelectorNode node)
    {
        if (node.Predicate != null)
        {
            node.Predicate.Accept(this);
        }
    }

    public override void Visit(MyPredicate node)
    {
        node.Condition.Accept(this);
    }

    public override void Visit(ExpressionNode node)
    {
        throw new NotImplementedException();
    }

    // Agrega más métodos de visita para otros tipos de nodos según sea necesario
}

public enum CardType
{
    UnitCard,
    WhetherCard,
    IncreaseCard,
    Oro,
    LeaderCard
}

public enum Faction
{
    Fire,
    Water,
    Earth,
    NorthernRealms,
    Air
}

public enum Position
{
    M,
    R,
    S
}

public enum Source
{
    board,
    parent
}

public delegate bool MyDelegate(int x);
