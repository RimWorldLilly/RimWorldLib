using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001796 RID: 6038
	public class ScenPart_Rule_DisallowDesignator : ScenPart_Rule
	{
		// Token: 0x06009674 RID: 38516 RVA: 0x00369F7C File Offset: 0x0036817C
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowDesignator(this.def.designatorType, false);
		}
	}
}
