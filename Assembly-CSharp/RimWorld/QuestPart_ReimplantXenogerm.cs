using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200108A RID: 4234
	public class QuestPart_ReimplantXenogerm : QuestPart_MakeLord
	{
		// Token: 0x06006A14 RID: 27156 RVA: 0x002558DA File Offset: 0x00253ADA
		protected override Lord MakeLord()
		{
			if (!ModLister.CheckBiotech("reimplanting reward"))
			{
				return null;
			}
			return LordMaker.MakeNewLord(this.faction, new LordJob_ReimplantXenogerm(this.gatherSpot, this.waitDurationTicks, this.inSignalReimplanted), base.Map, null);
		}

		// Token: 0x06006A15 RID: 27157 RVA: 0x00255914 File Offset: 0x00253B14
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.gatherSpot, "gatherSpot", default(IntVec3), false);
			Scribe_Values.Look<string>(ref this.inSignalReimplanted, "inSignalReimplanted", null, false);
			Scribe_Values.Look<int>(ref this.waitDurationTicks, "waitDurationTicks", 15000, false);
		}

		// Token: 0x0400436E RID: 17262
		public IntVec3 gatherSpot;

		// Token: 0x0400436F RID: 17263
		public string inSignalReimplanted;

		// Token: 0x04004370 RID: 17264
		public int waitDurationTicks;
	}
}
