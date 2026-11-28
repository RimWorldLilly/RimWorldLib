using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019D5 RID: 6613
	public class CompUsesMeditationFocus : ThingComp
	{
		// Token: 0x0600A673 RID: 42611 RVA: 0x003B5A17 File Offset: 0x003B3C17
		public override void PostDrawExtraSelectionOverlays()
		{
			MeditationUtility.DrawMeditationSpotOverlay(this.parent.Position, this.parent.Map);
		}
	}
}
