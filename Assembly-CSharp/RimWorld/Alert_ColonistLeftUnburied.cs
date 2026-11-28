using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD1 RID: 7121
	public class Alert_ColonistLeftUnburied : Alert
	{
		// Token: 0x0600B66C RID: 46700 RVA: 0x00404338 File Offset: 0x00402538
		public static bool IsCorpseOfColonist(Corpse corpse)
		{
			return corpse.InnerPawn.Faction == Faction.OfPlayer && corpse.InnerPawn.def.race.Humanlike && !corpse.InnerPawn.IsQuestLodger() && !corpse.InnerPawn.IsSlave && !corpse.InnerPawn.IsSubhuman && !corpse.InnerPawn.IsShambler && corpse.Spawned;
		}

		// Token: 0x170020F4 RID: 8436
		// (get) Token: 0x0600B66D RID: 46701 RVA: 0x004043AC File Offset: 0x004025AC
		private List<Thing> UnburiedColonistCorpses
		{
			get
			{
				this.unburiedColonistCorpsesResult.Clear();
				foreach (Map map in Find.Maps)
				{
					if (map.mapPawns.AnyFreeColonistSpawned)
					{
						List<Thing> list = map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.Corpse));
						for (int i = 0; i < list.Count; i++)
						{
							Corpse corpse = (Corpse)list[i];
							if (Alert_ColonistLeftUnburied.IsCorpseOfColonist(corpse))
							{
								this.unburiedColonistCorpsesResult.Add(corpse);
							}
						}
					}
				}
				return this.unburiedColonistCorpsesResult;
			}
		}

		// Token: 0x0600B66E RID: 46702 RVA: 0x0040445C File Offset: 0x0040265C
		public Alert_ColonistLeftUnburied()
		{
			this.defaultLabel = "AlertColonistLeftUnburied".Translate();
			this.defaultExplanation = "AlertColonistLeftUnburiedDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600B66F RID: 46703 RVA: 0x004044AB File Offset: 0x004026AB
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.UnburiedColonistCorpses);
		}

		// Token: 0x040068C8 RID: 26824
		private List<Thing> unburiedColonistCorpsesResult = new List<Thing>();
	}
}
