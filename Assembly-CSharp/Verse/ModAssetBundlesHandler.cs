using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200033B RID: 827
	public class ModAssetBundlesHandler
	{
		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001945 RID: 6469 RVA: 0x00097B78 File Offset: 0x00095D78
		private string BundleSuffixForCurrentOs
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				if (platform <= RuntimePlatform.WindowsPlayer)
				{
					if (platform <= RuntimePlatform.OSXPlayer)
					{
						return "_mac";
					}
					if (platform != RuntimePlatform.WindowsPlayer)
					{
						goto IL_3A;
					}
				}
				else if (platform != RuntimePlatform.WindowsEditor)
				{
					if (platform == RuntimePlatform.LinuxPlayer || platform == RuntimePlatform.LinuxEditor)
					{
						return "_linux";
					}
					goto IL_3A;
				}
				return "_win";
				IL_3A:
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported platform for asset bundle loading: ");
				defaultInterpolatedStringHandler.AppendFormatted<RuntimePlatform>(Application.platform);
				throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x00097BEF File Offset: 0x00095DEF
		public ModAssetBundlesHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x00097C0C File Offset: 0x00095E0C
		public void ReloadAll(bool hotReload = false)
		{
			foreach (KeyValuePair<string, FileInfo> keyValuePair in ModContentPack.GetAllFilesForMod(this.mod, GenFilePaths.ContentPath<AssetBundle>(), new Func<string, bool>(this.IsAcceptableExtension), null))
			{
				string text;
				FileInfo fileInfo;
				keyValuePair.Deconstruct(out text, out fileInfo);
				FileInfo fileInfo2 = fileInfo;
				string bundleNameWithoutOsSpecifier = this.GetBundleNameWithoutOsSpecifier(fileInfo2);
				bool flag = bundleNameWithoutOsSpecifier == null || fileInfo2.Name.Replace(bundleNameWithoutOsSpecifier, "") == this.BundleSuffixForCurrentOs;
				if (flag)
				{
					AssetBundle assetBundle = AssetBundle.LoadFromFile(fileInfo2.FullName);
					if (assetBundle != null)
					{
						if (!this.loadedAssetBundles.Contains(assetBundle))
						{
							this.loadedAssetBundles.Add(assetBundle);
						}
					}
					else
					{
						Log.Error("Could not load asset bundle at " + fileInfo2.FullName);
					}
				}
			}
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x00097D04 File Offset: 0x00095F04
		private bool IsAcceptableExtension(string extension)
		{
			return extension.NullOrEmpty();
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00097D14 File Offset: 0x00095F14
		private string GetBundleNameWithoutOsSpecifier(FileInfo file)
		{
			string name = file.Name;
			if (name.EndsWith("_linux"))
			{
				string text = name;
				int length = "_linux".Length;
				return text.Substring(0, text.Length - length);
			}
			if (name.EndsWith("_mac"))
			{
				string text = name;
				int length = "_mac".Length;
				return text.Substring(0, text.Length - length);
			}
			if (name.EndsWith("_win"))
			{
				string text = name;
				int length = "_win".Length;
				return text.Substring(0, text.Length - length);
			}
			return null;
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x00097DA7 File Offset: 0x00095FA7
		public void ClearDestroy()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				for (int i = 0; i < this.loadedAssetBundles.Count; i++)
				{
					this.loadedAssetBundles[i].Unload(true);
				}
				this.loadedAssetBundles.Clear();
			});
		}

		// Token: 0x040014E8 RID: 5352
		private ModContentPack mod;

		// Token: 0x040014E9 RID: 5353
		public List<AssetBundle> loadedAssetBundles = new List<AssetBundle>();

		// Token: 0x040014EA RID: 5354
		public static readonly string[] TextureExtensions = new string[]
		{
			".png",
			".psd",
			".jpg",
			".jpeg"
		};

		// Token: 0x040014EB RID: 5355
		public static readonly string[] AudioClipExtensions = new string[]
		{
			".wav",
			".mp3",
			".ogg"
		};

		// Token: 0x040014EC RID: 5356
		public static readonly string[] ShaderExtensions = new string[]
		{
			".shader"
		};

		// Token: 0x040014ED RID: 5357
		public const string LinuxBundleSuffix = "_linux";

		// Token: 0x040014EE RID: 5358
		public const string MacBundleSuffix = "_mac";

		// Token: 0x040014EF RID: 5359
		public const string WindowsBundleSuffix = "_win";
	}
}
