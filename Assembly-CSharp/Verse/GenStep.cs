using System;

namespace Verse
{
	// Token: 0x0200016E RID: 366
	public abstract class GenStep
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000AE4 RID: 2788
		public abstract int SeedPart { get; }

		// Token: 0x06000AE5 RID: 2789
		public abstract void Generate(Map map, GenStepParams parms);

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0003BDDE File Offset: 0x00039FDE
		public virtual void PostMapInitialized(Map map, GenStepParams parms)
		{
		}

		// Token: 0x04000866 RID: 2150
		public GenStepDef def;
	}
}
