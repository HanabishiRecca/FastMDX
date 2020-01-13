using System;

namespace FastMDX {
    class ParsingException : Exception {
        public override string Message => "Parsing error.";
    }

    class NameCantBeEmptyException : Exception {
        public override string Message => "Name can't be empty.";
    }
}
