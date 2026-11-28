using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000709 RID: 1801
	public static class MatsFromSpectrum
	{
		// Token: 0x060038FC RID: 14588 RVA: 0x0016DB6D File Offset: 0x0016BD6D
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x0016DB7B File Offset: 0x0016BD7B
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			return SolidColorMaterials.NewSolidColorMaterial(ColorsFromSpectrum.Get(spectrum, val), shader);
		}
	}
}
