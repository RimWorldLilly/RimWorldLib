using System;

namespace LudeonTK
{
	// Token: 0x02000043 RID: 67
	public struct DebugMenuOption
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x0000CBA3 File Offset: 0x0000ADA3
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}

		// Token: 0x040000D7 RID: 215
		public DebugMenuOptionMode mode;

		// Token: 0x040000D8 RID: 216
		public string label;

		// Token: 0x040000D9 RID: 217
		public Action method;
	}
}
