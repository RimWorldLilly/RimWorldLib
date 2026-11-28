using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022BD RID: 8893
	public class QuestNode_CreateIncidents : QuestNode
	{
		// Token: 0x0600DB74 RID: 56180 RVA: 0x0051C03C File Offset: 0x0051A23C
		protected override bool TestRunInt(Slate slate)
		{
			return this.incidentDef.GetValue(slate) != null && this.points.GetValue(slate) >= this.incidentDef.GetValue(slate).minThreatPoints && this.points.GetValue(slate) <= this.incidentDef.GetValue(slate).maxThreatPoints;
		}

		// Token: 0x0600DB75 RID: 56181 RVA: 0x0051C098 File Offset: 0x0051A298
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int value = this.duration.GetValue(slate);
			Quest quest = QuestGen.quest;
			int value2 = this.startOffsetTicks.GetValue(slate);
			IncidentDef value3 = this.incidentDef.GetValue(slate);
			Map map = slate.Get<Map>("map", null, false);
			float value4 = this.points.GetValue(slate);
			Faction value5 = this.faction.GetValue(slate);
			string delayInSignal = slate.Get<string>("inSignal", null, false);
			string disableSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			int? value6 = this.randomIncidents.GetValue(slate);
			if (value6 != null)
			{
				int num = 0;
				for (;;)
				{
					int num2 = num;
					int? num3 = value6;
					if (!(num2 < num3.GetValueOrDefault() & num3 != null))
					{
						break;
					}
					this.CreateDelayedIncident(Rand.Range(value2, value), delayInSignal, disableSignal, value3, map, value4, value5);
					num++;
				}
			}
			int? value7 = this.intervalTicks.GetValue(slate);
			if (value7 != null)
			{
				int num4 = Mathf.FloorToInt((float)value / (float)value7.Value);
				for (int i = 0; i < num4; i++)
				{
					int delayTicks = Mathf.Max(i * value7.Value, value2);
					this.CreateDelayedIncident(delayTicks, delayInSignal, disableSignal, value3, map, value4, value5);
				}
			}
		}

		// Token: 0x0600DB76 RID: 56182 RVA: 0x0051C1D8 File Offset: 0x0051A3D8
		private void CreateDelayedIncident(int delayTicks, string delayInSignal, string disableSignal, IncidentDef incident, Map map, float points, Faction faction)
		{
			Quest quest = QuestGen.quest;
			QuestPart_Delay questPart_Delay = new QuestPart_Delay();
			questPart_Delay.delayTicks = delayTicks;
			questPart_Delay.inSignalEnable = delayInSignal;
			questPart_Delay.inSignalDisable = disableSignal;
			questPart_Delay.debugLabel = questPart_Delay.delayTicks.ToStringTicksToDays("F1") + "_" + this.incidentDef.ToString();
			quest.AddPart(questPart_Delay);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.incident = incident;
			questPart_Incident.inSignal = questPart_Delay.OutSignalCompleted;
			questPart_Incident.SetIncidentParmsAndRemoveTarget(new IncidentParms
			{
				forced = true,
				target = map,
				points = points,
				faction = faction
			});
			quest.AddPart(questPart_Incident);
		}

		// Token: 0x04008C77 RID: 35959
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008C78 RID: 35960
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x04008C79 RID: 35961
		public SlateRef<IncidentDef> incidentDef;

		// Token: 0x04008C7A RID: 35962
		public SlateRef<int?> intervalTicks;

		// Token: 0x04008C7B RID: 35963
		public SlateRef<int?> randomIncidents;

		// Token: 0x04008C7C RID: 35964
		public SlateRef<int> startOffsetTicks;

		// Token: 0x04008C7D RID: 35965
		public SlateRef<int> duration;

		// Token: 0x04008C7E RID: 35966
		public SlateRef<float> points;

		// Token: 0x04008C7F RID: 35967
		public SlateRef<Faction> faction;
	}
}
