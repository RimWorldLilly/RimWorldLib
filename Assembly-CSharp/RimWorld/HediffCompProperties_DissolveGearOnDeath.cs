using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002088 RID: 8328
	public class HediffCompProperties_DissolveGearOnDeath : HediffCompProperties
	{
		// Token: 0x0600D20D RID: 53773 RVA: 0x004D2A50 File Offset: 0x004D0C50
		public HediffCompProperties_DissolveGearOnDeath()
		{
			this.compClass = typeof(HediffComp_DissolveGearOnDeath);
		}

		// Token: 0x04008515 RID: 34069
		public FleckDef fleck;

		// Token: 0x04008516 RID: 34070
		public ThingDef mote;

		// Token: 0x04008517 RID: 34071
		public int moteCount = 3;

		// Token: 0x04008518 RID: 34072
		public FloatRange moteOffsetRange = new FloatRange(0.2f, 0.4f);

		// Token: 0x04008519 RID: 34073
		public ThingDef filth;

		// Token: 0x0400851A RID: 34074
		public int filthCount = 4;

		// Token: 0x0400851B RID: 34075
		public HediffDef injuryCreatedOnDeath;

		// Token: 0x0400851C RID: 34076
		public IntRange injuryCount;

		// Token: 0x0400851D RID: 34077
		public SoundDef sound;
	}
}
