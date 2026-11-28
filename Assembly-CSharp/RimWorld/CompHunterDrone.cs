using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020018C3 RID: 6339
	public class CompHunterDrone : ThingComp
	{
		// Token: 0x17001B45 RID: 6981
		// (get) Token: 0x06009EEA RID: 40682 RVA: 0x003905AD File Offset: 0x0038E7AD
		private CompProperties_HunterDrone Props
		{
			get
			{
				return (CompProperties_HunterDrone)this.props;
			}
		}

		// Token: 0x06009EEB RID: 40683 RVA: 0x003905BA File Offset: 0x0038E7BA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.wickStarted, "wickStarted", false, false);
			Scribe_Values.Look<int>(ref this.wickTicks, "wickTicks", 0, false);
		}

		// Token: 0x06009EEC RID: 40684 RVA: 0x003905E8 File Offset: 0x0038E7E8
		public override void CompTickInterval(int delta)
		{
			if (this.wickStarted)
			{
				return;
			}
			if (!this.parent.IsHashIntervalTick(30, delta))
			{
				return;
			}
			Pawn pawn = this.parent as Pawn;
			if (pawn == null)
			{
				return;
			}
			if (pawn.Spawned && !pawn.Downed && PawnUtility.EnemiesAreNearby(pawn, 9, true, 1.5f, 1, false))
			{
				this.StartWick();
			}
		}

		// Token: 0x06009EED RID: 40685 RVA: 0x00390647 File Offset: 0x0038E847
		public override void CompTick()
		{
			if (!this.wickStarted)
			{
				return;
			}
			this.wickSoundSustainer.Maintain();
			this.wickTicks--;
			if (this.wickTicks <= 0)
			{
				this.Detonate(null);
			}
		}

		// Token: 0x06009EEE RID: 40686 RVA: 0x0039067C File Offset: 0x0038E87C
		private void StartWick()
		{
			if (this.wickStarted)
			{
				return;
			}
			this.wickStarted = true;
			this.overlayBurningWick = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.BurningWick));
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
			this.wickTicks = 120;
		}

		// Token: 0x06009EEF RID: 40687 RVA: 0x003906E9 File Offset: 0x0038E8E9
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			if (dinfo == null)
			{
				return;
			}
			this.Detonate(prevMap);
		}

		// Token: 0x06009EF0 RID: 40688 RVA: 0x003906FC File Offset: 0x0038E8FC
		private void Detonate(Map map = null)
		{
			IntVec3 position = this.parent.Position;
			if (map == null)
			{
				map = this.parent.Map;
			}
			if (!this.parent.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
			GenExplosion.DoExplosion(position, map, this.Props.explosionRadius, this.Props.explosionDamageType, this.parent, this.Props.explosionDamageAmount, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			Corpse corpse = base.ParentHolder as Corpse;
			if (corpse != null)
			{
				corpse.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04005E0D RID: 24077
		private bool wickStarted;

		// Token: 0x04005E0E RID: 24078
		private int wickTicks;

		// Token: 0x04005E0F RID: 24079
		[Unsaved(false)]
		private Sustainer wickSoundSustainer;

		// Token: 0x04005E10 RID: 24080
		[Unsaved(false)]
		private OverlayHandle? overlayBurningWick;
	}
}
