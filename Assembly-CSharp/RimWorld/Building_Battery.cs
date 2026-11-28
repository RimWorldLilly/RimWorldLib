using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A9B RID: 6811
	[StaticConstructorOnStartup]
	public class Building_Battery : Building
	{
		// Token: 0x0600ABC7 RID: 43975 RVA: 0x003CA45B File Offset: 0x003C865B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToExplode, "ticksToExplode", 0, false);
		}

		// Token: 0x0600ABC8 RID: 43976 RVA: 0x003CA478 File Offset: 0x003C8678
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			CompPowerBattery comp = base.GetComp<CompPowerBattery>();
			GenDraw.FillableBarRequest r = new GenDraw.FillableBarRequest
			{
				center = drawLoc + Vector3.up * 0.1f,
				size = Building_Battery.BarSize,
				fillPercent = comp.StoredEnergy / comp.Props.storedEnergyMax,
				filledMat = Building_Battery.BatteryBarFilledMat,
				unfilledMat = Building_Battery.BatteryBarUnfilledMat,
				margin = 0.15f
			};
			Rot4 rotation = base.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
			if (this.ticksToExplode > 0 && base.Spawned)
			{
				base.Map.overlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
			}
		}

		// Token: 0x0600ABC9 RID: 43977 RVA: 0x003CA544 File Offset: 0x003C8744
		protected override void Tick()
		{
			base.Tick();
			if (this.ticksToExplode <= 0)
			{
				return;
			}
			if (this.wickSustainer == null)
			{
				this.StartWickSustainer();
			}
			else
			{
				this.wickSustainer.Maintain();
			}
			this.ticksToExplode--;
			if (this.ticksToExplode == 0)
			{
				IntVec3 center = base.Spawned ? this.OccupiedRect().RandomCell : base.PositionHeld;
				float radius = Rand.Range(0.5f, 1f) * 3f;
				GenExplosion.DoExplosion(center, base.MapHeld, radius, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
				base.GetComp<CompPowerBattery>().DrawPower(400f);
			}
		}

		// Token: 0x0600ABCA RID: 43978 RVA: 0x003CA64C File Offset: 0x003C884C
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && this.ticksToExplode == 0 && dinfo.Def == DamageDefOf.Flame && Rand.Value < 0.05f && base.GetComp<CompPowerBattery>().StoredEnergy > 500f)
			{
				this.ticksToExplode = Rand.Range(70, 150);
				this.StartWickSustainer();
			}
		}

		// Token: 0x0600ABCB RID: 43979 RVA: 0x003CA6B8 File Offset: 0x003C88B8
		private void StartWickSustainer()
		{
			SoundInfo info = SoundInfo.InMap(base.SpawnedParentOrMe, MaintenanceType.PerTick);
			this.wickSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		// Token: 0x040063F8 RID: 25592
		private int ticksToExplode;

		// Token: 0x040063F9 RID: 25593
		private Sustainer wickSustainer;

		// Token: 0x040063FA RID: 25594
		private static readonly Vector2 BarSize = new Vector2(1.3f, 0.4f);

		// Token: 0x040063FB RID: 25595
		private const float MinEnergyToExplode = 500f;

		// Token: 0x040063FC RID: 25596
		private const float EnergyToLoseWhenExplode = 400f;

		// Token: 0x040063FD RID: 25597
		private const float ExplodeChancePerDamage = 0.05f;

		// Token: 0x040063FE RID: 25598
		private static readonly Material BatteryBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f), false);

		// Token: 0x040063FF RID: 25599
		private static readonly Material BatteryBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
