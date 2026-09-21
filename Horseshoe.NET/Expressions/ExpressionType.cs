using System;
using System.Collections.Generic;
using System.Text;

namespace Horseshoe.NET.Expressions
{
    public enum ExpressionType
    {
        Undefined,

        // Individual expressions
        TokenNumber,    // e.g. 1, 2, 3, 4.5, -6.7, ⅓, ⅔, π, etc.
        TokenString,    // e.g. "Hello World" or 'Hello World'
        TokenKeyword,   // e.g. "sin", "cos", "and", "or", etc., also function names
        TokenOperator,  // e.g. +, -, *, /, ^, =, ≈, ≠, etc.

        // Expression containers
        Sequence,       // e.g. "(1 + 2) * 3" defines 2 sequences: "(1 + 2)" and "(1 + 2) * 3"
        SequenceOpen,   // open paren
        SequenceClose,  // close paren
        Function,       // defined by TokenKeyword and SequenceOpen/SequenceClose
        FunctionArgSeperator, // comma
    }
}
