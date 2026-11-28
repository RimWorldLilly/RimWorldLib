using System;

namespace Verse
{
	// Token: 0x020001DC RID: 476
	public class WorldGenStepDef : Def
	{
		// Token: 0x06000D39 RID: 3385 RVA: 0x00044477 File Offset: 0x00042677
		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}

		// Token: 0x04000D7A RID: 3450
		public float order;

		// Token: 0x04000D7B RID: 3451
		public WorldGenStep worldGenStep;
	}
}
