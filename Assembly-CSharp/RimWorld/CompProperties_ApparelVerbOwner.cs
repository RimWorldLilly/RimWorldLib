using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001810 RID: 6160
	public class CompProperties_ApparelVerbOwner : CompProperties
	{
		// Token: 0x060099E2 RID: 39394 RVA: 0x00379F62 File Offset: 0x00378162
		public CompProperties_ApparelVerbOwner()
		{
			this.compClass = typeof(CompApparelVerbOwner);
		}

		// Token: 0x060099E3 RID: 39395 RVA: 0x00379F88 File Offset: 0x00378188
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_ApparelVerbOwner.<ConfigErrors>d__4 <ConfigErrors>d__ = new CompProperties_ApparelVerbOwner.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005BB7 RID: 23479
		public KeyBindingDef hotKey;

		// Token: 0x04005BB8 RID: 23480
		public bool displayGizmoWhileUndrafted = true;

		// Token: 0x04005BB9 RID: 23481
		public bool displayGizmoWhileDrafted = true;
	}
}
