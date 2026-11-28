using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200023F RID: 575
	public interface ICellBoolGiver
	{
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060010FD RID: 4349
		Color Color { get; }

		// Token: 0x060010FE RID: 4350
		bool GetCellBool(int index);

		// Token: 0x060010FF RID: 4351
		Color GetCellExtraColor(int index);
	}
}
