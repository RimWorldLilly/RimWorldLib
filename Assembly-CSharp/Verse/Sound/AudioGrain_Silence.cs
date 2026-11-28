using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x020007BE RID: 1982
	public class AudioGrain_Silence : AudioGrain
	{
		// Token: 0x06003FA5 RID: 16293 RVA: 0x00197665 File Offset: 0x00195865
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioGrain_Silence.<GetResolvedGrains>d__1 <GetResolvedGrains>d__ = new AudioGrain_Silence.<GetResolvedGrains>d__1(-2);
			<GetResolvedGrains>d__.<>4__this = this;
			return <GetResolvedGrains>d__;
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x00197675 File Offset: 0x00195875
		public override int GetHashCode()
		{
			return this.durationRange.GetHashCode();
		}

		// Token: 0x04002A2B RID: 10795
		[EditSliderRange(0f, 5f)]
		public FloatRange durationRange = new FloatRange(1f, 2f);
	}
}
