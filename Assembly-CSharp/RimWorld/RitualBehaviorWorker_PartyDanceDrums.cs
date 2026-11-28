using System;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001685 RID: 5765
	public class RitualBehaviorWorker_PartyDanceDrums : RitualBehaviorWorker
	{
		// Token: 0x0600908F RID: 37007 RVA: 0x00350D61 File Offset: 0x0034EF61
		public RitualBehaviorWorker_PartyDanceDrums()
		{
		}

		// Token: 0x06009090 RID: 37008 RVA: 0x00350D70 File Offset: 0x0034EF70
		public RitualBehaviorWorker_PartyDanceDrums(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x06009091 RID: 37009 RVA: 0x00350D80 File Offset: 0x0034EF80
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_PartyDanceDrums(target, ritual, obligation, this.def.stages, assignments, organizer);
		}

		// Token: 0x170018F8 RID: 6392
		// (get) Token: 0x06009092 RID: 37010 RVA: 0x00350D99 File Offset: 0x0034EF99
		public override Sustainer SoundPlaying
		{
			get
			{
				return this.soundPlaying;
			}
		}

		// Token: 0x170018F9 RID: 6393
		// (get) Token: 0x06009093 RID: 37011 RVA: 0x00350DA1 File Offset: 0x0034EFA1
		public override bool ChecksReservations
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009094 RID: 37012 RVA: 0x00350DA4 File Offset: 0x0034EFA4
		public override void Tick(LordJob_Ritual ritual)
		{
			base.Tick(ritual);
			TargetInfo selectedTarget = ritual.selectedTarget;
			if (Find.TickManager.TicksGame % 20 == 0 || this.numActiveDrums == -1)
			{
				this.numActiveDrums = 0;
				foreach (Thing thing in selectedTarget.Map.listerBuldingOfDefInProximity.GetForCell(selectedTarget.Cell, (float)this.def.maxEnhancerDistance, ThingDefOf.Drum, null))
				{
					Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
					if (thing.GetRoom(RegionType.Set_All) == selectedTarget.Thing.GetRoom(RegionType.Set_All) && building_MusicalInstrument != null && building_MusicalInstrument.IsBeingPlayed)
					{
						this.numActiveDrums++;
						this.anyDrumReached = true;
					}
				}
			}
			bool flag = this.numActiveDrums > 0 || this.anyDrumReached;
			SoundDef soundDef = (!this.def.soundDefsPerEnhancerCount.NullOrEmpty<SoundDef>() && flag) ? this.def.soundDefsPerEnhancerCount[Math.Min(this.numActiveDrums, this.def.soundDefsPerEnhancerCount.Count - 1)] : null;
			if (soundDef != null && (this.soundPlaying == null || this.soundPlaying.def != soundDef))
			{
				this.soundPlaying = soundDef.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
			}
			if (flag)
			{
				Sustainer sustainer = this.soundPlaying;
				if (sustainer == null)
				{
					return;
				}
				sustainer.Maintain();
			}
		}

		// Token: 0x04005834 RID: 22580
		private Sustainer soundPlaying;

		// Token: 0x04005835 RID: 22581
		private int numActiveDrums = -1;

		// Token: 0x04005836 RID: 22582
		private bool anyDrumReached;
	}
}
