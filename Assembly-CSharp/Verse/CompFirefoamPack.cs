using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000538 RID: 1336
	public class CompFirefoamPack : CompAIUsablePack
	{
		// Token: 0x06002756 RID: 10070 RVA: 0x000F60DC File Offset: 0x000F42DC
		protected override float ChanceToUse(Pawn wearer)
		{
			if (wearer.GetAttachment(ThingDefOf.Fire) != null)
			{
				return 1f;
			}
			int num = GenRadial.NumCellsInRadius(1.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = wearer.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(wearer.Map))
				{
					List<Thing> thingList = c.GetThingList(wearer.Map);
					int j = 0;
					while (j < thingList.Count)
					{
						if (thingList[j] is Fire || thingList[j].HasAttachment(ThingDefOf.Fire))
						{
							if (i == 0)
							{
								return 1f;
							}
							return 0.2f;
						}
						else
						{
							j++;
						}
					}
				}
			}
			return 0f;
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000F6194 File Offset: 0x000F4394
		protected override void UsePack(Pawn wearer)
		{
			Verb_FirefoamPop.Pop(wearer, this.parent.TryGetComp<CompExplosive>(), this.parent.TryGetComp<CompApparelReloadable>());
		}

		// Token: 0x04001C43 RID: 7235
		private const float ChanceToUseWithNearbyFire = 0.2f;
	}
}
