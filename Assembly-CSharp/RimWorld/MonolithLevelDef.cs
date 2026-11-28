using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FAF RID: 4015
	public class MonolithLevelDef : Def
	{
		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x0600660D RID: 26125 RVA: 0x00244BBE File Offset: 0x00242DBE
		public Texture2D UIIcon
		{
			get
			{
				return this.uiIcon;
			}
		}

		// Token: 0x0600660E RID: 26126 RVA: 0x00244BC6 File Offset: 0x00242DC6
		public override void PostLoad()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				}
			});
		}

		// Token: 0x0600660F RID: 26127 RVA: 0x00244BD9 File Offset: 0x00242DD9
		public override IEnumerable<string> ConfigErrors()
		{
			MonolithLevelDef.<ConfigErrors>d__41 <ConfigErrors>d__ = new MonolithLevelDef.<ConfigErrors>d__41(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003DB6 RID: 15798
		public int level;

		// Token: 0x04003DB7 RID: 15799
		public int graphicIndex;

		// Token: 0x04003DB8 RID: 15800
		public bool monolithGlows;

		// Token: 0x04003DB9 RID: 15801
		public int anomalyThreatTier;

		// Token: 0x04003DBA RID: 15802
		public bool useInactiveAnomalyThreatFraction;

		// Token: 0x04003DBB RID: 15803
		public bool useActiveAnomalyThreatFraction;

		// Token: 0x04003DBC RID: 15804
		public float anomalyThreatFractionFactor = 1f;

		// Token: 0x04003DBD RID: 15805
		public float anomalyThreatFraction;

		// Token: 0x04003DBE RID: 15806
		public List<IncidentDef> incidentsOnReached;

		// Token: 0x04003DBF RID: 15807
		public bool triggersGrayPall;

		// Token: 0x04003DC0 RID: 15808
		public List<GameConditionDef> unreachableDuringConditions;

		// Token: 0x04003DC1 RID: 15809
		public bool advanceThroughActivation;

		// Token: 0x04003DC2 RID: 15810
		public int desiredHarbingerTreeCount;

		// Token: 0x04003DC3 RID: 15811
		public bool postEndgame;

		// Token: 0x04003DC4 RID: 15812
		public int monolithGlowRadiusOverride = -1;

		// Token: 0x04003DC5 RID: 15813
		public KnowledgeCategoryDef monolithStudyCategory;

		// Token: 0x04003DC6 RID: 15814
		public EntityCategoryDef entityCatagoryCompletionRequired;

		// Token: 0x04003DC7 RID: 15815
		public int entityCountCompletionRequired;

		// Token: 0x04003DC8 RID: 15816
		public float anomalyMentalBreakChance;

		// Token: 0x04003DC9 RID: 15817
		public SoundDef activateSound;

		// Token: 0x04003DCA RID: 15818
		public SoundDef activatedSound;

		// Token: 0x04003DCB RID: 15819
		public List<MonolithAttachment> attachments;

		// Token: 0x04003DCC RID: 15820
		public IntVec2? sizeIncludingAttachments;

		// Token: 0x04003DCD RID: 15821
		public string uiIconPath;

		// Token: 0x04003DCE RID: 15822
		[MustTranslate]
		public string monolithLabel;

		// Token: 0x04003DCF RID: 15823
		[MustTranslate]
		public string monolithDescription;

		// Token: 0x04003DD0 RID: 15824
		[MustTranslate]
		public string levelInspectText;

		// Token: 0x04003DD1 RID: 15825
		[MustTranslate]
		public string extraQuestDescription;

		// Token: 0x04003DD2 RID: 15826
		[MustTranslate]
		public string activateGizmoText;

		// Token: 0x04003DD3 RID: 15827
		[MustTranslate]
		public string activateFloatMenuText;

		// Token: 0x04003DD4 RID: 15828
		[MustTranslate]
		public string activateGizmoDescription;

		// Token: 0x04003DD5 RID: 15829
		[MustTranslate]
		public string pawnSentToActivateMessage;

		// Token: 0x04003DD6 RID: 15830
		[MustTranslate]
		public string monolithCanBeActivatedText;

		// Token: 0x04003DD7 RID: 15831
		[MustTranslate]
		public string activateQuestText;

		// Token: 0x04003DD8 RID: 15832
		[MustTranslate]
		public string activatableLetterLabel;

		// Token: 0x04003DD9 RID: 15833
		[MustTranslate]
		public string activatableLetterText;

		// Token: 0x04003DDA RID: 15834
		[MustTranslate]
		public string activatedLetterText;

		// Token: 0x04003DDB RID: 15835
		private Texture2D uiIcon;
	}
}
