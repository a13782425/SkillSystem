using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSSkill
{
    public static class SkillParser
    {
        private const string SKILL = "Skill";
        private const string SKILL_START = "{";
        private const string SKILL_End = "}";

        #region 表征类型
        private enum SkillIdTokenType
        {
            Identifier,
            SkillId,
            SkillData,
        }

        #endregion

        private static int _currentLength = 0;
        private static string _codeStr = "";
        private static int _codeLength = 0;


        public static Dictionary<int, string> ParserSkillIds(string data)
        {
            Dictionary<int, string> tempDic = new Dictionary<int, string>();
            _codeStr = data;
            _codeLength = data.Length;
            _currentLength = 0;

            int skillId = 0;
            SkillIdTokenType tokenType = SkillIdTokenType.Identifier;
            string str = "";
            string skillStr = "";
            bool isLeftBrace = false;

            while (IsCanRead())
            {
                char c = ReadChar();
                switch (tokenType)
                {
                    case SkillIdTokenType.SkillId:
                        if (IsNumber(c))
                        {
                            str += c;
                        }
                        else
                        {
                            RecoverLength();
                            tokenType = SkillIdTokenType.Identifier;
                            continue;
                        }
                        break;
                    case SkillIdTokenType.SkillData:
                        if (c == '}')
                        {
                            RecoverLength();
                            tokenType = SkillIdTokenType.Identifier;
                            continue;
                        }
                        skillStr += c;
                        break;
                    case SkillIdTokenType.Identifier:
                        switch (c)
                        {
                            case '(':
                                if (str == SKILL)
                                {
                                    tokenType = SkillIdTokenType.SkillId;
                                    str = "";
                                }
                                break;
                            case ')':
                                if (string.IsNullOrEmpty(str))
                                {
                                    throw new Exception("技能ID为空");
                                }
                                if (int.TryParse(str, out skillId))
                                {
                                    str = "";
                                }
                                else
                                {
                                    throw new Exception("技能ID错误");
                                }
                                break;
                            case '{':
                                tokenType = SkillIdTokenType.SkillData;
                                skillStr = "";
                                isLeftBrace = true;
                                break;
                            case '}':
                                if (!isLeftBrace)
                                {
                                    throw new Exception("技能文件被修改,'{'消失");
                                }
                                if (tempDic.ContainsKey(skillId))
                                {
                                    throw new Exception("技能Id:" + skillId + "的技能已经存在");
                                }
                                tempDic.Add(skillId, skillStr);
                                skillId = 0;
                                skillStr = "";
                                str = "";
                                if (IsCanRead())
                                {
                                    if (ReadChar() != ',')
                                    {
                                        goto End;
                                    }
                                }
                                break;
                            case '\r':
                            case '\n':
                            case '\t':
                                break;
                            default:
                                str += c;
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            End: return tempDic;
        }
















        private static bool IsNumber(char ch)
        {
            int i = (int)ch;
            return i > 47 && i < 58;
        }

        private static bool IsCanRead()
        {
            return _currentLength < _codeLength && _currentLength >= 0;
        }

        private static char ReadChar()
        {
            if (string.IsNullOrEmpty(_codeStr))
            {
                throw new Exception("代码为空!");
            }
            if (_currentLength >= _codeLength || _currentLength < 0)
            {
                throw new Exception("当前长度大于代码长度!");
            }
            char c = _codeStr[_currentLength];
            _currentLength++;
            return c;
        }
        /// <summary>
        /// 恢复已读取长度
        /// </summary>
        private static void RecoverLength()
        {
            _currentLength--;
        }
    }
}
