using System;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02002402 RID: 9218
	internal class FilesystemFile : VirtualFile
	{
		// Token: 0x170028C7 RID: 10439
		// (get) Token: 0x0600E6A1 RID: 59041 RVA: 0x0055C704 File Offset: 0x0055A904
		public override string Name
		{
			get
			{
				return this.fileInfo.Name;
			}
		}

		// Token: 0x170028C8 RID: 10440
		// (get) Token: 0x0600E6A2 RID: 59042 RVA: 0x0055C711 File Offset: 0x0055A911
		public override string FullPath
		{
			get
			{
				return this.fileInfo.FullName;
			}
		}

		// Token: 0x170028C9 RID: 10441
		// (get) Token: 0x0600E6A3 RID: 59043 RVA: 0x0055C71E File Offset: 0x0055A91E
		public override bool Exists
		{
			get
			{
				return this.fileInfo.Exists;
			}
		}

		// Token: 0x170028CA RID: 10442
		// (get) Token: 0x0600E6A4 RID: 59044 RVA: 0x0055C72B File Offset: 0x0055A92B
		public override long Length
		{
			get
			{
				if (!this.fileInfo.Exists)
				{
					return 0L;
				}
				return this.fileInfo.Length;
			}
		}

		// Token: 0x0600E6A5 RID: 59045 RVA: 0x0055C748 File Offset: 0x0055A948
		public FilesystemFile(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
		}

		// Token: 0x0600E6A6 RID: 59046 RVA: 0x0055C757 File Offset: 0x0055A957
		public override Stream CreateReadStream()
		{
			return this.fileInfo.OpenRead();
		}

		// Token: 0x0600E6A7 RID: 59047 RVA: 0x0055C764 File Offset: 0x0055A964
		public override byte[] ReadAllBytes()
		{
			return File.ReadAllBytes(this.fileInfo.FullName);
		}

		// Token: 0x0600E6A8 RID: 59048 RVA: 0x0055C776 File Offset: 0x0055A976
		public override string[] ReadAllLines()
		{
			return File.ReadAllLines(this.fileInfo.FullName);
		}

		// Token: 0x0600E6A9 RID: 59049 RVA: 0x0055C788 File Offset: 0x0055A988
		public override string ReadAllText()
		{
			return File.ReadAllText(this.fileInfo.FullName);
		}

		// Token: 0x0600E6AA RID: 59050 RVA: 0x0055C79A File Offset: 0x0055A99A
		public static implicit operator FilesystemFile(FileInfo fileInfo)
		{
			return new FilesystemFile(fileInfo);
		}

		// Token: 0x0600E6AB RID: 59051 RVA: 0x0055C7A4 File Offset: 0x0055A9A4
		public override string ToString()
		{
			return string.Format("FilesystemFile [{0}], Length {1}", this.FullPath, this.Length.ToString());
		}

		// Token: 0x04009277 RID: 37495
		private FileInfo fileInfo;
	}
}
