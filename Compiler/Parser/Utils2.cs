using System;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http.Headers;
public class Symbol
{
    public string Name { get; }
    public Type Type { get; set;}
    public bool IsFunction { get; }
    public List<Type> Parameters { get; }
    public Dictionary<string, Symbol> Members { get; } 

    public Symbol(string name, Type type, bool isFunction = false, List<Type> parameters = null)
    {
        Name = name;
        Type = type;
        IsFunction = isFunction;
        Parameters = parameters ?? new List<Type>();
        Members = new Dictionary<string, Symbol>(); 
    }
    public void AddMember(string memberName, Symbol memberSymbol)
    {
        if (!Members.ContainsKey(memberName))
        {
            Members[memberName] = memberSymbol;
        }
        else
        {
            Console.WriteLine($"Error: El miembro '{memberName}' ya está declarado en el tipo '{Name}'.");
        }
    }

    public Symbol GetMemberInGlobalTab(string memberName)
    {
        Members.TryGetValue(memberName, out var memberSymbol);
        return memberSymbol;
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
    private static SymbolTable globalSymbolTable; 

    private static readonly Dictionary<string, Type> TypeMapping = new Dictionary<string, Type>
    {
        { "Number", typeof(Number) },
        { "string", typeof(string) },
        { "bool", typeof(bool) },
        { "float", typeof(float) },
        { "double", typeof(double) },
        { "CardType", typeof(CardType) },
        { "Context", typeof(Context) },
        {"CardList", typeof(CardList)},
        {"Method", typeof(Method)},
        {"Effect", typeof(Effect)}
        // Agrega otros tipos primitivos o personalizados según sea necesario
    };
    public SemanticVisitor()
    {
        globalSymbolTable = new SymbolTable();
        RegisterGlobalSymbols();
        currentSymbolTable = new SymbolTable(globalSymbolTable);
    }
    private void RegisterGlobalSymbols()
    {
        // Registrar el tipo 'Card' con sus propiedades
        var cardSymbol = new Symbol("CardType", typeof(CardType));
        var power = new Symbol("Power", typeof(Context));
        var context = new Symbol("Context",typeof(Number));
        cardSymbol.AddMember("Name", new Symbol("Name", typeof(string)));
        cardSymbol.AddMember("Power", power);
        cardSymbol.AddMember("Faction", new Symbol("Faction", typeof(Faction)));
        context.AddMember("Prueba",new Symbol("Prueba",typeof(Number)));
        context.AddMember("blablaprueba",new Symbol("Prueba",typeof(Number)));
        // Agregar 'Card' al mapa global de tipos
        globalSymbolTable.AddSymbol("CardType", cardSymbol);
        globalSymbolTable.AddSymbol("Power", power);
        globalSymbolTable.AddSymbol("Context", context);


        // Otros tipos y propiedades
        // Registrar más tipos y sus miembros aquí...
    }

    private Type GetMappedType(string typeName)
    {
        if (TypeMapping.ContainsKey(typeName))
        {
            return TypeMapping[typeName];
        }
        else
        {
            // Intentar obtener el tipo por completo (para tipos definidos por el usuario o tipos complejos)
            var type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }
            else
            {
                ReportError($"Unknown type: {typeName}");
                return null; 
            }
        }
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
                return "Number"; // o "float" si corresponde
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
                if (propertyAccess.Target is IdentifierNode)
                {
                    //tipo del target y verifica si existe
                    Symbol target = currentSymbolTable.GetSymbol(((IdentifierNode)propertyAccess.Target).Name);
                    if (target == null)
                    {
                        ReportError($"El target {((IdentifierNode)propertyAccess.Target).Name} no esta declarado"); 
                        return "unknown";
                    }    
                    string targetType = target.Type.ToString();
                    //verifica si existe la propiedad del tipo del target
                    Symbol targetTypeSymbol = currentSymbolTable.GetSymbol(targetType);
                    if (targetTypeSymbol == null)
                    {
                        ReportError($"El tipo del target {((IdentifierNode)propertyAccess.Target).Name} no contiene ninguna propiedad con su nombre "); 
                        return "unknown";
                    } 
                    //verifica si el symbolo del tipo del target contiene alguna propiedad con el nombre de la property de accesProperty
                    string propertyName = propertyAccess.Property.Name;
                    Symbol propertyType = targetTypeSymbol.GetMemberInGlobalTab(propertyName);
                    if (propertyType == null)
                    {
                        ReportError($"No existe la propiedad {propertyName} para el tipo {targetType}");
                        return "unknown";
                    }
                    string type = propertyType.Type.ToString();//borrar esta linea es solo para debugear
                    return propertyType.Type.ToString();
                }
                string targetTyp = EvaluateType(propertyAccess.Target);
                //si regresaste es porque existe el targetType
                Symbol targetTypSymbol = currentSymbolTable.GetSymbol(targetTyp);
                if (targetTypSymbol == null)
                {
                ReportError($"El tipo del target {((IdentifierNode)propertyAccess.Target).Name} no contiene ninguna propiedad con su nombre "); 
                return "unknown";
                } 
                //verifica si el symbolo del tipo del target contiene alguna propiedad con el nombre de la property de accesProperty
                string propertyNam = propertyAccess.Property.Name;
                Symbol propertyTyp = targetTypSymbol.GetMemberInGlobalTab(propertyNam);
                if (propertyTyp == null)
                {
                    ReportError($"No existe la propiedad {propertyNam} para el tipo {targetTyp}");
                    return "unknown";
                }
                string respuesta =  propertyTyp.Type.ToString();//borrar esta linea es solo para debugear 
                return propertyTyp.Type.ToString();



        }
        return "unknown";
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
        // Primero, obtenemos el nombre de la variable que está siendo asignada
        if (node.Variable is PropertyAccessNode)
        {
            var propertyAccesType = EvaluateType(node.Variable);
            var valueType = EvaluateType(node.Value);
            if (!TypesAreCompatible(propertyAccesType,valueType))
            {
                ReportError($"Type mismatch: cannot assign '{valueType}' to '{propertyAccesType}'.");
            }
            // Evaluar y procesar el valor de la expresión en el lado derecho de la asignación
            node.Value.Accept(this);
            return;
        }
        var variableName = (node.Variable as IdentifierNode)?.Name;
        if (string.IsNullOrEmpty(variableName))
        {
            ReportError("Invalid assignment target.");
            return;
        }

        // Verificar si la variable está declarada en el alcance actual o en algún alcance padre
        var variableSymbol = currentSymbolTable.GetSymbol(variableName);

        if (variableSymbol == null)
        {
            // Si la variable no está declarada, se declara ahora con el tipo de la expresión asignada
            var exprType = EvaluateType(node.Value);
            if (exprType == "unknown")
            {
                ReportError($"Unable to determine the type of the expression assigned to '{variableName}'.");
                return;
            }

            // Agregar la variable a la tabla de símbolos actual
            var newSymbol = new Symbol(variableName, GetMappedType(exprType));
            currentSymbolTable.AddSymbol(variableName, newSymbol);
        }
        else
        {
            // Si ya está declarada, verificar que el tipo de la variable sea compatible con el tipo de la expresión
            var exprType = EvaluateType(node.Value);
            if (!TypesAreCompatible(variableSymbol.Type.ToString(), exprType))
            {
                ReportError($"Type mismatch: cannot assign '{exprType}' to '{variableSymbol.Type}' in variable '{variableName}'.");
            }
        }

        // Evaluar y procesar el valor de la expresión en el lado derecho de la asignación
        node.Value.Accept(this);
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
        currentSymbolTable.AddSymbol(node.Name,new Symbol(node.Name,typeof(Effect)));

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

        node.Variable.Accept(this);//asumi que esta variable siempre va a ser de tipo card
        var variable = currentSymbolTable.GetSymbol(node.Variable.Name);
        variable.Type = typeof(CardType);
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
        // Verifica si el identificador es dinámico
        if (node.IsDynamic)
        {
            currentSymbolTable.AddSymbol(node.Name, new Symbol(node.Name, typeof(IdentifierNode)));
            return; // No reportar error, ya que es un identificador dinámico
        }
        if (node.IsContext)
        {
            currentSymbolTable.AddSymbol(node.Name, new Symbol(node.Name,typeof(Context)));
            return; 
        }

        // Si no es dinámico, realiza el chequeo estándar
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
        // Evaluar el tipo del objetivo
        var targetType = EvaluateType(node.Target);
        var targetSymbol = GetMappedType(targetType);

        if (targetSymbol != null)
        {
            var propertySymbol = targetSymbol.GetMember(node.Property.Name);
            if (propertySymbol == null)
            {
                ReportError($"Property '{node.Property.Name}' does not exist in type '{targetType}'.");
            }
        }
        else
        {
            ReportError($"Type '{targetType}' is not defined.");
        }

        // Visitar los hijos
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

public class Context{}

public class CardList{}

public class Method{} 

public class Effect{}

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
