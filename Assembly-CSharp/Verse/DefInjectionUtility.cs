using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld.QuestGen;

namespace Verse
{
	// Token: 0x0200020E RID: 526
	public static class DefInjectionUtility
	{
		// Token: 0x06000F45 RID: 3909 RVA: 0x00054E14 File Offset: 0x00053014
		public static void ForEachPossibleDefInjection(Type defType, DefInjectionUtility.PossibleDefInjectionTraverser action, ModMetaData onlyFromMod = null)
		{
			foreach (Def def in GenDefDatabase.GetAllDefsInDatabaseForDef(defType))
			{
				if (onlyFromMod == null || (def.modContentPack != null && !(def.modContentPack.PackageId != onlyFromMod.PackageId)))
				{
					DefInjectionUtility.ForEachPossibleDefInjectionInDef(def, action);
				}
			}
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x00054E84 File Offset: 0x00053084
		private static void ForEachPossibleDefInjectionInDef(Def def, DefInjectionUtility.PossibleDefInjectionTraverser action)
		{
			HashSet<object> visited = new HashSet<object>();
			DefInjectionUtility.ForEachPossibleDefInjectionInDefRecursive(def, def.defName, def.defName, visited, true, def, action);
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x00054EB0 File Offset: 0x000530B0
		private static void ForEachPossibleDefInjectionInDefRecursive(object obj, string curNormalizedPath, string curSuggestedPath, HashSet<object> visited, bool translationAllowed, Def def, DefInjectionUtility.PossibleDefInjectionTraverser action)
		{
			if (obj == null)
			{
				return;
			}
			if (obj is Thing)
			{
				return;
			}
			if (!obj.GetType().IsValueType && visited.Contains(obj))
			{
				return;
			}
			visited.Add(obj);
			foreach (FieldInfo fieldInfo in DefInjectionUtility.FieldsInDeterministicOrder(obj.GetType()))
			{
				object value = fieldInfo.GetValue(obj);
				bool flag = translationAllowed && !fieldInfo.HasAttribute<NoTranslateAttribute>() && !fieldInfo.HasAttribute<UnsavedAttribute>();
				if (!(value is Def))
				{
					if (typeof(string).IsAssignableFrom(fieldInfo.FieldType))
					{
						string currentValue = (string)value;
						string text = curNormalizedPath + "." + fieldInfo.Name;
						string suggestedPath = curSuggestedPath + "." + fieldInfo.Name;
						string text2;
						if (TKeySystem.TrySuggestTKeyPath(text, out text2, null))
						{
							suggestedPath = text2;
						}
						action(suggestedPath, text, false, currentValue, null, flag, false, fieldInfo, def);
					}
					else
					{
						IEnumerable<string> enumerable = value as IEnumerable<string>;
						if (enumerable != null)
						{
							bool flag2 = fieldInfo.HasAttribute<TranslationCanChangeCountAttribute>();
							string text3 = curNormalizedPath + "." + fieldInfo.Name;
							string suggestedPath2 = curSuggestedPath + "." + fieldInfo.Name;
							string text4;
							if (TKeySystem.TrySuggestTKeyPath(text3, out text4, null))
							{
								suggestedPath2 = text4;
							}
							action(suggestedPath2, text3, true, null, enumerable, flag, flag && flag2, fieldInfo, def);
						}
						else
						{
							IEnumerable enumerable2 = value as IEnumerable;
							if (enumerable2 != null)
							{
								int num = 0;
								using (IEnumerator enumerator2 = enumerable2.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj2 = enumerator2.Current;
										if (obj2 != null && !(obj2 is Def) && GenTypes.IsCustomType(obj2.GetType()))
										{
											string text5 = TranslationHandleUtility.GetBestHandleWithIndexForListElement(enumerable2, obj2);
											if (text5.NullOrEmpty())
											{
												text5 = num.ToString();
											}
											string curNormalizedPath2 = string.Concat(new string[]
											{
												curNormalizedPath,
												".",
												fieldInfo.Name,
												".",
												num.ToString()
											});
											string curSuggestedPath2 = string.Concat(new string[]
											{
												curSuggestedPath,
												".",
												fieldInfo.Name,
												".",
												text5
											});
											DefInjectionUtility.ForEachPossibleDefInjectionInDefRecursive(obj2, curNormalizedPath2, curSuggestedPath2, visited, flag, def, action);
										}
										num++;
									}
									continue;
								}
							}
							if (value != null && GenTypes.IsCustomType(value.GetType()))
							{
								string curNormalizedPath3 = curNormalizedPath + "." + fieldInfo.Name;
								string curSuggestedPath3 = curSuggestedPath + "." + fieldInfo.Name;
								DefInjectionUtility.ForEachPossibleDefInjectionInDefRecursive(value, curNormalizedPath3, curSuggestedPath3, visited, flag, def, action);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x000551A4 File Offset: 0x000533A4
		public static bool ShouldCheckMissingInjection(string str, FieldInfo fi, Def def)
		{
			if (def.generated)
			{
				return false;
			}
			if (str.NullOrEmpty())
			{
				return false;
			}
			if (fi.HasAttribute<NoTranslateAttribute>() || fi.HasAttribute<UnsavedAttribute>() || fi.HasAttribute<MayTranslateAttribute>())
			{
				return false;
			}
			if (fi.HasAttribute<MustTranslate_SlateRefAttribute>())
			{
				return SlateRefUtility.MustTranslate(str, fi);
			}
			return fi.HasAttribute<MustTranslateAttribute>() || str.Contains(' ');
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00055204 File Offset: 0x00053404
		private static List<FieldInfo> FieldsInDeterministicOrder(Type type)
		{
			List<FieldInfo> result;
			if (DefInjectionUtility.cachedFieldsInDeterministicOrder.TryGetValue(type, out result))
			{
				return result;
			}
			List<FieldInfo> list = (from x in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			orderby x.HasAttribute<UnsavedAttribute>() || x.HasAttribute<NoTranslateAttribute>(), x.Name == "label" descending, x.Name == "description" descending, x.Name
			select x).ToList<FieldInfo>();
			DefInjectionUtility.cachedFieldsInDeterministicOrder.Add(type, list);
			return list;
		}

		// Token: 0x04000EB5 RID: 3765
		private static Dictionary<Type, List<FieldInfo>> cachedFieldsInDeterministicOrder = new Dictionary<Type, List<FieldInfo>>();

		// Token: 0x0200259A RID: 9626
		// (Invoke) Token: 0x0600F024 RID: 61476
		public delegate void PossibleDefInjectionTraverser(string suggestedPath, string normalizedPath, bool isCollection, string currentValue, IEnumerable<string> currentValueCollection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def);
	}
}
