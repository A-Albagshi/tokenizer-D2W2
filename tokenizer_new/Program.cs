using System;
using System.Collections.Generic;

namespace tokenizer_new
{

    public class Token
    {
        public string type;
        public string value;
        public int position;
        public int lineNumber;
    }

    public class Tokenizer
    {
        public string input;
        public int currentPosition;
        public int lineNumber;

        public Tokenizer(string input)
        {
            this.input = input;
            this.currentPosition = -1;
            this.lineNumber = 1;
        }

        public char peek()
        {
            if (this.hsaMore())
            {
                return this.input[this.currentPosition + 1];
            }
            else
            {
                return '\0';
            }

        }

        public char next()
        {
            char currentChar = this.input[++this.currentPosition];
            if (currentChar == '\n')
            {
                this.lineNumber++;
            }
            return currentChar;

        }

        public bool hsaMore()
        {
            return (this.currentPosition + 1) < this.input.Length;
        }

        public Token tokinze(Tokenizable[] handlers)
        {
            foreach (var t in handlers)
            {
                if (t.tokenizable(this))
                {
                    return t.tokeinze(this);
                }
            }
            /*throw new Exception("Unexpected Tokens");*/
            return null;
        }
    }

    public abstract class Tokenizable
    {
        public abstract bool tokenizable(Tokenizer tokenizer);
        public abstract Token tokeinze(Tokenizer tokenizer);
    }



    public class NumberTokenzier : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hsaMore() && Char.IsDigit(t.peek());
        }
        public override Token tokeinze(Tokenizer t)
        {
            Token token = new Token();
            token.type = "number";
            token.value = "";
            token.position = t.currentPosition;
            token.lineNumber = t.lineNumber;

            while (t.hsaMore() && Char.IsDigit(t.peek()))
            {
                token.value += t.next();
            }
            return token;

        }
    }

    public class IdTokenzier : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hsaMore() && (Char.IsLetter(t.peek()) || t.peek() == '_');
        }
        public override Token tokeinze(Tokenizer t)
        {
            Token token = new Token();
            token.type = "id";
            token.value = "";
            token.position = t.currentPosition;
            token.lineNumber = t.lineNumber;

            while (t.hsaMore() && (Char.IsLetterOrDigit(t.peek()) || t.peek() == '_'))
            {
                token.value += t.next();
            }
            return token;

        }
    }

    public class WhiteSpaceTokenzier : Tokenizable
    {
        public override bool tokenizable(Tokenizer t)
        {
            return t.hsaMore() && Char.IsWhiteSpace(t.peek());
        }
        public override Token tokeinze(Tokenizer t)
        {
            Token token = new Token();
            token.type = "white space";
            token.value = "";
            token.position = t.currentPosition;
            token.lineNumber = t.lineNumber;

            while (t.hsaMore() && Char.IsWhiteSpace(t.peek()))
            {
                token.value += t.next();
            }
            return token;

        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            string case1 = "_id ali 123 - a3l 924 ";
            string case2 = "123 924 ";

            Tokenizer t = new Tokenizer(case1);
            Tokenizable[] handlers = new Tokenizable[] { new IdTokenzier(), new NumberTokenzier(), new WhiteSpaceTokenzier() };
            Token token = t.tokinze(handlers);

            while (token != null)
            {
                Console.WriteLine("the value is " + token.value + " the type is " + token.type);
                token = t.tokinze(handlers);
            }

        }
    }

    /*static List<string> tokenizer(string input)
    {
        if (input == null || input.Trim().Length == 0)
            return null;

        List<string> tokens = new List<string>();

        int i = 0;
        string token = null;

        while (i < input.Length)
        {
            token = "";
            if (Char.IsLetter(input[i]) || input[i] == '_')
            {
                token += input[i++];

                while ((i < input.Length) && (Char.IsLetterOrDigit(input[i]) || input[i] == '_'))
                {
                    token += input[i++];
                }
                tokens.Add(token);
                continue;
            }
            else if (Char.IsDigit(input[i]))
            {
                token += input[i++];
                while ((i < input.Length) && (Char.IsDigit(input[i])))
                {
                    token += input[i++];
                }
                tokens.Add(token);
                continue;

            }
            else if (Char.IsWhiteSpace(input[i]))
            {
                token += input[i++];
                while ((i < input.Length) && (Char.IsWhiteSpace(input[i])))
                {
                    token += input[i++];
                }
                tokens.Add(token);
                continue;
            }
        }
        i++;
        return tokens;
    }*/
}


