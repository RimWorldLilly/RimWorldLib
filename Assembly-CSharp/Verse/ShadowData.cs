using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002C3 RID: 707
	public class ShadowData
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x000809FA File Offset: 0x0007EBFA
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001589 RID: 5513 RVA: 0x00080A07 File Offset: 0x0007EC07
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x00080A14 File Offset: 0x0007EC14
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}

		// Token: 0x040012BC RID: 4796
		public Vector3 volume = Vector3.one;

		// Token: 0x040012BD RID: 4797
		public Vector3 offset = Vector3.zero;
	}
}
