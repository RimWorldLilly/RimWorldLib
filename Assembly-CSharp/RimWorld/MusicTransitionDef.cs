using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB2 RID: 4018
	public class MusicTransitionDef : Def
	{
		// Token: 0x06006616 RID: 26134 RVA: 0x00244C80 File Offset: 0x00242E80
		public override IEnumerable<string> ConfigErrors()
		{
			MusicTransitionDef.<ConfigErrors>d__5 <ConfigErrors>d__ = new MusicTransitionDef.<ConfigErrors>d__5(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003DF3 RID: 15859
		public Type workerType = typeof(MusicTransition);

		// Token: 0x04003DF4 RID: 15860
		public int priority = 100;

		// Token: 0x04003DF5 RID: 15861
		public MusicSequenceDef sequence;

		// Token: 0x04003DF6 RID: 15862
		public MusicDangerRequirement dangerRequirement;

		// Token: 0x04003DF7 RID: 15863
		public bool overridesInterruptions;
	}
}
