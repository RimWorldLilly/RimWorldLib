using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001933 RID: 6451
	public class CompPlantHarmRadius : ThingComp
	{
		// Token: 0x17001BF6 RID: 7158
		// (get) Token: 0x0600A1D7 RID: 41431 RVA: 0x0039DB1A File Offset: 0x0039BD1A
		protected CompProperties_PlantHarmRadius Props
		{
			get
			{
				return (CompProperties_PlantHarmRadius)this.props;
			}
		}

		// Token: 0x17001BF7 RID: 7159
		// (get) Token: 0x0600A1D8 RID: 41432 RVA: 0x0039DB27 File Offset: 0x0039BD27
		public float AgeDays
		{
			get
			{
				return (float)this.plantHarmAge / 60000f;
			}
		}

		// Token: 0x17001BF8 RID: 7160
		// (get) Token: 0x0600A1D9 RID: 41433 RVA: 0x0039DB36 File Offset: 0x0039BD36
		public float CurrentRadius
		{
			get
			{
				return this.Props.radiusPerDayCurve.Evaluate(this.AgeDays);
			}
		}

		// Token: 0x0600A1DA RID: 41434 RVA: 0x0039DB4E File Offset: 0x0039BD4E
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.plantHarmAge, "plantHarmAge", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		// Token: 0x0600A1DB RID: 41435 RVA: 0x0039DB74 File Offset: 0x0039BD74
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostPostMake();
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		// Token: 0x0600A1DC RID: 41436 RVA: 0x0039DB90 File Offset: 0x0039BD90
		public override string CompInspectStringExtra()
		{
			return "FoliageKillRadius".Translate() + ": " + this.CurrentRadius.ToString("0.0") + " (+" + "PerDay".Translate((this.Props.radiusPerDayCurve.Evaluate(this.AgeDays + 1f) - this.Props.radiusPerDayCurve.Evaluate(this.AgeDays)).ToString("0.0")) + ")";
		}

		// Token: 0x0600A1DD RID: 41437 RVA: 0x0039DC38 File Offset: 0x0039BE38
		public override void CompTick()
		{
			if (!this.parent.Spawned || (this.initiatableComp != null && !this.initiatableComp.Initiated))
			{
				return;
			}
			this.plantHarmAge++;
			this.ticksToPlantHarm--;
			if (this.ticksToPlantHarm <= 0)
			{
				float currentRadius = this.CurrentRadius;
				float num = 3.1415927f * currentRadius * currentRadius * this.Props.harmFrequencyPerArea;
				float num2 = 60f / num;
				int num3;
				if (num2 >= 1f)
				{
					this.ticksToPlantHarm = GenMath.RoundRandom(num2);
					num3 = 1;
				}
				else
				{
					this.ticksToPlantHarm = 1;
					num3 = GenMath.RoundRandom(1f / num2);
				}
				for (int i = 0; i < num3; i++)
				{
					this.HarmRandomPlantInRadius(currentRadius);
				}
			}
		}

		// Token: 0x0600A1DE RID: 41438 RVA: 0x0039DCF8 File Offset: 0x0039BEF8
		private void HarmRandomPlantInRadius(float radius)
		{
			IntVec3 c = this.parent.Position + (Rand.InsideUnitCircleVec3 * radius).ToIntVec3();
			if (!c.InBounds(this.parent.Map))
			{
				return;
			}
			Plant plant = c.GetPlant(this.parent.Map);
			if (plant != null)
			{
				if (this.Props.ignoreSpecialTrees && plant.def.plant.treeCategory == TreeCategory.Super)
				{
					return;
				}
				if (plant.LeaflessNow)
				{
					if (Rand.Value < this.Props.leaflessPlantKillChance)
					{
						plant.Kill(null, null);
						return;
					}
				}
				else
				{
					plant.MakeLeafless(Plant.LeaflessCause.Poison, this.Props.messageOnCropDeath);
				}
			}
		}

		// Token: 0x04005F9F RID: 24479
		private int plantHarmAge;

		// Token: 0x04005FA0 RID: 24480
		private int ticksToPlantHarm;

		// Token: 0x04005FA1 RID: 24481
		protected CompInitiatable initiatableComp;
	}
}
