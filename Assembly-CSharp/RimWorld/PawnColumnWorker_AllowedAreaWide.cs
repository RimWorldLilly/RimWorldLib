using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001DC9 RID: 7625
	public class PawnColumnWorker_AllowedAreaWide : PawnColumnWorker_AllowedArea
	{
		// Token: 0x0600C621 RID: 50721 RVA: 0x0048A459 File Offset: 0x00488659
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(350, this.GetMinWidth(table), this.GetMaxWidth(table));
		}
	}
}
