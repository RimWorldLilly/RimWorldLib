using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AFD RID: 6909
	public class OrbitalStrike : ThingWithComps
	{
		// Token: 0x17001FDC RID: 8156
		// (get) Token: 0x0600B0B0 RID: 45232 RVA: 0x003E4562 File Offset: 0x003E2762
		protected int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17001FDD RID: 8157
		// (get) Token: 0x0600B0B1 RID: 45233 RVA: 0x003E4575 File Offset: 0x003E2775
		protected int TicksLeft
		{
			get
			{
				return this.duration - this.TicksPassed;
			}
		}

		// Token: 0x0600B0B2 RID: 45234 RVA: 0x003E4584 File Offset: 0x003E2784
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x0600B0B3 RID: 45235 RVA: 0x003E45F2 File Offset: 0x003E27F2
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.Comps_PostDraw();
		}

		// Token: 0x0600B0B4 RID: 45236 RVA: 0x003E45FC File Offset: 0x003E27FC
		public virtual void StartStrike()
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartStrike() on unspawned thing.");
				return;
			}
			this.angle = OrbitalStrike.AngleRange.RandomInRange;
			this.startTick = Find.TickManager.TicksGame;
			CompAffectsSky comp = base.GetComp<CompAffectsSky>();
			if (comp != null)
			{
				comp.StartFadeInHoldFadeOut(30, this.duration - 30 - 15, 15, 1f);
			}
			base.GetComp<CompOrbitalBeam>().StartAnimation(this.duration, 10, this.angle);
		}

		// Token: 0x0600B0B5 RID: 45237 RVA: 0x003E467D File Offset: 0x003E287D
		protected override void Tick()
		{
			base.Tick();
			if (this.TicksPassed >= this.duration)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x040065F2 RID: 26098
		public int duration;

		// Token: 0x040065F3 RID: 26099
		public Thing instigator;

		// Token: 0x040065F4 RID: 26100
		public ThingDef weaponDef;

		// Token: 0x040065F5 RID: 26101
		private float angle;

		// Token: 0x040065F6 RID: 26102
		private int startTick;

		// Token: 0x040065F7 RID: 26103
		private static readonly FloatRange AngleRange = new FloatRange(-12f, 12f);

		// Token: 0x040065F8 RID: 26104
		private const int SkyColorFadeInTicks = 30;

		// Token: 0x040065F9 RID: 26105
		private const int SkyColorFadeOutTicks = 15;

		// Token: 0x040065FA RID: 26106
		private const int OrbitalBeamFadeOutTicks = 10;
	}
}
