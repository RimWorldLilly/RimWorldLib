using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x020007BD RID: 1981
	public class AudioGrain_Folder : AudioGrain
	{
		// Token: 0x06003FA3 RID: 16291 RVA: 0x00197642 File Offset: 0x00195842
		public override IEnumerable<ResolvedGrain> GetResolvedGrains()
		{
			AudioGrain_Folder.<GetResolvedGrains>d__1 <GetResolvedGrains>d__ = new AudioGrain_Folder.<GetResolvedGrains>d__1(-2);
			<GetResolvedGrains>d__.<>4__this = this;
			return <GetResolvedGrains>d__;
		}

		// Token: 0x04002A2A RID: 10794
		[LoadAlias("clipPath")]
		[NoTranslate]
		public string clipFolderPath = "";
	}
}
