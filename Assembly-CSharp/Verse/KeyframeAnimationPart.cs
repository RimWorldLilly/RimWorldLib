using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000116 RID: 278
	public class KeyframeAnimationPart : AnimationPart
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x00035A11 File Offset: 0x00033C11
		protected override Type DefaultWorker
		{
			get
			{
				return typeof(AnimationWorker_Keyframes);
			}
		}

		// Token: 0x04000482 RID: 1154
		public List<Keyframe> keyframes;
	}
}
