using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000189 RID: 393
	public class MaterialTextureProperty : MaterialNamedProperty
	{
		// Token: 0x04000992 RID: 2450
		[NoTranslate]
		public string path;

		// Token: 0x04000993 RID: 2451
		public Vector2 scale = Vector2.one;

		// Token: 0x04000994 RID: 2452
		public Vector2 offset = Vector2.zero;

		// Token: 0x04000995 RID: 2453
		public bool alphaFromGrayscale;
	}
}
