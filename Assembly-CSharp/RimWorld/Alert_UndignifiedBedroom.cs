using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE5 RID: 7141
	public class Alert_UndignifiedBedroom : Alert
	{
		// Token: 0x0600B6C0 RID: 46784 RVA: 0x004062B8 File Offset: 0x004044B8
		public Alert_UndignifiedBedroom()
		{
			this.defaultLabel = "UndignifiedBedroom".Translate();
			this.defaultExplanation = "UndignifiedBedroomDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x17002108 RID: 8456
		// (get) Token: 0x0600B6C1 RID: 46785 RVA: 0x00406314 File Offset: 0x00404514
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
						if (pawn.royalty != null && !pawn.Suspended && pawn.royalty.AnyUnmetBedroomRequirements())
						{
							this.targetsResult.Add(pawn);
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x0600B6C2 RID: 46786 RVA: 0x004063BC File Offset: 0x004045BC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0600B6C3 RID: 46787 RVA: 0x004063C9 File Offset: 0x004045C9
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.targetsResult.Select(delegate(Pawn t)
			{
				RoyalTitle royalTitle = t.royalty.HighestTitleWithBedroomRequirements();
				RoyalTitleDef royalTitleDef = royalTitle.RoomRequirementGracePeriodActive(t) ? royalTitle.def.GetPreviousTitle(royalTitle.faction) : royalTitle.def;
				string[] array = t.royalty.GetUnmetBedroomRequirements(false, false).ToArray<string>();
				string[] array2 = t.royalty.GetUnmetBedroomRequirements(true, true).ToArray<string>();
				bool flag = royalTitleDef != null && array.Length != 0;
				this.sb.Length = 0;
				if (flag)
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
					if (flag)
					{
						this.sb.Append("\n\n");
					}
					this.sb.Append(t.LabelShort + " (" + royalTitle.def.GetLabelFor(t.gender) + ", " + "RoomRequirementGracePeriodDesc".Translate(royalTitle.RoomRequirementGracePeriodDaysLeft.ToString("0.0")) + ")" + ":\n" + array2.ToLineList("  - "));
				}
				return this.sb.ToString();
			}).ToLineList("\n", false);
		}

		// Token: 0x040068EB RID: 26859
		private List<Pawn> targetsResult = new List<Pawn>();

		// Token: 0x040068EC RID: 26860
		private StringBuilder sb = new StringBuilder();
	}
}
