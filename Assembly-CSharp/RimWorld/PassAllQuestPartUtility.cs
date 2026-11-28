using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200104C RID: 4172
	public static class PassAllQuestPartUtility
	{
		// Token: 0x06006914 RID: 26900 RVA: 0x00251B3C File Offset: 0x0024FD3C
		public static bool AllReceived(List<string> inSignals, List<bool> signalsReceived)
		{
			if (inSignals.Count != signalsReceived.Count)
			{
				return false;
			}
			for (int i = 0; i < signalsReceived.Count; i++)
			{
				if (!signalsReceived[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
