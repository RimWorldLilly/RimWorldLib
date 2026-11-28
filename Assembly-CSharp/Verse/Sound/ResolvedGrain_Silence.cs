using System;

namespace Verse.Sound
{
	// Token: 0x020007C1 RID: 1985
	public class ResolvedGrain_Silence : ResolvedGrain
	{
		// Token: 0x06003FAD RID: 16301 RVA: 0x0019772C File Offset: 0x0019592C
		public ResolvedGrain_Silence(AudioGrain_Silence sourceGrain)
		{
			this.sourceGrain = sourceGrain;
			this.duration = sourceGrain.durationRange.RandomInRange;
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x0019774C File Offset: 0x0019594C
		public override string ToString()
		{
			return "Silence";
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x00197754 File Offset: 0x00195954
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ResolvedGrain_Silence resolvedGrain_Silence = obj as ResolvedGrain_Silence;
			return resolvedGrain_Silence != null && resolvedGrain_Silence.sourceGrain == this.sourceGrain;
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x00197780 File Offset: 0x00195980
		public override int GetHashCode()
		{
			return this.sourceGrain.GetHashCode();
		}

		// Token: 0x04002A2E RID: 10798
		public AudioGrain_Silence sourceGrain;
	}
}
