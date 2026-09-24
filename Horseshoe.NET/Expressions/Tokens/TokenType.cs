namespace Horseshoe.NET.Expressions.Tokens
{
    public enum TokenType
    {
        Undefined,

        // Individual tokens
        Number,    // e.g. 1, 2, 3, 4.5, -6.7, ⅓, ⅔, π, etc.
        String,    // e.g. "Hello World" or 'Hello World'
        Date,      // e.g. #5/6/2013#
        Word,      // e.g. keywords e.g. HighDate, etc., also function names e.g. Sin, And, If, etc., also object context properties e.g. Age from Employee instance
        Operator,  // e.g. +, -, *, /, ^, =, ≈, ≠, etc.
        Scope,     // i.e. (, ), ","
        Whitespace,// i.e. space, tab, etc. (all ignored)

        // Token containers
        Group,          // e.g. "(1 + 2) * 3" defines 2 sequences: "(1 + 2)" and "(1 + 2) * 3"
    }
}
