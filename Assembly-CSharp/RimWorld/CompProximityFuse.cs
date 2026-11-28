using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001942 RID: 6466
	public class CompProximityFuse : ThingComp
	{
		// Token: 0x17001C10 RID: 7184
		// (get) Token: 0x0600A23F RID: 41535 RVA: 0x0039FF45 File Offset: 0x0039E145
		public CompProperties_ProximityFuse Props
		{
			get
			{
				return (CompProperties_ProximityFuse)this.props;
			}
		}

		// Token: 0x0600A240 RID: 41536 RVA: 0x0039FF52 File Offset: 0x0039E152
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
		}

		// Token: 0x0600A241 RID: 41537 RVA: 0x0039FF6C File Offset: 0x0039E16C
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompProximityFuse.<CompGetGizmosExtra>d__3 <CompGetGizmosExtra>d__ = new CompProximityFuse.<CompGetGizmosExtra>d__3(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A242 RID: 41538 RVA: 0x0039FF7C File Offset: 0x0039E17C
		public override void CompTickRare()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForDef(this.Props.target), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), this.Props.radius, null, null, 0, -1, false, RegionType.Set_Passable, false, false) != null)
			{
				this.parent.GetComp<CompExplosive>().StartWick(null);
			}
		}
	}
}
