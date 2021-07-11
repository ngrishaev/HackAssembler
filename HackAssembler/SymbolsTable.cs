﻿using System;
using System.Collections.Generic;

namespace HackAssembler
{
    public class SymbolsTable
    {
        private Dictionary<string, int> _table;
        private readonly SymbolParser _symbolParser;

        public SymbolsTable(IEnumerable<string> codeLines)
        {
            _table = GetDefaultSymbols();
            _symbolParser = new SymbolParser();
            ParseCodeForLabelSymbols(codeLines, _table);

            foreach (var pair in _table)
            {
                Console.WriteLine($"Symbol: {pair.Key}. Value: {pair.Value}");
            }
        }

        private void ParseCodeForLabelSymbols(IEnumerable<string> codeLines, Dictionary<string, int> symbolsTable)
        {
            var lineIndex = 0;
            foreach (var codeLine in codeLines)
            {
                if (_symbolParser.IsLabelSymbol(codeLine))
                {
                    var symbol = _symbolParser.GetLabelSymbol(codeLine);
                    AddLabelSymbol(symbol, lineIndex);
                    continue;
                }
                
                lineIndex++;
            }
        }

        private void AddLabelSymbol(string symbol, int lineIndex)
        {
            if (_table.ContainsKey(symbol) == false)
            {
                _table[symbol] = lineIndex;
            }
            else
            {
                throw new FormatException($"{symbol} label duplicate at line {lineIndex}");
            }
        }

        private Dictionary<string, int> GetDefaultSymbols() =>
            new Dictionary<string, int>
            {
                {"R0", 0},
                {"R1", 1},
                {"R2", 2},
                {"R3", 3},
                {"R4", 4},
                {"R5", 5},
                {"R6", 6},
                {"R7", 7},
                {"R8", 8},
                {"R9", 9},
                {"R10", 10},
                {"R11", 11},
                {"R12", 12},
                {"R13", 13},
                {"R14", 14},
                {"R15", 15},
                {"SCREEN", 16384},
                {"KBD", 24576},
                {"SP", 0},
                {"LCL", 1},
                {"ARG", 2},
                {"THIS", 4},
                {"THAT", 5},
            };
    }
    
    public class SymbolParser{
        
        public bool IsLabelSymbol(string codeLine) => 
            codeLine[0] == '(' && codeLine[^1] == ')';
        
        public bool IsVariableSymbol(string codeLine)
        {
            if (codeLine[0] != '@')
                return false;
            
            if (int.TryParse(codeLine.Substring(1), out _))
                return false;
            
            return true;
        }

        public string GetLabelSymbol(string codeLine) => 
            codeLine.Substring(1, codeLine.Length - 2);
        
        public string GetVariableSymbol(string codeLine) => 
            codeLine.Substring(1, codeLine.Length - 1);
    }
}