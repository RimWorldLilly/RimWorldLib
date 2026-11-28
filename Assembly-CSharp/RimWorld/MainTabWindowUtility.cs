using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA1 RID: 4001
	public static class MainTabWindowUtility
	{
		// Token: 0x060065F4 RID: 26100 RVA: 0x00244314 File Offset: 0x00242514
		public static void NotifyAllPawnTables_PawnsChanged()
		{
			if (Find.WindowStack == null || !UnityData.IsInMainThread)
			{
				return;
			}
			WindowStack windowStack = Find.WindowStack;
			for (int i = 0; i < windowStack.Count; i++)
			{
				MainTabWindow_PawnTable mainTabWindow_PawnTable = windowStack[i] as MainTabWindow_PawnTable;
				if (mainTabWindow_PawnTable != null)
				{
					mainTabWindow_PawnTable.Notify_PawnsChanged();
				}
			}
		}
	}
}
