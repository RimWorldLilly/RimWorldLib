using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200067C RID: 1660
	public interface IWindowDrawing
	{
		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x0600343F RID: 13375
		GUIStyle EmptyStyle { get; }

		// Token: 0x06003440 RID: 13376
		void DoWindowBackground(Rect rect);

		// Token: 0x06003441 RID: 13377
		bool DoCloseButton(Rect rect, string text);

		// Token: 0x06003442 RID: 13378
		bool DoClostButtonSmall(Rect rect);

		// Token: 0x06003443 RID: 13379
		void BeginGroup(Rect rect);

		// Token: 0x06003444 RID: 13380
		void EndGroup();

		// Token: 0x06003445 RID: 13381
		void DoGrayOut(Rect rect);
	}
}
