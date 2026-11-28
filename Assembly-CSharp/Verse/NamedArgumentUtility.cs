using System;

namespace Verse
{
	// Token: 0x02000209 RID: 521
	public static class NamedArgumentUtility
	{
		// Token: 0x06000F2A RID: 3882 RVA: 0x000527E0 File Offset: 0x000509E0
		public static NamedArgument Named(this object arg, string label)
		{
			return new NamedArgument(arg, label);
		}
	}
}
