using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B4E RID: 6990
	public class ShuttleIncoming : Skyfaller, IActiveTransporter, IThingHolder
	{
		// Token: 0x17002074 RID: 8308
		// (get) Token: 0x0600B36D RID: 45933 RVA: 0x003F3C76 File Offset: 0x003F1E76
		// (set) Token: 0x0600B36E RID: 45934 RVA: 0x003F3C8E File Offset: 0x003F1E8E
		public ActiveTransporterInfo Contents
		{
			get
			{
				return ((ActiveTransporter)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveTransporter)this.innerContainer[0]).Contents = value;
			}
		}

		// Token: 0x0600B36F RID: 45935 RVA: 0x003F3CA8 File Offset: 0x003F1EA8
		protected override void SpawnThings()
		{
			for (int i = this.innerContainer.Count - 1; i >= 0; i--)
			{
				Thing thing = this.innerContainer[i];
				foreach (IntVec3 c in GenAdj.CellsOccupiedBy(base.Position, thing.Rotation, thing.def.Size))
				{
					foreach (Thing thing2 in base.Map.thingGrid.ThingsAt(c).ToList<Thing>())
					{
						if (thing2.def.IsBlueprint)
						{
							thing2.Destroy(DestroyMode.Cancel);
						}
					}
				}
				foreach (Thing thing3 in base.Map.thingGrid.ThingsAt(ThingUtility.InteractionCellWhenAt(thing.def, base.Position, thing.Rotation, base.Map)).ToList<Thing>())
				{
					if (thing3.def.IsBlueprint)
					{
						thing3.Destroy(DestroyMode.Cancel);
					}
				}
			}
			base.SpawnThings();
		}
	}
}
