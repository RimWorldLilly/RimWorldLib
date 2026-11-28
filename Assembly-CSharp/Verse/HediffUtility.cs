using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200049C RID: 1180
	public static class HediffUtility
	{
		// Token: 0x0600209A RID: 8346 RVA: 0x000C340C File Offset: 0x000C160C
		public static T TryGetComp<T>(this Hediff hd) where T : HediffComp
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return default(T);
			}
			if (hediffWithComps.comps != null)
			{
				for (int i = 0; i < hediffWithComps.comps.Count; i++)
				{
					T t = hediffWithComps.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x000C3475 File Offset: 0x000C1675
		public static bool TryGetComp<T>(this Hediff hd, out T comp) where T : HediffComp
		{
			comp = hd.TryGetComp<T>();
			return comp != null;
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x000C3494 File Offset: 0x000C1694
		public static bool IsTended(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_TendDuration hediffComp_TendDuration = hediffWithComps.TryGetComp<HediffComp_TendDuration>();
			return hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000C34C0 File Offset: 0x000C16C0
		public static bool IsPermanent(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_GetsPermanent hediffComp_GetsPermanent = hediffWithComps.TryGetComp<HediffComp_GetsPermanent>();
			return hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent;
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x000C34EC File Offset: 0x000C16EC
		public static bool FullyImmune(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_Immunizable hediffComp_Immunizable = hediffWithComps.TryGetComp<HediffComp_Immunizable>();
			return hediffComp_Immunizable != null && hediffComp_Immunizable.FullyImmune;
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x000C3517 File Offset: 0x000C1717
		public static bool CanHealFromTending(this Hediff_Injury hd)
		{
			return hd.IsTended() && !hd.IsPermanent();
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x000C352C File Offset: 0x000C172C
		public static bool CanHealNaturally(this Hediff_Injury hd)
		{
			return !hd.IsPermanent();
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x000C3538 File Offset: 0x000C1738
		public static int CountAddedAndImplantedParts(this HediffSet hs)
		{
			int num = 0;
			List<Hediff> hediffs = hs.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.countsAsAddedPartOrImplant)
				{
					num++;
				}
			}
			return num;
		}
	}
}
