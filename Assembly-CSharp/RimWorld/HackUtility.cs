using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B4 RID: 6324
	public static class HackUtility
	{
		// Token: 0x06009E84 RID: 40580 RVA: 0x0038E6B7 File Offset: 0x0038C8B7
		public static bool IsHackable(this Thing thing)
		{
			return thing.TryGetComp<CompHackable>() != null;
		}

		// Token: 0x06009E85 RID: 40581 RVA: 0x0038E6C4 File Offset: 0x0038C8C4
		public static bool IsHacked(this Thing thing)
		{
			CompHackable compHackable = thing.TryGetComp<CompHackable>();
			return compHackable != null && compHackable.IsHacked;
		}

		// Token: 0x06009E86 RID: 40582 RVA: 0x0038E6E4 File Offset: 0x0038C8E4
		public static bool IsCapableOfHacking(Pawn pawn)
		{
			return !pawn.IsSubhuman && pawn.skills != null && !pawn.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled && !StatDefOf.HackingSpeed.Worker.IsDisabledFor(pawn) && !pawn.WorkTagIsDisabled(WorkTags.Intellectual) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}
	}
}
