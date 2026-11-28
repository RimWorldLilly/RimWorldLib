using System;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02002406 RID: 9222
	public abstract class VirtualFile
	{
		// Token: 0x170028D5 RID: 10453
		// (get) Token: 0x0600E6D8 RID: 59096
		public abstract string Name { get; }

		// Token: 0x170028D6 RID: 10454
		// (get) Token: 0x0600E6D9 RID: 59097
		public abstract string FullPath { get; }

		// Token: 0x170028D7 RID: 10455
		// (get) Token: 0x0600E6DA RID: 59098
		public abstract bool Exists { get; }

		// Token: 0x170028D8 RID: 10456
		// (get) Token: 0x0600E6DB RID: 59099
		public abstract long Length { get; }

		// Token: 0x0600E6DC RID: 59100
		public abstract Stream CreateReadStream();

		// Token: 0x0600E6DD RID: 59101
		public abstract string ReadAllText();

		// Token: 0x0600E6DE RID: 59102
		public abstract string[] ReadAllLines();

		// Token: 0x0600E6DF RID: 59103
		public abstract byte[] ReadAllBytes();
	}
}
