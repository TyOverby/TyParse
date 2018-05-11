using System;
using System.Collections.Immutable;

namespace TyParse
{

    public delegate bool TryParseDelegate<T>(string input, out T output);

    public static class Combinators
    {
        internal class XorImpl<T> : Parser<T>
        {
            private readonly Parser<T>[] parsers;

            public XorImpl(Parser<T>[] parsers)
            {
                this.parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
            }

            public Result<T> Parse(ImmutableArray<string> input)
            {
                var errors = new System.Collections.Generic.List<string>();
                var duplicateErrors = "";
                bool duplicate = false;
                Ok<T> found = null;

                foreach (var parser in parsers)
                {
                    switch (parser.Parse(input))
                    {
                        case Ok<T> ok:
                            if (found is null)
                            {
                                found = ok;
                            }
                            else if (duplicate)
                            {
                                duplicateErrors += $" vs {ok.Value}";
                            }
                            else
                            {
                                duplicate = true;
                                duplicateErrors = $"Multiple possible parses detected {found.Value} vs {ok.Value}";
                            }
                            break;
                        case Err<T> err:
                            errors.Add(err.Message);
                            break;
                    }
                }

                if (duplicate)
                {
                    return new Err<T>(duplicateErrors, input);
                }
                if (found is null)
                {
                    return new Err<T>(String.Join('\n', errors), input);
                }

                return found;
            }
        }
        internal class OrImpl<T> : Parser<T>
        {
            private readonly Parser<T>[] parsers;

            public OrImpl(Parser<T>[] parsers)
            {
                this.parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
            }

            public Result<T> Parse(ImmutableArray<string> input)
            {
                var errors = new System.Collections.Generic.List<string>();
                foreach (var parser in parsers)
                {
                    switch (parser.Parse(input))
                    {
                        case Ok<T> ok:
                            return ok;
                        case Err<T> err:
                            errors.Add(err.Message);
                            break;
                    }
                }
                return new Err<T>(String.Join('\n', errors), input);
            }
        }

        public static Parser<R> Select<T, R>(this Parser<T> parser, Func<T, R> mapper) =>
            new MapImpl<T, R>(parser, mapper);

        public static Parser<R> Bind<T, R>(this Parser<T> parser, Func<T, Func<R, Result<R>>, Func<string, Result<R>>, Result<R>> binder) =>
            new BindImpl<T, R>(parser, binder);

        public static Parser<T> TryParse<T>(this Parser<string> parser, TryParseDelegate<T> tpd, string typeName) =>
            new TryParseBinderImpl<T>(parser, tpd, typeName);

        public static Parser<T> Or<T>(params Parser<T>[] parsers) => new OrImpl<T>(parsers);

        public static Parser<T> Xor<T>(params Parser<T>[] parsers) => new XorImpl<T>(parsers);

        public static Parser<(A, B)> And<A, B>(Parser<A> a, Parser<B> b) =>
            new AndImpl<A, B>(a, b);

        public static Parser<(A, B, C)> And<A, B, C>(Parser<A> a, Parser<B> b, Parser<C> c) =>
            And(And(a, b), c).Select(t => (t.Item1.Item1, t.Item1.Item2, t.Item2));

        public static Parser<(A, B, C, D)> And<A, B, C, D>(Parser<A> a, Parser<B> b, Parser<C> c, Parser<D> d) =>
            And(And(a, b, c), d).Select(t => (t.Item1.Item1, t.Item1.Item2, t.Item1.Item3, t.Item2));

        public static Parser<(A, B, C, D, E)> And<A, B, C, D, E>(Parser<A> a, Parser<B> b, Parser<C> c, Parser<D> d, Parser<E> e) =>
            And(And(a, b, c, d), e).Select(t => (t.Item1.Item1, t.Item1.Item2, t.Item1.Item3, t.Item1.Item4, t.Item2));
    }
}
