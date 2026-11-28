using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019D7 RID: 6615
	public class CompVoidStructure : CompInteractable
	{
		// Token: 0x17001D1B RID: 7451
		// (get) Token: 0x0600A687 RID: 42631 RVA: 0x003B5E89 File Offset: 0x003B4089
		protected virtual bool Activatable
		{
			get
			{
				return !this.activated;
			}
		}

		// Token: 0x17001D1C RID: 7452
		// (get) Token: 0x0600A688 RID: 42632 RVA: 0x003B5E94 File Offset: 0x003B4094
		public override bool Active
		{
			get
			{
				return this.activated;
			}
		}

		// Token: 0x17001D1D RID: 7453
		// (get) Token: 0x0600A689 RID: 42633 RVA: 0x003B5E9C File Offset: 0x003B409C
		protected virtual SoundDef AmbientSound
		{
			get
			{
				if (!this.Active)
				{
					return SoundDefOf.VoidStructure_AmbientPreActivate;
				}
				return SoundDefOf.VoidStructure_AmbientPostActivate;
			}
		}

		// Token: 0x0600A68A RID: 42634 RVA: 0x003B5EB4 File Offset: 0x003B40B4
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active && this.ambienceEffecter == null)
			{
				this.ambienceEffecter = EffecterDefOf.VoidStructureActivatedAmbience.Spawn(this.parent, this.parent.Map, 1f);
			}
			if (this.AmbientSound != null)
			{
				if (this.ambienceSustainer == null || this.ambienceSustainer.Ended)
				{
					this.ambienceSustainer = this.AmbientSound.TrySpawnSustainer(this.parent);
				}
				if (this.ambienceSustainer.def != this.AmbientSound)
				{
					this.ambienceSustainer.End();
				}
			}
			Effecter effecter = this.ambienceEffecter;
			if (effecter == null)
			{
				return;
			}
			effecter.EffectTick(this.parent, this.parent);
		}

		// Token: 0x0600A68B RID: 42635 RVA: 0x003B5F7C File Offset: 0x003B417C
		protected override void OnInteracted(Pawn caster)
		{
			this.parent.overrideGraphicIndex = new int?(1);
			this.parent.DirtyMapMesh(this.parent.Map);
			EffecterDefOf.VoidStructureActivated.Spawn(this.parent, this.parent.Map, 1f);
			CompGlower comp = this.parent.GetComp<CompGlower>();
			if (comp != null)
			{
				comp.GlowRadius = comp.Props.glowRadius + 1f;
				comp.GlowColor = new ColorInt(240, 160, 184);
			}
			this.activated = true;
		}

		// Token: 0x0600A68C RID: 42636 RVA: 0x003B6018 File Offset: 0x003B4218
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (!this.Activatable)
			{
				return false;
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}

		// Token: 0x0600A68D RID: 42637 RVA: 0x003B6031 File Offset: 0x003B4231
		public void Reset()
		{
			this.activated = false;
		}

		// Token: 0x0600A68E RID: 42638 RVA: 0x003B603C File Offset: 0x003B423C
		public override string CompInspectStringExtra()
		{
			if (this.Active)
			{
				return "Activated".Translate().CapitalizeFirst();
			}
			if (!this.Activatable)
			{
				return "";
			}
			return base.CompInspectStringExtra();
		}

		// Token: 0x0600A68F RID: 42639 RVA: 0x003B607D File Offset: 0x003B427D
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompVoidStructure.<CompFloatMenuOptions>d__15 <CompFloatMenuOptions>d__ = new CompVoidStructure.<CompFloatMenuOptions>d__15(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x0600A690 RID: 42640 RVA: 0x003B6094 File Offset: 0x003B4294
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompVoidStructure.<CompGetGizmosExtra>d__16 <CompGetGizmosExtra>d__ = new CompVoidStructure.<CompGetGizmosExtra>d__16(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A691 RID: 42641 RVA: 0x003B60A4 File Offset: 0x003B42A4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.activated, "activated", false, false);
		}

		// Token: 0x04006223 RID: 25123
		private const int ActiveGraphicIndex = 1;

		// Token: 0x04006224 RID: 25124
		protected bool activated;

		// Token: 0x04006225 RID: 25125
		private Sustainer ambienceSustainer;

		// Token: 0x04006226 RID: 25126
		private Effecter ambienceEffecter;
	}
}
