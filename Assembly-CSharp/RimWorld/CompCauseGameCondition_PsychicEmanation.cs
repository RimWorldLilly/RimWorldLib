using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020017FC RID: 6140
	public class CompCauseGameCondition_PsychicEmanation : CompCauseGameCondition
	{
		// Token: 0x17001A0D RID: 6669
		// (get) Token: 0x06009938 RID: 39224 RVA: 0x003771D9 File Offset: 0x003753D9
		public new CompProperties_CausesGameCondition_PsychicEmanation Props
		{
			get
			{
				return (CompProperties_CausesGameCondition_PsychicEmanation)this.props;
			}
		}

		// Token: 0x17001A0E RID: 6670
		// (get) Token: 0x06009939 RID: 39225 RVA: 0x003771E6 File Offset: 0x003753E6
		public PsychicDroneLevel Level
		{
			get
			{
				return this.droneLevel;
			}
		}

		// Token: 0x17001A0F RID: 6671
		// (get) Token: 0x0600993A RID: 39226 RVA: 0x003771EE File Offset: 0x003753EE
		private bool DroneLevelIncreases
		{
			get
			{
				return this.Props.droneLevelIncreaseInterval != int.MinValue;
			}
		}

		// Token: 0x0600993B RID: 39227 RVA: 0x00377205 File Offset: 0x00375405
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
			this.droneLevel = this.Props.droneLevel;
		}

		// Token: 0x0600993C RID: 39228 RVA: 0x00377230 File Offset: 0x00375430
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && this.DroneLevelIncreases && !this.parent.BeingTransportedOnGravship)
			{
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			}
		}

		// Token: 0x0600993D RID: 39229 RVA: 0x00377284 File Offset: 0x00375484
		public override void CompTick()
		{
			base.CompTick();
			if (!this.parent.Spawned || !this.DroneLevelIncreases || !this.Active)
			{
				return;
			}
			this.ticksToIncreaseDroneLevel--;
			if (this.ticksToIncreaseDroneLevel <= 0)
			{
				this.IncreaseDroneLevel();
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
			}
		}

		// Token: 0x0600993E RID: 39230 RVA: 0x003772E4 File Offset: 0x003754E4
		private void IncreaseDroneLevel()
		{
			if (this.droneLevel == PsychicDroneLevel.BadExtreme)
			{
				return;
			}
			this.droneLevel += 1;
			TaggedString text = "LetterPsychicDroneLevelIncreased".Translate(this.gender.GetLabel(false));
			Find.LetterStack.ReceiveLetter("LetterLabelPsychicDroneLevelIncreased".Translate(), text, LetterDefOf.NegativeEvent, null, 0, true);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			base.ReSetupAllConditions();
		}

		// Token: 0x0600993F RID: 39231 RVA: 0x0037735E File Offset: 0x0037555E
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)condition;
			gameCondition_PsychicEmanation.gender = this.gender;
			gameCondition_PsychicEmanation.level = this.Level;
		}

		// Token: 0x06009940 RID: 39232 RVA: 0x00377385 File Offset: 0x00375585
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.ticksToIncreaseDroneLevel, "ticksToIncreaseDroneLevel", 0, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.droneLevel, "droneLevel", PsychicDroneLevel.None, false);
		}

		// Token: 0x06009941 RID: 39233 RVA: 0x003773C3 File Offset: 0x003755C3
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompCauseGameCondition_PsychicEmanation.<CompGetGizmosExtra>d__15 <CompGetGizmosExtra>d__ = new CompCauseGameCondition_PsychicEmanation.<CompGetGizmosExtra>d__15(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009942 RID: 39234 RVA: 0x003773D4 File Offset: 0x003755D4
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst() + "\n" + "PsychicDroneLevel".Translate(this.droneLevel.GetLabelCap()));
		}

		// Token: 0x06009943 RID: 39235 RVA: 0x0037745C File Offset: 0x0037565C
		public override void RandomizeSettings(Site site)
		{
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
			if (site.ActualThreatPoints < 800f)
			{
				this.droneLevel = PsychicDroneLevel.BadLow;
				return;
			}
			if (site.ActualThreatPoints < 2000f)
			{
				this.droneLevel = PsychicDroneLevel.BadMedium;
				return;
			}
			this.droneLevel = PsychicDroneLevel.BadHigh;
		}

		// Token: 0x04005B81 RID: 23425
		public Gender gender;

		// Token: 0x04005B82 RID: 23426
		private int ticksToIncreaseDroneLevel;

		// Token: 0x04005B83 RID: 23427
		private PsychicDroneLevel droneLevel = PsychicDroneLevel.BadHigh;
	}
}
