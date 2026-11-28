using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000131 RID: 305
	public class ColorGenerator_Single : ColorGenerator
	{
		// Token: 0x060009DF RID: 2527 RVA: 0x00036C90 File Offset: 0x00034E90
		public override Color NewRandomizedColor()
		{
			return this.color;
		}

		// Token: 0x0400058D RID: 1421
		public Color color;
	}
}
