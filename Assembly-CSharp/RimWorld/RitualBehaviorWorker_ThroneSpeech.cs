using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001684 RID: 5764
	public class RitualBehaviorWorker_ThroneSpeech : RitualBehaviorWorker
	{
		// Token: 0x0600908C RID: 37004 RVA: 0x00350D38 File Offset: 0x0034EF38
		public RitualBehaviorWorker_ThroneSpeech()
		{
		}

		// Token: 0x0600908D RID: 37005 RVA: 0x00350D40 File Offset: 0x0034EF40
		public RitualBehaviorWorker_ThroneSpeech(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600908E RID: 37006 RVA: 0x00350D49 File Offset: 0x0034EF49
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Joinable_Speech(target, organizer, ritual, this.def.stages, assignments, true);
		}
	}
}
