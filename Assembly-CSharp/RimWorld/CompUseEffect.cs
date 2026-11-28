using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A2D RID: 6701
	public abstract class CompUseEffect : ThingComp
	{
		// Token: 0x17001D82 RID: 7554
		// (get) Token: 0x0600A838 RID: 43064 RVA: 0x003BCA4B File Offset: 0x003BAC4B
		public virtual float OrderPriority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001D83 RID: 7555
		// (get) Token: 0x0600A839 RID: 43065 RVA: 0x003BCA52 File Offset: 0x003BAC52
		private CompProperties_UseEffect Props
		{
			get
			{
				return (CompProperties_UseEffect)this.props;
			}
		}

		// Token: 0x0600A83A RID: 43066 RVA: 0x003BCA60 File Offset: 0x003BAC60
		public virtual void DoEffect(Pawn usedBy)
		{
			if (usedBy.Map == Find.CurrentMap)
			{
				if (this.Props.doCameraShake && usedBy.Spawned)
				{
					Find.CameraDriver.shaker.DoShake(1f);
				}
				if (this.Props.moteOnUsed != null)
				{
					MoteMaker.MakeAttachedOverlay(usedBy, this.Props.moteOnUsed, Vector3.zero, this.Props.moteOnUsedScale, -1f);
				}
				if (this.Props.fleckOnUsed != null)
				{
					FleckMaker.AttachedOverlay(usedBy, this.Props.fleckOnUsed, Vector3.zero, this.Props.fleckOnUsedScale, -1f);
				}
				if (this.Props.effecterOnUsed != null)
				{
					this.Props.effecterOnUsed.SpawnMaintained(usedBy, new TargetInfo(this.parent.Position, this.parent.Map, false), 1f);
				}
				Effecter effecter = this.effecter;
				if (effecter == null)
				{
					return;
				}
				effecter.Cleanup();
			}
		}

		// Token: 0x0600A83B RID: 43067 RVA: 0x003BCB64 File Offset: 0x003BAD64
		public virtual void PrepareTick()
		{
			if (this.Props.warmupEffecter != null)
			{
				if (this.effecter == null)
				{
					this.effecter = this.Props.warmupEffecter.Spawn(this.parent, this.parent.Map, 1f);
				}
				Effecter effecter = this.effecter;
				if (effecter == null)
				{
					return;
				}
				effecter.EffectTick(this.parent, this.parent);
			}
		}

		// Token: 0x0600A83C RID: 43068 RVA: 0x003BCBD8 File Offset: 0x003BADD8
		public virtual TaggedString ConfirmMessage(Pawn p)
		{
			return null;
		}

		// Token: 0x0600A83D RID: 43069 RVA: 0x003BCBE0 File Offset: 0x003BADE0
		public virtual bool SelectedUseOption(Pawn p)
		{
			return false;
		}

		// Token: 0x0600A83E RID: 43070 RVA: 0x003BCBE3 File Offset: 0x003BADE3
		public virtual AcceptanceReport CanBeUsedBy(Pawn p)
		{
			return true;
		}

		// Token: 0x040062DA RID: 25306
		private const float CameraShakeMag = 1f;

		// Token: 0x040062DB RID: 25307
		private Effecter effecter;
	}
}
