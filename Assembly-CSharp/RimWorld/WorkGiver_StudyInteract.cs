using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C40 RID: 3136
	public class WorkGiver_StudyInteract : WorkGiver_StudyBase
	{
		// Token: 0x06005937 RID: 22839 RVA: 0x0020E240 File Offset: 0x0020C440
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (t == pawn)
			{
				return false;
			}
			CompStudiable compStudiable = t.TryGetComp<CompStudiable>();
			if (compStudiable == null || compStudiable.KnowledgeCategory != null)
			{
				return false;
			}
			if (!compStudiable.EverStudiable())
			{
				JobFailReason.IsSilent();
				return false;
			}
			if (!compStudiable.CurrentlyStudiable())
			{
				if (compStudiable.Props.frequencyTicks > 0 && compStudiable.TicksTilNextStudy > 0)
				{
					JobFailReason.Is("CanBeStudiedInDuration".Translate(compStudiable.TicksTilNextStudy.ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst(), null);
				}
				else
				{
					JobFailReason.IsSilent();
				}
				return false;
			}
			IntVec3 intVec;
			if (!SocialInteractionUtility.TryGetAdjacentInteractionCell(pawn, t, forced, out intVec))
			{
				JobFailReason.Is("CannotStandNear".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x06005938 RID: 22840 RVA: 0x0020E308 File Offset: 0x0020C508
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.StudyInteract, t);
		}
	}
}
