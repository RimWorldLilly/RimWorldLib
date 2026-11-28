using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Verse
{
	// Token: 0x0200035A RID: 858
	public static class TKeySystem
	{
		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001A55 RID: 6741 RVA: 0x0009D789 File Offset: 0x0009B989
		private static bool ShouldUseHardcodedMapping
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x0009D78C File Offset: 0x0009B98C
		public static void Clear()
		{
			TKeySystem.keys.Clear();
			TKeySystem.tKeyToNormalizedTranslationKey.Clear();
			TKeySystem.translationKeyToTKey.Clear();
			TKeySystem.loadErrors.Clear();
			TKeySystem.treatAsList.Clear();
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x0009D7C0 File Offset: 0x0009B9C0
		public static void Parse(XmlDocument document)
		{
			foreach (object obj in document.ChildNodes[0].ChildNodes)
			{
				TKeySystem.ParseDefNode((XmlNode)obj);
			}
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x0009D824 File Offset: 0x0009BA24
		public static void MarkTreatAsList(XmlNode node)
		{
			TKeySystem.treatAsList.Add(node);
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x0009D832 File Offset: 0x0009BA32
		public static void BuildMappings()
		{
			if (TKeySystem.ShouldUseHardcodedMapping)
			{
				TKeySystem.tKeyToNormalizedTranslationKey.AddRange(TKeySystemHardcodedMapping.TKeyToNormalizedTranslationKey);
				TKeySystem.translationKeyToTKey.AddRange(TKeySystemHardcodedMapping.TranslationKeyToTKey);
				return;
			}
			TKeySystem.BuildMappings_Calculate();
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x0009D860 File Offset: 0x0009BA60
		private static void BuildMappings_Calculate()
		{
			Dictionary<string, string> tmpTranslationKeyToTKey = new Dictionary<string, string>();
			foreach (TKeySystem.TKeyRef tkeyRef in TKeySystem.keys)
			{
				string normalizedTranslationKey = TKeySystem.GetNormalizedTranslationKey(tkeyRef);
				string text;
				if (TKeySystem.tKeyToNormalizedTranslationKey.TryGetValue(tkeyRef.tKeyPath, out text))
				{
					TKeySystem.loadErrors.Add(string.Concat(new string[]
					{
						"Duplicate TKey: ",
						tkeyRef.tKeyPath,
						" -> NEW=",
						normalizedTranslationKey,
						" | OLD",
						text,
						" - Ignoring old"
					}));
				}
				else
				{
					TKeySystem.tKeyToNormalizedTranslationKey.Add(tkeyRef.tKeyPath, normalizedTranslationKey);
					tmpTranslationKeyToTKey.Add(normalizedTranslationKey, tkeyRef.tKeyPath);
				}
			}
			DefInjectionUtility.PossibleDefInjectionTraverser <>9__2;
			foreach (string typeName in (from k in TKeySystem.keys
			select k.defTypeName).Distinct<string>())
			{
				Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(typeName, null);
				DefInjectionUtility.PossibleDefInjectionTraverser action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate(string suggestedPath, string normalizedPath, bool isCollection, string currentValue, IEnumerable<string> currentValueCollection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def)
					{
						string text2;
						string text3;
						if (translationAllowed && !TKeySystem.TryGetNormalizedPath(suggestedPath, out text2) && TKeySystem.TrySuggestTKeyPath(normalizedPath, out text3, tmpTranslationKeyToTKey))
						{
							if (tmpTranslationKeyToTKey.ContainsKey(suggestedPath) && tmpTranslationKeyToTKey[suggestedPath] != text3)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(79, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Trying to add duplicate TKey with different path. '");
								defaultInterpolatedStringHandler.AppendFormatted(tmpTranslationKeyToTKey[suggestedPath]);
								defaultInterpolatedStringHandler.AppendLiteral("' will be overwritten by '");
								defaultInterpolatedStringHandler.AppendFormatted(text3);
								defaultInterpolatedStringHandler.AppendLiteral("'.");
								Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							tmpTranslationKeyToTKey.SetOrAdd(suggestedPath, text3);
						}
					});
				}
				DefInjectionUtility.ForEachPossibleDefInjection(typeInAnyAssembly, action, null);
			}
			foreach (KeyValuePair<string, string> keyValuePair in tmpTranslationKeyToTKey)
			{
				TKeySystem.translationKeyToTKey.Add(keyValuePair.Key, keyValuePair.Value);
			}
			TKeySystem.<BuildMappings_Calculate>g__Compare|14_0(TKeySystem.tKeyToNormalizedTranslationKey, TKeySystemHardcodedMapping.TKeyToNormalizedTranslationKey, "tKeyToNormalizedTranslationKey(calculated)", "TKeySystemHardcodedMapping.TKeyToNormalizedTranslationKey");
			TKeySystem.<BuildMappings_Calculate>g__Compare|14_0(TKeySystem.translationKeyToTKey, TKeySystemHardcodedMapping.TranslationKeyToTKey, "translationKeyToTKey(calculated)", "TKeySystemHardcodedMapping.TranslationKeyToTKey");
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x0009DA60 File Offset: 0x0009BC60
		public static bool TryGetNormalizedPath(string tKeyPath, out string normalizedPath)
		{
			return TKeySystem.TryFindShortestReplacementPath(tKeyPath, delegate(string path, out string result)
			{
				return TKeySystem.tKeyToNormalizedTranslationKey.TryGetValue(path, out result);
			}, out normalizedPath);
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x0009DA88 File Offset: 0x0009BC88
		private static bool TryFindShortestReplacementPath(string path, TKeySystem.PathMatcher matcher, out string result)
		{
			if (matcher(path, out result))
			{
				return true;
			}
			int num = 100;
			int num2 = path.Length - 1;
			for (;;)
			{
				if (num2 > 0 && path[num2] != '.')
				{
					num2--;
				}
				else
				{
					if (path[num2] == '.')
					{
						string path2 = path.Substring(0, num2);
						if (matcher(path2, out result))
						{
							break;
						}
					}
					num2--;
					num--;
					if (num2 <= 0 || num <= 0)
					{
						goto IL_6D;
					}
				}
			}
			result += path.Substring(num2);
			return true;
			IL_6D:
			result = null;
			return false;
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x0009DB08 File Offset: 0x0009BD08
		public static bool TrySuggestTKeyPath(string translationPath, out string tKeyPath, Dictionary<string, string> lookup = null)
		{
			if (lookup == null)
			{
				lookup = TKeySystem.translationKeyToTKey;
			}
			return TKeySystem.TryFindShortestReplacementPath(translationPath, delegate(string path, out string result)
			{
				return lookup.TryGetValue(path, out result);
			}, out tKeyPath);
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x0009DB48 File Offset: 0x0009BD48
		private static string GetNormalizedTranslationKey(TKeySystem.TKeyRef tKeyRef)
		{
			string text = "";
			XmlNode currentNode;
			for (currentNode = tKeyRef.node; currentNode != tKeyRef.defRootNode; currentNode = currentNode.ParentNode)
			{
				if (currentNode.Name == "li" || TKeySystem.treatAsList.Contains(currentNode.ParentNode))
				{
					text = "." + (from XmlNode n in currentNode.ParentNode.ChildNodes
					where TKeySystem.<GetNormalizedTranslationKey>g__ShouldConsiderNode|19_0(n)
					select n).FirstIndexOf((XmlNode n) => n == currentNode).ToString() + text;
				}
				else
				{
					text = "." + currentNode.Name + text;
				}
			}
			return tKeyRef.defName + text;
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x0009DC48 File Offset: 0x0009BE48
		private static void ParseDefNode(XmlNode node)
		{
			if (TKeySystem.ShouldUseHardcodedMapping)
			{
				return;
			}
			string text = null;
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "defName")
				{
					text = xmlNode.InnerText;
					break;
				}
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			TKeySystem.<>c__DisplayClass20_0 CS$<>8__locals1;
			CS$<>8__locals1.tKeyRefTemplate = default(TKeySystem.TKeyRef);
			CS$<>8__locals1.tKeyRefTemplate.defName = text;
			CS$<>8__locals1.tKeyRefTemplate.defTypeName = node.Name;
			CS$<>8__locals1.tKeyRefTemplate.defRootNode = node;
			TKeySystem.<ParseDefNode>g__CrawlNodesRecursive|20_1(node, ref CS$<>8__locals1);
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x0009DD40 File Offset: 0x0009BF40
		[CompilerGenerated]
		internal static void <BuildMappings_Calculate>g__Compare|14_0(Dictionary<string, string> a, Dictionary<string, string> b, string aName, string bName)
		{
			foreach (KeyValuePair<string, string> keyValuePair in a)
			{
				string b2;
				if (!b.TryGetValue(keyValuePair.Key, out b2))
				{
					Log.Warning(bName + " is missing " + keyValuePair.Key);
				}
				else if (keyValuePair.Value != b2)
				{
					Log.Warning(string.Concat(new string[]
					{
						bName,
						"'s value doesn't match for ",
						keyValuePair.Key,
						". Should be ",
						keyValuePair.Value
					}));
				}
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in b)
			{
				if (!a.ContainsKey(keyValuePair2.Key))
				{
					Log.Warning(string.Concat(new string[]
					{
						bName,
						" has ",
						keyValuePair2.Key,
						" but it's not present in ",
						aName,
						". We should most likely remove it from the list."
					}));
				}
			}
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x0009DE7C File Offset: 0x0009C07C
		[CompilerGenerated]
		internal static bool <GetNormalizedTranslationKey>g__ShouldConsiderNode|19_0(XmlNode node)
		{
			XmlAttribute xmlAttribute = node.Attributes["MayRequire"];
			string text = (xmlAttribute != null) ? xmlAttribute.Value : null;
			if (text != null && !ModLister.AllModsActiveNoSuffix(text.Split(',', StringSplitOptions.None)))
			{
				return false;
			}
			XmlAttributeCollection attributes = node.Attributes;
			string[] array;
			if (attributes == null)
			{
				array = null;
			}
			else
			{
				XmlAttribute xmlAttribute2 = attributes["MayRequire"];
				array = ((xmlAttribute2 != null) ? xmlAttribute2.Value.ToLower().Split(',', StringSplitOptions.None) : null);
			}
			string[] array2 = array;
			return array2.NullOrEmpty<string>() || ModLister.AnyModActiveNoSuffix(array2);
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x0009DF00 File Offset: 0x0009C100
		[CompilerGenerated]
		internal static void <ParseDefNode>g__ProcessNode|20_0(XmlNode n, ref TKeySystem.<>c__DisplayClass20_0 A_1)
		{
			XmlAttribute xmlAttribute;
			if (n.Attributes != null && (xmlAttribute = n.Attributes["TKey"]) != null)
			{
				TKeySystem.TKeyRef tKeyRefTemplate = A_1.tKeyRefTemplate;
				tKeyRefTemplate.tKey = xmlAttribute.Value;
				tKeyRefTemplate.node = n;
				tKeyRefTemplate.tKeyPath = tKeyRefTemplate.defName + "." + tKeyRefTemplate.tKey;
				TKeySystem.keys.Add(tKeyRefTemplate);
			}
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x0009DF70 File Offset: 0x0009C170
		[CompilerGenerated]
		internal static void <ParseDefNode>g__CrawlNodesRecursive|20_1(XmlNode n, ref TKeySystem.<>c__DisplayClass20_0 A_1)
		{
			TKeySystem.<ParseDefNode>g__ProcessNode|20_0(n, ref A_1);
			foreach (object obj in n.ChildNodes)
			{
				TKeySystem.<ParseDefNode>g__CrawlNodesRecursive|20_1((XmlNode)obj, ref A_1);
			}
		}

		// Token: 0x0400156E RID: 5486
		private static List<TKeySystem.TKeyRef> keys = new List<TKeySystem.TKeyRef>();

		// Token: 0x0400156F RID: 5487
		public static List<string> loadErrors = new List<string>();

		// Token: 0x04001570 RID: 5488
		private static HashSet<XmlNode> treatAsList = new HashSet<XmlNode>();

		// Token: 0x04001571 RID: 5489
		private static Dictionary<string, string> tKeyToNormalizedTranslationKey = new Dictionary<string, string>();

		// Token: 0x04001572 RID: 5490
		private static Dictionary<string, string> translationKeyToTKey = new Dictionary<string, string>();

		// Token: 0x04001573 RID: 5491
		public const string AttributeName = "TKey";

		// Token: 0x02002688 RID: 9864
		private struct TKeyRef
		{
			// Token: 0x04009DDC RID: 40412
			public string defName;

			// Token: 0x04009DDD RID: 40413
			public string defTypeName;

			// Token: 0x04009DDE RID: 40414
			public XmlNode defRootNode;

			// Token: 0x04009DDF RID: 40415
			public XmlNode node;

			// Token: 0x04009DE0 RID: 40416
			public string tKey;

			// Token: 0x04009DE1 RID: 40417
			public string tKeyPath;
		}

		// Token: 0x02002689 RID: 9865
		private struct PossibleDefInjection
		{
			// Token: 0x04009DE2 RID: 40418
			public string normalizedPath;

			// Token: 0x04009DE3 RID: 40419
			public string path;
		}

		// Token: 0x0200268A RID: 9866
		// (Invoke) Token: 0x0600F40D RID: 62477
		private delegate bool PathMatcher(string path, out string match);
	}
}
