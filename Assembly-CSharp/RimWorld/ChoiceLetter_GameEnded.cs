using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D56 RID: 7510
	public class ChoiceLetter_GameEnded : ChoiceLetter
	{
		// Token: 0x170023E6 RID: 9190
		// (get) Token: 0x0600C25F RID: 49759 RVA: 0x0046D2CB File Offset: 0x0046B4CB
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023E7 RID: 9191
		// (get) Token: 0x0600C260 RID: 49760 RVA: 0x0046D2CE File Offset: 0x0046B4CE
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				ChoiceLetter_GameEnded.<get_Choices>d__3 <get_Choices>d__ = new ChoiceLetter_GameEnded.<get_Choices>d__3(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x0600C262 RID: 49762 RVA: 0x0046D2E8 File Offset: 0x0046B4E8
		[CompilerGenerated]
		internal static AcceptanceReport <get_Choices>g__CanCreateNewWanderers|3_0(ref ChoiceLetter_GameEnded.<>c__DisplayClass3_0 A_0)
		{
			if (A_0.hoursRemaining > 0f)
			{
				return "GameOverCreateNewWanderersWait".Translate(Math.Ceiling((double)A_0.hoursRemaining));
			}
			bool flag = false;
			if (Current.Game == null)
			{
				return false;
			}
			using (IEnumerator<Map> enumerator = Current.Game.PlayerHomeMaps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Tile.Layer.IsRootSurface)
					{
						return true;
					}
					flag = true;
				}
			}
			if (flag)
			{
				return "NoWandererDestination".Translate();
			}
			return "NoColony".Translate();
		}
	}
}
