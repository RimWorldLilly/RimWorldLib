using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F4E RID: 8014
	public class SpecialThingFilterWorker_CorpsesLarge : SpecialThingFilterWorker
	{
		// Token: 0x0600CC6E RID: 52334 RVA: 0x004B5A5C File Offset: 0x004B3C5C
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.BodySize >= 0.75f;
		}

		// Token: 0x0600CC6F RID: 52335 RVA: 0x004B5A8C File Offset: 0x004B3C8C
		public override bool CanEverMatch(ThingDef def)
		{
			if (def.IsCorpse)
			{
				IngestibleProperties ingestible = def.ingestible;
				bool flag;
				if (ingestible == null)
				{
					flag = (null != null);
				}
				else
				{
					ThingDef sourceDef = ingestible.sourceDef;
					flag = (((sourceDef != null) ? sourceDef.race : null) != null);
				}
				if (flag)
				{
					RaceProperties race = def.ingestible.sourceDef.race;
					for (int i = 0; i < race.lifeStageAges.Count; i++)
					{
						if (race.lifeStageAges[i].def.bodySizeFactor >= 0.75f)
						{
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x0600CC70 RID: 52336 RVA: 0x004B5B0C File Offset: 0x004B3D0C
		public override bool AlwaysMatches(ThingDef def)
		{
			if (def.IsCorpse)
			{
				IngestibleProperties ingestible = def.ingestible;
				bool flag;
				if (ingestible == null)
				{
					flag = (null != null);
				}
				else
				{
					ThingDef sourceDef = ingestible.sourceDef;
					flag = (((sourceDef != null) ? sourceDef.race : null) != null);
				}
				if (flag)
				{
					RaceProperties race = def.ingestible.sourceDef.race;
					for (int i = 0; i < race.lifeStageAges.Count; i++)
					{
						if (race.lifeStageAges[i].def.bodySizeFactor < 0.75f)
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008214 RID: 33300
		private const float MinBodySize = 0.75f;
	}
}
