﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Configuration.Mapping.Model;
using MongoDB.Configuration.Mapping;
using System.Text;
using MongoDB.Bson;

namespace MongoDB.Serialization.Descriptors
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class ClassMapPropertyDescriptorBase : IPropertyDescriptor
    {
        private readonly IMappingStore _mappingStore;
        private readonly JavascriptMemberNameReplacer _codeReplacer;
        /// <summary>
        /// 
        /// </summary>
        protected readonly IClassMap ClassMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassMapPropertyDescriptorBase"/> class.
        /// </summary>
        /// <param name="mappingStore">The mapping store.</param>
        /// <param name="classMap">The class map.</param>
        protected ClassMapPropertyDescriptorBase(IMappingStore mappingStore, IClassMap classMap)
        {
            if (mappingStore == null)
                throw new ArgumentNullException("mappingStore");
            if (classMap == null)
                throw new ArgumentNullException("classMap");

            _mappingStore = mappingStore;
            ClassMap = classMap;
            _codeReplacer = new JavascriptMemberNameReplacer(_mappingStore);
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<BsonProperty> GetProperties();

        /// <summary>
        /// Creates the property.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="value">The value.</param>
        /// <param name="isDictionary">if set to <c>true</c> [is dictionary].</param>
        /// <returns></returns>
        protected BsonProperty CreateProperty(string alias, Type valueType, object value, bool isDictionary)
        {
            return CreateProperty(alias, new BsonPropertyValue(valueType, value, isDictionary));
        }

        /// <summary>
        /// Creates the property.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        protected BsonProperty CreateProperty(string alias, BsonPropertyValue value)
        {
            return new BsonProperty(alias) { Value = value };
        }

        /// <summary>
        /// Shoulds the persist discriminator.
        /// </summary>
        /// <returns></returns>
        protected bool ShouldAddDiscriminator()
        {
            return ClassMap.IsSubClass;
        }

        /// <summary>
        /// Gets the name of the alias from member.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        protected MemberMapAndAlias GetAliasFromMemberName(string name)
        {
            var memberMap = ClassMap.GetMemberMapFromMemberName(name);
            if (memberMap != null)
                return new MemberMapAndAlias() { MemberMap = memberMap, Alias = memberMap.Alias };

            if (!name.Contains("."))
                return new MemberMapAndAlias() { Alias = name };

            var sb = new StringBuilder();

            var parts = name.Split('.');
            memberMap = ClassMap.GetMemberMapFromMemberName(parts[0]);
            if (memberMap == null)
                return new MemberMapAndAlias() { Alias = name };

            sb.Append(memberMap.Alias);
            var currentType = memberMap.MemberReturnType;
            for (int i = 1; i < parts.Length; i++)
            {
                if(memberMap != null)
                {
                    var collectionMemberMap = memberMap as CollectionMemberMap;
                    if (collectionMemberMap != null)
                    {
                        currentType = ((CollectionMemberMap)memberMap).ElementType;
                        if (IsNumeric(parts[i])) //we are an array indexer
                        {
                            sb.Append(".").Append(parts[i]);
                            continue;
                        }
                    }

                    var classMap = _mappingStore.GetClassMap(currentType);
                    memberMap = classMap.GetMemberMapFromMemberName(parts[i]);
                }

                if (memberMap == null)
                    sb.Append(".").Append(parts[i]);
                else
                {
                    sb.Append(".").Append(memberMap.Alias);
                    currentType = memberMap.MemberReturnType;
                }
            }

            return new MemberMapAndAlias() { MemberMap = memberMap, Alias = sb.ToString() };
        }

        protected string TranslateJavascript(string code)
        {
            return _codeReplacer.Replace(code, ClassMap);
        }

        private static bool IsNumeric(IEnumerable<char> str)
        {
            return str.All(t => char.IsDigit(t));
        }

        protected class MemberMapAndAlias
        {
            public string Alias { get; set; }

            public PersistentMemberMap MemberMap { get; set; }
        }

        /// <summary>
        /// This is an extremely rudimentary lexer designed solely for efficiency.
        /// </summary>
        private class JavascriptMemberNameReplacer
        {
            private const char EOF = '\0';
            private readonly IMappingStore _mappingStore;
            private IClassMap _classMap;
            private string _input;
            private int _position;
            private StringBuilder _output;

            private char Current
            {
                get 
                {
                    if (_position >= _input.Length)
                        return EOF;

                    return _input[_position]; 
                }
            }

            public JavascriptMemberNameReplacer(IMappingStore mappingStore)
            {
                _mappingStore = mappingStore;
            }

            public string Replace(string input, IClassMap classMap)
            {
                _classMap = classMap;
                _input = input;
                _output = new StringBuilder();
                _position = 0;
                while (Read()){}
                return _output.ToString();
            }

            private bool Read()
            {
                if (ReadChar(true) == 't' && ReadChar(true) == 'h' && ReadChar(true) == 'i' && ReadChar(true) == 's' && ReadChar(true) == '.')
                {
                    MatchMembers();
                }
                return Current != EOF;
            }

            private char ReadChar(bool includeInOutput)
            {
                char c = Current;
                _position++;
                if(c != EOF && includeInOutput)
                    _output.Append(c);
                return c;
            }

            private void MatchMembers()
            {
                Type currentType = _classMap.ClassType;
            Member:
                string memberName = MatchMember();
                var classMap = _mappingStore.GetClassMap(currentType);
                var memberMap = classMap.GetMemberMapFromMemberName(memberName);
                if (memberMap == null)
                {
                    _output.Append(memberName);
                    return;
                }
                
                _output.Append(memberMap.Alias);
                currentType = memberMap.MemberReturnType;

                var c = ReadChar(true);
                if (c == '[')
                {
                    MatchIndexer();
                    if (memberMap is CollectionMemberMap)
                        currentType = ((CollectionMemberMap)memberMap).ElementType;
                    c = ReadChar(true);
                }
                
                if (c == '.')
                    goto Member;
            }

            private string MatchMember()
            {
                StringBuilder memberName = new StringBuilder();
                char c = Current;
                while (Char.IsLetterOrDigit(c) || c == '_' || c == '$')
                {
                    ReadChar(false);
                    memberName.Append(c);
                    c = Current;
                }

                return memberName.ToString();
            }

            private void MatchIndexer()
            {
                var c = ReadChar(true);
                while (c != ']' && c != EOF)
                {
                    c = ReadChar(true);
                }
            }

        }
    }
}