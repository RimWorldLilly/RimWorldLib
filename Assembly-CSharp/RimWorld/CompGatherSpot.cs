using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200189D RID: 6301
	public class CompGatherSpot : ThingComp
	{
		// Token: 0x17001B05 RID: 6917
		// (get) Token: 0x06009DCE RID: 40398 RVA: 0x0038B04D File Offset: 0x0038924D
		// (set) Token: 0x06009DCF RID: 40399 RVA: 0x0038B058 File Offset: 0x00389258
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				if (value == this.active)
				{
					return;
				}
				this.active = value;
				if (this.parent.Spawned)
				{
					if (this.active)
					{
						this.parent.Map.gatherSpotLister.RegisterActivated(this);
						return;
					}
					this.parent.Map.gatherSpotLister.RegisterDeactivated(this);
				}
			}
		}

		// Token: 0x06009DD0 RID: 40400 RVA: 0x0038B0B8 File Offset: 0x003892B8
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
		}

		// Token: 0x06009DD1 RID: 40401 RVA: 0x0038B0CC File Offset: 0x003892CC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.parent.Faction != Faction.OfPlayer && !respawningAfterLoad)
			{
				this.active = false;
			}
			if (this.Active)
			{
				this.parent.Map.gatherSpotLister.RegisterActivated(this);
			}
		}

		// Token: 0x06009DD2 RID: 40402 RVA: 0x0038B11A File Offset: 0x0038931A
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			if (this.Active)
			{
				map.gatherSpotLister.RegisterDeactivated(this);
			}
		}

		// Token: 0x06009DD3 RID: 40403 RVA: 0x0038B138 File Offset: 0x00389338
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompGatherSpot.<CompGetGizmosExtra>d__7 <CompGetGizmosExtra>d__ = new CompGatherSpot.<CompGetGizmosExtra>d__7(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005D6A RID: 23914
		private bool active = true;
	}
}
