using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0C RID: 3852
	public class MapPortalProperties
	{
		// Token: 0x040038CE RID: 14542
		public MapGeneratorDef pocketMapGenerator;

		// Token: 0x040038CF RID: 14543
		public ThingDef exitDef;

		// Token: 0x040038D0 RID: 14544
		public int pocketMapSize = 100;

		// Token: 0x040038D1 RID: 14545
		[MustTranslate]
		public string enteredLetterLabel;

		// Token: 0x040038D2 RID: 14546
		[MustTranslate]
		public string enteredLetterText;

		// Token: 0x040038D3 RID: 14547
		[MustTranslate]
		public LetterDef enteredLetterDef;

		// Token: 0x040038D4 RID: 14548
		public SoundDef traverseSound;
	}
}
