using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200007A RID: 122
	public static class GenFile
	{
		// Token: 0x06000513 RID: 1299 RVA: 0x0001DFD0 File Offset: 0x0001C1D0
		public static string TextFromRawFile(string filePath)
		{
			return File.ReadAllText(filePath);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001DFD8 File Offset: 0x0001C1D8
		public static string TextFromResourceFile(string filePath)
		{
			TextAsset textAsset = Resources.Load<TextAsset>("Text/" + filePath);
			if (textAsset == null)
			{
				Log.Message("Found no text asset in resources at " + filePath);
				return null;
			}
			return GenFile.GetTextWithoutBOM(textAsset);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001E018 File Offset: 0x0001C218
		public static string GetTextWithoutBOM(TextAsset textAsset)
		{
			string result = null;
			using (MemoryStream memoryStream = new MemoryStream(textAsset.bytes))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001E078 File Offset: 0x0001C278
		public static IEnumerable<string> LinesFromFile(string filePath)
		{
			GenFile.<LinesFromFile>d__3 <LinesFromFile>d__ = new GenFile.<LinesFromFile>d__3(-2);
			<LinesFromFile>d__.<>3__filePath = filePath;
			return <LinesFromFile>d__;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001E088 File Offset: 0x0001C288
		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool useLinuxLineEndings = false, Func<string, string> destFileNameGetter = null)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
			}
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				string text = fileInfo.Name;
				if (destFileNameGetter != null)
				{
					text = destFileNameGetter(text);
				}
				string text2 = Path.Combine(destDirName, text);
				if (useLinuxLineEndings && (fileInfo.Extension == ".sh" || fileInfo.Extension == ".txt"))
				{
					if (!File.Exists(text2))
					{
						File.WriteAllText(text2, File.ReadAllText(fileInfo.FullName).Replace("\r\n", "\n").Replace("\r", "\n"));
					}
				}
				else
				{
					fileInfo.CopyTo(text2, false);
				}
			}
			if (copySubDirs)
			{
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
					GenFile.DirectoryCopy(directoryInfo2.FullName, destDirName2, copySubDirs, useLinuxLineEndings, null);
				}
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001E1B0 File Offset: 0x0001C3B0
		public static string SanitizedFileName(string fileName)
		{
			char[] invalidFileNameChars = GenFile.InvalidFileNameChars;
			string text = "";
			for (int i = 0; i < fileName.Length; i++)
			{
				if (!invalidFileNameChars.Contains(fileName[i]))
				{
					ReadOnlySpan<char> str = text;
					char c = fileName[i];
					text = str + new ReadOnlySpan<char>(ref c);
				}
			}
			if (text.Length == 0)
			{
				text = "unnamed";
			}
			return text;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001E214 File Offset: 0x0001C414
		public static string ResolveCaseInsensitiveFilePath(string dir, string targetFileName)
		{
			string text = Path.Combine(dir, targetFileName);
			if (File.Exists(text))
			{
				return text;
			}
			if (Directory.Exists(dir))
			{
				foreach (string path in Directory.GetFiles(dir))
				{
					if (string.Compare(Path.GetFileName(path), targetFileName, StringComparison.CurrentCultureIgnoreCase) == 0)
					{
						ReadOnlySpan<char> str = dir;
						char directorySeparatorChar = Path.DirectorySeparatorChar;
						return str + new ReadOnlySpan<char>(ref directorySeparatorChar) + Path.GetFileName(path);
					}
				}
			}
			ReadOnlySpan<char> str2 = dir;
			char directorySeparatorChar2 = Path.DirectorySeparatorChar;
			return str2 + new ReadOnlySpan<char>(ref directorySeparatorChar2) + targetFileName;
		}

		// Token: 0x040001C5 RID: 453
		private static readonly char[] InvalidFileNameChars = (from c in Enumerable.Range(0, 32)
		select (char)c).Concat("\"<>|:*?\\/").ToArray<char>();
	}
}
