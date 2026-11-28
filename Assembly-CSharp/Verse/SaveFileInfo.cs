using System;
using System.IO;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200051B RID: 1307
	public class SaveFileInfo
	{
		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060026BE RID: 9918 RVA: 0x000F05D8 File Offset: 0x000EE7D8
		private bool Valid
		{
			get
			{
				object obj = this.lockObject;
				bool result;
				lock (obj)
				{
					result = (this.gameVersion != null);
				}
				return result;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060026BF RID: 9919 RVA: 0x000F0620 File Offset: 0x000EE820
		public FileInfo FileInfo
		{
			get
			{
				return this.fileInfo;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x060026C0 RID: 9920 RVA: 0x000F0628 File Offset: 0x000EE828
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x060026C1 RID: 9921 RVA: 0x000F0630 File Offset: 0x000EE830
		public DateTime LastWriteTime
		{
			get
			{
				return this.lastWriteTime;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x000F0638 File Offset: 0x000EE838
		public string GameVersion
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return "LoadingVersionInfo".Translate();
						}
						if (!this.Valid)
						{
							return "???";
						}
						return this.gameVersion;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return "LoadingVersionInfo".Translate();
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x060026C3 RID: 9923 RVA: 0x000F06B8 File Offset: 0x000EE8B8
		public Color VersionColor
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return Color.gray;
						}
						if (!this.Valid)
						{
							return ColorLibrary.RedReadable;
						}
						if (VersionControl.MajorFromVersionString(this.gameVersion) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(this.gameVersion) == VersionControl.CurrentMinor)
						{
							return SaveFileInfo.UnimportantTextColor;
						}
						if (BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
						{
							return Color.yellow;
						}
						return ColorLibrary.RedReadable;
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return Color.gray;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x060026C4 RID: 9924 RVA: 0x000F0764 File Offset: 0x000EE964
		public TipSignal CompatibilityTip
		{
			get
			{
				bool flag = false;
				try
				{
					if (flag = this.TryLock(0))
					{
						if (!this.loaded)
						{
							return "LoadingVersionInfo".Translate();
						}
						if (!this.Valid)
						{
							return "SaveIsUnknownFormat".Translate();
						}
						if ((VersionControl.MajorFromVersionString(this.gameVersion) != VersionControl.CurrentMajor || VersionControl.MinorFromVersionString(this.gameVersion) != VersionControl.CurrentMinor) && !BackCompatibility.IsSaveCompatibleWith(this.gameVersion))
						{
							return "SaveIsFromDifferentGameVersion".Translate(VersionControl.CurrentVersionString, this.gameVersion);
						}
						if (VersionControl.BuildFromVersionString(this.gameVersion) != VersionControl.CurrentBuild)
						{
							return "SaveIsFromDifferentGameBuild".Translate(VersionControl.CurrentVersionString, this.gameVersion);
						}
						return "SaveIsFromThisGameBuild".Translate();
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.lockObject);
					}
				}
				return "LoadingVersionInfo".Translate();
			}
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x000F0890 File Offset: 0x000EEA90
		private bool TryLock(int timeoutMilliseconds)
		{
			return Monitor.TryEnter(this.lockObject, timeoutMilliseconds);
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x000F089E File Offset: 0x000EEA9E
		public SaveFileInfo(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
			this.fileName = fileInfo.Name;
			this.lastWriteTime = fileInfo.LastWriteTime;
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x000F08D0 File Offset: 0x000EEAD0
		public void LoadData()
		{
			object obj = this.lockObject;
			lock (obj)
			{
				this.gameVersion = ScribeMetaHeaderUtility.GameVersionOf(this.fileInfo);
				this.loaded = true;
			}
		}

		// Token: 0x04001BED RID: 7149
		private FileInfo fileInfo;

		// Token: 0x04001BEE RID: 7150
		private string gameVersion;

		// Token: 0x04001BEF RID: 7151
		private DateTime lastWriteTime;

		// Token: 0x04001BF0 RID: 7152
		private string fileName;

		// Token: 0x04001BF1 RID: 7153
		private bool loaded;

		// Token: 0x04001BF2 RID: 7154
		private object lockObject = new object();

		// Token: 0x04001BF3 RID: 7155
		public static readonly Color UnimportantTextColor = new Color(1f, 1f, 1f, 0.5f);
	}
}
