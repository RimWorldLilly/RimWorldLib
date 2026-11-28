using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000331 RID: 817
	public static class ContentFinder<T> where T : class
	{
		// Token: 0x06001911 RID: 6417 RVA: 0x00095B58 File Offset: 0x00093D58
		public static T Get(string itemPath, bool reportFailure = true)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a resource \"" + itemPath + "\" from a different thread. All resources must be loaded in the main thread.");
				return default(T);
			}
			T t = default(T);
			if (typeof(T) != typeof(Shader) && ModContentHolder<T>.anyReloaded)
			{
				List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
				for (int i = runningModsListForReading.Count - 1; i >= 0; i--)
				{
					ModContentHolder<T> contentHolder = runningModsListForReading[i].GetContentHolder<T>();
					if (contentHolder.contentList.Count != 0)
					{
						t = contentHolder.Get(itemPath);
						if (t != null)
						{
							return t;
						}
					}
				}
			}
			if (typeof(T) == typeof(Texture2D))
			{
				t = (T)((object)Resources.Load<Texture2D>(GenFilePaths.ContentPath<Texture2D>() + itemPath));
			}
			if (typeof(T) == typeof(AudioClip))
			{
				t = (T)((object)Resources.Load<AudioClip>(GenFilePaths.ContentPath<AudioClip>() + itemPath));
			}
			if (t != null)
			{
				return t;
			}
			if (typeof(T) == typeof(Texture2D) || typeof(T) == typeof(AudioClip) || typeof(T) == typeof(Shader))
			{
				T t2 = ContentFinder<T>.TryFindAssetInModBundles(itemPath);
				if (t2 != null)
				{
					return t2;
				}
			}
			if (reportFailure)
			{
				string value = (ContentFinderRequester.requester != null) ? (" for def '" + ContentFinderRequester.requester.defName + "'") : "";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Could not load ");
				defaultInterpolatedStringHandler.AppendFormatted(typeof(T).Name);
				defaultInterpolatedStringHandler.AppendLiteral(" at '");
				defaultInterpolatedStringHandler.AppendFormatted(itemPath);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				defaultInterpolatedStringHandler.AppendFormatted(value);
				defaultInterpolatedStringHandler.AppendLiteral(" in any active mod or in base resources.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return default(T);
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x00095D78 File Offset: 0x00093F78
		public static T TryFindAssetInModBundles(string itemPath)
		{
			if (!UnityData.IsInMainThread)
			{
				return ContentFinder<T>.TryFindAssetInModBundlesUncached(itemPath);
			}
			int num = ContentFinder<T>.LoadedAssetBundlesStamp();
			if (num != ContentFinder<T>.bundleAssetCacheStamp)
			{
				ContentFinder<T>.bundleAssetCache.Clear();
				ContentFinder<T>.bundleAssetCacheStamp = num;
			}
			T t;
			if (ContentFinder<T>.bundleAssetCache.TryGetValue(itemPath, out t))
			{
				if (t != null)
				{
					Object @object = t as Object;
					if (@object == null || !(@object != null))
					{
						goto IL_5D;
					}
				}
				return t;
			}
			IL_5D:
			T t2 = ContentFinder<T>.TryFindAssetInModBundlesUncached(itemPath);
			ContentFinder<T>.bundleAssetCache[itemPath] = t2;
			return t2;
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x00095DF8 File Offset: 0x00093FF8
		private static int LoadedAssetBundlesStamp()
		{
			List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
			int num = runningModsListForReading.Count;
			for (int i = 0; i < runningModsListForReading.Count; i++)
			{
				num = num * 31 + runningModsListForReading[i].assetBundles.loadedAssetBundles.Count;
			}
			return num;
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x00095E40 File Offset: 0x00094040
		private static T TryFindAssetInModBundlesUncached(string itemPath)
		{
			string[] array;
			if (typeof(T) == typeof(Texture2D))
			{
				array = ModAssetBundlesHandler.TextureExtensions;
			}
			else if (typeof(T) == typeof(AudioClip))
			{
				array = ModAssetBundlesHandler.AudioClipExtensions;
			}
			else
			{
				if (!(typeof(T) == typeof(Shader)))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
					defaultInterpolatedStringHandler.AppendLiteral(" for asset bundle loading. Did you forget to add a new type?");
					throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				array = ModAssetBundlesHandler.ShaderExtensions;
			}
			List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
			string path = Path.Combine("Assets", "Data");
			for (int i = runningModsListForReading.Count - 1; i >= 0; i--)
			{
				string path2 = Path.Combine(path, runningModsListForReading[i].FolderName);
				string path3 = Path.Combine(path, runningModsListForReading[i].PackageIdPlayerFacing);
				bool flag = !runningModsListForReading[i].IsOfficialMod;
				foreach (AssetBundle assetBundle in runningModsListForReading[i].assetBundles.loadedAssetBundles)
				{
					string str = Path.Combine(Path.Combine(path2, GenFilePaths.ContentPath<T>()), itemPath);
					string str2 = Path.Combine(Path.Combine(path3, GenFilePaths.ContentPath<T>()), itemPath);
					foreach (string str3 in array)
					{
						T t = assetBundle.LoadAsset(str + str3, typeof(T)) as T;
						if (t != null)
						{
							return t;
						}
						if (flag)
						{
							T t2 = assetBundle.LoadAsset(str2 + str3, typeof(T)) as T;
							if (t2 != null)
							{
								return t2;
							}
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x00096080 File Offset: 0x00094280
		public static IEnumerable<T> GetAllInFolder(string folderPath)
		{
			ContentFinder<T>.<GetAllInFolder>d__6 <GetAllInFolder>d__ = new ContentFinder<T>.<GetAllInFolder>d__6(-2);
			<GetAllInFolder>d__.<>3__folderPath = folderPath;
			return <GetAllInFolder>d__;
		}

		// Token: 0x040014B4 RID: 5300
		private static readonly Dictionary<string, T> bundleAssetCache = new Dictionary<string, T>();

		// Token: 0x040014B5 RID: 5301
		private static int bundleAssetCacheStamp = -1;
	}
}
