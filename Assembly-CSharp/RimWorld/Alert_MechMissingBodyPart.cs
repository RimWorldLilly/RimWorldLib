using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C13 RID: 7187
	public class Alert_MechMissingBodyPart : Alert
	{
		// Token: 0x0600B77B RID: 46971 RVA: 0x0040ADD0 File Offset: 0x00408FD0
		public Alert_MechMissingBodyPart()
		{
			this.defaultLabel = "AlertMechMissingBodyPart".Translate();
			this.labelPlural = "AlertMechsMissingBodyPart".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x0600B77C RID: 46972 RVA: 0x0040AE20 File Offset: 0x00409020
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Pawn> list = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].IsColonyMech && this.HasMissingBodyPart(list[j]))
					{
						this.targets.Add(list[j]);
					}
				}
			}
		}

		// Token: 0x0600B77D RID: 46973 RVA: 0x0040AEA8 File Offset: 0x004090A8
		private bool HasMissingBodyPart(Pawn pawn)
		{
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = hediffs[i] as Hediff_MissingPart;
				if (hediff_MissingPart != null && hediffs[i].ageTicks > 240000 && allParts.Contains(hediff_MissingPart.Part))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B77E RID: 46974 RVA: 0x0040AF21 File Offset: 0x00409121
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0600B77F RID: 46975 RVA: 0x0040AF34 File Offset: 0x00409134
		public override string GetLabel()
		{
			if (this.targets.Count != 1)
			{
				return this.labelPlural;
			}
			return this.defaultLabel;
		}

		// Token: 0x0600B780 RID: 46976 RVA: 0x0040AF54 File Offset: 0x00409154
		public override TaggedString GetExplanation()
		{
			return "AlertMechMissingBodyPartDesc".Translate() + ":\n" + (from p in this.targets
			select p.LabelCap).ToLineList("  - ", false);
		}

		// Token: 0x0400693A RID: 26938
		private const int MinAgeTicks = 240000;

		// Token: 0x0400693B RID: 26939
		private List<Pawn> targets = new List<Pawn>();

		// Token: 0x0400693C RID: 26940
		private string labelPlural;
	}
}
