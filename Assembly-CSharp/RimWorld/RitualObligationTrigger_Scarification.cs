using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016B2 RID: 5810
	public class RitualObligationTrigger_Scarification : RitualObligationTrigger_EveryMember
	{
		// Token: 0x17001907 RID: 6407
		// (get) Token: 0x06009181 RID: 37249 RVA: 0x00353A48 File Offset: 0x00351C48
		public override string TriggerExtraDesc
		{
			get
			{
				return "RitualScarificationTriggerExtraDesc".Translate(this.ritual.ideo.RequiredScars, this.ritual.ideo.memberName.Named("IDEOMEMBER"), this.ritual.ideo.MemberNamePlural.Named("IDEOMEMBERPLURAL"));
			}
		}

		// Token: 0x06009182 RID: 37250 RVA: 0x00353AB0 File Offset: 0x00351CB0
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
						RitualObligationTrigger_Scarification.existingObligations.Add(ritualObligation.targetA.Thing as Pawn);
					}
				}
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
				{
					if (!RitualObligationTrigger_Scarification.existingObligations.Contains(pawn) && pawn.Ideo != null && pawn.Ideo == this.ritual.ideo && !pawn.IsPrisoner)
					{
						int hediffCount = pawn.health.hediffSet.GetHediffCount(HediffDefOf.Scarification);
						if (pawn.Ideo.RequiredScars > hediffCount)
						{
							this.ritual.AddObligation(new RitualObligation(this.ritual, pawn, false));
						}
					}
				}
			}
			finally
			{
				RitualObligationTrigger_Scarification.existingObligations.Clear();
			}
		}

		// Token: 0x04005861 RID: 22625
		private static List<Pawn> existingObligations = new List<Pawn>();
	}
}
