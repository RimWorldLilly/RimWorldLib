using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200044E RID: 1102
	public class HediffComp_Invisibility : HediffComp
	{
		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001F80 RID: 8064 RVA: 0x000BF6A4 File Offset: 0x000BD8A4
		private CompHoldingPlatformTarget PlatformTarget
		{
			get
			{
				CompHoldingPlatformTarget result;
				if ((result = this.platformTarget) == null)
				{
					result = (this.platformTarget = base.Pawn.TryGetComp<CompHoldingPlatformTarget>());
				}
				return result;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001F81 RID: 8065 RVA: 0x000BF6CF File Offset: 0x000BD8CF
		public HediffCompProperties_Invisibility Props
		{
			get
			{
				return (HediffCompProperties_Invisibility)this.props;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001F82 RID: 8066 RVA: 0x000BF6DC File Offset: 0x000BD8DC
		private float FadeIn
		{
			get
			{
				return Mathf.Clamp01((float)(Find.TickManager.TicksGame - base.Pawn.mindState.lastBecameVisibleTick) / (float)this.Props.fadeDurationTicks);
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001F83 RID: 8067 RVA: 0x000BF70C File Offset: 0x000BD90C
		private float FadeOut
		{
			get
			{
				return 1f - Mathf.Clamp01((float)(Find.TickManager.TicksGame - base.Pawn.mindState.lastBecameInvisibleTick) / (float)this.Props.fadeDurationTicks);
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001F84 RID: 8068 RVA: 0x000BF742 File Offset: 0x000BD942
		private bool ShouldBeVisible
		{
			get
			{
				return base.Pawn.mindState.lastBecameVisibleTick >= base.Pawn.mindState.lastBecameInvisibleTick;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001F85 RID: 8069 RVA: 0x000BF769 File Offset: 0x000BD969
		private float FadePct
		{
			get
			{
				if (!this.ShouldBeVisible)
				{
					return this.FadeOut;
				}
				return this.FadeIn;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001F86 RID: 8070 RVA: 0x000BF780 File Offset: 0x000BD980
		public bool PsychologicallyVisible
		{
			get
			{
				return this.ForcedVisible || this.FadePct > 0f;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x000BF79C File Offset: 0x000BD99C
		private bool ForcedVisible
		{
			get
			{
				if (base.Pawn.Downed)
				{
					return true;
				}
				if (Find.TickManager.TicksGame < this.lastDisrupted + this.Props.recoverFromDisruptedTicks)
				{
					return true;
				}
				if (base.Pawn.Faction == null || !base.Pawn.Faction.IsPlayer)
				{
					Pawn_StanceTracker stances = base.Pawn.stances;
					bool? flag;
					if (stances == null)
					{
						flag = null;
					}
					else
					{
						StunHandler stunner = stances.stunner;
						flag = ((stunner != null) ? new bool?(stunner.Stunned) : null);
					}
					bool? flag2 = flag;
					if (flag2.GetValueOrDefault())
					{
						return true;
					}
				}
				if (base.Pawn.IsBurning())
				{
					return true;
				}
				if (this.Props.affectedByDisruptor && base.Pawn.health.hediffSet.HasHediff(HediffDefOf.DisruptorFlash, false))
				{
					return true;
				}
				if (base.Pawn.health.hediffSet.HasHediff(HediffDefOf.CoveredInFirefoam, false))
				{
					return true;
				}
				if (base.Pawn.ParentHolder is Pawn_CarryTracker)
				{
					return true;
				}
				CompHoldingPlatformTarget compHoldingPlatformTarget = this.PlatformTarget;
				return compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform;
			}
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x000BF8C0 File Offset: 0x000BDAC0
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<bool>(ref this.wasForcedVisibleLastTick, "wasForcedVisibleLastTick", false, false);
			Scribe_Values.Look<int>(ref this.lastDisrupted, "lastTookDamageTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.everVisible, "everVisible", false, false);
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x000BF90D File Offset: 0x000BDB0D
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.BecomeInvisible(true);
			this.UpdateTarget();
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x000BF923 File Offset: 0x000BDB23
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			this.UpdateTarget();
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x000BF934 File Offset: 0x000BDB34
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (!this.ShouldBeVisible)
			{
				if (!this.wasForcedVisibleLastTick && this.ForcedVisible && this.FadePct == 0f)
				{
					base.Pawn.mindState.lastForcedVisibleTick = Find.TickManager.TicksGame;
					EffecterDefOf.ForcedVisible.Spawn(this.parent.pawn, this.parent.pawn.MapHeld, 1f).Cleanup();
					base.Pawn.Notify_ForcedVisible();
				}
				if (this.wasForcedVisibleLastTick && !this.ForcedVisible)
				{
					base.Pawn.mindState.lastBecameInvisibleTick = Find.TickManager.TicksGame;
					base.Pawn.Notify_BecameInvisible();
				}
			}
			this.wasForcedVisibleLastTick = this.ForcedVisible;
			if (!this.everVisible && this.PsychologicallyVisible)
			{
				this.everVisible = true;
				List<EntityCodexEntryDef> entries;
				if (ModsConfig.AnomalyActive && !this.Props.visibleToPlayer && AnomalyUtility.ShouldNotifyCodex(base.Pawn, EntityDiscoveryType.BecameVisible, out entries))
				{
					Find.EntityCodex.SetDiscovered(entries, base.Pawn.def, base.Pawn);
					return;
				}
				Find.HiddenItemsManager.SetDiscovered(base.Pawn.def);
			}
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x000BFA6C File Offset: 0x000BDC6C
		private void UpdateTarget()
		{
			if (!ModLister.CheckRoyaltyOrAnomaly("Invisibility hediff"))
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			if (pawn.Spawned)
			{
				pawn.Map.attackTargetsCache.UpdateTarget(pawn);
			}
			if (pawn.RaceProps.Humanlike)
			{
				PortraitsCache.SetDirty(pawn);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
			}
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x000BFAC8 File Offset: 0x000BDCC8
		public void BecomeVisible(bool instant = false)
		{
			if (this.ShouldBeVisible)
			{
				return;
			}
			if (this.wasForcedVisibleLastTick)
			{
				instant = true;
			}
			if (instant)
			{
				EffecterDefOf.ForcedVisible.Spawn(this.parent.pawn, this.parent.pawn.Map, 1f).Cleanup();
				base.Pawn.mindState.lastBecameVisibleTick = Find.TickManager.TicksGame - this.Props.fadeDurationTicks;
			}
			else
			{
				base.Pawn.mindState.lastBecameVisibleTick = Find.TickManager.TicksGame;
			}
			if (!this.ForcedVisible && !this.wasForcedVisibleLastTick)
			{
				base.Pawn.Notify_BecameVisible();
			}
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x000BFB78 File Offset: 0x000BDD78
		public void BecomeInvisible(bool instant = false)
		{
			if (!this.ShouldBeVisible)
			{
				return;
			}
			if (instant)
			{
				base.Pawn.mindState.lastBecameInvisibleTick = Find.TickManager.TicksGame - this.Props.fadeDurationTicks;
			}
			else
			{
				base.Pawn.mindState.lastBecameInvisibleTick = Find.TickManager.TicksGame;
			}
			if (!this.ForcedVisible)
			{
				base.Pawn.Notify_BecameInvisible();
			}
			base.Pawn.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x000BFBFB File Offset: 0x000BDDFB
		public float GetAlpha()
		{
			if (this.Props.visibleToPlayer)
			{
				return 1f;
			}
			if (this.ForcedVisible)
			{
				return 1f;
			}
			return HediffComp_Invisibility.RevealCurve.Evaluate(this.FadePct);
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x000BFC2E File Offset: 0x000BDE2E
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			this.DisruptInvisibility();
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x000BFC36 File Offset: 0x000BDE36
		public void DisruptInvisibility()
		{
			this.lastDisrupted = Find.TickManager.TicksGame;
		}

		// Token: 0x04001897 RID: 6295
		private int lastDisrupted = -99999;

		// Token: 0x04001898 RID: 6296
		private bool wasForcedVisibleLastTick;

		// Token: 0x04001899 RID: 6297
		private bool everVisible;

		// Token: 0x0400189A RID: 6298
		private static readonly SimpleCurve RevealCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 0.5f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x0400189B RID: 6299
		[Unsaved(false)]
		private CompHoldingPlatformTarget platformTarget;
	}
}
