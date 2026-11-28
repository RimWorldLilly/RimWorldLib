using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A97 RID: 6807
	public class Building_AncientMechRemains : Building
	{
		// Token: 0x0600ABB3 RID: 43955 RVA: 0x003CA076 File Offset: 0x003C8276
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			Find.History.Notify_MechanoidDatacoreOppurtunityAvailable();
		}

		// Token: 0x0600ABB4 RID: 43956 RVA: 0x003CA08C File Offset: 0x003C828C
		protected override void Tick()
		{
			base.Tick();
			if (this.effecter == null)
			{
				this.effecter = EffecterDefOf.AncientExostriderRemainsPulse.SpawnAttached(this, base.Map, 1f);
			}
			Effecter effecter = this.effecter;
			if (effecter == null)
			{
				return;
			}
			effecter.EffectTick(this, this);
		}

		// Token: 0x0600ABB5 RID: 43957 RVA: 0x003CA0DF File Offset: 0x003C82DF
		public override void Notify_AbandonedAtTile(PlanetTile tile)
		{
			base.Notify_AbandonedAtTile(tile);
			Find.History.Notify_MechanoidDatacoreReadOrLost();
		}

		// Token: 0x040063F0 RID: 25584
		private Effecter effecter;
	}
}
