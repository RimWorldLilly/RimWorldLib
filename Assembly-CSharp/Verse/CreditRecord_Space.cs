using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000697 RID: 1687
	public class CreditRecord_Space : CreditsEntry
	{
		// Token: 0x06003529 RID: 13609 RVA: 0x0015A452 File Offset: 0x00158652
		public CreditRecord_Space()
		{
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x0015A465 File Offset: 0x00158665
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x0015A47F File Offset: 0x0015867F
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x0015A487 File Offset: 0x00158687
		public override void Draw(Rect rect)
		{
		}

		// Token: 0x040022D6 RID: 8918
		private float height = 10f;
	}
}
