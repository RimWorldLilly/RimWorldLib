using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DB RID: 6363
	public class CompProperties_LeavingsLetter : CompProperties
	{
		// Token: 0x06009F99 RID: 40857 RVA: 0x003939B8 File Offset: 0x00391BB8
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			ThingFilter thingFilter = this.leavingsFilter;
			if (thingFilter == null)
			{
				return;
			}
			thingFilter.ResolveReferences();
		}

		// Token: 0x06009F9A RID: 40858 RVA: 0x003939D1 File Offset: 0x00391BD1
		public CompProperties_LeavingsLetter()
		{
			this.compClass = typeof(CompLeavingsLetter);
		}

		// Token: 0x04005E59 RID: 24153
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04005E5A RID: 24154
		[MustTranslate]
		public string letterText;

		// Token: 0x04005E5B RID: 24155
		public LetterDef letterDef;

		// Token: 0x04005E5C RID: 24156
		public ThingFilter leavingsFilter;
	}
}
