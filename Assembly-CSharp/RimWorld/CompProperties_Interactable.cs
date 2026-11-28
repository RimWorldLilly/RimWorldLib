using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CD RID: 6349
	public class CompProperties_Interactable : CompProperties
	{
		// Token: 0x06009F12 RID: 40722 RVA: 0x00391170 File Offset: 0x0038F370
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Interactable.<ConfigErrors>d__30 <ConfigErrors>d__ = new CompProperties_Interactable.<ConfigErrors>d__30(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005E1C RID: 24092
		public int cooldownTicks;

		// Token: 0x04005E1D RID: 24093
		public int activeTicks;

		// Token: 0x04005E1E RID: 24094
		public int ticksToActivate;

		// Token: 0x04005E1F RID: 24095
		public int cooldownFleckSpawnIntervalTicks;

		// Token: 0x04005E20 RID: 24096
		public float cooldownFleckScale = 1f;

		// Token: 0x04005E21 RID: 24097
		public bool cooldownPreventsRefuel;

		// Token: 0x04005E22 RID: 24098
		public bool maintainProgress;

		// Token: 0x04005E23 RID: 24099
		public bool showMustBeActivatedByColonist = true;

		// Token: 0x04005E24 RID: 24100
		public bool forceNormalSpeedOnInteracting;

		// Token: 0x04005E25 RID: 24101
		public bool remainingSecondsInInspectString;

		// Token: 0x04005E26 RID: 24102
		public bool ignoreForbidden;

		// Token: 0x04005E27 RID: 24103
		public bool requiresPower;

		// Token: 0x04005E28 RID: 24104
		public TargetingParameters targetingParameters;

		// Token: 0x04005E29 RID: 24105
		public StatDef activateStat;

		// Token: 0x04005E2A RID: 24106
		public FleckDef fleckOnUsed;

		// Token: 0x04005E2B RID: 24107
		public float fleckOnUsedScale;

		// Token: 0x04005E2C RID: 24108
		public FleckDef cooldownFleck;

		// Token: 0x04005E2D RID: 24109
		public SoundDef soundActivate;

		// Token: 0x04005E2E RID: 24110
		public EffecterDef interactionEffecter;

		// Token: 0x04005E2F RID: 24111
		[MustTranslate]
		public string jobString;

		// Token: 0x04005E30 RID: 24112
		[MustTranslate]
		public string activatingStringPending;

		// Token: 0x04005E31 RID: 24113
		[MustTranslate]
		public string activatingString;

		// Token: 0x04005E32 RID: 24114
		[MustTranslate]
		public string activateLabelString;

		// Token: 0x04005E33 RID: 24115
		[MustTranslate]
		public string activateDescString;

		// Token: 0x04005E34 RID: 24116
		[MustTranslate]
		public string guiLabelString;

		// Token: 0x04005E35 RID: 24117
		[MustTranslate]
		public string onCooldownString;

		// Token: 0x04005E36 RID: 24118
		[MustTranslate]
		public string inspectString;

		// Token: 0x04005E37 RID: 24119
		[MustTranslate]
		public string messageCompletedString;

		// Token: 0x04005E38 RID: 24120
		[MustTranslate]
		public string messageCooldownEnded;

		// Token: 0x04005E39 RID: 24121
		[NoTranslate]
		public string activateTexPath;
	}
}
