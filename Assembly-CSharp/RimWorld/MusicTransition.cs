using System;

namespace RimWorld
{
	// Token: 0x020017C6 RID: 6086
	public class MusicTransition
	{
		// Token: 0x170019D6 RID: 6614
		// (get) Token: 0x060097FC RID: 38908 RVA: 0x00371F8C File Offset: 0x0037018C
		// (set) Token: 0x060097FD RID: 38909 RVA: 0x00371F94 File Offset: 0x00370194
		public MusicTransitionDef def { get; private set; }

		// Token: 0x170019D7 RID: 6615
		// (get) Token: 0x060097FE RID: 38910 RVA: 0x00371F9D File Offset: 0x0037019D
		// (set) Token: 0x060097FF RID: 38911 RVA: 0x00371FA5 File Offset: 0x003701A5
		public MusicManagerPlay musicManager { get; private set; }

		// Token: 0x06009800 RID: 38912 RVA: 0x00371FAE File Offset: 0x003701AE
		public void InitializeWorker(MusicTransitionDef def, MusicManagerPlay musicManager)
		{
			this.def = def;
			this.musicManager = musicManager;
		}

		// Token: 0x06009801 RID: 38913 RVA: 0x00371FBE File Offset: 0x003701BE
		public virtual bool IsTransitionSatisfied()
		{
			return (this.def.dangerRequirement != MusicDangerRequirement.RequiresDanger || this.musicManager.DangerMusicMode) && (this.def.dangerRequirement != MusicDangerRequirement.RequiresNoDanger || !this.musicManager.DangerMusicMode);
		}
	}
}
