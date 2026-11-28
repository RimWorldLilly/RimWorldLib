using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02002405 RID: 9221
	public abstract class VirtualDirectory
	{
		// Token: 0x170028D2 RID: 10450
		// (get) Token: 0x0600E6CD RID: 59085
		public abstract string Name { get; }

		// Token: 0x170028D3 RID: 10451
		// (get) Token: 0x0600E6CE RID: 59086
		public abstract string FullPath { get; }

		// Token: 0x170028D4 RID: 10452
		// (get) Token: 0x0600E6CF RID: 59087
		public abstract bool Exists { get; }

		// Token: 0x0600E6D0 RID: 59088
		public abstract VirtualDirectory GetDirectory(string directoryName);

		// Token: 0x0600E6D1 RID: 59089
		public abstract VirtualFile GetFile(string filename);

		// Token: 0x0600E6D2 RID: 59090
		public abstract IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption);

		// Token: 0x0600E6D3 RID: 59091
		public abstract IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption);

		// Token: 0x0600E6D4 RID: 59092 RVA: 0x0055D17D File Offset: 0x0055B37D
		public string ReadAllText(string filename)
		{
			return this.GetFile(filename).ReadAllText();
		}

		// Token: 0x0600E6D5 RID: 59093 RVA: 0x0055D18B File Offset: 0x0055B38B
		public bool FileExists(string filename)
		{
			return this.GetFile(filename).Exists;
		}

		// Token: 0x0600E6D6 RID: 59094 RVA: 0x0055D199 File Offset: 0x0055B399
		public override string ToString()
		{
			return this.FullPath;
		}
	}
}
