using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C16 RID: 7190
	public class Alert_MechDamaged : Alert
	{
		// Token: 0x0600B78A RID: 46986 RVA: 0x0040B240 File Offset: 0x00409440
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Pawn> list = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].IsColonyMech && MechRepairUtility.CanRepair(list[j]))
					{
						this.targets.Add(list[j]);
					}
				}
			}
		}

		// Token: 0x0600B78B RID: 46987 RVA: 0x0040B2C5 File Offset: 0x004094C5
		public Alert_MechDamaged()
		{
			this.defaultLabel = "AlertMechNeedsRepair".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x0600B78C RID: 46988 RVA: 0x0040B2F4 File Offset: 0x004094F4
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0600B78D RID: 46989 RVA: 0x0040B308 File Offset: 0x00409508
		public override TaggedString GetExplanation()
		{
			return "AlertMechNeedsRepairDescPrefix".Translate() + ":\n" + (from p in this.targets
			select p.LabelCap).ToLineList("  - ", false) + "\n\n" + "AlertMechNeedsRepairDesc".Translate();
		}

		// Token: 0x0400693F RID: 26943
		private List<Pawn> targets = new List<Pawn>();
	}
}
