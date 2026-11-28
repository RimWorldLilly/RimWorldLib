using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000133 RID: 307
	public class ColorGenerator_Options : ColorGenerator
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x00036D8C File Offset: 0x00034F8C
		public override Color ExemplaryColor
		{
			get
			{
				ColorOption colorOption = null;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (colorOption == null || this.options[i].weight > colorOption.weight)
					{
						colorOption = this.options[i];
					}
				}
				if (colorOption == null)
				{
					return Color.white;
				}
				if (colorOption.only.a >= 0f)
				{
					return colorOption.only;
				}
				return new Color((colorOption.min.r + colorOption.max.r) / 2f, (colorOption.min.g + colorOption.max.g) / 2f, (colorOption.min.b + colorOption.max.b) / 2f, (colorOption.min.a + colorOption.max.a) / 2f);
			}
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00036E75 File Offset: 0x00035075
		public override Color NewRandomizedColor()
		{
			return this.options.RandomElementByWeight((ColorOption pi) => pi.weight).RandomizedColor();
		}

		// Token: 0x0400058F RID: 1423
		public List<ColorOption> options = new List<ColorOption>();
	}
}
