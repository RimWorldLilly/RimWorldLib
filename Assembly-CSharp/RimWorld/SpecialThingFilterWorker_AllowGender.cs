using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F61 RID: 8033
	public abstract class SpecialThingFilterWorker_AllowGender : SpecialThingFilterWorker
	{
		// Token: 0x0600CCB2 RID: 52402 RVA: 0x004B60CE File Offset: 0x004B42CE
		protected SpecialThingFilterWorker_AllowGender(Gender targetGender)
		{
			this.targetGender = targetGender;
		}

		// Token: 0x0600CCB3 RID: 52403 RVA: 0x004B60E0 File Offset: 0x004B42E0
		public override bool Matches(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && pawn.gender == this.targetGender;
		}

		// Token: 0x0600CCB4 RID: 52404 RVA: 0x004B6107 File Offset: 0x004B4307
		public override bool CanEverMatch(ThingDef def)
		{
			return def.category == ThingCategory.Pawn;
		}

		// Token: 0x04008215 RID: 33301
		private Gender targetGender;
	}
}
