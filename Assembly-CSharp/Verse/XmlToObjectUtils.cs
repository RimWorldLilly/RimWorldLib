using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using JetBrains.Annotations;

namespace Verse
{
	// Token: 0x02000517 RID: 1303
	public static class XmlToObjectUtils
	{
		// Token: 0x0600269C RID: 9884 RVA: 0x000EF580 File Offset: 0x000ED780
		[CanBeNull]
		public static FieldInfo DoFieldSearch(Type typeBeingDeserialized, XmlNode fieldNode, XmlNode xmlRootForDebug)
		{
			string name = fieldNode.Name;
			FieldInfo fieldInfo = XmlToObjectUtils.DirectGetFieldByName(typeBeingDeserialized, name, xmlRootForDebug);
			if (fieldInfo != null)
			{
				return fieldInfo;
			}
			DeepProfiler.Start("Search for field alias");
			try
			{
				XmlToObjectUtils.FieldAliasCache key = new XmlToObjectUtils.FieldAliasCache(typeBeingDeserialized, name);
				if (!XmlToObjectUtils.fieldAliases.TryGetValue(key, out fieldInfo))
				{
					foreach (FieldInfo fieldInfo2 in typeBeingDeserialized.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					{
						using (IEnumerator<LoadAliasAttribute> enumerator = fieldInfo2.GetCustomAttributes(true).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.alias.EqualsIgnoreCase(name))
								{
									fieldInfo = fieldInfo2;
									break;
								}
							}
						}
						if (fieldInfo != null)
						{
							break;
						}
					}
					XmlToObjectUtils.fieldAliases.Add(key, fieldInfo);
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			if (fieldInfo != null)
			{
				UnsavedAttribute unsavedAttribute = fieldInfo.TryGetAttribute<UnsavedAttribute>();
				if (unsavedAttribute != null && !unsavedAttribute.allowLoading)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(85, 3);
					defaultInterpolatedStringHandler.AppendLiteral("XML error: ");
					defaultInterpolatedStringHandler.AppendFormatted(fieldNode.OuterXml);
					defaultInterpolatedStringHandler.AppendLiteral(" corresponds to a field in type ");
					defaultInterpolatedStringHandler.AppendFormatted(typeBeingDeserialized.Name);
					defaultInterpolatedStringHandler.AppendLiteral(" which has an Unsaved attribute. Context: ");
					defaultInterpolatedStringHandler.AppendFormatted(xmlRootForDebug.OuterXml);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					return null;
				}
			}
			if (fieldInfo != null)
			{
				return fieldInfo;
			}
			DeepProfiler.Start("Check for ignored missing field");
			FieldInfo result;
			try
			{
				bool flag = false;
				XmlAttributeCollection attributes = fieldNode.Attributes;
				XmlAttribute xmlAttribute = (attributes != null) ? attributes["IgnoreIfNoMatchingField"] : null;
				if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
				{
					flag = true;
				}
				else
				{
					object[] customAttributes = typeBeingDeserialized.GetCustomAttributes(typeof(IgnoreSavedElementAttribute), true);
					for (int i = 0; i < customAttributes.Length; i++)
					{
						if (string.Equals(((IgnoreSavedElementAttribute)customAttributes[i]).elementToIgnore, name, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(63, 3);
					defaultInterpolatedStringHandler2.AppendLiteral("XML error: ");
					defaultInterpolatedStringHandler2.AppendFormatted(fieldNode.OuterXml);
					defaultInterpolatedStringHandler2.AppendLiteral(" doesn't correspond to any field in type ");
					defaultInterpolatedStringHandler2.AppendFormatted(typeBeingDeserialized.Name);
					defaultInterpolatedStringHandler2.AppendLiteral(". Context: ");
					defaultInterpolatedStringHandler2.AppendFormatted(xmlRootForDebug.OuterXml);
					Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				result = null;
			}
			finally
			{
				DeepProfiler.End();
			}
			return result;
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000EF7F8 File Offset: 0x000ED9F8
		private static FieldInfo DirectGetFieldByName(Type type, string token, XmlNode debugXmlNode)
		{
			Dictionary<string, FieldInfo> dictionary;
			if (!XmlToObjectUtils.fieldInfoLookup.TryGetValue(type, out dictionary))
			{
				dictionary = new Dictionary<string, FieldInfo>();
				XmlToObjectUtils.fieldInfoLookup.Add(type, dictionary);
			}
			FieldInfo fieldInfo;
			if (dictionary.TryGetValue(token, out fieldInfo))
			{
				return fieldInfo;
			}
			fieldInfo = XmlToObjectUtils.SearchTypeHierarchy(type, token, false);
			if (fieldInfo != null)
			{
				dictionary.Add(token, fieldInfo);
				return fieldInfo;
			}
			fieldInfo = XmlToObjectUtils.SearchTypeHierarchy(type, token, true);
			if (fieldInfo == null)
			{
				return null;
			}
			dictionary.Add(token, fieldInfo);
			if (type.HasAttribute<CaseInsensitiveXMLParsing>())
			{
				return fieldInfo;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(83, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Attempt to use string ");
			defaultInterpolatedStringHandler.AppendFormatted(token);
			defaultInterpolatedStringHandler.AppendLiteral(" to refer to field ");
			defaultInterpolatedStringHandler.AppendFormatted(fieldInfo.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" in type ");
			defaultInterpolatedStringHandler.AppendFormatted<Type>(type);
			defaultInterpolatedStringHandler.AppendLiteral("; xml tags are now case-sensitive");
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			if (debugXmlNode != null)
			{
				text = text + ". XML: " + debugXmlNode.OuterXml;
			}
			Log.Error(text);
			return fieldInfo;
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000EF8F0 File Offset: 0x000EDAF0
		private static FieldInfo SearchTypeHierarchy(Type type, string name, bool ignoreCase)
		{
			Dictionary<ValueTuple<Type, string>, FieldInfo> dictionary = ignoreCase ? XmlToObjectUtils.getFieldIgnoreCaseCache : XmlToObjectUtils.getFieldCache;
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			if (ignoreCase)
			{
				bindingFlags |= BindingFlags.IgnoreCase;
			}
			FieldInfo field;
			for (;;)
			{
				if (dictionary.TryGetValue(new ValueTuple<Type, string>(type, name), out field))
				{
					if (field != null)
					{
						break;
					}
				}
				else
				{
					field = type.GetField(name, bindingFlags);
					dictionary.Add(new ValueTuple<Type, string>(type, name), field);
					if (field != null)
					{
						return field;
					}
				}
				Type baseType = type.BaseType;
				if (baseType == null || !(baseType != typeof(object)))
				{
					goto IL_79;
				}
				type = baseType;
			}
			return field;
			IL_79:
			return null;
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000EF978 File Offset: 0x000EDB78
		public static MethodInfo CustomDataLoadMethodOf(Type type)
		{
			MethodInfo result;
			if (XmlToObjectUtils.customDataLoadMethodCache.TryGetValue(type, out result))
			{
				return result;
			}
			MethodInfo method = type.GetMethod("LoadDataFromXmlCustom", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			XmlToObjectUtils.customDataLoadMethodCache.Add(type, method);
			return method;
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x000EF9B4 File Offset: 0x000EDBB4
		public static MethodInfo PostLoadMethodOf(Type type)
		{
			MethodInfo result;
			if (XmlToObjectUtils.postLoadMethodCache.TryGetValue(type, out result))
			{
				return result;
			}
			MethodInfo method = type.GetMethod("PostLoad");
			XmlToObjectUtils.postLoadMethodCache.Add(type, method);
			return method;
		}

		// Token: 0x04001BD0 RID: 7120
		private static readonly Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoLookup = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		// Token: 0x04001BD1 RID: 7121
		private static readonly Dictionary<XmlToObjectUtils.FieldAliasCache, FieldInfo> fieldAliases = new Dictionary<XmlToObjectUtils.FieldAliasCache, FieldInfo>(EqualityComparer<XmlToObjectUtils.FieldAliasCache>.Default);

		// Token: 0x04001BD2 RID: 7122
		private static readonly Dictionary<ValueTuple<Type, string>, FieldInfo> getFieldCache = new Dictionary<ValueTuple<Type, string>, FieldInfo>();

		// Token: 0x04001BD3 RID: 7123
		private static readonly Dictionary<ValueTuple<Type, string>, FieldInfo> getFieldIgnoreCaseCache = new Dictionary<ValueTuple<Type, string>, FieldInfo>();

		// Token: 0x04001BD4 RID: 7124
		private static Dictionary<Type, MethodInfo> customDataLoadMethodCache = new Dictionary<Type, MethodInfo>();

		// Token: 0x04001BD5 RID: 7125
		public const string PostLoadMethodName = "PostLoad";

		// Token: 0x04001BD6 RID: 7126
		private static Dictionary<Type, MethodInfo> postLoadMethodCache = new Dictionary<Type, MethodInfo>();

		// Token: 0x02002767 RID: 10087
		private struct FieldAliasCache : IEquatable<XmlToObjectUtils.FieldAliasCache>
		{
			// Token: 0x0600F80B RID: 63499 RVA: 0x005A0068 File Offset: 0x0059E268
			public FieldAliasCache(Type type, string fieldName)
			{
				this.type = type;
				this.fieldName = fieldName.ToLower();
			}

			// Token: 0x0600F80C RID: 63500 RVA: 0x005A007D File Offset: 0x0059E27D
			public bool Equals(XmlToObjectUtils.FieldAliasCache other)
			{
				return this.type == other.type && string.Equals(this.fieldName, other.fieldName);
			}

			// Token: 0x0600F80D RID: 63501 RVA: 0x005A00A5 File Offset: 0x0059E2A5
			public override int GetHashCode()
			{
				return HashCode.Combine<Type, string>(this.type, this.fieldName);
			}

			// Token: 0x0400A129 RID: 41257
			public Type type;

			// Token: 0x0400A12A RID: 41258
			public string fieldName;
		}
	}
}
