using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C15 RID: 7189
	public class Alert_PsychicBondedSeparated : Alert
	{
		// Token: 0x0600B786 RID: 46982 RVA: 0x0040B0FF File Offset: 0x004092FF
		public Alert_PsychicBondedSeparated()
		{
			this.defaultLabel = "AlertPsychicBondedPawnsSeparated".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x0600B787 RID: 46983 RVA: 0x0040B12E File Offset: 0x0040932E
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0600B788 RID: 46984 RVA: 0x0040B144 File Offset: 0x00409344
		public override TaggedString GetExplanation()
		{
			return "AlertPsychicBondedPawnsSeparatedDesc".Translate() + ":\n" + (from x in this.targets
			select x.NameShortColored.Resolve()).ToLineList("  - ", true);
		}

		// Token: 0x0600B789 RID: 46985 RVA: 0x0040B1A0 File Offset: 0x004093A0
		private void GetTargets()
		{
			this.targets.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned)
			{
				if (pawn.RaceProps.Humanlike && pawn.Faction == Faction.OfPlayer)
				{
					Hediff_PsychicBond hediff_PsychicBond = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
					if (hediff_PsychicBond != null && !ThoughtWorker_PsychicBondProximity.NearPsychicBondedPerson(pawn, hediff_PsychicBond))
					{
						this.targets.Add(pawn);
					}
				}
			}
		}

		// Token: 0x0400693E RID: 26942
		private List<Pawn> targets = new List<Pawn>();
	}
}
