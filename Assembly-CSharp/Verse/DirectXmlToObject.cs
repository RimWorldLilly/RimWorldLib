using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Verse
{
	// Token: 0x02000510 RID: 1296
	public static class DirectXmlToObject
	{
		// Token: 0x06002652 RID: 9810 RVA: 0x000E9C98 File Offset: 0x000E7E98
		public static Func<XmlNode, bool, object> GetObjectFromXmlMethod(Type type)
		{
			Func<XmlNode, bool, object> func;
			if (!DirectXmlToObject.objectFromXmlMethods.TryGetValue(type, out func))
			{
				MethodInfo method = typeof(DirectXmlToObject).GetMethod("ObjectFromXmlReflection", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				DirectXmlToObject.tmpOneTypeArray[0] = type;
				func = (Func<XmlNode, bool, object>)Delegate.CreateDelegate(typeof(Func<XmlNode, bool, object>), method.MakeGenericMethod(DirectXmlToObject.tmpOneTypeArray));
				DirectXmlToObject.objectFromXmlMethods.Add(type, func);
			}
			return func;
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x000E9D00 File Offset: 0x000E7F00
		private static object ObjectFromXmlReflection<T>(XmlNode xmlRoot, bool doPostLoad)
		{
			return DirectXmlToObject.ObjectFromXml<T>(xmlRoot, doPostLoad);
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000E9D10 File Offset: 0x000E7F10
		public static T ObjectFromXml<T>(XmlNode xmlRoot, bool doPostLoad)
		{
			XmlAttribute xmlAttribute = xmlRoot.Attributes["IsNull"];
			T result;
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				result = default(T);
				return result;
			}
			Type typeFromHandle = typeof(T);
			MethodInfo methodInfo = XmlToObjectUtils.CustomDataLoadMethodOf(typeFromHandle);
			if (methodInfo != null)
			{
				xmlRoot = XmlInheritance.GetResolvedNodeFor(xmlRoot);
				Type type = DirectXmlToObject.ClassTypeOf<T>(xmlRoot);
				DirectXmlToObject.currentlyInstantiatingObjectOfType.Push(type);
				T t;
				try
				{
					t = (T)((object)Activator.CreateInstance(type));
				}
				finally
				{
					DirectXmlToObject.currentlyInstantiatingObjectOfType.Pop();
				}
				try
				{
					methodInfo.Invoke(t, new object[]
					{
						xmlRoot
					});
				}
				catch (Exception ex)
				{
					string[] array = new string[6];
					array[0] = "Exception in custom XML loader for ";
					int num = 1;
					Type type2 = typeFromHandle;
					array[num] = ((type2 != null) ? type2.ToString() : null);
					array[2] = ". Node is:\n ";
					array[3] = xmlRoot.OuterXml;
					array[4] = "\n\nException is:\n ";
					int num2 = 5;
					Exception ex2 = ex;
					array[num2] = ((ex2 != null) ? ex2.ToString() : null);
					Log.Error(string.Concat(array));
					t = default(T);
				}
				if (doPostLoad)
				{
					DirectXmlToObject.TryDoPostLoad(t);
				}
				return t;
			}
			if (GenTypes.IsSlateRef(typeFromHandle))
			{
				try
				{
					return ParseHelper.FromString<T>(DirectXmlToObject.InnerTextWithReplacedNewlinesOrXML(xmlRoot));
				}
				catch (Exception ex3)
				{
					string[] array2 = new string[6];
					array2[0] = "Exception parsing ";
					array2[1] = xmlRoot.OuterXml;
					array2[2] = " to type ";
					int num3 = 3;
					Type type3 = typeFromHandle;
					array2[num3] = ((type3 != null) ? type3.ToString() : null);
					array2[4] = ": ";
					int num4 = 5;
					Exception ex4 = ex3;
					array2[num4] = ((ex4 != null) ? ex4.ToString() : null);
					Log.Error(string.Concat(array2));
				}
				return default(T);
			}
			if (xmlRoot.ChildNodes.Count == 1 && xmlRoot.FirstChild.NodeType == XmlNodeType.Text)
			{
				try
				{
					return ParseHelper.FromString<T>(xmlRoot.InnerText);
				}
				catch (Exception ex5)
				{
					string[] array3 = new string[6];
					array3[0] = "Exception parsing ";
					array3[1] = xmlRoot.OuterXml;
					array3[2] = " to type ";
					int num5 = 3;
					Type type4 = typeFromHandle;
					array3[num5] = ((type4 != null) ? type4.ToString() : null);
					array3[4] = ": ";
					int num6 = 5;
					Exception ex6 = ex5;
					array3[num6] = ((ex6 != null) ? ex6.ToString() : null);
					Log.Error(string.Concat(array3));
				}
				return default(T);
			}
			if (xmlRoot.ChildNodes.Count == 1 && xmlRoot.FirstChild.NodeType == XmlNodeType.CDATA)
			{
				if (typeFromHandle != typeof(string))
				{
					Log.Error("CDATA can only be used for strings. Bad xml: " + xmlRoot.OuterXml);
					return default(T);
				}
				return (T)((object)xmlRoot.FirstChild.Value);
			}
			else
			{
				if (GenTypes.HasFlagsAttribute(typeFromHandle))
				{
					List<T> list = DirectXmlToObject.ListFromXml<T>(xmlRoot);
					int num7 = 0;
					foreach (T t2 in list)
					{
						int num8 = (int)((object)t2);
						num7 |= num8;
					}
					return (T)((object)num7);
				}
				if (GenTypes.IsList(typeFromHandle))
				{
					Func<XmlNode, object> func = null;
					if (!DirectXmlToObject.listFromXmlMethods.TryGetValue(typeFromHandle, out func))
					{
						MethodInfo method = typeof(DirectXmlToObject).GetMethod("ListFromXmlReflection", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						Type[] genericArguments = typeFromHandle.GetGenericArguments();
						func = (Func<XmlNode, object>)Delegate.CreateDelegate(typeof(Func<XmlNode, object>), method.MakeGenericMethod(genericArguments));
						DirectXmlToObject.listFromXmlMethods.Add(typeFromHandle, func);
					}
					return (T)((object)func(xmlRoot));
				}
				if (GenTypes.IsDictionary(typeFromHandle))
				{
					Func<XmlNode, object> func2 = null;
					if (!DirectXmlToObject.dictionaryFromXmlMethods.TryGetValue(typeFromHandle, out func2))
					{
						MethodInfo method2 = typeof(DirectXmlToObject).GetMethod("DictionaryFromXmlReflection", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						Type[] genericArguments2 = typeFromHandle.GetGenericArguments();
						func2 = (Func<XmlNode, object>)Delegate.CreateDelegate(typeof(Func<XmlNode, object>), method2.MakeGenericMethod(genericArguments2));
						DirectXmlToObject.dictionaryFromXmlMethods.Add(typeFromHandle, func2);
					}
					return (T)((object)func2(xmlRoot));
				}
				if (!xmlRoot.HasChildNodes)
				{
					if (typeFromHandle == typeof(string))
					{
						return (T)((object)"");
					}
					XmlAttribute xmlAttribute2 = xmlRoot.Attributes["IsNull"];
					if (xmlAttribute2 != null && xmlAttribute2.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
					{
						return default(T);
					}
					if (GenTypes.IsListHashSetOrDictionary(typeFromHandle))
					{
						return (T)((object)Activator.CreateInstance(typeof(T)));
					}
				}
				xmlRoot = XmlInheritance.GetResolvedNodeFor(xmlRoot);
				Type type = DirectXmlToObject.ClassTypeOf<T>(xmlRoot);
				Type type5 = Nullable.GetUnderlyingType(type) ?? type;
				DirectXmlToObject.currentlyInstantiatingObjectOfType.Push(type5);
				T t;
				try
				{
					t = (T)((object)Activator.CreateInstance(type5));
				}
				catch (InvalidCastException)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot cast XML type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(type5);
					defaultInterpolatedStringHandler.AppendLiteral(" to C# type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidCastException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				finally
				{
					DirectXmlToObject.currentlyInstantiatingObjectOfType.Pop();
				}
				HashSet<string> hashSet = null;
				if (xmlRoot.ChildNodes.Count > 1)
				{
					hashSet = new HashSet<string>();
				}
				XmlNodeList childNodes = xmlRoot.ChildNodes;
				int count = childNodes.Count;
				for (int i = 0; i < count; i++)
				{
					XmlNode xmlNode = childNodes[i];
					if (!(xmlNode is XmlComment))
					{
						if (count > 1 && !hashSet.Add(xmlNode.Name))
						{
							string[] array4 = new string[8];
							array4[0] = "XML ";
							int num9 = 1;
							Type type6 = typeFromHandle;
							array4[num9] = ((type6 != null) ? type6.ToString() : null);
							array4[2] = " defines the same field twice: ";
							array4[3] = xmlNode.Name;
							array4[4] = ".\n\nField contents: ";
							array4[5] = xmlNode.InnerText;
							array4[6] = ".\n\nWhole XML:\n\n";
							array4[7] = xmlRoot.OuterXml;
							Log.Error(string.Concat(array4));
						}
						FieldInfo fieldInfo = XmlToObjectUtils.DoFieldSearch(type5, xmlNode, xmlRoot);
						if (!(fieldInfo == null))
						{
							if (GenTypes.IsDef(fieldInfo.FieldType))
							{
								if (xmlNode.InnerText.NullOrEmpty())
								{
									fieldInfo.SetValue(t, null);
								}
								else
								{
									XmlAttribute xmlAttribute3 = xmlNode.Attributes["MayRequire"];
									XmlAttribute xmlAttribute4 = xmlNode.Attributes["MayRequireAnyOf"];
									DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(t, fieldInfo, xmlNode.InnerText, (xmlAttribute3 != null) ? xmlAttribute3.Value.ToLower() : null, (xmlAttribute4 != null) ? xmlAttribute4.Value.ToLower() : null, null);
								}
							}
							else
							{
								object value = null;
								try
								{
									value = DirectXmlToObject.GetObjectFromXmlMethod(fieldInfo.FieldType)(xmlNode, doPostLoad);
								}
								catch (Exception ex7)
								{
									string str = "Exception loading from ";
									XmlNode xmlNode2 = xmlNode;
									string str2 = (xmlNode2 != null) ? xmlNode2.ToString() : null;
									string str3 = ": ";
									Exception ex8 = ex7;
									Log.Error(str + str2 + str3 + ((ex8 != null) ? ex8.ToString() : null));
									goto IL_6BB;
								}
								if (!typeFromHandle.IsValueType)
								{
									fieldInfo.SetValue(t, value);
								}
								else
								{
									object obj = t;
									fieldInfo.SetValue(obj, value);
									t = (T)((object)obj);
								}
							}
						}
					}
					IL_6BB:;
				}
				if (doPostLoad)
				{
					DirectXmlToObject.TryDoPostLoad(t);
				}
				return t;
			}
			return result;
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x000EA460 File Offset: 0x000E8660
		private static Type ClassTypeOf<T>(XmlNode xmlRoot)
		{
			XmlAttribute xmlAttribute = xmlRoot.Attributes["Class"];
			if (xmlAttribute == null)
			{
				return typeof(T);
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlAttribute.Value, typeof(T).Namespace);
			if (typeInAnyAssembly == null)
			{
				Log.Error("Could not find type named " + xmlAttribute.Value + " from node " + xmlRoot.OuterXml);
				return typeof(T);
			}
			return typeInAnyAssembly;
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x000EA4DC File Offset: 0x000E86DC
		private static void TryDoPostLoad(object obj)
		{
			DeepProfiler.Start("TryDoPostLoad");
			try
			{
				MethodInfo methodInfo = XmlToObjectUtils.PostLoadMethodOf(obj.GetType());
				if (methodInfo != null)
				{
					methodInfo.Invoke(obj, null);
				}
			}
			catch (Exception ex)
			{
				string str = "Exception while executing PostLoad on ";
				string str2 = obj.ToStringSafe<object>();
				string str3 = ": ";
				Exception ex2 = ex;
				Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000EA560 File Offset: 0x000E8760
		private static object ListFromXmlReflection<T>(XmlNode listRootNode)
		{
			return DirectXmlToObject.ListFromXml<T>(listRootNode);
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000EA568 File Offset: 0x000E8768
		private static List<T> ListFromXml<T>(XmlNode listRootNode)
		{
			List<T> list = new List<T>();
			try
			{
				bool flag = GenTypes.IsDef(typeof(T));
				foreach (object obj in listRootNode.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (DirectXmlToObject.ValidateListNode(xmlNode, listRootNode, typeof(T)))
					{
						XmlAttribute xmlAttribute = xmlNode.Attributes["MayRequire"];
						XmlAttribute xmlAttribute2 = xmlNode.Attributes["MayRequireAnyOf"];
						if (flag)
						{
							DirectXmlCrossRefLoader.RegisterListWantsCrossRef<T>(list, xmlNode.InnerText, listRootNode.Name, (xmlAttribute != null) ? xmlAttribute.Value : null, (xmlAttribute2 != null) ? xmlAttribute2.Value : null);
						}
						else if (xmlAttribute != null && !xmlAttribute.Value.NullOrEmpty() && !ModLister.AllModsActiveNoSuffix(xmlAttribute.Value.Split(',', StringSplitOptions.None)))
						{
							if (DirectXmlCrossRefLoader.MistypedMayRequire(xmlAttribute.Value))
							{
								Log.Error("Faulty MayRequire: " + xmlAttribute.Value);
							}
						}
						else if (xmlAttribute2 == null || xmlAttribute2.Value.NullOrEmpty() || ModLister.AnyModActiveNoSuffix(xmlAttribute2.Value.Split(',', StringSplitOptions.None)))
						{
							try
							{
								list.Add(DirectXmlToObject.ObjectFromXml<T>(xmlNode, true));
							}
							catch (Exception value)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Exception loading list element ");
								defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
								defaultInterpolatedStringHandler.AppendLiteral(" from XML: ");
								defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
								defaultInterpolatedStringHandler.AppendLiteral("\nXML:\n");
								defaultInterpolatedStringHandler.AppendFormatted(listRootNode.OuterXml);
								Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				string str = "Exception loading list from XML: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null) + "\nXML:\n" + listRootNode.OuterXml);
			}
			return list;
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x000EA7A8 File Offset: 0x000E89A8
		private static object DictionaryFromXmlReflection<K, V>(XmlNode dictRootNode)
		{
			return DirectXmlToObject.DictionaryFromXml<K, V>(dictRootNode);
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x000EA7B0 File Offset: 0x000E89B0
		private static Dictionary<K, V> DictionaryFromXml<K, V>(XmlNode dictRootNode)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			try
			{
				bool flag = GenTypes.IsDef(typeof(K));
				bool flag2 = GenTypes.IsDef(typeof(V));
				if (!flag && !flag2)
				{
					using (IEnumerator enumerator = dictRootNode.ChildNodes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							XmlNode xmlNode = (XmlNode)obj;
							if (DirectXmlToObject.ValidateListNode(xmlNode, dictRootNode, typeof(KeyValuePair<K, V>)))
							{
								K key = DirectXmlToObject.ObjectFromXml<K>(xmlNode["key"], true);
								V value = DirectXmlToObject.ObjectFromXml<V>(xmlNode["value"], true);
								dictionary.Add(key, value);
							}
						}
						goto IL_100;
					}
				}
				foreach (object obj2 in dictRootNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj2;
					if (DirectXmlToObject.ValidateListNode(xmlNode2, dictRootNode, typeof(KeyValuePair<K, V>)))
					{
						DirectXmlCrossRefLoader.RegisterDictionaryWantsCrossRef<K, V>(dictionary, xmlNode2, dictRootNode.Name);
					}
				}
				IL_100:;
			}
			catch (Exception ex)
			{
				string str = "Malformed dictionary XML. Node: ";
				string outerXml = dictRootNode.OuterXml;
				string str2 = ".\n\nException: ";
				Exception ex2 = ex;
				Log.Error(str + outerXml + str2 + ((ex2 != null) ? ex2.ToString() : null));
			}
			return dictionary;
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x000EA914 File Offset: 0x000E8B14
		public static bool ValidateListNode(XmlNode listEntryNode, XmlNode listRootNode, Type listItemType)
		{
			if (listEntryNode is XmlComment)
			{
				return false;
			}
			if (listEntryNode is XmlText)
			{
				Log.Error("XML format error: Raw text found inside a list element. Did you mean to surround it with list item <li> tags? " + listRootNode.OuterXml);
				return false;
			}
			if (listEntryNode.Name != "li" && XmlToObjectUtils.CustomDataLoadMethodOf(listItemType) == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(116, 3);
				defaultInterpolatedStringHandler.AppendLiteral("XML format error: List item found with name ");
				defaultInterpolatedStringHandler.AppendFormatted(listEntryNode.Name);
				defaultInterpolatedStringHandler.AppendLiteral(" that is not <");
				defaultInterpolatedStringHandler.AppendFormatted("li");
				defaultInterpolatedStringHandler.AppendLiteral(">, and which does not have a custom XML loader method, in ");
				defaultInterpolatedStringHandler.AppendFormatted(listRootNode.OuterXml);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return false;
			}
			return true;
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x000EA9CD File Offset: 0x000E8BCD
		public static string InnerTextWithReplacedNewlinesOrXML(XmlNode xmlNode)
		{
			if (xmlNode.ChildNodes.Count == 1 && xmlNode.FirstChild.NodeType == XmlNodeType.Text)
			{
				return xmlNode.InnerText.Replace("\\n", "\n");
			}
			return xmlNode.InnerXml;
		}

		// Token: 0x04001B85 RID: 7045
		public static Stack<Type> currentlyInstantiatingObjectOfType = new Stack<Type>();

		// Token: 0x04001B86 RID: 7046
		public const string DictionaryKeyName = "key";

		// Token: 0x04001B87 RID: 7047
		public const string DictionaryValueName = "value";

		// Token: 0x04001B88 RID: 7048
		public const string LoadDataFromXmlCustomMethodName = "LoadDataFromXmlCustom";

		// Token: 0x04001B89 RID: 7049
		private static Dictionary<Type, Func<XmlNode, object>> listFromXmlMethods = new Dictionary<Type, Func<XmlNode, object>>();

		// Token: 0x04001B8A RID: 7050
		private static Dictionary<Type, Func<XmlNode, object>> dictionaryFromXmlMethods = new Dictionary<Type, Func<XmlNode, object>>();

		// Token: 0x04001B8B RID: 7051
		private static readonly Type[] tmpOneTypeArray = new Type[1];

		// Token: 0x04001B8C RID: 7052
		private static readonly Dictionary<Type, Func<XmlNode, bool, object>> objectFromXmlMethods = new Dictionary<Type, Func<XmlNode, bool, object>>();
	}
}
