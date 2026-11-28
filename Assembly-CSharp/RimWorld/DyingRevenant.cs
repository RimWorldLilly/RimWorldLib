using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AED RID: 6893
	public class DyingRevenant : Thing
	{
		// Token: 0x0600B00E RID: 45070 RVA: 0x003E0A07 File Offset: 0x003DEC07
		protected override void Tick()
		{
			if (Find.TickManager.TicksGame >= this.completeTick)
			{
				this.Complete();
			}
		}

		// Token: 0x0600B00F RID: 45071 RVA: 0x003E0A24 File Offset: 0x003DEC24
		public void InitWith(Pawn revenant)
		{
			this.bioSignature = revenant.TryGetComp<CompRevenant>().biosignature;
			Effecter effecter = EffecterDefOf.RevenantDeath.SpawnMaintained(base.Position, base.Map, 1f);
			this.completeTick = base.TickSpawned + effecter.ticksLeft;
		}

		// Token: 0x0600B010 RID: 45072 RVA: 0x003E0A74 File Offset: 0x003DEC74
		public void Complete()
		{
			if (FilthMaker.TryMakeFilth(base.PositionHeld, base.Map, ThingDefOf.Filth_RevenantBloodPool, 1, FilthSourceFlags.None, true))
			{
				EffecterDefOf.RevenantKilledCompleteBurst.SpawnMaintained(base.PositionHeld, base.Map, 1f);
				foreach (IntVec3 c in CellRect.CenteredOn(base.PositionHeld, 2))
				{
					Plant plant = c.GetPlant(base.Map);
					if (plant != null && plant.MaxHitPoints < 100)
					{
						plant.Destroy(DestroyMode.Vanish);
					}
				}
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.RevenantSpine, null);
			thing.TryGetComp<CompBiosignatureOwner>().biosignature = this.bioSignature;
			GenSpawn.Spawn(thing, base.PositionHeld, base.Map, WipeMode.Vanish);
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600B011 RID: 45073 RVA: 0x003E0B58 File Offset: 0x003DED58
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.completeTick, "completeTick", 0, false);
			Scribe_Values.Look<int>(ref this.bioSignature, "bioSignature", 0, false);
		}

		// Token: 0x040065A2 RID: 26018
		private int bioSignature;

		// Token: 0x040065A3 RID: 26019
		private int completeTick;
	}
}
