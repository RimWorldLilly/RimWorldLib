using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001931 RID: 6449
	public class CompPlantDamager : ThingComp
	{
		// Token: 0x17001BF5 RID: 7157
		// (get) Token: 0x0600A1CC RID: 41420 RVA: 0x0039D8A5 File Offset: 0x0039BAA5
		public CompProperties_PlantDamager Props
		{
			get
			{
				return (CompProperties_PlantDamager)this.props;
			}
		}

		// Token: 0x0600A1CD RID: 41421 RVA: 0x0039D8B2 File Offset: 0x0039BAB2
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.ticksToPlantHarm = this.Props.ticksBetweenDamage;
		}

		// Token: 0x0600A1CE RID: 41422 RVA: 0x0039D8CC File Offset: 0x0039BACC
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.queuedCycles, "queuedCycles", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		// Token: 0x0600A1CF RID: 41423 RVA: 0x0039D8F2 File Offset: 0x0039BAF2
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				return;
			}
			this.queuedCycles = this.Props.cycleCountOnSpawn;
		}

		// Token: 0x0600A1D0 RID: 41424 RVA: 0x0039D90C File Offset: 0x0039BB0C
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			while (this.queuedCycles > 0)
			{
				this.DamageCycle();
				this.queuedCycles--;
			}
			this.ticksToPlantHarm--;
			if (this.ticksToPlantHarm <= 0)
			{
				this.ticksToPlantHarm = this.Props.ticksBetweenDamage;
				this.DamageCycle();
			}
		}

		// Token: 0x0600A1D1 RID: 41425 RVA: 0x0039D974 File Offset: 0x0039BB74
		public void DamageCycle()
		{
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(this.parent.Position, this.Props.radius, true))
			{
				if (intVec.InBounds(this.parent.Map))
				{
					float distance = intVec.DistanceTo(this.parent.Position);
					float amount = this.ComputeDamage(distance);
					Plant plant = intVec.GetPlant(this.parent.Map);
					if (plant != null && (!this.Props.ignoreAnima || (plant.def != ThingDefOf.Plant_GrassAnima && plant.def != ThingDefOf.Plant_TreeAnima)))
					{
						plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, amount, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					}
				}
			}
		}

		// Token: 0x0600A1D2 RID: 41426 RVA: 0x0039DA70 File Offset: 0x0039BC70
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompPlantDamager.<CompGetGizmosExtra>d__10 <CompGetGizmosExtra>d__ = new CompPlantDamager.<CompGetGizmosExtra>d__10(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A1D3 RID: 41427 RVA: 0x0039DA80 File Offset: 0x0039BC80
		private float ComputeDamage(float distance)
		{
			return this.Props.damagePerCycle * CompPlantDamager.DamageMultiplierOverDistance.Evaluate(distance / this.Props.radius);
		}

		// Token: 0x04005F97 RID: 24471
		private int ticksToPlantHarm;

		// Token: 0x04005F98 RID: 24472
		private int queuedCycles;

		// Token: 0x04005F99 RID: 24473
		private static readonly SimpleCurve DamageMultiplierOverDistance = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.2f),
				true
			}
		};
	}
}
