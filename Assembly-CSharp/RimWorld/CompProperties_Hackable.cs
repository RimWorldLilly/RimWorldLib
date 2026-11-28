using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B2 RID: 6322
	public class CompProperties_Hackable : CompProperties
	{
		// Token: 0x06009E5B RID: 40539 RVA: 0x0038D468 File Offset: 0x0038B668
		public CompProperties_Hackable()
		{
			this.compClass = typeof(CompHackable);
		}

		// Token: 0x04005DAE RID: 23982
		public float defence;

		// Token: 0x04005DAF RID: 23983
		public EffecterDef effectHacking;

		// Token: 0x04005DB0 RID: 23984
		public QuestScriptDef completedQuest;

		// Token: 0x04005DB1 RID: 23985
		public bool glowIfHacked;

		// Token: 0x04005DB2 RID: 23986
		public SoundDef hackingCompletedSound;

		// Token: 0x04005DB3 RID: 23987
		public IntRange lockoutDurationHoursRange = IntRange.Zero;

		// Token: 0x04005DB4 RID: 23988
		public bool showProgressAfterHackCompletion = true;

		// Token: 0x04005DB5 RID: 23989
		public bool lockoutPermanently;

		// Token: 0x04005DB6 RID: 23990
		public bool onlyRemotelyHackable;

		// Token: 0x04005DB7 RID: 23991
		public int intellectualSkillPrerequisite;

		// Token: 0x04005DB8 RID: 23992
		public List<ThingDefCountClass> dropOnHacked = new List<ThingDefCountClass>();

		// Token: 0x04005DB9 RID: 23993
		public List<ThingDefCountClass> dropOnDestroyed = new List<ThingDefCountClass>();

		// Token: 0x04005DBA RID: 23994
		[MustTranslate]
		public string notHackedInspectString;

		// Token: 0x04005DBB RID: 23995
		[MustTranslate]
		public string hackedInspectString;

		// Token: 0x04005DBC RID: 23996
		[MustTranslate]
		public string hackedMessage;

		// Token: 0x04005DBD RID: 23997
		[MustTranslate]
		public string autohackWarningString;

		// Token: 0x04005DBE RID: 23998
		[MustTranslate]
		public string hackedLetterLabel;

		// Token: 0x04005DBF RID: 23999
		[MustTranslate]
		public string hackedLetterText;

		// Token: 0x04005DC0 RID: 24000
		public LetterDef hackedLetterDef;

		// Token: 0x04005DC1 RID: 24001
		[MustTranslate]
		public string destroyedLetterLabel;

		// Token: 0x04005DC2 RID: 24002
		[MustTranslate]
		public string destroyedLetterText;

		// Token: 0x04005DC3 RID: 24003
		public LetterDef destroyedLetterDef;

		// Token: 0x04005DC4 RID: 24004
		public GraphicData unhackedGraphicData;

		// Token: 0x04005DC5 RID: 24005
		public GraphicData hackedGraphicData;

		// Token: 0x04005DC6 RID: 24006
		public Vector3 unhackedGraphicOffset;

		// Token: 0x04005DC7 RID: 24007
		public Vector3 hackedGraphicOffset;

		// Token: 0x04005DC8 RID: 24008
		public DrawerType graphicDrawerType = DrawerType.MapMeshOnly;
	}
}
