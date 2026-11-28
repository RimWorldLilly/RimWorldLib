using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001865 RID: 6245
	public class CompDisruptorFlare : ThingComp
	{
		// Token: 0x17001AB3 RID: 6835
		// (get) Token: 0x06009C44 RID: 40004 RVA: 0x003842CD File Offset: 0x003824CD
		public CompProperties_DisruptorFlare Props
		{
			get
			{
				return (CompProperties_DisruptorFlare)this.props;
			}
		}

		// Token: 0x17001AB4 RID: 6836
		// (get) Token: 0x06009C45 RID: 40005 RVA: 0x003842DA File Offset: 0x003824DA
		private int WarnTick
		{
			get
			{
				return this.destroyDelayedComp.DestructionTick - 900;
			}
		}

		// Token: 0x17001AB5 RID: 6837
		// (get) Token: 0x06009C46 RID: 40006 RVA: 0x003842ED File Offset: 0x003824ED
		private float WarnAlpha
		{
			get
			{
				return Mathf.InverseLerp((float)this.WarnTick, (float)this.destroyDelayedComp.DestructionTick, (float)Find.TickManager.TicksGame);
			}
		}

		// Token: 0x17001AB6 RID: 6838
		// (get) Token: 0x06009C47 RID: 40007 RVA: 0x00384312 File Offset: 0x00382512
		private float GlowRadius
		{
			get
			{
				return this.Props.radius + 2f;
			}
		}

		// Token: 0x06009C48 RID: 40008 RVA: 0x00384328 File Offset: 0x00382528
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.glowComp = this.parent.GetComp<CompGlower>();
			if (this.glowComp != null)
			{
				this.glowComp.GlowRadius = this.GlowRadius;
			}
			this.destroyDelayedComp = this.parent.GetComp<CompDestroyAfterDelay>();
			if (respawningAfterLoad)
			{
				return;
			}
			foreach (Pawn pawn in this.parent.Map.mapPawns.AllPawnsSpawned.ToList<Pawn>())
			{
				if (pawn.Spawned && pawn.Position.InHorDistOf(this.parent.Position, this.Props.radius) && GenSight.LineOfSightToThing(this.parent.Position, pawn, this.parent.Map, false, null))
				{
					if (!pawn.health.hediffSet.HasHediff(HediffDefOf.DisruptorFlash, false))
					{
						this.PsychicStun(pawn);
					}
					pawn.health.AddHediff(HediffDefOf.DisruptorFlash, null, null, null);
					CompRevenant compRevenant = pawn.TryGetComp<CompRevenant>();
					if (compRevenant != null && pawn.IsPsychologicallyInvisible())
					{
						compRevenant.SetState(RevenantState.Escape);
					}
				}
			}
		}

		// Token: 0x06009C49 RID: 40009 RVA: 0x0038447C File Offset: 0x0038267C
		private void PsychicStun(Pawn pawn)
		{
			float statValue = pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
			if (statValue > 0f)
			{
				Pawn_StanceTracker stances = pawn.stances;
				if (stances == null)
				{
					return;
				}
				StunHandler stunner = stances.stunner;
				if (stunner == null)
				{
					return;
				}
				stunner.StunFor(Mathf.RoundToInt(120f * statValue), null, true, true, false);
			}
		}

		// Token: 0x06009C4A RID: 40010 RVA: 0x003844CC File Offset: 0x003826CC
		public override void CompTick()
		{
			if (this.parent.Spawned)
			{
				if (this.attachedEffecter == null)
				{
					this.attachedEffecter = this.Props.effecterDef.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
				}
				if (this.WarnTick - Find.TickManager.TicksGame > 80)
				{
					Effecter effecter = this.attachedEffecter;
					if (effecter != null)
					{
						effecter.EffectTick(this.parent, this.parent);
					}
				}
			}
			else
			{
				Effecter effecter2 = this.attachedEffecter;
				if (effecter2 != null)
				{
					effecter2.Cleanup();
				}
				this.attachedEffecter = null;
			}
			if (Find.TickManager.TicksGame >= this.WarnTick)
			{
				if (this.destroyWarningEffecter == null)
				{
					this.destroyWarningEffecter = this.Props.destroyWarningEffecterDef.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
				}
				this.destroyWarningEffecter.EffectTick(this.parent, this.parent);
				float warnAlpha = this.WarnAlpha;
				ColorInt glowColor = this.glowComp.GlowColor;
				glowColor.a = (int)((byte)Mathf.Lerp((float)glowColor.a, 0f, warnAlpha));
				this.glowComp.GlowRadius = Mathf.Lerp(this.GlowRadius, 0.1f, warnAlpha);
				this.glowComp.GlowColor = glowColor;
				if (this.attachedEffecter != null)
				{
					this.attachedEffecter.children[0].colorOverride = new Color?(new Color(1f, 1f, 1f, warnAlpha));
				}
			}
		}

		// Token: 0x04005CD7 RID: 23767
		private const int BaseStunDuration = 120;

		// Token: 0x04005CD8 RID: 23768
		private const int DestroyWarningTicks = 900;

		// Token: 0x04005CD9 RID: 23769
		private Effecter destroyWarningEffecter;

		// Token: 0x04005CDA RID: 23770
		private CompDestroyAfterDelay destroyDelayedComp;

		// Token: 0x04005CDB RID: 23771
		private Effecter attachedEffecter;

		// Token: 0x04005CDC RID: 23772
		private CompGlower glowComp;
	}
}
