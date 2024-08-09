public class Token
{
    public string Value { get; private set; }
    public TokenType Type { get; private set; }
    public CodeLocation Location { get; private set; }
    public Token(TokenType type, string value, CodeLocation location)
    {
        this.Type = type;
        this.Value = value;
        this.Location = location;
    }

    public override string ToString() => $"{Type}: {Value}";
}

public struct CodeLocation
{
    public string File;
    public int Line;
    public int Column;
}


public enum TokenType
{
    Unknwon,
    Number,
    Text,
    Type, // para string, bool, int
    Keyword,
    Identifier,
    Symbol,
    Comma,
    ClosedCurlyBrace,
    Dot,
    CardType,
}

public class TokenValues
{
    protected TokenValues() { }

    public const string Add = "+"; // +
    public const string Sub = "-"; // -
    public const string Mul = "*"; // *
    public const string Div = "/"; // /

    public const string And = "&&";
    public const string Or = "||";

    public const string LessThan = "<";
    public const string GreaterThan = ">";
    public const string LessEqualThan = "<=";
    public const string GreaterEqualThan = ">=";
    public const string LogicEqual = "==";

    public const string Concatenation = "@";
    public const string ConcatenationWithSpace = "@@";
              

    public const string Assign = "="; // =
    public const string Increment = "++";
    public const string DeIncrement = "--";
    public const string AddAssign = "+=";
    public const string SubstAssign = "-=";
    public const string MultAssign = "*=";
    public const string DivideAssign = "/=";
    public const string Dot = "."; 
    public const string ValueSeparator = ","; 
    public const string StatementSeparator = ";"; 
    public const string Colon = ":"; 
    public const string Arrow = "=>";
    
    public const string OpenBracket = "("; // (
    public const string ClosedBracket = ")"; // )
    public const string ClosedSquareBraket = "]";
    public const string OpenSquareBraket = "[";
    public const string OpenCurlyBraces = "{"; // {
    public const string ClosedCurlyBraces = "}"; // }

    public const string For = "for"; 
    public const string While = "while"; 
    public const string card = "card"; 
    public const string effect = "effect"; 
    public const string Name = "Name"; 
    public const string Params = "Params"; 
    public const string stringType = "String"; 
    public const string boolType = "Bool"; 
    public const string False = "false"; 
    public const string True = "true"; 
    public const string intType = "Number"; 
    public const string id = "id"; 
    public const string Action = "Action"; 
    public const string targets = "targets"; 
    public const string Context = "Context"; 
    public const string TriggerPlayer = "TriggerPlayer"; 
    public const string Board = "Board"; 
    public const string HandOfPlayer = "HandOfPlayer"; 
    public const string FieldOfPlayer = "FieldOfPlayer"; 
    public const string GraveyardOfPlayer = "GraveyardOfPlayer"; 
    public const string DeckOfPlayer = "DeckOfPlayer"; 
    public const string Deck = "Deck"; 
    public const string Field = "Field"; 
    public const string Graveyard = "Graveyard"; 
    public const string Owner = "Owner"; 
    public const string Find = "Find"; 
    public const string Push = "Push"; 
    public const string SendBottom = "SendBottom"; 
    public const string Pop = "Pop"; 
    public const string Remove = "Remove"; 
    public const string Shuffle = "Shuffle"; 
    public const string Type = "Type"; 
    public const string Faction = "Faction"; 
    public const string Power = "Power"; 
    public const string Range = "Range"; 
    public const string OnActivation = "OnActivation"; 
    public const string Selector = "Selector"; 
    public const string Source = "Source"; 
    public const string hand = "hand";   
    public const string otherHand = "otherHand"; 
    public const string deck = "deck"; 
    public const string otherDeck = "otherDeck"; 
    public const string field = "field";   
    public const string otherField = "otherField";
    public const string parent = "parent";
    public const string Single = "Single";
    public const string Predicate = "Predicate";
    public const string PostAction = "PostAction";  
    public const string Oro = "Oro"; 

}
