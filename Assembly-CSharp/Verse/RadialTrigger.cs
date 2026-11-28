using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000551 RID: 1361
	public class RadialTrigger : PawnTrigger
	{
		// Token: 0x0600287A RID: 10362 RVA: 0x000FA020 File Offset: 0x000F8220
		protected override void Tick()
		{
			if (!this.IsHashIntervalTick(60))
			{
				return;
			}
			Map map = base.Map;
			int num = GenRadial.NumCellsInRadius((float)this.maxRadius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = base.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (base.TriggeredBy(thingList[j]) && (!this.lineOfSight || GenSight.LineOfSightToThing(base.Position, thingList[j], map, false, null)))
						{
							base.ActivatedBy((Pawn)thingList[j]);
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000FA0E4 File Offset: 0x000F82E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.maxRadius, "maxRadius", 0, false);
			Scribe_Values.Look<bool>(ref this.lineOfSight, "lineOfSight", false, false);
		}

		// Token: 0x04001C95 RID: 7317
		public int maxRadius;

		// Token: 0x04001C96 RID: 7318
		public bool lineOfSight;
	}
}
