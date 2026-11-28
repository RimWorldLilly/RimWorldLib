using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C73 RID: 7283
	[StaticConstructorOnStartup]
	public class Command_Hide_ZoneStockpile : Command_Hide
	{
		// Token: 0x0600B9EE RID: 47598 RVA: 0x00419413 File Offset: 0x00417613
		public Command_Hide_ZoneStockpile(IHideable hideable) : base(hideable)
		{
		}

		// Token: 0x0600B9EF RID: 47599 RVA: 0x0041941C File Offset: 0x0041761C
		protected override IEnumerable<FloatMenuOption> GetOptions()
		{
			return Command_Hide_ZoneStockpile.GetHideOptions();
		}

		// Token: 0x0600B9F0 RID: 47600 RVA: 0x00419423 File Offset: 0x00417623
		public static IEnumerable<FloatMenuOption> GetHideOptions()
		{
			return new Command_Hide_ZoneStockpile.<GetHideOptions>d__3(-2);
		}

		// Token: 0x040069DE RID: 27102
		private static readonly Texture2D IconTex = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true);
	}
}
