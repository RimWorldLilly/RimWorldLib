using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using RimWorld;
using RimWorld.QuestGen;

namespace Verse
{
	// Token: 0x02000082 RID: 130
	public static class GenTypes
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x00020314 File Offset: 0x0001E514
		private static IEnumerable<Assembly> AllActiveAssemblies
		{
			get
			{
				return new GenTypes.<get_AllActiveAssemblies>d__2(-2);
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x00020320 File Offset: 0x0001E520
		public static List<Type> AllTypes
		{
			get
			{
				if (GenTypes.allTypesCached == null)
				{
					GenTypes.allTypesCached = new List<Type>();
					GenTypes.tmpAllTypesHashSet.Clear();
					foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
					{
						Type[] array = null;
						try
						{
							array = assembly.GetTypes();
						}
						catch (ReflectionTypeLoadException ex)
						{
							string str = "Exception getting types in assembly ";
							string str2 = assembly.ToString();
							string str3 = ". Some types may not work correctly. Exception: ";
							ReflectionTypeLoadException ex2 = ex;
							Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
							try
							{
								Type[] types = ex.Types;
								if (types != null)
								{
									array = (from x in types
									where x != null && x.TypeInitializer != null
									select x).ToArray<Type>();
								}
							}
							catch (Exception ex3)
							{
								string str4 = "Could not resolve assembly types fallback. Exception: ";
								Exception ex4 = ex3;
								Log.Error(str4 + ((ex4 != null) ? ex4.ToString() : null));
							}
						}
						if (array != null)
						{
							for (int i = 0; i < array.Length; i++)
							{
								if (array[i] != null && GenTypes.tmpAllTypesHashSet.Add(array[i]))
								{
									GenTypes.allTypesCached.Add(array[i]);
								}
							}
						}
					}
					GenTypes.tmpAllTypesHashSet.Clear();
				}
				return GenTypes.allTypesCached;
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00020484 File Offset: 0x0001E684
		public static List<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			List<Type> result;
			if (GenTypes.cachedTypesWithAttribute.TryGetValue(typeof(TAttr), out result))
			{
				return result;
			}
			List<Type> list = (from x in GenTypes.AllTypes.AsParallel<Type>()
			where x.HasAttribute<TAttr>()
			select x).ToList<Type>();
			GenTypes.cachedTypesWithAttribute.Add(typeof(TAttr), list);
			return list;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x000204F8 File Offset: 0x0001E6F8
		public static List<Type> AllSubclasses(this Type baseType)
		{
			if (!GenTypes.cachedSubclasses.ContainsKey(baseType))
			{
				GenTypes.cachedSubclasses.Add(baseType, (from x in GenTypes.AllTypes.AsParallel<Type>()
				where x.IsSubclassOf(baseType)
				select x).ToList<Type>());
			}
			return GenTypes.cachedSubclasses[baseType];
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00020564 File Offset: 0x0001E764
		public static bool SameOrSubclassOf<T>(this Type baseType)
		{
			return baseType.SameOrSubclassOf(typeof(T));
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00020576 File Offset: 0x0001E776
		public static bool SameOrSubclassOf(this Type baseType, Type parentType)
		{
			return baseType == parentType || baseType.IsSubclassOf(parentType);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0002058C File Offset: 0x0001E78C
		public static List<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			if (!GenTypes.cachedSubclassesNonAbstract.ContainsKey(baseType))
			{
				GenTypes.cachedSubclassesNonAbstract.Add(baseType, (from x in GenTypes.AllTypes.AsParallel<Type>()
				where x.IsSubclassOf(baseType) && !x.IsAbstract
				select x).ToList<Type>());
			}
			return GenTypes.cachedSubclassesNonAbstract[baseType];
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000205F8 File Offset: 0x0001E7F8
		public static List<Type> AllAssignableOfNonAbstract(this Type baseType)
		{
			if (!GenTypes.cachedAssignableNonAbstract.ContainsKey(baseType))
			{
				GenTypes.cachedAssignableNonAbstract.Add(baseType, (from x in GenTypes.AllTypes.AsParallel<Type>()
				where baseType.IsAssignableFrom(x) && !x.IsAbstract
				select x).ToList<Type>());
			}
			return GenTypes.cachedAssignableNonAbstract[baseType];
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00020664 File Offset: 0x0001E864
		public static void ClearCache()
		{
			GenTypes.cachedSubclasses.Clear();
			GenTypes.cachedSubclassesNonAbstract.Clear();
			GenTypes.cachedAssignableNonAbstract.Clear();
			GenTypes.cachedTypesWithAttribute.Clear();
			GenTypes.allTypesCached = null;
			GenTypes.typesInIgnoredNamespacesByName.Clear();
			GenTypes.isSealedCache.Clear();
			GenTypes.initializedTypesInIgnoredNamespaces = false;
			AlertsReadout.allAlertTypesCached = null;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x000206BF File Offset: 0x0001E8BF
		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any<Type>()
			select type;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x000206EB File Offset: 0x0001E8EB
		public static IEnumerable<Type> InstantiableDescendantsAndSelf(this Type baseType)
		{
			GenTypes.<InstantiableDescendantsAndSelf>d__19 <InstantiableDescendantsAndSelf>d__ = new GenTypes.<InstantiableDescendantsAndSelf>d__19(-2);
			<InstantiableDescendantsAndSelf>d__.<>3__baseType = baseType;
			return <InstantiableDescendantsAndSelf>d__;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x000206FC File Offset: 0x0001E8FC
		private static Type TryGetTypeInIgnoredNamespace(string typeName)
		{
			if (!GenTypes.initializedTypesInIgnoredNamespaces)
			{
				foreach (Type type in GenTypes.AllTypes)
				{
					if (type.Namespace == null || GenTypes.IgnoredNamespaceNames.Contains(type.Namespace))
					{
						GenTypes.typesInIgnoredNamespacesByName[type.Name] = type;
					}
				}
				GenTypes.initializedTypesInIgnoredNamespaces = true;
			}
			Type result;
			if (GenTypes.typesInIgnoredNamespacesByName.TryGetValue(typeName, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00020794 File Offset: 0x0001E994
		public static Type GetTypeInAnyAssembly(string typeName, string namespaceIfAmbiguous = null)
		{
			if (namespaceIfAmbiguous == null || GenTypes.IgnoredNamespaceNames.Contains(namespaceIfAmbiguous))
			{
				Type type = GenTypes.TryGetTypeInIgnoredNamespace(typeName);
				if (type != null)
				{
					return type;
				}
			}
			GenTypes.TypeCacheKey key = new GenTypes.TypeCacheKey(typeName, namespaceIfAmbiguous);
			Type type2 = null;
			if (!GenTypes.typeCache.TryGetValue(key, out type2))
			{
				type2 = GenTypes.GetTypeInAnyAssemblyInt(typeName, namespaceIfAmbiguous);
				GenTypes.typeCache.Add(key, type2);
			}
			return type2;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x000207F4 File Offset: 0x0001E9F4
		public static bool HasFlagsAttribute(Type type)
		{
			bool result;
			if (GenTypes.hasFlagsAttributeCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = Attribute.IsDefined(type, typeof(FlagsAttribute));
			GenTypes.hasFlagsAttributeCache.Add(type, flag);
			return flag;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00020830 File Offset: 0x0001EA30
		public static bool IsList(Type type)
		{
			bool result;
			if (GenTypes.isListCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = type.HasGenericDefinition(typeof(List<>));
			GenTypes.isListCache.Add(type, flag);
			return flag;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0002086C File Offset: 0x0001EA6C
		public static bool IsDictionary(Type type)
		{
			bool result;
			if (GenTypes.isDictionaryCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = type.HasGenericDefinition(typeof(Dictionary<, >));
			GenTypes.isDictionaryCache.Add(type, flag);
			return flag;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000208A8 File Offset: 0x0001EAA8
		public static bool IsSlateRef(Type type)
		{
			bool result;
			if (GenTypes.isSlateRefCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = typeof(ISlateRef).IsAssignableFrom(type);
			GenTypes.isSlateRefCache.Add(type, flag);
			return flag;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x000208E4 File Offset: 0x0001EAE4
		public static bool IsListHashSetOrDictionary(Type type)
		{
			bool result;
			if (GenTypes.isListHashSetOrDictionaryCached.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = false;
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(List<>) || genericTypeDefinition == typeof(HashSet<>) || genericTypeDefinition == typeof(Dictionary<, >))
				{
					flag = true;
				}
			}
			GenTypes.isListHashSetOrDictionaryCached.Add(type, flag);
			return flag;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00020958 File Offset: 0x0001EB58
		public static bool IsDef(Type type)
		{
			bool result;
			if (GenTypes.isDefCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = typeof(Def).IsAssignableFrom(type);
			GenTypes.isDefCache.Add(type, flag);
			return flag;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00020994 File Offset: 0x0001EB94
		public static bool IsDefThreaded(Type type)
		{
			object obj = GenTypes.isDefCacheLock;
			bool result;
			lock (obj)
			{
				result = GenTypes.IsDef(type);
			}
			return result;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000209D8 File Offset: 0x0001EBD8
		private static Type GetTypeInAnyAssemblyInt(string typeName, string namespaceIfAmbiguous = null)
		{
			Type typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(typeName);
			if (typeInAnyAssemblyRaw != null)
			{
				return typeInAnyAssemblyRaw;
			}
			if (!namespaceIfAmbiguous.NullOrEmpty() && GenTypes.IgnoredNamespaceNames.Contains(namespaceIfAmbiguous))
			{
				typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(namespaceIfAmbiguous + "." + typeName);
				if (typeInAnyAssemblyRaw != null)
				{
					return typeInAnyAssemblyRaw;
				}
			}
			foreach (string str in GenTypes.IgnoredNamespaceNames)
			{
				typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(str + "." + typeName);
				if (typeInAnyAssemblyRaw != null)
				{
					return typeInAnyAssemblyRaw;
				}
			}
			if (GenTypes.TryGetMixedAssemblyGenericType(typeName, out typeInAnyAssemblyRaw))
			{
				return typeInAnyAssemblyRaw;
			}
			return null;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00020A94 File Offset: 0x0001EC94
		private static bool TryGetMixedAssemblyGenericType(string typeName, out Type type)
		{
			type = GenTypes.GetTypeInAnyAssemblyRaw(typeName);
			if (type == null && typeName.Contains("`"))
			{
				try
				{
					Match match = Regex.Match(typeName, "(?<MainType>.+`(?<ParamCount>[0-9]+))(?<Types>\\[.*\\])");
					if (match.Success)
					{
						int capacity = int.Parse(match.Groups["ParamCount"].Value);
						string value = match.Groups["Types"].Value;
						List<string> list = new List<string>(capacity);
						foreach (object obj in Regex.Matches(value, "\\[(?<Type>.*?)\\],?"))
						{
							Match match2 = (Match)obj;
							if (match2.Success)
							{
								list.Add(match2.Groups["Type"].Value.Trim());
							}
						}
						Type[] array = new Type[list.Count];
						for (int i = 0; i < list.Count; i++)
						{
							Type type2;
							if (!GenTypes.TryGetMixedAssemblyGenericType(list[i], out type2))
							{
								return false;
							}
							array[i] = type2;
						}
						Type type3;
						if (GenTypes.TryGetMixedAssemblyGenericType(match.Groups["MainType"].Value, out type3))
						{
							type = type3.MakeGenericType(array);
						}
					}
				}
				catch (Exception ex)
				{
					string str = "Error in TryGetMixedAssemblyGenericType with typeName=";
					string str2 = ": ";
					Exception ex2 = ex;
					Log.ErrorOnce(str + typeName + str2 + ((ex2 != null) ? ex2.ToString() : null), typeName.GetHashCode());
				}
			}
			return type != null;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00020C54 File Offset: 0x0001EE54
		private static Type GetTypeInAnyAssemblyRaw(string typeName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(typeName);
			if (num <= 2299065237U)
			{
				if (num <= 1092586446U)
				{
					if (num <= 431052896U)
					{
						if (num != 296283782U)
						{
							if (num != 398550328U)
							{
								if (num == 431052896U)
								{
									if (typeName == "byte?")
									{
										return typeof(byte?);
									}
								}
							}
							else if (typeName == "string")
							{
								return typeof(string);
							}
						}
						else if (typeName == "char?")
						{
							return typeof(char?);
						}
					}
					else if (num != 513669818U)
					{
						if (num != 520654156U)
						{
							if (num == 1092586446U)
							{
								if (typeName == "float?")
								{
									return typeof(float?);
								}
							}
						}
						else if (typeName == "decimal")
						{
							return typeof(decimal);
						}
					}
					else if (typeName == "uint?")
					{
						return typeof(uint?);
					}
				}
				else if (num <= 1454009365U)
				{
					if (num != 1189328644U)
					{
						if (num != 1299622921U)
						{
							if (num == 1454009365U)
							{
								if (typeName == "sbyte?")
								{
									return typeof(sbyte?);
								}
							}
						}
						else if (typeName == "decimal?")
						{
							return typeof(decimal?);
						}
					}
					else if (typeName == "long?")
					{
						return typeof(long?);
					}
				}
				else if (num <= 1630192034U)
				{
					if (num != 1603400371U)
					{
						if (num == 1630192034U)
						{
							if (typeName == "ushort")
							{
								return typeof(ushort);
							}
						}
					}
					else if (typeName == "int?")
					{
						return typeof(int?);
					}
				}
				else if (num != 1683620383U)
				{
					if (num == 2299065237U)
					{
						if (typeName == "double?")
						{
							return typeof(double?);
						}
					}
				}
				else if (typeName == "byte")
				{
					return typeof(byte);
				}
			}
			else if (num <= 2823553821U)
			{
				if (num <= 2515107422U)
				{
					if (num != 2471414311U)
					{
						if (num != 2508976771U)
						{
							if (num == 2515107422U)
							{
								if (typeName == "int")
								{
									return typeof(int);
								}
							}
						}
						else if (typeName == "ulong?")
						{
							return typeof(ulong?);
						}
					}
					else if (typeName == "ushort?")
					{
						return typeof(ushort?);
					}
				}
				else if (num <= 2699759368U)
				{
					if (num != 2667225454U)
					{
						if (num == 2699759368U)
						{
							if (typeName == "double")
							{
								return typeof(double);
							}
						}
					}
					else if (typeName == "ulong")
					{
						return typeof(ulong);
					}
				}
				else if (num != 2797886853U)
				{
					if (num == 2823553821U)
					{
						if (typeName == "char")
						{
							return typeof(char);
						}
					}
				}
				else if (typeName == "float")
				{
					return typeof(float);
				}
			}
			else if (num <= 3286667814U)
			{
				if (num != 3122818005U)
				{
					if (num != 3270303571U)
					{
						if (num == 3286667814U)
						{
							if (typeName == "bool?")
							{
								return typeof(bool?);
							}
						}
					}
					else if (typeName == "long")
					{
						return typeof(long);
					}
				}
				else if (typeName == "short")
				{
					return typeof(short);
				}
			}
			else if (num <= 3415750305U)
			{
				if (num != 3365180733U)
				{
					if (num == 3415750305U)
					{
						if (typeName == "uint")
						{
							return typeof(uint);
						}
					}
				}
				else if (typeName == "bool")
				{
					return typeof(bool);
				}
			}
			else if (num != 3996115294U)
			{
				if (num == 4088464520U)
				{
					if (typeName == "sbyte")
					{
						return typeof(sbyte);
					}
				}
			}
			else if (typeName == "short?")
			{
				return typeof(short?);
			}
			Type type = typeof(GenTypes).Assembly.GetType(typeName, false, true);
			if (type != null)
			{
				return type;
			}
			foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
			{
				Type type2 = assembly.GetType(typeName, false, true);
				if (type2 != null)
				{
					return type2;
				}
			}
			Type type3 = Type.GetType(typeName, false, true);
			if (type3 != null)
			{
				return type3;
			}
			return null;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00021204 File Offset: 0x0001F404
		public static string GetTypeNameWithoutIgnoredNamespaces(Type type)
		{
			if (type.IsGenericType)
			{
				return type.ToString();
			}
			if (GenTypes.IgnoredNamespaceNames.Contains(type.Namespace))
			{
				return type.Name;
			}
			return type.FullName;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00021234 File Offset: 0x0001F434
		public static bool IsCustomType(Type type)
		{
			string @namespace = type.Namespace;
			return @namespace == null || (!@namespace.StartsWith("System") && !@namespace.StartsWith("UnityEngine") && !@namespace.StartsWith("Steamworks"));
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00021277 File Offset: 0x0001F477
		public static bool Isnt<T>(this object obj)
		{
			return !(obj is T);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00021285 File Offset: 0x0001F485
		public static bool Isnt<T>(this object obj, out T casted) where T : class
		{
			casted = (obj as T);
			return casted == null;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x000212A8 File Offset: 0x0001F4A8
		public static bool IsSealedWithCache(this Type type)
		{
			Dictionary<Type, bool> obj = GenTypes.isSealedCache;
			bool result;
			lock (obj)
			{
				bool flag2;
				if (GenTypes.isSealedCache.TryGetValue(type, out flag2))
				{
					result = flag2;
				}
				else
				{
					result = (GenTypes.isSealedCache[type] = type.IsSealed);
				}
			}
			return result;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0002130C File Offset: 0x0001F50C
		public static bool IsMethodOverriden(this Type type, string method)
		{
			return type.GetMethod(method).IsOverriden();
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0002131A File Offset: 0x0001F51A
		public static bool IsOverriden(this MethodInfo method)
		{
			return method.IsVirtual && method.DeclaringType != method.GetBaseDefinition().DeclaringType;
		}

		// Token: 0x040001D6 RID: 470
		public static readonly HashSet<string> IgnoredNamespaceNames = new HashSet<string>
		{
			"RimWorld",
			"Verse",
			"LudeonTK",
			"Verse.AI",
			"Verse.AI.Group",
			"Verse.Sound",
			"Verse.Grammar",
			"RimWorld.Planet",
			"RimWorld.BaseGen",
			"RimWorld.QuestGen",
			"RimWorld.SketchGen",
			"System"
		};

		// Token: 0x040001D7 RID: 471
		private static List<Type> allTypesCached;

		// Token: 0x040001D8 RID: 472
		private static HashSet<Type> tmpAllTypesHashSet = new HashSet<Type>();

		// Token: 0x040001D9 RID: 473
		private static Dictionary<Type, List<Type>> cachedTypesWithAttribute = new Dictionary<Type, List<Type>>();

		// Token: 0x040001DA RID: 474
		private static Dictionary<Type, List<Type>> cachedSubclasses = new Dictionary<Type, List<Type>>();

		// Token: 0x040001DB RID: 475
		private static Dictionary<Type, List<Type>> cachedSubclassesNonAbstract = new Dictionary<Type, List<Type>>();

		// Token: 0x040001DC RID: 476
		private static readonly Dictionary<Type, List<Type>> cachedAssignableNonAbstract = new Dictionary<Type, List<Type>>();

		// Token: 0x040001DD RID: 477
		private static readonly Dictionary<string, Type> typesInIgnoredNamespacesByName = new Dictionary<string, Type>();

		// Token: 0x040001DE RID: 478
		private static bool initializedTypesInIgnoredNamespaces = false;

		// Token: 0x040001DF RID: 479
		private static Dictionary<GenTypes.TypeCacheKey, Type> typeCache = new Dictionary<GenTypes.TypeCacheKey, Type>(EqualityComparer<GenTypes.TypeCacheKey>.Default);

		// Token: 0x040001E0 RID: 480
		private static Dictionary<Type, bool> hasFlagsAttributeCache = new Dictionary<Type, bool>();

		// Token: 0x040001E1 RID: 481
		private static Dictionary<Type, bool> isListCache = new Dictionary<Type, bool>();

		// Token: 0x040001E2 RID: 482
		private static Dictionary<Type, bool> isDictionaryCache = new Dictionary<Type, bool>();

		// Token: 0x040001E3 RID: 483
		private static Dictionary<Type, bool> isSlateRefCache = new Dictionary<Type, bool>();

		// Token: 0x040001E4 RID: 484
		private static Dictionary<Type, bool> isListHashSetOrDictionaryCached = new Dictionary<Type, bool>();

		// Token: 0x040001E5 RID: 485
		private static Dictionary<Type, bool> isDefCache = new Dictionary<Type, bool>();

		// Token: 0x040001E6 RID: 486
		private static object isDefCacheLock = new object();

		// Token: 0x040001E7 RID: 487
		private static readonly Dictionary<Type, bool> isSealedCache = new Dictionary<Type, bool>();

		// Token: 0x020024C1 RID: 9409
		private struct TypeCacheKey : IEquatable<GenTypes.TypeCacheKey>
		{
			// Token: 0x0600EB61 RID: 60257 RVA: 0x0056FF93 File Offset: 0x0056E193
			public override int GetHashCode()
			{
				if (this.namespaceIfAmbiguous == null)
				{
					return this.typeName.GetHashCode();
				}
				return (17 * 31 + this.typeName.GetHashCode()) * 31 + this.namespaceIfAmbiguous.GetHashCode();
			}

			// Token: 0x0600EB62 RID: 60258 RVA: 0x0056FFC9 File Offset: 0x0056E1C9
			public bool Equals(GenTypes.TypeCacheKey other)
			{
				return string.Equals(this.typeName, other.typeName) && string.Equals(this.namespaceIfAmbiguous, other.namespaceIfAmbiguous);
			}

			// Token: 0x0600EB63 RID: 60259 RVA: 0x0056FFF1 File Offset: 0x0056E1F1
			public override bool Equals(object obj)
			{
				return obj is GenTypes.TypeCacheKey && this.Equals((GenTypes.TypeCacheKey)obj);
			}

			// Token: 0x0600EB64 RID: 60260 RVA: 0x00570009 File Offset: 0x0056E209
			public TypeCacheKey(string typeName, string namespaceIfAmbigous = null)
			{
				this.typeName = typeName;
				this.namespaceIfAmbiguous = namespaceIfAmbigous;
			}

			// Token: 0x0400968D RID: 38541
			public string typeName;

			// Token: 0x0400968E RID: 38542
			public string namespaceIfAmbiguous;
		}
	}
}
