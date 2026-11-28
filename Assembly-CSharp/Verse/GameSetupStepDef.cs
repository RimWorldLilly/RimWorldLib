using System;

namespace Verse
{
	// Token: 0x0200016B RID: 363
	public class GameSetupStepDef : Def
	{
		// Token: 0x06000ADB RID: 2779 RVA: 0x0003BD94 File Offset: 0x00039F94
		public override void PostLoad()
		{
			base.PostLoad();
			this.setupStep.def = this;
		}

		// Token: 0x0400085E RID: 2142
		public float order;

		// Token: 0x0400085F RID: 2143
		public bool executeAfterWorldSetup;

		// Token: 0x04000860 RID: 2144
		public GameSetupStep setupStep;
	}
}
