using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001764 RID: 5988
	public class UniqueIDsManager : IExposable
	{
		// Token: 0x06009505 RID: 38149 RVA: 0x00364617 File Offset: 0x00362817
		public int GetNextThingID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextThingID);
		}

		// Token: 0x06009506 RID: 38150 RVA: 0x00364624 File Offset: 0x00362824
		public int GetNextBillID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBillID);
		}

		// Token: 0x06009507 RID: 38151 RVA: 0x00364631 File Offset: 0x00362831
		public int GetNextFactionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextFactionID);
		}

		// Token: 0x06009508 RID: 38152 RVA: 0x0036463E File Offset: 0x0036283E
		public int GetNextLordID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLordID);
		}

		// Token: 0x06009509 RID: 38153 RVA: 0x0036464B File Offset: 0x0036284B
		public int GetNextTaleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTaleID);
		}

		// Token: 0x0600950A RID: 38154 RVA: 0x00364658 File Offset: 0x00362858
		public int GetNextPassingShipID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPassingShipID);
		}

		// Token: 0x0600950B RID: 38155 RVA: 0x00364665 File Offset: 0x00362865
		public int GetNextWorldObjectID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldObjectID);
		}

		// Token: 0x0600950C RID: 38156 RVA: 0x00364672 File Offset: 0x00362872
		public int GetNextMapID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMapID);
		}

		// Token: 0x0600950D RID: 38157 RVA: 0x0036467F File Offset: 0x0036287F
		public int GetNextCaravanID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextCaravanID);
		}

		// Token: 0x0600950E RID: 38158 RVA: 0x0036468C File Offset: 0x0036288C
		public int GetNextAreaID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAreaID);
		}

		// Token: 0x0600950F RID: 38159 RVA: 0x00364699 File Offset: 0x00362899
		public int GetNextAnimalPenID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAnimalPenID);
		}

		// Token: 0x06009510 RID: 38160 RVA: 0x003646A6 File Offset: 0x003628A6
		public int GetNextTransporterGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTransporterGroupID);
		}

		// Token: 0x06009511 RID: 38161 RVA: 0x003646B3 File Offset: 0x003628B3
		public int GetNextAncientCryptosleepCasketGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAncientCryptosleepCasketGroupID);
		}

		// Token: 0x06009512 RID: 38162 RVA: 0x003646C0 File Offset: 0x003628C0
		public int GetNextJobID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextJobID);
		}

		// Token: 0x06009513 RID: 38163 RVA: 0x003646CD File Offset: 0x003628CD
		public int GetNextSignalTagID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextSignalTagID);
		}

		// Token: 0x06009514 RID: 38164 RVA: 0x003646DA File Offset: 0x003628DA
		public int GetNextWorldFeatureID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldFeatureID);
		}

		// Token: 0x06009515 RID: 38165 RVA: 0x003646E7 File Offset: 0x003628E7
		public int GetNextHediffID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextHediffID);
		}

		// Token: 0x06009516 RID: 38166 RVA: 0x003646F4 File Offset: 0x003628F4
		public int GetNextBattleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBattleID);
		}

		// Token: 0x06009517 RID: 38167 RVA: 0x00364701 File Offset: 0x00362901
		public int GetNextLogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLogID);
		}

		// Token: 0x06009518 RID: 38168 RVA: 0x0036470E File Offset: 0x0036290E
		public int GetNextLetterID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLetterID);
		}

		// Token: 0x06009519 RID: 38169 RVA: 0x0036471B File Offset: 0x0036291B
		public int GetNextArchivedDialogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextArchivedDialogID);
		}

		// Token: 0x0600951A RID: 38170 RVA: 0x00364728 File Offset: 0x00362928
		public int GetNextMessageID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMessageID);
		}

		// Token: 0x0600951B RID: 38171 RVA: 0x00364735 File Offset: 0x00362935
		public int GetNextZoneID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextZoneID);
		}

		// Token: 0x0600951C RID: 38172 RVA: 0x00364742 File Offset: 0x00362942
		public int GetNextPlanID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPlanID);
		}

		// Token: 0x0600951D RID: 38173 RVA: 0x0036474F File Offset: 0x0036294F
		public int GetNextQuestID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextQuestID);
		}

		// Token: 0x0600951E RID: 38174 RVA: 0x0036475C File Offset: 0x0036295C
		public int GetNextGameConditionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextGameConditionID);
		}

		// Token: 0x0600951F RID: 38175 RVA: 0x00364769 File Offset: 0x00362969
		public int GetNextIdeoID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextIdeoID);
		}

		// Token: 0x06009520 RID: 38176 RVA: 0x00364776 File Offset: 0x00362976
		public int GetNextPreceptID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPreceptID);
		}

		// Token: 0x06009521 RID: 38177 RVA: 0x00364783 File Offset: 0x00362983
		public int GetNextRitualObligationID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextRitualObligationID);
		}

		// Token: 0x06009522 RID: 38178 RVA: 0x00364790 File Offset: 0x00362990
		public int GetNextPresenceDemandID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPresenceDemandID);
		}

		// Token: 0x06009523 RID: 38179 RVA: 0x0036479D File Offset: 0x0036299D
		public int GetNextTransportShipID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTransportShipID);
		}

		// Token: 0x06009524 RID: 38180 RVA: 0x003647AA File Offset: 0x003629AA
		public int GetNextShipJobID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextShipJobID);
		}

		// Token: 0x06009525 RID: 38181 RVA: 0x003647B7 File Offset: 0x003629B7
		public int GetNextRitualRoleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextRitualRoleID);
		}

		// Token: 0x06009526 RID: 38182 RVA: 0x003647C4 File Offset: 0x003629C4
		public int GetNextAbilityID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAbilityID);
		}

		// Token: 0x06009527 RID: 38183 RVA: 0x003647D1 File Offset: 0x003629D1
		public int GetNextGeneID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextGeneID);
		}

		// Token: 0x06009528 RID: 38184 RVA: 0x003647DE File Offset: 0x003629DE
		public int GetNextCocoonGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextCocoonGroupID);
		}

		// Token: 0x06009529 RID: 38185 RVA: 0x003647EB File Offset: 0x003629EB
		public int GetNextStorageGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextStorageGroupID);
		}

		// Token: 0x0600952A RID: 38186 RVA: 0x003647F8 File Offset: 0x003629F8
		public int GetNextPitGateIncidentID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPitGateIncidentID);
		}

		// Token: 0x0600952B RID: 38187 RVA: 0x00364805 File Offset: 0x00362A05
		public int GetNextPsychicRitualID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPsychicRitualID);
		}

		// Token: 0x0600952C RID: 38188 RVA: 0x00364812 File Offset: 0x00362A12
		public int GetNextPsychicRitualToilID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPsychicRitualToilID);
		}

		// Token: 0x0600952D RID: 38189 RVA: 0x0036481F File Offset: 0x00362A1F
		public int GetNextStructureSketchID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextStructureSketchID);
		}

		// Token: 0x0600952E RID: 38190 RVA: 0x0036482C File Offset: 0x00362A2C
		public int GetNextRoomID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextRoomID);
		}

		// Token: 0x0600952F RID: 38191 RVA: 0x00364839 File Offset: 0x00362A39
		public UniqueIDsManager()
		{
			this.nextThingID = Rand.Range(0, 1000);
		}

		// Token: 0x06009530 RID: 38192 RVA: 0x00364854 File Offset: 0x00362A54
		private static int GetNextID(ref int nextID)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars && !Find.UniqueIDsManager.wasLoaded)
			{
				Log.Warning("Getting next unique ID during LoadingVars before UniqueIDsManager was loaded. Assigning a random value.");
				return Rand.Int;
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Log.Warning("Getting next unique ID during saving This may cause bugs.");
			}
			int result = nextID;
			nextID++;
			if (nextID == 2147483647)
			{
				Log.Warning("Next ID is at max value. Resetting to 0. This may cause bugs.");
				nextID = 0;
			}
			return result;
		}

		// Token: 0x06009531 RID: 38193 RVA: 0x003648B8 File Offset: 0x00362AB8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextThingID, "nextThingID", 0, false);
			Scribe_Values.Look<int>(ref this.nextBillID, "nextBillID", 0, false);
			Scribe_Values.Look<int>(ref this.nextFactionID, "nextFactionID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLordID, "nextLordID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTaleID, "nextTaleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPassingShipID, "nextPassingShipID", 0, false);
			Scribe_Values.Look<int>(ref this.nextWorldObjectID, "nextWorldObjectID", 0, false);
			Scribe_Values.Look<int>(ref this.nextMapID, "nextMapID", 0, false);
			Scribe_Values.Look<int>(ref this.nextCaravanID, "nextCaravanID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAreaID, "nextAreaID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAnimalPenID, "nextAnimalPenID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTransporterGroupID, "nextTransporterGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAncientCryptosleepCasketGroupID, "nextAncientCryptosleepCasketGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextJobID, "nextJobID", 0, false);
			Scribe_Values.Look<int>(ref this.nextSignalTagID, "nextSignalTagID", 0, false);
			Scribe_Values.Look<int>(ref this.nextWorldFeatureID, "nextWorldFeatureID", 0, false);
			Scribe_Values.Look<int>(ref this.nextHediffID, "nextHediffID", 0, false);
			Scribe_Values.Look<int>(ref this.nextBattleID, "nextBattleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLogID, "nextLogID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLetterID, "nextLetterID", 0, false);
			Scribe_Values.Look<int>(ref this.nextArchivedDialogID, "nextArchivedDialogID", 0, false);
			Scribe_Values.Look<int>(ref this.nextMessageID, "nextMessageID", 0, false);
			Scribe_Values.Look<int>(ref this.nextZoneID, "nextZoneID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPlanID, "nextPlanID", 0, false);
			Scribe_Values.Look<int>(ref this.nextQuestID, "nextQuestID", 0, false);
			Scribe_Values.Look<int>(ref this.nextGameConditionID, "nextGameConditionID", 0, false);
			Scribe_Values.Look<int>(ref this.nextIdeoID, "nextIdeoID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPreceptID, "nextPreceptID", 0, false);
			Scribe_Values.Look<int>(ref this.nextRitualObligationID, "nextRitualObligationID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPresenceDemandID, "nextPresenceDemandID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTransportShipID, "nextTransportShipID", 0, false);
			Scribe_Values.Look<int>(ref this.nextShipJobID, "nextShipJobID", 0, false);
			Scribe_Values.Look<int>(ref this.nextRitualRoleID, "nextRitualRoleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAbilityID, "nextAbilityID", 0, false);
			Scribe_Values.Look<int>(ref this.nextGeneID, "nextGeneID", 0, false);
			Scribe_Values.Look<int>(ref this.nextCocoonGroupID, "nextCocoonGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextStorageGroupID, "nextStorageGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPitGateIncidentID, "nextPitGateIncidentID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPsychicRitualToilID, "nextPsychicRitualToilID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPsychicRitualID, "nextPsychicRitualID", 0, false);
			Scribe_Values.Look<int>(ref this.nextStructureSketchID, "nextStructureSketchID", 0, false);
			Scribe_Values.Look<int>(ref this.nextRoomID, "nextRoomID", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.wasLoaded = true;
			}
		}

		// Token: 0x040059D0 RID: 22992
		private int nextThingID;

		// Token: 0x040059D1 RID: 22993
		private int nextBillID;

		// Token: 0x040059D2 RID: 22994
		private int nextFactionID;

		// Token: 0x040059D3 RID: 22995
		private int nextLordID;

		// Token: 0x040059D4 RID: 22996
		private int nextTaleID;

		// Token: 0x040059D5 RID: 22997
		private int nextPassingShipID;

		// Token: 0x040059D6 RID: 22998
		private int nextWorldObjectID;

		// Token: 0x040059D7 RID: 22999
		private int nextMapID;

		// Token: 0x040059D8 RID: 23000
		private int nextCaravanID;

		// Token: 0x040059D9 RID: 23001
		private int nextAreaID;

		// Token: 0x040059DA RID: 23002
		private int nextAnimalPenID;

		// Token: 0x040059DB RID: 23003
		private int nextTransporterGroupID;

		// Token: 0x040059DC RID: 23004
		private int nextAncientCryptosleepCasketGroupID;

		// Token: 0x040059DD RID: 23005
		private int nextJobID;

		// Token: 0x040059DE RID: 23006
		private int nextSignalTagID;

		// Token: 0x040059DF RID: 23007
		private int nextWorldFeatureID;

		// Token: 0x040059E0 RID: 23008
		private int nextHediffID;

		// Token: 0x040059E1 RID: 23009
		private int nextBattleID;

		// Token: 0x040059E2 RID: 23010
		private int nextLogID;

		// Token: 0x040059E3 RID: 23011
		private int nextLetterID;

		// Token: 0x040059E4 RID: 23012
		private int nextArchivedDialogID;

		// Token: 0x040059E5 RID: 23013
		private int nextMessageID;

		// Token: 0x040059E6 RID: 23014
		private int nextZoneID;

		// Token: 0x040059E7 RID: 23015
		private int nextPlanID;

		// Token: 0x040059E8 RID: 23016
		private int nextQuestID;

		// Token: 0x040059E9 RID: 23017
		private int nextGameConditionID;

		// Token: 0x040059EA RID: 23018
		private int nextIdeoID;

		// Token: 0x040059EB RID: 23019
		private int nextPreceptID;

		// Token: 0x040059EC RID: 23020
		private int nextRitualObligationID;

		// Token: 0x040059ED RID: 23021
		private int nextPresenceDemandID;

		// Token: 0x040059EE RID: 23022
		private int nextTransportShipID;

		// Token: 0x040059EF RID: 23023
		private int nextShipJobID;

		// Token: 0x040059F0 RID: 23024
		private int nextRitualRoleID;

		// Token: 0x040059F1 RID: 23025
		private int nextAbilityID;

		// Token: 0x040059F2 RID: 23026
		private int nextGeneID;

		// Token: 0x040059F3 RID: 23027
		private int nextCocoonGroupID;

		// Token: 0x040059F4 RID: 23028
		private int nextStorageGroupID;

		// Token: 0x040059F5 RID: 23029
		private int nextPitGateIncidentID;

		// Token: 0x040059F6 RID: 23030
		private int nextPsychicRitualToilID;

		// Token: 0x040059F7 RID: 23031
		private int nextPsychicRitualID;

		// Token: 0x040059F8 RID: 23032
		private int nextStructureSketchID;

		// Token: 0x040059F9 RID: 23033
		private int nextRoomID;

		// Token: 0x040059FA RID: 23034
		private bool wasLoaded;
	}
}
