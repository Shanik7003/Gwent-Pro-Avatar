using System;
using System.Collections.Generic;
using System.Diagnostics;
using Engine;
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
        ObjectsMapping.Add("deck",GetTriggerPlayerHand());
        ObjectsMapping.Add("otherDeck",GetOtherPlayerHand());
        ObjectsMapping.Add("field",GetTriggerPlayerHand());
        ObjectsMapping.Add("otherField",GetOtherPlayerHand());
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

    public void Visit(Assignment node)
    {
        var value = EvaluateExpression(node.Value);
        var  variable = EvaluateExpression(node.Variable);
        if (Variables.ContainsValue(variable) || ObjectsMapping.ContainsValue(variable))//si ya lo contiene pues cambia el valor del objecto
        {
            variable = value;     
        }
        else//si no contiene y la estas creando ahora entonces que el Evaluate de identifier devuelva un string y añade la nueva variable a Variables 
        {
            Variables[(string)variable] = value;
        }
    }

    public void Visit(MethodCallNode node)
    {
        object target = EvaluateExpression(node.Target);//TODO confio en que el target de los methodsCall siempre sea por referencia 
        string method = node.Funtion.Name;
        if (node.Param != null)
        {
            object param = EvaluateExpression(node.Param);
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
        throw new NotImplementedException();
    }

    public void Visit(EffectNode node)
    {
        node.Action.Accept(this);
        throw new NotImplementedException();
    }

    public void Visit(ParamNode node)
    {
        throw new NotImplementedException();
    }

    public void Visit(BinaryOperation node)
    {
        throw new NotImplementedException();
    }

    public void Visit(IdentifierNode node)
    {
        throw new NotImplementedException();
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

    private object EvaluateExpression(ExpressionNode expression)
    {
        throw new NotImplementedException();
    }

    private void SetVariableValue(ExpressionNode variable, object value)
    {
        // Asignación de valores a las variables
    }

    private void ExecuteMethod(string method, object target, object param = null)
    {
        switch (method)
        {
            case "Find":
            case "Push":
            case "SendBottom":
            case "Pop":
            case "Remove":
            case "Shuffle":
            case "HandOfPlayer":
            case "FieldOfPlayer":
            case "GraveyardOfPlayer":
            case "DeckOfPlayer":

            default:
                break;
        }
        // Implementación de la ejecución de métodos
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

}
