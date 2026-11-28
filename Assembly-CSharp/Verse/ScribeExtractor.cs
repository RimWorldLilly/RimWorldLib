using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000524 RID: 1316
	public static class ScribeExtractor
	{
		// Token: 0x060026E8 RID: 9960 RVA: 0x000F1918 File Offset: 0x000EFB18
		public static T ValueFromNode<T>(XmlNode subNode, T defaultValue)
		{
			if (subNode == null)
			{
				return defaultValue;
			}
			XmlAttribute xmlAttribute = subNode.Attributes["IsNull"];
			T result;
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				result = default(T);
				return result;
			}
			try
			{
				try
				{
					return ParseHelper.FromString<T>(subNode.InnerText);
				}
				catch (Exception ex)
				{
					string[] array = new string[6];
					array[0] = "Exception parsing node ";
					array[1] = subNode.OuterXml;
					array[2] = " into a ";
					int num = 3;
					Type typeFromHandle = typeof(T);
					array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
					array[4] = ":\n";
					array[5] = ex.ToString();
					Log.Error(string.Concat(array));
				}
				result = default(T);
			}
			catch (Exception ex2)
			{
				string str = "Exception loading XML: ";
				Exception ex3 = ex2;
				Log.Error(str + ((ex3 != null) ? ex3.ToString() : null));
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x000F1A10 File Offset: 0x000EFC10
		public static T DefFromNode<T>(XmlNode subNode) where T : Def, new()
		{
			if (subNode == null || subNode.InnerText == null || subNode.InnerText == "null")
			{
				return default(T);
			}
			string text = BackCompatibility.BackCompatibleDefName(typeof(T), subNode.InnerText, false, subNode);
			T namedSilentFail = DefDatabase<T>.GetNamedSilentFail(text);
			if (namedSilentFail == null && !BackCompatibility.WasDefRemoved(subNode.InnerText, typeof(T)))
			{
				if (text == subNode.InnerText)
				{
					string str = "Could not load reference to ";
					Type typeFromHandle = typeof(T);
					Log.Error(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " named " + subNode.InnerText);
				}
				else
				{
					string[] array = new string[6];
					array[0] = "Could not load reference to ";
					int num = 1;
					Type typeFromHandle2 = typeof(T);
					array[num] = ((typeFromHandle2 != null) ? typeFromHandle2.ToString() : null);
					array[2] = " named ";
					array[3] = subNode.InnerText;
					array[4] = " after compatibility-conversion to ";
					array[5] = text;
					Log.Error(string.Concat(array));
				}
				BackCompatibility.PostCouldntLoadDef(subNode.InnerText);
			}
			return namedSilentFail;
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x000F1B24 File Offset: 0x000EFD24
		public static T DefFromNodeUnsafe<T>(XmlNode subNode)
		{
			Func<XmlNode, Def> func;
			if (!ScribeExtractor.defFromNodeCached.TryGetValue(typeof(T), out func))
			{
				MethodInfo method = typeof(ScribeExtractor).GetMethod("DefFromNode", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
				{
					typeof(T)
				});
				func = (Func<XmlNode, Def>)Delegate.CreateDelegate(typeof(Func<XmlNode, Def>), method);
				ScribeExtractor.defFromNodeCached.Add(typeof(T), func);
			}
			return (T)((object)func(subNode));
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x000F1BB0 File Offset: 0x000EFDB0
		private static IExposable CreateInstance(Type type, object[] ctorArgs)
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			return (IExposable)Activator.CreateInstance(type, bindingAttr, null, ctorArgs, null, null);
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x000F1BD0 File Offset: 0x000EFDD0
		public static T SaveableFromNode<T>(XmlNode subNode, object[] ctorArgs)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error("Called SaveableFromNode(), but mode is " + Scribe.mode.ToString());
				return default(T);
			}
			if (subNode == null)
			{
				return default(T);
			}
			XmlAttribute xmlAttribute = subNode.Attributes["IsNull"];
			T result;
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				result = default(T);
			}
			else
			{
				try
				{
					XmlAttribute xmlAttribute2 = subNode.Attributes["Class"];
					string text = (xmlAttribute2 != null) ? xmlAttribute2.Value : typeof(T).FullName;
					Type type = BackCompatibility.GetBackCompatibleType(typeof(T), text, subNode);
					if (type == null)
					{
						Type bestFallbackType = ScribeExtractor.GetBestFallbackType<T>(subNode);
						string[] array = new string[8];
						array[0] = "Could not find class ";
						array[1] = text;
						array[2] = " while resolving node ";
						array[3] = subNode.Name;
						array[4] = ". Trying to use ";
						int num = 5;
						Type type2 = bestFallbackType;
						array[num] = ((type2 != null) ? type2.ToString() : null);
						array[6] = " instead. Full node: ";
						array[7] = subNode.OuterXml;
						Log.Error(string.Concat(array));
						type = bestFallbackType;
					}
					if (type.IsAbstract)
					{
						string str = "Can't load abstract class ";
						Type type3 = type;
						throw new ArgumentException(str + ((type3 != null) ? type3.ToString() : null));
					}
					IExposable exposable = ScribeExtractor.CreateInstance(type, ctorArgs);
					bool flag = typeof(T).IsValueType || typeof(Name).IsAssignableFrom(typeof(T));
					if (!flag)
					{
						Scribe.loader.crossRefs.RegisterForCrossRefResolve(exposable);
					}
					XmlNode curXmlParent = Scribe.loader.curXmlParent;
					IExposable curParent = Scribe.loader.curParent;
					string curPathRelToParent = Scribe.loader.curPathRelToParent;
					Scribe.loader.curXmlParent = subNode;
					Scribe.loader.curParent = exposable;
					Scribe.loader.curPathRelToParent = null;
					try
					{
						exposable.ExposeData();
					}
					finally
					{
						Scribe.loader.curXmlParent = curXmlParent;
						Scribe.loader.curParent = curParent;
						Scribe.loader.curPathRelToParent = curPathRelToParent;
					}
					if (!flag)
					{
						Scribe.loader.initer.RegisterForPostLoadInit(exposable);
					}
					result = (T)((object)exposable);
				}
				catch (Exception ex)
				{
					result = default(T);
					string str2 = "SaveableFromNode exception: ";
					Exception ex2 = ex;
					Log.Error(str2 + ((ex2 != null) ? ex2.ToString() : null) + "\nSubnode:\n" + subNode.OuterXml);
				}
			}
			return result;
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000F1E70 File Offset: 0x000F0070
		private static Type GetBestFallbackType<T>(XmlNode node)
		{
			if (typeof(Thing).IsAssignableFrom(typeof(T)))
			{
				ThingDef thingDef = ScribeExtractor.TryFindDef<ThingDef>(node, "def");
				if (thingDef != null)
				{
					return thingDef.thingClass;
				}
			}
			else if (typeof(Hediff).IsAssignableFrom(typeof(T)))
			{
				HediffDef hediffDef = ScribeExtractor.TryFindDef<HediffDef>(node, "def");
				if (hediffDef != null)
				{
					return hediffDef.hediffClass;
				}
			}
			else if (typeof(Ability).IsAssignableFrom(typeof(T)))
			{
				AbilityDef abilityDef = ScribeExtractor.TryFindDef<AbilityDef>(node, "def");
				if (abilityDef != null)
				{
					return abilityDef.abilityClass;
				}
			}
			else if (typeof(Thought).IsAssignableFrom(typeof(T)))
			{
				ThoughtDef thoughtDef = ScribeExtractor.TryFindDef<ThoughtDef>(node, "def");
				if (thoughtDef != null)
				{
					return thoughtDef.thoughtClass;
				}
			}
			return typeof(T);
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x000F1F50 File Offset: 0x000F0150
		private static TDef TryFindDef<TDef>(XmlNode node, string defNodeName) where TDef : Def, new()
		{
			XmlElement xmlElement = node[defNodeName];
			if (xmlElement == null)
			{
				return default(TDef);
			}
			return DefDatabase<TDef>.GetNamedSilentFail(BackCompatibility.BackCompatibleDefName(typeof(TDef), xmlElement.InnerText, false, null));
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x000F1F90 File Offset: 0x000F0190
		public static LocalTargetInfo LocalTargetInfoFromNode(XmlNode node, string label, LocalTargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						return new LocalTargetInfo(IntVec3.FromString(innerText));
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					return LocalTargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			return defaultValue;
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x000F204C File Offset: 0x000F024C
		public static TargetInfo TargetInfoFromNode(XmlNode node, string label, TargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						string str;
						string targetLoadID;
						ScribeExtractor.ExtractCellAndMapPairFromTargetInfo(innerText, out str, out targetLoadID);
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(Map), "map");
						return new TargetInfo(IntVec3.FromString(str), null, true);
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
					return TargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), label + "/map");
			return defaultValue;
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x000F2164 File Offset: 0x000F0364
		public static GlobalTargetInfo GlobalTargetInfoFromNode(XmlNode node, string label, GlobalTargetInfo defaultValue)
		{
			LoadIDsWantedBank loadIDs = Scribe.loader.crossRefs.loadIDs;
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					string innerText = node.InnerText;
					if (innerText.Length != 0 && innerText[0] == '(')
					{
						string str;
						string targetLoadID;
						ScribeExtractor.ExtractCellAndMapPairFromTargetInfo(innerText, out str, out targetLoadID);
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(targetLoadID, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
						return new GlobalTargetInfo(IntVec3.FromString(str), null, true);
					}
					PlanetTile tile;
					if (PlanetTile.TryParse(innerText, out tile))
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
						return new GlobalTargetInfo(tile);
					}
					if (innerText.Length != 0 && innerText[0] == '@')
					{
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), "thing");
						loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
						loadIDs.RegisterLoadIDReadFromXml(innerText.Substring(1), typeof(WorldObject), "worldObject");
						return GlobalTargetInfo.Invalid;
					}
					loadIDs.RegisterLoadIDReadFromXml(innerText, typeof(Thing), "thing");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), "map");
					loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), "worldObject");
					return GlobalTargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Thing), label + "/thing");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(Map), label + "/map");
			loadIDs.RegisterLoadIDReadFromXml(null, typeof(WorldObject), label + "/worldObject");
			return defaultValue;
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x000F2394 File Offset: 0x000F0594
		public static LocalTargetInfo ResolveLocalTargetInfo(LocalTargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					IntVec3 cell = loaded.Cell;
					if (thing != null)
					{
						return new LocalTargetInfo(thing);
					}
					return new LocalTargetInfo(cell);
				}
				finally
				{
					Scribe.ExitNode();
				}
				return loaded;
			}
			return loaded;
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x000F23F4 File Offset: 0x000F05F4
		public static TargetInfo ResolveTargetInfo(TargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					Map map = Scribe.loader.crossRefs.TakeResolvedRef<Map>("map");
					IntVec3 cell = loaded.Cell;
					if (thing != null)
					{
						return new TargetInfo(thing);
					}
					if (cell.IsValid && map != null)
					{
						return new TargetInfo(cell, map, false);
					}
					return TargetInfo.Invalid;
				}
				finally
				{
					Scribe.ExitNode();
				}
				return loaded;
			}
			return loaded;
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x000F2480 File Offset: 0x000F0680
		public static GlobalTargetInfo ResolveGlobalTargetInfo(GlobalTargetInfo loaded, string label)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					Thing thing = Scribe.loader.crossRefs.TakeResolvedRef<Thing>("thing");
					Map map = Scribe.loader.crossRefs.TakeResolvedRef<Map>("map");
					WorldObject worldObject = Scribe.loader.crossRefs.TakeResolvedRef<WorldObject>("worldObject");
					IntVec3 cell = loaded.Cell;
					PlanetTile tile = loaded.Tile;
					if (thing != null)
					{
						return new GlobalTargetInfo(thing);
					}
					if (worldObject != null)
					{
						return new GlobalTargetInfo(worldObject);
					}
					if (cell.IsValid)
					{
						if (map != null)
						{
							return new GlobalTargetInfo(cell, map, false);
						}
						return GlobalTargetInfo.Invalid;
					}
					else
					{
						if (tile.Valid)
						{
							return new GlobalTargetInfo(tile);
						}
						return GlobalTargetInfo.Invalid;
					}
				}
				finally
				{
					Scribe.ExitNode();
				}
				return loaded;
			}
			return loaded;
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x000F255C File Offset: 0x000F075C
		public static BodyPartRecord BodyPartFromNode(XmlNode node, string label, BodyPartRecord defaultValue)
		{
			if (node != null && Scribe.EnterNode(label))
			{
				try
				{
					XmlAttribute xmlAttribute = node.Attributes["IsNull"];
					if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
					{
						return null;
					}
					BodyDef bodyDef = ScribeExtractor.DefFromNode<BodyDef>(Scribe.loader.curXmlParent["body"]);
					if (bodyDef == null)
					{
						return null;
					}
					XmlElement xmlElement = Scribe.loader.curXmlParent["index"];
					int index = (xmlElement != null) ? int.Parse(xmlElement.InnerText) : -1;
					index = BackCompatibility.GetBackCompatibleBodyPartIndex(bodyDef, index);
					return bodyDef.GetPartAtIndex(index);
				}
				finally
				{
					Scribe.ExitNode();
				}
				return defaultValue;
			}
			return defaultValue;
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x000F261C File Offset: 0x000F081C
		private static void ExtractCellAndMapPairFromTargetInfo(string str, out string cell, out string map)
		{
			int num = str.IndexOf(')');
			cell = str.Substring(0, num + 1);
			int num2 = str.IndexOf(',', num + 1);
			map = str.Substring(num2 + 1);
			map = map.TrimStart(' ');
		}

		// Token: 0x04001C07 RID: 7175
		private static readonly Dictionary<Type, Func<XmlNode, Def>> defFromNodeCached = new Dictionary<Type, Func<XmlNode, Def>>();
	}
}
