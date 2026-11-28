using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018EC RID: 6380
	public class CompProperties_MechGestatorTank : CompProperties
	{
		// Token: 0x06009FF5 RID: 40949 RVA: 0x00394FD5 File Offset: 0x003931D5
		public CompProperties_MechGestatorTank()
		{
			this.compClass = typeof(CompMechGestatorTank);
		}

		// Token: 0x04005E9A RID: 24218
		[MustTranslate]
		public string triggeredMessage;

		// Token: 0x04005E9B RID: 24219
		public FloatRange triggerRadiusRange = new FloatRange(6f, 12f);

		// Token: 0x04005E9C RID: 24220
		public GraphicData dormantGraphic;

		// Token: 0x04005E9D RID: 24221
		public GraphicData emptyGraphic;

		// Token: 0x04005E9E RID: 24222
		public SoundDef triggerSound;

		// Token: 0x04005E9F RID: 24223
		public List<PawnKindDefWeight> mechKindOptions = new List<PawnKindDefWeight>();
	}
}
