using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000E0 RID: 224
	public class ConditionalStatAffecter_Clothed : ConditionalStatAffecter
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x00030098 File Offset: 0x0002E298
		public override string Label
		{
			get
			{
				return "StatsReport_Clothed".Translate();
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x000300AC File Offset: 0x0002E2AC
		public override bool Applies(StatRequest req)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.apparel != null)
				{
					using (List<Apparel>.Enumerator enumerator = pawn.apparel.WornApparel.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.def.apparel.countsAsClothingForNudity)
							{
								return true;
							}
						}
					}
					return false;
				}
			}
			return false;
		}
	}
}
