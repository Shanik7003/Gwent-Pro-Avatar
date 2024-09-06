using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Timers;
using UnityEngine;
public class Symbol
{
    public string Name { get; }
    public Type Type { get; set;}
    public bool IsFunction { get; } 
    public List<Symbol> Parameters { get; }
    public List<Type> FunctionParametersType { get; }
    public Dictionary<string, Symbol> Members { get; } 

    public Symbol(string name, Type type, bool isFunction = false)
    {
        Name = name;
        Type = type;
        IsFunction = isFunction;
        FunctionParametersType =  new List<Type>();
        Parameters = new List<Symbol>();
        Members = new Dictionary<string, Symbol>(); 
    }

    public Symbol(string name, Type type, Type parameterType, bool isFunction = false)
    {
        Name = name;
        Type = type;
        IsFunction = isFunction;
        //UnityEngine.Debug.Log("Creando nuevo symbol: "+ "name: "+ name +"Type: "+ type.ToString()+" parameterType: " + parameterType); 
        FunctionParametersType = new List<Type>{parameterType};
        Parameters = new List<Symbol>();
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

    public string ToString()
    {
        return $"Symbol(Name: {Name}, Type: {Type}, IsFunction: {IsFunction}, FunctionParametersTypes: {string.Join(", ", FunctionParametersType)})";
    }
}

public class SymbolTable
{
    private Dictionary<string, Symbol> symbols;
    public SymbolTable Parent { get; private set; }
    public List<SymbolTable> Children { get; private set; } 

    public SymbolTable(SymbolTable parent = null)
    {
        symbols = new Dictionary<string, Symbol>();
        Children = new List<SymbolTable>();
        Parent = parent;
        Parent?.Children.Add(this);  // Agregar este `SymbolTable` a los hijos del padre
    }

    public void AddSymbol(string name, Symbol symbol)
    {
        if (!symbols.ContainsKey(name))
        {
            symbols[name] = symbol;
        }
        else
        {
            PrintCurrentSymbolTable();
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

    public void PrintCurrentSymbolTable()
    { 
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("Símbolos en el alcance actual:");
        Console.ResetColor();
        foreach (var kvp in symbols)
        {
            Console.WriteLine($"  {kvp.Value}");
        }
    }

    public void PrintAllScopes()
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("Todos los Alcances:");
        Console.ResetColor();
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
    void Accept(IASTVisitor visitor);
}

// Clase abstracta que representa el visitante
public interface IASTVisitor
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
    public abstract void Visit(CompoundAssignmentNode node);
}

public class SemanticVisitor : IASTVisitor
{
    private SymbolTable currentSymbolTable;
    private static SymbolTable globalSymbolTable; 
    private List<CompilingError> semanticErrors;

    private static readonly Dictionary<string, Type> TypeMapping = new Dictionary<string, Type>
    {
        { "Number", typeof(Number) },
        { "string", typeof(string) },
        { "String", typeof(string) },
        { "bool", typeof(bool) },
        { "Bool", typeof(bool) },
        { "float", typeof(float) },
        { "double", typeof(double) },
        { "Engine.CardType", typeof(Engine.CardType) },
        { "Context", typeof(Context) },
        { "CardList", typeof(CardList ) },
        { "Method", typeof(Method) }, 
        { "Effect", typeof(Effect) },
        { "Null", typeof(Null) },
        { "Engine.Player", typeof(Engine.Player) }
        // Agrega otros tipos primitivos o personalizados según sea necesario
    };
    public SemanticVisitor(List<CompilingError> errors)
    {
        globalSymbolTable = new SymbolTable();
        RegisterGlobalSymbols();
        currentSymbolTable = new SymbolTable(globalSymbolTable);
        semanticErrors = errors;
        
    }
    private void RegisterGlobalSymbols()
    {
        // Registrar el tipo 'Card' con sus propiedades
        var cardSymbol = new Symbol("Engine.Card", typeof(Engine.Card));
        cardSymbol.AddMember("Name", new Symbol("Name", typeof(string)));
        cardSymbol.AddMember("Power", new Symbol("Power", typeof(Number)));
        cardSymbol.AddMember("Faction", new Symbol("Faction", typeof(Engine.Faction)));
        cardSymbol.AddMember("CardType", new Symbol("CardType", typeof(Engine.CardType)));
        cardSymbol.AddMember("Range",new Symbol("Range",typeof(CompilerPosition[])));
        cardSymbol.AddMember("Owner",new Symbol("Owner",typeof(Engine.Player)));        
        globalSymbolTable.AddSymbol("Engine.Card", cardSymbol);

        //Registrar el tipo Context con sus propiedades
        var context = new Symbol("Context", typeof(Context));
        context.AddMember("TriggerPlayer", new Symbol("Triggerplayer",typeof(Number)));  
        context.AddMember("Board", new Symbol("Board",typeof(CardList))); 
        context.AddMember("HandOfPlayer", new Symbol("HandOfPlayer",typeof(CardList),typeof(Engine.Player),true));
        context.AddMember("FieldOfPlayer", new Symbol("FieldOfPlayer",typeof(CardList),typeof(Engine.Player),true));
        context.AddMember("GraveyardOfPlayer", new Symbol("GraveyardOfPlayer",typeof(CardList),typeof(Engine.Player),true));
        context.AddMember("DeckOfPlayer", new Symbol("DeckOfPlayer",typeof(CardList),typeof(Engine.Player),true));
        context.AddMember("Hand", new Symbol("Hand",typeof(CardList)));
        context.AddMember("Deck", new Symbol("Deck",typeof(CardList)));
        context.AddMember("Field", new Symbol("Field",typeof(CardList)));
        context.AddMember("Graveyard", new Symbol("Graveyard",typeof(CardList)));
        globalSymbolTable.AddSymbol("Context", context);

        //Registrar el tipo CardList con sus metodos 
        var cardList = new Symbol("CardList",typeof(CardList));
        cardList.AddMember("Find",new Symbol("Find",typeof(CardList),typeof(MyPredicate),true));
        cardList.AddMember("Push",new Symbol("Push",typeof(Null),typeof(Engine.Card),true));
        cardList.AddMember("Add",new Symbol("Add",typeof(Null),typeof(Engine.Card),true));
        cardList.AddMember("SendBottom",new Symbol("SendBottom",typeof(Null),typeof(Engine.Card),true));
        cardList.AddMember("Pop",new Symbol("Pop",typeof(Engine.Card),true));
        cardList.AddMember("Remove",new Symbol("Remove",typeof(Null),typeof(Engine.Card),true));
        cardList.AddMember("Shuffle",new Symbol("Shuffle",typeof(Null),true));
        globalSymbolTable.AddSymbol("CardList",cardList);

        //Registrar todas las facciones y las source y todos los enum del juego 
        var FireNation = new Symbol("FireNation",typeof(Engine.Faction));
        var WaterTribe = new Symbol("WaterTribe",typeof(Engine.Faction));
        var EarthKingdom = new Symbol("EarthKingdom",typeof(Engine.Faction));
        var AirNomads = new Symbol("AirNomads",typeof(Engine.Faction));
        globalSymbolTable.AddSymbol("FireNation",FireNation);
        globalSymbolTable.AddSymbol("WaterTribe",WaterTribe);
        globalSymbolTable.AddSymbol("AirNomads",AirNomads);
        globalSymbolTable.AddSymbol("EarthKingdom",EarthKingdom);
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
                throw new Exception($"Unknown type: {typeName}");
                return null; 
            }
        }
    }

    private void AddSemanticError(CodeLocation location, string message)
    {
        semanticErrors.Add(new CompilingError(location, ErrorCode.SemanticError, message)); 
    }

    private string EvaluateType(ExpressionNode expression)
    {
        switch (expression)
        {
            case Number number:
                return "Number"; // o "float" si corresponde
            case Text text:
            //puede ser un string de una faction o una source o algo significativo del juego
                //*! var symb = currentSymbolTable.GetSymbol(text.Value);
                //*! if (symb != null)
                //*! {
                //*!     return symb.Type.ToString();
                //*! }
                //si no es una string del juego especifica retorna el string 
                return "string";
            case IdentifierNode identifierNode:
                var symbol = currentSymbolTable.GetSymbol(identifierNode.Name);
                if (symbol != null)
                {
                    return symbol.Type.ToString();
                }
                else
                {
                   AddSemanticError(expression.Location,$"Variable '{identifierNode.Name}' is not declared");
                    return "unknown";
                }
            case BinaryOperation binaryOperation:
                var leftType = EvaluateType(binaryOperation.Left);
                var rightType = EvaluateType(binaryOperation.Right);
                if (leftType == rightType)
                {
                    return leftType;
                }
                else if(leftType is string)// si no fueron iguales y la izquierda es string, evaluala por si acaso
                {
                    var symb = currentSymbolTable.GetSymbol(leftType);
                    if (symb != null)
                    {
                        return symb.Type.ToString();
                    }
                }
                else if(rightType is string)
                {
                    var symb = currentSymbolTable.GetSymbol(rightType);
                    if (symb != null)
                    {
                        return symb.Type.ToString();
                    }
                }
                
                AddSemanticError(binaryOperation.Location,$"Type mismatch in binary operation: {leftType} and {rightType}");
                return "unknown";
                
            case ExpressionMethodCall methodCall:
                string targeType = EvaluateType(methodCall.Target);
                Symbol targeTypeSymbol = currentSymbolTable.GetSymbol(targeType);
                if (targeTypeSymbol == null)
                {
                   AddSemanticError(methodCall.Location,$"El tipo  {targeType} no contiene ninguna propiedad"); 
                    return "unknown";
                } 
                string functionName = methodCall.Funtion.Name;
                Symbol functionSymbol = targeTypeSymbol.GetMemberInGlobalTab(functionName);  
                if (functionSymbol == null)
                {
                   AddSemanticError(methodCall.Location,$"No existe la propiedad {functionName} para el tipo {targeType}");
                    return "unknown";
                }
                
                //verificaciones del parametro 
                if (methodCall.Param != null && functionSymbol.FunctionParametersType.Count > 0)//si el methodCall tiene un parametro y el tipo de la funcion requiere un parametro 
                {
                    if (methodCall.Param is MyPredicate && functionSymbol.Name != "Find")
                    {
                        AddSemanticError(methodCall.Location,$"Invalid parameterType in funtion {methodCall.Funtion.Name}, Expected type: '{functionSymbol.FunctionParametersType[0].Name}'");
                    }
                    else if(methodCall.Param is MyPredicate && functionSymbol.Name == "Find")
                    {
                        return functionSymbol.Type.ToString();
                    }
                    //verificar si el parametro del methodCall es del tipo requerido 
                    string paramType = EvaluateType((ExpressionNode)methodCall.Param);//evalua el tipo del parametro 
                    string functionParamType = functionSymbol.FunctionParametersType[0].ToString();
                    if (!TypesAreCompatible(paramType,functionParamType))
                    {
                        AddSemanticError(methodCall.Location,$"Invalid parameterType in funtion {methodCall.Funtion.Name}, Expected type: '{functionSymbol.FunctionParametersType[0].Name}'");
                    }
                }
                else if (methodCall.Param == null)
                {
                    if (functionSymbol.FunctionParametersType.Count > 0)
                    {
                        AddSemanticError(methodCall.Location,$"There is no argument given that corresponds to the required parameter '{functionSymbol.FunctionParametersType[0].Name}' of the method {methodCall.Funtion.Name}");
                    }
                } 
                else if(functionSymbol.FunctionParametersType.Count == 0)
                {
                    if (methodCall.Param != null)
                    {
                        AddSemanticError(methodCall.Location,$"Invalid expression term '{methodCall.Param}'");
                    }
                }

                return functionSymbol.Type.ToString();
            case PropertyAccessNode propertyAccess:
                if (propertyAccess.Target is IdentifierNode)
                {
                    //tipo del target y verifica si existe
                    Symbol target = currentSymbolTable.GetSymbol(((IdentifierNode)propertyAccess.Target).Name);
                    if (target == null)
                    {
                       AddSemanticError(propertyAccess.Location,$"El target {((IdentifierNode)propertyAccess.Target).Name} no esta declarado"); 
                        return "unknown";
                    }    
                    string targetType = target.Type.ToString();
                    //verifica si existe la propiedad del tipo del target
                    Symbol targetTypeSymbol = currentSymbolTable.GetSymbol(targetType);
                    if (targetTypeSymbol == null)
                    {
                       AddSemanticError(propertyAccess.Location,$"El tipo {targetType} no contiene ninguna propiedad"); 
                        return "unknown";
                    } 
                    //verifica si el symbolo del tipo del target contiene alguna propiedad con el nombre de la property de accesProperty
                    string propertyName = propertyAccess.Property.Name;
                    Symbol propertyType = targetTypeSymbol.GetMemberInGlobalTab(propertyName);
                    if (propertyType == null)
                    {
                       AddSemanticError(propertyAccess.Location,$"No existe la propiedad {propertyName} para el tipo {targetType}");
                        return "unknown";
                    }
                    return propertyType.Type.ToString();
                }
                string targetTyp = EvaluateType(propertyAccess.Target);
                //si regresaste es porque existe el targetType
                Symbol targetTypSymbol = currentSymbolTable.GetSymbol(targetTyp);
                if (targetTypSymbol == null)
                {
                    AddSemanticError(propertyAccess.Location,$"El tipo del target {propertyAccess.Target} no contiene ninguna propiedad con su nombre "); 
                    return "unknown";
                } 
                //verifica si el symbolo del tipo del target contiene alguna propiedad con el nombre de la property de accesProperty
                string propertyNam = propertyAccess.Property.Name;
                Symbol propertyTyp = targetTypSymbol.GetMemberInGlobalTab(propertyNam);
                if (propertyTyp == null)
                {
                   AddSemanticError(propertyAccess.Location,$"No existe la propiedad {propertyNam} para el tipo {targetTyp}");
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

    public void Visit(RootNode node)
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

    public  void Visit(Assignment node)
    {
        // Primero, obtenemos el nombre de la variable que está siendo asignada
        if (node.Variable is PropertyAccessNode)
        {
            var propertyAccesType = EvaluateType(node.Variable);
            var valueType = EvaluateType(node.Value);
            if (!TypesAreCompatible(propertyAccesType,valueType))
            {
               AddSemanticError(node.Location,$"Type mismatch: cannot assign '{valueType}' to '{propertyAccesType}'.");
            }
            // Evaluar y procesar el valor de la expresión en el lado derecho de la asignación
            node.Value.Accept(this);
            return;
        }
        var variableName = (node.Variable as IdentifierNode)?.Name;
        if (string.IsNullOrEmpty(variableName))
        {
           AddSemanticError(node.Location,"Invalid assignment target.");
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
               AddSemanticError(node.Location,$"Unable to determine the type of the expression assigned to '{variableName}'.");
                return;
            }

            // Agregar la variable a la tabla de símbolos actual
            try
            {
                var newSymbol = new Symbol(variableName, GetMappedType(exprType));
                currentSymbolTable.AddSymbol(variableName, newSymbol);
            }
            catch (System.Exception ex)
            {
                AddSemanticError(node.Location,ex.Message);
            }
        }
        else
        {
            // Si ya está declarada, verificar que el tipo de la variable sea compatible con el tipo de la expresión
            var exprType = EvaluateType(node.Value);
            if (!TypesAreCompatible(variableSymbol.Type.ToString(), exprType))
            {
               AddSemanticError(node.Location,$"Type mismatch: cannot assign '{exprType}' to '{variableSymbol.Type}' in variable '{variableName}'.");
            }
        }

        // Evaluar y procesar el valor de la expresión en el lado derecho de la asignación
        node.Value.Accept(this);
    }

    public void Visit(CompoundAssignmentNode node)
    {
        var variableType  = EvaluateType(node.Variable);
        var valueType = EvaluateType(node.Value);
        if(variableType != "Number" || valueType != "Number")
        {
            AddSemanticError(node.Location,$"Type mismatch: cannot execute operation '{node.Operator}' to variableType: '{variableType}' with alueType:'{valueType}'.");
        }
    }


    public void Visit(MethodCallNode node)
    {
        string targetType = EvaluateType(node.Target);
        Symbol targetTypeSymbol = currentSymbolTable.GetSymbol(targetType);
        if (targetTypeSymbol == null)
        {
           AddSemanticError(node.Location,$"El tipo {targetType} no esta declarado");
        }
        Symbol functionSymbol = targetTypeSymbol.GetMemberInGlobalTab(node.Funtion.Name);
        if (functionSymbol == null)
        {
           AddSemanticError(node.Location,$"El metodo {node.Funtion.Name} no puede ser aplicado a el tipo {targetType}");
        }
        //verificaciones del parametro 
        if (node.Param != null && functionSymbol.FunctionParametersType.Count > 0)//si el methodCall tiene un parametro y el tipo de la funcion requiere un parametro 
        {
            if (node.Param is MyPredicate && functionSymbol.Name == "Find")
            {
                UnityEngine.Debug.Log("el parametro es un predicado y la funcion es Find");
                return;
            }
            //verificar si el parametro del methodCall es del tipo requerido 
            string paramType = EvaluateType((ExpressionNode)node.Param);//evalua el tipo del parametro 
            string functionParamType = functionSymbol.FunctionParametersType[0].ToString();
            if (!TypesAreCompatible(paramType,functionParamType))
            {
               AddSemanticError(node.Location,$"Invalid parameterType in funtion {node.Funtion.Name}, Expected type: '{functionSymbol.FunctionParametersType[0].Name}'");
            }
        }
        else if (node.Param == null)
        {
            if (functionSymbol.FunctionParametersType.Count > 0)
            {
               AddSemanticError(node.Location,$"There is no argument given that corresponds to the required parameter '{functionSymbol.FunctionParametersType[0].Name}' of the method {node.Funtion.Name}");
            }
        } 
        else if(functionSymbol.FunctionParametersType.Count == 0)
        {
            if (node.Param != null)
            {
               AddSemanticError(node.Location,$"Invalid expression term '{node.Param}'");
            }
        }

    }

    public void Visit(EffectNode node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();
       
        if (EvaluateType(node.Name) != "string")
        {
            AddSemanticError(node.Location,"El nombre del efecto debe ser de tipo string");
        }

        globalSymbolTable.AddSymbol(((Text)node.Name).Value ,new Symbol(((Text)node.Name).Value,typeof(Effect)));
        Symbol Effect = globalSymbolTable.GetSymbol(((Text)node.Name).Value);//creo que a este se le puede quitar lo amarillo porque nunca va a ser

        foreach (var param in node.Params)
        {
            param.Accept(this);
            try
            {
                Type debug = GetMappedType(param.Type);
                Effect.Parameters.Add(new Symbol(param.Name, debug));
            }
            catch (System.Exception ex)
            {
                AddSemanticError(node.Location,ex.Message);
            }
        }

        node.Action.Accept(this);

        currentSymbolTable = currentSymbolTable.ExitScope();
    }

    public void Visit(ParamNode node)
    {
        var paramName = node.Name;
        var paramType = node.Type;

        if (currentSymbolTable.ContainsSymbol(paramName))
        {
           AddSemanticError(node.Location,$"Parameter '{paramName}' is already declared");
        }
        else
        {
            try
            {
                globalSymbolTable.AddSymbol(paramName, new Symbol(paramName, GetMappedType(paramType)));     
            }
            catch (System.Exception ex)
            {
                AddSemanticError(node.Location,ex.Message);
            }
        }
    }

    public void Visit(ActionNode node)
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

    public void Visit(ForStatement node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();

        node.Variable.Accept(this);//asumi que esta variable siempre va a ser de tipo card
        node.Iterable.Accept(this);
        foreach (var statement in node.Body)
        {
            statement.Accept(this);
        }

        currentSymbolTable = currentSymbolTable.ExitScope();
    }

    public void Visit(WhileStatement node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();

        if (!node.Condition.IsLogicalExp)
        {
           AddSemanticError(node.Location,"La Expresion dentro del while debe ser una expresion logica");
        }
        node.Condition.Accept(this);

        foreach (var statement in node.Body)
        {
            statement.Accept(this);
        }

        currentSymbolTable = currentSymbolTable.ExitScope();
    }

    public void Visit(BinaryOperation node)
    {
        var leftType = EvaluateType(node.Left);
        var rightType = EvaluateType(node.Right);
        
        if (!TypesAreCompatible(leftType, rightType))
        {
            if (leftType == "string")
            {
                var v = EvaluateStringExpression(node.Left);
                if (currentSymbolTable.ContainsSymbol((string)v))
                {
                    if(rightType != currentSymbolTable.GetSymbol((string)v).Type.ToString())
                    {
                        AddSemanticError(node.Location,$"Type mismatch in binary operation: {leftType} and {rightType}");
                    }
                }
            }
            else if (rightType == "string")
            {
                var s = EvaluateStringExpression(node.Right);
                if (currentSymbolTable.ContainsSymbol((string)s))
                {
                    if(leftType != currentSymbolTable.GetSymbol((string)s).Type.ToString())
                    {
                        AddSemanticError(node.Location,$"Type mismatch in binary operation: {leftType} and {rightType}");
                    }
                }
            }
            // else
            // {
            //     AddSemanticError(node.Location,$"Type mismatch in binary operation: {leftType} and {rightType}");
            // }
        }

        node.Left.Accept(this);
        node.Right.Accept(this);
    }

    public void Visit(IdentifierNode node)
    {
        // Verifica si el identificador es dinámico
        if (node.IsContext)
        {
            currentSymbolTable.AddSymbol(node.Name, new Symbol(node.Name,typeof(Context)));
            return; 
        }
        else if (node.IsCard)
        {
            currentSymbolTable.AddSymbol(node.Name, new Symbol(node.Name,typeof(Engine.Card)));
            return; 
        }
        else if (node.IsCardList)
        {
            currentSymbolTable.AddSymbol(node.Name, new Symbol(node.Name,typeof(CardList)));
            return; 
        }
        else if (node.IsDynamic)
        {
            currentSymbolTable.AddSymbol(node.Name, new Symbol(node.Name, typeof(IdentifierNode)));
            return; // No reportar error, ya que es un identificador dinámico
        }
        //currentSymbolTable.PrintAllScopes();
        // Si no es dinámico, realiza el chequeo estándar
        if (!currentSymbolTable.ContainsSymbol(node.Name))
        {
           AddSemanticError(node.Location,$"Variable '{node.Name}' is not declared");
        }
    }

    public void Visit(Number node)
    {
        // Nothing to check for a number literal
    }

    public void Visit(PropertyAccessNode node)
    {
        // Evaluar el tipo del objetivo
        var targetType = EvaluateType(node.Target);
        Symbol targetSymbol = currentSymbolTable.GetSymbol(targetType);

        if (targetSymbol != null)
        {
            var propertySymbol = targetSymbol.GetMemberInGlobalTab(node.Property.Name);
            if (propertySymbol == null)
            {
               AddSemanticError(node.Location,$"Property '{node.Property.Name}' does not exist in type '{targetType}'.");
            }
        }
        else
        {
           AddSemanticError(node.Location,$"Type '{targetType}' is not defined.");
        }
    }

    public void Visit(CardNode node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();

        //?Type
        string type = EvaluateType(node.Type);
        if (type != "string") AddSemanticError(node.Location,$"El campo Type de las cartas debe ser de tipo string");
        var typ = EvaluateStringExpression(node.Type);
        if (!Enum.TryParse(typ as string, out Engine.CardType cardType))
        {
            AddSemanticError(node.Location,  $"Tipo de carta inválido: {typ}");
        }

        //?Name
        string nametype = EvaluateType(node.Name);
        if (nametype != "string") AddSemanticError(node.Location,$"El campo Name de las cartas debe ser de tipo string");
        
        //?Faction
        string factionType = EvaluateType(node.Faction);
        if (factionType != "string") AddSemanticError(node.Location,$"El campo Faction de las cartas debe ser de tipo string");
        var faction = EvaluateStringExpression(node.Faction);
        if (!Enum.TryParse(faction as string, out Engine.Faction cardFaction))
        {
            AddSemanticError(node.Location,  $"Tipo de carta inválido: {faction}");
        }
        
        //?Power
        string power = EvaluateType(node.Power);
        if(power != "Number") AddSemanticError(node.Location,$"El campo Power de las cartas debe ser de tipo Number");
        
        foreach (var effect in node.EffectList)
        {
            effect.Accept(this);
        }

        currentSymbolTable = currentSymbolTable.ExitScope();
    }

    public void Visit(EffectInvocationNode node)
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

    public void Visit(EffectField node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();

        //verificar que el name del efecto este declarado anteriormente
        if (EvaluateType(node.Name) != "string")
        {
            AddSemanticError(node.Location,$"El campo Name de EffectInvocation debe ser de tipo string");
        }

        string effect = EvaluateStringExpression(node.Name) as string;
        if (!currentSymbolTable.ContainsSymbol(effect))
        {
            AddSemanticError(node.Location,"El efecto {effect} no esta declarado, no existe");
        }

        node.Name.Accept(this);//esto comprueba que exista un efecto real con ese nombre que ahora tendra que hacerce en el evaluate
        //comparar el numero de parametros del nodo y del efecto que ya esta declarado en la tabla global 
        if (node.Params != null)
        {
            int nodeParams = node.Params.Count;
            Symbol effectSymbol = globalSymbolTable.GetSymbol(effect);
            if (effectSymbol == null)
            {
               AddSemanticError(node.Location,$"El simbolo {effect} no esta declarado");
                return;
            }
            else
            {
                int effectParams = effectSymbol.Parameters.Count;
                if (nodeParams != effectParams)
                {
                   AddSemanticError(node.Location,$"El efecto {node.Name} requiere {effectParams} parametros, no coincide con la cantidad de parametros proporcionados: {nodeParams}");
                }
            }  
            foreach (var param in node.Params)
            {
                param.Accept(this);
            }
        }

        currentSymbolTable = currentSymbolTable.ExitScope();   
    }

    public void Visit(CardParam node)
    {
        node.Name.Accept(this);
        //si llego aqui es porque estaba declarado ya entonces hay que verificar que sea el tipo que debe ser
        Symbol param = currentSymbolTable.GetSymbol(node.Name.Name);
        string paramType = param.Type.ToString();
        var valueType = EvaluateType(node.Value);
       if (!TypesAreCompatible(paramType,valueType))
       {
          AddSemanticError(node.Location,$"Type mismatch in binary operation: {paramType} and {valueType}");
       }
        node.Name.Accept(this);
        node.Value.Accept(this);
    }

    public void Visit(SelectorNode node)
    {
        currentSymbolTable = currentSymbolTable.EnterScope();

        if (node.Predicate != null)
        {
            node.Predicate.Accept(this);
        }

        currentSymbolTable = currentSymbolTable.ExitScope();
    }

    public void Visit(MyPredicate node)
    {
        node.Param.Accept(this);
        if (!node.Condition.IsLogicalExp)
        {
           AddSemanticError(node.Location,"La condicion del predicado debe ser una expresion logica");
        }
        node.Condition.Accept(this);
    }

    public void Visit(ExpressionNode node)
    {
       // System.Console.WriteLine("Todavia no implemetado el Visit(ExpressionNode)");
    }

    private object EvaluateStringExpression(ExpressionNode expression)
    {
        switch (expression)
        {
            case Text text:
                return text.Value;
            case BinaryOperation binaryOperationNode:
                var leftValue = EvaluateStringExpression(binaryOperationNode.Left);
                var rightValue = EvaluateStringExpression(binaryOperationNode.Right);
                return EvaluateBinaryOperation(binaryOperationNode.Operator, leftValue, rightValue);
            
            default:
                return null;
        }
        
    }
    private object EvaluateBinaryOperation(string op, object leftValue, object rightValue)
    {
        switch (op)
        {
            case "@":
                return (leftValue as string) + (rightValue as string);
            case "@@":
                return (leftValue as string) + " " +(rightValue as string);
            default:
                return null;
        }
    }
}

public class CardList{}

public class Method{} 

public class Effect{}

public class Null {}
public class Context{}

public enum CompilerPosition
{
    M,
    R,
    S
}

public enum Source
{
    parent,
    board,
    hand,
    otherHand,
    deck,
    otherDeck,
    field,
    otherField,

}

public delegate bool MyDelegate(int x);
