using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE3 RID: 7139
	public class Alert_RoyalNoThroneAssigned : Alert
	{
		// Token: 0x0600B6B7 RID: 46775 RVA: 0x00405E34 File Offset: 0x00404034
		public Alert_RoyalNoThroneAssigned()
		{
			this.defaultLabel = "NeedThroneAssigned".Translate();
			this.defaultExplanation = "NeedThroneAssignedDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x17002106 RID: 8454
		// (get) Token: 0x0600B6B8 RID: 46776 RVA: 0x00405E84 File Offset: 0x00404084
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
						if (pawn.royalty != null && !pawn.Suspended && pawn.royalty.CanRequireThroneroom())
						{
							bool flag = false;
							List<RoyalTitle> allTitlesForReading = pawn.royalty.AllTitlesForReading;
							for (int j = 0; j < allTitlesForReading.Count; j++)
							{
								if (!allTitlesForReading[j].def.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
								{
									flag = true;
									break;
								}
							}
							if (flag && pawn.ownership.AssignedThrone == null)
							{
								this.targetsResult.Add(pawn);
							}
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x0600B6B9 RID: 46777 RVA: 0x00405F90 File Offset: 0x00404190
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0600B6BA RID: 46778 RVA: 0x00405FA0 File Offset: 0x004041A0
		public override TaggedString GetExplanation()
		{
			string text = this.defaultExplanation;
			if (MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive)
			{
				text += "\n\n" + "RoomRequirementGracePeriodDesc".Translate(MoveColonyUtility.TitleAndRoleRequirementGracePeriodTicksLeft.TicksToDays().ToString("0.0"));
			}
			return text;
		}

		// Token: 0x040068E8 RID: 26856
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
