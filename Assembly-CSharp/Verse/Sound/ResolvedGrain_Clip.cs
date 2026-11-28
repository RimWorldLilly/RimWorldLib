using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007C0 RID: 1984
	public class ResolvedGrain_Clip : ResolvedGrain
	{
		// Token: 0x06003FA9 RID: 16297 RVA: 0x001976AD File Offset: 0x001958AD
		public ResolvedGrain_Clip(AudioClip clip)
		{
			this.clip = clip;
			this.duration = clip.length;
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x001976C8 File Offset: 0x001958C8
		public override string ToString()
		{
			return "Clip:" + this.clip.name;
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x001976E0 File Offset: 0x001958E0
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ResolvedGrain_Clip resolvedGrain_Clip = obj as ResolvedGrain_Clip;
			return resolvedGrain_Clip != null && resolvedGrain_Clip.clip == this.clip;
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0019770F File Offset: 0x0019590F
		public override int GetHashCode()
		{
			if (this.clip == null)
			{
				return 0;
			}
			return this.clip.GetHashCode();
		}

		// Token: 0x04002A2D RID: 10797
		public AudioClip clip;
	}
}
