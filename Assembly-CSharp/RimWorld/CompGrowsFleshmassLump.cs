using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B0 RID: 6320
	public class CompGrowsFleshmassLump : ThingComp
	{
		// Token: 0x17001B25 RID: 6949
		// (get) Token: 0x06009E54 RID: 40532 RVA: 0x0038D2D3 File Offset: 0x0038B4D3
		private CompProperties_GrowsFleshmassLump Props
		{
			get
			{
				return (CompProperties_GrowsFleshmassLump)this.props;
			}
		}

		// Token: 0x06009E55 RID: 40533 RVA: 0x0038D2E0 File Offset: 0x0038B4E0
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lumpCellIndex, "lumpCellIndex", 0, false);
			Scribe_Values.Look<int>(ref this.nextSpawnTick, "nextSpawnTick", 0, false);
			Scribe_Collections.Look<IntVec3>(ref this.lumpCells, "lumpCells", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06009E56 RID: 40534 RVA: 0x0038D31C File Offset: 0x0038B51C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.nextSpawnTick = Find.TickManager.TicksGame + this.Props.ticksBetweenFleshmassRange.RandomInRange;
				this.lumpCells = GridShapeMaker.IrregularLump(this.parent.Position, this.parent.Map, this.Props.maxFleshmassRange.RandomInRange, null).ToList<IntVec3>();
			}
		}

		// Token: 0x06009E57 RID: 40535 RVA: 0x0038D384 File Offset: 0x0038B584
		public override void CompTick()
		{
			if (this.lumpCellIndex < this.lumpCells.Count - 1 && Find.TickManager.TicksGame >= this.nextSpawnTick)
			{
				IntVec3 intVec = this.lumpCells[this.lumpCellIndex];
				if (intVec.Standable(this.parent.Map))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Fleshmass_Active, null), intVec, this.parent.Map, WipeMode.FullRefund).SetFaction(this.parent.Faction, null);
					EffecterDefOf.MeatExplosion.Spawn(intVec, this.parent.Map, 1f).Cleanup();
				}
				this.nextSpawnTick = Find.TickManager.TicksGame + this.Props.ticksBetweenFleshmassRange.RandomInRange;
				this.lumpCellIndex++;
			}
		}

		// Token: 0x04005DAB RID: 23979
		private int nextSpawnTick;

		// Token: 0x04005DAC RID: 23980
		private List<IntVec3> lumpCells;

		// Token: 0x04005DAD RID: 23981
		private int lumpCellIndex;
	}
}
