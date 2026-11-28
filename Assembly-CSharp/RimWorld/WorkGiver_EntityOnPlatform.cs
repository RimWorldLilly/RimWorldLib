using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C49 RID: 3145
	public abstract class WorkGiver_EntityOnPlatform : WorkGiver_Scanner
	{
		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06005967 RID: 22887 RVA: 0x0020EAB1 File Offset: 0x0020CCB1
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.EntityHolder);
			}
		}

		// Token: 0x06005968 RID: 22888 RVA: 0x0020EABA File Offset: 0x0020CCBA
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.AnomalyActive;
		}

		// Token: 0x06005969 RID: 22889 RVA: 0x0020EAC4 File Offset: 0x0020CCC4
		public override string PostProcessedGerund(Job job)
		{
			Pawn entity = this.GetEntity(job.targetA.Thing);
			return "DoWorkAtThing".Translate(this.def.gerund.Named("GERUND"), entity.LabelShort.Named("TARGETLABEL"));
		}

		// Token: 0x0600596A RID: 22890
		protected abstract Pawn GetEntity(Thing potentialPlatform);
	}
}
