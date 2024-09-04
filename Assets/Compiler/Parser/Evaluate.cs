using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using Engine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ExecutionVisitor : IASTVisitor
{
    private Dictionary<string,object> ObjectsMapping;
    private Dictionary<string,object> Variables ;
    public RootNode Ast;
    public ExecutionVisitor(RootNode ast)
    {
        ObjectsMapping = new Dictionary<string, object>();
        InicializedObjectsMapping();
        Variables = new Dictionary<string, object>();
        Ast = ast;
    }
    static void PrintDictionary(Dictionary<string, object> dictionary)
    {
        foreach (var kvp in dictionary)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }

    public void InicializedObjectsMapping()
    {
        ObjectsMapping.Add("TriggerPlayer",GetTriggerPlayer());
        ObjectsMapping.Add("Hand",GetTriggerPlayerHand());
        ObjectsMapping.Add("Deck",GetTriggerPlayerDeck());
        ObjectsMapping.Add("Field",GetTriggerPlayerField());
        ObjectsMapping.Add("Graveyard",GetTriggerPlayerGraveyard());
        ObjectsMapping.Add("Board",Game.GameInstance.Board);
        ObjectsMapping.Add("board",Game.GameInstance.Board);
        ObjectsMapping.Add("hand",GetTriggerPlayerHand());
        ObjectsMapping.Add("otherHand",GetOtherPlayerHand());
        ObjectsMapping.Add("deck",GetTriggerPlayerDeck());
        ObjectsMapping.Add("otherDeck",GetOtherPlayerDeck());
        ObjectsMapping.Add("field",GetTriggerPlayerField());
        ObjectsMapping.Add("otherField",GetOtherPlayerField());
        ObjectsMapping.Add("FireNation",Engine.Faction.FireNation);
        ObjectsMapping.Add("WaterTribe", Engine.Faction.WaterTribe);
        ObjectsMapping.Add("AirNomads", Engine.Faction.AirNomads);
        ObjectsMapping.Add("EarthKingdom", Engine.Faction.EarthKingdom);
    }

    public void Visit(RootNode node)
    {   
        foreach (var card in node.Cards)
        {
            card.Accept(this);
        }
    }

    public void Visit(CardNode node)
    {
        //? crea una carta del juego real y añadela a AllCards
        Card card = new(node.Type,node.Name.Name,node.Faction,node.Power,AdecuatePosition(node.Position),Game.GameInstance.GenerateGuid());
        Game.GameInstance.AddCard(card);//la añade al diccionario de todas las cartas del juego;

    }
    public Position AdecuatePosition(CompilerPosition[] Range)
    {
        string range = "";
        foreach (var pos in Range)
        {
            range += pos.ToString();
        }
        if (range.Contains('M') && range.Contains('R')&& range.Contains('S'))
        {
            return Position.MRS;
        }
        if (range.Contains('M') && range.Contains('R'))
        {
            return Position.MR;
        }
        if (range.Contains('R') && range.Contains('S'))
        {
            return Position.RS;
        }
        if (range.Contains('M') && range.Contains('S'))
        {
            return Position.MS;
        }
        if (range.Contains('M'))
        {
            return Position.M;
        }
        if (range.Contains('R'))
        {
            return Position.R;
        }
        if (range.Contains('S'))
        {
            return Position.S;
        }
        return Position.Leaderposition;
    }
    public void Visit(EffectInvocationNode node)
    {
        node.EffectField.Accept(this);

        if (node.Selector != null)
        {
            node.Selector.Accept(this);
        }

        //?! AQUI!!! antes de ejecutar el postAction es que hay que visitar el effecto para que eso haga que los efectos se hagan en elorden adecuado 
        foreach (var effect in Ast.Effects)
        {
           if (effect.Name.Name == node.EffectField.Name.Name)
           {
                effect.Accept(this);
           }
        }

        if (node.PostAction != null)
        {
            node.PostAction.Accept(this);
        }
    }

    public void Visit(EffectField node)
    {
        //guarda los parametros en la tabla de parametros 
        Variables = new Dictionary<string, object>();

        foreach (var param in node.Params)
        {
            Variables[param.Name.Name] = param.Value; //*! ya añadi los parametros 
        }
    }
    public void Visit(SelectorNode node)
    {
        List<Card> source = (List<Card>)ObjectsMapping[node.Source.ToString()];
        List<Card> targets = new List<Card>();

        foreach (var card in source)
        {
            Variables[node.Predicate.Param.Name] = card;
            if ((bool)EvaluateExpression(node.Predicate.Condition))
            {
                targets.Add(card);
            }
        }
        Variables["3007_targets"] = targets;//*! ya añadi la lista de targets
    }
    
    public void Visit(ActionNode node)
    {
        Variables[node.Targets.Name] = Variables["3007_targets"];

        foreach (var statement in node.Statements)
        {
            statement.Accept(this);
        }
    }
    static void SetValue(ref object obj, object value)
    {
        obj = value;
    }
    public void Visit(Assignment node)
    {
        var value = EvaluateExpression(node.Value);

        if (node.Variable is PropertyAccessNode)
        {
            object vari = EvaluateExpression(((PropertyAccessNode)node.Variable).Target);
            if (vari is Card)//entonces estan actulizando el valor de alguna propiedad de una carta 
            {
                if (((PropertyAccessNode)node.Variable).Property.Name == "Power")
                {
                    if (((Card)vari).player.Field.Contains((Card)vari)) // si la carta esta en el field tienes que actualizar los puntos del player de la carta
                    {
                        if(((Card)vari).points > (double)value)
                        {
                            ((Card)vari).player.Points -= ((Card)vari).points - (double)value;
                        }
                        else if(((Card)vari).points < (double)value)
                        {
                            ((Card)vari).player.Points += (double)value - ((Card)vari).points;
                        }
                    }
                    //y ademas cambiar los puntos de la carta, si no estaba en el Field de todas forma se sto hay que hacerlo 
                    ((Card)vari).points = (double)value;
                }
                //*!no veo que mas se le pueda cambiar a la carta como usuario?? no se le debe poder cambiar el nombre o la 
                //*!faction o si???
                return;
            }
        }

        var  variable = EvaluateExpression(node.Variable);

        if (variable is string)//es porque estan declarando una nueva variable
        {
            Variables[(string)variable] = value;
            return;
        }

        if (Variables.ContainsKey(((IdentifierNode)node.Variable).Name))//si ya lo contiene pues cambia el valor del objecto
        {
            Variables[((IdentifierNode)node.Variable).Name] = value; 
        }

        PrintDictionary(Variables);
    }

    public void Visit(MethodCallNode node)
    {
        object target = EvaluateExpression(node.Target);//TODO confio en que el target de los methodsCall siempre sea por referencia 
        string method = node.Funtion.Name;
        if (node.Param != null)
        {
            if (node.Param is MyPredicate)
            {
                ExecuteMethod(method, target,node.Param);
                return;
            }
            object param = EvaluateExpression((IdentifierNode)node.Param);
            ExecuteMethod(method,target, param);
        }

        else ExecuteMethod(method,target);
    }

    public void Visit(ForStatement node)
    {
        var collection = EvaluateExpression(node.Iterable) as IEnumerable<Engine.Card>;
        foreach (var item in collection)
        {
            Variables[node.Variable.Name] = item;
            foreach (var statement in node.Body)
            {
                statement.Accept(this);
            }
        }
    }

    public void Visit(WhileStatement node)
    {
        while ((bool)EvaluateExpression(node.Condition))
        {
            foreach (var statement in node.Body)
            {
                statement.Accept(this);
            }
        }
    }
    public void Visit(ExpressionNode node)
    {
        throw new NotImplementedException();
    }

    public void Visit(PropertyAccessNode node)
    {
        EvaluateExpression(node);
    }

    public void Visit(EffectNode node)
    {
        node.Action.Accept(this);
    }

    public void Visit(ParamNode node)
    {
        throw new NotImplementedException();
    }

    public void Visit(BinaryOperation node)
    {
        var leftValue = EvaluateExpression(node.Left);
        var rightValue = EvaluateExpression(node.Right);
        var result = EvaluateBinaryOperation(node.Operator, leftValue, rightValue);
        // You might want to store this result or use it within the current context
    }
    

    public void Visit(IdentifierNode node)
    {
        var value = EvaluateIdentifier(node);
        // Depending on the context, you may store this value, return it, or pass it along
    }


    public void Visit(Number node)
    {
        throw new NotImplementedException();
    }


    public void Visit(CardParam node)
    {
        throw new NotImplementedException();
    }



    public void Visit(MyPredicate node)
    {
        throw new NotImplementedException();
    }

    private object ExecuteMethod(string method, object target, object param = null)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target), $"The target object for method '{method}' is null.");
        }
        

        if (target is List<Card> cardList)
        {
            switch (method)
            {
                case "Find":
                    if (param is MyPredicate predicate)
                    {
                        return Find(cardList, predicate);
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid parameter type for method '{method}'. Expected 'MyPredicate', got '{param?.GetType().Name}'.");
                    }
                case "Add":
                    if (param is Card card)
                    {
                        Add(cardList, card);
                        return null;
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid parameter type for method '{method}'. Expected 'Card', got '{param?.GetType().Name}'.");
                    }
                case "Push":
                    if (param is Card addedCard)
                    {
                        Push(cardList, addedCard);
                        return null;
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid parameter type for method '{method}'. Expected 'Card', got '{param?.GetType().Name}'.");
                    }

                case "SendBottom":
                    if (param is Card bottomCard)
                    {
                        SendBottom(cardList, bottomCard);
                        return null;
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid parameter type for method '{method}'. Expected 'Card', got '{param?.GetType().Name}'.");
                    }

                case "Pop":
                    return Pop(cardList);

                case "Remove":
                    if (param is Card removeCard)
                    {
                        Remove(cardList, removeCard);
                        return null;
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid parameter type for method '{method}'. Expected 'Card', got '{param?.GetType().Name}'.");
                    }

                case "Shuffle":
                    Shuffle(cardList);
                    return null;

                default:
                    throw new NotSupportedException($"Method '{method}' is not supported for type 'List<Card>'.");
            }
        }
        else if (target is Player player)
        {
            switch (method)
            {
                case "HandOfPlayer":
                    return GetPlayerHand(player);

                case "FieldOfPlayer":
                    return GetPlayerField(player);

                case "GraveyardOfPlayer":
                    return GetPlayerGraveyard(player);

                case "DeckOfPlayer":
                    return GetPlayerDeck(player);

                default:
                    throw new NotSupportedException($"Method '{method}' is not supported for type 'Player'.");
            }
        }
        else
        {
            throw new NotSupportedException($"Target type '{target.GetType().Name}' is not supported by method '{method}'.");
        }
    }


    private List<Card> Find(List<Card> target, MyPredicate param)
    {
        List<Card> container = new();
        foreach (var item in target)
        {
            Variables[param.Param.Name] = item;
            if ((bool)EvaluateExpression(param.Condition))
            {
                container.Add(item);
            } 
        }
        return container;
    }

    private void Add(List<Card>target, Card param)
    {
        if (target == Game.GameInstance.Player1.Deck || target == Game.GameInstance.Player2.Deck || target == Game.GameInstance.Player1.Graveyard || target == Game.GameInstance.Player2.Graveyard)
        {
            param.MoveCardAndDesapeare(target);
        } 
        param.MoveCard(target);
    }
    private void Push(List<Card> target, Card param)
    {
        if (target == Game.GameInstance.Player1.Deck || target == Game.GameInstance.Player2.Deck || target == Game.GameInstance.Player1.Graveyard || target == Game.GameInstance.Player2.Graveyard)
        {
            param.MoveCardAndDesapeare(target); 
        }
        //TODO para las listas no apiladas tiene que agregarse a la derecha de la fila 
         param.MoveCardToRight(target);
        
    }

    private void SendBottom(List<Card> target, Card param)
    {
        if (target == Game.GameInstance.Player1.Deck || target == Game.GameInstance.Player2.Deck || target == Game.GameInstance.Player1.Graveyard || target == Game.GameInstance.Player2.Graveyard)
        {
            //*!aqui hay que hacerlo asi obligado para que verdaderamnete la carta se coloque en el fondo de la lista, en vez de llamar simplemente al metodo MoveCardAndDesapeare()
            param.Ubication.Remove(param);
            target.Insert(0,param);
            param.Ubication = target;
            param.NotifyObservers(Engine.EventType.CardMovedAndDesapeare,target);
            // param.MoveCardAndDesapeare(target);
        }
        //TODO agregarse a la izquierda de la fila 
        param.MoveCardToLeft(target);
        //target.Insert(0,param);
    }
    private Card Pop(List<Card>target)
    {
        if (target.Count == 0)
        {
            throw new InvalidOperationException("Cannot pop from an empty list.");
        }
        var card = target[target.Count - 1];
        target.RemoveAt(target.Count - 1);
        return card;
    }

    private void Remove(List<Card>target,Card param)
    {
        param.RemoveCard();
    }

    private void Shuffle(List<Card>targets)
    {
        //TODO hacer el efecto visual para shuffle
        System.Random rng = new System.Random();
        int n = targets.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = targets[k];
            targets[k] = targets[n];
            targets[n] = value;
        }
    }
    private List<Card> GetPlayerDeck(Player player)
    {
        if (TurnManager.Instance.GetCurrentPlayer() == player)
        {
            return TurnManager.Instance.GetCurrentPlayer().Deck;
        }
        else
        {
            return TurnManager.Instance.GetCurrentEnemy().Deck;
        }
    }
    private List<Card> GetPlayerHand(Player player)
    {
        if (TurnManager.Instance.GetCurrentPlayer() == player)
        {
            return TurnManager.Instance.GetCurrentPlayer().Hand;
        }
        else
        {
            return TurnManager.Instance.GetCurrentEnemy().Hand;
        }
    }
    private List<Card> GetPlayerField(Player player)
    {
        if (TurnManager.Instance.GetCurrentPlayer() == player)
        {
            return TurnManager.Instance.GetCurrentPlayer().Field;
        }
        else
        {
            return TurnManager.Instance.GetCurrentEnemy().Field;
        }
    }
    private List<Card> GetPlayerGraveyard(Player player)
    {
        if (TurnManager.Instance.GetCurrentPlayer() == player)
        {
            return TurnManager.Instance.GetCurrentPlayer().Graveyard;
        }
        else
        {
            return TurnManager.Instance.GetCurrentEnemy().Graveyard;
        }
    }

    private object EvaluatePropertyAccess(PropertyAccessNode propertyAccessNode)
    {

        object target = EvaluateExpression(propertyAccessNode.Target);

        if (Variables.ContainsValue(target) || ObjectsMapping.ContainsKey(propertyAccessNode.Property.Name))
        {
            if (target is Card)
            {
                if (propertyAccessNode.Property.Name == "Power")
                {
                    return ((Card)target).points;
                }
                if (propertyAccessNode.Property.Name == "Name")
                {
                    return ((Card)target).name;
                }
                if (propertyAccessNode.Property.Name == "Faction")
                {
                    return ((Card)target).faction;
                }
                if (propertyAccessNode.Property.Name == "Owner")
                {
                    return ((Card)target).player;
                }
            }
            //*! si no es una carta entonces es una de las listas del context y como es property son las que no tienen parametros 
            return ObjectsMapping[propertyAccessNode.Property.Name];
            
        }

        else
        {
            throw new Exception($"Variables no contiene el valor buscado: {target}");
        }

        throw new Exception("Se rompió el EvaluatePropertyAccess");
    }
    private object GetPropertyObject(PropertyAccessNode node)
    {
        object target = EvaluateExpression(node.Target);
        return target;
    }

    private Player GetTriggerPlayer()
    {
        if (TurnManager.Instance != null)
        {
            return TurnManager.Instance.GetCurrentPlayer();
        }
        return null;
    }
    private List<Card> GetTriggerPlayerHand()
    {
        if (TurnManager.Instance != null)
        {
            return TurnManager.Instance.GetCurrentPlayer().Hand;
        }
        return null;
    }
    private List<Card> GetOtherPlayerHand()
    {
        if (TurnManager.Instance != null)
        {
            return TurnManager.Instance.GetCurrentEnemy().Hand;
        }
        return null;
    }
    private List<Card> GetTriggerPlayerDeck()
    {
        if (TurnManager.Instance != null)
        {
            return TurnManager.Instance.GetCurrentPlayer().Deck;
        }
        return null;
    }
    private List<Card> GetOtherPlayerDeck()
    {
        if (TurnManager.Instance != null)
        {
            return TurnManager.Instance.GetCurrentEnemy().Deck;
        }
        return null;
    }
    private List<Card> GetTriggerPlayerField()
    {
        if (TurnManager.Instance != null)
        {
            return TurnManager.Instance.GetCurrentPlayer().Field;
        }
        return null;
    }
    private List<Card> GetOtherPlayerField()
    {
        if (TurnManager.Instance != null)
        {
            return TurnManager.Instance.GetCurrentEnemy().Field;
        }
        return null;
    }

    private List<Card> GetTriggerPlayerGraveyard()
    {
        if (TurnManager.Instance != null)
        {
            return TurnManager.Instance.GetCurrentPlayer().Graveyard;
        }
        return null;
    }
    private List<Card> GetOtherPlayerGraveyard()
    {
        if (TurnManager.Instance != null)
        {
            return TurnManager.Instance.GetCurrentEnemy().Graveyard;
        }
        return null;
    }

    private Player GetOwner(Card card)
    {
        throw new NotImplementedException();
    }

    private object EvaluateExpression(ExpressionNode expression)
    {
        switch (expression)
        {
            case Number numberNode:
                return numberNode.Value;
            
            case BinaryOperation binaryOperationNode:
                var leftValue = EvaluateExpression(binaryOperationNode.Left);
                var rightValue = EvaluateExpression(binaryOperationNode.Right);
                return EvaluateBinaryOperation(binaryOperationNode.Operator, leftValue, rightValue);
            
            case IdentifierNode identifierNode:
                return EvaluateIdentifier(identifierNode);
            
            case ExpressionMethodCall methodCallNode:
                object target = EvaluateExpression(methodCallNode.Target);
                if (methodCallNode.Param != null)
                {
                    return ExecuteMethod(methodCallNode.Funtion.Name,target,methodCallNode.Param);
                }
                return ExecuteMethod(methodCallNode.Funtion.Name,target);
            
            case PropertyAccessNode propertyAccessNode:
                return EvaluatePropertyAccess(propertyAccessNode);
            
            default:
                throw new NotSupportedException($"Unsupported expression type: {expression.GetType().Name}");
        }
    }
    private object EvaluateBinaryOperation(string op, object leftValue, object rightValue)
    {
        switch (op)
        {
            case "+":
                return (leftValue as double?) + (rightValue as double?);
            case "-":
                return (leftValue as double?) - (rightValue as double?);
            case "*":
                return (leftValue as double?) * (rightValue as double?);
            case "/":
                return (leftValue as double?) / (rightValue as double?);
            case "&&":
                return (bool)leftValue && (bool)rightValue;
            case "||":
                return (bool)leftValue || (bool)rightValue;
            case "==":
                return leftValue.Equals(rightValue);
            case "!=":
                return !leftValue.Equals(rightValue);
            case "<":
                return Convert.ToDouble(leftValue) < Convert.ToDouble(rightValue);
            case ">":
                return (leftValue as double?) > (rightValue as double?);
            case "<=":
                return (leftValue as double?) <= (rightValue as double?);
            case ">=":
                return (leftValue as double?) >= (rightValue as double?);
            default:
                throw new NotSupportedException($"Unsupported operator: {op}");
        }
    }
    private object EvaluateIdentifier(IdentifierNode identifierNode)
    {
        if (Variables.TryGetValue(identifierNode.Name, out var value))
        {
            return value;
        }
        else if (ObjectsMapping.TryGetValue(identifierNode.Name, out value))
        {
            return value;
        }
        else 
        {
           return identifierNode.Name;
        }
        throw new KeyNotFoundException($"Identifier not found: {identifierNode.Name}");
    }
}
