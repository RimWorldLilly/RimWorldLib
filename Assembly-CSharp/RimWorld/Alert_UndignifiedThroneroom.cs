using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE4 RID: 7140
	public class Alert_UndignifiedThroneroom : Alert
	{
		// Token: 0x0600B6BB RID: 46779 RVA: 0x00406000 File Offset: 0x00404200
		public Alert_UndignifiedThroneroom()
		{
			this.defaultLabel = "UndignifiedThroneroom".Translate();
			this.defaultExplanation = "UndignifiedThroneroomDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x17002107 RID: 8455
		// (get) Token: 0x0600B6BC RID: 46780 RVA: 0x0040605C File Offset: 0x0040425C
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
					{
						if (pawn.royalty != null && !pawn.Suspended && pawn.royalty.AnyUnmetThroneroomRequirements())
						{
							this.targetsResult.Add(pawn);
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x0600B6BD RID: 46781 RVA: 0x00406104 File Offset: 0x00404304
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0600B6BE RID: 46782 RVA: 0x00406111 File Offset: 0x00404311
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.targetsResult.Select(delegate(Pawn t)
			{
				RoyalTitle royalTitle = t.royalty.HighestTitleWithThroneRoomRequirements();
				RoyalTitleDef royalTitleDef = royalTitle.RoomRequirementGracePeriodActive(t) ? royalTitle.def.GetPreviousTitle(royalTitle.faction) : royalTitle.def;
				string[] array = t.royalty.GetUnmetThroneroomRequirements(false, false).ToArray<string>();
				string[] array2 = t.royalty.GetUnmetThroneroomRequirements(true, true).ToArray<string>();
				this.sb.Length = 0;
				if (array.Length != 0)
				{
					this.sb.Append(string.Concat(new string[]
					{
						t.LabelShort,
						" (",
						royalTitleDef.GetLabelFor(t.gender),
						"):\n",
						array.ToLineList("  - ")
					}));
				}
				if (array2.Length != 0)
				{
					if (array.Length != 0)
					{
						this.sb.Append("\n\n");
					}
					this.sb.Append(t.LabelShort + " (" + royalTitle.def.GetLabelFor(t.gender) + ", " + "RoomRequirementGracePeriodDesc".Translate(royalTitle.RoomRequirementGracePeriodDaysLeft.ToString("0.0")) + ")" + ":\n" + array2.ToLineList("  - "));
				}
				return this.sb.ToString();
			}).ToLineList("\n", false);
		}

		// Token: 0x040068E9 RID: 26857
		private List<Pawn> targetsResult = new List<Pawn>();

		// Token: 0x040068EA RID: 26858
		private StringBuilder sb = new StringBuilder();
	}
}
