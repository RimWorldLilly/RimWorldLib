using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x02000950 RID: 2384
	public static class LordUtility
	{
		// Token: 0x0600492C RID: 18732 RVA: 0x001C95EF File Offset: 0x001C77EF
		public static Lord GetLord(this Pawn p)
		{
			return p.lord;
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x001C95F7 File Offset: 0x001C77F7
		public static bool TryGetLord(this Pawn p, out Lord lord)
		{
			lord = p.GetLord();
			return lord != null;
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x001C9608 File Offset: 0x001C7808
		public static Lord GetLord(this Building b)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Lord lord = maps[i].lordManager.LordOf(b);
				if (lord != null)
				{
					return lord;
				}
			}
			return null;
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x001C9648 File Offset: 0x001C7848
		public static Lord GetLord(this Corpse c)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Lord lord = maps[i].lordManager.LordOf(c);
				if (lord != null)
				{
					return lord;
				}
			}
			return null;
		}
	}
}
