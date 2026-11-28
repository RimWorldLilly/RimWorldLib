using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EEC RID: 3820
	public class CompProperties_Flickable : CompProperties
	{
		// Token: 0x060063CA RID: 25546 RVA: 0x0023C621 File Offset: 0x0023A821
		public CompProperties_Flickable()
		{
			this.compClass = typeof(CompFlickable);
		}

		// Token: 0x04003844 RID: 14404
		[NoTranslate]
		public string commandTexture = "UI/Commands/DesirePower";

		// Token: 0x04003845 RID: 14405
		[NoTranslate]
		public string commandLabelKey = "CommandDesignateTogglePowerLabel";

		// Token: 0x04003846 RID: 14406
		[NoTranslate]
		public string commandDescKey = "CommandDesignateTogglePowerDesc";
	}
}
