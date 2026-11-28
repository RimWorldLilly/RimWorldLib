using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02002400 RID: 9216
	public static class AbstractFilesystem
	{
		// Token: 0x0600E695 RID: 59029 RVA: 0x0055C504 File Offset: 0x0055A704
		public static void ClearAllCache()
		{
			TarDirectory.ClearCache();
		}

		// Token: 0x0600E696 RID: 59030 RVA: 0x0055C50C File Offset: 0x0055A70C
		public static List<VirtualDirectory> GetDirectories(string filesystemPath, string searchPattern, SearchOption searchOption, bool allowArchiveAndRealFolderDuplicates = false)
		{
			List<VirtualDirectory> list = new List<VirtualDirectory>();
			foreach (string text in Directory.GetDirectories(filesystemPath, searchPattern, searchOption))
			{
				string text2 = text + ".tar";
				if (!allowArchiveAndRealFolderDuplicates && File.Exists(text2))
				{
					list.Add(TarDirectory.ReadFromFileOrCache(text2));
				}
				else
				{
					list.Add(new FilesystemDirectory(text));
				}
			}
			foreach (string text3 in Directory.GetFiles(filesystemPath, searchPattern, searchOption))
			{
				if (!(Path.GetExtension(text3) != ".tar"))
				{
					if (!allowArchiveAndRealFolderDuplicates)
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text3);
						bool flag = false;
						using (List<VirtualDirectory>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.Name == fileNameWithoutExtension)
								{
									flag = true;
									break;
								}
							}
						}
						if (flag)
						{
							goto IL_D7;
						}
					}
					list.Add(TarDirectory.ReadFromFileOrCache(text3));
				}
				IL_D7:;
			}
			return list;
		}

		// Token: 0x0600E697 RID: 59031 RVA: 0x0055C60C File Offset: 0x0055A80C
		public static VirtualDirectory GetDirectory(string filesystemPath)
		{
			if (Path.GetExtension(filesystemPath) == ".tar")
			{
				return TarDirectory.ReadFromFileOrCache(filesystemPath);
			}
			string text = filesystemPath + ".tar";
			if (File.Exists(text))
			{
				return TarDirectory.ReadFromFileOrCache(text);
			}
			return new FilesystemDirectory(filesystemPath);
		}
	}
}
