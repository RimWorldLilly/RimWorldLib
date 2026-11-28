using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B4 RID: 6580
	public abstract class CompTerrainPump : ThingComp
	{
		// Token: 0x17001CC0 RID: 7360
		// (get) Token: 0x0600A52D RID: 42285 RVA: 0x003AF345 File Offset: 0x003AD545
		private CompProperties_TerrainPump Props
		{
			get
			{
				return (CompProperties_TerrainPump)this.props;
			}
		}

		// Token: 0x17001CC1 RID: 7361
		// (get) Token: 0x0600A52E RID: 42286 RVA: 0x003AF352 File Offset: 0x003AD552
		private float ProgressDays
		{
			get
			{
				return (float)this.progressTicks / 60000f;
			}
		}

		// Token: 0x17001CC2 RID: 7362
		// (get) Token: 0x0600A52F RID: 42287 RVA: 0x003AF361 File Offset: 0x003AD561
		protected float CurrentRadius
		{
			get
			{
				return Mathf.Min(this.Props.radius, this.ProgressDays / this.Props.daysToRadius * this.Props.radius);
			}
		}

		// Token: 0x17001CC3 RID: 7363
		// (get) Token: 0x0600A530 RID: 42288 RVA: 0x003AF391 File Offset: 0x003AD591
		protected bool Working
		{
			get
			{
				return this.powerComp == null || this.powerComp.PowerOn;
			}
		}

		// Token: 0x17001CC4 RID: 7364
		// (get) Token: 0x0600A531 RID: 42289 RVA: 0x003AF3A8 File Offset: 0x003AD5A8
		private int TicksUntilRadiusInteger
		{
			get
			{
				float num = Mathf.Ceil(this.CurrentRadius) - this.CurrentRadius;
				if (num < 1E-05f)
				{
					num = 1f;
				}
				float num2 = this.Props.radius / this.Props.daysToRadius;
				return (int)(num / num2 * 60000f);
			}
		}

		// Token: 0x0600A532 RID: 42290 RVA: 0x003AF3F8 File Offset: 0x003AD5F8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x0600A533 RID: 42291 RVA: 0x003AF40B File Offset: 0x003AD60B
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			this.progressTicks = 0;
		}

		// Token: 0x0600A534 RID: 42292 RVA: 0x003AF414 File Offset: 0x003AD614
		public override void CompTickRare()
		{
			if (this.Working)
			{
				this.progressTicks += 250;
				int num = GenRadial.NumCellsInRadius(this.CurrentRadius);
				for (int i = 0; i < num; i++)
				{
					this.AffectCell(this.parent.Position + GenRadial.RadialPattern[i]);
				}
			}
		}

		// Token: 0x0600A535 RID: 42293
		protected abstract void AffectCell(IntVec3 c);

		// Token: 0x0600A536 RID: 42294 RVA: 0x003AF474 File Offset: 0x003AD674
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.progressTicks, "progressTicks", 0, false);
		}

		// Token: 0x0600A537 RID: 42295 RVA: 0x003AF488 File Offset: 0x003AD688
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.CurrentRadius < this.Props.radius - 0.0001f)
			{
				GenDraw.DrawRadiusRing(this.parent.Position, this.CurrentRadius);
			}
		}

		// Token: 0x0600A538 RID: 42296 RVA: 0x003AF4BC File Offset: 0x003AD6BC
		public override string CompInspectStringExtra()
		{
			string text = "TimePassed".Translate().CapitalizeFirst() + ": " + this.progressTicks.ToStringTicksToPeriod(true, false, true, true, false) + "\n" + "CurrentRadius".Translate().CapitalizeFirst() + ": " + this.CurrentRadius.ToString("F1");
			if (this.ProgressDays < this.Props.daysToRadius && this.Working)
			{
				text += "\n" + "RadiusExpandsIn".Translate().CapitalizeFirst() + ": " + this.TicksUntilRadiusInteger.ToStringTicksToPeriod(true, false, true, true, false);
			}
			return text;
		}

		// Token: 0x0600A539 RID: 42297 RVA: 0x003AF5A6 File Offset: 0x003AD7A6
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompTerrainPump.<CompGetGizmosExtra>d__19 <CompGetGizmosExtra>d__ = new CompTerrainPump.<CompGetGizmosExtra>d__19(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0400618C RID: 24972
		private CompPowerTrader powerComp;

		// Token: 0x0400618D RID: 24973
		private int progressTicks;
	}
}
