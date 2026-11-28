using System;

namespace Verse
{
	// Token: 0x0200016C RID: 364
	public abstract class GameSetupStep
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000ADD RID: 2781
		public abstract int SeedPart { get; }

		// Token: 0x06000ADE RID: 2782
		public abstract void GenerateFresh();

		// Token: 0x06000ADF RID: 2783 RVA: 0x0003BDB0 File Offset: 0x00039FB0
		public virtual void GenerateWithoutWorldData()
		{
			this.GenerateFresh();
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0003BDB8 File Offset: 0x00039FB8
		public virtual void GenerateFromScribe()
		{
		}

		// Token: 0x04000861 RID: 2145
		public GameSetupStepDef def;
	}
}
