using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD3 RID: 7123
	public class Alert_EntityNeedsTend : Alert
	{
		// Token: 0x0600B674 RID: 46708 RVA: 0x0040466D File Offset: 0x0040286D
		public Alert_EntityNeedsTend()
		{
			this.defaultLabel = "EntityNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170020F6 RID: 8438
		// (get) Token: 0x0600B675 RID: 46709 RVA: 0x004046A8 File Offset: 0x004028A8
		private List<Pawn> NeedingEntities
		{
			get
			{
				this.needingEntitiesResult.Clear();
				foreach (Map map in Find.Maps)
				{
					foreach (Thing potentialPlatform in map.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder))
					{
						Pawn tendableEnityFromPotentialPlatform = WorkGiver_TendEntity.GetTendableEnityFromPotentialPlatform(potentialPlatform);
						if (tendableEnityFromPotentialPlatform != null && HealthAIUtility.ShouldBeTendedNowByPlayerUrgent(tendableEnityFromPotentialPlatform))
						{
							this.needingEntitiesResult.Add(tendableEnityFromPotentialPlatform);
						}
					}
				}
				return this.needingEntitiesResult;
			}
		}

		// Token: 0x0600B676 RID: 46710 RVA: 0x00404760 File Offset: 0x00402960
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.needingEntitiesResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "EntityNeedsTreatmentDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B677 RID: 46711 RVA: 0x004047FC File Offset: 0x004029FC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingEntities);
		}

		// Token: 0x040068CB RID: 26827
		private List<Pawn> needingEntitiesResult = new List<Pawn>();

		// Token: 0x040068CC RID: 26828
		private StringBuilder sb = new StringBuilder();
	}
}
