using System;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001EFC RID: 7932
	[DefOf]
	public static class ImpactSoundTypeDefOf
	{
		// Token: 0x0600CAB4 RID: 51892 RVA: 0x004A64B6 File Offset: 0x004A46B6
		static ImpactSoundTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ImpactSoundTypeDefOf));
		}

		// Token: 0x040080FB RID: 33019
		public static ImpactSoundTypeDef Bullet;
	}
}
