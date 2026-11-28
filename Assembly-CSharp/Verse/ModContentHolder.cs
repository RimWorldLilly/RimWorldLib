using System;
using System.Collections.Generic;
using System.IO;
using KTrie;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200033C RID: 828
	public class ModContentHolder<T> where T : class
	{
		// Token: 0x0600194D RID: 6477 RVA: 0x00097E6C File Offset: 0x0009606C
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x00097E94 File Offset: 0x00096094
		public void ClearDestroy()
		{
			if (typeof(Object).IsAssignableFrom(typeof(T)))
			{
				foreach (T localObj2 in this.contentList.Values)
				{
					T localObj = localObj2;
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						Object.Destroy((Object)((object)localObj));
					});
				}
			}
			this.contentList.Clear();
			this.contentListTrie.Clear();
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00097F34 File Offset: 0x00096134
		public void ReloadAll(bool hotReload = false)
		{
			ModContentHolder<T>.anyReloaded = true;
			foreach (Pair<string, LoadedContentItem<T>> pair in ModContentLoader<T>.LoadAllForMod(this.mod))
			{
				string text = pair.First;
				text = text.Replace('\\', '/');
				string text2 = GenFilePaths.ContentPath<T>();
				if (text.StartsWith(text2))
				{
					text = text.Substring(text2.Length);
				}
				if (text.EndsWith(Path.GetExtension(text)))
				{
					text = text.Substring(0, text.Length - Path.GetExtension(text).Length);
				}
				if (this.contentList.ContainsKey(text))
				{
					if (!hotReload)
					{
						string[] array = new string[6];
						array[0] = "Tried to load duplicate ";
						int num = 1;
						Type typeFromHandle = typeof(T);
						array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
						array[2] = " with path: ";
						int num2 = 3;
						VirtualFile internalFile = pair.Second.internalFile;
						array[num2] = ((internalFile != null) ? internalFile.ToString() : null);
						array[4] = " and internal path: ";
						array[5] = text;
						Log.Warning(string.Concat(array));
					}
				}
				else
				{
					this.contentList.Add(text, pair.Second.contentItem);
					this.contentListTrie.Add(text);
				}
			}
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00098088 File Offset: 0x00096288
		public T Get(string path)
		{
			T result;
			if (this.contentList.TryGetValue(path, out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x000980B0 File Offset: 0x000962B0
		public IEnumerable<T> GetAllUnderPath(string pathRoot)
		{
			ModContentHolder<T>.<GetAllUnderPath>d__8 <GetAllUnderPath>d__ = new ModContentHolder<T>.<GetAllUnderPath>d__8(-2);
			<GetAllUnderPath>d__.<>4__this = this;
			<GetAllUnderPath>d__.<>3__pathRoot = pathRoot;
			return <GetAllUnderPath>d__;
		}

		// Token: 0x040014F0 RID: 5360
		private ModContentPack mod;

		// Token: 0x040014F1 RID: 5361
		public Dictionary<string, T> contentList = new Dictionary<string, T>();

		// Token: 0x040014F2 RID: 5362
		private StringTrieSet contentListTrie = new StringTrieSet();

		// Token: 0x040014F3 RID: 5363
		internal static bool anyReloaded;
	}
}
