using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016DD RID: 5853
	public class RitualOutcomeEffectWorker_RemoveConsumableBuilding : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x17001928 RID: 6440
		// (get) Token: 0x06009274 RID: 37492 RVA: 0x00357910 File Offset: 0x00355B10
		public override bool ApplyOnFailure
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001929 RID: 6441
		// (get) Token: 0x06009275 RID: 37493 RVA: 0x00357913 File Offset: 0x00355B13
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return this.def.allowAttachableOutcome;
			}
		}

		// Token: 0x06009276 RID: 37494 RVA: 0x00357920 File Offset: 0x00355B20
		public RitualOutcomeEffectWorker_RemoveConsumableBuilding()
		{
		}

		// Token: 0x06009277 RID: 37495 RVA: 0x00357928 File Offset: 0x00355B28
		public RitualOutcomeEffectWorker_RemoveConsumableBuilding(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06009278 RID: 37496 RVA: 0x00357934 File Offset: 0x00355B34
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			if (!jobRitual.cancelled)
			{
				base.Apply(progress, totalPresence, jobRitual);
			}
			if (jobRitual.selectedTarget.HasThing)
			{
				Thing thing = jobRitual.selectedTarget.Thing;
				if (this.def.effecter != null)
				{
					this.def.effecter.Spawn(thing, jobRitual.selectedTarget.Map, 1f).Cleanup();
				}
				if (this.def.fleckDef != null)
				{
					CellRect cellRect = thing.OccupiedRect();
					for (int i = 0; i < cellRect.Area * this.def.flecksPerCell; i++)
					{
						FleckCreationData dataStatic = FleckMaker.GetDataStatic(cellRect.RandomVector3, thing.Map, this.def.fleckDef, this.def.fleckScaleRange.RandomInRange);
						dataStatic.rotation = this.def.fleckRotationRange.RandomInRange;
						dataStatic.velocityAngle = this.def.fleckVelocityAngle.RandomInRange;
						dataStatic.velocitySpeed = this.def.fleckVelocitySpeed.RandomInRange;
						thing.Map.flecks.CreateFleck(dataStatic);
					}
				}
				if (this.def.filthDefToSpawn != null)
				{
					foreach (IntVec3 c in thing.OccupiedRect())
					{
						FilthMaker.TryMakeFilth(c, thing.Map, this.def.filthDefToSpawn, this.def.filthCountToSpawn.RandomInRange, FilthSourceFlags.None, true);
					}
				}
				thing.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
