using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MartianRobots.Common.Domain;
using MartianRobots.Common.Interfaces;

namespace MartianRobots
{
    internal partial class World
    {
        private class Parser : IDisposable
        {
            private readonly Lexer _lexer;
            private IEnumerator<string[]> _tokenIterator;
            private readonly Dictionary<char, Func<IEnumerator<string[]>, Instruction>> _dispatcher;

            public Parser(IRobotInstructionHandler[] instructionHandlers)
            {
                var defs = new TokenDefinition[]
                {
                    new TokenDefinition(@"(([-+]?[1-9][0-9]*)|0)","int"),
                    new TokenDefinition(@"[a-zA-Z]","char")
                };

                _lexer = new Lexer(defs);
            
                _dispatcher = new Dictionary<char, Func<IEnumerator<string[]>, Instruction>>();
                foreach (var rc in instructionHandlers)
                {
                    _dispatcher.Add(rc.Key, rc.ParseArguments);
                }
            }

            public IReadOnlyList<Instruction> Parse(string line, bool genericParse)
            {
                var commands = new List<Instruction>();
                _tokenIterator = _lexer.Process(line).GetEnumerator();
                _tokenIterator.MoveNext();

                while (_tokenIterator.Current[0] != "EOF")
                {
                    if (genericParse)
                    {
                        commands.Add(GenericRule(_tokenIterator));
                    }
                    else if (_tokenIterator.Current[0] == "char")
                    {
                        if (_dispatcher.TryGetValue(_tokenIterator.Current[1][0], out var rule))
                        {
                            commands.Add(rule(_tokenIterator));
                        }
                    }
                }

                return commands;
            }

            private Instruction GenericRule(IEnumerator<string[]> tokenIterator)
            {
                var arguments = new List<string>();
                while (tokenIterator.Current[0] != "EOF")
                {
                    arguments.Add(_tokenIterator.Current[1]);
                    _tokenIterator.MoveNext();
                }

                return new Instruction('$', arguments);
            }

            public void Dispose()
            {
                _tokenIterator?.Dispose();
            }
        }


        private sealed class TokenDefinition
        {
            public readonly string Matcher;
            public readonly string Token;

            public TokenDefinition(string regex, string token)
            {
                this.Matcher = regex;
                this.Token = token;
            }
        }

        private sealed class Lexer
        {
            private readonly Regex regex;
            private readonly List<string> groupNames;

            public Lexer(TokenDefinition[] tokenDefinitions)
            {
                groupNames = new List<string>();
                var pattern = new List<string>();
                foreach (var def in tokenDefinitions)
                {
                    groupNames.Add(def.Token);
                    pattern.Add($@"(?<{def.Token}>{def.Matcher})");
                }

                regex = new Regex(string.Join("|", pattern));
            }

            public IEnumerable<string[]> Process(string inputString)
            {
                var localString = inputString;
                var position = 0;
                var totalLenght = localString.Length;
                var len = localString.Length;
                while (true)
                {
                    var match = regex.Match(localString, position, len);
                    if (match.Success)
                    {
                        foreach (var groupName in groupNames)
                        {
                            var grp = match.Groups[groupName];
                            if (grp.Success)
                            {
                                position = match.Index + match.Length;
                                len = totalLenght - position;
                                yield return new string[] { groupName, match.ToString() };
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                yield return new string[] { "EOF", "" };
            }
        }
    }
}
