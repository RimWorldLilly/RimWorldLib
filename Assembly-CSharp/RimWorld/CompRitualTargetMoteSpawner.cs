using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A2A RID: 6698
	public class CompRitualTargetMoteSpawner : CompRitualEffectSpawner
	{
		// Token: 0x17001D80 RID: 7552
		// (get) Token: 0x0600A82C RID: 43052 RVA: 0x003BC891 File Offset: 0x003BAA91
		private CompProperties_RitualTargetMoteSpawner Props
		{
			get
			{
				return (CompProperties_RitualTargetMoteSpawner)this.props;
			}
		}

		// Token: 0x0600A82D RID: 43053 RVA: 0x003BC89E File Offset: 0x003BAA9E
		protected override void Tick_InRitual(LordJob_Ritual ritual)
		{
			Mote mote = this.mote;
			if (mote == null)
			{
				return;
			}
			mote.Maintain();
		}

		// Token: 0x0600A82E RID: 43054 RVA: 0x003BC8B0 File Offset: 0x003BAAB0
		protected override void Tick_InRitualInterval(LordJob_Ritual ritual)
		{
			if (this.mote == null || this.mote.Destroyed)
			{
				this.mote = MoteMaker.MakeStaticMote(this.parent.Position.ToVector3Shifted(), this.parent.Map, this.Props.mote, 1f, false, 0f);
			}
		}

		// Token: 0x0600A82F RID: 43055 RVA: 0x003BC911 File Offset: 0x003BAB11
		protected override void Tick_OutOfRitualInterval()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				Mote mote = this.mote;
				if (mote != null)
				{
					mote.Destroy(DestroyMode.Vanish);
				}
			}
			this.mote = null;
		}

		// Token: 0x040062D6 RID: 25302
		private Mote mote;
	}
}
