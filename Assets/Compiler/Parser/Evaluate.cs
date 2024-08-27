using System;
using System.Collections.Generic;
using System.Diagnostics;
using Engine;
using UnityEngine.Pool;

public class ExecutionVisitor : IASTVisitor
{
    private Dictionary<string,object> ObjectsMapping;

    public ExecutionVisitor(Dictionary<string,object> objectsMapping)
    {
        ObjectsMapping = objectsMapping;
        InicializedObjectsMapping();
    }

    public void InicializedObjectsMapping()
    {
        ObjectsMapping.Add("TriggerPlayer",GetTriggerPlayer());
        ObjectsMapping.Add("Hand",GetTriggerPlayerHand());
        ObjectsMapping.Add("Deck",GetTriggerPlayerDeck());
        ObjectsMapping.Add("Field",GetTriggerPlayerField());
        ObjectsMapping.Add("Graveyard",GetTriggerPlayerGraveyard());
        ObjectsMapping.Add("Board",Game.GameInstance.Board);
    }

    public void Visit(RootNode node)
    {
        // foreach (var effect in node.Effects)
        // {
        //     effect.Accept(this);
        // }
   
        foreach (var card in node.Cards)
        {
            card.Accept(this);
        }
    }

    public void Visit(CardNode node)
    {
        //crea una carta del juego real y añadela a AllCards
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

        if (node.PostAction != null)
        {
            node.PostAction.Accept(this);
        }
    }

    public void Visit(ActionNode node)
    {
        foreach (var statement in node.Statements)
        {
            statement.Accept(this);
        }
    }

    public void Visit(Assignment node)
    {
        var value = EvaluateExpression(node.Value);
        SetVariableValue(node.Variable, value);
    }

    public void Visit(MethodCallNode node)
    {
        ExecuteMethod(node);
    }

    public void Visit(ForStatement node)
    {
        var collection = EvaluateExpression(node.Iterable) as IEnumerable<Engine.Card>;
        foreach (var item in collection)
        {
            SetVariableValue(node.Variable, item);
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

    public void Visit(EffectField node)
    {
        throw new NotImplementedException();
    }

    public void Visit(CardParam node)
    {
        throw new NotImplementedException();
    }

    public void Visit(SelectorNode node)
    {
        throw new NotImplementedException();
    }

    public void Visit(MyPredicate node)
    {
        throw new NotImplementedException();
    }

    private object EvaluateExpression(ExpressionNode expression)
    {
        // Implementación de la evaluación de expresiones
        return null;
    }

    private void SetVariableValue(ExpressionNode variable, object value)
    {
        // Asignación de valores a las variables
    }

    private void ExecuteMethod(MethodCallNode node)
    {
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
    private List<Card> GetTriggerPlayerDeck()
    {
        if (TurnManager.Instance != null)
        {
            return TurnManager.Instance.GetCurrentPlayer().Deck;
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
    private List<Card> GetTriggerPlayerGraveyard()
    {
        if (TurnManager.Instance != null)
        {
            return TurnManager.Instance.GetCurrentPlayer().Graveyard;
        }
        return null;
    }

}
