using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001643 RID: 5699
	public class PreceptWorker_RitualSeat : PreceptWorker
	{
		// Token: 0x06008ECB RID: 36555 RVA: 0x00347B44 File Offset: 0x00345D44
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
		{
			PreceptWorker_RitualSeat.tmpRitualSeats.Clear();
			List<ThingDef> list = null;
			foreach (MemeDef memeDef in ideo.memes)
			{
				if (!memeDef.requireAnyRitualSeat.NullOrEmpty<ThingDef>())
				{
					if (list == null)
					{
						PreceptWorker_RitualSeat.tmpRitualSeats.AddRange(memeDef.requireAnyRitualSeat);
						list = PreceptWorker_RitualSeat.tmpRitualSeats;
					}
					else
					{
						PreceptWorker_RitualSeat.tmpRitualSeatsMerge.Clear();
						foreach (ThingDef item in memeDef.requireAnyRitualSeat)
						{
							if (list.Contains(item))
							{
								PreceptWorker_RitualSeat.tmpRitualSeatsMerge.Add(item);
							}
						}
						list.Clear();
						list.AddRange(PreceptWorker_RitualSeat.tmpRitualSeatsMerge);
						if (list.Count == 0)
						{
							Log.Error("Ideo has 2 memes which have conflicting ritual set requirements!");
						}
					}
				}
			}
			return list == null || list.Contains(def);
		}

		// Token: 0x04005777 RID: 22391
		private static List<ThingDef> tmpRitualSeats = new List<ThingDef>();

		// Token: 0x04005778 RID: 22392
		private static List<ThingDef> tmpRitualSeatsMerge = new List<ThingDef>();
	}
}
