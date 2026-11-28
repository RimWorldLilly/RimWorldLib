using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BAD RID: 7085
	public class Alert_MeatHunger : Alert_Critical
	{
		// Token: 0x0600B5DA RID: 46554 RVA: 0x00400C08 File Offset: 0x003FEE08
		public Alert_MeatHunger()
		{
			this.defaultLabel = "AlertMeatHunger".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireAnomaly = true;
		}

		// Token: 0x170020E7 RID: 8423
		// (get) Token: 0x0600B5DB RID: 46555 RVA: 0x00400C54 File Offset: 0x003FEE54
		private List<Pawn> MeatHungerPawns
		{
			get
			{
				this.targets.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i];
					if (pawn.IsGhoul)
					{
						Pawn_NeedsTracker needs = pawn.needs;
						if (((needs != null) ? needs.food : null) != null && pawn.needs.food.Starving)
						{
							this.targets.Add(pawn);
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0600B5DC RID: 46556 RVA: 0x00400CD0 File Offset: 0x003FEED0
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.targets)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("AlertMeatHungerDesc".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(":\n");
			defaultInterpolatedStringHandler.AppendFormatted(this.sb.ToString().TrimEndNewlines());
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("AlertMeatHungerDescAppended".Translate());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600B5DD RID: 46557 RVA: 0x00400DB4 File Offset: 0x003FEFB4
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.MeatHungerPawns);
		}

		// Token: 0x0400688F RID: 26767
		private List<Pawn> targets = new List<Pawn>();

		// Token: 0x04006890 RID: 26768
		private StringBuilder sb = new StringBuilder();
	}
}
