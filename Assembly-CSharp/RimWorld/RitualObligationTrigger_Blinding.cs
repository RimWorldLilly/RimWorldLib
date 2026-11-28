using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016B4 RID: 5812
	public class RitualObligationTrigger_Blinding : RitualObligationTrigger_EveryMember
	{
		// Token: 0x17001908 RID: 6408
		// (get) Token: 0x06009187 RID: 37255 RVA: 0x00353C95 File Offset: 0x00351E95
		public override string TriggerExtraDesc
		{
			get
			{
				return "RitualBlindingTriggerExtraDesc".Translate(this.ritual.ideo.memberName.Named("IDEOMEMBER"));
			}
		}

		// Token: 0x06009188 RID: 37256 RVA: 0x00353CC0 File Offset: 0x00351EC0
		protected override void Recache()
		{
			try
			{
				if (this.ritual.activeObligations != null)
				{
					this.ritual.activeObligations.RemoveAll(delegate(RitualObligation o)
					{
						Pawn pawn2 = o.targetA.Thing as Pawn;
						return pawn2 != null && pawn2.Ideo != this.ritual.ideo;
					});
					foreach (RitualObligation ritualObligation in this.ritual.activeObligations)
					{
						RitualObligationTrigger_Blinding.existingObligations.Add(ritualObligation.targetA.Thing as Pawn);
					}
				}
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
				{
					if (!RitualObligationTrigger_Blinding.existingObligations.Contains(pawn) && pawn.Ideo != null && pawn.Ideo == this.ritual.ideo && !pawn.IsPrisoner)
					{
						if (pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord p) => p.def == BodyPartDefOf.Eye))
						{
							this.ritual.AddObligation(new RitualObligation(this.ritual, pawn, false));
						}
					}
				}
			}
			finally
			{
				RitualObligationTrigger_Blinding.existingObligations.Clear();
			}
		}

		// Token: 0x04005862 RID: 22626
		private static List<Pawn> existingObligations = new List<Pawn>();
	}
}
