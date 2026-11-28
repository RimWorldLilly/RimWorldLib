using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000552 RID: 1362
	public class RectTrigger : PawnTrigger
	{
		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x0600287D RID: 10365 RVA: 0x000FA118 File Offset: 0x000F8318
		// (set) Token: 0x0600287E RID: 10366 RVA: 0x000FA120 File Offset: 0x000F8320
		public CellRect Rect
		{
			get
			{
				return this.rect;
			}
			set
			{
				this.rect = value;
				if (base.Spawned)
				{
					this.rect.ClipInsideMap(base.Map);
				}
			}
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000FA143 File Offset: 0x000F8343
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.rect.ClipInsideMap(base.Map);
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000FA160 File Offset: 0x000F8360
		protected override void Tick()
		{
			if (this.destroyIfUnfogged && !this.rect.CenterCell.Fogged(base.Map))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!this.IsHashIntervalTick(60) || base.Destroyed)
			{
				return;
			}
			Map map = base.Map;
			for (int i = this.rect.minZ; i <= this.rect.maxZ; i++)
			{
				for (int j = this.rect.minX; j <= this.rect.maxX; j++)
				{
					List<Thing> thingList = new IntVec3(j, 0, i).GetThingList(map);
					for (int k = 0; k < thingList.Count; k++)
					{
						if (base.TriggeredBy(thingList[k]))
						{
							base.ActivatedBy((Pawn)thingList[k]);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000FA238 File Offset: 0x000F8438
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.rect, "rect", default(CellRect), false);
			Scribe_Values.Look<bool>(ref this.destroyIfUnfogged, "destroyIfUnfogged", false, false);
			Scribe_Values.Look<bool>(ref this.activateOnExplosion, "activateOnExplosion", false, false);
		}

		// Token: 0x04001C97 RID: 7319
		private CellRect rect;

		// Token: 0x04001C98 RID: 7320
		public bool destroyIfUnfogged;

		// Token: 0x04001C99 RID: 7321
		public bool activateOnExplosion;
	}
}
