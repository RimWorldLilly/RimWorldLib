using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007EB RID: 2027
	public class SampleOneShotManager
	{
		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06004034 RID: 16436 RVA: 0x0019956E File Offset: 0x0019776E
		public IEnumerable<SampleOneShot> PlayingOneShots
		{
			get
			{
				return this.samples;
			}
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x00199578 File Offset: 0x00197778
		private float CameraDistanceSquaredOf(SoundInfo info)
		{
			return (float)(Find.CameraDriver.MapPosition - info.Maker.Cell).LengthHorizontalSquared;
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x001995AC File Offset: 0x001977AC
		private float ImportanceOf(SampleOneShot sample)
		{
			return this.ImportanceOf(sample.subDef.parentDef, sample.info, sample.AgeRealTime);
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x001995CC File Offset: 0x001977CC
		private float ImportanceOf(SoundDef def, SoundInfo info, float ageRealTime)
		{
			if (def.priorityMode == VoicePriorityMode.PrioritizeNearest)
			{
				return 1f / (this.CameraDistanceSquaredOf(info) + 1f);
			}
			if (def.priorityMode == VoicePriorityMode.PrioritizeNewest)
			{
				return 1f / (ageRealTime + 1f);
			}
			if (def.priorityMode == VoicePriorityMode.PrioritizeExisting)
			{
				return ageRealTime;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x0019961C File Offset: 0x0019781C
		public bool CanAddPlayingOneShot(SoundDef def, SoundInfo info)
		{
			return SoundDefHelper.CorrectContextNow(def, info.Maker.Map) && (from s in this.samples
			where s.subDef.parentDef == def && (s.AgeRealTime < 0.05f || s.startFrame == Time.frameCount)
			select s).Count<SampleOneShot>() < def.MaxSimultaneousSamples;
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x00199680 File Offset: 0x00197880
		public void TryAddPlayingOneShot(SampleOneShot newSample)
		{
			if ((from s in this.samples
			where s.subDef.IsSameOrHasSameTag(newSample.subDef)
			select s).Count<SampleOneShot>() >= newSample.subDef.parentDef.maxVoices)
			{
				SampleOneShot sampleOneShot = this.LeastImportantOf(newSample.subDef);
				sampleOneShot.source.Stop();
				this.samples.Remove(sampleOneShot);
			}
			this.samples.Add(newSample);
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x00199708 File Offset: 0x00197908
		private SampleOneShot LeastImportantOf(SubSoundDef def)
		{
			SampleOneShot sampleOneShot = null;
			for (int i = 0; i < this.samples.Count; i++)
			{
				SampleOneShot sampleOneShot2 = this.samples[i];
				if (sampleOneShot2.subDef.IsSameOrHasSameTag(def) && (sampleOneShot == null || this.ImportanceOf(sampleOneShot2) < this.ImportanceOf(sampleOneShot)))
				{
					sampleOneShot = sampleOneShot2;
				}
			}
			return sampleOneShot;
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x00199760 File Offset: 0x00197960
		private SampleOneShot LeastImportantOf(SoundDef def)
		{
			SampleOneShot sampleOneShot = null;
			for (int i = 0; i < this.samples.Count; i++)
			{
				SampleOneShot sampleOneShot2 = this.samples[i];
				if (sampleOneShot2.subDef.parentDef == def && (sampleOneShot == null || this.ImportanceOf(sampleOneShot2) < this.ImportanceOf(sampleOneShot)))
				{
					sampleOneShot = sampleOneShot2;
				}
			}
			return sampleOneShot;
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x001997B8 File Offset: 0x001979B8
		public void SampleOneShotManagerUpdate()
		{
			for (int i = 0; i < this.samples.Count; i++)
			{
				this.samples[i].Update();
			}
			this.cleanupList.Clear();
			for (int j = 0; j < this.samples.Count; j++)
			{
				SampleOneShot sampleOneShot = this.samples[j];
				if (sampleOneShot.source == null || (!sampleOneShot.source.isPlaying && (!sampleOneShot.subDef.tempoAffectedByGameSpeed || !Find.TickManager.Paused)) || !SoundDefHelper.CorrectContextNow(sampleOneShot.subDef.parentDef, sampleOneShot.Map))
				{
					if (sampleOneShot.source != null && sampleOneShot.source.isPlaying)
					{
						sampleOneShot.source.Stop();
					}
					sampleOneShot.SampleCleanup();
					this.cleanupList.Add(sampleOneShot);
				}
			}
			if (this.cleanupList.Count > 0)
			{
				this.samples.RemoveAll((SampleOneShot s) => this.cleanupList.Contains(s));
			}
		}

		// Token: 0x04002AAB RID: 10923
		private List<SampleOneShot> samples = new List<SampleOneShot>();

		// Token: 0x04002AAC RID: 10924
		private List<SampleOneShot> cleanupList = new List<SampleOneShot>();
	}
}
