using System;

namespace RimWorld
{
	// Token: 0x020017E9 RID: 6121
	public interface IActivity
	{
		// Token: 0x060098B2 RID: 39090
		void OnActivityActivated();

		// Token: 0x060098B3 RID: 39091
		void OnPassive();

		// Token: 0x060098B4 RID: 39092
		bool ShouldGoPassive();

		// Token: 0x060098B5 RID: 39093
		bool CanBeSuppressed();

		// Token: 0x060098B6 RID: 39094
		bool CanActivate();

		// Token: 0x060098B7 RID: 39095
		string ActivityTooltipExtra();
	}
}
