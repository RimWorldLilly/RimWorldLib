using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000380 RID: 896
	public class Pawn_DrawTracker
	{
		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001B44 RID: 6980 RVA: 0x000A8564 File Offset: 0x000A6764
		public Vector3 DrawPos
		{
			get
			{
				this.tweener.PreDrawPosCalculation();
				return (this.tweener.TweenedPos + this.jitterer.CurrentOffset + this.leaner.LeanOffset + this.OffsetForcedByJob() + this.FlyingOffset()).WithY(this.pawn.def.Altitude + this.FlightYOffset() + this.SeededYOffset);
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001B45 RID: 6981 RVA: 0x000A85E0 File Offset: 0x000A67E0
		public float SeededYOffset { get; }

		// Token: 0x06001B46 RID: 6982 RVA: 0x000A85E8 File Offset: 0x000A67E8
		public Pawn_DrawTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.tweener = new PawnTweener(pawn);
			this.jitterer = new JitterHandler();
			this.leaner = new PawnLeaner(pawn);
			this.renderer = new PawnRenderer(pawn);
			this.ui = new PawnUIOverlay(pawn);
			this.footprintMaker = new PawnFootprintMaker(pawn);
			this.breathMoteMaker = new PawnBreathMoteMaker(pawn);
			this.waterRippleMaker = new PawnWaterRippleMaker(pawn);
			this.SeededYOffset = Rand.RangeSeeded(-0.03658537f, 0.03658537f, pawn.thingIDNumber);
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x000A867C File Offset: 0x000A687C
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			this.jitterer.ProcessPostTickVisuals(ticksPassed);
			this.footprintMaker.ProcessPostTickVisuals(ticksPassed);
			this.breathMoteMaker.ProcessPostTickVisuals(ticksPassed);
			this.waterRippleMaker.ProcessPostTickVisuals(ticksPassed);
			this.leaner.ProcessPostTickVisuals(ticksPassed);
			this.renderer.ProcessPostTickVisuals(ticksPassed);
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x000A86E0 File Offset: 0x000A68E0
		public void DrawShadowAt(Vector3 loc)
		{
			using (new ProfilerBlock("Draw Shadow At()"))
			{
				this.renderer.RenderShadowOnlyAt(loc);
			}
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x000A8728 File Offset: 0x000A6928
		private Vector3 OffsetForcedByJob()
		{
			Pawn_JobTracker jobs = this.pawn.jobs;
			if (((jobs != null) ? jobs.curDriver : null) != null)
			{
				return this.pawn.jobs.curDriver.ForcedBodyOffset;
			}
			return Vector3.zero;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x000A875E File Offset: 0x000A695E
		private Vector3 FlyingOffset()
		{
			if (this.pawn.flight == null)
			{
				return Vector3.zero;
			}
			return new Vector3(0f, 0f, 0.6f) * this.pawn.flight.PositionOffsetFactor;
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x000A879C File Offset: 0x000A699C
		private float FlightYOffset()
		{
			if (this.pawn.flight == null)
			{
				return 0f;
			}
			return 0.03658537f * this.pawn.flight.PositionOffsetFactor;
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x000A87C7 File Offset: 0x000A69C7
		public void Notify_Spawned()
		{
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x000A87D4 File Offset: 0x000A69D4
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.leaner.Notify_WarmingCastAlongLine(newShootLine, ShootPosition);
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x000A87E3 File Offset: 0x000A69E3
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed || !this.pawn.Spawned)
			{
				return;
			}
			this.jitterer.Notify_DamageApplied(dinfo);
			this.renderer.Notify_DamageApplied(dinfo);
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x000A8818 File Offset: 0x000A6A18
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed)
			{
				return;
			}
			this.jitterer.Notify_DamageDeflected(dinfo);
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x000A8834 File Offset: 0x000A6A34
		public void Notify_MeleeAttackOn(Thing Target)
		{
			if (Target.Position != this.pawn.Position)
			{
				this.jitterer.AddOffset(0.5f, (Target.Position - this.pawn.Position).AngleFlat);
				return;
			}
			if (Target.DrawPos != this.pawn.DrawPos)
			{
				this.jitterer.AddOffset(0.25f, (Target.DrawPos - this.pawn.DrawPos).AngleFlat());
			}
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x000A88CC File Offset: 0x000A6ACC
		public void Notify_DebugAffected()
		{
			for (int i = 0; i < 10; i++)
			{
				FleckMaker.ThrowAirPuffUp(this.pawn.DrawPosHeld.Value, this.pawn.MapHeld);
			}
			this.jitterer.AddOffset(0.05f, (float)Rand.Range(0, 360));
		}

		// Token: 0x0400166C RID: 5740
		private Pawn pawn;

		// Token: 0x0400166D RID: 5741
		public PawnTweener tweener;

		// Token: 0x0400166E RID: 5742
		private JitterHandler jitterer;

		// Token: 0x0400166F RID: 5743
		public PawnLeaner leaner;

		// Token: 0x04001670 RID: 5744
		public PawnRenderer renderer;

		// Token: 0x04001671 RID: 5745
		public PawnUIOverlay ui;

		// Token: 0x04001672 RID: 5746
		private PawnFootprintMaker footprintMaker;

		// Token: 0x04001673 RID: 5747
		private PawnBreathMoteMaker breathMoteMaker;

		// Token: 0x04001674 RID: 5748
		private PawnWaterRippleMaker waterRippleMaker;

		// Token: 0x04001675 RID: 5749
		private const float MeleeJitterDistance = 0.5f;
	}
}
