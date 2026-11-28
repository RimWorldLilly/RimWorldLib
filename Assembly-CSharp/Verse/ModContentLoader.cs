using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.IO;
using UnityEngine;
using UnityStubs;

namespace Verse
{
	// Token: 0x0200033E RID: 830
	public static class ModContentLoader<T> where T : class
	{
		// Token: 0x06001953 RID: 6483 RVA: 0x000980E0 File Offset: 0x000962E0
		public static bool IsAcceptableExtension(string extension)
		{
			string[] array;
			if (typeof(T) == typeof(AudioClip))
			{
				array = ModContentLoader<T>.AcceptableExtensionsAudio;
			}
			else if (typeof(T) == typeof(Texture2D))
			{
				array = ModContentLoader<T>.AcceptableExtensionsTexture;
			}
			else
			{
				if (!(typeof(T) == typeof(string)))
				{
					string str = "Unknown content type ";
					Type typeFromHandle = typeof(T);
					Log.Error(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
					return false;
				}
				array = ModContentLoader<T>.AcceptableExtensionsString;
			}
			foreach (string b in array)
			{
				if (extension.ToLower() == b)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x000981A2 File Offset: 0x000963A2
		public static IEnumerable<Pair<string, LoadedContentItem<T>>> LoadAllForMod(ModContentPack mod)
		{
			ModContentLoader<T>.<LoadAllForMod>d__4 <LoadAllForMod>d__ = new ModContentLoader<T>.<LoadAllForMod>d__4(-2);
			<LoadAllForMod>d__.<>3__mod = mod;
			return <LoadAllForMod>d__;
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x000981B4 File Offset: 0x000963B4
		private static bool ShadowedByDds(string filename, HashSet<string> ddsFiles)
		{
			if (typeof(T) == typeof(Texture2D) && filename.Length > 4 && !filename.Substring(filename.Length - 4).Equals(".dds", StringComparison.OrdinalIgnoreCase))
			{
				string text = filename.ToLowerInvariant();
				return ddsFiles.Contains(text.Substring(0, text.Length - 4) + ".dds");
			}
			return false;
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x00098228 File Offset: 0x00096428
		public static List<string> PrefetchPaths(ModContentPack mod)
		{
			Dictionary<string, FileInfo> allFilesForMod = ModContentPack.GetAllFilesForMod(mod, GenFilePaths.ContentPath<T>(), new Func<string, bool>(ModContentLoader<T>.IsAcceptableExtension), null);
			ModContentLoader<T>.prefetchScans[mod] = allFilesForMod;
			HashSet<string> ddsFiles = (from k in allFilesForMod.Keys
			select k.ToLowerInvariant() into k
			where k.EndsWith(".dds")
			select k).ToHashSet<string>();
			List<string> list = new List<string>(allFilesForMod.Count);
			foreach (KeyValuePair<string, FileInfo> keyValuePair in allFilesForMod)
			{
				string text;
				FileInfo fileInfo;
				keyValuePair.Deconstruct(out text, out fileInfo);
				string filename = text;
				FileInfo fileInfo2 = fileInfo;
				if (!ModContentLoader<T>.ShadowedByDds(filename, ddsFiles))
				{
					list.Add(fileInfo2.FullName);
				}
			}
			return list;
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0009831C File Offset: 0x0009651C
		public static LoadedContentItem<T> LoadItem(VirtualFile file)
		{
			try
			{
				if (typeof(T) == typeof(string))
				{
					return new LoadedContentItem<T>(file, (T)((object)file.ReadAllText()));
				}
				if (typeof(T) == typeof(Texture2D))
				{
					return new LoadedContentItem<T>(file, (T)((object)ModContentLoader<T>.LoadTexture(file)));
				}
				if (typeof(T) == typeof(AudioClip))
				{
					T t = (T)((object)AudioClip.LoadFromFile(file.FullPath));
					Object @object = t as Object;
					if (@object != null)
					{
						@object.name = Path.GetFileNameWithoutExtension(file.Name);
					}
					return new LoadedContentItem<T>(file, t);
				}
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Exception loading ");
				defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
				defaultInterpolatedStringHandler.AppendLiteral(" from file.\nabsFilePath: ");
				defaultInterpolatedStringHandler.AppendFormatted(file.FullPath);
				defaultInterpolatedStringHandler.AppendLiteral("\nException: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (typeof(T) == typeof(Texture2D))
			{
				return (LoadedContentItem<T>)new LoadedContentItem<Texture2D>(file, BaseContent.BadTex);
			}
			return null;
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x00098494 File Offset: 0x00096694
		private static Texture2D LoadTexture(VirtualFile file)
		{
			FilesystemFile filesystemFile = file as FilesystemFile;
			if ((filesystemFile != null) ? (!ContentPacks.FileExists(filesystemFile.FullPath)) : (!file.Exists))
			{
				return null;
			}
			if (file.Name.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
			{
				return ModDdsLoader.TryLoadDds(file);
			}
			Texture2D texture2D = ModContentLoader<T>.LoadTextureViaImageConversion(file);
			texture2D.name = Path.GetFileNameWithoutExtension(file.Name);
			return texture2D;
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x000984FC File Offset: 0x000966FC
		private static Texture2D LoadTextureViaImageConversion(VirtualFile file)
		{
			FilesystemFile filesystemFile = file as FilesystemFile;
			Texture2D texture2D;
			if (filesystemFile != null)
			{
				texture2D = Texture2D.LoadFromFile(filesystemFile.FullPath);
			}
			else
			{
				texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, true);
				texture2D.LoadImage(file.ReadAllBytes());
			}
			if ((texture2D.width < 4 || texture2D.height < 4 || !Mathf.IsPowerOfTwo(texture2D.width) || !Mathf.IsPowerOfTwo(texture2D.height)) && Prefs.TextureCompression)
			{
				int num = StaticTextureAtlas.CalculateMaxMipmapsForDxtSupport(texture2D);
				if (Prefs.LogVerbose)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(225, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Texture ");
					defaultInterpolatedStringHandler.AppendFormatted(file.Name);
					defaultInterpolatedStringHandler.AppendLiteral(" is being reloaded with reduced mipmap count (clamped to ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral(") due to non-power-of-two dimensions: (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(texture2D.width);
					defaultInterpolatedStringHandler.AppendLiteral("x");
					defaultInterpolatedStringHandler.AppendFormatted<int>(texture2D.height);
					defaultInterpolatedStringHandler.AppendLiteral("). This will be slower to load, and will look worse when zoomed out. Consider using a power-of-two texture size instead.");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (!UnityData.ComputeShadersSupported && !(file is FilesystemFile))
				{
					Texture2D texture2D2 = new Texture2D(texture2D.width, texture2D.height, TextureFormat.Alpha8, num, false);
					Object.DestroyImmediate(texture2D);
					texture2D = texture2D2;
					texture2D.LoadImage(file.ReadAllBytes());
				}
			}
			bool flag = true;
			if (Prefs.TextureCompression && flag)
			{
				if (!UnityData.ComputeShadersSupported)
				{
					texture2D.Compress(true);
					texture2D.filterMode = FilterMode.Trilinear;
					texture2D.anisoLevel = 2;
					texture2D.Apply(true, true);
				}
				else
				{
					texture2D.filterMode = FilterMode.Trilinear;
					texture2D.anisoLevel = 2;
					texture2D.Apply(true, true);
					texture2D = StaticTextureAtlas.FastCompressDXT(texture2D, true);
				}
			}
			else
			{
				texture2D.filterMode = FilterMode.Trilinear;
				texture2D.anisoLevel = 2;
				texture2D.Apply(true, true);
			}
			return texture2D;
		}

		// Token: 0x040014F6 RID: 5366
		private static string[] AcceptableExtensionsAudio = new string[]
		{
			".wav",
			".mp3",
			".ogg",
			".xm",
			".it",
			".mod",
			".s3m"
		};

		// Token: 0x040014F7 RID: 5367
		private static string[] AcceptableExtensionsTexture = new string[]
		{
			".png",
			".jpg",
			".jpeg",
			".psd",
			".dds"
		};

		// Token: 0x040014F8 RID: 5368
		private static string[] AcceptableExtensionsString = new string[]
		{
			".txt"
		};

		// Token: 0x040014F9 RID: 5369
		private static readonly ConcurrentDictionary<ModContentPack, Dictionary<string, FileInfo>> prefetchScans = new ConcurrentDictionary<ModContentPack, Dictionary<string, FileInfo>>();
	}
}
