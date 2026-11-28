using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E0 RID: 6368
	public class CompFleckEmitterLongTerm : ThingComp
	{
		// Token: 0x17001B7D RID: 7037
		// (get) Token: 0x06009FA5 RID: 40869 RVA: 0x00393B8F File Offset: 0x00391D8F
		// (set) Token: 0x06009FA6 RID: 40870 RVA: 0x00393B97 File Offset: 0x00391D97
		public bool Enabled { get; set; }

		// Token: 0x17001B7E RID: 7038
		// (get) Token: 0x06009FA7 RID: 40871 RVA: 0x00393BA0 File Offset: 0x00391DA0
		private CompProperties_FleckEmitterLongTerm Props
		{
			get
			{
				return (CompProperties_FleckEmitterLongTerm)this.props;
			}
		}

		// Token: 0x17001B7F RID: 7039
		// (get) Token: 0x06009FA8 RID: 40872 RVA: 0x00393BAD File Offset: 0x00391DAD
		private bool ShouldEmit
		{
			get
			{
				return this.Enabled || this.Props.forceEnabled;
			}
		}

		// Token: 0x06009FA9 RID: 40873 RVA: 0x00393BC4 File Offset: 0x00391DC4
		private void EmissionTick(IFleckCreator fleckDestination)
		{
			if (this.ShouldEmit && Rand.Value < this.Props.spawnChance)
			{
				CompProperties_FleckEmitterLongTerm props = this.Props;
				Vector3 vector = this.parent.TrueCenter() + props.spawnOffsetFromCenter + Rand.InsideUnitCircleVec3 * props.spawnRadius;
				if (!vector.ToIntVec3().ShouldSpawnMotesAt(this.parent.Map, true))
				{
					return;
				}
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector, this.parent.Map, props.fleckDef, props.fleckScale);
				dataStatic.rotationRate = Rand.Range(props.minRotationRate, props.maxRotationRate);
				dataStatic.velocityAngle = Rand.Range(props.minVelocityAngle, props.maxVelocityAngle);
				dataStatic.velocitySpeed = Rand.Range(props.minVelocitySpeed, props.maxVelocitySpeed);
				if (fleckDestination is FleckSystem)
				{
					dataStatic.spawnPosition.y = dataStatic.def.altitudeLayer.AltitudeFor(dataStatic.def.altitudeLayerIncOffset);
				}
				fleckDestination.CreateFleck(dataStatic);
			}
		}

		// Token: 0x06009FAA RID: 40874 RVA: 0x00393CDC File Offset: 0x00391EDC
		public void Prewarm()
		{
			if (!this.ShouldEmit || this.Props.prewarmCycles <= 0f)
			{
				return;
			}
			FleckSystem system = this.parent.Map.flecks.CreateFleckSystemFor(this.Props.fleckDef);
			system.Prewarm(this.Props.fleckDef.Lifetime * this.Props.prewarmCycles, null, delegate
			{
				this.EmissionTick(system);
			});
			this.parent.Map.flecks.HandOverSystem(system);
		}

		// Token: 0x06009FAB RID: 40875 RVA: 0x00393D86 File Offset: 0x00391F86
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.Prewarm();
		}

		// Token: 0x06009FAC RID: 40876 RVA: 0x00393D8E File Offset: 0x00391F8E
		public override void CompTick()
		{
			this.EmissionTick(this.parent.Map.flecks);
		}
	}
}
