using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E2 RID: 6370
	public class CompLootSpawn : ThingComp
	{
		// Token: 0x17001B80 RID: 7040
		// (get) Token: 0x06009FAF RID: 40879 RVA: 0x00393DC6 File Offset: 0x00391FC6
		private CompProperties_LootSpawn Props
		{
			get
			{
				return (CompProperties_LootSpawn)this.props;
			}
		}

		// Token: 0x06009FB0 RID: 40880 RVA: 0x00393DD4 File Offset: 0x00391FD4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				return;
			}
			Building_Crate building_Crate = (Building_Crate)this.parent;
			if (this.Props.contents == null)
			{
				building_Crate.Open();
				return;
			}
			List<Thing> list = this.Props.contents.root.Generate(default(ThingSetMakerParams));
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Thing thing = list[i];
				if (!building_Crate.TryAcceptThing(thing, false))
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}
	}
}
