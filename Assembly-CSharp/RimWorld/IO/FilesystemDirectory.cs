using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02002401 RID: 9217
	internal class FilesystemDirectory : VirtualDirectory
	{
		// Token: 0x170028C4 RID: 10436
		// (get) Token: 0x0600E698 RID: 59032 RVA: 0x0055C653 File Offset: 0x0055A853
		public override string Name
		{
			get
			{
				return this.dirInfo.Name;
			}
		}

		// Token: 0x170028C5 RID: 10437
		// (get) Token: 0x0600E699 RID: 59033 RVA: 0x0055C660 File Offset: 0x0055A860
		public override string FullPath
		{
			get
			{
				return this.dirInfo.FullName;
			}
		}

		// Token: 0x170028C6 RID: 10438
		// (get) Token: 0x0600E69A RID: 59034 RVA: 0x0055C66D File Offset: 0x0055A86D
		public override bool Exists
		{
			get
			{
				return this.dirInfo.Exists;
			}
		}

		// Token: 0x0600E69B RID: 59035 RVA: 0x0055C67A File Offset: 0x0055A87A
		public FilesystemDirectory(string dir)
		{
			this.dirInfo = new DirectoryInfo(dir);
		}

		// Token: 0x0600E69C RID: 59036 RVA: 0x0055C68E File Offset: 0x0055A88E
		public FilesystemDirectory(DirectoryInfo dir)
		{
			this.dirInfo = dir;
		}

		// Token: 0x0600E69D RID: 59037 RVA: 0x0055C69D File Offset: 0x0055A89D
		public override IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption)
		{
			FilesystemDirectory.<GetDirectories>d__9 <GetDirectories>d__ = new FilesystemDirectory.<GetDirectories>d__9(-2);
			<GetDirectories>d__.<>4__this = this;
			<GetDirectories>d__.<>3__searchPattern = searchPattern;
			<GetDirectories>d__.<>3__searchOption = searchOption;
			return <GetDirectories>d__;
		}

		// Token: 0x0600E69E RID: 59038 RVA: 0x0055C6BB File Offset: 0x0055A8BB
		public override VirtualDirectory GetDirectory(string directoryName)
		{
			return new FilesystemDirectory(Path.Combine(this.FullPath, directoryName));
		}

		// Token: 0x0600E69F RID: 59039 RVA: 0x0055C6CE File Offset: 0x0055A8CE
		public override VirtualFile GetFile(string filename)
		{
			return new FilesystemFile(new FileInfo(Path.Combine(this.FullPath, filename)));
		}

		// Token: 0x0600E6A0 RID: 59040 RVA: 0x0055C6E6 File Offset: 0x0055A8E6
		public override IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption)
		{
			FilesystemDirectory.<GetFiles>d__12 <GetFiles>d__ = new FilesystemDirectory.<GetFiles>d__12(-2);
			<GetFiles>d__.<>4__this = this;
			<GetFiles>d__.<>3__searchPattern = searchPattern;
			<GetFiles>d__.<>3__searchOption = searchOption;
			return <GetFiles>d__;
		}

		// Token: 0x04009276 RID: 37494
		private DirectoryInfo dirInfo;
	}
}
