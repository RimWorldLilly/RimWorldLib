using System;
using System.Linq;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000429 RID: 1065
	public class HediffComp_Disorientation : HediffComp
	{
		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001F14 RID: 7956 RVA: 0x000BDA5B File Offset: 0x000BBC5B
		private HediffCompProperties_Disorientation Props
		{
			get
			{
				return (HediffCompProperties_Disorientation)this.props;
			}
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x000BDA68 File Offset: 0x000BBC68
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.Props.wanderMtbHours > 0f && base.Pawn.Spawned && !base.Pawn.Downed && base.Pawn.Awake() && base.Pawn.CurJobDef.suspendable && this.Props.wanderMtbHours > 0f && base.Pawn.IsHashIntervalTick(60, delta) && Rand.MTBEventOccurs(this.Props.wanderMtbHours, 2500f, 60f) && base.Pawn.CurJob.def != JobDefOf.GotoMindControlled)
			{
				IntVec3 c2 = (from c in GenRadial.RadialCellsAround(base.Pawn.Position, this.Props.wanderRadius, false)
				where c.Standable(base.Pawn.MapHeld) && base.Pawn.CanReach(c, PathEndMode.OnCell, Danger.Unspecified, false, false, TraverseMode.ByPawn)
				select c).RandomElementWithFallback(IntVec3.Invalid);
				if (c2.IsValid)
				{
					MoteMaker.MakeThoughtBubble(base.Pawn, "Things/Mote/Disoriented", false);
					Job job = JobMaker.MakeJob(JobDefOf.GotoMindControlled, c2);
					job.expiryInterval = this.Props.singleWanderDurationTicks;
					base.Pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
				}
			}
		}

		// Token: 0x0400184E RID: 6222
		private const string MoteTexPath = "Things/Mote/Disoriented";
	}
}
