using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE6 RID: 7142
	public class Alert_TitleRequiresBedroom : Alert
	{
		// Token: 0x0600B6C5 RID: 46789 RVA: 0x0040657C File Offset: 0x0040477C
		public Alert_TitleRequiresBedroom()
		{
			this.defaultLabel = "NeedBedroomAssigned".Translate();
			this.defaultExplanation = "NeedBedroomAssignedDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x17002109 RID: 8457
		// (get) Token: 0x0600B6C6 RID: 46790 RVA: 0x004065CC File Offset: 0x004047CC
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (pawn.royalty != null && pawn.royalty.CanRequireBedroom() && pawn.royalty.HighestTitleWithBedroomRequirements() != null && !pawn.Suspended && !pawn.royalty.HasPersonalBedroom())
							{
								this.targetsResult.Add(pawn);
							}
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x0600B6C7 RID: 46791 RVA: 0x004066A4 File Offset: 0x004048A4
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0600B6C8 RID: 46792 RVA: 0x004066B4 File Offset: 0x004048B4
		public override TaggedString GetExplanation()
		{
			string text = this.defaultExplanation;
			if (MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive)
			{
				text += "\n\n" + "RoomRequirementGracePeriodDesc".Translate(MoveColonyUtility.TitleAndRoleRequirementGracePeriodTicksLeft.TicksToDays().ToString("0.0"));
			}
			return text;
		}

		// Token: 0x040068ED RID: 26861
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
