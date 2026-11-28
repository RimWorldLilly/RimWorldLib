using System;

namespace LudeonTK
{
	// Token: 0x0200003D RID: 61
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugOutputAttribute : Attribute
	{
		// Token: 0x06000192 RID: 402 RVA: 0x0000B9B7 File Offset: 0x00009BB7
		public DebugOutputAttribute()
		{
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000B9CA File Offset: 0x00009BCA
		public DebugOutputAttribute(bool onlyWhenPlaying)
		{
			this.onlyWhenPlaying = onlyWhenPlaying;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000B9E4 File Offset: 0x00009BE4
		public DebugOutputAttribute(string category, bool onlyWhenPlaying = false) : this(onlyWhenPlaying)
		{
			this.category = category;
		}

		// Token: 0x040000B1 RID: 177
		public string name;

		// Token: 0x040000B2 RID: 178
		public string category = "General";

		// Token: 0x040000B3 RID: 179
		public bool onlyWhenPlaying;
	}
}
