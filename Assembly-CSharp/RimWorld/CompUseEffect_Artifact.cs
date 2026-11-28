using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A32 RID: 6706
	public class CompUseEffect_Artifact : CompUseEffect
	{
		// Token: 0x17001D85 RID: 7557
		// (get) Token: 0x0600A848 RID: 43080 RVA: 0x003BCD0D File Offset: 0x003BAF0D
		public CompProperties_UseEffectArtifact Props
		{
			get
			{
				return (CompProperties_UseEffectArtifact)this.props;
			}
		}

		// Token: 0x0600A849 RID: 43081 RVA: 0x003BCD1C File Offset: 0x003BAF1C
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(this.parent.Position, usedBy.MapHeld, false));
			}
			usedBy.records.Increment(RecordDefOf.ArtifactsActivated);
		}
	}
}
