using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001407 RID: 5127
	public class CompAbilityEffect_FleckOnTarget : CompAbilityEffect
	{
		// Token: 0x170015BB RID: 5563
		// (get) Token: 0x06007F03 RID: 32515 RVA: 0x002D67CB File Offset: 0x002D49CB
		public new CompProperties_AbilityFleckOnTarget Props
		{
			get
			{
				return (CompProperties_AbilityFleckOnTarget)this.props;
			}
		}

		// Token: 0x06007F04 RID: 32516 RVA: 0x002D67D8 File Offset: 0x002D49D8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.Props.preCastTicks <= 0)
			{
				SoundDef sound = this.Props.sound;
				if (sound != null)
				{
					sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
				}
				this.SpawnAll(target);
			}
		}

		// Token: 0x06007F05 RID: 32517 RVA: 0x002D6832 File Offset: 0x002D4A32
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			CompAbilityEffect_FleckOnTarget.<GetPreCastActions>d__3 <GetPreCastActions>d__ = new CompAbilityEffect_FleckOnTarget.<GetPreCastActions>d__3(-2);
			<GetPreCastActions>d__.<>4__this = this;
			return <GetPreCastActions>d__;
		}

		// Token: 0x06007F06 RID: 32518 RVA: 0x002D6844 File Offset: 0x002D4A44
		private void SpawnAll(LocalTargetInfo target)
		{
			if (!this.Props.fleckDefs.NullOrEmpty<FleckDef>())
			{
				for (int i = 0; i < this.Props.fleckDefs.Count; i++)
				{
					this.SpawnFleck(target, this.Props.fleckDefs[i]);
				}
				return;
			}
			this.SpawnFleck(target, this.Props.fleckDef);
		}

		// Token: 0x06007F07 RID: 32519 RVA: 0x002D68AC File Offset: 0x002D4AAC
		private void SpawnFleck(LocalTargetInfo target, FleckDef def)
		{
			if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned)
			{
				FleckMaker.AttachedOverlay(target.Thing, def, Vector3.zero, this.Props.scale, -1f);
				return;
			}
			FleckMaker.Static(target.Cell, this.parent.pawn.Map, def, this.Props.scale);
		}
	}
}
