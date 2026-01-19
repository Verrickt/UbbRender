using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UbbRender.Common;

public class Tokenizer
{
    private readonly string _input;
    private int _position;
    private readonly List<Token> _tokens = new();

    public Tokenizer(string input)
    {
        _input = input ?? string.Empty;
    }

    public List<Token> Tokenize()
    {
        _position = 0;
        _tokens.Clear();

        while (_position < _input.Length)
        {
            var currentChar = _input[_position];

            if (currentChar == '[')
            {
                ProcessTag();
            }
            else if (currentChar == '\n' || currentChar == '\r')
            {
                ProcessNewLine();
            }
            else
            {
                ProcessText();
            }
        }

        return _tokens;
    }
    private void ProcessNewLine()
    {
        var start = _position;

        // 检查是否是 \r\n 组合
        if (_position < _input.Length - 1 &&
            _input[_position] == '\r' &&
            _input[_position + 1] == '\n')
        {
            _position += 2; // 跳过 \r\n
            _tokens.Add(new Token(TokenType.NewLine, "\r\n", start));
        }
        else if (_input[_position] == '\r')
        {
            _position++; // 跳过 \r
            _tokens.Add(new Token(TokenType.NewLine, "\r", start));
        }
        else // \n
        {
            _position++; // 跳过 \n
            _tokens.Add(new Token(TokenType.NewLine, "\n", start));
        }
    }
    private void ProcessText()
    {
        var start = _position;

        while (_position < _input.Length)
        {
            var currentChar = _input[_position];

            if (currentChar == '[' || currentChar == '\n' || currentChar == '\r')
                break;

            _position++;
        }

        if (_position > start)
        {
            var text = _input.Substring(start, _position - start);
            _tokens.Add(new Token(TokenType.Text, text, start));
        }
    }

    private void ProcessTag()
    {
        var start = _position;
        _position++; // 跳过 '['

        // 检查是否为闭合标签
        bool isCloseTag = false;
        if (_position < _input.Length && _input[_position] == '/')
        {
            isCloseTag = true;
            _position++;
        }

        // 查找标签结束位置
        while (_position < _input.Length)
        {
            var currentChar = _input[_position];

            if (currentChar == ']')
            {
                var tagContent = _input.Substring(start + 1, _position - start - 1);

                // 检查是否为自闭合标签
                bool isSelfClose = tagContent.EndsWith("/");
                if (isSelfClose)
                {
                    tagContent = tagContent.Substring(0, tagContent.Length - 1);
                }

                _position++; // 跳过 ']'

                if (!isCloseTag && !isSelfClose && IsEmoticonTag(tagContent))
                {
                    // 表情标签作为自闭合标签处理
                    _tokens.Add(new Token(TokenType.SelfCloseTag, tagContent, start));
                }
                else
                {
                    var tokenType = isCloseTag ? TokenType.CloseTag :
                                   isSelfClose ? TokenType.SelfCloseTag : TokenType.OpenTag;
                    _tokens.Add(new Token(tokenType, tagContent.Trim('/'), start));
                }
                return;
            }

            _position++;
        }

        // 如果没有找到 ']'，则将其视为普通文本
        _position = start;
        ProcessText();
    }
    // 检查是否是表情标签
    private bool IsEmoticonTag(string tagContent)
    {
        // 检查是否包含空格（有属性就不是表情标签）
        if (tagContent.Contains(' '))
            return false;

        return EmoticonRules.IsEmoticonTag(tagContent);
    }
}

