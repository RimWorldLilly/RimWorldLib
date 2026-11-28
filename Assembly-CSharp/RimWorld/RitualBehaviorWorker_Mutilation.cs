using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001681 RID: 5761
	public class RitualBehaviorWorker_Mutilation : RitualBehaviorWorker
	{
		// Token: 0x0600907A RID: 36986 RVA: 0x003508EC File Offset: 0x0034EAEC
		public RitualBehaviorWorker_Mutilation()
		{
		}

		// Token: 0x0600907B RID: 36987 RVA: 0x003508F4 File Offset: 0x0034EAF4
		public RitualBehaviorWorker_Mutilation(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600907C RID: 36988 RVA: 0x003508FD File Offset: 0x0034EAFD
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Ritual_Mutilation(target, ritual, obligation, this.def.stages, assignments, organizer);
		}
	}
}
