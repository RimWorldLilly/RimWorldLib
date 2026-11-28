using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Tar;

namespace RimWorld.IO
{
	// Token: 0x02002403 RID: 9219
	internal class TarDirectory : VirtualDirectory
	{
		// Token: 0x0600E6AC RID: 59052 RVA: 0x0055C7CF File Offset: 0x0055A9CF
		public static void ClearCache()
		{
			TarDirectory.cache.Clear();
		}

		// Token: 0x0600E6AD RID: 59053 RVA: 0x0055C7DC File Offset: 0x0055A9DC
		public static TarDirectory ReadFromFileOrCache(string file)
		{
			string key = file.Replace('\\', '/');
			TarDirectory tarDirectory;
			if (!TarDirectory.cache.TryGetValue(key, out tarDirectory))
			{
				tarDirectory = new TarDirectory(file, "");
				tarDirectory.lazyLoadArchive = file;
				TarDirectory.cache.Add(key, tarDirectory);
			}
			return tarDirectory;
		}

		// Token: 0x0600E6AE RID: 59054 RVA: 0x0055C824 File Offset: 0x0055AA24
		private void CheckLazyLoad()
		{
			if (this.lazyLoadArchive != null)
			{
				using (FileStream fileStream = File.OpenRead(this.lazyLoadArchive))
				{
					using (TarInputStream tarInputStream = new TarInputStream(fileStream))
					{
						TarDirectory.ParseTAR(this, tarInputStream, this.lazyLoadArchive);
					}
				}
				this.lazyLoadArchive = null;
			}
		}

		// Token: 0x0600E6AF RID: 59055 RVA: 0x0055C894 File Offset: 0x0055AA94
		private static void ParseTAR(TarDirectory root, TarInputStream input, string fullPath)
		{
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			List<TarEntry> list = new List<TarEntry>();
			List<TarDirectory> list2 = new List<TarDirectory>();
			byte[] buffer = new byte[16384];
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					TarEntry nextEntry;
					while ((nextEntry = input.GetNextEntry()) != null)
					{
						TarDirectory.ReadTarEntryData(input, memoryStream, buffer);
						dictionary.Add(nextEntry.Name, memoryStream.ToArray());
						list.Add(nextEntry);
						memoryStream.Position = 0L;
						memoryStream.SetLength(0L);
					}
				}
				list2.Add(root);
				foreach (TarEntry tarEntry in from e in list
				where e.IsDirectory && !string.IsNullOrEmpty(e.Name)
				select e)
				{
					string str = TarDirectory.FormatFolderPath(tarEntry.Name);
					list2.Add(new TarDirectory(fullPath + "/" + str, str));
				}
				foreach (TarEntry tarEntry2 in from e in list
				where !e.IsDirectory
				select e)
				{
					string b = TarDirectory.FormatFolderPath(Path.GetDirectoryName(tarEntry2.Name));
					TarDirectory tarDirectory = null;
					foreach (TarDirectory tarDirectory2 in list2)
					{
						if (tarDirectory2.inArchiveFullPath == b)
						{
							tarDirectory = tarDirectory2;
							break;
						}
					}
					tarDirectory.files.Add(new TarFile(dictionary[tarEntry2.Name], fullPath + "/" + tarEntry2.Name, Path.GetFileName(tarEntry2.Name)));
				}
				foreach (TarDirectory tarDirectory3 in list2)
				{
					if (!string.IsNullOrEmpty(tarDirectory3.inArchiveFullPath))
					{
						string b2 = TarDirectory.FormatFolderPath(Path.GetDirectoryName(tarDirectory3.inArchiveFullPath));
						TarDirectory tarDirectory4 = null;
						foreach (TarDirectory tarDirectory5 in list2)
						{
							if (tarDirectory5.inArchiveFullPath == b2)
							{
								tarDirectory4 = tarDirectory5;
								break;
							}
						}
						tarDirectory4.subDirectories.Add(tarDirectory3);
					}
				}
			}
			finally
			{
				input.Close();
			}
		}

		// Token: 0x0600E6B0 RID: 59056 RVA: 0x0055CBD0 File Offset: 0x0055ADD0
		private static string FormatFolderPath(string str)
		{
			if (str.Length == 0)
			{
				return str;
			}
			if (str.IndexOf('\\') != -1)
			{
				str = str.Replace('\\', '/');
			}
			if (str[str.Length - 1] == '/')
			{
				str = str.Substring(0, str.Length - 1);
			}
			return str;
		}

		// Token: 0x0600E6B1 RID: 59057 RVA: 0x0055CC24 File Offset: 0x0055AE24
		private static void ReadTarEntryData(TarInputStream tarIn, Stream outStream, byte[] buffer = null)
		{
			if (buffer == null)
			{
				buffer = new byte[4096];
			}
			for (int i = tarIn.Read(buffer, 0, buffer.Length); i > 0; i = tarIn.Read(buffer, 0, buffer.Length))
			{
				outStream.Write(buffer, 0, i);
			}
		}

		// Token: 0x0600E6B2 RID: 59058 RVA: 0x0055CC67 File Offset: 0x0055AE67
		private static IEnumerable<TarDirectory> EnumerateAllChildrenRecursive(TarDirectory of)
		{
			TarDirectory.<EnumerateAllChildrenRecursive>d__8 <EnumerateAllChildrenRecursive>d__ = new TarDirectory.<EnumerateAllChildrenRecursive>d__8(-2);
			<EnumerateAllChildrenRecursive>d__.<>3__of = of;
			return <EnumerateAllChildrenRecursive>d__;
		}

		// Token: 0x0600E6B3 RID: 59059 RVA: 0x0055CC77 File Offset: 0x0055AE77
		private static IEnumerable<TarFile> EnumerateAllFilesRecursive(TarDirectory of)
		{
			TarDirectory.<EnumerateAllFilesRecursive>d__9 <EnumerateAllFilesRecursive>d__ = new TarDirectory.<EnumerateAllFilesRecursive>d__9(-2);
			<EnumerateAllFilesRecursive>d__.<>3__of = of;
			return <EnumerateAllFilesRecursive>d__;
		}

		// Token: 0x0600E6B4 RID: 59060 RVA: 0x0055CC88 File Offset: 0x0055AE88
		private static Func<string, bool> GetPatternMatcher(string searchPattern)
		{
			Func<string, bool> func = null;
			if (searchPattern.Length == 1 && searchPattern[0] == '*')
			{
				func = ((string str) => true);
			}
			else if (searchPattern.Length > 2 && searchPattern[0] == '*' && searchPattern[1] == '.')
			{
				string extension = searchPattern.Substring(2);
				func = ((string str) => str.Substring(str.Length - extension.Length) == extension);
			}
			if (func == null)
			{
				func = ((string str) => false);
			}
			return func;
		}

		// Token: 0x170028CB RID: 10443
		// (get) Token: 0x0600E6B5 RID: 59061 RVA: 0x0055CD2E File Offset: 0x0055AF2E
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170028CC RID: 10444
		// (get) Token: 0x0600E6B6 RID: 59062 RVA: 0x0055CD36 File Offset: 0x0055AF36
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x170028CD RID: 10445
		// (get) Token: 0x0600E6B7 RID: 59063 RVA: 0x0055CD3E File Offset: 0x0055AF3E
		public override bool Exists
		{
			get
			{
				return this.exists;
			}
		}

		// Token: 0x0600E6B8 RID: 59064 RVA: 0x0055CD48 File Offset: 0x0055AF48
		private TarDirectory(string fullPath, string inArchiveFullPath)
		{
			this.name = Path.GetFileName(fullPath);
			if (this.name.IndexOf(".tar") == this.name.Length - 4)
			{
				this.name = this.name.Substring(0, this.name.Length - 4);
			}
			this.fullPath = fullPath;
			this.inArchiveFullPath = inArchiveFullPath;
			this.exists = true;
		}

		// Token: 0x0600E6B9 RID: 59065 RVA: 0x0055CDD0 File Offset: 0x0055AFD0
		private TarDirectory()
		{
			this.exists = false;
		}

		// Token: 0x0600E6BA RID: 59066 RVA: 0x0055CDF8 File Offset: 0x0055AFF8
		public override VirtualDirectory GetDirectory(string directoryName)
		{
			this.CheckLazyLoad();
			string text = directoryName;
			if (!string.IsNullOrEmpty(this.fullPath))
			{
				text = this.fullPath + "/" + text;
			}
			foreach (TarDirectory tarDirectory in this.subDirectories)
			{
				if (tarDirectory.fullPath == text)
				{
					return tarDirectory;
				}
			}
			return TarDirectory.NotFound;
		}

		// Token: 0x0600E6BB RID: 59067 RVA: 0x0055CE84 File Offset: 0x0055B084
		public override VirtualFile GetFile(string filename)
		{
			this.CheckLazyLoad();
			VirtualDirectory virtualDirectory = this;
			string[] array = filename.Split(new char[]
			{
				'/',
				'\\'
			});
			for (int i = 0; i < array.Length - 1; i++)
			{
				virtualDirectory = virtualDirectory.GetDirectory(array[i]);
			}
			filename = array[array.Length - 1];
			if (virtualDirectory == this)
			{
				using (List<TarFile>.Enumerator enumerator = this.files.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TarFile tarFile = enumerator.Current;
						if (tarFile.Name == filename)
						{
							return tarFile;
						}
					}
					goto IL_93;
				}
				goto IL_8B;
				IL_93:
				return TarFile.NotFound;
			}
			IL_8B:
			return virtualDirectory.GetFile(filename);
		}

		// Token: 0x0600E6BC RID: 59068 RVA: 0x0055CF3C File Offset: 0x0055B13C
		public override IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption)
		{
			TarDirectory.<GetFiles>d__28 <GetFiles>d__ = new TarDirectory.<GetFiles>d__28(-2);
			<GetFiles>d__.<>4__this = this;
			<GetFiles>d__.<>3__searchPattern = searchPattern;
			<GetFiles>d__.<>3__searchOption = searchOption;
			return <GetFiles>d__;
		}

		// Token: 0x0600E6BD RID: 59069 RVA: 0x0055CF5A File Offset: 0x0055B15A
		public override IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption)
		{
			TarDirectory.<GetDirectories>d__29 <GetDirectories>d__ = new TarDirectory.<GetDirectories>d__29(-2);
			<GetDirectories>d__.<>4__this = this;
			<GetDirectories>d__.<>3__searchPattern = searchPattern;
			<GetDirectories>d__.<>3__searchOption = searchOption;
			return <GetDirectories>d__;
		}

		// Token: 0x0600E6BE RID: 59070 RVA: 0x0055CF78 File Offset: 0x0055B178
		public override string ToString()
		{
			return string.Format("TarDirectory [{0}], {1} files", this.fullPath, this.files.Count.ToString());
		}

		// Token: 0x04009278 RID: 37496
		private static Dictionary<string, TarDirectory> cache = new Dictionary<string, TarDirectory>();

		// Token: 0x04009279 RID: 37497
		private string lazyLoadArchive;

		// Token: 0x0400927A RID: 37498
		private static readonly TarDirectory NotFound = new TarDirectory();

		// Token: 0x0400927B RID: 37499
		private string fullPath;

		// Token: 0x0400927C RID: 37500
		private string inArchiveFullPath;

		// Token: 0x0400927D RID: 37501
		private string name;

		// Token: 0x0400927E RID: 37502
		private bool exists;

		// Token: 0x0400927F RID: 37503
		public List<TarDirectory> subDirectories = new List<TarDirectory>();

		// Token: 0x04009280 RID: 37504
		public List<TarFile> files = new List<TarFile>();
	}
}
