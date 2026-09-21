using System;
using System.Collections.Generic;
using System.Text;

namespace Horseshoe.NET.Expressions
{
    public enum TokenType
    {
        Undefined,

        // Individual expressions
        Number,    // e.g. 1, 2, 3, 4.5, -6.7, ⅓, ⅔, π, etc.
        String,    // e.g. "Hello World" or 'Hello World'
        Date,      // e.g. #5/6/2013#
        Word,      // e.g. keywords e.g. HighDate, etc., also function names e.g. Sin, And, If, etc., also object context properties e.g. Age from Employee instance
        Operator,  // e.g. +, -, *, /, ^, =, ≈, ≠, etc.
        Scope,     // i.e. (, ), ","
        Whitespace,// i.e. (, ), ","

        // Expression containers
        Sequence,       // e.g. "(1 + 2) * 3" defines 2 sequences: "(1 + 2)" and "(1 + 2) * 3"
        SequenceOpen,   // open paren
        SequenceClose,  // close paren
        Function,       // defined by TokenKeyword and SequenceOpen/SequenceClose e.g. AND(_, _)
        FunctionArgSeparator, // comma
    }
}
