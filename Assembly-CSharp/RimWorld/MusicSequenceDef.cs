using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB0 RID: 4016
	public class MusicSequenceDef : Def
	{
		// Token: 0x06006613 RID: 26131 RVA: 0x00244C2C File Offset: 0x00242E2C
		public override IEnumerable<string> ConfigErrors()
		{
			MusicSequenceDef.<ConfigErrors>d__19 <ConfigErrors>d__ = new MusicSequenceDef.<ConfigErrors>d__19(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003DDC RID: 15836
		public Type workerType = typeof(MusicSequenceWorker);

		// Token: 0x04003DDD RID: 15837
		public SongDef song;

		// Token: 0x04003DDE RID: 15838
		public List<SongDef> songs;

		// Token: 0x04003DDF RID: 15839
		public MusicSequenceDef nextSequence;

		// Token: 0x04003DE0 RID: 15840
		public bool loop;

		// Token: 0x04003DE1 RID: 15841
		public bool canBeInterrupted = true;

		// Token: 0x04003DE2 RID: 15842
		public bool useTransitionForLifetime = true;

		// Token: 0x04003DE3 RID: 15843
		public bool transitionOnDanger;

		// Token: 0x04003DE4 RID: 15844
		public bool transitionOnNoDanger;

		// Token: 0x04003DE5 RID: 15845
		public bool endOnNoDanger;

		// Token: 0x04003DE6 RID: 15846
		public bool endOnDanger;

		// Token: 0x04003DE7 RID: 15847
		public FloatRange loopDelayRange = FloatRange.Zero;

		// Token: 0x04003DE8 RID: 15848
		public float fadeoutLastSongDuration = 10f;

		// Token: 0x04003DE9 RID: 15849
		public float fadeInDuration;

		// Token: 0x04003DEA RID: 15850
		public bool loopFadeout;

		// Token: 0x04003DEB RID: 15851
		public float? overrideFadeout;

		// Token: 0x04003DEC RID: 15852
		public float? overrideVolume;

		// Token: 0x04003DED RID: 15853
		public float? minTimeToPlay;

		// Token: 0x04003DEE RID: 15854
		public float? pausedVolumeFactor;
	}
}
