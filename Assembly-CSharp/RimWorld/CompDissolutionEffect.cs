using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020019DF RID: 6623
	public abstract class CompDissolutionEffect : ThingComp
	{
		// Token: 0x0600A6D1 RID: 42705 RVA: 0x003B71FD File Offset: 0x003B53FD
		public virtual void DoDissolutionEffectMap(int amount)
		{
		}

		// Token: 0x0600A6D2 RID: 42706 RVA: 0x003B71FF File Offset: 0x003B53FF
		public virtual void DoDissolutionEffectWorld(int amount, PlanetTile tile)
		{
		}

		// Token: 0x0600A6D3 RID: 42707 RVA: 0x003B7201 File Offset: 0x003B5401
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Dissolution effect"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}
	}
}
