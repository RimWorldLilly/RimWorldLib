using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C0C RID: 7180
	public class Alert_SubjectHasNowOverseer : Alert
	{
		// Token: 0x1700211D RID: 8477
		// (get) Token: 0x0600B75C RID: 46940 RVA: 0x0040A290 File Offset: 0x00408490
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.tmpMechsCanGoFeralNow.Clear();
				this.tmpMechsCanGoFeralSoon.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
				{
					if (pawn.IsColonyMechRequiringMechanitor())
					{
						this.targets.Add(pawn);
						if (pawn.OverseerSubject.DelayUntilFeralCheckTicks > 0)
						{
							this.tmpMechsCanGoFeralSoon.Add(pawn);
						}
						else
						{
							this.tmpMechsCanGoFeralNow.Add(pawn);
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0600B75D RID: 46941 RVA: 0x0040A348 File Offset: 0x00408548
		public Alert_SubjectHasNowOverseer()
		{
			this.defaultLabel = "AlertMechLacksOverseer".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x0600B75E RID: 46942 RVA: 0x0040A398 File Offset: 0x00408598
		public override TaggedString GetExplanation()
		{
			TaggedString taggedString = "AlertMechLacksOverseerDesc".Translate(Faction.OfMechanoids);
			if (this.tmpMechsCanGoFeralSoon.Count > 0)
			{
				taggedString += "\n\n" + "AlertMechLacksOverseerUncontrolled".Translate() + ":\n" + (from m in this.tmpMechsCanGoFeralSoon
				select m.LabelCap).ToLineList("  - ", false);
			}
			if (this.tmpMechsCanGoFeralNow.Count > 0)
			{
				taggedString += "\n\n" + "AlertMechLacksOverseerMayGoFeral".Translate() + ":\n" + (from m in this.tmpMechsCanGoFeralNow
				select m.LabelCap).ToLineList("  - ", false);
			}
			return taggedString;
		}

		// Token: 0x0600B75F RID: 46943 RVA: 0x0040A495 File Offset: 0x00408695
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0400692D RID: 26925
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400692E RID: 26926
		private List<Pawn> tmpMechsCanGoFeralNow = new List<Pawn>();

		// Token: 0x0400692F RID: 26927
		private List<Pawn> tmpMechsCanGoFeralSoon = new List<Pawn>();
	}
}
