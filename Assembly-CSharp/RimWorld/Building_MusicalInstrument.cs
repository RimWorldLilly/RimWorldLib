using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AC1 RID: 6849
	public class Building_MusicalInstrument : Building
	{
		// Token: 0x0600AE38 RID: 44600 RVA: 0x003D6543 File Offset: 0x003D4743
		public static bool IsAffectedByInstrument(ThingDef instrumentDef, IntVec3 instrumentPos, IntVec3 pawnPos, Map map)
		{
			return instrumentPos.DistanceTo(pawnPos) < instrumentDef.building.instrumentRange && instrumentPos.GetRoom(map) == pawnPos.GetRoom(map);
		}

		// Token: 0x17001F47 RID: 8007
		// (get) Token: 0x0600AE39 RID: 44601 RVA: 0x003D656B File Offset: 0x003D476B
		public bool IsBeingPlayed
		{
			get
			{
				return this.currentPlayer != null;
			}
		}

		// Token: 0x17001F48 RID: 8008
		// (get) Token: 0x0600AE3A RID: 44602 RVA: 0x003D6578 File Offset: 0x003D4778
		public FloatRange SoundRange
		{
			get
			{
				if (this.soundPlaying == null)
				{
					return FloatRange.Zero;
				}
				if (this.soundPlaying.def.subSounds.NullOrEmpty<SubSoundDef>())
				{
					return FloatRange.Zero;
				}
				return this.soundPlaying.def.subSounds.First<SubSoundDef>().distRange;
			}
		}

		// Token: 0x0600AE3B RID: 44603 RVA: 0x003D65CA File Offset: 0x003D47CA
		public void StartPlaying(Pawn player)
		{
			if (!ModLister.CheckRoyaltyOrIdeology("Musical instrument"))
			{
				return;
			}
			this.currentPlayer = player;
		}

		// Token: 0x0600AE3C RID: 44604 RVA: 0x003D65E0 File Offset: 0x003D47E0
		protected override void Tick()
		{
			base.Tick();
			if (this.currentPlayer != null)
			{
				if (this.def.soundPlayInstrument != null && this.soundPlaying == null)
				{
					this.soundPlaying = this.def.soundPlayInstrument.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.PerTick));
				}
			}
			else
			{
				this.soundPlaying = null;
			}
			Sustainer sustainer = this.soundPlaying;
			if (sustainer == null)
			{
				return;
			}
			sustainer.Maintain();
		}

		// Token: 0x0600AE3D RID: 44605 RVA: 0x003D6657 File Offset: 0x003D4857
		public void StopPlaying()
		{
			this.currentPlayer = null;
		}

		// Token: 0x0600AE3E RID: 44606 RVA: 0x003D6660 File Offset: 0x003D4860
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.currentPlayer, "currentPlayer", false);
		}

		// Token: 0x0600AE3F RID: 44607 RVA: 0x003D6679 File Offset: 0x003D4879
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_MusicalInstrument.<GetGizmos>d__11 <GetGizmos>d__ = new Building_MusicalInstrument.<GetGizmos>d__11(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x040064DF RID: 25823
		private Pawn currentPlayer;

		// Token: 0x040064E0 RID: 25824
		private Sustainer soundPlaying;
	}
}
