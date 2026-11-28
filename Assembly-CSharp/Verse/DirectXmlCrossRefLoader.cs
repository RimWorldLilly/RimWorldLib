using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Verse
{
	// Token: 0x0200050A RID: 1290
	public static class DirectXmlCrossRefLoader
	{
		// Token: 0x06002631 RID: 9777 RVA: 0x000E890D File Offset: 0x000E6B0D
		public static bool MistypedMayRequire(string mayRequireMod)
		{
			return false;
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x000E8910 File Offset: 0x000E6B10
		public static bool LoadingInProgress
		{
			get
			{
				return DirectXmlCrossRefLoader.wantedRefs.Count > 0;
			}
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x000E8920 File Offset: 0x000E6B20
		public static void RegisterObjectWantsCrossRef(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, string mayRequireAnyMod = null, Type assumeFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object, FieldInfo, string)");
			try
			{
				if (wanter.GetType().IsValueType)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(112, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot use value types for object cross reference. ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(wanter.GetType());
					defaultInterpolatedStringHandler.AppendLiteral(" is a value type but wants a cross reference to ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(fi.FieldType);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted(targetDefName);
					defaultInterpolatedStringHandler.AppendLiteral(" via field ");
					defaultInterpolatedStringHandler.AppendFormatted(fi.Name);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, fi, targetDefName, mayRequireMod, mayRequireAnyMod, assumeFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000E89F8 File Offset: 0x000E6BF8
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, string targetDefName, string mayRequireMod = null, string mayRequireAnyMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,string)");
			try
			{
				FieldInfo field = wanter.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (wanter.GetType().IsValueType)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(112, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot use value types for object cross reference. ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(wanter.GetType());
					defaultInterpolatedStringHandler.AppendLiteral(" is a value type but wants a cross reference to ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>((field != null) ? field.FieldType : null);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted(targetDefName);
					defaultInterpolatedStringHandler.AppendLiteral(" via field ");
					defaultInterpolatedStringHandler.AppendFormatted(((field != null) ? field.Name : null) ?? fieldName);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, field, targetDefName, mayRequireMod, mayRequireAnyMod, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000E8AF0 File Offset: 0x000E6CF0
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, XmlNode parentNode, string mayRequireMod = null, string mayRequireAnyMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,XmlNode)");
			try
			{
				if (wanter.GetType().IsValueType)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(111, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot use value types for object cross reference. ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(wanter.GetType());
					defaultInterpolatedStringHandler.AppendLiteral(" is a value type but wants a cross reference to ");
					defaultInterpolatedStringHandler.AppendFormatted(parentNode.Name);
					defaultInterpolatedStringHandler.AppendLiteral(" via field ");
					defaultInterpolatedStringHandler.AppendFormatted(fieldName);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string text = mayRequireMod;
				if (mayRequireMod == null)
				{
					XmlAttributeCollection attributes = parentNode.Attributes;
					if (attributes == null)
					{
						text = null;
					}
					else
					{
						XmlAttribute xmlAttribute = attributes["MayRequire"];
						text = ((xmlAttribute != null) ? xmlAttribute.Value.ToLower() : null);
					}
				}
				string mayRequireMod2 = text;
				string text2 = mayRequireAnyMod;
				if (mayRequireAnyMod == null)
				{
					XmlAttributeCollection attributes2 = parentNode.Attributes;
					if (attributes2 == null)
					{
						text2 = null;
					}
					else
					{
						XmlAttribute xmlAttribute2 = attributes2["MayRequireAnyOf"];
						text2 = ((xmlAttribute2 != null) ? xmlAttribute2.Value.ToLower() : null);
					}
				}
				string mayRequireAnyMod2 = text2;
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), parentNode.Name, mayRequireMod2, mayRequireAnyMod2, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000E8C18 File Offset: 0x000E6E18
		public static void RegisterListWantsCrossRef<T>(List<T> wanterList, string targetDefName, object debugWanterInfo = null, string mayRequireMod = null, string mayRequireAnyMod = null)
		{
			DeepProfiler.Start("RegisterListWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForList<T> wantedRefForList;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterList, out wantedRef))
				{
					wantedRefForList = new DirectXmlCrossRefLoader.WantedRefForList<T>(wanterList, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterList, wantedRefForList);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForList);
				}
				else
				{
					wantedRefForList = (DirectXmlCrossRefLoader.WantedRefForList<T>)wantedRef;
				}
				wantedRefForList.AddWantedListEntry(targetDefName, mayRequireMod, mayRequireAnyMod);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000E8C8C File Offset: 0x000E6E8C
		public static void RegisterDictionaryWantsCrossRef<K, V>(Dictionary<K, V> wanterDict, XmlNode entryNode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("RegisterDictionaryWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForDictionary<K, V> wantedRefForDictionary;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterDict, out wantedRef))
				{
					wantedRefForDictionary = new DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>(wanterDict, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForDictionary);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterDict, wantedRefForDictionary);
				}
				else
				{
					wantedRefForDictionary = (DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>)wantedRef;
				}
				wantedRefForDictionary.AddWantedDictEntry(entryNode);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000E8CFC File Offset: 0x000E6EFC
		public static T TryResolveDef<T>(string defName, FailMode failReportMode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("TryResolveDef");
			T result;
			try
			{
				T t = (T)((object)GenDefDatabase.GetDefSilentFail(typeof(T), defName, true));
				if (t != null)
				{
					result = t;
				}
				else
				{
					if (failReportMode == FailMode.LogErrors)
					{
						string str = "Could not resolve cross-reference to ";
						Type typeFromHandle = typeof(T);
						string text = str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " named " + defName.ToStringSafe<string>();
						if (debugWanterInfo != null)
						{
							text = text + " (wanter=" + debugWanterInfo.ToStringSafe<object>() + ")";
						}
						Log.Error(text);
					}
					result = default(T);
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			return result;
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x000E8DAC File Offset: 0x000E6FAC
		public static void Clear()
		{
			DeepProfiler.Start("Clear");
			try
			{
				DirectXmlCrossRefLoader.wantedRefs.Clear();
				DirectXmlCrossRefLoader.wantedListDictRefs.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000E8DF0 File Offset: 0x000E6FF0
		public static void ResolveAllWantedCrossReferences(FailMode failReportMode)
		{
			DeepProfiler.Start("ResolveAllWantedCrossReferences");
			try
			{
				HashSet<DirectXmlCrossRefLoader.WantedRef> resolvedRefs = new HashSet<DirectXmlCrossRefLoader.WantedRef>();
				object resolvedRefsLock = new object();
				DeepProfiler.enabled = false;
				GenThreading.ParallelForEach<DirectXmlCrossRefLoader.WantedRef>(DirectXmlCrossRefLoader.wantedRefs, delegate(DirectXmlCrossRefLoader.WantedRef wantedRef)
				{
					if (wantedRef.TryResolve(failReportMode))
					{
						object resolvedRefsLock = resolvedRefsLock;
						lock (resolvedRefsLock)
						{
							resolvedRefs.Add(wantedRef);
						}
					}
				}, -1);
				foreach (DirectXmlCrossRefLoader.WantedRef wantedRef2 in resolvedRefs)
				{
					wantedRef2.Apply();
				}
				DirectXmlCrossRefLoader.wantedRefs.RemoveAll((DirectXmlCrossRefLoader.WantedRef x) => resolvedRefs.Contains(x));
				DeepProfiler.enabled = true;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x04001B81 RID: 7041
		private static List<DirectXmlCrossRefLoader.WantedRef> wantedRefs = new List<DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x04001B82 RID: 7042
		private static Dictionary<object, DirectXmlCrossRefLoader.WantedRef> wantedListDictRefs = new Dictionary<object, DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x02002753 RID: 10067
		private abstract class WantedRef
		{
			// Token: 0x0600F7C0 RID: 63424
			public abstract bool TryResolve(FailMode failReportMode);

			// Token: 0x0600F7C1 RID: 63425 RVA: 0x0059EEDC File Offset: 0x0059D0DC
			public virtual void Apply()
			{
			}

			// Token: 0x0400A0EE RID: 41198
			public object wanter;
		}

		// Token: 0x02002754 RID: 10068
		private class WantedRefForObject : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x17002B57 RID: 11095
			// (get) Token: 0x0600F7C3 RID: 63427 RVA: 0x0059EEE8 File Offset: 0x0059D0E8
			private bool BadCrossRefAllowed
			{
				get
				{
					return (!this.mayRequireMod.NullOrEmpty() && !ModLister.AllModsActiveNoSuffix(this.mayRequireMod.Split(',', StringSplitOptions.None))) || (!this.mayRequireAnyMod.NullOrEmpty<string>() && !ModLister.AnyModActiveNoSuffix(this.mayRequireAnyMod));
				}
			}

			// Token: 0x0600F7C4 RID: 63428 RVA: 0x0059EF38 File Offset: 0x0059D138
			public WantedRefForObject(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, string mayRequireAnyMod = null, Type overrideFieldType = null)
			{
				this.wanter = wanter;
				this.fi = fi;
				this.defName = targetDefName;
				this.mayRequireMod = mayRequireMod;
				this.overrideFieldType = overrideFieldType;
				this.mayRequireAnyMod = ((mayRequireAnyMod != null) ? mayRequireAnyMod.ToLower().Split(',', StringSplitOptions.None) : null);
			}

			// Token: 0x0600F7C5 RID: 63429 RVA: 0x0059EF8C File Offset: 0x0059D18C
			public override bool TryResolve(FailMode failReportMode)
			{
				if (this.fi == null)
				{
					Log.Error("Trying to resolve null field for def named " + this.defName.ToStringSafe<string>());
					return false;
				}
				Type type = this.overrideFieldType ?? this.fi.FieldType;
				this.resolvedDef = GenDefDatabase.GetDefSilentFail(type, this.defName, true);
				if (DirectXmlCrossRefLoader.MistypedMayRequire(this.mayRequireMod))
				{
					Log.Error("Faulty MayRequire at def " + this.defName.ToStringSafe<string>() + ": " + this.mayRequireMod);
				}
				if (!this.mayRequireAnyMod.NullOrEmpty<string>())
				{
					foreach (string str in this.mayRequireAnyMod)
					{
						if (DirectXmlCrossRefLoader.MistypedMayRequire(str))
						{
							Log.Error("Faulty MayRequire at def " + this.defName.ToStringSafe<string>() + ": " + str);
						}
					}
				}
				if (this.resolvedDef == null)
				{
					if (failReportMode == FailMode.LogErrors && !this.BadCrossRefAllowed)
					{
						string[] array2 = new string[8];
						array2[0] = "Could not resolve cross-reference: No ";
						int num = 1;
						Type type2 = type;
						array2[num] = ((type2 != null) ? type2.ToString() : null);
						array2[2] = " named ";
						array2[3] = this.defName.ToStringSafe<string>();
						array2[4] = " found to give to ";
						int num2 = 5;
						Type type3 = this.wanter.GetType();
						array2[num2] = ((type3 != null) ? type3.ToString() : null);
						array2[6] = " ";
						array2[7] = this.wanter.ToStringSafe<object>();
						Log.Error(string.Concat(array2));
					}
					return false;
				}
				SoundDef soundDef = this.resolvedDef as SoundDef;
				if (soundDef != null && soundDef.isUndefined)
				{
					string[] array3 = new string[9];
					array3[0] = "Could not resolve cross-reference: No ";
					int num3 = 1;
					Type type4 = type;
					array3[num3] = ((type4 != null) ? type4.ToString() : null);
					array3[2] = " named ";
					array3[3] = this.defName.ToStringSafe<string>();
					array3[4] = " found to give to ";
					int num4 = 5;
					Type type5 = this.wanter.GetType();
					array3[num4] = ((type5 != null) ? type5.ToString() : null);
					array3[6] = " ";
					array3[7] = this.wanter.ToStringSafe<object>();
					array3[8] = " (using undefined sound instead)";
					Log.Warning(string.Concat(array3));
				}
				this.fi.SetValue(this.wanter, this.resolvedDef);
				return true;
			}

			// Token: 0x0400A0EF RID: 41199
			private readonly FieldInfo fi;

			// Token: 0x0400A0F0 RID: 41200
			private readonly string defName;

			// Token: 0x0400A0F1 RID: 41201
			private readonly string mayRequireMod;

			// Token: 0x0400A0F2 RID: 41202
			private readonly string[] mayRequireAnyMod;

			// Token: 0x0400A0F3 RID: 41203
			private readonly Type overrideFieldType;

			// Token: 0x0400A0F4 RID: 41204
			private Def resolvedDef;
		}

		// Token: 0x02002755 RID: 10069
		private class WantedRefForList<T> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x0600F7C6 RID: 63430 RVA: 0x0059F1AF File Offset: 0x0059D3AF
			public WantedRefForList(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x0600F7C7 RID: 63431 RVA: 0x0059F1D0 File Offset: 0x0059D3D0
			public void AddWantedListEntry(string newTargetDefName, string mayRequireMod = null, string mayRequireAnyMod = null)
			{
				if (!mayRequireMod.NullOrEmpty() && this.mayRequireMods == null)
				{
					this.mayRequireMods = new List<string>();
					for (int i = 0; i < this.defNames.Count; i++)
					{
						this.mayRequireMods.Add(null);
					}
				}
				if (!mayRequireAnyMod.NullOrEmpty() && this.mayRequireModsAny == null)
				{
					this.mayRequireModsAny = new Dictionary<string, List<string>>();
					for (int j = 0; j < this.defNames.Count; j++)
					{
						this.mayRequireModsAny.Add(this.defNames[j], new List<string>());
					}
				}
				this.defNames.Add(newTargetDefName);
				if (this.mayRequireMods != null)
				{
					this.mayRequireMods.Add(mayRequireMod);
				}
				if (this.mayRequireModsAny != null && mayRequireAnyMod != null)
				{
					foreach (string text in mayRequireAnyMod.ToLower().Split(',', StringSplitOptions.None))
					{
						List<string> list;
						if (this.mayRequireModsAny.TryGetValue(newTargetDefName, out list))
						{
							list.Add(text.Trim());
						}
						else
						{
							this.mayRequireModsAny.Add(newTargetDefName, new List<string>
							{
								text.Trim()
							});
						}
					}
				}
			}

			// Token: 0x0600F7C8 RID: 63432 RVA: 0x0059F2F4 File Offset: 0x0059D4F4
			public override bool TryResolve(FailMode failReportMode)
			{
				bool flag = false;
				for (int i = 0; i < this.defNames.Count; i++)
				{
					bool flag2 = this.mayRequireMods != null && i < this.mayRequireMods.Count && !this.mayRequireMods[i].NullOrEmpty() && !ModLister.AllModsActiveNoSuffix(this.mayRequireMods[i].Split(',', StringSplitOptions.None));
					List<string> modIds;
					if (this.mayRequireModsAny != null && this.mayRequireModsAny.TryGetValue(this.defNames[i], out modIds) && !ModLister.AnyModActiveNoSuffix(modIds))
					{
						flag2 = true;
					}
					if (this.mayRequireMods != null && i < this.mayRequireMods.Count && DirectXmlCrossRefLoader.MistypedMayRequire(this.mayRequireMods[i]))
					{
						Log.Error("Faulty MayRequire: " + this.mayRequireMods[i]);
					}
					T t = DirectXmlCrossRefLoader.TryResolveDef<T>(this.defNames[i], flag2 ? FailMode.Silent : failReportMode, this.debugWanterInfo);
					if (t != null)
					{
						((List<T>)this.wanter).Add(t);
						this.defNames.RemoveAt(i);
						if (this.mayRequireMods != null && i < this.mayRequireMods.Count)
						{
							this.mayRequireMods.RemoveAt(i);
						}
						i--;
					}
					else
					{
						flag = true;
					}
				}
				return !flag;
			}

			// Token: 0x0400A0F5 RID: 41205
			private List<string> defNames = new List<string>();

			// Token: 0x0400A0F6 RID: 41206
			private List<string> mayRequireMods;

			// Token: 0x0400A0F7 RID: 41207
			private Dictionary<string, List<string>> mayRequireModsAny;

			// Token: 0x0400A0F8 RID: 41208
			private object debugWanterInfo;
		}

		// Token: 0x02002756 RID: 10070
		private class WantedRefForDictionary<K, V> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x0600F7C9 RID: 63433 RVA: 0x0059F44C File Offset: 0x0059D64C
			public WantedRefForDictionary(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x0600F7CA RID: 63434 RVA: 0x0059F478 File Offset: 0x0059D678
			public void AddWantedDictEntry(XmlNode entryNode)
			{
				this.wantedDictRefs.Add(entryNode);
			}

			// Token: 0x0600F7CB RID: 63435 RVA: 0x0059F488 File Offset: 0x0059D688
			public override bool TryResolve(FailMode failReportMode)
			{
				failReportMode = FailMode.LogErrors;
				bool flag = GenTypes.IsDef(typeof(K));
				bool flag2 = GenTypes.IsDef(typeof(V));
				foreach (XmlNode xmlNode in this.wantedDictRefs)
				{
					XmlNode xmlNode2 = xmlNode["key"];
					XmlNode xmlNode3 = xmlNode["value"];
					string text = (xmlNode2 != null) ? xmlNode2.InnerText : null;
					string text2 = (xmlNode3 != null) ? xmlNode3.InnerText : null;
					object first;
					object second;
					if (text == null || text2 == null)
					{
						if (failReportMode == FailMode.LogErrors)
						{
							string text3 = "Missing 'key' and/or 'value'.";
							if (this.debugWanterInfo != null)
							{
								text3 = text3 + " (wanter=" + this.debugWanterInfo.ToStringSafe<object>() + ")";
							}
							Log.Error(text3);
						}
						first = default(K);
						second = default(V);
					}
					else
					{
						if (flag)
						{
							first = DirectXmlCrossRefLoader.TryResolveDef<K>(text, failReportMode, this.debugWanterInfo);
						}
						else
						{
							first = xmlNode2;
						}
						if (flag2)
						{
							second = DirectXmlCrossRefLoader.TryResolveDef<V>(text2, failReportMode, this.debugWanterInfo);
						}
						else
						{
							second = xmlNode3;
						}
					}
					this.makingData.Add(new Pair<object, object>(first, second));
				}
				return true;
			}

			// Token: 0x0600F7CC RID: 63436 RVA: 0x0059F5F4 File Offset: 0x0059D7F4
			public override void Apply()
			{
				Dictionary<K, V> dictionary = (Dictionary<K, V>)this.wanter;
				dictionary.Clear();
				foreach (Pair<object, object> pair in this.makingData)
				{
					try
					{
						object obj = pair.First;
						object obj2 = pair.Second;
						if (obj is XmlNode)
						{
							obj = DirectXmlToObject.ObjectFromXml<K>(obj as XmlNode, true);
						}
						if (obj2 is XmlNode)
						{
							obj2 = DirectXmlToObject.ObjectFromXml<V>(obj2 as XmlNode, true);
						}
						dictionary.Add((K)((object)obj), (V)((object)obj2));
					}
					catch
					{
						string str = "Failed to load key/value pair: ";
						object first = pair.First;
						string str2 = (first != null) ? first.ToString() : null;
						string str3 = ", ";
						object second = pair.Second;
						Log.Error(str + str2 + str3 + ((second != null) ? second.ToString() : null));
					}
				}
			}

			// Token: 0x0400A0F9 RID: 41209
			private List<XmlNode> wantedDictRefs = new List<XmlNode>();

			// Token: 0x0400A0FA RID: 41210
			private object debugWanterInfo;

			// Token: 0x0400A0FB RID: 41211
			private List<Pair<object, object>> makingData = new List<Pair<object, object>>();
		}
	}
}
