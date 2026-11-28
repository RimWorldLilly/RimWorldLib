using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E63 RID: 3683
	public class ThoughtWorker_HediffThoughtRandom : ThoughtWorker
	{
		// Token: 0x060061E3 RID: 25059 RVA: 0x002341D4 File Offset: 0x002323D4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				HediffWithComps hediffWithComps = hediff as HediffWithComps;
				if (hediffWithComps != null)
				{
					foreach (HediffComp hediffComp in hediffWithComps.comps)
					{
						HediffComp_GiveRandomSituationalThought hediffComp_GiveRandomSituationalThought = hediffComp as HediffComp_GiveRandomSituationalThought;
						if (hediffComp_GiveRandomSituationalThought != null && hediffComp_GiveRandomSituationalThought.selectedThought == this.def)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
