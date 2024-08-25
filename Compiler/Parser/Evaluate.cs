public class ExecutionVisitor : IASTVisitor
{
    private Context context;
    private Card targetCard;

    public ExecutionVisitor(Context context, Card targetCard)
    {
        this.context = context;
        this.targetCard = targetCard;
    }

    public void Visit(RootNode node)
    {
        foreach (var effect in node.Effects)
        {
            effect.Accept(this);
        }

        foreach (var card in node.Cards)
        {
            card.Accept(this);
        }
    }

    public void Visit(CardNode node)
    {
        foreach (var effect in node.EffectList)
        {
            effect.Accept(this);
        }
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
        var collection = EvaluateExpression(node.Iterable) as IEnumerable<Card>;
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

}
