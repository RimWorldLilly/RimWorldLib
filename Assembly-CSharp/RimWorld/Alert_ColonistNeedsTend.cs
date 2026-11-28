using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001BD2 RID: 7122
	public class Alert_ColonistNeedsTend : Alert
	{
		// Token: 0x0600B670 RID: 46704 RVA: 0x004044B8 File Offset: 0x004026B8
		public Alert_ColonistNeedsTend()
		{
			this.defaultLabel = "ColonistNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170020F5 RID: 8437
		// (get) Token: 0x0600B671 RID: 46705 RVA: 0x004044F4 File Offset: 0x004026F4
		private List<Pawn> NeedingColonists
		{
			get
			{
				this.needingColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if ((pawn.Spawned || pawn.BrieflyDespawned()) && pawn.health.HasHediffsNeedingTendByPlayer(true))
					{
						Building_Bed building_Bed = pawn.CurrentBed();
						if ((building_Bed == null || !building_Bed.Medical) && !Alert_ColonistNeedsRescuing.NeedsRescue(pawn) && !ChildcareUtility.BabyBeingPlayedWith(pawn))
						{
							this.needingColonistsResult.Add(pawn);
						}
					}
				}
				return this.needingColonistsResult;
			}
		}

		// Token: 0x0600B672 RID: 46706 RVA: 0x0040459C File Offset: 0x0040279C
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.needingColonistsResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				return "ColonistNeedsTreatmentDescController".Translate(this.sb.ToString().TrimEndNewlines());
			}
			return "ColonistNeedsTreatmentDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B673 RID: 46707 RVA: 0x00404660 File Offset: 0x00402860
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingColonists);
		}

		// Token: 0x040068C9 RID: 26825
		private List<Pawn> needingColonistsResult = new List<Pawn>();

		// Token: 0x040068CA RID: 26826
		private StringBuilder sb = new StringBuilder();
	}
}
