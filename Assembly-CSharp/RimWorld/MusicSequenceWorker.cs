using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017C5 RID: 6085
	public class MusicSequenceWorker
	{
		// Token: 0x170019D2 RID: 6610
		// (get) Token: 0x060097EB RID: 38891 RVA: 0x00371C8C File Offset: 0x0036FE8C
		// (set) Token: 0x060097EC RID: 38892 RVA: 0x00371C94 File Offset: 0x0036FE94
		public MusicSequenceDef def { get; private set; }

		// Token: 0x170019D3 RID: 6611
		// (get) Token: 0x060097ED RID: 38893 RVA: 0x00371C9D File Offset: 0x0036FE9D
		// (set) Token: 0x060097EE RID: 38894 RVA: 0x00371CA5 File Offset: 0x0036FEA5
		public MusicManagerPlay musicManager { get; private set; }

		// Token: 0x170019D4 RID: 6612
		// (get) Token: 0x060097EF RID: 38895 RVA: 0x00371CAE File Offset: 0x0036FEAE
		// (set) Token: 0x060097F0 RID: 38896 RVA: 0x00371CB6 File Offset: 0x0036FEB6
		public int timesLooped { get; set; }

		// Token: 0x170019D5 RID: 6613
		// (get) Token: 0x060097F1 RID: 38897 RVA: 0x00371CBF File Offset: 0x0036FEBF
		public bool IsPlaying
		{
			get
			{
				return this.musicManager.MusicSequenceWorker == this && this.musicManager.IsPlaying;
			}
		}

		// Token: 0x060097F2 RID: 38898 RVA: 0x00371CDC File Offset: 0x0036FEDC
		public void InitializeWorker(MusicSequenceDef def, MusicManagerPlay musicManager)
		{
			this.def = def;
			this.musicManager = musicManager;
			MusicSequenceWorker.songs.Clear();
			if (def.songs != null)
			{
				MusicSequenceWorker.songs.AddRange(def.songs);
			}
			this.Shuffle();
		}

		// Token: 0x060097F3 RID: 38899 RVA: 0x00371D14 File Offset: 0x0036FF14
		public virtual bool ShouldTransition()
		{
			return (!this.IsPlaying && !this.def.transitionOnDanger && !this.def.transitionOnNoDanger) || (this.def.transitionOnDanger && this.musicManager.DangerMusicMode) || (this.def.transitionOnNoDanger && !this.musicManager.DangerMusicMode);
		}

		// Token: 0x060097F4 RID: 38900 RVA: 0x00371D7C File Offset: 0x0036FF7C
		public virtual bool ShouldLoop()
		{
			return this.def.loop;
		}

		// Token: 0x060097F5 RID: 38901 RVA: 0x00371D8C File Offset: 0x0036FF8C
		public virtual bool ShouldEnd()
		{
			return this.MinTimeSatisfied() && ((this.def.endOnDanger && this.musicManager.DangerMusicMode) || (this.def.endOnNoDanger && !this.musicManager.DangerMusicMode));
		}

		// Token: 0x060097F6 RID: 38902 RVA: 0x00371DDC File Offset: 0x0036FFDC
		public virtual bool CanBeInterrupted()
		{
			return this.def.canBeInterrupted;
		}

		// Token: 0x060097F7 RID: 38903 RVA: 0x00371DEC File Offset: 0x0036FFEC
		public virtual SongDef SelectSong()
		{
			if (this.def.song != null)
			{
				return this.def.song;
			}
			if (this.timesLooped % MusicSequenceWorker.songs.Count == 0)
			{
				this.Shuffle();
			}
			return MusicSequenceWorker.songs[this.timesLooped % this.def.songs.Count];
		}

		// Token: 0x060097F8 RID: 38904 RVA: 0x00371E4C File Offset: 0x0037004C
		private void Shuffle()
		{
			if (MusicSequenceWorker.songs.Count <= 2)
			{
				return;
			}
			SongDef last = MusicSequenceWorker.songs.GetLast<SongDef>();
			int i = MusicSequenceWorker.songs.Count;
			while (i > 1)
			{
				i--;
				int num = Rand.Range(0, i + 1);
				List<SongDef> list = MusicSequenceWorker.songs;
				int index = num;
				List<SongDef> list2 = MusicSequenceWorker.songs;
				int index2 = i;
				SongDef value = MusicSequenceWorker.songs[i];
				SongDef value2 = MusicSequenceWorker.songs[num];
				list[index] = value;
				list2[index2] = value2;
			}
			if (MusicSequenceWorker.songs[0] == last)
			{
				int num2 = Rand.Range(1, MusicSequenceWorker.songs.Count);
				List<SongDef> list3 = MusicSequenceWorker.songs;
				List<SongDef> list = MusicSequenceWorker.songs;
				int index2 = num2;
				SongDef value2 = MusicSequenceWorker.songs[num2];
				SongDef value = MusicSequenceWorker.songs[0];
				list3[0] = value2;
				list[index2] = value;
			}
		}

		// Token: 0x060097F9 RID: 38905 RVA: 0x00371F2C File Offset: 0x0037012C
		public bool MinTimeSatisfied()
		{
			return this.def.minTimeToPlay == null || this.timesLooped > 0 || this.musicManager.SongTime >= this.def.minTimeToPlay.Value;
		}

		// Token: 0x04005AF5 RID: 23285
		private static readonly List<SongDef> songs = new List<SongDef>();
	}
}
