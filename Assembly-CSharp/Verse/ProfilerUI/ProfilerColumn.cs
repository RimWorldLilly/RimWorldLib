using System;

namespace Verse.ProfilerUI
{
	// Token: 0x0200079A RID: 1946
	public sealed class ProfilerColumn
	{
		// Token: 0x06003DF8 RID: 15864 RVA: 0x0018A862 File Offset: 0x00188A62
		public ProfilerColumn(ProfilerColumnKind kind, float width, Func<string> header, Action sort = null)
		{
			this.kind = kind;
			this.width = width;
			this.header = header;
			this.sort = sort;
		}

		// Token: 0x040028D3 RID: 10451
		public ProfilerColumnKind kind;

		// Token: 0x040028D4 RID: 10452
		public float width;

		// Token: 0x040028D5 RID: 10453
		public Func<string> header;

		// Token: 0x040028D6 RID: 10454
		public Action sort;
	}
}
