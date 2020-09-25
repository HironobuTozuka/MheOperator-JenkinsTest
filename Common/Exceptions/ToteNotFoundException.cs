using System;
using System.Collections.Generic;

namespace Common.Exceptions
{
    public class ToteNotFoundException : Exception
    {
        private readonly string _notFoundTote;

        public ToteNotFoundException(string notFoundTote)
        {
            _notFoundTote = notFoundTote;
        }

        public override string ToString()
        {
            return $"Tote not found: {_notFoundTote}";
        }
    }
}