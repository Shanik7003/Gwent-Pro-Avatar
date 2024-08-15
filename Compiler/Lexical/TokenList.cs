
using System;
using System.Collections;
using System.Collections.Generic;

/* This List has functions to operate over a list of tokens.
The methods are simple, you can understand them easily */
public class TokenList : IEnumerable<Token>
{
    private List<Token> tokens;
    private int position;
    public int Position { get { return position; } }
    public bool End => position == tokens.Count-1;

    public TokenList(IEnumerable<Token> tokens)
    {
        this.tokens = new List<Token>(tokens);
        position = -1;
    }
    public bool MatchActualToken(string value)
    {
        if (tokens[Position].Value == value)
        {
            return true;
        }
        return false;
    }
     /*si existe una NextPosition la consume (actualiza position) */
    public bool Next()
    {
        if (CanLookAhead())
        {
            // System.Console.WriteLine($"Consumido {tokens[Position].Value}");
            position++;
        }

        return position < tokens.Count;
    }

    /*the next position must match the given type */
    public bool MatchType( TokenType type )
    {
        if (CanLookAhead() && LookAhead().Type == type)
        {
            position++;
            return true;
        }

        return false;
    }

    /* In this case, the next position must match the given value */
    public bool MatchValue(string value)
    {            
        if (CanLookAhead() && LookAhead().Value == value)
        {
            position++;
            return true;
        }

        return false;
    }
    public bool Match(Token token)
    {
        if (CanLookAhead() && LookAhead() == token)
        {
            position++;
            return true;
        }
        return false;
    }

    public bool CanLookAhead()
    {
        return tokens.Count - position > 1;
    }

    public Token LookAhead()
    {
        if(CanLookAhead())
            return tokens[position + 1];
        throw new Exception("Can't lookahead");
    }
    
    public Token Expect(string expectedValue)
    {
        var token = LookAhead();
        if (token.Value != expectedValue)
        {
            throw new Exception($"Expected '{expectedValue}' but found '{token.Value}'");
        }
        Next(); // Consume the token
        return token;
    }

    public Token Expect(TokenType expectedType)
    {
        var token = LookAhead();
        if (token.Type != expectedType)
        {
            throw new Exception($"Expected token of type '{expectedType}' but found '{token.Type}'");
        }
        Next(); // Consume the token
        return token;
    }

    public IEnumerator<Token> GetEnumerator()
    {
        for (int i = position; i < tokens.Count; i++)
            yield return tokens[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

