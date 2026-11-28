using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x02000547 RID: 1351
	public static class ThingCompUtility
	{
		// Token: 0x060027F3 RID: 10227 RVA: 0x000F6FA0 File Offset: 0x000F51A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T TryGetComp<T>(this Thing thing) where T : ThingComp
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return default(T);
			}
			return thingWithComps.GetComp<T>();
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x000F6FC8 File Offset: 0x000F51C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetComp<T>(this Thing thing, out T comp) where T : ThingComp
		{
			comp = default(T);
			ThingWithComps thingWithComps = thing as ThingWithComps;
			return thingWithComps != null && thingWithComps.TryGetComp(out comp);
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000F6FEF File Offset: 0x000F51EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetComp<T>(this ThingWithComps thing, out T comp) where T : ThingComp
		{
			comp = thing.GetComp<T>();
			return comp != null;
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x000F700C File Offset: 0x000F520C
		public static ThingComp TryGetComp(this Thing thing, CompProperties properties)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return null;
			}
			return thingWithComps.GetCompByDefType(properties);
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x000F702C File Offset: 0x000F522C
		public static bool HasComp<T>(this Thing thing) where T : ThingComp
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			return ((thingWithComps != null) ? thingWithComps.GetComp<T>() : default(T)) != null;
		}
	}
}
