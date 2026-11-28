using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200012F RID: 303
	public abstract class ColorGenerator
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x00036C62 File Offset: 0x00034E62
		public virtual Color ExemplaryColor
		{
			get
			{
				Rand.PushState(764543439);
				Color result = this.NewRandomizedColor();
				Rand.PopState();
				return result;
			}
		}

		// Token: 0x060009DB RID: 2523
		public abstract Color NewRandomizedColor();
	}
}
