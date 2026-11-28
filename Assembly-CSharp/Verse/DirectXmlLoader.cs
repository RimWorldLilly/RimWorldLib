using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200050B RID: 1291
	public static class DirectXmlLoader
	{
		// Token: 0x0600263C RID: 9788 RVA: 0x000E8ED8 File Offset: 0x000E70D8
		public static LoadableXmlAsset[] XmlAssetsInModFolder(ModContentPack mod, string folderPath, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			Dictionary<string, FileInfo> dictionary = new Dictionary<string, FileInfo>();
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, folderPath));
				if (directoryInfo.Exists)
				{
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
					{
						if (!fileInfo.Name.StartsWith("._") && !fileInfo.Name.StartsWith("."))
						{
							string key = fileInfo.FullName.Substring(text.Length + 1);
							dictionary.TryAdd(key, fileInfo);
						}
					}
				}
			}
			if (dictionary.Count == 0)
			{
				return DirectXmlLoader.EmptyXmlAssetsArray;
			}
			List<FileInfo> list2 = dictionary.Values.ToList<FileInfo>();
			LoadableXmlAsset[] assets = new LoadableXmlAsset[list2.Count];
			ConcurrentBag<KeyValuePair<int, FileInfo>> toLoad = new ConcurrentBag<KeyValuePair<int, FileInfo>>();
			for (int k = 0; k < list2.Count; k++)
			{
				int key2 = k;
				FileInfo value = list2[k];
				toLoad.Add(new KeyValuePair<int, FileInfo>(key2, value));
			}
			Thread[] array = new Thread[2];
			ThreadStart <>9__0;
			for (int l = 0; l < array.Length; l++)
			{
				Thread[] array2 = array;
				int num = l;
				ThreadStart start;
				if ((start = <>9__0) == null)
				{
					start = (<>9__0 = delegate()
					{
						KeyValuePair<int, FileInfo> keyValuePair2;
						while (toLoad.TryTake(out keyValuePair2))
						{
							int key4 = keyValuePair2.Key;
							FileInfo value3 = keyValuePair2.Value;
							assets[key4] = new LoadableXmlAsset(value3, mod);
						}
					});
				}
				Thread thread = new Thread(start);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
				defaultInterpolatedStringHandler.AppendLiteral("DirectXmlLoader Thread ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(l + 1);
				defaultInterpolatedStringHandler.AppendLiteral(" of ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(2);
				thread.Name = defaultInterpolatedStringHandler.ToStringAndClear();
				array2[num] = thread;
				array[l].Start();
			}
			KeyValuePair<int, FileInfo> keyValuePair;
			while (toLoad.TryTake(out keyValuePair))
			{
				int key3 = keyValuePair.Key;
				FileInfo value2 = keyValuePair.Value;
				assets[key3] = new LoadableXmlAsset(value2, mod);
			}
			Thread[] array3 = array;
			for (int m = 0; m < array3.Length; m++)
			{
				array3[m].Join();
			}
			return assets;
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000E9119 File Offset: 0x000E7319
		public static IEnumerable<T> LoadXmlDataInResourcesFolder<T>(string folderPath) where T : new()
		{
			DirectXmlLoader.<LoadXmlDataInResourcesFolder>d__2<T> <LoadXmlDataInResourcesFolder>d__ = new DirectXmlLoader.<LoadXmlDataInResourcesFolder>d__2<T>(-2);
			<LoadXmlDataInResourcesFolder>d__.<>3__folderPath = folderPath;
			return <LoadXmlDataInResourcesFolder>d__;
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000E9129 File Offset: 0x000E7329
		public static T ItemFromXmlFile<T>(string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (!new FileInfo(filePath).Exists)
			{
				return Activator.CreateInstance<T>();
			}
			return DirectXmlLoader.ItemFromXmlString<T>(File.ReadAllText(filePath), filePath, resolveCrossRefs);
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000E914B File Offset: 0x000E734B
		public static T ItemFromXmlFile<T>(VirtualDirectory directory, string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (!directory.FileExists(filePath))
			{
				return Activator.CreateInstance<T>();
			}
			return DirectXmlLoader.ItemFromXmlString<T>(directory.ReadAllText(filePath), directory.FullPath + "/" + filePath, resolveCrossRefs);
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000E917C File Offset: 0x000E737C
		public static T ItemFromXmlString<T>(string xmlContent, string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (resolveCrossRefs && DirectXmlCrossRefLoader.LoadingInProgress)
			{
				Log.Error("Cannot call ItemFromXmlString with resolveCrossRefs=true while loading is already in progress (forgot to resolve or clear cross refs from previous loading?).");
			}
			T result;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xmlContent);
				T t = DirectXmlToObject.ObjectFromXml<T>(xmlDocument.DocumentElement, false);
				if (resolveCrossRefs)
				{
					try
					{
						DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
					}
					finally
					{
						DirectXmlCrossRefLoader.Clear();
					}
				}
				result = t;
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading file at " + filePath + ". Loading defaults instead. Exception was: " + ex.ToString());
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000E920C File Offset: 0x000E740C
		public static Def DefFromNode(XmlNode node, LoadableXmlAsset loadingAsset)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				return null;
			}
			XmlAttribute xmlAttribute = node.Attributes["Abstract"];
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}
			XmlNode resolvedNodeFor = XmlInheritance.GetResolvedNodeFor(node);
			string text = node.Name;
			XmlAttribute xmlAttribute2 = resolvedNodeFor.Attributes["Class"];
			if (xmlAttribute2 != null)
			{
				text = xmlAttribute2.Value;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(node.Name, null);
			if (typeInAnyAssembly == null || !GenTypes.IsDef(typeInAnyAssembly))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Type ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(" is not a Def type or could not be found, in file ");
				defaultInterpolatedStringHandler.AppendFormatted((loadingAsset != null) ? loadingAsset.name : "(unknown)");
				defaultInterpolatedStringHandler.AppendLiteral(". Context: ");
				defaultInterpolatedStringHandler.AppendFormatted(node.OuterXml);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), text.GetHashCode());
				return null;
			}
			Func<XmlNode, bool, object> objectFromXmlMethod = DirectXmlToObject.GetObjectFromXmlMethod(typeInAnyAssembly);
			Def def = null;
			try
			{
				def = (Def)objectFromXmlMethod(node, true);
				def.ResolveDefNameHash();
			}
			catch (Exception ex)
			{
				string str = "Exception loading def from file ";
				string str2 = (loadingAsset != null) ? loadingAsset.name : "(unknown)";
				string str3 = ": ";
				Exception ex2 = ex;
				Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
			}
			return def;
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000E936C File Offset: 0x000E756C
		public static IEnumerable<T> AllGameItemsFromAsset<T>(LoadableXmlAsset asset) where T : new()
		{
			DirectXmlLoader.<AllGameItemsFromAsset>d__7<T> <AllGameItemsFromAsset>d__ = new DirectXmlLoader.<AllGameItemsFromAsset>d__7<T>(-2);
			<AllGameItemsFromAsset>d__.<>3__asset = asset;
			return <AllGameItemsFromAsset>d__;
		}

		// Token: 0x04001B83 RID: 7043
		private static readonly LoadableXmlAsset[] EmptyXmlAssetsArray = Array.Empty<LoadableXmlAsset>();
	}
}
