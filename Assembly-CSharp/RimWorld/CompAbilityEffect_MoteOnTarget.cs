using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200141E RID: 5150
	public class CompAbilityEffect_MoteOnTarget : CompAbilityEffect
	{
		// Token: 0x170015C6 RID: 5574
		// (get) Token: 0x06007F45 RID: 32581 RVA: 0x002D7708 File Offset: 0x002D5908
		public new CompProperties_AbilityMoteOnTarget Props
		{
			get
			{
				return (CompProperties_AbilityMoteOnTarget)this.props;
			}
		}

		// Token: 0x06007F46 RID: 32582 RVA: 0x002D7718 File Offset: 0x002D5918
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

		// Token: 0x06007F47 RID: 32583 RVA: 0x002D7772 File Offset: 0x002D5972
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			CompAbilityEffect_MoteOnTarget.<GetPreCastActions>d__3 <GetPreCastActions>d__ = new CompAbilityEffect_MoteOnTarget.<GetPreCastActions>d__3(-2);
			<GetPreCastActions>d__.<>4__this = this;
			return <GetPreCastActions>d__;
		}

		// Token: 0x06007F48 RID: 32584 RVA: 0x002D7784 File Offset: 0x002D5984
		private void SpawnAll(LocalTargetInfo target)
		{
			if (!this.Props.moteDefs.NullOrEmpty<ThingDef>())
			{
				for (int i = 0; i < this.Props.moteDefs.Count; i++)
				{
					this.SpawnMote(target, this.Props.moteDefs[i]);
				}
				return;
			}
			this.SpawnMote(target, this.Props.moteDef);
		}

		// Token: 0x06007F49 RID: 32585 RVA: 0x002D77EC File Offset: 0x002D59EC
		private void SpawnMote(LocalTargetInfo target, ThingDef def)
		{
			if (target.HasThing)
			{
				MoteMaker.MakeAttachedOverlay(target.Thing, def, Vector3.zero, this.Props.scale, -1f);
				return;
			}
			MoteMaker.MakeStaticMote(target.Cell, this.parent.pawn.Map, def, this.Props.scale);
		}
	}
}
