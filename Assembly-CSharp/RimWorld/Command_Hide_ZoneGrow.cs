using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C72 RID: 7282
	[StaticConstructorOnStartup]
	public class Command_Hide_ZoneGrow : Command_Hide
	{
		// Token: 0x0600B9EA RID: 47594 RVA: 0x004193E8 File Offset: 0x004175E8
		public Command_Hide_ZoneGrow(IHideable hideable) : base(hideable)
		{
		}

		// Token: 0x0600B9EB RID: 47595 RVA: 0x004193F1 File Offset: 0x004175F1
		protected override IEnumerable<FloatMenuOption> GetOptions()
		{
			return Command_Hide_ZoneGrow.GetHideOptions();
		}

		// Token: 0x0600B9EC RID: 47596 RVA: 0x004193F8 File Offset: 0x004175F8
		public static IEnumerable<FloatMenuOption> GetHideOptions()
		{
			return new Command_Hide_ZoneGrow.<GetHideOptions>d__3(-2);
		}

		// Token: 0x040069DD RID: 27101
		private static readonly Texture2D IconTex = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true);
	}
}
